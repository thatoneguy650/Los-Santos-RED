using ExtensionsMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DispatchScannerFiles;

public static class ScannerScript2
{

    private static List<string> RadioStart = new List<string>() { AudioBeeps.Radio_Start_1.FileName };
    private static List<Dispatch.AudioSet> AttentionAllUnits = new List<Dispatch.AudioSet>()
    {
        new Dispatch.AudioSet(new List<string>() { attention_all_units_gen.Attentionallunits.FileName}," attention all units"),
        new Dispatch.AudioSet(new List<string>() { attention_all_units_gen.Attentionallunits1.FileName }," attention all units"),
        new Dispatch.AudioSet(new List<string>() { attention_all_units_gen.Attentionallunits3.FileName }," attention all units"),
    };
    private static List<Dispatch.AudioSet> OfficersReport = new List<Dispatch.AudioSet>()
    {
        new Dispatch.AudioSet(new List<string>() { we_have.OfficersReport_1.FileName}," officers report"),
        new Dispatch.AudioSet(new List<string>() { we_have.OfficersReport_2.FileName }," officers report"),
    };
    private static List<Dispatch.AudioSet> CiviliansReport = new List<Dispatch.AudioSet>()
    {
        new Dispatch.AudioSet(new List<string>() { we_have.CitizensReport_1.FileName }, " attention all units"),
        new Dispatch.AudioSet(new List<string>() { we_have.CitizensReport_2.FileName }," attention all units"),
        new Dispatch.AudioSet(new List<string>() { we_have.CitizensReport_3.FileName }," attention all units"),
        new Dispatch.AudioSet(new List<string>() { we_have.CitizensReport_4.FileName }," attention all units"),
    };
    private static List<Dispatch.AudioSet> LethalForce = new List<Dispatch.AudioSet>()
    {
        new Dispatch.AudioSet(new List<string>() { lethal_force.Useofdeadlyforceauthorized.FileName}," use of ~r~Deadly Force~s~ authorized"),
        new Dispatch.AudioSet(new List<string>() { lethal_force.Useofdeadlyforceisauthorized.FileName }," use of ~r~Deadly Force~s~ is authorized"),
        new Dispatch.AudioSet(new List<string>() { lethal_force.Useofdeadlyforceisauthorized1.FileName }," use of ~r~Deadly Force~s~ is authorized"),
        new Dispatch.AudioSet(new List<string>() { lethal_force.Useoflethalforceisauthorized.FileName }," use of ~r~Lethal Force~s~ is authorized"),
        new Dispatch.AudioSet(new List<string>() { lethal_force.Useofdeadlyforcepermitted1.FileName }," use of ~r~Deadly Force~s~ permitted"),
    };
    private enum LocationSpecificity
    {
        Nothing = 0,
        Zone = 1,
        HeadingAndZone = 2,
        HeadingAndStreet = 3,
        HeadingStreetAndZone = 4,
        StreetAndZone = 5,
        Street = 6,
    }
    private static void SetupLists()
    {


        Dispatch OfficerDown = new Dispatch()
        {
            IncludeAttention = true,
            ResultsInLethalForce = true,
            NotificationTitle = "Officer Down",
            MainAudioSet = new List<Dispatch.AudioSet>()
            {
                new Dispatch.AudioSet(new List<string>() { we_have.We_Have_1.FileName, crime_officer_down.AcriticalsituationOfficerdown.FileName }," we have a critical situation, ~r~Officer Down~s~"),
                new Dispatch.AudioSet(new List<string>() { we_have.We_Have_1.FileName, crime_officer_down.AnofferdownpossiblyKIA.FileName }," we have an ~r~Officer Down~s~, possibly KIA"),
                new Dispatch.AudioSet(new List<string>() { we_have.We_Have_1.FileName, crime_officer_down.Anofficerdown.FileName }," we have an ~r~Officer Down~s~"),
                new Dispatch.AudioSet(new List<string>() { we_have.We_Have_2.FileName, crime_officer_down.Anofficerdownconditionunknown.FileName }," we have an ~r~Officer Down~s~, condition unknown"),
            },
            SecondaryAudioSet = new List<Dispatch.AudioSet>()
            {
                new Dispatch.AudioSet(new List<string>() { dispatch_respond_code.AllunitsrespondCode99.FileName }," ~s~all units repond ~o~Code-99~s~"),
                new Dispatch.AudioSet(new List<string>() { dispatch_respond_code.AllunitsrespondCode99emergency.FileName }," ~s~all units repond ~o~Code-99 Emergency~s~"),
                new Dispatch.AudioSet(new List<string>() { dispatch_respond_code.Code99allunitsrespond.FileName }," ~o~Code-99 ~s~all units repond~s~~"),
                new Dispatch.AudioSet(new List<string>() { custom_wanted_level_line.Code99allavailableunitsconvergeonsuspect.FileName }," ~o~Code-99 ~s~all available units converge on suspect~s~"),
                new Dispatch.AudioSet(new List<string>() { custom_wanted_level_line.Wehavea1099allavailableunitsrespond.FileName }," we have a ~o~10-99 ~s~ all available units repond~s~"),
                new Dispatch.AudioSet(new List<string>() { dispatch_respond_code.Code99allunitsrespond.FileName }," ~o~Code-99 ~s~all units respond~s~"),
                new Dispatch.AudioSet(new List<string>() { dispatch_respond_code.EmergencyallunitsrespondCode99.FileName }," Emergency all units respond ~o~Code-99 ~s~"),
            }
        };

        Dispatch AssaultingOfficer = new Dispatch()
        {
            IncludeAttention = true,
            ResultsInLethalForce = true,
            NotificationTitle = "Assault on an Officer",
            MainAudioSet = new List<Dispatch.AudioSet>()
            {
                new Dispatch.AudioSet(new List<string>() {  crime_assault_on_an_officer.Anassaultonanofficer.FileName }," an ~r~Assault on an Officer~s~"),
                new Dispatch.AudioSet(new List<string>() { crime_assault_on_an_officer.Anofficerassault.FileName }," an ~r~Officer Assault~s~"),
            },
        };

        Dispatch AttemptingSuicide = new Dispatch()
        {
            NotificationTitle = "Suicide Attempt",
            LocationDescription = LocationSpecificity.Street,
            MainAudioSet = new List<Dispatch.AudioSet>()
            {
                new Dispatch.AudioSet(new List<string>() { crime_9_14a_attempted_suicide.Apossibleattemptedsuicide.FileName }," an ~r~Attempted Suicide~s~"),
                new Dispatch.AudioSet(new List<string>() { crime_9_14a_attempted_suicide.Anattemptedsuicide.FileName }," an ~r~Attempted Suicide~s~")
            }
        };

        Dispatch FelonySpeeding = new Dispatch()
        {
            NotificationTitle = "Felony Speeding",
            IncludeDrivingVehicle = true,
            IncludeDrivignSpeed = true,
            MainAudioSet = new List<Dispatch.AudioSet>()
            {
                new Dispatch.AudioSet(new List<string>() { crime_speeding_felony.Aspeedingfelony.FileName }," a ~r~Speeding Felony~s~"),
            },
        };

        Dispatch PedHitAndRun = new Dispatch()
        {
            NotificationTitle = "Pedestrian Hit-and-Run",
            IncludeDrivingVehicle = true,
            LocationDescription = LocationSpecificity.HeadingAndStreet,
            MainAudioSet = new List<Dispatch.AudioSet>()
            {
                new Dispatch.AudioSet(new List<string>() { crime_ped_struck_by_veh.Apedestrianstruck.FileName}," a ~r~Pedestrian Struck~s~"),
                new Dispatch.AudioSet(new List<string>() { crime_ped_struck_by_veh.Apedestrianstruck1.FileName }," a ~r~Pedestrian Struck~s~"),
                new Dispatch.AudioSet(new List<string>() { crime_ped_struck_by_veh.Apedestrianstruckbyavehicle.FileName }," a ~r~Pedestrian Struck~s~ by a vehicle~s~"),
                new Dispatch.AudioSet(new List<string>() { crime_ped_struck_by_veh.Apedestrianstruckbyavehicle1.FileName }," a ~r~Pedestrian Struck~s~ by a vehicle~s~"),
            },
        };


    }


    //public static void PedestrianHitAndRun(DispatchQueueItem ItemToPlay)
    //{
    //    List<string> ScannerList = new List<string>();
    //    string Subtitles = "";
    //    string NotificationTitle = GetNotificationSubtitle(ItemToPlay.ReportedBy);
    //    AttentionType WhoToNotifiy = GetWhoToNotify(ItemToPlay.ReportedBy);

    //    DispatchNotification Notification = new DispatchNotification("Police Scanner", NotificationTitle, "Pedestrian Hit-and-Run");
    //    ReportGenericStart(ref ScannerList, ref Subtitles, WhoToNotifiy, ItemToPlay.ReportedBy, Game.LocalPlayer.Character.Position);
    //    ScannerList.Add(new List<string>() { crime_ped_struck_by_veh.Apedestrianstruck.FileName, crime_ped_struck_by_veh.Apedestrianstruck1.FileName, crime_ped_struck_by_veh.Apedestrianstruckbyavehicle.FileName, crime_ped_struck_by_veh.Apedestrianstruckbyavehicle1.FileName }.PickRandom());
    //    Subtitles += " a ~r~Pedestrian Struck~s~";
    //    AddVehicleDescription(ItemToPlay.VehicleToReport, ref ScannerList, false, ref Subtitles, ref Notification, true, false, true, ItemToPlay.ReportedBy == ReportType.Officers);
    //    ReportGenericEnd(ref ScannerList, NearType.HeadingAndStreet, ref Subtitles, ref Notification, Game.LocalPlayer.Character.Position);
    //    PlayAudioList(new DispatchAudioEvent(ScannerList, Subtitles, Notification, ItemToPlay.CanBeInterrupted, false));
    //}



    private static void BuildDispatch(Dispatch DispatchToPlay, DispatchCallIn ReportInformation)
    {
        DispatchEvent EventToPlay = new DispatchEvent();
        EventToPlay.SoundsToPlay.Add(RadioStart.PickRandom());

        if (DispatchToPlay.IncludeAttention)
        {
            Dispatch.AudioSet Attention = AttentionAllUnits.PickRandom();
            EventToPlay.SoundsToPlay.Concat(Attention.Sounds);
            EventToPlay.Subtitles += Attention.Subtitles;
        }

        if (DispatchToPlay.IncludeReportedBy)
        {
            if (ReportInformation.SeenByOfficers)
            {
                Dispatch.AudioSet OfficerReportSet = AttentionAllUnits.PickRandom();
                EventToPlay.SoundsToPlay.Concat(OfficerReportSet.Sounds);
                EventToPlay.Subtitles += OfficerReportSet.Subtitles;
            }
            else
            {
                Dispatch.AudioSet CivilianReportSet = CiviliansReport.PickRandom();
                EventToPlay.SoundsToPlay.Concat(CivilianReportSet.Sounds);
                EventToPlay.Subtitles += CivilianReportSet.Subtitles;
            }
        }


        Dispatch.AudioSet Main = DispatchToPlay.MainAudioSet.PickRandom();
        EventToPlay.SoundsToPlay.Concat(Main.Sounds);
        EventToPlay.Subtitles += Main.Subtitles;

        Dispatch.AudioSet Secondary = DispatchToPlay.SecondaryAudioSet.PickRandom();
        if(Secondary != null)//change this to a bool in the audioset
        {
            EventToPlay.SoundsToPlay.Concat(Secondary.Sounds);
            EventToPlay.Subtitles += Secondary.Subtitles;
        }

        if(DispatchToPlay.ResultsInLethalForce)
        {
            Dispatch.AudioSet LethalForceSet = LethalForce.PickRandom();
            EventToPlay.SoundsToPlay.Concat(LethalForceSet.Sounds);
            EventToPlay.Subtitles += LethalForceSet.Subtitles;
        }



        


    }
    private class Dispatch
    {
        public bool IncludeAttention { get; set; } = false;
        public bool IncludeReportedBy { get; set; } = true;
        public string NotificationTitle { get; set; } = "Police Scanner";
        public string NotificationSubtitle { get; set; }
        public string NotificationText { get; set; }
        public List<AudioSet> MainAudioSet { get; set; } = new List<AudioSet>();
        public List<AudioSet> SecondaryAudioSet { get; set; } = new List<AudioSet>();
        public bool MarkVehicleAsStolen { get; set; } = false;
        public bool IncludeDrivingVehicle { get; set; } = false;
        public bool IncludeDrivignSpeed { get; set; } = false;
        public bool ReportCharctersPosition { get; set; } = true;
        public int Priority { get; set; } = 0;
        public bool ResultsInLethalForce { get; set; } = false;
        public bool ResultsInStolenCarSpotted { get; set; } = false;
        public bool IsTrafficViolation { get; set; } = false;
        public bool IsAmbient { get; set; } = false;
        public int ResultingWantedLevel { get; set; }
        public LocationSpecificity LocationDescription { get; set; } = LocationSpecificity.Nothing;
        public Dispatch()
        {

        }
        public class AudioSet
        {
            public AudioSet(List<string> sounds, string subtitles)
            {
                Sounds = sounds;
                Subtitles = subtitles;
            }
            public List<string> Sounds { get; set; }
            public string Subtitles { get; set; }

        }
    }
    private class DispatchCallIn
    {
        public DispatchCallIn(bool suspectStatusOnFoot, bool reportedByOfficers)
        {
            SeenOnFoot = suspectStatusOnFoot;
            SeenByOfficers = reportedByOfficers;
        }
        public float Speed { get; set; }
        public GTAWeapon WeaponSeen { get; set; }
        public VehicleExt VehicleSeen { get; set; }
        public bool SeenOnFoot { get; set; } = true;
        public bool SeenByOfficers { get; set; } = false;
    }
    private class DispatchEvent
    {
        public DispatchEvent()
        {

        }
        public List<string> SoundsToPlay { get; set; }
        public string Subtitles { get; set; }
        public bool CanBeInterrupted { get; set; } = true;
        public bool CanInterrupt { get; set; } = true;
        public string NotificationTitle { get; set; } = "Police Scanner";
        public string NotificationSubtitle { get; set; } = "~g~Status~s~";
        public string NotificationText { get; set; } = "~b~Scanner Audio~s~";
    }

}

