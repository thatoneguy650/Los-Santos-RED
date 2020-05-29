using ExtensionsMethods;
using NAudio.Wave;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Media;
using static DispatchScannerFiles;
using static Zones;

public static class ScannerScript
{
    private static uint GameTimeLastAnnouncedDispatch;
    private static List<uint> NotificationHandles = new List<uint>();
    private static readonly Random rnd;
    private static WaveOutEvent outputDevice;
    private static AudioFileReader audioFile;
    private static bool ExecutingQueue = false;
    private static List<DispatchQueueItem> DispatchQueue = new List<DispatchQueueItem>();
    private static List<DispatchLettersNumber> LettersAndNumbersLookup = new List<DispatchLettersNumber>();
    private static List<ColorLookup> ColorLookups = new List<ColorLookup>();
    private static List<string> DamagedScannerAliases = new List<string>();
    private static bool CurrentlyPlayingCanBeInterrupted = false;
    private static uint GameTimeLastDisplayedSubtitle;
    private static uint GameTimeLastCivilianReported;

    public static Dispatch SuspectReacquired { get; set; }
    public static bool CancelAudio { get; set; }
    public static bool IsPlayingAudio { get; set; }
    public static bool ReportedMilitaryDeployed { get; set; } = false;
    public static bool ReportedAirSupportRequested { get; set; } = false;
    public static bool ReportedLethalForceAuthorized { get; set; } = false;
    public static bool ReportedWeaponsFree { get; set; } = false;
    public static bool ReportedOfficerDown { get; set; } = false;
    public static bool IsRunning { get; set; } = true;
    public static int LastCivilianReportedPriority { get; set; }
    public static bool RecentAnnouncedDispatch
    {
        get
        {
            if (GameTimeLastAnnouncedDispatch == 0)
                return false;
            else if (Game.GameTime - GameTimeLastAnnouncedDispatch <= 30000)
                return true;
            else
                return false;
        }
    }
    public static bool AudioPlaying
    {
        get
        {
            return outputDevice != null;
        }
    }
    public enum NearType
    {
        Nothing = 0,
        Zone = 1,
        HeadingAndZone = 2,
        HeadingAndStreet = 3,
        HeadingStreetAndZone = 4,
        StreetAndZone = 5,
        Street = 6,
    }
    public enum ReportType
    {
        Nobody = -1,
        Civilians = 0,
        Officers = 1,
        WeHave = 2,
    }
    public enum AttentionType
    {
        Nobody = -1,
        AllUnits = 0,
        LocalUnits = 1,
        SpecificUnits = 2,//unused, maybe some weekend ill listen to all that crap

    }
    public static void Initialize()
    {
        GameTimeLastDisplayedSubtitle = 0;
        outputDevice = null;
        audioFile = default;
        ExecutingQueue = false;
        DispatchQueue = new List<DispatchQueueItem>();
        ReportedAirSupportRequested = false;
        ReportedMilitaryDeployed = false;
        ReportedLethalForceAuthorized = false;
        ReportedWeaponsFree = false;
        ReportedOfficerDown = false;
        LettersAndNumbersLookup = new List<DispatchLettersNumber>();
        ColorLookups = new List<ColorLookup>();
        NotificationHandles = new List<uint>();
        DamagedScannerAliases = new List<string>();
        CancelAudio = false;
        IsPlayingAudio = false;
        SetupLists();
    }
    public static void Dispose()
    {
        IsRunning = false;
        AbortAllAudio();
    }
    private static void SetupLists()
    {
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('A', lp_letters_high.Adam.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('B', lp_letters_high.Boy.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('C', lp_letters_high.Charles.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('D', lp_letters_high.David.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('E', lp_letters_high.Edward.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('F', lp_letters_high.Frank.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('G', lp_letters_high.George.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('H', lp_letters_high.Henry.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('I', lp_letters_high.Ita.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('J', lp_letters_high.John.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('K', lp_letters_high.King.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('L', lp_letters_high.Lincoln.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('M', lp_letters_high.Mary.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('N', lp_letters_high.Nora.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('O', lp_letters_high.Ocean.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('P', lp_letters_high.Paul.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('Q', lp_letters_high.Queen.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('R', lp_letters_high.Robert.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('S', lp_letters_high.Sam.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('T', lp_letters_high.Tom.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('U', lp_letters_high.Union.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('V', lp_letters_high.Victor.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('W', lp_letters_high.William.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('X', lp_letters_high.XRay.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('Y', lp_letters_high.Young.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('Z', lp_letters_high.Zebra.FileName));

        LettersAndNumbersLookup.Add(new DispatchLettersNumber('A', lp_letters_high.Adam1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('B', lp_letters_high.Boy1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('C', lp_letters_high.Charles1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('E', lp_letters_high.Edward1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('F', lp_letters_high.Frank1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('G', lp_letters_high.George1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('H', lp_letters_high.Henry1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('I', lp_letters_high.Ita1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('J', lp_letters_high.John1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('K', lp_letters_high.King1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('L', lp_letters_high.Lincoln1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('M', lp_letters_high.Mary1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('N', lp_letters_high.Nora1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('O', lp_letters_high.Ocean1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('P', lp_letters_high.Paul1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('Q', lp_letters_high.Queen1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('R', lp_letters_high.Robert1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('S', lp_letters_high.Sam1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('T', lp_letters_high.Tom1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('U', lp_letters_high.Union1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('V', lp_letters_high.Victor1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('W', lp_letters_high.William1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('X', lp_letters_high.XRay1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('Y', lp_letters_high.Young1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('Z', lp_letters_high.Zebra1.FileName));



        LettersAndNumbersLookup.Add(new DispatchLettersNumber('1', lp_numbers.One.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('2', lp_numbers.Two.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('3', lp_numbers.Three.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('4', lp_numbers.Four.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('5', lp_numbers.Five.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('6', lp_numbers.Six.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('7', lp_numbers.Seven.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('8', lp_numbers.Eight.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('9', lp_numbers.Nine.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('0', lp_numbers.Zero.FileName));

        LettersAndNumbersLookup.Add(new DispatchLettersNumber('1', lp_numbers.One1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('2', lp_numbers.Two1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('3', lp_numbers.Three1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('4', lp_numbers.Four1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('5', lp_numbers.Five1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('6', lp_numbers.Six1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('7', lp_numbers.Seven1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('8', lp_numbers.Eight1.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('9', lp_numbers.Niner.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('0', lp_numbers.Zero1.FileName));

        LettersAndNumbersLookup.Add(new DispatchLettersNumber('1', lp_numbers.One2.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('2', lp_numbers.Two2.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('3', lp_numbers.Three2.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('4', lp_numbers.Four2.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('5', lp_numbers.Five2.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('6', lp_numbers.Six2.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('7', lp_numbers.Seven2.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('8', lp_numbers.Eight2.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('9', lp_numbers.Niner2.FileName));
        LettersAndNumbersLookup.Add(new DispatchLettersNumber('0', lp_numbers.Zero2.FileName));

        ColorLookups.Add(new ColorLookup(colour.COLORRED01.FileName, System.Drawing.Color.Red));
        ColorLookups.Add(new ColorLookup(colour.COLORAQUA01.FileName, System.Drawing.Color.Aqua));
        ColorLookups.Add(new ColorLookup(colour.COLORBEIGE01.FileName, System.Drawing.Color.Beige));
        ColorLookups.Add(new ColorLookup(colour.COLORBLACK01.FileName, System.Drawing.Color.Black));
        ColorLookups.Add(new ColorLookup(colour.COLORBLUE01.FileName, System.Drawing.Color.Blue));
        ColorLookups.Add(new ColorLookup(colour.COLORBROWN01.FileName, System.Drawing.Color.Brown));
        ColorLookups.Add(new ColorLookup(colour.COLORDARKBLUE01.FileName, System.Drawing.Color.DarkBlue));
        ColorLookups.Add(new ColorLookup(colour.COLORDARKGREEN01.FileName, System.Drawing.Color.DarkGreen));
        ColorLookups.Add(new ColorLookup(colour.COLORDARKGREY01.FileName, System.Drawing.Color.DarkGray));
        ColorLookups.Add(new ColorLookup(colour.COLORDARKORANGE01.FileName, System.Drawing.Color.DarkOrange));
        ColorLookups.Add(new ColorLookup(colour.COLORDARKRED01.FileName, System.Drawing.Color.DarkRed));
        ColorLookups.Add(new ColorLookup(colour.COLORGOLD01.FileName, System.Drawing.Color.Gold));
        ColorLookups.Add(new ColorLookup(colour.COLORGREEN01.FileName, System.Drawing.Color.Green));
        ColorLookups.Add(new ColorLookup(colour.COLORGREY01.FileName, System.Drawing.Color.Gray));
        ColorLookups.Add(new ColorLookup(colour.COLORGREY02.FileName, System.Drawing.Color.Gray));
        ColorLookups.Add(new ColorLookup(colour.COLORLIGHTBLUE01.FileName, System.Drawing.Color.LightBlue));
        ColorLookups.Add(new ColorLookup(colour.COLORMAROON01.FileName, System.Drawing.Color.Maroon));
        ColorLookups.Add(new ColorLookup(colour.COLORORANGE01.FileName, System.Drawing.Color.Orange));
        ColorLookups.Add(new ColorLookup(colour.COLORPINK01.FileName, System.Drawing.Color.Pink));
        ColorLookups.Add(new ColorLookup(colour.COLORPURPLE01.FileName, System.Drawing.Color.Purple));
        ColorLookups.Add(new ColorLookup(colour.COLORRED01.FileName, System.Drawing.Color.Red));
        ColorLookups.Add(new ColorLookup(colour.COLORSILVER01.FileName, System.Drawing.Color.Silver));
        ColorLookups.Add(new ColorLookup(colour.COLORWHITE01.FileName, System.Drawing.Color.White));
        ColorLookups.Add(new ColorLookup(colour.COLORYELLOW01.FileName, System.Drawing.Color.Yellow));

        DamagedScannerAliases.Add(extra_prefix.Damaged.FileName);


        Dispatch AttemptingSuicide = new Dispatch()
        {
            MainAudioSet = new List<Dispatch.AudioSet>()
            {
                new Dispatch.AudioSet(new List<string>() { crime_9_14a_attempted_suicide.Apossibleattemptedsuicide.FileName }," an ~r~Attempted Suicide~s~"),
                new Dispatch.AudioSet(new List<string>() { crime_9_14a_attempted_suicide.Anattemptedsuicide.FileName }," an ~r~Attempted Suicide~s~")
            }
        };


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




    }

    private static void AssaultingOfficer(DispatchQueueItem ItemToPlay)
    {
        if (ReportedLethalForceAuthorized || ReportedOfficerDown)
            return;
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        string NotificationTitle = GetNotificationSubtitle(ItemToPlay.ReportedBy);
       // AttentionType WhoToNotifiy = GetWhoToNotify(ItemToPlay.ReportedBy);
       // ReportGenericStart(ref ScannerList, ref Subtitles, WhoToNotifiy, ItemToPlay.ReportedBy, Game.LocalPlayer.Character.Position);
        Subtitles += " we have an ~r~Assault on an Officer~s~";
        DispatchNotification Notification = new DispatchNotification("Police Scanner", NotificationTitle, "Assault on an Officer");
        ScannerList.Add(new List<string>() { crime_assault_on_an_officer.Anassaultonanofficer.FileName, crime_assault_on_an_officer.Anofficerassault.FileName }.PickRandom());
        AddLethalForceAuthorized(ref ScannerList, ref Subtitles, ref Notification);
        ReportGenericEnd(ref ScannerList, NearType.Nothing, ref Subtitles, ref Notification, Game.LocalPlayer.Character.Position);
        PlayAudioList(new DispatchAudioEvent(ScannerList, Subtitles, Notification, ItemToPlay.CanBeInterrupted, true));
    }
    private static void PlayDispatch(Dispatch DispatchToPlay)
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";





        Vector3 PositionToReport = Game.LocalPlayer.Character.Position;
        if (DispatchToPlay.ReportedBy == ReportType.Civilians)
            PositionToReport = Investigation.InvestigationPosition;

        
        // ReportGenericStart(ref ScannerList, ref Subtitles, ItemToPlay.WhoToNotify, ItemToPlay.ReportedBy, PositionToReport);


        //ScannerList.Concat(ItemToPlay.PossibleAudioToPlay.PickRandom().AudioList.ToList());


       // Subtitles += ItemToPlay.SubtitleText;


        //if (DispatchToPlay.IncludeDrivingVehicle)
        //{
        //    AddDrivingVehicle(ref ScannerList, ref Subtitles, ref Notification, DispatchToPlay.VehicleToReport, DispatchToPlay.ReportedBy);
        //}







        if (DispatchToPlay.NotificationSubtitle == "")
        {
            if (DispatchToPlay.ReportedBy == ReportType.Officers)
                DispatchToPlay.NotificationSubtitle = "~o~Crime Observed~s~";
            else
                DispatchToPlay.NotificationSubtitle = "~y~Crime Reported~s~";
        }
        DispatchNotification Notification = new DispatchNotification(DispatchToPlay.NotificationTitle, DispatchToPlay.NotificationSubtitle, DispatchToPlay.NotificationText);


        ReportGenericEnd(ref ScannerList, NearType.Street, ref Subtitles, ref Notification, PositionToReport);
        bool CanBeInterrupted = DispatchToPlay.ReportedBy == ReportType.Civilians ? true : false;
        //PlayAudioList(new DispatchAudioEvent(ScannerList, Subtitles, Notification, CanBeInterrupted, !CanBeInterrupted));

        if (DispatchToPlay.MarkVehicleAsStolen && DispatchToPlay.VehicleToReport != null)
        {
            MarkVehicleAsStolen(DispatchToPlay.VehicleToReport);
        }
    }

    public static void PlayAudioList(DispatchAudioEvent MyAudioEvent)
    {
        /////////Maybe?
        if (MyAudioEvent.CanInterrupt && CurrentlyPlayingCanBeInterrupted)
        {
            Debugging.WriteToLog("PlayAudioList", "Aborting Audio In the Middle");
            AbortAllAudio();
        }
        Debugging.WriteToLog("PlayAudioList", string.Format("CanBeInterrupted:{0}CanInterrupt:{1}Name:{2}", MyAudioEvent.CanBeInterrupted, MyAudioEvent.CanInterrupt, MyAudioEvent.Subtitles));
        GameFiber PlayAudioList = GameFiber.StartNew(delegate
        {
            while (IsPlayingAudio)
            {
                GameFiber.Yield();
            }

            if (MyAudioEvent.NotificationToDisplay != null && General.MySettings.Police.DispatchNotifications)
            {
                RemoveAllNotifications();
                NotificationHandles.Add(Game.DisplayNotification(MyAudioEvent.NotificationToDisplay.TextureDict, MyAudioEvent.NotificationToDisplay.TextureName, MyAudioEvent.NotificationToDisplay.Title, MyAudioEvent.NotificationToDisplay.Subtitle, MyAudioEvent.NotificationToDisplay.Text));
            }
            CurrentlyPlayingCanBeInterrupted = MyAudioEvent.CanBeInterrupted;

            //foreach (string audioname in MyAudioEvent.MainAudio)
            //{
            //    PlayAudio(audioname);

            //    while (IsPlayingAudio)
            //    {
            //        if (MyAudioEvent.Subtitles != "" && General.MySettings.Police.DispatchSubtitles && Game.GameTime - GameTimeLastDisplayedSubtitle >= 1500)
            //        {
            //            Game.DisplaySubtitle(MyAudioEvent.Subtitles, 2000);
            //            GameTimeLastDisplayedSubtitle = Game.GameTime;
            //        }
            //        GameFiber.Yield();
            //    }
            //    if (CancelAudio)
            //    {
            //        CancelAudio = false;
            //        Debugging.WriteToLog("PlayAudioList", "CancelAudio Set to False");
            //        break;
            //    }
            //    GameTimeLastAnnouncedDispatch = Game.GameTime;
            //}
        }, "PlayAudioList");
        Debugging.GameFibers.Add(PlayAudioList);
    }
    private static void PlayAudio(string _Audio)
    {
        try
        {
            if (_Audio == "")
                return;
            if (outputDevice == null)
            {
                outputDevice = new WaveOutEvent();
                outputDevice.PlaybackStopped += OnPlaybackStopped;
            }
            if (audioFile == null)
            {
                audioFile = new AudioFileReader(string.Format("Plugins\\LosSantosRED\\audio\\{0}", _Audio))
                {
                    Volume = General.MySettings.Police.DispatchAudioVolume
                };
                outputDevice.Init(audioFile);
            }
            else
            {
                outputDevice.Init(audioFile);
            }
            outputDevice.Play();
            IsPlayingAudio = true;
        }
        catch (Exception e)
        {
            Debugging.WriteToLog("PlayAudio", e.Message);
        }
    }
    private static void OnPlaybackStopped(object sender, StoppedEventArgs args)
    {
        outputDevice.Dispose();
        outputDevice = null;
        if (audioFile != null)
        {
            audioFile.Dispose();
        }
        audioFile = null;
        IsPlayingAudio = false;
    }
    public static void AddDispatchToQueue(DispatchQueueItem _ItemToAdd)
    {
        //if (!DispatchQueue.Any(x => x.Type == _ItemToAdd.Type))
        //{
        //    DispatchQueue.Add(_ItemToAdd);
        //}
    }
    public static void Tick()
    {
        if (IsRunning)
        {
            if (!General.MySettings.Police.DispatchAudio)
            {
                DispatchQueue.Clear();
                return;
            }

            if (DispatchQueue.Count > 0 && !ExecutingQueue)
            {
                //Debugging.WriteToLog("PlayDispatchQueue", "Delegate Started");
                ExecutingQueue = true;
                GameFiber PlayDispatchQueue = GameFiber.StartNew(delegate
                {
                    GameFiber.Sleep(rnd.Next(1500, 2500));

                    //foreach(DispatchQueueItem Stuff in DispatchQueue.OrderBy(x => x.Priority))
                    //{
                    //    Debugging.WriteToLog("ToPlay", string.Format("Type: {0} {1}", Stuff.Type,Stuff.Priority));
                    //}
                    DispatchQueue.OrderBy(x => x.Priority);
                    if (DispatchQueue.Any(x => !x.IsAmbient))
                    {
                        DispatchQueue.RemoveAll(x => x.IsAmbient);
                    }
                    if (DispatchQueue.Any(x => x.ReportedBy == ReportType.Officers))
                    {
                        DispatchQueue.RemoveAll(x => x.ReportedBy != ReportType.Officers);
                    }
                    if (DispatchQueue.Count() > 1)
                    {
                        DispatchQueueItem HighestItem = DispatchQueue.OrderBy(x => x.Priority).FirstOrDefault();
                        DispatchQueue.Clear();
                        if (HighestItem != null)
                        {
                            DispatchQueue.Add(HighestItem);
                        }
                    }

                    if (DispatchQueue.Any(x => x.ReportedBy == ReportType.Civilians) && Game.GameTime - GameTimeLastCivilianReported <= 45000)
                    {
                        foreach (DispatchQueueItem ItemToPlay in DispatchQueue)
                        {
                            //if (ItemToPlay.Priority <= LastCivilianReportedPriority)
                            //{
                            //    Debugging.WriteToLog("Ignoring Low Priority", ItemToPlay.Type.ToString());
                            //}
                        }
                        DispatchQueue.RemoveAll(x => x.Priority <= LastCivilianReportedPriority);
                    }

                    while (DispatchQueue.Count > 0)
                    {
                        DispatchQueueItem Item = DispatchQueue.OrderBy(x => x.Priority).ToList()[0];

                        if (Item.ReportedBy == ReportType.Civilians && PlayerState.IsWanted)
                        {
                            //Debugging.WriteToLog("Ignoring Civilian Repoted", Item.Type.ToString());
                        }
                        else
                        {
                            if (Item.ReportedBy == ReportType.Civilians)
                            {
                                GameTimeLastCivilianReported = Game.GameTime;
                                LastCivilianReportedPriority = Item.Priority;
                            }
                           // Debugging.WriteToLog("Playing", Item.Type.ToString());
                            //if (Item.Type == AvailableDispatch.AssaultingOfficer)
                            //    AssaultingOfficer(Item);
                            //else if (Item.Type == AvailableDispatch.CarryingWeapon)
                            //    CarryingWeapon(Item);
                            //else if (Item.Type == AvailableDispatch.FelonySpeeding)
                            //    FelonySpeeding(Item);
                            //else if (Item.Type == AvailableDispatch.LethalForceAuthorized)
                            //    LethalForceAuthorized(Item);
                            //else if (Item.Type == AvailableDispatch.OfficerDown)
                            //    OfficerDown(Item);
                            //else if (Item.Type == AvailableDispatch.PedestrianHitAndRun)
                            //    PedestrianHitAndRun(Item);
                            //else if (Item.Type == AvailableDispatch.RecklessDriving)
                            //    RecklessDriving(Item);
                            //else if (Item.Type == AvailableDispatch.ShootingAtPolice)
                            //    ShootingAtPolice(Item);
                            //else if (Item.Type == AvailableDispatch.SpottedStolenCar)
                            //    SpottedStolenCar(Item);
                            //else if (Item.Type == AvailableDispatch.ReportStolenVehicle)
                            //    ReportStolenVehicle(Item);
                            //else if (Item.Type == AvailableDispatch.SuspectArrested)
                            //    SuspectArrested(Item);
                            //else if (Item.Type == AvailableDispatch.SuspectEvadedOfficers)
                            //    SuspectEvadedOfficers(Item);
                            //else if (Item.Type == AvailableDispatch.SuspectLost)
                            //    SuspectLost(Item);
                            //else if (Item.Type == AvailableDispatch.SuspectWasted)
                            //    SuspectWasted(Item);
                            //else if (Item.Type == AvailableDispatch.ThreateningOfficerWithFirearm)
                            //    ThreateningOfficerWithFirearm(Item);
                            //else if (Item.Type == AvailableDispatch.VehicleHitAndRun)
                            //    VehicleHitAndRun(Item);
                            //else if (Item.Type == AvailableDispatch.WeaponsFree)
                            //    WeaponsFree(Item);
                            //else if (Item.Type == AvailableDispatch.SuspiciousActivity)
                            //    SuspiciousActivity(Item);
                            //else if (Item.Type == AvailableDispatch.SuspiciousVehicle)
                            //    SuspiciousVehicle(Item);
                            //else if (Item.Type == AvailableDispatch.GrandTheftAuto)
                            //    GrandTheftAuto(Item);
                            //else if (Item.Type == AvailableDispatch.SuspectReacquired)
                            //    SuspectReacquired(Item);
                            //else if (Item.Type == AvailableDispatch.RequestBackup)
                            //    RequestBackup(Item);
                            //else if (Item.Type == AvailableDispatch.RunningARedLight)
                            //    RunningARedLight(Item);
                            //else if (Item.Type == AvailableDispatch.SuspectSpotted)
                            //    SuspectSpotted(Item);
                            //else if (Item.Type == AvailableDispatch.CriminalActivity)
                            //    CriminalActivity(Item);
                            //else if (Item.Type == AvailableDispatch.ShotsFired)
                            //    ShotsFired(Item);
                            //else if (Item.Type == AvailableDispatch.ResumePatrol)
                            //    ResumePatrol(Item);
                            //else if (Item.Type == AvailableDispatch.LostVisualOnSuspect)
                            //    LostVisualOnSuspect(Item);
                            //else if (Item.Type == AvailableDispatch.TerroistActivity)
                            //    TerroristActivity(Item);
                            //else if (Item.Type == AvailableDispatch.TrespassingOnGovernmentProperty)
                            //    TrespassingOnGovernmentProperty(Item);
                            //else if (Item.Type == AvailableDispatch.SuspectChangedVehicle)
                            //    SuspectChangedVehicle(Item);
                            //else if (Item.Type == AvailableDispatch.CivlianFatality)
                            //    CivlianFatality(Item);
                            //else if (Item.Type == AvailableDispatch.CivilianInjury)
                            //    CivilianInjury(Item);
                            //else if (Item.Type == AvailableDispatch.CivilianShot)
                            //    CivilianShot(Item);
                            //else if (Item.Type == AvailableDispatch.StealingAirVehicle)
                            //    StealingAirVehicle(Item);
                            //else if (Item.Type == AvailableDispatch.SuspectResisitingArrest)
                            //    SuspectResisitingArrest(Item);
                            //else if (Item.Type == AvailableDispatch.CivilianMugged)
                            //    CivilianMugged(Item);
                            //else if (Item.Type == AvailableDispatch.NoFurtherUnitsNeeded)
                            //    NoFurtherUnitsNeeded(Item);
                            //else if (Item.Type == AvailableDispatch.AttemptingSuicide)
                            //    AttemptingSuicide(Item);
                            //else if (Item.Type == AvailableDispatch.MilitaryDeployed)
                            //    MilitaryDeployed(Item);
                            //else if (Item.Type == AvailableDispatch.AirSupportRequested)
                            //    AirSupportRequested(Item);

                            //if (Item.ResultingWantedLevel > 0)
                            //    WantedLevel.SetWantedLevel(Item.ResultingWantedLevel, string.Format("Set Wanted After Dispatch: {0}", Item.Type), true);
                        }
                        if (DispatchQueue.Contains(Item))
                            DispatchQueue.Remove(Item);
                    }
                    ExecutingQueue = false;
                }, "PlayDispatchQueue");
                Debugging.GameFibers.Add(PlayDispatchQueue);
            }
        }
    }
    public static void ClearDispatchQueue()
    {
        DispatchQueue.Clear();
    }

    //private static bool CanRun(DispatchQueueItem ItemToPlay)
    //{
    //    if (ItemToPlay.Type == AvailableDispatch.AssaultingOfficer && (ReportedLethalForceAuthorized || ReportedOfficerDown))
    //        return false;
    //    else
    //        return true;
    //}
    //Starting
    private static void ReportGenericStart(ref List<string> ScannerList, ref string Subtitles, AttentionType WhoToNotify, ReportType ReportedBy, Vector3 PlaceToReport)
    {
        ScannerList.Add(new List<string> { AudioBeeps.Radio_Start_1.FileName, AudioBeeps.Radio_Start_2.FileName }.PickRandom());
        Subtitles = "";
        if (WhoToNotify == AttentionType.AllUnits)
        {
            ScannerList.Add(new List<string>() { attention_all_units_gen.Attentionallunits.FileName, attention_all_units_gen.Attentionallunits1.FileName, attention_all_units_gen.Attentionallunits3.FileName, attention_all_units_gen.Attentionallunits3.FileName }.PickRandom());
            Subtitles += "Attention all units,";
        }
        bool SubsBlank = Subtitles == "";
        if (ReportedBy == ReportType.Officers)
        {
            ScannerList.Add(new List<string>() { we_have.OfficersReport_1.FileName, we_have.OfficersReport_2.FileName }.PickRandom());
            if (SubsBlank)
                Subtitles += "Officers report";
            else
                Subtitles += " officers report";
        }
        else if (ReportedBy == ReportType.Civilians)
        {
            ScannerList.Add(new List<string>() { we_have.CitizensReport_1.FileName, we_have.CitizensReport_2.FileName, we_have.CitizensReport_3.FileName, we_have.CitizensReport_4.FileName }.PickRandom());
            if (SubsBlank)
                Subtitles += "Civilians report";
            else
                Subtitles += " civilians report";
        }
        else if (ReportedBy == ReportType.WeHave)
        {
            ScannerList.Add(new List<string>() { we_have.We_Have_1.FileName, we_have.We_Have_2.FileName }.PickRandom());
            if (SubsBlank)
                Subtitles += "We have";
            else
                Subtitles += " we have";
        }
        Subtitles.Trim();
    }
    private static void ReportGenericEnd(ref List<string> ScannerList, NearType Near, ref string Subtitles, ref DispatchNotification Notification, Vector3 LocationToReport)
    {
        if (Near == NearType.Zone)
        {
            AddZone(ref ScannerList, ref Subtitles, LocationToReport, ref Notification);
        }
        else if (Near == NearType.HeadingAndZone)
        {
            AddHeading(ref ScannerList, ref Subtitles);
            AddZone(ref ScannerList, ref Subtitles, LocationToReport, ref Notification);
        }
        else if (Near == NearType.HeadingAndStreet)
        {
            AddHeading(ref ScannerList, ref Subtitles);
            if (!AddStreet(ref ScannerList, ref Subtitles, ref Notification))
                AddZone(ref ScannerList, ref Subtitles, LocationToReport, ref Notification);
        }
        else if (Near == NearType.HeadingStreetAndZone)
        {
            AddHeading(ref ScannerList, ref Subtitles);
            AddStreet(ref ScannerList, ref Subtitles, ref Notification);
            AddZone(ref ScannerList, ref Subtitles, LocationToReport, ref Notification);
        }
        else if (Near == NearType.Street)
        {
            if (!AddStreet(ref ScannerList, ref Subtitles, ref Notification))
                AddZone(ref ScannerList, ref Subtitles, LocationToReport, ref Notification);
        }
        else if (Near == NearType.StreetAndZone)
        {
            AddStreet(ref ScannerList, ref Subtitles, ref Notification);
            AddZone(ref ScannerList, ref Subtitles, LocationToReport, ref Notification);
        }
        ScannerList.Add(new List<string> { AudioBeeps.Radio_End_1.FileName, AudioBeeps.Radio_End_2.FileName }.PickRandom());
    }
    public static bool AddHeading(ref List<string> ScannerList, ref string Subtitles)
    {
        if (Police.AnyRecentlySeenPlayer)
        {
            ScannerList.Add((new List<string>() { suspect_heading.TargetLastSeenHeading.FileName, suspect_heading.TargetReportedHeading.FileName, suspect_heading.TargetSeenHeading.FileName, suspect_heading.TargetSpottedHeading.FileName }).PickRandom());
            Subtitles += " ~s~suspect heading~s~";
            string heading = GetSimpleCompassHeading();
            if (heading == "N")
            {
                ScannerList.Add(direction_heading.North.FileName);
                Subtitles += " ~g~North~s~";
            }
            else if (heading == "S")
            {
                ScannerList.Add(direction_heading.South.FileName);
                Subtitles += " ~g~South~s~";
            }
            else if (heading == "E")
            {
                ScannerList.Add(direction_heading.East.FileName);
                Subtitles += " ~g~East~s~";
            }
            else if (heading == "W")
            {
                ScannerList.Add(direction_heading.West.FileName);
                Subtitles += " ~g~West~s~";
            }
            return true;
        }
        else
            return false;
    }
    public static bool AddStreet(ref List<string> ScannerList, ref string Subtitles, ref DispatchNotification Notification)
    {
        Street MyStreet = PlayerLocation.PlayerCurrentStreet;
        if (MyStreet != null && MyStreet.DispatchFile != "")
        {
            ScannerList.Add((new List<string>() { conjunctives.On.FileName, conjunctives.On1.FileName, conjunctives.On2.FileName, conjunctives.On3.FileName, conjunctives.On4.FileName }).PickRandom());
            ScannerList.Add(MyStreet.DispatchFile);
            Subtitles += " ~s~on ~HUD_COLOUR_YELLOWLIGHT~" + MyStreet.Name + "~s~";
            Notification.Text += "~n~Location: ~HUD_COLOUR_YELLOWLIGHT~" + MyStreet.Name + "~s~";

            if (PlayerLocation.PlayerCurrentCrossStreet != null)
            {
                Street MyCrossStreet = PlayerLocation.PlayerCurrentCrossStreet;
                if (MyCrossStreet != null && MyCrossStreet.DispatchFile != "")
                {
                    ScannerList.Add((new List<string>() { conjunctives.AT01.FileName }).PickRandom());
                    ScannerList.Add(MyCrossStreet.DispatchFile);
                    Notification.Text += " ~s~at ~HUD_COLOUR_YELLOWLIGHT~" + MyCrossStreet.Name + "~s~";
                    Subtitles += " ~s~at ~HUD_COLOUR_YELLOWLIGHT~" + MyCrossStreet.Name + "~s~";
                }
            }
            return true;
        }
        return false;
    }
    public static bool AddZone(ref List<string> ScannerList, ref string Subtitles, Vector3 LocationToGetZone, ref DispatchNotification Notification)
    {
        Zone MyZone = GetZoneAtLocation(LocationToGetZone);
        if (MyZone != null && MyZone.ScannerValue != "")
        {
            ScannerList.Add(new List<string> { conjunctives.Nearumm.FileName, conjunctives.Closetoum.FileName, conjunctives.Closetoum.FileName, conjunctives.Closetouhh.FileName }.PickRandom());
            ScannerList.Add(MyZone.ScannerValue);
            Subtitles += " ~s~near ~p~" + MyZone.DisplayName + "~s~";
            Notification.Text += "~n~Location: ~p~" + MyZone.DisplayName + "~s~";
            return true;
        }
        return false;
    }
    private static void AddDrivingVehicle(ref List<string> ScannerList, ref string Subtitles, ref DispatchNotification Notification, VehicleExt VehicleToReport, ReportType ReportedBy)
    {
        if (VehicleToReport == null)
        {
            VehicleToReport = PlayerState.CurrentVehicle;
        }
        if (VehicleToReport != null && !VehicleToReport.HasBeenDescribedByDispatch)
        {
            if (PlayerState.IsWanted)
            {
                ScannerList.Add(suspect_last_seen.SuspectSpotted.FileName);
                Subtitles += "Suspect spotted driving a";
            }
            else
            {
                ScannerList.Add(suspect_last_seen.TargetLastReported.FileName);
                Subtitles += "Target last reported driving a";
            }
            ScannerList.Add(new List<string>() { conjunctives.Drivinga.FileName }.PickRandom());
            AddVehicleDescription(VehicleToReport, ref ScannerList, ReportedBy == ReportType.Civilians, ref Subtitles, ref Notification, false, true, false, ReportedBy == ReportType.Officers);
        }
    }
    private static string GetSimpleCompassHeading()
    {
        float Heading = Game.LocalPlayer.Character.Heading;
        string Abbreviation;

        //yeah could be simpler, whatever idk computers are fast
        if (Heading >= 354.375f || Heading <= 5.625f) { Abbreviation = "N"; }
        else if (Heading >= 5.625f && Heading <= 16.875f) { Abbreviation = "N"; }
        else if (Heading >= 16.875f && Heading <= 28.125f) { Abbreviation = "N"; }
        else if (Heading >= 28.125f && Heading <= 39.375f) { Abbreviation = "N"; }
        else if (Heading >= 39.375f && Heading <= 50.625f) { Abbreviation = "N"; }
        else if (Heading >= 50.625f && Heading <= 61.875f) { Abbreviation = "N"; }
        else if (Heading >= 61.875f && Heading <= 73.125f) { Abbreviation = "E"; }
        else if (Heading >= 73.125f && Heading <= 84.375f) { Abbreviation = "E"; }
        else if (Heading >= 84.375f && Heading <= 95.625f) { Abbreviation = "E"; }
        else if (Heading >= 95.625f && Heading <= 106.875f) { Abbreviation = "E"; }
        else if (Heading >= 106.875f && Heading <= 118.125f) { Abbreviation = "E"; }
        else if (Heading >= 118.125f && Heading <= 129.375f) { Abbreviation = "S"; }
        else if (Heading >= 129.375f && Heading <= 140.625f) { Abbreviation = "S"; }
        else if (Heading >= 140.625f && Heading <= 151.875f) { Abbreviation = "S"; }
        else if (Heading >= 151.875f && Heading <= 163.125f) { Abbreviation = "S"; }
        else if (Heading >= 163.125f && Heading <= 174.375f) { Abbreviation = "S"; }
        else if (Heading >= 174.375f && Heading <= 185.625f) { Abbreviation = "S"; }
        else if (Heading >= 185.625f && Heading <= 196.875f) { Abbreviation = "S"; }
        else if (Heading >= 196.875f && Heading <= 208.125f) { Abbreviation = "S"; }
        else if (Heading >= 208.125f && Heading <= 219.375f) { Abbreviation = "S"; }
        else if (Heading >= 219.375f && Heading <= 230.625f) { Abbreviation = "S"; }
        else if (Heading >= 230.625f && Heading <= 241.875f) { Abbreviation = "S"; }
        else if (Heading >= 241.875f && Heading <= 253.125f) { Abbreviation = "W"; }
        else if (Heading >= 253.125f && Heading <= 264.375f) { Abbreviation = "W"; }
        else if (Heading >= 264.375f && Heading <= 275.625f) { Abbreviation = "W"; }
        else if (Heading >= 275.625f && Heading <= 286.875f) { Abbreviation = "W"; }
        else if (Heading >= 286.875f && Heading <= 298.125f) { Abbreviation = "W"; }
        else if (Heading >= 298.125f && Heading <= 309.375f) { Abbreviation = "N"; }
        else if (Heading >= 309.375f && Heading <= 320.625f) { Abbreviation = "N"; }
        else if (Heading >= 320.625f && Heading <= 331.875f) { Abbreviation = "N"; }
        else if (Heading >= 331.875f && Heading <= 343.125f) { Abbreviation = "N"; }
        else if (Heading >= 343.125f && Heading <= 354.375f) { Abbreviation = "N"; }
        else if (Heading >= 354.375f || Heading <= 5.625f) { Abbreviation = "N"; }
        else { Abbreviation = ""; }

        return Abbreviation;
    }
    public static bool ReportHeadingAndStreet(ref List<string> ScannerList, ref string Subtitles, ref DispatchNotification Notification)
    {
        if (PlayerLocation.PlayerCurrentStreet != null && PlayerLocation.PlayerCurrentStreet.DispatchFile != "")
        {
            ScannerList.Add((new List<string>() { suspect_heading.TargetLastSeenHeading.FileName, suspect_heading.TargetReportedHeading.FileName, suspect_heading.TargetSeenHeading.FileName, suspect_heading.TargetSpottedHeading.FileName }).PickRandom());
            Subtitles += "~r~Target spotted~s~ heading~s~";
            Notification.Text += "~n~Heading";
            string heading = GetSimpleCompassHeading();
            if (heading == "N")
            {
                ScannerList.Add(direction_heading.North.FileName);
                Subtitles += " North";
                Notification.Text += " North";
            }
            else if (heading == "S")
            {
                ScannerList.Add(direction_heading.South.FileName);
                Subtitles += " South";
                Notification.Text += " South";
            }
            else if (heading == "E")
            {
                ScannerList.Add(direction_heading.East.FileName);
                Subtitles += " East";
                Notification.Text += " East";
            }
            else if (heading == "W")
            {
                ScannerList.Add(direction_heading.West.FileName);
                Subtitles += " West";
                Notification.Text += " West";
            }

            if (PlayerLocation.PlayerCurrentStreet != null)
            {
                Street MyStreet = PlayerLocation.PlayerCurrentStreet;
                if (MyStreet != null && MyStreet.DispatchFile != "")
                {
                    ScannerList.Add((new List<string>() { conjunctives.On.FileName, conjunctives.On1.FileName, conjunctives.On2.FileName, conjunctives.On3.FileName, conjunctives.On4.FileName }).PickRandom());
                    ScannerList.Add(MyStreet.DispatchFile);
                    Subtitles += " ~s~on ~HUD_COLOUR_YELLOWLIGHT~" + MyStreet.Name + "~s~";
                    Notification.Text += " ~s~on ~HUD_COLOUR_YELLOWLIGHT~" + MyStreet.Name + "~s~";

                    if (PlayerLocation.PlayerCurrentCrossStreet != null)
                    {
                        Street MyCrossStreet = PlayerLocation.PlayerCurrentCrossStreet;
                        if (MyCrossStreet != null && MyCrossStreet.DispatchFile != "")
                        {
                            ScannerList.Add((new List<string>() { conjunctives.AT01.FileName, conjunctives.AT02.FileName }).PickRandom());
                            ScannerList.Add(MyCrossStreet.DispatchFile);
                            Subtitles += " ~s~at ~HUD_COLOUR_YELLOWLIGHT~" + MyCrossStreet.Name + "~s~";
                            Notification.Text += " ~s~at ~HUD_COLOUR_YELLOWLIGHT~" + MyCrossStreet.Name + "~s~";
                        }
                    }
                    return true;
                }
            }
            return false;
        }
        else
        {
            return false;
        }
    }
    private static void MarkVehicleAsStolen(VehicleExt StolenCar)
    {
        GameFiber ReportStolenVehicle = GameFiber.StartNew(delegate
        {
            GameFiber.Sleep(10000);
            StolenCar.WasReportedStolen = true;
            if (StolenCar.CarPlate != null && StolenCar.CarPlate.PlateNumber == StolenCar.OriginalLicensePlate.PlateNumber) //if you changed it between when it was reported, dont count it
                StolenCar.CarPlate.IsWanted = true;

            foreach (LicensePlate Plate in LicensePlateTheft.SpareLicensePlates)
            {
                if (Plate.PlateNumber == StolenCar.OriginalLicensePlate.PlateNumber)
                {
                    Plate.IsWanted = true;
                }
            }
            Debugging.WriteToLog("StolenVehicles", String.Format("Vehicle {0} was just reported stolen", StolenCar.VehicleEnt.Handle));

        }, "PlayDispatchQueue");
        Debugging.GameFibers.Add(ReportStolenVehicle);
    }
    private static string GetNotificationSubtitle(ReportType ReportedBy)
    {
        string NotificationTitle;
        if (ReportedBy == ReportType.Officers)
            NotificationTitle = "~o~Crime Observed~s~";
        else
            NotificationTitle = "~y~Crime Reported~s~";

        return NotificationTitle;
    }
    public static System.Drawing.Color GetBaseColor(System.Drawing.Color PrimaryColor)
    {
        List<System.Drawing.Color> BaseColorList = new List<System.Drawing.Color>
        {
            System.Drawing.Color.Red,
            System.Drawing.Color.Aqua,
            System.Drawing.Color.Beige,
            System.Drawing.Color.Black,
            System.Drawing.Color.Blue,
            System.Drawing.Color.Brown,
            System.Drawing.Color.DarkBlue,
            System.Drawing.Color.DarkGreen,
            System.Drawing.Color.DarkGray,
            System.Drawing.Color.DarkOrange,
            System.Drawing.Color.DarkRed,
            System.Drawing.Color.Gold,
            System.Drawing.Color.Green,
            System.Drawing.Color.Gray,
            System.Drawing.Color.LightBlue,
            System.Drawing.Color.Maroon,
            System.Drawing.Color.Orange,
            System.Drawing.Color.Pink,
            System.Drawing.Color.Purple,
            System.Drawing.Color.Silver,
            System.Drawing.Color.White,
            System.Drawing.Color.Yellow
        };

        System.Drawing.Color MyColor = PrimaryColor;

        int Index = Extensions.ClosestColor2(BaseColorList, MyColor);

        return BaseColorList[Index];
    }
    public static string GetManufacturerScannerFile(Vehicles.Manufacturer manufacturer)
    {
        if (manufacturer == Vehicles.Manufacturer.Albany)
            return DispatchScannerFiles.manufacturer.ALBANY01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Annis)
            return DispatchScannerFiles.manufacturer.ANNIS01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Benefactor)
            return DispatchScannerFiles.manufacturer.BENEFACTOR01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Burgerfahrzeug)
            return DispatchScannerFiles.manufacturer.BF01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Bollokan)
            return DispatchScannerFiles.manufacturer.BOLLOKAN01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Bravado)
            return DispatchScannerFiles.manufacturer.BRAVADO01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Brute)
            return DispatchScannerFiles.manufacturer.BRUTE01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Buckingham)
            return "";
        else if (manufacturer == Vehicles.Manufacturer.Canis)
            return DispatchScannerFiles.manufacturer.CANIS01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Chariot)
            return DispatchScannerFiles.manufacturer.CHARIOT01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Cheval)
            return DispatchScannerFiles.manufacturer.CHEVAL01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Classique)
            return DispatchScannerFiles.manufacturer.CLASSIQUE01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Coil)
            return DispatchScannerFiles.manufacturer.COIL01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Declasse)
            return DispatchScannerFiles.manufacturer.DECLASSE01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Dewbauchee)
            return DispatchScannerFiles.manufacturer.DEWBAUCHEE01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Dinka)
            return DispatchScannerFiles.manufacturer.DINKA01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.DUDE)
            return "";
        else if (manufacturer == Vehicles.Manufacturer.Dundreary)
            return DispatchScannerFiles.manufacturer.DUNDREARY01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Emperor)
            return DispatchScannerFiles.manufacturer.EMPEROR01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Enus)
            return DispatchScannerFiles.manufacturer.ENUS01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Fathom)
            return DispatchScannerFiles.manufacturer.FATHOM01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Gallivanter)
            return DispatchScannerFiles.manufacturer.GALLIVANTER01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Grotti)
            return DispatchScannerFiles.manufacturer.GROTTI01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.HVY)
            return DispatchScannerFiles.manufacturer.HVY01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Hijak)
            return DispatchScannerFiles.manufacturer.HIJAK01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Imponte)
            return DispatchScannerFiles.manufacturer.IMPONTE01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Invetero)
            return DispatchScannerFiles.manufacturer.INVETERO01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.JackSheepe)
            return DispatchScannerFiles.manufacturer.JACKSHEEPE01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Jobuilt)
            return DispatchScannerFiles.manufacturer.JOEBUILT01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Karin)
            return DispatchScannerFiles.manufacturer.KARIN01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.KrakenSubmersibles)
            return "";
        else if (manufacturer == Vehicles.Manufacturer.Lampadati)
            return DispatchScannerFiles.manufacturer.LAMPADATI01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.LibertyChopShop)
            return "";
        else if (manufacturer == Vehicles.Manufacturer.LibertyCityCycles)
            return "";
        else if (manufacturer == Vehicles.Manufacturer.MaibatsuCorporation)
            return DispatchScannerFiles.manufacturer.MAIBATSU01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Mammoth)
            return DispatchScannerFiles.manufacturer.MAMMOTH01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.MTL)
            return DispatchScannerFiles.manufacturer.MTL01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Nagasaki)
            return DispatchScannerFiles.manufacturer.NAGASAKI01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Obey)
            return DispatchScannerFiles.manufacturer.OBEY01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Ocelot)
            return DispatchScannerFiles.manufacturer.OCELOT01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Overflod)
            return DispatchScannerFiles.manufacturer.OVERFLOD01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Pegassi)
            return DispatchScannerFiles.manufacturer.PEGASI01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Pfister)
            return "";
        else if (manufacturer == Vehicles.Manufacturer.Principe)
            return DispatchScannerFiles.manufacturer.PRINCIPE01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Progen)
            return DispatchScannerFiles.manufacturer.PROGEN01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.ProLaps)
            return "";
        else if (manufacturer == Vehicles.Manufacturer.RUNE)
            return "";
        else if (manufacturer == Vehicles.Manufacturer.Schyster)
            return DispatchScannerFiles.manufacturer.SCHYSTER01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Shitzu)
            return DispatchScannerFiles.manufacturer.SHITZU01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Speedophile)
            return DispatchScannerFiles.manufacturer.SPEEDOPHILE01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Stanley)
            return DispatchScannerFiles.manufacturer.STANLEY01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.SteelHorse)
            return DispatchScannerFiles.manufacturer.STEELHORSE01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Truffade)
            return DispatchScannerFiles.manufacturer.TRUFFADE01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Ubermacht)
            return DispatchScannerFiles.manufacturer.UBERMACHT01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Vapid)
            return DispatchScannerFiles.manufacturer.VAPID01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Vulcar)
            return DispatchScannerFiles.manufacturer.VULCAR01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Vysser)
            return "";
        else if (manufacturer == Vehicles.Manufacturer.Weeny)
            return DispatchScannerFiles.manufacturer.WEENY01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.WesternCompany)
            return DispatchScannerFiles.manufacturer.WESTERNCOMPANY01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.WesternMotorcycleCompany)
            return DispatchScannerFiles.manufacturer.WESTERNMOTORCYCLECOMPANY01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Willard)
            return "";
        else if (manufacturer == Vehicles.Manufacturer.Zirconium)
            return DispatchScannerFiles.manufacturer.ZIRCONIUM01.FileName;
        else
            return "";
    }
    public static string GetVehicleClassScannerFile(Vehicles.VehicleClass myVehicleClass)
    {
        if (myVehicleClass == Vehicles.VehicleClass.Boats)
            return vehicle_category.Boat01.FileName;
        else if (myVehicleClass == Vehicles.VehicleClass.Commercial)
            return "";
        else if (myVehicleClass == Vehicles.VehicleClass.Compacts)
            return vehicle_category.Sedan.FileName;
        else if (myVehicleClass == Vehicles.VehicleClass.Coupes)
            return vehicle_category.Coupe01.FileName;
        else if (myVehicleClass == Vehicles.VehicleClass.Cycles)
            return vehicle_category.Bicycle01.FileName;
        else if (myVehicleClass == Vehicles.VehicleClass.Emergency)
            return "";
        else if (myVehicleClass == Vehicles.VehicleClass.Helicopters)
            return vehicle_category.Helicopter01.FileName;
        else if (myVehicleClass == Vehicles.VehicleClass.Industrial)
            return vehicle_category.IndustrialVehicle01.FileName;
        else if (myVehicleClass == Vehicles.VehicleClass.Military)
            return "";
        else if (myVehicleClass == Vehicles.VehicleClass.Motorcycles)
            return vehicle_category.Motorcycle01.FileName;
        else if (myVehicleClass == Vehicles.VehicleClass.Muscle)
            return vehicle_category.MuscleCar01.FileName;
        else if (myVehicleClass == Vehicles.VehicleClass.OffRoad)
            return vehicle_category.OffRoad01.FileName;
        else if (myVehicleClass == Vehicles.VehicleClass.Planes)
            return "";
        else if (myVehicleClass == Vehicles.VehicleClass.Sedans)
            return vehicle_category.Sedan.FileName;
        else if (myVehicleClass == Vehicles.VehicleClass.Service)
            return vehicle_category.Service01.FileName;
        else if (myVehicleClass == Vehicles.VehicleClass.Sports)
            return vehicle_category.SportsCar01.FileName;
        else if (myVehicleClass == Vehicles.VehicleClass.SportsClassics)
            return vehicle_category.Classic01.FileName;
        else if (myVehicleClass == Vehicles.VehicleClass.Super)
            return vehicle_category.PerformanceCar01.FileName;
        else if (myVehicleClass == Vehicles.VehicleClass.SUVs)
            return vehicle_category.SUV01.FileName;
        else if (myVehicleClass == Vehicles.VehicleClass.Trailer)
            return "";
        else if (myVehicleClass == Vehicles.VehicleClass.Trains)
            return vehicle_category.Train01.FileName;
        else if (myVehicleClass == Vehicles.VehicleClass.Unknown)
            return "";
        else if (myVehicleClass == Vehicles.VehicleClass.Utility)
            return vehicle_category.UtilityVehicle01.FileName;
        else if (myVehicleClass == Vehicles.VehicleClass.Vans)
            return vehicle_category.Van01.FileName;
        else
            return "";
    }
    private static void AddLethalForceAuthorized(ref List<string> ScannerList, ref string Subtitles, ref DispatchNotification Notification)
    {
        if (!ReportedLethalForceAuthorized)
        {
            ScannerList.Add(new List<string>() { lethal_force.Useofdeadlyforceauthorized.FileName, lethal_force.Useofdeadlyforceisauthorized.FileName, lethal_force.Useofdeadlyforceisauthorized1.FileName, lethal_force.Useoflethalforceisauthorized.FileName, lethal_force.Useofdeadlyforcepermitted1.FileName }.PickRandom());
            Subtitles += " use of ~r~Deadly Force~s~ is authorized";
            Notification.Text += "~n~Use of ~r~Deadly Force~s~ is authorized";
            ReportedLethalForceAuthorized = true;
        }
    }
    public static void AddVehicleDescription(VehicleExt VehicleDescription, ref List<string> ScannerList, bool IncludeLicensePlate, ref string Subtitles, ref DispatchNotification Notification, bool IncludeAAudio, bool IncludePoliceDescription, bool IncludeIn, bool AddPossiblyStolenStolen)
    {
        if (VehicleDescription == null)
            return;
        if (VehicleDescription.HasBeenDescribedByDispatch)
            return;
        else
            VehicleDescription.HasBeenDescribedByDispatch = true;

        Notification.Text += "~n~Vehicle:~s~";

        if (VehicleDescription.VehicleEnt.IsPoliceVehicle)
        {
            if (IncludePoliceDescription)
            {
                Subtitles += " ~r~Stolen Police Car~s~";
                Notification.Text += " ~r~Stolen Police Car~s~";
                ScannerList.Add(vehicle_category.PoliceSedan01.FileName);
            }
        }
        else
        {
            if (IncludeIn)
            {
                ScannerList.Add(new List<string>() { conjunctives.Inuhh2.FileName, conjunctives.Inuhh3.FileName }.PickRandom());
                if (VehicleDescription.IsStolen & AddPossiblyStolenStolen)
                {

                    ScannerList.Add(crime_stolen_vehicle.Apossiblestolenvehicle.FileName);
                    Subtitles += " ~s~in a possible stolen vehicle~s~";
                    Notification.Text += " ~r~Stolen~s~";
                }
            }

            Vehicles.VehicleInfo VehicleInformation = Vehicles.GetVehicleInfo(VehicleDescription);
            System.Drawing.Color BaseColor = GetBaseColor(VehicleDescription.DescriptionColor);
            ColorLookup LookupColor = ColorLookups.Where(x => x.BaseColor == BaseColor).PickRandom();
            string ManufacturerScannerFile;
            string VehicleClassScannerFile;
            if (VehicleInformation != null)
            {
                Debugging.WriteToLog("Description", string.Format("VehicleInformation.ModelScannerFile {0}", VehicleInformation.ModelScannerFile.ToString()));
                ManufacturerScannerFile = GetManufacturerScannerFile(VehicleInformation.Manufacturer);
                VehicleClassScannerFile = GetVehicleClassScannerFile(VehicleInformation.VehicleClass);
                if (LookupColor != null && (VehicleInformation.ModelScannerFile != "" || VehicleInformation.ModelScannerFile != "" || VehicleClassScannerFile != ""))
                {
                    if (IncludeAAudio)
                    {
                        Subtitles += " ~s~a~s~";
                        ScannerList.Add(new List<string>() { conjunctives.A01.FileName, conjunctives.A02.FileName }.PickRandom());
                    }
                    if (VehicleInformation.VehicleClass != Vehicles.VehicleClass.Emergency)
                    {
                        Subtitles += " ~s~" + LookupColor.BaseColor.Name + "~s~";
                        Notification.Text += " ~s~" + LookupColor.BaseColor.Name + "~s~";
                        ScannerList.Add(LookupColor.ScannerFile);
                    }
                    if (ManufacturerScannerFile != "")
                    {
                        Subtitles += " ~p~" + VehicleInformation.Manufacturer + "~s~";
                        Notification.Text += " ~p~" + VehicleInformation.Manufacturer + "~s~";
                        ScannerList.Add(ManufacturerScannerFile);
                    }
                    if (VehicleInformation.ModelScannerFile != "")
                    {
                        string ModelName = VehicleInformation.Name.ToLower();
                        unsafe
                        {
                            IntPtr ptr = NativeFunction.CallByName<IntPtr>("GET_DISPLAY_NAME_FROM_VEHICLE_MODEL", VehicleDescription.VehicleEnt.Model.Hash);
                            ModelName = Marshal.PtrToStringAnsi(ptr);
                        }
                        unsafe
                        {
                            IntPtr ptr2 = NativeFunction.CallByHash<IntPtr>(0x7B5280EBA9840C72, ModelName);
                            ModelName = Marshal.PtrToStringAnsi(ptr2);
                        }
                        if (ModelName == "CARNOTFOUND" || ModelName == "NULL")
                            ModelName = VehicleInformation.Name.ToLower();

                        Subtitles += " ~g~" + ModelName + "~s~";
                        Notification.Text += " ~g~" + ModelName + "~s~";
                        ScannerList.Add(VehicleInformation.ModelScannerFile);
                    }
                    else if (VehicleClassScannerFile != "")
                    {
                        string subText = Vehicles.GetVehicleTypeSubtitle(VehicleInformation.VehicleClass);
                        Subtitles += " ~b~" + subText + "~s~";
                        Notification.Text += " ~b~" + subText + "~s~";
                        ScannerList.Add(VehicleClassScannerFile);
                    }
                }
            }
        }
        if (IncludeLicensePlate)
        {
            ScannerList.Add(suspect_license_plate.SuspectsLicensePlate.FileName);
            Subtitles += "~s~. Suspects License Plate: ~y~" + VehicleDescription.OriginalLicensePlate.PlateNumber.ToUpper() + "~s~";//VehicleDescription.VehicleEnt.LicensePlate.ToUpper() + "~s~";
            Notification.Text += "~n~License Plate: ~y~" + VehicleDescription.OriginalLicensePlate.PlateNumber.ToUpper() + "~s~";
            foreach (char c in VehicleDescription.OriginalLicensePlate.PlateNumber)
            {
                string DispatchFileName = LettersAndNumbersLookup.Where(x => x.AlphaNumeric == c).PickRandom().ScannerFile;
                ScannerList.Add(DispatchFileName);
            }
        }
    }
    public static void AddVehicleClass(VehicleExt VehicleDescription, ref List<string> ScannerList, ref string Subtitles, ref DispatchNotification Notification)
    {
        if (VehicleDescription != null)
        {
            Vehicles.VehicleInfo VehicleInformation = Vehicles.GetVehicleInfo(VehicleDescription);
            if (VehicleInformation != null)
            {
                string VehicleClassScannerFile = GetVehicleClassScannerFile(VehicleInformation.VehicleClass);
                if (VehicleClassScannerFile != "")
                {
                    string subText = Vehicles.GetVehicleTypeSubtitle(VehicleInformation.VehicleClass);
                    Subtitles += " driving a ~b~" + subText + "~s~";
                    Notification.Text += "~n~Vehicle: ~b~" + subText + "~s~";
                    ScannerList.Add(conjunctives.Drivinga.FileName);
                    ScannerList.Add(VehicleClassScannerFile);
                    return;
                }
            }
        }
        Notification.Text += "~n~Vehicle: Unknown";
        Subtitles += " driving a um";
        ScannerList.Add(conjunctives.DrivingAUmmm.FileName);
    }
    public static string GetVehicleDisplayName(Vehicle VehicleDescription)
    {
        string ModelName;
        unsafe
        {
            IntPtr ptr = NativeFunction.CallByName<IntPtr>("GET_DISPLAY_NAME_FROM_VEHICLE_MODEL", VehicleDescription.Model.Hash);
            ModelName = Marshal.PtrToStringAnsi(ptr);
        }
        unsafe
        {
            IntPtr ptr2 = NativeFunction.CallByHash<IntPtr>(0x7B5280EBA9840C72, ModelName);
            ModelName = Marshal.PtrToStringAnsi(ptr2);
        }
        if (ModelName == "CARNOTFOUND" || ModelName == "NULL")
            ModelName = "";

        return ModelName;
    }
    public static void AddSpeed(ref List<string> ScannerList, float Speed, ref string Subtitles, ref DispatchNotification Notification)
    {
        if (Speed >= 40f)
        {
            ScannerList.Add(suspect_last_seen.TargetLastReported.FileName);
            Subtitles += " ~s~target last reported~s~";
            if (Speed >= 40f && Speed < 50f)
            {
                ScannerList.Add(doing_speed.Doing40mph.FileName);
                Subtitles += " ~s~doing ~o~40 mph~s~";
                Notification.Text += "~n~Speed Exceeding: ~o~40 mph~s~";
            }
            else if (Speed >= 50f && Speed < 60f)
            {
                ScannerList.Add(doing_speed.Doing50mph.FileName);
                Subtitles += " ~s~doing ~o~50 mph~s~";
                Notification.Text += "~n~Speed Exceeding: ~o~50 mph~s~";
            }
            else if (Speed >= 60f && Speed < 70f)
            {
                ScannerList.Add(doing_speed.Doing60mph.FileName);
                Subtitles += " ~s~doing ~o~60 mph~s~";
                Notification.Text += "~n~Speed Exceeding: ~o~60 mph~s~";
            }
            else if (Speed >= 70f && Speed < 80f)
            {
                ScannerList.Add(doing_speed.Doing70mph.FileName);
                Subtitles += " ~s~doing ~o~70 mph~s~";
                Notification.Text += "~n~Speed Exceeding: ~o~70 mph~s~";
            }
            else if (Speed >= 80f && Speed < 90f)
            {
                ScannerList.Add(doing_speed.Doing80mph.FileName);
                Subtitles += " ~s~doing ~o~80 mph~s~";
                Notification.Text += "~n~Speed Exceeding: ~o~80 mph~s~";
            }
            else if (Speed >= 90f && Speed < 100f)
            {
                ScannerList.Add(doing_speed.Doing90mph.FileName);
                Subtitles += " ~s~doing ~o~90 mph~s~";
                Notification.Text += "~n~Speed Exceeding: ~o~90 mph~s~";
            }
            else if (Speed >= 100f && Speed < 104f)
            {
                ScannerList.Add(doing_speed.Doing100mph.FileName);
                Subtitles += " ~s~doing ~o~100 mph~s~";
                Notification.Text += "~n~Speed Exceeding: ~o~100 mph~s~";
            }
            else if (Speed >= 105f)
            {
                ScannerList.Add(doing_speed.Doingover100mph.FileName);
                Subtitles += " ~s~doing ~o~over 100 mph~s~";
                Notification.Text += "~n~Speed Exceeding: ~o~105 mph~s~";
            }
        }
    }
    public static void AbortAllAudio()
    {
        DispatchQueue.Clear();
        if (AudioPlaying)
        {
            CancelAudio = true;
            outputDevice.Stop();
        }
        DispatchQueue.Clear();
        if (AudioPlaying)
        {
            CancelAudio = true;
            outputDevice.Stop();
        }
        DispatchQueue.Clear();

        RemoveAllNotifications();
    }
    public static void ResetReportedItems()
    {
        ReportedWeaponsFree = false;
        ReportedLethalForceAuthorized = false;
        ReportedOfficerDown = false;
        ReportedMilitaryDeployed = false;
        ReportedAirSupportRequested = false;
    }
    private static void RemoveAllNotifications()
    {
        foreach (uint handles in NotificationHandles)
        {
            Game.RemoveNotification(handles);
        }
        NotificationHandles.Clear();
    }

    public class Dispatch
    {


        public float Speed { get; set; }
        public GTAWeapon WeaponToReport { get; set; }
        public VehicleExt VehicleToReport { get; set; }
        public bool SuspectStatusOnFoot { get; set; } = true;
        public ReportType ReportedBy { get; set; } = ReportType.Officers;


        
        public bool IncludeAttention { get; set; } = false;
        public string NotificationTitle { get; set; } = "Police Scanner";
        public string NotificationSubtitle { get; set; }
        public string NotificationText { get; set; }
        public List<AudioSet> MainAudioSet { get; set; } = new List<AudioSet>();
        public List<AudioSet> SecondaryAudioSet { get; set; } = new List<AudioSet>();
        public bool MarkVehicleAsStolen { get; set; } = false;
        public bool IncludeDrivingVehicle { get; set; } = false;
        public bool ReportCharctersPosition { get; set; } = true;


        public int Priority { get; set; } = 0;
        public bool ResultsInLethalForce { get; set; } = false;
        public bool ResultsInStolenCarSpotted { get; set; } = false;
        public bool IsTrafficViolation { get; set; } = false;
        public bool IsAmbient { get; set; } = false;
        public int ResultingWantedLevel { get; set; }


        public Dispatch()
        {

        }
        public class DispatchAudioList
        {
            public List<string> AudioList { get; set; } = new List<string>();
            public DispatchAudioList()
            {

            }
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
        //public class DispatchNotification
        //{

        //    public string NotificationSubtitleAuto
        //    {
        //        get
        //        {
        //            if (ReportedBy == ReportType.Officers)
        //                return "~o~Crime Observed~s~";
        //            else
        //                return "~y~Crime Reported~s~";
        //        }
        //    }
        //}
    }




    public class DispatchAudioEvent
    {
        public List<string> SoundsToPlay;
        public string Subtitles = "";
        public DispatchNotification NotificationToDisplay;
        public bool CanBeInterrupted = true;
        public bool CanInterrupt = true;
        public DispatchAudioEvent(List<string> _SoundsToPlay)
        {
            SoundsToPlay = _SoundsToPlay;
        }
        public DispatchAudioEvent(List<string> _SoundsToPlay, string _Subtitles)
        {
            SoundsToPlay = _SoundsToPlay;
            Subtitles = _Subtitles;
        }
        public DispatchAudioEvent(List<string> _SoundsToPlay, string _Subtitles, DispatchNotification _NotificationToDisplay)
        {
            SoundsToPlay = _SoundsToPlay;
            Subtitles = _Subtitles;
            NotificationToDisplay = _NotificationToDisplay;
        }
        public DispatchAudioEvent(List<string> _SoundsToPlay, string _Subtitles, DispatchNotification _NotificationToDisplay, bool _CanBeInterrupted, bool _CanInterrupt)
        {
            SoundsToPlay = _SoundsToPlay;
            Subtitles = _Subtitles;
            NotificationToDisplay = _NotificationToDisplay;
            CanBeInterrupted = _CanBeInterrupted;
            CanInterrupt = _CanInterrupt;
        }
    }

    public class DispatchNotification
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Text { get; set; }
        public string TextureDict { get; set; } = "CHAR_CALL911";
        public string TextureName { get; set; } = "CHAR_CALL911";
        public DispatchNotification(string _Title, string _Subtitle, string _Text)
        {
            Title = _Title;
            Subtitle = _Subtitle;
            Text = _Text;
        }
    }

    public class DispatchLettersNumber
    {
        public char AlphaNumeric { get; set; }
        public string ScannerFile { get; set; }

        public DispatchLettersNumber(char _AlphaNumeric, string _ScannerFile)
        {
            AlphaNumeric = _AlphaNumeric;
            ScannerFile = _ScannerFile;
        }

    }
    public class DispatchQueueItem
    {
        public enum DispatchType
        {
            TrafficViolation = 1,
            FelonyCrimes = 2,
            AnnounceStolen = 3,
            AnnounceLost = 4,
        }
        //public AvailableDispatch Type { get; set; }
        public int Priority { get; set; } = 0;
        public bool ResultsInLethalForce { get; set; } = false;
        public bool ResultsInStolenCarSpotted { get; set; } = false;
        public bool IsTrafficViolation { get; set; } = false;
        public bool IsAmbient { get; set; } = false;
        public float Speed { get; set; }
        public int ResultingWantedLevel { get; set; }
        public GTAWeapon WeaponToReport { get; set; }
        public VehicleExt VehicleToReport { get; set; }
        public bool SuspectStatusOnFoot { get; set; } = true;
        public ReportType ReportedBy { get; set; } = ReportType.Officers;
        public bool CanBeInterrupted
        {
            get
            {
                if (ReportedBy == ReportType.Civilians)
                    return true;
                else
                    return false;
            }
        }
        //public DispatchQueueItem(AvailableDispatch _Type, int _Priority)
        //{
        //    Type = _Type;
        //    Priority = _Priority;
        //}
        //public DispatchQueueItem(AvailableDispatch _Type, int _Priority, bool _ResultsInLethalForce)
        //{
        //    Type = _Type;
        //    Priority = _Priority;
        //    ResultsInLethalForce = _ResultsInLethalForce;
        //}
    }
    public class VehicleModelNameLookup
    {
        public string ModelName { get; set; }
        public string ScannerFile { get; set; }
        public string ManufacturerScannerFile { get; set; } = "";

        public VehicleModelNameLookup(string _ScannerFile, string _ModelName)
        {
            ModelName = _ModelName;
            ScannerFile = _ScannerFile;
        }
        public VehicleModelNameLookup(string _ScannerFile, string _ModelName, string _ManufacturerScannerFile)
        {
            ModelName = _ModelName;
            ScannerFile = _ScannerFile;
            ManufacturerScannerFile = _ManufacturerScannerFile;
        }

    }
    public class ColorLookup
    {
        public System.Drawing.Color BaseColor { get; set; }
        public string ScannerFile { get; set; }

        public ColorLookup(string _ScannerFile, System.Drawing.Color _BaseColor)
        {
            BaseColor = _BaseColor;
            ScannerFile = _ScannerFile;
        }

    }
}

