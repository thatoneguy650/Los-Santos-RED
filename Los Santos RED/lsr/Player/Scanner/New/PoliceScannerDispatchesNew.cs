﻿using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DispatchScannerFiles;

public class PoliceScannerDispatchesNew
{
    private IPoliceRespondable Player;
    private IAudioPlayable AudioPlayer;
    private ISettingsProvideable Settings;
    private ITimeReportable Time;
    private IEntityProvideable World;
    private PoliceScannerNew PoliceScanner;
    public Dispatch AimingWeaponAtPolice { get; set; }
    public Dispatch AnnounceStolenVehicle { get; set; }
    public Dispatch ArmedRobbery { get; set; }
    public Dispatch AssaultingCivilians { get; set; }
    public Dispatch AssaultingCiviliansWithDeadlyWeapon { get; set; }
    public Dispatch AssaultingOfficer { get; set; }
    public Dispatch AttemptingSuicide { get; set; }
    public Dispatch AttemptToReacquireSuspect { get; set; }
    public Dispatch CarryingWeapon { get; set; }
    public Dispatch ChangedVehicles { get; set; }
    public Dispatch CivilianDown { get; set; }
    public Dispatch CivilianInjury { get; set; }
    public Dispatch CivilianShot { get; set; }
    public Dispatch CriminalActivity { get; set; }
    public Dispatch DealingDrugs { get; set; }
    public Dispatch DealingGuns { get; set; }
    public Dispatch DrivingAtStolenVehicle { get; set; }
    public Dispatch DrunkDriving { get; set; }
    public Dispatch ExcessiveSpeed { get; set; }
    public Dispatch FelonySpeeding { get; set; }
    public Dispatch FirefightingServicesRequired { get; set; }
    public Dispatch GotOffFreeway { get; set; }
    public Dispatch GotOnFreeway { get; set; }
    public Dispatch GrandTheftAuto { get; set; }
    public Dispatch Harassment { get; set; }
    public Dispatch Kidnapping { get; set; }
    public Dispatch LethalForceAuthorized { get; set; }
    public Dispatch MedicalServicesRequired { get; set; }
    public Dispatch Mugging { get; set; }
    public Dispatch NoFurtherUnitsNeeded { get; set; }
    public Dispatch OfficerDown { get; set; }
    public Dispatch OfficerMIA { get; set; }
    public Dispatch OfficerNeedsAssistance { get; set; }
    public Dispatch OfficersNeeded { get; set; }
    public Dispatch OnFoot { get; set; }
    public Dispatch PedHitAndRun { get; set; }
    public Dispatch PublicIntoxication { get; set; }
    public Dispatch PublicNuisance { get; set; }
    public Dispatch RecklessDriving { get; set; }
    public Dispatch RemainInArea { get; set; }
    public Dispatch RequestAirSupport { get; set; }
    public Dispatch RequestBackup { get; set; }
    public Dispatch RequestBackupSimple { get; set; }
    public Dispatch RequestFIBUnits { get; set; }
    public Dispatch RequestMilitaryUnits { get; set; }
    public Dispatch RequestNOOSEUnits { get; set; }
    public Dispatch RequestNooseUnitsAlt { get; set; }
    public Dispatch RequestNooseUnitsAlt2 { get; set; }
    public Dispatch RequestSwatAirSupport { get; set; }
    public Dispatch ShotsFiredStatus { get; set; }
    public Dispatch ResistingArrest { get; set; }
    public Dispatch ResumePatrol { get; set; }
    public Dispatch RunningARedLight { get; set; }
    public Dispatch ShotsFired { get; set; }
    public Dispatch ShotsFiredAtAnOfficer { get; set; }
    public Dispatch StealingAirVehicle { get; set; }
    public Dispatch SuspectArrested { get; set; }
    public Dispatch SuspectEvaded { get; set; }
    public Dispatch SuspectSpotted { get; set; }
    public Dispatch SuspectWasted { get; set; }
    public Dispatch SuspiciousActivity { get; set; }
    public Dispatch SuspiciousVehicle { get; set; }
    public Dispatch TamperingWithVehicle { get; set; }
    public Dispatch TerroristActivity { get; set; }
    public Dispatch ThreateningOfficerWithFirearm { get; set; }
    public Dispatch TrespassingOnGovernmentProperty { get; set; }
    public Dispatch VehicleCrashed { get; set; }
    public Dispatch VehicleHitAndRun { get; set; }
    public Dispatch VehicleStartedFire { get; set; }
    public Dispatch PublicVagrancy { get; set; }
    public Dispatch WantedSuspectSpotted { get; set; }
    public Dispatch WeaponsFree { get; set; }

    public CallsignScannerAudio CallsignScannerAudio { get; set; }
    public StreetScannerAudio StreetScannerAudio { get; set; }
    public VehicleScannerAudio VehicleScannerAudio { get; set; }
    public ZoneScannerAudio ZoneScannerAudio { get; set; }


    public List<AudioSet> RespondCode2Set { get; set; }
    public List<AudioSet> RespondCode3Set { get; set; }
    public List<AudioSet> UnitEnRouteSet { get; set; }
    public List<AudioSet> AttentionAllUnits { get; set; }

    public List<string> RadioEnd { get; set; }
    public List<string> RadioStart { get; set; }

    public List<AudioSet> OfficersReport { get; set; }
    public List<AudioSet> LicensePlateSet { get; set; }
    public List<AudioSet> LethalForce { get; set; }
    public List<AudioSet> CiviliansReport { get; set; }

    public List<Dispatch> DispatchList { get; set; } = new List<Dispatch>();
    public List<CrimeDispatch> DispatchLookup { get; set; }




    public PoliceScannerDispatchesNew(IEntityProvideable world, IPoliceRespondable currentPlayer, IAudioPlayable audioPlayer, ISettingsProvideable settings, ITimeReportable time, PoliceScannerNew policeScanner)
    {
        AudioPlayer = audioPlayer;
        Player = currentPlayer;
        World = world;
        Settings = settings;
        Time = time;
        PoliceScanner = policeScanner;
        VehicleScannerAudio = new VehicleScannerAudio();
        StreetScannerAudio = new StreetScannerAudio();
        ZoneScannerAudio = new ZoneScannerAudio();
        CallsignScannerAudio = new CallsignScannerAudio();
    }
    public void Setup()
    {
        VehicleScannerAudio.ReadConfig();
        StreetScannerAudio.ReadConfig();
        ZoneScannerAudio.ReadConfig();
        CallsignScannerAudio.ReadConfig();
        DefaultConfig();
    }


    //Builder
    private void AddAttentionRandomUnit(DispatchEvent dispatchEvent)
    {
        dispatchEvent.SoundsToPlay.Add(RadioStart.PickRandom());
        dispatchEvent.SoundsToPlay.Add(new List<string>() { attention_unit_specific.Attentionunit.FileName, attention_unit_specific.Dispatchcallingunit.FileName, attention_unit_specific.Dispatchcallingunitumm.FileName,
                                                                    attention_specific.Attentioncaruhh.FileName, attention_specific.Attentioncaruhh1.FileName, attention_specific.Attentioncaruhhorof.FileName, attention_specific.Dispatchtocarumm.FileName, attention_specific.Dispatchtocarumm1.FileName, }.PickRandom());
        dispatchEvent.SoundsToPlay.Add(new List<string>() { car_code_composite.SevenEdwardSeven.FileName,
                                                                    car_code_composite._1Adam13.FileName,
                                                                    car_code_composite._1Adam5.FileName,
                                                                    car_code_composite._1David4.FileName,
                                                                    car_code_composite._2Edward5.FileName,
                                                                    car_code_composite._2Edward6.FileName,
                                                                    car_code_composite._2Lincoln8.FileName,
                                                                    car_code_composite._3Lincoln12.FileName,
                                                                    car_code_composite._3Lincoln2.FileName,
                                                                    car_code_composite._4Mary5.FileName,
                                                                    car_code_composite._6David6.FileName,
                                                                    car_code_composite._7Edward14.FileName,
                                                                    car_code_composite._8Lincoln5.FileName,
                                                                    car_code_composite._8Mary7.FileName,
                                                                    car_code_composite._9David1.FileName,
                                                                    car_code_composite._9Lincoln15.FileName}.PickRandom());
        dispatchEvent.SoundsToPlay.Add(RadioEnd.PickRandom());
    }
    private void AddAttentionUnits(DispatchEvent dispatchEvent)
    {
        if (PoliceScanner.RecentlyMentionedUnits)
        {
            return;
        }
        bool AddedZoneUnits = false;
        bool AddedSingleUnit = false;
        int totalAdded = 0;
        int totalToAdd = Settings.SettingsManager.ScannerSettings.NumberOfUnitsToAnnounce;
        List<string> CallSigns = new List<string>();
        foreach (Cop UnitToCall in World.Pedestrians.Police.Where(x => x.IsRespondingToInvestigation || x.IsRespondingToWanted).OrderBy(x => x.DistanceToPlayer))
        {
            if (UnitToCall != null && UnitToCall.Division != -1)
            {
                string CallSign = $"{UnitToCall.Division}-{UnitToCall.UnityType}-{UnitToCall.BeatNumber}";
                if (!CallSigns.Contains(CallSign))
                {
                    CallSigns.Add(CallSign);
                    EntryPoint.WriteToConsole($"Scanner Calling Specific Unit {CallSign}");
                    List<string> CallsignAudio = CallsignScannerAudio.GetAudio(UnitToCall.Division, UnitToCall.UnityType, UnitToCall.BeatNumber);
                    if (CallsignAudio != null)
                    {
                        if (!AddedSingleUnit)
                        {
                            dispatchEvent.SoundsToPlay.Add(new List<string>() { attention_unit_specific.Attentionunit.FileName, attention_unit_specific.Dispatchcallingunit.FileName, attention_unit_specific.Dispatchcallingunitumm.FileName,
                                                                   // attention_specific.Attentioncaruhh.FileName, attention_specific.Attentioncaruhh1.FileName, attention_specific.Attentioncaruhhorof.FileName, attention_specific.Dispatchtocarumm.FileName, attention_specific.Dispatchtocarumm1.FileName,
                                }.PickRandom());
                            AddedSingleUnit = true;
                            dispatchEvent.NotificationText += $"~n~~s~Responding:";
                        }
                        else
                        {
                            dispatchEvent.SoundsToPlay.Add(new List<string>() { officer.Unituhh.FileName, officer.Unitumm.FileName }.PickRandom());
                        }

                        dispatchEvent.NotificationText += $" ~p~{CallSign}~s~";

                        dispatchEvent.SoundsToPlay.AddRange(CallsignAudio);
                        totalAdded++;
                        AddedZoneUnits = true;
                    }
                }
            }
            if (totalAdded >= totalToAdd)
            {
                break;
            }
        }
        if (!AddedZoneUnits)
        {
            Zone MyZone = Player.CurrentLocation.CurrentZone;
            if (MyZone != null)
            {
                ZoneLookup zoneAudio = ZoneScannerAudio.GetLookup(MyZone.InternalGameName);
                if (zoneAudio != null)
                {
                    string ScannerAudio = zoneAudio.ScannerUnitValues.PickRandom();
                    if (ScannerAudio != "" && ScannerAudio != null && ScannerAudio.Length > 2)
                    {
                        dispatchEvent.SoundsToPlay.Add(ScannerAudio);
                        AddedZoneUnits = true;
                    }
                }
            }
        }
        //if(!AddedZoneUnits)
        //{
        //    dispatchEvent.SoundsToPlay.Add(new List<string>() { attention_unit_specific.Attentionunit.FileName, attention_unit_specific.Dispatchcallingunit.FileName, attention_unit_specific.Dispatchcallingunitumm.FileName,
        //                                                        attention_specific.Attentioncaruhh.FileName, attention_specific.Attentioncaruhh1.FileName, attention_specific.Attentioncaruhhorof.FileName, attention_specific.Dispatchtocarumm.FileName, attention_specific.Dispatchtocarumm1.FileName, }.PickRandom());

        //    dispatchEvent.SoundsToPlay.Add(new List<string>() { car_code_composite.SevenEdwardSeven.FileName,
        //                                                        car_code_composite._1Adam13.FileName,
        //                                                        car_code_composite._1Adam5.FileName,
        //                                                        car_code_composite._1David4.FileName,
        //                                                        car_code_composite._2Edward5.FileName,
        //                                                        car_code_composite._2Edward6.FileName,
        //                                                        car_code_composite._2Lincoln8.FileName,
        //                                                        car_code_composite._3Lincoln12.FileName,
        //                                                        car_code_composite._3Lincoln2.FileName,
        //                                                        car_code_composite._4Mary5.FileName,
        //                                                        car_code_composite._6David6.FileName,
        //                                                        car_code_composite._7Edward14.FileName,
        //                                                        car_code_composite._8Lincoln5.FileName,
        //                                                        car_code_composite._8Mary7.FileName,
        //                                                        car_code_composite._9David1.FileName,
        //                                                        car_code_composite._9Lincoln15.FileName}.PickRandom());
        //    AddedZoneUnits = true;
        //}
        if (AddedZoneUnits)
        {
            dispatchEvent.HasUnitAudio = true;
            dispatchEvent.UnitAudioAmount = totalAdded;
        }
    }
    private void AddAudioSet(DispatchEvent dispatchEvent, AudioSet audioSet)
    {
        if (audioSet != null)
        {
            dispatchEvent.SoundsToPlay.AddRange(audioSet.Sounds);
            dispatchEvent.Subtitles += " " + audioSet.Subtitles;
        }
    }
    private void AddHaveDescription(DispatchEvent dispatchEvent)
    {
        dispatchEvent.NotificationText += "~n~~r~Have Description~s~";
    }
    private void AddHeading(DispatchEvent dispatchEvent)
    {
        dispatchEvent.SoundsToPlay.Add(new List<string>() { suspect_heading.TargetLastSeenHeading.FileName, suspect_heading.TargetReportedHeading.FileName, suspect_heading.TargetSeenHeading.FileName, suspect_heading.TargetSpottedHeading.FileName }.PickRandom());
        dispatchEvent.Subtitles += " ~s~suspect heading~s~";
        string heading = NativeHelper.GetSimpleCompassHeading(Game.LocalPlayer.Character.Heading);
        if (heading == "N")
        {
            dispatchEvent.SoundsToPlay.Add(direction_heading.North.FileName);
            dispatchEvent.Subtitles += " ~g~North~s~";
        }
        else if (heading == "S")
        {
            dispatchEvent.SoundsToPlay.Add(direction_heading.South.FileName);
            dispatchEvent.Subtitles += " ~g~South~s~";
        }
        else if (heading == "E")
        {
            dispatchEvent.SoundsToPlay.Add(direction_heading.East.FileName);
            dispatchEvent.Subtitles += " ~g~East~s~";
        }
        else if (heading == "W")
        {
            dispatchEvent.SoundsToPlay.Add(direction_heading.West.FileName);
            dispatchEvent.Subtitles += " ~g~West~s~";
        }
    }
    private void AddLethalForce(DispatchEvent dispatchEvent)
    {
        if (!PoliceScanner.ReportedLethalForceAuthorized)
        {
            AddAudioSet(dispatchEvent, LethalForce.PickRandom());
            PoliceScanner.ReportedLethalForceAuthorized = true;
        }
    }
    private void AddLocationDescription(DispatchEvent dispatchEvent, LocationSpecificity locationSpecificity)
    {
        BasicLocation NearbyLocation = World.Places.ActiveLocations.Where(x => !string.IsNullOrEmpty(x.ScannerFilePath) && x.DistanceToPlayer <= 100f).OrderBy(x => x.DistanceToPlayer).FirstOrDefault();

        if (NearbyLocation != null && !PoliceScanner.RecentlyMentionedLocation)
        {
            AddLocation(dispatchEvent, NearbyLocation);
        }
        else
        {
            if (locationSpecificity == LocationSpecificity.HeadingAndStreet)
            {
                AddHeading(dispatchEvent);
            }
            if (locationSpecificity == LocationSpecificity.Street || locationSpecificity == LocationSpecificity.HeadingAndStreet || locationSpecificity == LocationSpecificity.StreetAndZone)
            {
                AddStreet(dispatchEvent);
            }
            if (locationSpecificity == LocationSpecificity.Zone || locationSpecificity == LocationSpecificity.StreetAndZone)
            {
                AddZone(dispatchEvent);
            }
        }
    }
    private void AddRapSheet(DispatchEvent dispatchEvent)
    {
        dispatchEvent.NotificationText = "Wanted For:" + Player.PoliceResponse.PrintCrimes();
    }
    private void AddRequestAirSupport(DispatchEvent dispatchEvent)
    {
        if (!PoliceScanner.ReportedRequestAirSupport)
        {
            AddAudioSet(dispatchEvent, new List<AudioSet>()
                {
                    new AudioSet(new List<string>() { officer_requests_air_support.Officersrequestinghelicoptersupport.FileName },"officers requesting helicopter support"),
                    new AudioSet(new List<string>() { officer_requests_air_support.Code99unitsrequestimmediateairsupport.FileName },"code-99 units request immediate air support"),
                    new AudioSet(new List<string>() { officer_requests_air_support.Officersrequireaerialsupport.FileName },"officers require aerial support"),
                    new AudioSet(new List<string>() { officer_requests_air_support.Officersrequireaerialsupport1.FileName },"officers require aerial support"),
                    new AudioSet(new List<string>() { officer_requests_air_support.Officersrequireairsupport.FileName },"officers require air support"),
                    new AudioSet(new List<string>() { officer_requests_air_support.Unitsrequestaerialsupport.FileName },"units request aerial support"),
                    new AudioSet(new List<string>() { officer_requests_air_support.Unitsrequestingairsupport.FileName },"units requesting air support"),
                    new AudioSet(new List<string>() { officer_requests_air_support.Unitsrequestinghelicoptersupport.FileName },"units requesting helicopter support"),
                }.PickRandom());
            PoliceScanner.ReportedRequestAirSupport = true;
            dispatchEvent.NotificationText += "~n~~r~Air Support Requested~s~";
        }
    }
    private void AddSpeed(DispatchEvent dispatchEvent, float Speed)
    {
        Speed = Speed * 2.23694f;//convert to mph
        dispatchEvent.SoundsToPlay.Add(suspect_last_seen.TargetLastReported.FileName);
        dispatchEvent.Subtitles += " ~s~target last reported~s~";
        if (Speed >= 40f)
        {
            if (Speed >= 40f && Speed < 50f)
            {
                dispatchEvent.SoundsToPlay.Add(doing_speed.Doing40mph.FileName);
                dispatchEvent.Subtitles += " ~s~doing ~o~40 mph~s~";
                dispatchEvent.NotificationText += "~n~Speed Exceeding: ~o~40 mph~s~";
            }
            else if (Speed >= 50f && Speed < 60f)
            {
                dispatchEvent.SoundsToPlay.Add(doing_speed.Doing50mph.FileName);
                dispatchEvent.Subtitles += " ~s~doing ~o~50 mph~s~";
                dispatchEvent.NotificationText += "~n~Speed Exceeding: ~o~50 mph~s~";
            }
            else if (Speed >= 60f && Speed < 70f)
            {
                dispatchEvent.SoundsToPlay.Add(doing_speed.Doing60mph.FileName);
                dispatchEvent.Subtitles += " ~s~doing ~o~60 mph~s~";
                dispatchEvent.NotificationText += "~n~Speed Exceeding: ~o~60 mph~s~";
            }
            else if (Speed >= 70f && Speed < 80f)
            {
                dispatchEvent.SoundsToPlay.Add(doing_speed.Doing70mph.FileName);
                dispatchEvent.Subtitles += " ~s~doing ~o~70 mph~s~";
                dispatchEvent.NotificationText += "~n~Speed Exceeding: ~o~70 mph~s~";
            }
            else if (Speed >= 80f && Speed < 90f)
            {
                dispatchEvent.SoundsToPlay.Add(doing_speed.Doing80mph.FileName);
                dispatchEvent.Subtitles += " ~s~doing ~o~80 mph~s~";
                dispatchEvent.NotificationText += "~n~Speed Exceeding: ~o~80 mph~s~";
            }
            else if (Speed >= 90f && Speed < 100f)
            {
                dispatchEvent.SoundsToPlay.Add(doing_speed.Doing90mph.FileName);
                dispatchEvent.Subtitles += " ~s~doing ~o~90 mph~s~";
                dispatchEvent.NotificationText += "~n~Speed Exceeding: ~o~90 mph~s~";
            }
            else if (Speed >= 100f && Speed < 104f)
            {
                dispatchEvent.SoundsToPlay.Add(doing_speed.Doing100mph.FileName);
                dispatchEvent.Subtitles += " ~s~doing ~o~100 mph~s~";
                dispatchEvent.NotificationText += "~n~Speed Exceeding: ~o~100 mph~s~";
            }
            else if (Speed >= 105f)
            {
                dispatchEvent.SoundsToPlay.Add(doing_speed.Doingover100mph.FileName);
                dispatchEvent.Subtitles += " ~s~doing ~o~over 100 mph~s~";
                dispatchEvent.NotificationText += "~n~Speed Exceeding: ~o~105 mph~s~";
            }
        }
        else
        {
            dispatchEvent.SoundsToPlay.Add(crime_speeding.Speeding.FileName);
            dispatchEvent.Subtitles += " ~s~speeding~s~";
            dispatchEvent.NotificationText += "~n~Speeding~s~";
        }
    }
    private void AddStreet(DispatchEvent dispatchEvent)
    {
        if (PoliceScanner.RecentlyMentionedStreet)
        {
            return;
        }
        Street MyStreet = Player.CurrentLocation.CurrentStreet;
        if (MyStreet != null)
        {
            string StreetAudio = StreetScannerAudio.GetAudio(MyStreet.Name);
            if (StreetAudio != "")
            {
                dispatchEvent.SoundsToPlay.Add(new List<string>() { conjunctives.On.FileName, conjunctives.On1.FileName, conjunctives.On2.FileName, conjunctives.On3.FileName, conjunctives.On4.FileName }.PickRandom());
                dispatchEvent.SoundsToPlay.Add(StreetAudio);
                dispatchEvent.Subtitles += " ~s~on ~HUD_COLOUR_YELLOWLIGHT~" + MyStreet.Name + "~s~";
                dispatchEvent.NotificationText += "~n~~HUD_COLOUR_YELLOWLIGHT~" + MyStreet.Name + "~s~";
                dispatchEvent.HasStreetAudio = true;

                if (Player.CurrentLocation.CurrentCrossStreet != null)
                {
                    Street MyCrossStreet = Player.CurrentLocation.CurrentCrossStreet;
                    if (MyCrossStreet != null)
                    {
                        string CrossStreetAudio = StreetScannerAudio.GetAudio(MyCrossStreet.Name);
                        if (CrossStreetAudio != "")
                        {
                            dispatchEvent.SoundsToPlay.Add(new List<string>() { conjunctives.AT01.FileName, conjunctives.AT02.FileName }.PickRandom());
                            dispatchEvent.SoundsToPlay.Add(CrossStreetAudio);
                            dispatchEvent.NotificationText += " ~s~at ~HUD_COLOUR_YELLOWLIGHT~" + MyCrossStreet.Name + "~s~";
                            dispatchEvent.Subtitles += " ~s~at ~HUD_COLOUR_YELLOWLIGHT~" + MyCrossStreet.Name + "~s~";
                        }
                    }
                }
            }
        }
    }
    private void AddVehicleDescription(DispatchEvent dispatchEvent, VehicleExt VehicleToDescribe, bool IncludeLicensePlate, Dispatch DispatchToPlay)
    {
        if (VehicleToDescribe == null)
            return;
        if (VehicleToDescribe.HasBeenDescribedByDispatch)
            return;
        //else
        //    VehicleToDescribe.HasBeenDescribedByDispatch = true;

        if (VehicleToDescribe != null && VehicleToDescribe.Vehicle.Exists())
        {
            dispatchEvent.NotificationText += "~n~Vehicle:~s~";
            //dispatchEvent.SoundsToPlay.Add(suspect_is.SuspectIs.FileName);
            //dispatchEvent.SoundsToPlay.Add(conjunctives.Drivinga.FileName);
            //dispatchEvent.Subtitles += " suspect is driving a ~s~";

            if (VehicleToDescribe.IsPolice)
            {
                dispatchEvent.SoundsToPlay.Add(suspect_is.SuspectIs.FileName);
                if (VehicleToDescribe.Vehicle.IsBike)
                {
                    dispatchEvent.SoundsToPlay.Add(conjunctives.Onuh.FileName);
                }
                else
                {
                    dispatchEvent.SoundsToPlay.Add(conjunctives.DrivingAUmmm.FileName);
                }

                if (RandomItems.RandomPercent(50))
                {
                    dispatchEvent.SoundsToPlay.Add(crime_stolen_cop_car.Astolenpolicevehicle1.FileName);
                }
                else
                {
                    dispatchEvent.SoundsToPlay.Add(crime_stolen_cop_car.Astolenpolicevehicle.FileName);
                }
                dispatchEvent.Subtitles += " suspect is driving a stolen police vehicle ~s~";
            }
            else
            {
                Color CarColor = VehicleToDescribe.VehicleColor(); //Vehicles.VehicleManager.VehicleColor(VehicleToDescribe);
                string MakeName = VehicleToDescribe.MakeName();// Vehicles.VehicleManager.MakeName(VehicleToDescribe);
                int ClassInt = VehicleToDescribe.ClassInt();// Vehicles.VehicleManager.ClassInt(VehicleToDescribe);
                string ClassName = VehicleScannerAudio.ClassName(ClassInt);
                string ModelName = VehicleToDescribe.ModelName();// Vehicles.VehicleManager.ModelName(VehicleToDescribe);

                string ColorAudio = VehicleScannerAudio.GetColorAudio(CarColor);
                string MakeAudio = VehicleScannerAudio.GetMakeAudio(MakeName);
                string ClassAudio = VehicleScannerAudio.GetClassAudio(ClassInt);
                string ModelAudio = VehicleScannerAudio.GetModelAudio(VehicleToDescribe.Vehicle.Model.Hash);

                //if(VehicleToDescribe.IsStolen)
                //{
                //    dispatchEvent.SoundsToPlay.Add(suspect_is.SuspectIs.FileName);
                //    dispatchEvent.SoundsToPlay.Add(conjunctives.In.FileName);
                //    dispatchEvent.Subtitles += " suspect is in a stolen vehicle, a ~s~";
                //    dispatchEvent.SoundsToPlay.Add(crime_10_851.Astolenvehicle.FileName);
                //    dispatchEvent.SoundsToPlay.Add(conjunctives.A01.FileName);
                //}
                //else
                //{
                dispatchEvent.SoundsToPlay.Add(suspect_is.SuspectIs.FileName);
                dispatchEvent.SoundsToPlay.Add(conjunctives.Drivinga.FileName);
                dispatchEvent.Subtitles += " suspect is driving a ~s~";

                //}

                if (ColorAudio != "")
                {
                    dispatchEvent.SoundsToPlay.Add(ColorAudio);
                    dispatchEvent.Subtitles += " ~s~" + CarColor.Name + "~s~";
                    dispatchEvent.NotificationText += " ~s~" + CarColor.Name + "~s~";
                }
                if (MakeAudio != "")
                {
                    dispatchEvent.SoundsToPlay.Add(MakeAudio);
                    dispatchEvent.Subtitles += " ~s~" + MakeName + "~s~";
                    dispatchEvent.NotificationText += " ~s~" + MakeName + "~s~";
                }

                if (ModelAudio != "")
                {
                    dispatchEvent.SoundsToPlay.Add(ModelAudio);
                    dispatchEvent.Subtitles += " ~s~" + ModelName + "~s~";
                    dispatchEvent.NotificationText += " ~s~" + ModelName + "~s~";
                }
                else if (ClassAudio != "")
                {
                    dispatchEvent.SoundsToPlay.Add(ClassAudio);
                    dispatchEvent.Subtitles += " ~s~" + ClassName + "~s~";
                    dispatchEvent.NotificationText += " ~s~" + ClassName + "~s~";
                }
            }
            if (IncludeLicensePlate)
            {
                AddAudioSet(dispatchEvent, LicensePlateSet.PickRandom());
                string LicensePlateText = VehicleToDescribe.OriginalLicensePlate.PlateNumber;
                dispatchEvent.SoundsToPlay.AddRange(VehicleScannerAudio.GetPlateAudio(LicensePlateText));
                dispatchEvent.Subtitles += " ~s~" + LicensePlateText + "~s~";
                dispatchEvent.NotificationText += " ~s~Plate: " + LicensePlateText + "~s~";
            }
            if (DispatchToPlay.Name == "Suspicious Vehicle")
            {
                dispatchEvent.NotificationText += "~n~~s~For: " + VehicleToDescribe.IsSuspicious(Time.IsNight) + "~s~";
            }
            //EntryPoint.WriteToConsole(string.Format("ScannerScript Color {0}, Make {1}, Class {2}, Model {3}, RawModel {4}", CarColor.Name, MakeName, ClassName, ModelName, VehicleToDescribe.Vehicle.Model.Name));
        }
    }
    private void AddWeaponDescription(DispatchEvent dispatchEvent, WeaponInformation WeaponToDescribe)
    {
        dispatchEvent.NotificationText += "~n~Weapon:~s~";
        dispatchEvent.SoundsToPlay.Add(suspect_is.SuspectIs.FileName);
        if (WeaponToDescribe == null)
        {
            dispatchEvent.SoundsToPlay.Add(carrying_weapon.Carryingaweapon.FileName);
            dispatchEvent.Subtitles += " suspect is carrying a ~r~weapon~s~";
            dispatchEvent.NotificationText += " Unknown";
        }
        else if (WeaponToDescribe.ModelName == "weapon_rpg")
        {
            dispatchEvent.SoundsToPlay.Add(carrying_weapon.ArmedwithanRPG.FileName);
            dispatchEvent.Subtitles += " suspect is armed with an ~r~RPG~s~";
            dispatchEvent.NotificationText += " RPG";
        }
        else if (WeaponToDescribe.ModelName == "weapon_bat")
        {
            dispatchEvent.SoundsToPlay.Add(carrying_weapon.Armedwithabat.FileName);
            dispatchEvent.Subtitles += " suspect is armed with a ~r~bat~s~";
            dispatchEvent.NotificationText += " Bat";
        }
        else if (WeaponToDescribe.ModelName == "weapon_grenadelauncher" || WeaponToDescribe.ModelName == "weapon_grenadelauncher_smoke" || WeaponToDescribe.ModelName == "weapon_compactlauncher")
        {
            dispatchEvent.SoundsToPlay.Add(carrying_weapon.Armedwithagrenadelauncher.FileName);
            dispatchEvent.Subtitles += " suspect is armed with a ~r~grenade launcher~s~";
            dispatchEvent.NotificationText += " Grenade Launcher";
        }
        else if (WeaponToDescribe.Category == WeaponCategory.Throwable || WeaponToDescribe.ModelName == "weapon_grenadelauncher_smoke" || WeaponToDescribe.ModelName == "weapon_compactlauncher")
        {
            dispatchEvent.SoundsToPlay.Add(carrying_weapon.Armedwithexplosives.FileName);
            dispatchEvent.Subtitles += " suspect is armed with ~r~explosives~s~";
            dispatchEvent.NotificationText += " Explosives";
        }
        else if (WeaponToDescribe.ModelName == "weapon_dagger" || WeaponToDescribe.ModelName == "weapon_knife" || WeaponToDescribe.ModelName == "weapon_switchblade")
        {
            dispatchEvent.SoundsToPlay.Add(carrying_weapon.Armedwithaknife.FileName);
            dispatchEvent.Subtitles += " suspect is armed with a ~r~knife~s~";
            dispatchEvent.NotificationText += " Knife";
        }
        else if (WeaponToDescribe.ModelName == "weapon_minigun")
        {
            dispatchEvent.SoundsToPlay.Add(carrying_weapon.Armedwithaminigun.FileName);
            dispatchEvent.Subtitles += " suspect is armed with a ~r~minigun~s~";
            dispatchEvent.NotificationText += " Minigun";
        }
        else if (WeaponToDescribe.ModelName == "weapon_sawnoffshotgun")
        {
            dispatchEvent.SoundsToPlay.Add(carrying_weapon.Armedwithasawedoffshotgun.FileName);
            dispatchEvent.Subtitles += " suspect is armed with a ~r~sawed off shotgun~s~";
            dispatchEvent.NotificationText += " Sawed Off Shotgun";
        }
        else if (WeaponToDescribe.Category == WeaponCategory.LMG)
        {
            dispatchEvent.SoundsToPlay.Add(carrying_weapon.Armedwithamachinegun.FileName);
            dispatchEvent.Subtitles += " suspect is armed with a ~r~machine gun~s~";
            dispatchEvent.NotificationText += " Machine Gun";
        }
        else if (WeaponToDescribe.Category == WeaponCategory.Pistol)
        {
            dispatchEvent.SoundsToPlay.Add(carrying_weapon.Armedwithafirearm.FileName);
            dispatchEvent.Subtitles += " suspect is armed with a ~r~pistol~s~";
            dispatchEvent.NotificationText += " Pistol";
        }
        else if (WeaponToDescribe.Category == WeaponCategory.Shotgun)
        {
            dispatchEvent.SoundsToPlay.Add(carrying_weapon.Armedwithashotgun.FileName);
            dispatchEvent.Subtitles += " suspect is armed with a ~r~shotgun~s~";
            dispatchEvent.NotificationText += " Shotgun";
        }
        else if (WeaponToDescribe.Category == WeaponCategory.SMG)
        {
            dispatchEvent.SoundsToPlay.Add(carrying_weapon.Armedwithasubmachinegun.FileName);
            dispatchEvent.Subtitles += " suspect is armed with a ~r~submachine gun~s~";
            dispatchEvent.NotificationText += " Submachine Gun";
        }
        else if (WeaponToDescribe.Category == WeaponCategory.AR)
        {
            dispatchEvent.SoundsToPlay.Add(carrying_weapon.Carryinganassaultrifle.FileName);
            dispatchEvent.Subtitles += " suspect is carrying an ~r~assault rifle~s~";
            dispatchEvent.NotificationText += " Assault Rifle";
        }
        else if (WeaponToDescribe.Category == WeaponCategory.Sniper)
        {
            dispatchEvent.SoundsToPlay.Add(carrying_weapon.Armedwithasniperrifle.FileName);
            dispatchEvent.Subtitles += " suspect is armed with a ~r~sniper rifle~s~";
            dispatchEvent.NotificationText += " Sniper Rifle";
        }
        else if (WeaponToDescribe.Category == WeaponCategory.Heavy)
        {
            dispatchEvent.SoundsToPlay.Add(status_message.HeavilyArmed.FileName);
            dispatchEvent.Subtitles += " suspect is ~r~heaviy armed~s~";
            dispatchEvent.NotificationText += " Heavy Weapon";
        }
        else if (WeaponToDescribe.Category == WeaponCategory.Melee)
        {
            dispatchEvent.SoundsToPlay.Add(carrying_weapon.Carryingaweapon.FileName);
            dispatchEvent.Subtitles += " suspect is carrying a ~r~weapon~s~";
            dispatchEvent.NotificationText += " Close Combat Weapon";
        }
        else
        {
            int Num = RandomItems.MyRand.Next(1, 5);
            if (Num == 1)
            {
                dispatchEvent.SoundsToPlay.Add(carrying_weapon.Armedwithafirearm.FileName);
                dispatchEvent.Subtitles += " suspect is armed with a ~r~firearm~s~";
            }
            else if (Num == 2)
            {
                dispatchEvent.SoundsToPlay.Add(carrying_weapon.Armedwithagat.FileName);
                dispatchEvent.Subtitles += " suspect is armed with a ~r~gat~s~";
            }
            else if (Num == 3)
            {
                dispatchEvent.SoundsToPlay.Add(carrying_weapon.Carryingafirearm.FileName);
                dispatchEvent.Subtitles += " suspect is carrying a ~r~firearm~s~";
            }
            else
            {
                dispatchEvent.SoundsToPlay.Add(carrying_weapon.Carryingagat.FileName);
                dispatchEvent.Subtitles += " suspect is carrying a ~r~gat~s~";
            }
            dispatchEvent.NotificationText += " Gat";
        }
    }
    private void AddWeaponsFree(DispatchEvent dispatchEvent)
    {
        if (!PoliceScanner.ReportedWeaponsFree)
        {
            AddAudioSet(dispatchEvent, new AudioSet(new List<string>() { custom_wanted_level_line.Suspectisarmedanddangerousweaponsfree.FileName }, "suspect is armed and dangerous, weapons free"));
            dispatchEvent.NotificationText += "~n~~r~Weapons Free~s~";
            PoliceScanner.ReportedWeaponsFree = true;
        }
    }
    private void AddLocation(DispatchEvent dispatchEvent, BasicLocation location)
    {
        if (PoliceScanner.RecentlyMentionedLocation)
        {
            return;
        }
        if (location != null)
        {
            string ScannerAudio = location.ScannerFilePath;
            if (ScannerAudio != "")
            {
                //dispatchEvent.HasZoneAudio = true;
                //if (MyZone.IsSpecificLocation || Settings.SettingsManager.ScannerSettings.UseNearForLocations)
                //{

                dispatchEvent.SoundsToPlay.Add(new List<string> { suspect_last_seen.TargetLastSeen.FileName, suspect_last_seen.TargetLastReported.FileName, suspect_last_seen.SuspectSpotted.FileName, suspect_last_seen.TargetIs.FileName, suspect_last_seen.TargetSpotted.FileName }.PickRandom());
                dispatchEvent.Subtitles += " ~s~suspect seen~s~";

                dispatchEvent.SoundsToPlay.Add(new List<string> { conjunctives.Nearumm.FileName, conjunctives.Closetoum.FileName, conjunctives.Closetouhh.FileName }.PickRandom());
                dispatchEvent.Subtitles += " ~s~near ~p~" + location.Name + "~s~";
                //}
                //else
                //{
                //    dispatchEvent.SoundsToPlay.Add(new List<string> { conjunctives.In.FileName }.PickRandom());
                //    dispatchEvent.Subtitles += " ~s~in ~p~" + location.Name + "~s~";
                //}
                dispatchEvent.SoundsToPlay.Add(ScannerAudio);
                dispatchEvent.NotificationText += "~n~~p~" + location.Name + "~s~";
                dispatchEvent.HasLocationAudio = true;
                location.GameTimeLastMentioned = Game.GameTime;
            }
        }
    }
    private void AddZone(DispatchEvent dispatchEvent)
    {
        if (PoliceScanner.RecentlyMentionedZone)
        {
            return;
        }
        Zone MyZone = Player.CurrentLocation.CurrentZone;
        if (MyZone != null)
        {
            string ScannerAudio = ZoneScannerAudio.GetAudio(MyZone.InternalGameName);
            if (ScannerAudio != "")
            {
                dispatchEvent.HasZoneAudio = true;
                if (MyZone.IsSpecificLocation || Settings.SettingsManager.ScannerSettings.UseNearForLocations)
                {
                    dispatchEvent.SoundsToPlay.Add(new List<string> { conjunctives.Nearumm.FileName, conjunctives.Closetoum.FileName, conjunctives.Closetouhh.FileName }.PickRandom());
                    dispatchEvent.Subtitles += " ~s~near ~p~" + MyZone.DisplayName + "~s~";
                }
                else
                {
                    dispatchEvent.SoundsToPlay.Add(new List<string> { conjunctives.In.FileName }.PickRandom());
                    dispatchEvent.Subtitles += " ~s~in ~p~" + MyZone.DisplayName + "~s~";
                }
                dispatchEvent.SoundsToPlay.Add(ScannerAudio);
                dispatchEvent.NotificationText += "~n~~p~" + MyZone.DisplayName + "~s~";
            }
        }
    }
    public void BuildDispatch(Dispatch DispatchToPlay, bool addtoPlayed)
    {
        EntryPoint.WriteToConsole($"SCANNER EVENT: Building {DispatchToPlay.Name}, MarkVehicleAsStolen: {DispatchToPlay.MarkVehicleAsStolen} Vehicle: {DispatchToPlay.LatestInformation?.VehicleSeen?.Vehicle.Handle} Instances: {DispatchToPlay.LatestInformation?.InstancesObserved}", 3);
        DispatchEvent EventToPlay = new DispatchEvent();
        if (DispatchToPlay.HasPreamble)
        {
            EventToPlay.SoundsToPlay.Add(RadioStart.PickRandom());
            AddAudioSet(EventToPlay, DispatchToPlay.PreambleAudioSet.PickRandom());
            EventToPlay.SoundsToPlay.Add(RadioEnd.PickRandom());
        }
        EventToPlay.SoundsToPlay.Add(RadioStart.PickRandom());
        EventToPlay.NotificationTitle = DispatchToPlay.NotificationTitle;

        if (DispatchToPlay.NotificationSubtitle != "")
        {
            EventToPlay.NotificationSubtitle = DispatchToPlay.NotificationSubtitle + "~s~";
        }
        else if (DispatchToPlay.IsStatus)
        {
            EventToPlay.NotificationSubtitle = "~g~Status";
        }
        else if (DispatchToPlay.LatestInformation.SeenByOfficers)
        {
            EventToPlay.NotificationSubtitle = "~r~Crime Observed";
        }
        else
        {
            EventToPlay.NotificationSubtitle = "~o~Crime Reported";
        }
        EventToPlay.NotificationText = DispatchToPlay.NotificationText;

        if (DispatchToPlay.IncludeAttentionAllUnits)
        {
            AddAudioSet(EventToPlay, AttentionAllUnits.PickRandom());
        }
        else if (!DispatchToPlay.LatestInformation.SeenByOfficers && !DispatchToPlay.IsStatus)
        {
            AddAttentionUnits(EventToPlay);
        }

        if (DispatchToPlay.IncludeReportedBy)
        {
            if (DispatchToPlay.LatestInformation.SeenByOfficers)
            {
                AddAudioSet(EventToPlay, OfficersReport.PickRandom());
            }
            else
            {
                AddAudioSet(EventToPlay, CiviliansReport.PickRandom());
            }
        }
        if (DispatchToPlay.LatestInformation.InstancesObserved > 1 && DispatchToPlay.MainMultiAudioSet.Any())
        {
            AddAudioSet(EventToPlay, DispatchToPlay.MainMultiAudioSet.PickRandom());
        }
        else
        {
            AddAudioSet(EventToPlay, DispatchToPlay.MainAudioSet.PickRandom());
        }

        if (DispatchToPlay.SecondaryAudioSet.Any())
        {
            AddAudioSet(EventToPlay, DispatchToPlay.SecondaryAudioSet.PickRandom());
        }
        if (DispatchToPlay.IncludeDrivingVehicle)
        {
            AddVehicleDescription(EventToPlay, DispatchToPlay.LatestInformation.VehicleSeen, !DispatchToPlay.LatestInformation.SeenByOfficers && DispatchToPlay.IncludeLicensePlate, DispatchToPlay);
            GameFiber.Yield();
        }
        if (DispatchToPlay.IncludeRapSheet)
        {
            AddRapSheet(EventToPlay);
        }
        if (DispatchToPlay.MarkVehicleAsStolen && DispatchToPlay.LatestInformation != null && DispatchToPlay.LatestInformation.VehicleSeen != null && Player.CurrentVehicle != null)//temp current vehicle BS
        {
            //THIS NEED TO NOT BE CURRENT VEHICLE, BUT OTHERWISE THE LINK GETS MESSED UP?
            Player.CurrentVehicle.WasReportedStolen = true;
            Player.CurrentVehicle.OriginalLicensePlate.IsWanted = true;
            if (Player.CurrentVehicle.OriginalLicensePlate.PlateNumber == Player.CurrentVehicle.CarPlate.PlateNumber)
            {
                Player.CurrentVehicle.CarPlate.IsWanted = true;
            }
        }
        if (DispatchToPlay.IncludeCarryingWeapon && (DispatchToPlay.LatestInformation.WeaponSeen != null || DispatchToPlay.Name == "Carrying Weapon"))
        {
            AddWeaponDescription(EventToPlay, DispatchToPlay.LatestInformation.WeaponSeen);
            GameFiber.Yield();
        }
        if (DispatchToPlay.ResultsInLethalForce && !LethalForceAuthorized.HasBeenPlayedThisWanted && DispatchToPlay.Name != LethalForceAuthorized.Name)
        {
            AddLethalForce(EventToPlay);
        }
        if (DispatchToPlay.CanAddExtras && Player.IsWanted && !Player.IsDead && Player.PoliceResponse.IsWeaponsFree && !WeaponsFree.HasBeenPlayedThisWanted && DispatchToPlay.Name != WeaponsFree.Name)
        {
            AddWeaponsFree(EventToPlay);
        }
        if (DispatchToPlay.CanAddExtras && Player.IsWanted && !Player.IsDead && World.Pedestrians.AnyHelicopterUnitsSpawned && !RequestAirSupport.HasBeenPlayedThisWanted && DispatchToPlay.Name != RequestAirSupport.Name)
        {
            AddRequestAirSupport(EventToPlay);
        }
        if (DispatchToPlay.CanAddExtras && DispatchToPlay.IncludeDrivingSpeed && Player.CurrentVehicle != null && Player.CurrentVehicle.Vehicle.Exists())
        {
            if (DispatchToPlay.LatestInformation.Speed <= Player.Character.Speed)
            {
                AddSpeed(EventToPlay, Player.Character.Speed);
            }
            else
            {
                AddSpeed(EventToPlay, DispatchToPlay.LatestInformation.Speed);
            }
            //AddSpeed(EventToPlay, DispatchToPlay.LatestInformation.Speed);// CurrentPlayer.CurrentVehicle.Vehicle.Speed);
            GameFiber.Yield();
        }
        if (DispatchToPlay.LocationDescription != LocationSpecificity.Nothing)
        {
            AddLocationDescription(EventToPlay, DispatchToPlay.LocationDescription);
            GameFiber.Yield();
        }
        if (DispatchToPlay.CanAddExtras && Player.PoliceResponse.PoliceHaveDescription && !DispatchToPlay.LatestInformation.SeenByOfficers && !DispatchToPlay.IsStatus)
        {
            AddHaveDescription(EventToPlay);
        }
        if (EventToPlay.SoundsToPlay.Count() == 1)//only has radio beep
        {
            return;
        }

        if (EventToPlay.HasUnitAudio)
        {
            if (Player.Investigation.InvestigationWantedLevel == 1)
            {
                EventToPlay.NotificationText += "~n~~o~Responding Code-2~s~";
                AddAudioSet(EventToPlay, RespondCode2Set.PickRandom());
            }
            else if (Player.Investigation.InvestigationWantedLevel > 1)
            {
                EventToPlay.NotificationText += "~n~~r~Responding Code-3~s~";
                AddAudioSet(EventToPlay, RespondCode3Set.PickRandom());
            }
        }

        EventToPlay.SoundsToPlay.Add(RadioEnd.PickRandom());

        if (EventToPlay.HasUnitAudio)
        {
            foreach (AudioSet audioSet in UnitEnRouteSet.OrderBy(x => Guid.NewGuid()).Take(EventToPlay.UnitAudioAmount).ToList())
            {
                EventToPlay.SoundsToPlay.Add(RadioStart.PickRandom());
                AddAudioSet(EventToPlay, audioSet);
                EventToPlay.SoundsToPlay.Add(RadioEnd.PickRandom());
            }
        }

        if (EventToPlay.Subtitles != "")
        {
            EventToPlay.Subtitles = NativeHelper.FirstCharToUpper(EventToPlay.Subtitles);
        }
        EventToPlay.Priority = DispatchToPlay.Priority;

        if (addtoPlayed)
        {
            DispatchToPlay.SetPlayed();
            if (DispatchToPlay.LatestInformation.SeenByOfficers && DispatchToPlay.Priority < PoliceScanner.HighestOfficerReportedPriority)
            {
                PoliceScanner.HighestOfficerReportedPriority = DispatchToPlay.Priority;
            }
            else if (!DispatchToPlay.LatestInformation.SeenByOfficers && !DispatchToPlay.IsStatus && DispatchToPlay.Priority < PoliceScanner.HighestCivilianReportedPriority)
            {
                PoliceScanner.HighestCivilianReportedPriority = DispatchToPlay.Priority;
            }
        }
        GameFiber.Yield();

        if (DispatchToPlay.CanAlwaysBeInterrupted)
        {
            EventToPlay.CanBeInterrupted = true;
        }
        if (DispatchToPlay.CanAlwaysInterrupt)
        {
            EventToPlay.CanInterrupt = true;
        }
        if (DispatchToPlay.AnyDispatchInterrupts)
        {
            EventToPlay.AnyDispatchInterrupts = true;
        }


        PoliceScanner.PlayDispatch(EventToPlay, DispatchToPlay.LatestInformation, DispatchToPlay);



        //PoliceScanner.pla
    }



    private void DefaultConfig()
    {
        SetupDispatches();
        DispatchLookup = new List<CrimeDispatch>
        {
            new CrimeDispatch("AttemptingSuicide",AttemptingSuicide),
            new CrimeDispatch("BrandishingWeapon",CarryingWeapon),
            new CrimeDispatch("ChangingPlates",TamperingWithVehicle),
            new CrimeDispatch("DrivingAgainstTraffic",RecklessDriving),
            new CrimeDispatch("DrivingOnPavement",RecklessDriving),
            new CrimeDispatch("FelonySpeeding",FelonySpeeding),
            new CrimeDispatch("FiringWeapon",ShotsFired),
            new CrimeDispatch("FiringWeaponNearPolice",ShotsFiredAtAnOfficer),
            new CrimeDispatch("GotInAirVehicleDuringChase",StealingAirVehicle),
            new CrimeDispatch("GrandTheftAuto",GrandTheftAuto),
            new CrimeDispatch("HitCarWithCar",VehicleHitAndRun),
            new CrimeDispatch("HitPedWithCar",PedHitAndRun),
            new CrimeDispatch("RunningARedLight",RunningARedLight),
            new CrimeDispatch("HurtingCivilians",CivilianInjury),
            new CrimeDispatch("HurtingPolice",AssaultingOfficer),
            new CrimeDispatch("KillingCivilians",CivilianDown),
            new CrimeDispatch("KillingPolice",OfficerDown),
            new CrimeDispatch("Mugging",Mugging),
            new CrimeDispatch("NonRoadworthyVehicle",SuspiciousVehicle),
            new CrimeDispatch("ResistingArrest",ResistingArrest),
            new CrimeDispatch("TrespessingOnGovtProperty",TrespassingOnGovernmentProperty),
            new CrimeDispatch("DrivingStolenVehicle",DrivingAtStolenVehicle),
            new CrimeDispatch("TerroristActivity",TerroristActivity),
            new CrimeDispatch("BrandishingCloseCombatWeapon",CarryingWeapon),
            new CrimeDispatch("SuspiciousActivity",SuspiciousActivity),
            new CrimeDispatch("DrunkDriving",DrunkDriving),
            new CrimeDispatch("Kidnapping",Kidnapping),
            new CrimeDispatch("PublicIntoxication",PublicIntoxication),
            new CrimeDispatch("InsultingOfficer",OfficerNeedsAssistance),//these are bad
            new CrimeDispatch("OfficersNeeded",OfficersNeeded),
            new CrimeDispatch("Harassment",Harassment),
            new CrimeDispatch("AssaultingCivilians",AssaultingCivilians),
            new CrimeDispatch("AssaultingWithDeadlyWeapon",AssaultingCiviliansWithDeadlyWeapon),
            new CrimeDispatch("DealingDrugs",DealingDrugs),
            new CrimeDispatch("DealingGuns",DealingGuns),
            new CrimeDispatch("AimingWeaponAtPolice",AimingWeaponAtPolice),
            new CrimeDispatch("ArmedRobbery",ArmedRobbery),
            new CrimeDispatch("PublicNuisance",PublicNuisance),
            new CrimeDispatch("PublicVagrancy",PublicVagrancy),
        };
        DispatchList = new List<Dispatch>
        {
            OfficerDown
            ,ShotsFiredAtAnOfficer
            ,AssaultingOfficer
            ,ThreateningOfficerWithFirearm
            ,TrespassingOnGovernmentProperty
            ,StealingAirVehicle
            ,ShotsFired
            ,CarryingWeapon
            ,CivilianDown
            ,CivilianShot
            ,CivilianInjury
            ,GrandTheftAuto
            ,SuspiciousActivity
            ,CriminalActivity
            ,Mugging
            ,TerroristActivity
            ,SuspiciousVehicle
            ,DrivingAtStolenVehicle
            ,ResistingArrest
            ,AttemptingSuicide
            ,FelonySpeeding
            ,PedHitAndRun
            ,VehicleHitAndRun
            ,RecklessDriving
            ,AnnounceStolenVehicle
            ,RequestAirSupport
            ,RequestMilitaryUnits
            ,RequestNOOSEUnits
            ,SuspectSpotted
            ,SuspectEvaded
            ,RemainInArea
            ,ResumePatrol
            ,AttemptToReacquireSuspect
            ,NoFurtherUnitsNeeded
            ,SuspectArrested
            ,SuspectWasted
            ,ChangedVehicles
            ,RequestBackup
            ,RequestBackupSimple
            ,WeaponsFree
            ,LethalForceAuthorized
            ,RunningARedLight
            ,DrunkDriving
            ,Kidnapping
            ,PublicIntoxication
            ,Harassment
            ,OfficerNeedsAssistance
            ,OfficersNeeded
            ,AssaultingCivilians
            ,AssaultingCiviliansWithDeadlyWeapon
            ,DealingDrugs
            ,DealingGuns
            ,WantedSuspectSpotted
            ,RequestNooseUnitsAlt
            ,RequestNooseUnitsAlt2
            ,RequestFIBUnits
            ,RequestSwatAirSupport
            ,AimingWeaponAtPolice
            ,OnFoot
            ,ExcessiveSpeed
            ,GotOnFreeway
            ,GotOffFreeway
            ,TamperingWithVehicle
            ,VehicleCrashed
            ,VehicleStartedFire
            ,ArmedRobbery
            ,MedicalServicesRequired
            ,FirefightingServicesRequired
            ,PublicNuisance
            ,ShotsFiredStatus
        };
    }
    private void SetupDispatches()
    {
        RadioStart = new List<string>() { AudioBeeps.Radio_Start_1.FileName };
        RadioEnd = new List<string>() { AudioBeeps.Radio_End_1.FileName };
        AttentionAllUnits = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { attention_all_units_gen.Attentionallunits.FileName},"attention all units"),
                new AudioSet(new List<string>() { attention_all_units_gen.Attentionallunits1.FileName },"attention all units"),
                new AudioSet(new List<string>() { attention_all_units_gen.Attentionallunits3.FileName },"attention all units"),
            };
        OfficersReport = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { we_have.OfficersReport_1.FileName},"officers report"),
                new AudioSet(new List<string>() { we_have.OfficersReport_2.FileName },"officers report"),
                new AudioSet(new List<string>() { we_have.UnitsReport_1.FileName },"units report"),
            };
        CiviliansReport = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { we_have.CitizensReport_1.FileName },"citizens report"),
                new AudioSet(new List<string>() { we_have.CitizensReport_2.FileName },"citizens report"),
                new AudioSet(new List<string>() { we_have.CitizensReport_3.FileName },"citizens report"),
                new AudioSet(new List<string>() { we_have.CitizensReport_4.FileName },"citizens report"),
            };
        LethalForce = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { lethal_force.Useofdeadlyforceauthorized.FileName},"use of deadly force authorized"),
                new AudioSet(new List<string>() { lethal_force.Useofdeadlyforceisauthorized.FileName },"use of deadly force is authorized"),
                new AudioSet(new List<string>() { lethal_force.Useofdeadlyforceisauthorized1.FileName },"use of deadly force is authorized"),
                new AudioSet(new List<string>() { lethal_force.Useoflethalforceisauthorized.FileName },"use of lethal force is authorized"),
                new AudioSet(new List<string>() { lethal_force.Useofdeadlyforcepermitted1.FileName },"use of deadly force permitted"),
            };
        LicensePlateSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { suspect_license_plate.SuspectLicensePlate.FileName},"suspect license plate"),
                new AudioSet(new List<string>() { suspect_license_plate.SuspectsLicensePlate01.FileName },"suspects license plate"),
                new AudioSet(new List<string>() { suspect_license_plate.SuspectsLicensePlate02.FileName },"suspects license plate"),
                new AudioSet(new List<string>() { suspect_license_plate.TargetLicensePlate.FileName },"target license plate"),
                new AudioSet(new List<string>() { suspect_license_plate.TargetsLicensePlate.FileName },"targets license plate"),
                new AudioSet(new List<string>() { suspect_license_plate.TargetVehicleLicensePlate.FileName },"target vehicle license plate"),
            };
        RespondCode3Set = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { dispatch_respond_code.UnitsrespondCode3.FileName },"units respond code-3"),
                new AudioSet(new List<string>() { dispatch_respond_code.RespondCode3.FileName },"units respond code-3"),
                new AudioSet(new List<string>() { dispatch_respond_code.UnitrespondCode3.FileName },"units respond code-3"),
            };
        RespondCode2Set = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { dispatch_respond_code.RespondCode2.FileName },"units respond code-2"),
                new AudioSet(new List<string>() { dispatch_respond_code.UnitrespondCode2.FileName },"units respond code-2"),
                new AudioSet(new List<string>() { dispatch_respond_code.UnitsrespondCode2.FileName },"units respond code-2"),
            };

        UnitEnRouteSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { s_m_y_cop_black_full_02.RogerEnRoute1.FileName},"Copy Dispatch."),
                new AudioSet(new List<string>() { s_m_y_cop_black_mini_01.RogerEnRoute1.FileName},"Copy that we are on our way."),
               // new AudioSet(new List<string>() { s_m_y_cop_black_mini_02.RogerEnRoute1.FileName},"we are en route"),//victor 13
                new AudioSet(new List<string>() { s_m_y_cop_black_mini_03.RogerEnRoute1.FileName},"Acknowledged, on our way."),
                //new AudioSet(new List<string>() { s_m_y_cop_black_mini_04.RogerEnRoute1.FileName},"we are en route"),//ocean-1
                new AudioSet(new List<string>() { s_m_y_cop_white_full_01.RogerEnRoute1.FileName},"Copy that we are on our way."),
                new AudioSet(new List<string>() { s_m_y_cop_white_full_02.RogerEnRoute1.FileName},"Copy that we are on our way."),
                new AudioSet(new List<string>() { s_m_y_cop_white_mini_02.RogerEnRoute1.FileName},"Copy that we are on our way."),
                //new AudioSet(new List<string>() { s_m_y_cop_white_mini_03.RogerEnRoute1.FileName},"we are en route"),//Specific unit
                //new AudioSet(new List<string>() { s_m_y_cop_white_mini_04.RogerEnRoute1.FileName},"we are en route"),//Specific unit
                new AudioSet(new List<string>() { s_m_y_hwaycop_black_full_01.RogerEnRoute1.FileName},"Copy that we are on our way."),
                //new AudioSet(new List<string>() { s_m_y_hwaycop_black_full_02.RogerEnRoute1.FileName},"we are en route"),//Specific unit
               // new AudioSet(new List<string>() { s_m_y_hwaycop_white_full_01.RogerEnRoute1.FileName},"we are en route"),//Specific unit
               // new AudioSet(new List<string>() { s_m_y_hwaycop_white_full_02.RogerEnRoute1.FileName},"we are en route"),//Specific unit
            };

        OfficerDown = new Dispatch()
        {
            Name = "Officer Down",
            IncludeAttentionAllUnits = true,
            ResultsInLethalForce = true,

            LocationDescription = LocationSpecificity.StreetAndZone,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { we_have.We_Have_1.FileName, crime_officer_down.AcriticalsituationOfficerdown.FileName },"we have a critical situation, officer down"),
               // new AudioSet(new List<string>() { we_have.We_Have_1.FileName, crime_officer_down.AnofferdownpossiblyKIA.FileName },"we have an officer down, possibly KIA"),
                new AudioSet(new List<string>() { we_have.We_Have_1.FileName, crime_officer_down.Anofficerdown.FileName },"we have an officer down"),
               // new AudioSet(new List<string>() { we_have.We_Have_2.FileName, crime_officer_down.Anofficerdownconditionunknown.FileName },"we have an officer down, condition unknown"),
            },
            SecondaryAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { dispatch_respond_code.AllunitsrespondCode99.FileName },"all units repond code-99"),
                new AudioSet(new List<string>() { dispatch_respond_code.AllunitsrespondCode99emergency.FileName },"all units repond code-99 emergency"),
                new AudioSet(new List<string>() { dispatch_respond_code.Code99allunitsrespond.FileName },"code-99 all units repond"),
                new AudioSet(new List<string>() { custom_wanted_level_line.Code99allavailableunitsconvergeonsuspect.FileName },"code-99 all available units converge on suspect"),
                new AudioSet(new List<string>() { custom_wanted_level_line.Wehavea1099allavailableunitsrespond.FileName },"we have a 10-99  all available units repond"),
                new AudioSet(new List<string>() { dispatch_respond_code.Code99allunitsrespond.FileName },"code-99 all units respond"),
                new AudioSet(new List<string>() { dispatch_respond_code.EmergencyallunitsrespondCode99.FileName },"emergency all units respond code-99"),
                new AudioSet(new List<string>() { escort_boss.Immediateassistancerequired.FileName },"immediate assistance required"),
            },
            MainMultiAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { we_have.We_Have_1.FileName, crime_officers_down.Multipleofficersdown.FileName },"we have multiple officers down"),
                new AudioSet(new List<string>() { we_have.We_Have_2.FileName, crime_officers_down.Severalofficersdown.FileName },"we have several officers down"),
            },
        };

        OfficerMIA = new Dispatch()
        {
            Name = "Officer MIA",
            IncludeAttentionAllUnits = true,
            ResultsInLethalForce = true,

            LocationDescription = LocationSpecificity.Street,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { we_have.We_Have_1.FileName, crime_officer_down.AnofferdownpossiblyKIA.FileName },"we have an officer down, possibly KIA"),
                new AudioSet(new List<string>() { we_have.We_Have_2.FileName, crime_officer_down.Anofficerdownconditionunknown.FileName },"we have an officer down, condition unknown"),
            },
            SecondaryAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { dispatch_respond_code.AllunitsrespondCode99.FileName },"all units repond code-99"),
                new AudioSet(new List<string>() { dispatch_respond_code.AllunitsrespondCode99emergency.FileName },"all units repond code-99 emergency"),
                new AudioSet(new List<string>() { dispatch_respond_code.Code99allunitsrespond.FileName },"code-99 all units repond"),
                new AudioSet(new List<string>() { custom_wanted_level_line.Code99allavailableunitsconvergeonsuspect.FileName },"code-99 all available units converge on suspect"),
                new AudioSet(new List<string>() { custom_wanted_level_line.Wehavea1099allavailableunitsrespond.FileName },"we have a 10-99  all available units repond"),
                new AudioSet(new List<string>() { dispatch_respond_code.Code99allunitsrespond.FileName },"code-99 all units respond"),
                new AudioSet(new List<string>() { dispatch_respond_code.EmergencyallunitsrespondCode99.FileName },"emergency all units respond code-99"),
                new AudioSet(new List<string>() { escort_boss.Immediateassistancerequired.FileName },"immediate assistance required"),
            },
        };
        ShotsFiredAtAnOfficer = new Dispatch()
        {
            Name = "Shots Fired at an Officer",
            IncludeAttentionAllUnits = true,
            ResultsInLethalForce = true,
            LocationDescription = LocationSpecificity.Street,
            CanBeReportedMultipleTimes = false,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_shots_fired_at_an_officer.Shotsfiredatanofficer.FileName },"shots fired at an officer"),
                new AudioSet(new List<string>() { crime_shots_fired_at_officer.Afirearmattackonanofficer.FileName },"a firearm attack on an officer"),
              //  new AudioSet(new List<string>() { crime_shots_fired_at_officer.Anofficershot.FileName },"an officer shot"),
                new AudioSet(new List<string>() { crime_shots_fired_at_officer.Anofficerunderfire.FileName },"a officer under fire"),
                new AudioSet(new List<string>() { crime_shots_fired_at_officer.Shotsfiredatanofficer.FileName },"a shots fired at an officer"),
            },
            SecondaryAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { dispatch_respond_code.AllunitsrespondCode99.FileName },"all units repond code-99"),
                new AudioSet(new List<string>() { dispatch_respond_code.AllunitsrespondCode99emergency.FileName },"all units repond code-99 emergency"),
                new AudioSet(new List<string>() { dispatch_respond_code.Code99allunitsrespond.FileName },"code-99 all units repond"),
                new AudioSet(new List<string>() { custom_wanted_level_line.Code99allavailableunitsconvergeonsuspect.FileName },"code-99 all available units converge on suspect"),
                new AudioSet(new List<string>() { custom_wanted_level_line.Wehavea1099allavailableunitsrespond.FileName },"we have a 10-99  all available units repond"),
                new AudioSet(new List<string>() { dispatch_respond_code.Code99allunitsrespond.FileName },"code-99 all units respond"),
                new AudioSet(new List<string>() { dispatch_respond_code.EmergencyallunitsrespondCode99.FileName },"emergency all units respond code-99"),
                new AudioSet(new List<string>() { escort_boss.Immediateassistancerequired.FileName },"immediate assistance required"),
            }
        };
        AssaultingOfficer = new Dispatch()
        {
            Name = "Assault on an Officer",
            LocationDescription = LocationSpecificity.Street,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_assault_on_an_officer.Anassaultonanofficer.FileName },"an assault on an officer"),
                new AudioSet(new List<string>() { crime_assault_on_an_officer.Anofficerassault.FileName },"an officer assault"),
            },
        };
        ThreateningOfficerWithFirearm = new Dispatch()
        {
            Name = "Threatening an Officer with a Firearm",
            LocationDescription = LocationSpecificity.StreetAndZone,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_suspect_threatening_an_officer_with_a_firearm.Asuspectthreateninganofficerwithafirearm.FileName },"a suspect threatening an officer with a firearm"),
            },
        };
        TrespassingOnGovernmentProperty = new Dispatch()
        {
            Name = "Trespassing on Government Property",
            ResultsInLethalForce = true,
            CanBeReportedMultipleTimes = false,
            LocationDescription = LocationSpecificity.Zone,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_trespassing_on_government_property.Trespassingongovernmentproperty.FileName },"trespassing on government property"),
            },
        };
        StealingAirVehicle = new Dispatch()
        {
            Name = "Stolen Air Vehicle",
            ResultsInLethalForce = true,
            IncludeDrivingVehicle = true,
            MarkVehicleAsStolen = true,
            LocationDescription = LocationSpecificity.Zone,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_stolen_aircraft.Astolenaircraft.FileName},"a stolen aircraft"),
                new AudioSet(new List<string>() { crime_hijacked_aircraft.Ahijackedaircraft.FileName },"a hijacked aircraft"),
                new AudioSet(new List<string>() { crime_theft_of_an_aircraft.Theftofanaircraft.FileName },"theft of an aircraft"),
            },
        };
        ShotsFired = new Dispatch()
        {
            Name = "Shots Fired",
            LocationDescription = LocationSpecificity.StreetAndZone,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_shooting.Afirearmssituationseveralshotsfired.FileName },"a firearms situation, several shots fired"),
                new AudioSet(new List<string>() { crime_shooting.Aweaponsincidentshotsfired.FileName },"a weapons incdient, shots fired"),
                new AudioSet(new List<string>() { crime_shoot_out.Ashootout.FileName },"a shoot-out"),
                new AudioSet(new List<string>() { crime_firearms_incident.AfirearmsincidentShotsfired.FileName },"a firearms incident, shots fired"),
                new AudioSet(new List<string>() { crime_firearms_incident.Anincidentinvolvingshotsfired.FileName },"an incident involving shots fired"),
                new AudioSet(new List<string>() { crime_firearms_incident.AweaponsincidentShotsfired.FileName },"a weapons incident, shots fired"),
            },
        };
        CarryingWeapon = new Dispatch()
        {
            Name = "Carrying Weapon",
            LocationDescription = LocationSpecificity.StreetAndZone,
            IncludeCarryingWeapon = true,
            CanBeReportedMultipleTimes = false,
        };
        CivilianDown = new Dispatch()
        {
            Name = "Civilian Down",
            LocationDescription = LocationSpecificity.StreetAndZone,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_civilian_fatality.Acivilianfatality.FileName },"civilian fatality"),
                new AudioSet(new List<string>() { crime_civilian_down.Aciviliandown.FileName },"civilian down"),

                new AudioSet(new List<string>() { crime_1_87.A187.FileName },"a 1-87"),
                new AudioSet(new List<string>() { crime_1_87.Ahomicide.FileName },"a homicide"),
            },
        };
        CivilianShot = new Dispatch()
        {
            Name = "Civilian Shot",
            LocationDescription = LocationSpecificity.Street,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_civillian_gsw.AcivilianGSW.FileName },"a civilian GSW"),
                new AudioSet(new List<string>() { crime_civillian_gsw.Acivilianshot.FileName },"a civilian shot"),
                new AudioSet(new List<string>() { crime_civillian_gsw.Agunshotwound.FileName },"a gunshot wound"),
            },
        };
        CivilianInjury = new Dispatch()
        {
            Name = "Civilian Injury",
            LocationDescription = LocationSpecificity.StreetAndZone,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_injured_civilian.Aninjuredcivilian.FileName },"an injured civilian"),
                new AudioSet(new List<string>() { crime_civilian_needing_assistance.Acivilianinneedofassistance.FileName },"a civilian in need of assistance"),
                new AudioSet(new List<string>() { crime_civilian_needing_assistance.Acivilianrequiringassistance.FileName },"a civilian requiring assistance"),
                new AudioSet(new List<string>() { crime_assault_on_a_civilian.Anassaultonacivilian.FileName },"an assault on a civilian"),
            },
        };
        GrandTheftAuto = new Dispatch()
        {
            Name = "Grand Theft Auto",
            IncludeDrivingVehicle = true,
            MarkVehicleAsStolen = true,
            IncludeLicensePlate = true,
            IncludeCarryingWeapon = true,
            LocationDescription = LocationSpecificity.HeadingAndStreet,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_grand_theft_auto.Agrandtheftauto.FileName },"a grand theft auto"),
                new AudioSet(new List<string>() { crime_grand_theft_auto.Agrandtheftautoinprogress.FileName },"a grand theft auto in progress"),
                new AudioSet(new List<string>() { crime_grand_theft_auto.AGTAinprogress.FileName },"a GTA in progress"),
                new AudioSet(new List<string>() { crime_grand_theft_auto.AGTAinprogress1.FileName },"a GTA in progress"),
            },
        };
        SuspiciousActivity = new Dispatch()
        {
            Name = "Suspicious Activity",
            LocationDescription = LocationSpecificity.StreetAndZone,
            IncludeCarryingWeapon = true,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_suspicious_activity.Suspiciousactivity.FileName },"suspicious activity"),
                new AudioSet(new List<string>() { crime_9_25.Asuspiciousperson.FileName },"a suspicious person"),
            },
        };
        TamperingWithVehicle = new Dispatch()
        {
            Name = "Tampering With Vehicle",
            LocationDescription = LocationSpecificity.StreetAndZone,
            IncludeCarryingWeapon = true,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_5_04.Tamperingwithavehicle.FileName },"tampering with a vehicle"),
            },
        };
        CriminalActivity = new Dispatch()
        {
            Name = "Criminal Activity",
            LocationDescription = LocationSpecificity.Street,
            IncludeCarryingWeapon = true,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_criminal_activity.Criminalactivity.FileName },"criminal activity"),
                new AudioSet(new List<string>() { crime_criminal_activity.Illegalactivity.FileName },"illegal activity"),
                new AudioSet(new List<string>() { crime_criminal_activity.Prohibitedactivity.FileName },"prohibited activity"),
            },
        };
        Mugging = new Dispatch()
        {
            Name = "Mugging",
            LocationDescription = LocationSpecificity.Street,
            IncludeCarryingWeapon = true,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_mugging.Apossiblemugging.FileName },"a possible mugging"),
            },
        };
        TerroristActivity = new Dispatch()
        {
            Name = "Terrorist Activity",
            LocationDescription = LocationSpecificity.Street,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_terrorist_activity.Possibleterroristactivity.FileName },"possible terrorist activity in progress"),
                new AudioSet(new List<string>() { crime_terrorist_activity.Possibleterroristactivity1.FileName },"possible terrorist activity in progress"),
                new AudioSet(new List<string>() { crime_terrorist_activity.Possibleterroristactivity2.FileName },"possible terrorist activity in progress"),
                new AudioSet(new List<string>() { crime_terrorist_activity.Terroristactivity.FileName },"terrorist activity"),
            },
        };

        ArmedRobbery = new Dispatch()
        {
            Name = "Armed Robbery",
            LocationDescription = LocationSpecificity.Street,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_robbery.Apossiblerobbery.FileName },"a possible robbery"),
                new AudioSet(new List<string>() { crime_2_11.Anarmedrobbery.FileName },"an armed robbery"),
                new AudioSet(new List<string>() { crime_robbery_with_a_firearm.Arobberywithafirearm.FileName },"a robbery with a firearm"),
                new AudioSet(new List<string>() { crime_hold_up.Aholdup.FileName },"a hold up"),
            },
        };

        PublicNuisance = new Dispatch()
        {
            Name = "Public Nuisance",
            LocationDescription = LocationSpecificity.Street,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_5_07.Apublicnuisance.FileName },"a public nuisance"),
            },
        };

        PublicVagrancy = new Dispatch()
        {
            Name = "Public Vagrancy",
            LocationDescription = LocationSpecificity.Street,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_drug_overdose.An11357PossibleOD.FileName },"a possible OD"),
                new AudioSet(new List<string>() { crime_unconscious_civilian.Anunconsciouscivilian.FileName },"an unconscious civilian"),
            },
        };


        SuspiciousVehicle = new Dispatch()
        {
            Name = "Suspicious Vehicle",
            IncludeDrivingVehicle = true,
            LocationDescription = LocationSpecificity.StreetAndZone,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_suspicious_vehicle.Asuspiciousvehicle.FileName },"a suspicious vehicle"),
            },
        };
        DrivingAtStolenVehicle = new Dispatch()
        {
            Name = "Driving a Stolen Vehicle",
            IncludeDrivingVehicle = true,
            LocationDescription = LocationSpecificity.HeadingAndStreet,
            IncludeDrivingSpeed = true,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_person_in_a_stolen_car.Apersoninastolencar.FileName},"a person in a stolen car"),
                new AudioSet(new List<string>() { crime_person_in_a_stolen_vehicle.Apersoninastolenvehicle.FileName },"a person in a stolen vehicle"),
            },
        };
        ResistingArrest = new Dispatch()
        {
            Name = "Resisting Arrest",
            LocationDescription = LocationSpecificity.Zone,
            IncludeCarryingWeapon = true,
            CanBeReportedMultipleTimes = false,
            CanAlwaysBeInterrupted = true,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_person_resisting_arrest.Apersonresistingarrest.FileName },"a person resisting arrest"),
                new AudioSet(new List<string>() { crime_suspect_resisting_arrest.Asuspectresistingarrest.FileName },"a suspect resisiting arrest"),

                new AudioSet(new List<string>() { crime_1_48_resist_arrest.Acriminalresistingarrest.FileName },"a criminal resisiting arrest"),
                new AudioSet(new List<string>() { crime_1_48_resist_arrest.Acriminalresistingarrest1.FileName },"a criminal resisiting arrest"),
                new AudioSet(new List<string>() { crime_1_48_resist_arrest.Asuspectfleeingacrimescene.FileName },"a suspect fleeing a crime scene"),
                new AudioSet(new List<string>() { crime_1_48_resist_arrest.Asuspectontherun.FileName },"a suspect on the run"),
            }
        };
        AttemptingSuicide = new Dispatch()
        {
            Name = "Suicide Attempt",
            LocationDescription = LocationSpecificity.Street,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_9_14a_attempted_suicide.Apossibleattemptedsuicide.FileName },"a possible attempted suicide"),
                new AudioSet(new List<string>() { crime_9_14a_attempted_suicide.Anattemptedsuicide.FileName },"an attempted suicide")
            }
        };
        FelonySpeeding = new Dispatch()
        {
            Name = "Felony Speeding",
            IncludeDrivingVehicle = true,
            VehicleIncludesIn = true,
            IncludeDrivingSpeed = true,
            LocationDescription = LocationSpecificity.Street,
            CanAlwaysBeInterrupted = true,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_speeding_felony.Aspeedingfelony.FileName },"a speeding felony"),
                new AudioSet(new List<string>() { crime_5_10.A510.FileName,crime_5_10.Speedingvehicle.FileName },"a 5-10, speeding vehicle"),
            },
        };
        PedHitAndRun = new Dispatch()
        {
            Name = "Pedestrian Hit-and-Run",
            LocationDescription = LocationSpecificity.Street,
            CanAlwaysBeInterrupted = true,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_ped_struck_by_veh.Apedestrianstruck.FileName},"a pedestrian struck"),
                new AudioSet(new List<string>() { crime_ped_struck_by_veh.Apedestrianstruck1.FileName },"a pedestrian struck"),
                new AudioSet(new List<string>() { crime_ped_struck_by_veh.Apedestrianstruckbyavehicle.FileName },"a pedestrian struck by a vehicle"),
                new AudioSet(new List<string>() { crime_ped_struck_by_veh.Apedestrianstruckbyavehicle1.FileName },"a pedestrian struck by a vehicle"),
            },
        };
        VehicleHitAndRun = new Dispatch()
        {
            Name = "Motor Vehicle Accident",
            LocationDescription = LocationSpecificity.Street,
            CanAlwaysBeInterrupted = true,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_motor_vehicle_accident.Amotorvehicleaccident.FileName},"a motor vehicle accident"),
                new AudioSet(new List<string>() { crime_motor_vehicle_accident.AnAEincident.FileName },"an A&E incident"),
                new AudioSet(new List<string>() { crime_motor_vehicle_accident.AseriousMVA.FileName },"a serious MVA"),
            },
        };
        RunningARedLight = new Dispatch()
        {
            Name = "Running a Red Light",
            LocationDescription = LocationSpecificity.Street,
            CanAlwaysBeInterrupted = true,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_person_running_a_red_light.Apersonrunningaredlight.FileName},"a person running a red light"),
            },
        };
        RecklessDriving = new Dispatch()
        {
            Name = "Reckless Driving",
            LocationDescription = LocationSpecificity.Street,
            CanAlwaysBeInterrupted = true,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_reckless_driver.Arecklessdriver.FileName},"a reckless driver"),
                new AudioSet(new List<string>() { crime_5_05.A505.FileName,crime_5_05.Adriveroutofcontrol.FileName },"a 505, a driver out of control"),
            },
        };
        DrunkDriving = new Dispatch()
        {
            Name = "Drunk Driving",
            LocationDescription = LocationSpecificity.Street,
            CanAlwaysBeInterrupted = true,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_5_02.Adriverundertheinfluence.FileName},"a driver under the influence"),
                new AudioSet(new List<string>() { crime_5_02.Adriverundertheinfluence1.FileName},"a driver under the influence"),
                new AudioSet(new List<string>() { crime_5_02.ADUI.FileName},"a dui"),
                new AudioSet(new List<string>() { crime_5_02.A502DUI.FileName},"a 502 dui"),
            },
        };
        AssaultingCivilians = new Dispatch()
        {
            Name = "Assault on a Civilian",
            LocationDescription = LocationSpecificity.Street,
            CanAlwaysBeInterrupted = true,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_assault.Apossibleassault.FileName},"a possible assault"),
                new AudioSet(new List<string>() { crime_assault.Apossibleassault1.FileName},"a possible assault"),
                new AudioSet(new List<string>() { crime_assault_on_a_civilian.Anassaultonacivilian.FileName},"an assault on a civilian"),
                new AudioSet(new List<string>() { crime_assault_and_battery.AnAE.FileName},"an A&B"),
                new AudioSet(new List<string>() { crime_assault_and_battery.Anassaultandbattery.FileName},"an assault and battery"),
            },
        };
        AimingWeaponAtPolice = new Dispatch()
        {
            Name = "Threatening Officer With Firearm",
            LocationDescription = LocationSpecificity.Street,
            CanAlwaysBeInterrupted = true,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_suspect_threatening_an_officer_with_a_firearm.Asuspectthreateninganofficerwithafirearm.FileName},"a suspect threatening and officer with a firearm"),
                new AudioSet(new List<string>() { crime_officer_in_danger.Anofficerindanger.FileName},"an officer in danger"),
            },
        };
        DealingDrugs = new Dispatch()
        {
            Name = "Dealing Drugs",
            LocationDescription = LocationSpecificity.Street,
            CanAlwaysBeInterrupted = true,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_drug_deal.Adrugdeal.FileName},"a drug deal"),
                new AudioSet(new List<string>() { crime_drug_deal.Adrugdealinprogress.FileName},"a drug deal in progress"),
                new AudioSet(new List<string>() { crime_drug_deal.Apossibledrugdeal.FileName},"a possible drug deal"),
                new AudioSet(new List<string>() { crime_drug_deal.Narcoticstrafficking.FileName},"narcotics trafficing"),
            },
        };


        //crime_road_blockade

        DealingGuns = new Dispatch()
        {
            Name = "Illegal Weapons Dealing",
            LocationDescription = LocationSpecificity.Street,
            CanAlwaysBeInterrupted = true,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_firearms_possession.Afirearmspossession.FileName},"a firearms possession"),
            },
        };

        AssaultingCiviliansWithDeadlyWeapon = new Dispatch()
        {
            Name = "Assault With a Deadly Weapon",
            LocationDescription = LocationSpecificity.Street,
            CanAlwaysBeInterrupted = true,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_assault_with_a_deadly_weapon.Assaultwithadeadlyweapon.FileName},"an assault with a deadly weapon"),
                new AudioSet(new List<string>() { crime_assault_with_a_deadly_weapon.AnADW.FileName},"an ADW"),
            },
        };
        Kidnapping = new Dispatch()
        {
            Name = "Kidnapping",
            LocationDescription = LocationSpecificity.Street,
            CanAlwaysBeInterrupted = true,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_2_07.Akidnapping.FileName},"a kidnapping"),
                new AudioSet(new List<string>() { crime_2_07.Akidnapping1.FileName},"a kidnapping"),
            },
        };
        PublicIntoxication = new Dispatch()
        {
            Name = "Public Intoxication",
            LocationDescription = LocationSpecificity.Street,
            CanAlwaysBeInterrupted = true,

            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_3_90.Publicintoxication.FileName},"public intoxication"),
            },
        };
        OfficerNeedsAssistance = new Dispatch()
        {
            Name = "Officer Needs Assistance",
            LocationDescription = LocationSpecificity.Street,
            CanAlwaysBeInterrupted = true,

            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_officer_in_need_of_assistance.Anofficerinneedofassistance.FileName},"an officer in need of assistance"),
                 new AudioSet(new List<string>() { crime_officer_in_need_of_assistance.Anofficerrequiringassistance.FileName},"an officer requiring assistance"),
            },
        };
        Harassment = new Dispatch()
        {
            Name = "Harassment",
            LocationDescription = LocationSpecificity.Street,
            CanAlwaysBeInterrupted = true,

            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_5_07.Apublicnuisance.FileName},"a public nuisance"),
                new AudioSet(new List<string>() { crime_disturbance.Apossibledisturbance.FileName},"a possible disturbance"),
                new AudioSet(new List<string>() { crime_disturbance.Adisturbance.FileName},"a disturbance"),
                new AudioSet(new List<string>() { crime_disturbance.Adisturbance1.FileName},"a disturbance"),
            },
        };
        OfficersNeeded = new Dispatch()
        {
            Name = "Officers Needed",
            LocationDescription = LocationSpecificity.Zone,
            CanAlwaysBeInterrupted = true,

            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { assistance_required.Officersneeded.FileName},"officers needed"),
                 new AudioSet(new List<string>() { assistance_required.Officersrequired.FileName},"officers required"),
            },
        };
        AnnounceStolenVehicle = new Dispatch()
        {
            Name = "Stolen Vehicle Reported",

            IncludeDrivingVehicle = true,
            CanAlwaysBeInterrupted = true,
            MarkVehicleAsStolen = true,
            IncludeLicensePlate = true,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() {crime_stolen_vehicle.Apossiblestolenvehicle.FileName},"a possible stolen vehicle"),
            },
        };
        RequestAirSupport = new Dispatch()
        {
            Name = "Air Support Requested",
            IsStatus = true,
            IncludeReportedBy = false,
            LocationDescription = LocationSpecificity.Zone,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { officer_requests_air_support.Officersrequestinghelicoptersupport.FileName },"officers requesting helicopter support"),
                new AudioSet(new List<string>() { officer_requests_air_support.Code99unitsrequestimmediateairsupport.FileName },"code-99 units request immediate air support"),
                new AudioSet(new List<string>() { officer_requests_air_support.Officersrequireaerialsupport.FileName },"officers require aerial support"),
                new AudioSet(new List<string>() { officer_requests_air_support.Officersrequireaerialsupport1.FileName },"officers require aerial support"),
                new AudioSet(new List<string>() { officer_requests_air_support.Officersrequireairsupport.FileName },"officers require air support"),
                new AudioSet(new List<string>() { officer_requests_air_support.Unitsrequestaerialsupport.FileName },"units request aerial support"),
                new AudioSet(new List<string>() { officer_requests_air_support.Unitsrequestingairsupport.FileName },"units requesting air support"),
                new AudioSet(new List<string>() { officer_requests_air_support.Unitsrequestinghelicoptersupport.FileName },"units requesting helicopter support"),
            },
        };

        RequestMilitaryUnits = new Dispatch()
        {
            IncludeAttentionAllUnits = true,
            Name = "Military Units Requested",
            IsStatus = true,
            IncludeReportedBy = false,
            LocationDescription = LocationSpecificity.Nothing,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { custom_wanted_level_line.Code13militaryunitsrequested.FileName },"code-13 military units requested"),
            },

            SecondaryAudioSet = new List<AudioSet>()
                {
                    new AudioSet(new List<string>() {dispatch_units_full.DispatchingunitsfromKazanskyAirForceBase.FileName },"dispatching units from Kazansky Air Force Base"),
                    new AudioSet(new List<string>() {dispatch_units_full.ScramblingmilitaryaircraftfromKazanskyAirForceBase.FileName },"scrambling military aircraft from Kazansky Air Force Base"),
                }
        };
        RequestNOOSEUnits = new Dispatch()
        {
            IncludeAttentionAllUnits = true,
            Name = "NOOSE Units Requested",
            IsStatus = true,
            IncludeReportedBy = false,
            LocationDescription = LocationSpecificity.Zone,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { dispatch_units_full.DispatchingSWATunitsfrompoliceheadquarters.FileName },"dispatching swat units from police headquarters"),
                new AudioSet(new List<string>() { dispatch_units_full.DispatchingSWATunitsfrompoliceheadquarters1.FileName },"dispatching swat units from police headquarters"),
            },
        };

        RequestNooseUnitsAlt = new Dispatch()
        {
            IncludeAttentionAllUnits = false,
            Name = "NOOSE Units Requested",
            IsStatus = true,
            IncludeReportedBy = false,
            CanAddExtras = false,
            LocationDescription = LocationSpecificity.Nothing,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { SWAT3.emergencytraffic.FileName, SWAT3.respondcode3.FileName},"dispatching NOOSE units"),
                new AudioSet(new List<string>() { SWAT3.emergencytraffic.FileName, SWAT3.requestingcode1alphain30minutes.FileName },"dispatching NOOSE units"),
            },
        };

        RequestNooseUnitsAlt2 = new Dispatch()
        {
            IncludeAttentionAllUnits = false,
            Name = "NOOSE Units Requested",
            IsStatus = true,
            IncludeReportedBy = false,
            LocationDescription = LocationSpecificity.Nothing,
            CanAddExtras = false,
            MainAudioSet = new List<AudioSet>()
            {
               // new AudioSet(new List<string>() { SWAT3.swat10minuteeta.FileName,SWAT3.suspectarmedusecaution.FileName},"dispatching swat units"),
               new AudioSet(new List<string>() { SWAT3.multipleswatunitsresponding.FileName,SWAT3.suspectarmedusecaution.FileName},"dispatching NOOSE units"),
                new AudioSet(new List<string>() { SWAT3.multipleswatunitsresponding.FileName, SWAT3.suspectsarmedwithheavyweaponsandbodyarmor.FileName },"dispatching NOOSE units"),
                new AudioSet(new List<string>() { SWAT3.swat10minuteeta.FileName },"dispatching NOOSE units"),
                new AudioSet(new List<string>() { SWAT3.calloutpending.FileName },"dispatching NOOSE units"),
            },
        };

        RequestFIBUnits = new Dispatch()
        {
            IncludeAttentionAllUnits = false,
            Name = "FIB-HRT Units Requested",
            IsStatus = true,
            IncludeReportedBy = false,
            CanAddExtras = false,
            LocationDescription = LocationSpecificity.Nothing,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { dispatch_units_full.FIBteamdispatchingfromstation.FileName},"dispatching FIB-HRT units"),
            },
        };


        RequestSwatAirSupport = new Dispatch()
        {
            Name = "Air Support Requested",
            IsStatus = true,
            IncludeReportedBy = false,
            LocationDescription = LocationSpecificity.Nothing,
            CanAddExtras = false,
            PreambleAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { SWAT3.emergencytraffic.FileName, AudioBeeps.Radio_End_1.FileName, AudioBeeps.Radio_Start_1.FileName,SWAT3.tendavidgoahead.FileName },"emergency traffic"),
               new AudioSet(new List<string>() { SWAT3.copyemergencytraffic.FileName, AudioBeeps.Radio_End_1.FileName, AudioBeeps.Radio_Start_1.FileName,SWAT3.tendavidgo.FileName },"emergency traffic"),
             },
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { SWAT3.airsupportimmediateinsertion.FileName,SWAT3.respondcode3.FileName },"officers requesting helicopter support"),
            },
        };





        ShotsFiredStatus = new Dispatch()
        {
            Name = "Shots Fired",
            IsStatus = true,
            IncludeReportedBy = true,
            CanBeReportedMultipleTimes = true,
            CanAddExtras = false,
            LocationDescription = LocationSpecificity.Zone,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_shots_fired_at_an_officer.Shotsfiredatanofficer.FileName },"shots fired at an officer"),
                new AudioSet(new List<string>() { crime_shots_fired_at_officer.Afirearmattackonanofficer.FileName },"a firearm attack on an officer"),
                new AudioSet(new List<string>() { crime_shots_fired_at_officer.Anofficerunderfire.FileName },"a officer under fire"),
                new AudioSet(new List<string>() { crime_shots_fired_at_officer.Shotsfiredatanofficer.FileName },"a shots fired at an officer"),


                new AudioSet(new List<string>() { crime_shooting.Afirearmssituationseveralshotsfired.FileName },"a shots fired at an officer"),
                new AudioSet(new List<string>() { crime_shooting.Aweaponsincidentshotsfired.FileName },"a shots fired at an officer"),

                new AudioSet(new List<string>() { crime_shoot_out.Ashootout.FileName },"a shots fired at an officer"),



            },
            //    SecondaryAudioSet = new List<AudioSet>()
            //{
            //    new AudioSet(new List<string>() { dispatch_respond_code.AllunitsrespondCode99.FileName },"all units repond code-99"),
            //    new AudioSet(new List<string>() { dispatch_respond_code.AllunitsrespondCode99emergency.FileName },"all units repond code-99 emergency"),
            //    new AudioSet(new List<string>() { dispatch_respond_code.Code99allunitsrespond.FileName },"code-99 all units repond"),
            //    new AudioSet(new List<string>() { custom_wanted_level_line.Code99allavailableunitsconvergeonsuspect.FileName },"code-99 all available units converge on suspect"),
            //    new AudioSet(new List<string>() { custom_wanted_level_line.Wehavea1099allavailableunitsrespond.FileName },"we have a 10-99  all available units repond"),
            //    new AudioSet(new List<string>() { dispatch_respond_code.Code99allunitsrespond.FileName },"code-99 all units respond"),
            //    new AudioSet(new List<string>() { dispatch_respond_code.EmergencyallunitsrespondCode99.FileName },"emergency all units respond code-99"),
            //    new AudioSet(new List<string>() { escort_boss.Immediateassistancerequired.FileName },"immediate assistance required"),
            //}
        };






        SuspectSpotted = new Dispatch()
        {
            Name = "Suspect Spotted",
            IsStatus = true,
            IncludeReportedBy = false,
            LocationDescription = LocationSpecificity.HeadingAndStreet,
            IncludeDrivingVehicle = true,
            CanAlwaysInterrupt = true,
            CanAlwaysBeInterrupted = true,

            PreambleAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { spot_suspect_cop_01.WeHaveAVisual1.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_01.WeHaveAVisual2.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_01.WeHaveAVisual3.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_01.WeHaveAVisual4.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_01.WeHaveAVisual5.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_01.WeHaveAVisual6.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_01.WeHaveAVisual7.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_01.WeHaveAVisual8.FileName },"suspect spotted"),

                new AudioSet(new List<string>() { spot_suspect_cop_02.WeHaveAVisual1.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_02.WeHaveAVisual2.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_02.WeHaveAVisual3.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_02.WeHaveAVisual4.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_02.WeHaveAVisual5.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_02.WeHaveAVisual6.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_02.WeHaveAVisual7.FileName },"suspect spotted"),

                new AudioSet(new List<string>() { spot_suspect_cop_03.WeHaveAVisual1.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_03.WeHaveAVisual2.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_03.WeHaveAVisual3.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_03.WeHaveAVisual4.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_03.WeHaveAVisual5.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_03.WeHaveAVisual6.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_03.WeHaveAVisual7.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_03.WeHaveAVisual8.FileName },"suspect spotted"),

                new AudioSet(new List<string>() { spot_suspect_cop_04.WeHaveAVisual1.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_04.WeHaveAVisual2.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_04.WeHaveAVisual3.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_04.WeHaveAVisual4.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_04.WeHaveAVisual5.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_04.WeHaveAVisual6.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_04.WeHaveAVisual7.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_04.WeHaveAVisual8.FileName },"suspect spotted"),

                new AudioSet(new List<string>() { spot_suspect_cop_05.WeHaveAVisual1.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_05.WeHaveAVisual2.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_05.WeHaveAVisual3.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_05.WeHaveAVisual4.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_05.WeHaveAVisual5.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_05.WeHaveAVisual6.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_05.WeHaveAVisual7.FileName },"suspect spotted"),
             },
        };
        WantedSuspectSpotted = new Dispatch()
        {
            Name = "Wanted Suspect Spotted",
            IsStatus = true,
            IncludeReportedBy = true,
            IncludeRapSheet = true,
            Priority = 10,
            LocationDescription = LocationSpecificity.HeadingAndStreet,
            IncludeDrivingVehicle = true,
            CanAlwaysInterrupt = true,
            PreambleAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { spot_suspect_cop_01.WeHaveAVisual1.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_01.WeHaveAVisual2.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_01.WeHaveAVisual3.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_01.WeHaveAVisual4.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_01.WeHaveAVisual5.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_01.WeHaveAVisual6.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_01.WeHaveAVisual7.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_01.WeHaveAVisual8.FileName },"suspect spotted"),

                new AudioSet(new List<string>() { spot_suspect_cop_02.WeHaveAVisual1.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_02.WeHaveAVisual2.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_02.WeHaveAVisual3.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_02.WeHaveAVisual4.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_02.WeHaveAVisual5.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_02.WeHaveAVisual6.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_02.WeHaveAVisual7.FileName },"suspect spotted"),

                new AudioSet(new List<string>() { spot_suspect_cop_03.WeHaveAVisual1.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_03.WeHaveAVisual2.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_03.WeHaveAVisual3.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_03.WeHaveAVisual4.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_03.WeHaveAVisual5.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_03.WeHaveAVisual6.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_03.WeHaveAVisual7.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_03.WeHaveAVisual8.FileName },"suspect spotted"),

                new AudioSet(new List<string>() { spot_suspect_cop_04.WeHaveAVisual1.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_04.WeHaveAVisual2.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_04.WeHaveAVisual3.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_04.WeHaveAVisual4.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_04.WeHaveAVisual5.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_04.WeHaveAVisual6.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_04.WeHaveAVisual7.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_04.WeHaveAVisual8.FileName },"suspect spotted"),

                new AudioSet(new List<string>() { spot_suspect_cop_05.WeHaveAVisual1.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_05.WeHaveAVisual2.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_05.WeHaveAVisual3.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_05.WeHaveAVisual4.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_05.WeHaveAVisual5.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_05.WeHaveAVisual6.FileName },"suspect spotted"),
                new AudioSet(new List<string>() { spot_suspect_cop_05.WeHaveAVisual7.FileName },"suspect spotted"),
             },
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_wanted_felon_on_the_loose.Awantedfelonontheloose.FileName },"a wanted felon on the loose"),
            },
        };
        OnFoot = new Dispatch()
        {
            Name = "On Foot",
            IsStatus = true,
            IncludeReportedBy = false,
            LocationDescription = LocationSpecificity.Nothing,
            IncludeDrivingVehicle = false,
            CanAlwaysBeInterrupted = true,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { suspect_is.SuspectIs.FileName, on_foot.Onfoot.FileName },"suspect is on foot"),
                new AudioSet(new List<string>() { suspect_is.SuspectIs.FileName, on_foot.Onfoot1.FileName },"suspect is on on foot"),

                new AudioSet(new List<string>() {s_f_y_cop_black_full_01.SuspectOnFoot2.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_f_y_cop_black_full_02.SuspectOnFoot2.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_f_y_cop_white_full_01.SuspectOnFoot2.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_f_y_cop_white_full_02.SuspectOnFoot2.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_cop_black_full_01.SuspectOnFoot2.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_cop_black_full_02.SuspectOnFoot2.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_cop_black_mini_01.SuspectOnFoot2.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_cop_black_mini_02.SuspectOnFoot2.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_cop_black_mini_03.SuspectOnFoot2.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_cop_black_mini_04.SuspectOnFoot2.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_cop_white_full_01.SuspectOnFoot2.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_cop_white_full_02.SuspectOnFoot2.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_cop_white_mini_01.SuspectOnFoot2.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_cop_white_mini_02.SuspectOnFoot2.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_cop_white_mini_03.SuspectOnFoot2.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_cop_white_mini_04.SuspectOnFoot2.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_black_full_01.SuspectOnFoot2.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_black_full_02.SuspectOnFoot2.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_white_full_01.SuspectOnFoot2.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_white_full_02.SuspectOnFoot2.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_sheriff_white_full_01.SuspectOnFoot2.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_sheriff_white_full_02.SuspectOnFoot2.FileName },"suspect is on on foot"),

                new AudioSet(new List<string>() {s_m_y_cop_black_full_02.SuspectOnFoot3.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_cop_black_mini_01.SuspectOnFoot3.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_cop_black_mini_02.SuspectOnFoot3.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_cop_black_mini_03.SuspectOnFoot3.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_cop_black_mini_04.SuspectOnFoot3.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_cop_white_full_01.SuspectOnFoot3.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_cop_white_full_02.SuspectOnFoot3.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_cop_white_mini_02.SuspectOnFoot3.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_cop_white_mini_03.SuspectOnFoot3.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_cop_white_mini_04.SuspectOnFoot3.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_black_full_01.SuspectOnFoot3.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_black_full_02.SuspectOnFoot3.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_white_full_01.SuspectOnFoot3.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_white_full_02.SuspectOnFoot3.FileName },"suspect is on on foot"),

                new AudioSet(new List<string>() {s_f_y_cop_black_full_01.SuspectOnFoot4.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_f_y_cop_black_full_02.SuspectOnFoot4.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_f_y_cop_white_full_01.SuspectOnFoot4.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_f_y_cop_white_full_02.SuspectOnFoot4.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_cop_black_full_01.SuspectOnFoot4.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_cop_white_full_01.SuspectOnFoot4.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_cop_white_full_02.SuspectOnFoot4.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_black_full_01.SuspectOnFoot4.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_black_full_02.SuspectOnFoot4.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_white_full_01.SuspectOnFoot4.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_white_full_02.SuspectOnFoot4.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_sheriff_white_full_01.SuspectOnFoot4.FileName },"suspect is on on foot"),
                new AudioSet(new List<string>() {s_m_y_sheriff_white_full_02.SuspectOnFoot4.FileName },"suspect is on on foot"),
            },
        };
        ExcessiveSpeed = new Dispatch()
        {
            Name = "Excessive Speed",
            IsStatus = true,
            IncludeReportedBy = false,
            IncludeDrivingVehicle = false,
            VehicleIncludesIn = true,
            IncludeDrivingSpeed = true,
            LocationDescription = LocationSpecificity.Street,
            CanAlwaysBeInterrupted = true,
            MainAudioSet = new List<AudioSet>()
            {
                //new AudioSet(new List<string>() { crime_speeding_felony.Aspeedingfelony.FileName },"a speeding felony"),
                //new AudioSet(new List<string>() { crime_5_10.A510.FileName,crime_5_10.Speedingvehicle.FileName },"a 5-10, speeding vehicle"),
            },
        };
        GotOnFreeway = new Dispatch()
        {
            Name = "Entered Freeway",
            IsStatus = true,
            IncludeReportedBy = false,
            LocationDescription = LocationSpecificity.Nothing,
            IncludeDrivingVehicle = false,
            CanAlwaysBeInterrupted = true,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { s_f_y_cop_black_full_01.SuspectEnteringTheFreeway.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_f_y_cop_black_full_02.SuspectEnteringTheFreeway.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_f_y_cop_white_full_01.SuspectEnteringTheFreeway.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_f_y_cop_white_full_02.SuspectEnteringTheFreeway.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_m_y_cop_black_full_01.SuspectEnteringTheFreeway.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_m_y_cop_black_full_02.SuspectEnteringTheFreeway.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_m_y_cop_black_mini_02.SuspectEnteringTheFreeway.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_m_y_cop_black_mini_03.SuspectEnteringTheFreeway.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_m_y_cop_black_mini_04.SuspectEnteringTheFreeway.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_m_y_cop_white_full_01.SuspectEnteringTheFreeway.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_m_y_cop_white_full_02.SuspectEnteringTheFreeway.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_m_y_cop_white_mini_01.SuspectEnteringTheFreeway.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_m_y_cop_white_mini_02.SuspectEnteringTheFreeway.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_m_y_cop_white_mini_03.SuspectEnteringTheFreeway.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_m_y_cop_white_mini_04.SuspectEnteringTheFreeway.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_m_y_hwaycop_black_full_01.SuspectEnteringTheFreeway.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_m_y_hwaycop_black_full_02.SuspectEnteringTheFreeway.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_m_y_hwaycop_white_full_01.SuspectEnteringTheFreeway.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_m_y_hwaycop_white_full_02.SuspectEnteringTheFreeway.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_m_y_sheriff_white_full_01.SuspectEnteringTheFreeway.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_m_y_sheriff_white_full_02.SuspectEnteringTheFreeway.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_f_y_cop_black_full_01.SuspectEnteringTheFreeway2.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_f_y_cop_black_full_02.SuspectEnteringTheFreeway2.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_f_y_cop_white_full_01.SuspectEnteringTheFreeway2.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_f_y_cop_white_full_02.SuspectEnteringTheFreeway2.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_m_y_cop_black_full_01.SuspectEnteringTheFreeway2.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_m_y_cop_black_full_02.SuspectEnteringTheFreeway2.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_m_y_cop_white_full_01.SuspectEnteringTheFreeway2.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_m_y_cop_white_full_02.SuspectEnteringTheFreeway2.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_m_y_hwaycop_black_full_01.SuspectEnteringTheFreeway2.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_m_y_hwaycop_black_full_02.SuspectEnteringTheFreeway2.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_m_y_hwaycop_white_full_01.SuspectEnteringTheFreeway2.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_m_y_hwaycop_white_full_02.SuspectEnteringTheFreeway2.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_m_y_sheriff_white_full_01.SuspectEnteringTheFreeway2.FileName },"suspect has entered the freeway"),
                new AudioSet(new List<string>() { s_m_y_sheriff_white_full_02.SuspectEnteringTheFreeway2.FileName },"suspect has entered the freeway"),
            },
        };
        GotOffFreeway = new Dispatch()
        {
            Name = "Exited Freeway",
            IsStatus = true,
            IncludeReportedBy = false,
            LocationDescription = LocationSpecificity.Nothing,
            IncludeDrivingVehicle = false,
            CanAlwaysBeInterrupted = true,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { s_f_y_cop_black_full_01.SuspectLeftFreeway.FileName},"suspect left the freeway"),
                new AudioSet(new List<string>() { s_f_y_cop_black_full_02.SuspectLeftFreeway.FileName},"suspect left the freeway"),
                new AudioSet(new List<string>() { s_f_y_cop_white_full_01.SuspectLeftFreeway.FileName},"suspect left the freeway"),
                new AudioSet(new List<string>() { s_f_y_cop_white_full_02.SuspectLeftFreeway.FileName},"suspect left the freeway"),
                new AudioSet(new List<string>() { s_m_y_cop_black_full_01.SuspectLeftFreeway.FileName},"suspect left the freeway"),
                new AudioSet(new List<string>() { s_m_y_cop_black_full_02.SuspectLeftFreeway.FileName},"suspect left the freeway"),
                new AudioSet(new List<string>() { s_m_y_cop_white_full_01.SuspectLeftFreeway.FileName},"suspect left the freeway"),
                new AudioSet(new List<string>() { s_m_y_cop_white_full_02.SuspectLeftFreeway.FileName},"suspect left the freeway"),
                new AudioSet(new List<string>() { s_m_y_hwaycop_black_full_01.SuspectLeftFreeway.FileName},"suspect left the freeway"),
                new AudioSet(new List<string>() { s_m_y_hwaycop_black_full_02.SuspectLeftFreeway.FileName},"suspect left the freeway"),
                new AudioSet(new List<string>() { s_m_y_hwaycop_white_full_01.SuspectLeftFreeway.FileName},"suspect left the freeway"),
                new AudioSet(new List<string>() { s_m_y_hwaycop_white_full_02.SuspectLeftFreeway.FileName},"suspect left the freeway"),
                new AudioSet(new List<string>() { s_m_y_sheriff_white_full_01.SuspectLeftFreeway.FileName},"suspect left the freeway"),
                new AudioSet(new List<string>() { s_m_y_sheriff_white_full_02.SuspectLeftFreeway.FileName},"suspect left the freeway"),
                new AudioSet(new List<string>() { s_f_y_cop_black_full_01.SuspectLeftFreeway2.FileName},"suspect left the freeway"),
                new AudioSet(new List<string>() { s_f_y_cop_black_full_02.SuspectLeftFreeway2.FileName},"suspect left the freeway"),
                new AudioSet(new List<string>() { s_f_y_cop_white_full_01.SuspectLeftFreeway2.FileName},"suspect left the freeway"),
                new AudioSet(new List<string>() { s_f_y_cop_white_full_02.SuspectLeftFreeway2.FileName},"suspect left the freeway"),
                new AudioSet(new List<string>() { s_m_y_cop_black_full_01.SuspectLeftFreeway2.FileName},"suspect left the freeway"),
                new AudioSet(new List<string>() { s_m_y_cop_black_full_02.SuspectLeftFreeway2.FileName},"suspect left the freeway"),
                new AudioSet(new List<string>() { s_m_y_cop_white_full_01.SuspectLeftFreeway2.FileName},"suspect left the freeway"),
                new AudioSet(new List<string>() { s_m_y_cop_white_full_02.SuspectLeftFreeway2.FileName},"suspect left the freeway"),
                new AudioSet(new List<string>() { s_m_y_hwaycop_black_full_01.SuspectLeftFreeway2.FileName},"suspect left the freeway"),
                new AudioSet(new List<string>() { s_m_y_hwaycop_black_full_02.SuspectLeftFreeway2.FileName},"suspect left the freeway"),
                new AudioSet(new List<string>() { s_m_y_hwaycop_white_full_01.SuspectLeftFreeway2.FileName},"suspect left the freeway"),
                new AudioSet(new List<string>() { s_m_y_hwaycop_white_full_02.SuspectLeftFreeway2.FileName},"suspect left the freeway"),
                new AudioSet(new List<string>() { s_m_y_sheriff_white_full_01.SuspectLeftFreeway2.FileName},"suspect left the freeway"),
                new AudioSet(new List<string>() { s_m_y_sheriff_white_full_02.SuspectLeftFreeway2.FileName},"suspect left the freeway"),
            },
        };

        MedicalServicesRequired = new Dispatch()
        {
            Name = "Medical Services Required",
            IncludeReportedBy = true,
            LocationDescription = LocationSpecificity.StreetAndZone,
            IncludeDrivingVehicle = false,
            CanAlwaysBeInterrupted = true,
            IsStatus = true,
            NotificationTitle = "Emergency Scanner",
            NotificationSubtitle = "~y~Injured Person Reported~s~",
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_medical_aid_requested.Medicalaidrequested.FileName},"medical aid requested"),
            },
        };

        FirefightingServicesRequired = new Dispatch()
        {
            Name = "Fire Fighting Services Required",
            IncludeReportedBy = true,
            LocationDescription = LocationSpecificity.StreetAndZone,
            IncludeDrivingVehicle = false,
            CanAlwaysBeInterrupted = true,
            IsStatus = true,
            NotificationTitle = "Emergency Scanner",
            NotificationSubtitle = "~y~Fire Reported~s~",
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { emergency.Apossiblefire.FileName},"a possible fire"),
            },
        };

        VehicleStartedFire = new Dispatch()
        {
            Name = "Vehicle On Fire",
            IsStatus = true,
            IncludeReportedBy = true,
            LocationDescription = LocationSpecificity.Nothing,
            IncludeDrivingVehicle = false,
            CanAlwaysBeInterrupted = true,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_vehicle_on_fire.Avehicleonfire.FileName},"a vehicle on fire"),
                new AudioSet(new List<string>() { crime_car_on_fire.Acarfire.FileName},"a vehicle on fire"),
                new AudioSet(new List<string>() { crime_car_on_fire.Acaronfire.FileName},"a vehicle on fire"),
                new AudioSet(new List<string>() { crime_car_on_fire.Anautomobileonfire.FileName},"a vehicle on fire"),
            },
        };
        VehicleCrashed = new Dispatch()
        {
            Name = "Vehicle Crashed",
            IsStatus = true,
            IncludeReportedBy = true,
            LocationDescription = LocationSpecificity.Nothing,
            IncludeDrivingVehicle = false,
            CanAlwaysBeInterrupted = true,
            PreambleAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { s_f_y_cop_black_full_01.SuspectCrashed.FileName},"suspect crashed"),
                new AudioSet(new List<string>() { s_f_y_cop_black_full_02.SuspectCrashed.FileName},"suspect crashed"),
                new AudioSet(new List<string>() { s_f_y_cop_white_full_01.SuspectCrashed.FileName},"suspect crashed"),
                new AudioSet(new List<string>() { s_f_y_cop_white_full_02.SuspectCrashed.FileName},"suspect crashed"),
                new AudioSet(new List<string>() { s_m_y_cop_black_full_01.SuspectCrashed.FileName},"suspect crashed"),
                new AudioSet(new List<string>() { s_m_y_cop_black_full_02.SuspectCrashed.FileName},"suspect crashed"),
                new AudioSet(new List<string>() { s_m_y_cop_white_full_01.SuspectCrashed.FileName},"suspect crashed"),
                new AudioSet(new List<string>() { s_m_y_cop_white_full_02.SuspectCrashed.FileName},"suspect crashed"),
                new AudioSet(new List<string>() { s_m_y_hwaycop_black_full_01.SuspectCrashed.FileName},"suspect crashed"),
                new AudioSet(new List<string>() { s_m_y_hwaycop_black_full_02.SuspectCrashed.FileName},"suspect crashed"),
                new AudioSet(new List<string>() { s_m_y_hwaycop_white_full_01.SuspectCrashed.FileName},"suspect crashed"),
                new AudioSet(new List<string>() { s_m_y_hwaycop_white_full_02.SuspectCrashed.FileName},"suspect crashed"),
                new AudioSet(new List<string>() { s_m_y_sheriff_white_full_01.SuspectCrashed.FileName},"suspect crashed"),
                new AudioSet(new List<string>() { s_m_y_sheriff_white_full_02.SuspectCrashed.FileName},"suspect crashed"),
            },
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crime_motor_vehicle_accident.Amotorvehicleaccident.FileName},"a motor vehicle accident"),
                new AudioSet(new List<string>() { crime_motor_vehicle_accident.AseriousMVA.FileName},"a motor vehicle accident"),
                new AudioSet(new List<string>() { crime_motor_vehicle_accident.AnAEincident.FileName},"a motor vehicle accident"),
            },
        };
        NoFurtherUnitsNeeded = new Dispatch()
        {
            Name = "Officers On-Site, Code 4-ADAM",
            IsStatus = true,
            IncludeReportedBy = false,
            CanAlwaysBeInterrupted = true,
            AnyDispatchInterrupts = true,

            NotificationTitle = "Emergency Scanner",
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { officers_on_scene.Officersareatthescene.FileName },"officers are at the scene"),
                new AudioSet(new List<string>() { officers_on_scene.Officersarrivedonscene.FileName },"offices have arrived on scene"),
                new AudioSet(new List<string>() { officers_on_scene.Officershavearrived.FileName },"officers have arrived"),
                new AudioSet(new List<string>() { officers_on_scene.Officersonscene.FileName },"officers on scene"),
                new AudioSet(new List<string>() { officers_on_scene.Officersonsite.FileName },"officers on site"),
            },
            SecondaryAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { no_further_units.Noadditionalofficersneeded.FileName },"no additional officers needed"),
                new AudioSet(new List<string>() { no_further_units.Noadditionalofficersneeded1.FileName },"no additional officers needed"),
                new AudioSet(new List<string>() { no_further_units.Nofurtherunitsrequired.FileName },"no further units required"),
                new AudioSet(new List<string>() { no_further_units.WereCode4Adam.FileName },"we're code-4 adam"),
                new AudioSet(new List<string>() { no_further_units.Code4Adamnoadditionalsupportneeded.FileName },"code-4 adam no additional support needed"),
            },
        };
        SuspectArrested = new Dispatch()
        {
            Name = "Suspect Apprehended",
            IsStatus = true,
            IncludeReportedBy = false,
            CanAlwaysInterrupt = true,
            AnyDispatchInterrupts = true,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crook_arrested.Officershaveapprehendedsuspect.FileName },"officers have apprehended suspect"),
                new AudioSet(new List<string>() { crook_arrested.Officershaveapprehendedsuspect1.FileName },"officers have apprehended suspect"),
            },
        };
        SuspectWasted = new Dispatch()
        {
            Name = "Suspect Neutralized",
            IsStatus = true,
            IncludeReportedBy = false,
            CanAlwaysInterrupt = true,
            AnyDispatchInterrupts = true,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { crook_killed.Criminaldown.FileName },"criminal down"),
                new AudioSet(new List<string>() { crook_killed.Suspectdown.FileName },"suspect down"),
                new AudioSet(new List<string>() { crook_killed.Suspectneutralized.FileName },"suspect neutralized"),
                new AudioSet(new List<string>() { crook_killed.Suspectdownmedicalexaminerenroute.FileName },"suspect down, medical examiner in route"),
                new AudioSet(new List<string>() { crook_killed.Suspectdowncoronerenroute.FileName },"suspect down, coroner in route"),
                new AudioSet(new List<string>() { crook_killed.Officershavepacifiedsuspect.FileName },"officers have pacified suspect"),
             },
        };
        ChangedVehicles = new Dispatch()
        {
            Name = "Suspect Changed Vehicle",
            IsStatus = true,
            IncludeDrivingVehicle = true,
            CanAlwaysInterrupt = true,
            LocationDescription = LocationSpecificity.StreetAndZone,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { "" },""),
             },
        };
        RequestBackup = new Dispatch()
        {
            IncludeAttentionAllUnits = true,
            Name = "Backup Required",
            IsStatus = true,
            IncludeReportedBy = false,
            CanAlwaysInterrupt = true,

            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { assistance_required.Assistanceneeded.FileName },"assistance needed"),
                new AudioSet(new List<string>() { assistance_required.Assistancerequired.FileName },"Assistance required"),
                new AudioSet(new List<string>() { assistance_required.Backupneeded.FileName },"backup needed"),
                new AudioSet(new List<string>() { assistance_required.Backuprequired.FileName },"backup required"),
                new AudioSet(new List<string>() { assistance_required.Officersneeded.FileName },"officers needed"),
                new AudioSet(new List<string>() { assistance_required.Officersrequired.FileName },"officers required"),
             },
            SecondaryAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { dispatch_respond_code.UnitsrespondCode3.FileName },"units respond code-3"),
             },
            PreambleAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() {s_f_y_cop_black_full_01.NeedBackup1.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_f_y_cop_black_full_02.NeedBackup1.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_f_y_cop_white_full_01.NeedBackup1.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_f_y_cop_white_full_02.NeedBackup1.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_black_full_01.NeedBackup1.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_black_full_02.NeedBackup1.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_white_full_01.NeedBackup1.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_white_full_02.NeedBackup1.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_black_full_01.NeedBackup1.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_black_full_02.NeedBackup1.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_white_full_01.NeedBackup1.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_white_full_02.NeedBackup1.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_sheriff_white_full_01.NeedBackup1.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_sheriff_white_full_02.NeedBackup1.FileName },"requesting backup"),

                new AudioSet(new List<string>() {s_f_y_cop_black_full_01.NeedBackup2.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_f_y_cop_black_full_02.NeedBackup2.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_f_y_cop_white_full_01.NeedBackup2.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_f_y_cop_white_full_02.NeedBackup2.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_black_full_01.NeedBackup2.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_black_full_02.NeedBackup2.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_white_full_01.NeedBackup2.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_white_full_02.NeedBackup2.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_black_full_01.NeedBackup2.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_black_full_02.NeedBackup2.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_white_full_01.NeedBackup2.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_white_full_02.NeedBackup2.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_sheriff_white_full_01.NeedBackup2.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_sheriff_white_full_02.NeedBackup2.FileName },"requesting backup"),

                new AudioSet(new List<string>() {s_f_y_cop_black_full_01.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_f_y_cop_black_full_02.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_f_y_cop_white_full_01.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_f_y_cop_white_full_02.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_black_full_01.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_black_full_02.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_black_mini_01.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_black_mini_02.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_black_mini_03.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_black_mini_04.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_white_full_01.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_white_full_02.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_white_mini_01.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_white_mini_02.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_white_mini_03.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_white_mini_04.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_black_full_01.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_black_full_02.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_white_full_01.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_white_full_02.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_sheriff_white_full_01.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_sheriff_white_full_02.NeedBackup3.FileName },"requesting backup"),
             },
        };
        RequestBackupSimple = new Dispatch()
        {
            IncludeAttentionAllUnits = false,
            Name = "Backup Required",
            IsStatus = true,
            IncludeReportedBy = false,
            CanAlwaysInterrupt = false,

            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() {s_f_y_cop_black_full_01.NeedBackup1.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_f_y_cop_black_full_02.NeedBackup1.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_f_y_cop_white_full_01.NeedBackup1.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_f_y_cop_white_full_02.NeedBackup1.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_black_full_01.NeedBackup1.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_black_full_02.NeedBackup1.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_white_full_01.NeedBackup1.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_white_full_02.NeedBackup1.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_black_full_01.NeedBackup1.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_black_full_02.NeedBackup1.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_white_full_01.NeedBackup1.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_white_full_02.NeedBackup1.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_sheriff_white_full_01.NeedBackup1.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_sheriff_white_full_02.NeedBackup1.FileName },"requesting backup"),

                new AudioSet(new List<string>() {s_f_y_cop_black_full_01.NeedBackup2.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_f_y_cop_black_full_02.NeedBackup2.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_f_y_cop_white_full_01.NeedBackup2.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_f_y_cop_white_full_02.NeedBackup2.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_black_full_01.NeedBackup2.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_black_full_02.NeedBackup2.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_white_full_01.NeedBackup2.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_white_full_02.NeedBackup2.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_black_full_01.NeedBackup2.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_black_full_02.NeedBackup2.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_white_full_01.NeedBackup2.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_white_full_02.NeedBackup2.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_sheriff_white_full_01.NeedBackup2.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_sheriff_white_full_02.NeedBackup2.FileName },"requesting backup"),

                new AudioSet(new List<string>() {s_f_y_cop_black_full_01.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_f_y_cop_black_full_02.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_f_y_cop_white_full_01.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_f_y_cop_white_full_02.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_black_full_01.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_black_full_02.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_black_mini_01.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_black_mini_02.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_black_mini_03.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_black_mini_04.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_white_full_01.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_white_full_02.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_white_mini_01.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_white_mini_02.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_white_mini_03.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_cop_white_mini_04.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_black_full_01.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_black_full_02.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_white_full_01.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_white_full_02.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_sheriff_white_full_01.NeedBackup3.FileName },"requesting backup"),
                new AudioSet(new List<string>() {s_m_y_sheriff_white_full_02.NeedBackup3.FileName },"requesting backup"),
             },
        };
        WeaponsFree = new Dispatch()
        {
            IncludeAttentionAllUnits = true,
            Name = "Weapons Free",
            IsStatus = true,
            IncludeReportedBy = false,
            CanAlwaysInterrupt = true,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { custom_wanted_level_line.Suspectisarmedanddangerousweaponsfree.FileName },"suspect is armed and dangerous, weapons free"),
             },
        };
        LethalForceAuthorized = new Dispatch()
        {
            IncludeAttentionAllUnits = true,
            Name = "Lethal Force Authorized",
            IsStatus = true,
            IncludeReportedBy = false,
            ResultsInLethalForce = true,
            CanAlwaysInterrupt = true,
        };
        //Status
        SuspectEvaded = new Dispatch()
        {
            Name = "Suspect Evaded",
            IsStatus = true,
            IncludeReportedBy = false,
            LocationDescription = LocationSpecificity.Zone,
            CanAlwaysInterrupt = false,
            CanAlwaysBeInterrupted = true,
            AnyDispatchInterrupts = true,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { suspect_eluded_pt_1.SuspectEvadedPursuingOfficiers.FileName },"suspect evaded pursuing officers"),
                new AudioSet(new List<string>() { suspect_eluded_pt_1.OfficiersHaveLostVisualOnSuspect.FileName },"officers have lost visual on suspect"),
            },
            PreambleAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() {s_m_y_cop_black_full_02.CantSeeSuspect1.FileName },"need location on suspect"),
                new AudioSet(new List<string>() {s_m_y_cop_black_mini_01.CantSeeSuspect1.FileName },"need location on suspect"),
                new AudioSet(new List<string>() {s_m_y_cop_black_mini_02.CantSeeSuspect1.FileName },"need location on suspect"),
                new AudioSet(new List<string>() {s_m_y_cop_black_mini_03.CantSeeSuspect1.FileName },"need location on suspect"),
                new AudioSet(new List<string>() {s_m_y_cop_black_mini_04.CantSeeSuspect1.FileName },"need location on suspect"),
                new AudioSet(new List<string>() {s_m_y_cop_white_full_01.CantSeeSuspect1.FileName },"need location on suspect"),
                new AudioSet(new List<string>() {s_m_y_cop_white_full_02.CantSeeSuspect1.FileName },"need location on suspect"),
                new AudioSet(new List<string>() {s_m_y_cop_white_mini_02.CantSeeSuspect1.FileName },"need location on suspect"),
                new AudioSet(new List<string>() {s_m_y_cop_white_mini_03.CantSeeSuspect1.FileName },"need location on suspect"),
                new AudioSet(new List<string>() {s_m_y_cop_white_mini_04.CantSeeSuspect1.FileName },"need location on suspect"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_black_full_01.CantSeeSuspect1.FileName },"need location on suspect"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_black_full_02.CantSeeSuspect1.FileName },"need location on suspect"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_white_full_01.CantSeeSuspect1.FileName },"need location on suspect"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_white_full_02.CantSeeSuspect1.FileName },"need location on suspect"),
             },
        };
        RemainInArea = new Dispatch()//runs when you lose wanted organicalls
        {
            Name = "Remain in Area",
            IsStatus = true,
            IncludeReportedBy = false,
            CanAlwaysInterrupt = true,
            CanAlwaysBeInterrupted = true,
            AnyDispatchInterrupts = true,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { suspect_eluded_pt_2.AllUnitsStayInTheArea.FileName },"all units stay in the area"),
                new AudioSet(new List<string>() { suspect_eluded_pt_2.AllUnitsRemainOnAlert.FileName },"all units remain on alert"),

              //  new AudioSet(new List<string>() { suspect_eluded_pt_2.AllUnitsStandby.FileName },"all units standby"),
                new AudioSet(new List<string>() { suspect_eluded_pt_2.AllUnitsStayInTheArea.FileName },"all units stay in the area"),
                new AudioSet(new List<string>() { suspect_eluded_pt_2.AllUnitsRemainOnAlert.FileName },"all units remain on alert"),
            },
        };
        AttemptToReacquireSuspect = new Dispatch()//is the status one
        {
            Name = "Attempt To Reacquire",
            IsStatus = true,
            IncludeReportedBy = false,
            LocationDescription = LocationSpecificity.Zone,
            CanAlwaysInterrupt = true,
            CanAlwaysBeInterrupted = true,
            AnyDispatchInterrupts = true,
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { attempt_to_find.AllunitsATonsuspects20.FileName },"all units ATL on suspects 20"),
                new AudioSet(new List<string>() { attempt_to_find.Allunitsattempttoreacquire.FileName },"all units attempt to reacquire"),
                new AudioSet(new List<string>() { attempt_to_find.Allunitsattempttoreacquirevisual.FileName },"all units attempt to reacquire visual"),
                new AudioSet(new List<string>() { attempt_to_find.RemainintheareaATL20onsuspect.FileName },"remain in the area, ATL-20 on suspect"),
                new AudioSet(new List<string>() { attempt_to_find.RemainintheareaATL20onsuspect1.FileName },"remain in the area, ATL-20 on suspect"),
            },
            PreambleAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() {s_m_y_cop_black_full_02.CantSeeSuspect1.FileName },"need location on suspect"),
                new AudioSet(new List<string>() {s_m_y_cop_black_mini_01.CantSeeSuspect1.FileName },"need location on suspect"),
                new AudioSet(new List<string>() {s_m_y_cop_black_mini_02.CantSeeSuspect1.FileName },"need location on suspect"),
                new AudioSet(new List<string>() {s_m_y_cop_black_mini_03.CantSeeSuspect1.FileName },"need location on suspect"),
                new AudioSet(new List<string>() {s_m_y_cop_black_mini_04.CantSeeSuspect1.FileName },"need location on suspect"),
                new AudioSet(new List<string>() {s_m_y_cop_white_full_01.CantSeeSuspect1.FileName },"need location on suspect"),
                new AudioSet(new List<string>() {s_m_y_cop_white_full_02.CantSeeSuspect1.FileName },"need location on suspect"),
                new AudioSet(new List<string>() {s_m_y_cop_white_mini_02.CantSeeSuspect1.FileName },"need location on suspect"),
                new AudioSet(new List<string>() {s_m_y_cop_white_mini_03.CantSeeSuspect1.FileName },"need location on suspect"),
                new AudioSet(new List<string>() {s_m_y_cop_white_mini_04.CantSeeSuspect1.FileName },"need location on suspect"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_black_full_01.CantSeeSuspect1.FileName },"need location on suspect"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_black_full_02.CantSeeSuspect1.FileName },"need location on suspect"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_white_full_01.CantSeeSuspect1.FileName },"need location on suspect"),
                new AudioSet(new List<string>() {s_m_y_hwaycop_white_full_02.CantSeeSuspect1.FileName },"need location on suspect"),
             },
        };
        ResumePatrol = new Dispatch()
        {
            Name = "Resume Patrol",
            IsStatus = true,
            IncludeReportedBy = false,
            CanAlwaysInterrupt = true,
            CanAlwaysBeInterrupted = true,
            AnyDispatchInterrupts = true,
            NotificationTitle = "Emergency Scanner",
            MainAudioSet = new List<AudioSet>()
            {
                new AudioSet(new List<string>() { officer_begin_patrol.Beginpatrol.FileName },"begin patrol"),
                new AudioSet(new List<string>() { officer_begin_patrol.Beginbeat.FileName },"begin beat"),

                new AudioSet(new List<string>() { officer_begin_patrol.Assigntopatrol.FileName },"assign to patrol"),
                new AudioSet(new List<string>() { officer_begin_patrol.Proceedtopatrolarea.FileName },"proceed to patrol area"),
                new AudioSet(new List<string>() { officer_begin_patrol.Proceedwithpatrol.FileName },"proceed with patrol"),
            },
            //SecondaryAudioSet = new List<AudioSet>()
            //{
            //    new AudioSet(new List<string>() { officer_begin_patrol.Beginpatrol.FileName },"begin patrol"),
            //}
        };
    }

}
