//using ExtensionsMethods;
//using LosSantosRED.lsr.Helper;
//using LosSantosRED.lsr.Interface;
//using LSR.Vehicles;
//using Rage;
//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Threading;


//public class DispatchReport//Has the current Info along with the crime or status
//{
//    private IDispatches Dispatches;
//    private IScannerPlayable ScannerPlayable;
//    private DispatchReportStaticData DispatchReportStaticData;//the general audio sets that make up this 
//    private ISettingsProvideable Settings;
//    private IEntityProvideable World;
//    private bool IncludeSpecificUnits => !DispatchReportInformation.SeenByOfficers && !DispatchReportStaticData.IsStatus;
//    public DispatchReport()
//    {
//    }
//    public DispatchReport(string dispatchID, IDispatches dispatches, DispatchReportInformation dispatchReportInformation, IScannerPlayable scannerPlayable, ISettingsProvideable settings, IEntityProvideable world)
//    {
//        DispatchID = dispatchID;
//        Dispatches = dispatches;
//        ScannerPlayable = scannerPlayable;
//        DispatchReportInformation = dispatchReportInformation;
//        Settings = settings;
//        World = world;
//    }
//    public string DispatchID { get; set; }
//    public DispatchReportInformation DispatchReportInformation { get; set; }//the most recent up to date info on the stuffo
//    public DispatchReportOutputEvent DispatchReportOutputEvent { get; set; }//the final audio list with all info hardcoded

//    public void Build()
//    {
//        DispatchReportOutputEvent = new DispatchReportOutputEvent();
//        DispatchReportStaticData = Dispatches.GetStaticData(DispatchID);
//        if (DispatchReportStaticData != null)
//        {
//            BuildPreamble();
//            BuildGeneral();
//            BuildAttention();
//            BuildReportBy();
//            BuildSets();
//            BuildDescriptions();
//            BuildLocations();
//            BuildPlayerWarning();
//            if (DispatchReportOutputEvent.SoundsToPlay.Count() == 1)//only has radio beep
//            {
//                return;
//            }
//            BuildUnitReply();
//            FinalizeReport();
//        }
//    }
//    private void BuildPreamble()
//    {
//        if (DispatchReportStaticData.HasPreamble)
//        {
//            DispatchReportOutputEvent.SoundsToPlay.Add(Dispatches.RadioStartBeep());
//            AddAudioSet(DispatchReportOutputEvent, DispatchReportStaticData.PreambleAudioSet.PickRandom());
//            DispatchReportOutputEvent.SoundsToPlay.Add(Dispatches.RadioEndBeep());
//        }
//    }
//    private void BuildGeneral()
//    {
//        DispatchReportOutputEvent.SoundsToPlay.Add(Dispatches.RadioStartBeep());
//        DispatchReportOutputEvent.NotificationTitle = DispatchReportStaticData.NotificationTitle;
//        if (DispatchReportStaticData.NotificationSubtitle != "")
//        {
//            DispatchReportOutputEvent.NotificationSubtitle = DispatchReportStaticData.NotificationSubtitle + "~s~";
//        }
//        else if (DispatchReportStaticData.IsStatus)
//        {
//            DispatchReportOutputEvent.NotificationSubtitle = "~g~Status~s~";
//        }
//        else if (DispatchReportInformation.SeenByOfficers)
//        {
//            DispatchReportOutputEvent.NotificationSubtitle = "~r~Crime Observed~s~";
//        }
//        else
//        {
//            DispatchReportOutputEvent.NotificationSubtitle = "~o~Crime Reported~s~";
//        }
//        DispatchReportOutputEvent.NotificationText = DispatchReportStaticData.NotificationText;
//    }
//    private void BuildAttention()
//    {
//        if (DispatchReportStaticData.IncludeAttentionAllUnits)
//        {
//            AddAudioSet(DispatchReportOutputEvent, Dispatches.AttentionAllUnits());
//        }
//        else if (IncludeSpecificUnits)
//        {
//            AddUnitAttention(DispatchReportOutputEvent);
//        }
//    }
//    private void BuildReportBy()
//    {
//        if (DispatchReportStaticData.IncludeReportedBy)
//        {
//            if (DispatchReportInformation.SeenByOfficers)
//            {
//                AddAudioSet(DispatchReportOutputEvent, Dispatches.OfficersReport());
//            }
//            else
//            {
//                AddAudioSet(DispatchReportOutputEvent, Dispatches.CiviliansReport());
//            }
//        }
//    }
//    private void BuildSets()
//    {
//        if (DispatchReportInformation.InstancesObserved > 1 && DispatchReportStaticData.MainMultiAudioSet.Any())
//        {
//            AddAudioSet(DispatchReportOutputEvent, DispatchReportStaticData.MainMultiAudioSet.PickRandom());
//        }
//        else
//        {
//            AddAudioSet(DispatchReportOutputEvent, DispatchReportStaticData.MainAudioSet.PickRandom());
//        }

//        if (DispatchReportStaticData.SecondaryAudioSet.Any())
//        {
//            AddAudioSet(DispatchReportOutputEvent, DispatchReportStaticData.SecondaryAudioSet.PickRandom());
//        }

//    }
//    private void BuildDescriptions()
//    {
//        if (DispatchReportStaticData.IncludeDrivingVehicle)
//        {
//            AddVehicleDescription(DispatchReportInformation.VehicleSeen, !DispatchReportInformation.SeenByOfficers && DispatchReportStaticData.IncludeLicensePlate, DispatchReportStaticData);
//            GameFiber.Yield();
//        }

//        if (DispatchReportStaticData.IncludeRapSheet)
//        {
//            AddRapSheet(DispatchReportOutputEvent);
//        }

//        if (DispatchReportStaticData.MarkVehicleAsStolen && DispatchReportInformation != null && DispatchReportInformation.VehicleSeen != null && Player.CurrentVehicle != null)//temp current vehicle BS
//        {
//            //THIS NEED TO NOT BE CURRENT VEHICLE, BUT OTHERWISE THE LINK GETS MESSED UP?
//            Player.CurrentVehicle.WasReportedStolen = true;
//            Player.CurrentVehicle.OriginalLicensePlate.IsWanted = true;
//            if (Player.CurrentVehicle.OriginalLicensePlate.PlateNumber == Player.CurrentVehicle.CarPlate.PlateNumber)
//            {
//                Player.CurrentVehicle.CarPlate.IsWanted = true;
//            }
//        }


//        if (DispatchReportStaticData.IncludeCarryingWeapon && (DispatchReportInformation.WeaponSeen != null || DispatchReportStaticData.Name == "Carrying Weapon"))
//        {
//            AddWeaponDescription(DispatchReportOutputEvent, DispatchReportInformation.WeaponSeen);
//            GameFiber.Yield();
//        }


//        if (DispatchReportStaticData.ResultsInLethalForce && !LethalForceAuthorized.HasBeenPlayedThisWanted && DispatchReportStaticData.Name != LethalForceAuthorized.Name)
//        {
//            AddLethalForce(DispatchReportOutputEvent);
//        }


//        if (DispatchReportStaticData.CanAddExtras && Player.IsWanted && !Player.IsDead && Player.PoliceResponse.IsWeaponsFree && !WeaponsFree.HasBeenPlayedThisWanted && DispatchReportStaticData.Name != WeaponsFree.Name)
//        {
//            AddWeaponsFree(DispatchReportOutputEvent);
//        }

//        if (DispatchReportStaticData.CanAddExtras && Player.IsWanted && !Player.IsDead && World.Pedestrians.AnyHelicopterUnitsSpawned && !RequestAirSupport.HasBeenPlayedThisWanted && DispatchReportStaticData.Name != RequestAirSupport.Name)
//        {
//            AddRequestAirSupport(DispatchReportOutputEvent);
//        }


//        if (DispatchReportStaticData.CanAddExtras && DispatchReportStaticData.IncludeDrivingSpeed && Player.CurrentVehicle != null && Player.CurrentVehicle.Vehicle.Exists())
//        {
//            if (DispatchReportInformation.Speed <= Player.Character.Speed)
//            {
//                AddSpeed(DispatchReportOutputEvent, Player.Character.Speed);
//            }
//            else
//            {
//                AddSpeed(DispatchReportOutputEvent, DispatchReportInformation.Speed);
//            }
//            //AddSpeed(EventToPlay, DispatchToPlay.LatestInformation.Speed);// CurrentPlayer.CurrentVehicle.Vehicle.Speed);
//            GameFiber.Yield();
//        }
//    }
//    private void BuildLocations()
//    {
//        if (DispatchReportStaticData.LocationDescription != LocationSpecificity.Nothing)
//        {
//            AddLocationDescription(DispatchReportStaticData.LocationDescription);
//            GameFiber.Yield();
//        }
//    }
//    private void BuildPlayerWarning()
//    {
//        if (DispatchReportStaticData.CanAddExtras && Player.PoliceResponse.PoliceHaveDescription && !DispatchReportInformation.SeenByOfficers && !DispatchReportStaticData.IsStatus)
//        {
//            AddHaveDescription();
//        }
//    }
//    private void BuildUnitReply()
//    {
//        if (DispatchReportOutputEvent.HasUnitAudio)
//        {
//            if (Player.Investigation.InvestigationWantedLevel == 1)
//            {
//                DispatchReportOutputEvent.NotificationText += "~n~~o~Responding Code-2~s~";
//                AddAudioSet(Dispatches.RespondCode2Set());
//            }
//            else if (Player.Investigation.InvestigationWantedLevel > 1)
//            {
//                DispatchReportOutputEvent.NotificationText += "~n~~r~Responding Code-3~s~";
//                AddAudioSet(Dispatches.RespondCode3Set());
//            }
//        }
//    }
//    private void FinalizeReport()
//    {
//        DispatchReportOutputEvent.SoundsToPlay.Add(Dispatches.RadioEndBeep());

//        if (DispatchReportOutputEvent.HasUnitAudio)
//        {
//            foreach (AudioSet audioSet in Dispatches.UnitEnRouteSet().OrderBy(x => Guid.NewGuid()).Take(DispatchReportOutputEvent.UnitAudioAmount).ToList())
//            {
//                DispatchReportOutputEvent.SoundsToPlay.Add(Dispatches.RadioStartBeep());
//                AddAudioSet(audioSet);
//                DispatchReportOutputEvent.SoundsToPlay.Add(Dispatches.RadioEndBeep());
//            }
//        }


//        if (DispatchReportOutputEvent.Subtitles != "")
//        {
//            DispatchReportOutputEvent.Subtitles = NativeHelper.FirstCharToUpper(DispatchReportOutputEvent.Subtitles);
//        }
//        DispatchReportOutputEvent.Priority = DispatchReportStaticData.Priority;


//        if (addtoPlayed)
//        {
//            DispatchReportStaticData.SetPlayed();
//            if (DispatchReportInformation.SeenByOfficers && DispatchReportStaticData.Priority < ScannerPlayable.HighestOfficerReportedPriority)
//            {
//                ScannerPlayable.HighestOfficerReportedPriority = DispatchReportStaticData.Priority;
//            }
//            else if (!DispatchReportInformation.SeenByOfficers && !DispatchReportStaticData.IsStatus && DispatchReportStaticData.Priority < ScannerPlayable.HighestCivilianReportedPriority)
//            {
//                ScannerPlayable.HighestCivilianReportedPriority = DispatchReportStaticData.Priority;
//            }
//        }


//        GameFiber.Yield();


//        if (DispatchReportStaticData.CanAlwaysBeInterrupted)
//        {
//            DispatchReportOutputEvent.CanBeInterrupted = true;
//        }
//        if (DispatchReportStaticData.CanAlwaysInterrupt)
//        {
//            DispatchReportOutputEvent.CanInterrupt = true;
//        }
//        if (DispatchReportStaticData.AnyDispatchInterrupts)
//        {
//            DispatchReportOutputEvent.AnyDispatchInterrupts = true;
//        }

//    }
//    private void AddAudioSet(AudioSet audioSet)
//    {
//        if (audioSet != null)
//        {
//            DispatchReportOutputEvent.SoundsToPlay.AddRange(audioSet.Sounds);
//            DispatchReportOutputEvent.Subtitles += " " + audioSet.Subtitles;
//        }
//    }





//    private void AddUnitAttention()
//    {
//        if (ScannerPlayable.RecentlyMentionedUnits)
//        {
//            return;
//        }
//        bool AddedZoneUnits = false;
//        bool AddedSingleUnit = false;
//        int totalAdded = 0;
//        int totalToAdd = Settings.SettingsManager.ScannerSettings.NumberOfUnitsToAnnounce;
//        List<string> CallSigns = new List<string>();
//        foreach (Cop UnitToCall in World.Pedestrians.Police.Where(x => x.IsRespondingToInvestigation || x.IsRespondingToWanted).OrderBy(x => x.DistanceToPlayer))
//        {
//            if (UnitToCall != null && UnitToCall.Division != -1)
//            {
//                string CallSign = $"{UnitToCall.Division}-{UnitToCall.UnityType}-{UnitToCall.BeatNumber}";
//                if (!CallSigns.Contains(CallSign))
//                {
//                    CallSigns.Add(CallSign);
//                    EntryPoint.WriteToConsole($"Scanner Calling Specific Unit {CallSign}");
//                    List<string> CallsignAudio = Dispatches.GetCallsignScannerAudio(UnitToCall.Division, UnitToCall.UnityType, UnitToCall.BeatNumber);
//                    if (CallsignAudio != null)
//                    {
//                        if (!AddedSingleUnit)
//                        {
//                            DispatchReportOutputEvent.SoundsToPlay.Add(Dispatches.AttentionSpecificUnit());
//                            AddedSingleUnit = true;
//                            DispatchReportOutputEvent.NotificationText += $"~n~~s~Responding:";
//                        }
//                        else
//                        {
//                            DispatchReportOutputEvent.SoundsToPlay.Add(Dispatches.UnitStart());
//                        }
//                        DispatchReportOutputEvent.NotificationText += $" ~p~{CallSign}~s~";
//                        DispatchReportOutputEvent.SoundsToPlay.AddRange(CallsignAudio);
//                        totalAdded++;
//                        AddedZoneUnits = true;
//                    }
//                }
//            }
//            if (totalAdded >= totalToAdd)
//            {
//                break;
//            }
//        }
//        if (!AddedZoneUnits)
//        {
//            Zone MyZone = Player.CurrentLocation.CurrentZone;
//            if (MyZone != null)
//            {
//                ZoneLookup zoneAudio = Dispatches.GetZoneScannerAudio(MyZone.InternalGameName);
//                if (zoneAudio != null)
//                {
//                    string ScannerAudio = zoneAudio.ScannerUnitValues.PickRandom();
//                    if (ScannerAudio != "" && ScannerAudio != null && ScannerAudio.Length > 2)
//                    {
//                        dispatchEvent.SoundsToPlay.Add(ScannerAudio);
//                        AddedZoneUnits = true;
//                    }
//                }
//            }
//        }
//        if (AddedZoneUnits)
//        {
//            dispatchEvent.HasUnitAudio = true;
//            dispatchEvent.UnitAudioAmount = totalAdded;
//        }
//    }
//    private void AddVehicleDescription()
//    {
//        if (DispatchReportInformation.VehicleSeen == null || DispatchReportInformation.VehicleSeen.HasBeenDescribedByDispatch || !DispatchReportInformation.VehicleSeen.Vehicle.Exists())
//        {
//            return;
//        }
//        DispatchReportOutputEvent.NotificationText += "~n~Vehicle:~s~";
//        if (DispatchReportInformation.VehicleSeen.IsPolice)
//        {
//            DispatchReportOutputEvent.SoundsToPlay.Add(suspect_is.SuspectIs.FileName);
//            if (DispatchReportInformation.VehicleSeen.Vehicle.IsBike)
//            {
//                DispatchReportOutputEvent.SoundsToPlay.Add(conjunctives.Onuh.FileName);
//            }
//            else
//            {
//                DispatchReportOutputEvent.SoundsToPlay.Add(conjunctives.DrivingAUmmm.FileName);
//            }
//            if (RandomItems.RandomPercent(50))
//            {
//                DispatchReportOutputEvent.SoundsToPlay.Add(crime_stolen_cop_car.Astolenpolicevehicle1.FileName);
//            }
//            else
//            {
//                DispatchReportOutputEvent.SoundsToPlay.Add(crime_stolen_cop_car.Astolenpolicevehicle.FileName);
//            }
//            DispatchReportOutputEvent.Subtitles += " suspect is driving a stolen police vehicle ~s~";
//        }
//        else
//        {
//            Color CarColor = DispatchReportInformation.VehicleSeen.VehicleColor(); //Vehicles.VehicleManager.VehicleColor(VehicleToDescribe);
//            string MakeName = DispatchReportInformation.VehicleSeen.MakeName();// Vehicles.VehicleManager.MakeName(VehicleToDescribe);
//            int ClassInt = DispatchReportInformation.VehicleSeen.ClassInt();// Vehicles.VehicleManager.ClassInt(VehicleToDescribe);
//            string ClassName = VehicleScannerAudio.ClassName(ClassInt);
//            string ModelName = DispatchReportInformation.VehicleSeen.ModelName();// Vehicles.VehicleManager.ModelName(VehicleToDescribe);

//            string ColorAudio = VehicleScannerAudio.GetColorAudio(CarColor);
//            string MakeAudio = VehicleScannerAudio.GetMakeAudio(MakeName);
//            string ClassAudio = VehicleScannerAudio.GetClassAudio(ClassInt);
//            string ModelAudio = VehicleScannerAudio.GetModelAudio(DispatchReportInformation.VehicleSeen.Vehicle.Model.Hash);

//            DispatchReportOutputEvent.SoundsToPlay.Add(suspect_is.SuspectIs.FileName);
//            DispatchReportOutputEvent.SoundsToPlay.Add(conjunctives.Drivinga.FileName);
//            DispatchReportOutputEvent.Subtitles += " suspect is driving a ~s~";

//            if (ColorAudio != "")
//            {
//                DispatchReportOutputEvent.SoundsToPlay.Add(ColorAudio);
//                DispatchReportOutputEvent.Subtitles += " ~s~" + CarColor.Name + "~s~";
//                DispatchReportOutputEvent.NotificationText += " ~s~" + CarColor.Name + "~s~";
//            }
//            if (MakeAudio != "")
//            {
//                DispatchReportOutputEvent.SoundsToPlay.Add(MakeAudio);
//                DispatchReportOutputEvent.Subtitles += " ~s~" + MakeName + "~s~";
//                DispatchReportOutputEvent.NotificationText += " ~s~" + MakeName + "~s~";
//            }

//            if (ModelAudio != "")
//            {
//                DispatchReportOutputEvent.SoundsToPlay.Add(ModelAudio);
//                DispatchReportOutputEvent.Subtitles += " ~s~" + ModelName + "~s~";
//                DispatchReportOutputEvent.NotificationText += " ~s~" + ModelName + "~s~";
//            }
//            else if (ClassAudio != "")
//            {
//                DispatchReportOutputEvent.SoundsToPlay.Add(ClassAudio);
//                DispatchReportOutputEvent.Subtitles += " ~s~" + ClassName + "~s~";
//                DispatchReportOutputEvent.NotificationText += " ~s~" + ClassName + "~s~";
//            }
//        }
//        if (DispatchReportStaticData.IncludeLicensePlate)
//        {
//            AddAudioSet(Dispatches.LicensePlateSet());
//            string LicensePlateText = DispatchReportInformation.VehicleSeen.OriginalLicensePlate.PlateNumber;
//            DispatchReportOutputEvent.SoundsToPlay.AddRange(VehicleScannerAudio.GetPlateAudio(LicensePlateText));
//            DispatchReportOutputEvent.Subtitles += " ~s~" + LicensePlateText + "~s~";
//            DispatchReportOutputEvent.NotificationText += " ~s~Plate: " + LicensePlateText + "~s~";
//        }
//        if (DispatchReportStaticData.Name == "Suspicious Vehicle")
//        {
//            DispatchReportOutputEvent.NotificationText += "~n~~s~For: " + DispatchReportInformation.VehicleSeen.IsSuspicious(Time.IsNight) + "~s~";
//        }
//        //EntryPoint.WriteToConsole(string.Format("ScannerScript Color {0}, Make {1}, Class {2}, Model {3}, RawModel {4}", CarColor.Name, MakeName, ClassName, ModelName, VehicleToDescribe.Vehicle.Model.Name));
        
//    }



//}

