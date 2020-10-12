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

public static class DispatchAudio
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
    public enum AvailableDispatch
    {
        ShootingAtPolice = 0,
        CarryingWeapon = 1,
        OfficerDown = 2,
        AssaultingOfficer = 3,
        ThreateningOfficerWithFirearm = 4,
        SuspectEvadedOfficers = 5,
        SuspectArrested = 6,
        SuspectWasted = 7,
        LethalForceAuthorized = 8,
        ReportStolenVehicle = 9,
        SuspectLost = 10,
        SpottedStolenCar = 11,
        PedestrianHitAndRun = 12,
        VehicleHitAndRun = 13,
        RecklessDriving = 14,
        FelonySpeeding = 15,
        WeaponsFree = 16,
        SuspiciousActivity = 17,
        SuspiciousVehicle = 18,
        GrandTheftAuto = 19,
        SuspectReacquired = 20,
        RequestBackup = 21,
        RunningARedLight = 22,
        SuspectSpotted = 23,
        CriminalActivity = 24,
        ShotsFired = 25,
        ResumePatrol = 27,
        LostVisualOnSuspect = 28,
        TerroistActivity = 29,
        TrespassingOnGovernmentProperty = 30,
        SuspectChangedVehicle = 31,
        CivlianFatality = 32,
        CivilianInjury = 33,
        CivilianShot = 34,
        StealingAirVehicle = 36,
        SuspectResisitingArrest = 37,
        CivilianMugged = 38,
        NoFurtherUnitsNeeded = 39,
        AttemptingSuicide = 40,
        MilitaryDeployed = 41,
        AirSupportRequested = 42,
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
    static DispatchAudio()
    {
        rnd = new Random();
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



        //DamagedScannerAliases.Add(extra_prefix.Battered.FileName);
       // DamagedScannerAliases.Add(extra_prefix.Beatup.FileName);
        DamagedScannerAliases.Add(extra_prefix.Damaged.FileName);
        // DamagedScannerAliases.Add(extra_prefix.Dented.FileName);
        // DamagedScannerAliases.Add(extra_prefix.Distressed.FileName);
        // DamagedScannerAliases.Add(extra_prefix.Rundown.FileName);
        // DamagedScannerAliases.Add(extra_prefix.Rundown1.FileName);


        Dispatch AttemptingSuicide = new Dispatch()
        {
            NotificationText = "Suicide Attempt",
            SubtitleText = " an ~r~Attempted Suicide~s~",
            WhoToNotify = AttentionType.Nobody,
            PossibleAudioToPlay = new List<Dispatch.DispatchAudioList>() {
                new Dispatch.DispatchAudioList(){ AudioList = new List<string>() { crime_9_14a_attempted_suicide.Anattemptedsuicide.FileName } },
                new Dispatch.DispatchAudioList(){ AudioList = new List<string>() { crime_9_14a_attempted_suicide.Apossibleattemptedsuicide.FileName } }
            }
        };


    }
    private static void PlayAudioList(DispatchAudioEvent MyAudioEvent)
    {
        /////////Maybe?
        if (MyAudioEvent.CanInterrupt && CurrentlyPlayingCanBeInterrupted)
        {
            //Debugging.WriteToLog("PlayAudioList", "Aborting Audio In the Middle");
            AbortAllAudio();
        }
        //Debugging.WriteToLog("PlayAudioList", string.Format("CanBeInterrupted:{0}CanInterrupt:{1}Name:{2}", MyAudioEvent.CanBeInterrupted,MyAudioEvent.CanInterrupt,MyAudioEvent.Subtitles));
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

            foreach (string audioname in MyAudioEvent.SoundsToPlay)
            {
                PlayAudio(audioname);

                while (IsPlayingAudio)
                {
                    if (MyAudioEvent.Subtitles != "" && General.MySettings.Police.DispatchSubtitles && Game.GameTime - GameTimeLastDisplayedSubtitle >= 1500)
                    {
                        Game.DisplaySubtitle(MyAudioEvent.Subtitles, 2000);
                        GameTimeLastDisplayedSubtitle = Game.GameTime;
                    }
                    GameFiber.Yield();
                }
                if (CancelAudio)
                {
                    CancelAudio = false;
                    //Debugging.WriteToLog("PlayAudioList", "CancelAudio Set to False");
                    break;
                }
                GameTimeLastAnnouncedDispatch = Game.GameTime;
            }
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
            Debugging.WriteToLog("PlayAudio",e.Message);
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
        return;
        if (!DispatchQueue.Any(x => x.Type == _ItemToAdd.Type))
        {
            DispatchQueue.Add(_ItemToAdd);
        }
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


            CheckEvents();




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
                            if (ItemToPlay.Priority <= LastCivilianReportedPriority)
                            {
                                //Debugging.WriteToLog("Ignoring Low Priority", ItemToPlay.Type.ToString());
                            }
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
                            //Debugging.WriteToLog("Playing", Item.Type.ToString());
                            if (Item.Type == AvailableDispatch.AssaultingOfficer)
                                AssaultingOfficer(Item);
                            else if (Item.Type == AvailableDispatch.CarryingWeapon)
                                CarryingWeapon(Item);
                            else if (Item.Type == AvailableDispatch.FelonySpeeding)
                                FelonySpeeding(Item);
                            else if (Item.Type == AvailableDispatch.LethalForceAuthorized)
                                LethalForceAuthorized(Item);
                            else if (Item.Type == AvailableDispatch.OfficerDown)
                                OfficerDown(Item);
                            else if (Item.Type == AvailableDispatch.PedestrianHitAndRun)
                                PedestrianHitAndRun(Item);
                            else if (Item.Type == AvailableDispatch.RecklessDriving)
                                RecklessDriving(Item);
                            else if (Item.Type == AvailableDispatch.ShootingAtPolice)
                                ShootingAtPolice(Item);
                            else if (Item.Type == AvailableDispatch.SpottedStolenCar)
                                SpottedStolenCar(Item);
                            else if (Item.Type == AvailableDispatch.ReportStolenVehicle)
                                ReportStolenVehicle(Item);
                            else if (Item.Type == AvailableDispatch.SuspectArrested)
                                SuspectArrested(Item);
                            else if (Item.Type == AvailableDispatch.SuspectEvadedOfficers)
                                SuspectEvadedOfficers(Item);
                            else if (Item.Type == AvailableDispatch.SuspectLost)
                                SuspectLost(Item);
                            else if (Item.Type == AvailableDispatch.SuspectWasted)
                                SuspectWasted(Item);
                            else if (Item.Type == AvailableDispatch.ThreateningOfficerWithFirearm)
                                ThreateningOfficerWithFirearm(Item);
                            else if (Item.Type == AvailableDispatch.VehicleHitAndRun)
                                VehicleHitAndRun(Item);
                            else if (Item.Type == AvailableDispatch.WeaponsFree)
                                WeaponsFree(Item);
                            else if (Item.Type == AvailableDispatch.SuspiciousActivity)
                                SuspiciousActivity(Item);
                            else if (Item.Type == AvailableDispatch.SuspiciousVehicle)
                                SuspiciousVehicle(Item);
                            else if (Item.Type == AvailableDispatch.GrandTheftAuto)
                                GrandTheftAuto(Item);
                            else if (Item.Type == AvailableDispatch.SuspectReacquired)
                                SuspectReacquired(Item);
                            else if (Item.Type == AvailableDispatch.RequestBackup)
                                RequestBackup(Item);
                            else if (Item.Type == AvailableDispatch.RunningARedLight)
                                RunningARedLight(Item);
                            else if (Item.Type == AvailableDispatch.SuspectSpotted)
                                SuspectSpotted(Item);
                            else if (Item.Type == AvailableDispatch.CriminalActivity)
                                CriminalActivity(Item);
                            else if (Item.Type == AvailableDispatch.ShotsFired)
                                ShotsFired(Item);
                            else if (Item.Type == AvailableDispatch.ResumePatrol)
                                ResumePatrol(Item);
                            else if (Item.Type == AvailableDispatch.LostVisualOnSuspect)
                                LostVisualOnSuspect(Item);
                            else if (Item.Type == AvailableDispatch.TerroistActivity)
                                TerroristActivity(Item);
                            else if (Item.Type == AvailableDispatch.TrespassingOnGovernmentProperty)
                                TrespassingOnGovernmentProperty(Item);
                            else if (Item.Type == AvailableDispatch.SuspectChangedVehicle)
                                SuspectChangedVehicle(Item);
                            else if (Item.Type == AvailableDispatch.CivlianFatality)
                                CivlianFatality(Item);
                            else if (Item.Type == AvailableDispatch.CivilianInjury)
                                CivilianInjury(Item);
                            else if (Item.Type == AvailableDispatch.CivilianShot)
                                CivilianShot(Item);
                            else if (Item.Type == AvailableDispatch.StealingAirVehicle)
                                StealingAirVehicle(Item);
                            else if (Item.Type == AvailableDispatch.SuspectResisitingArrest)
                                SuspectResisitingArrest(Item);
                            else if (Item.Type == AvailableDispatch.CivilianMugged)
                                CivilianMugged(Item);
                            else if (Item.Type == AvailableDispatch.NoFurtherUnitsNeeded)
                                NoFurtherUnitsNeeded(Item);
                            else if (Item.Type == AvailableDispatch.AttemptingSuicide)
                                AttemptingSuicide(Item);
                            else if (Item.Type == AvailableDispatch.MilitaryDeployed)
                                MilitaryDeployed(Item);
                            else if (Item.Type == AvailableDispatch.AirSupportRequested)
                                AirSupportRequested(Item);

                            if (Item.ResultingWantedLevel > 0)
                                WantedLevelScript.SetWantedLevel(Item.ResultingWantedLevel, string.Format("Set Wanted After Dispatch: {0}", Item.Type), true);
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
    private static void CheckEvents()
    {
        //if (PlayerState.WantedLevel == 5 && !ReportedMilitaryDeployed && PedList.CopPeds.Any(x => x.AssignedAgency.AgencyClassification == Agency.Classification.Military))
        //{
        //    AddDispatchToQueue(new DispatchQueueItem(AvailableDispatch.MilitaryDeployed, 1));
        //}
        //if (PlayerState.WantedLevel >= 2 && !ReportedAirSupportRequested && PedList.CopPeds.Any(x => x.IsInHelicopter))
        //{
        //    AddDispatchToQueue(new DispatchQueueItem(AvailableDispatch.AirSupportRequested, 1));
        //}
    }
    private static void Generic(Dispatch ItemToPlay)
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";

        if(ItemToPlay.NotificationSubtitle == "")
            ItemToPlay.NotificationSubtitle = GetNotificationSubtitle(ItemToPlay.ReportedBy);

        Vector3 PositionToReport = Game.LocalPlayer.Character.Position;
        if (ItemToPlay.ReportedBy == ReportType.Civilians)
            PositionToReport = Investigation.InvestigationPosition;

        DispatchNotification Notification = new DispatchNotification(ItemToPlay.NotificationTitle, ItemToPlay.NotificationSubtitle, ItemToPlay.NotificationText);
        ReportGenericStart(ref ScannerList, ref Subtitles, ItemToPlay.WhoToNotify, ItemToPlay.ReportedBy, PositionToReport);
        ScannerList.Concat(ItemToPlay.PossibleAudioToPlay.PickRandom().AudioList.ToList());
        Subtitles += ItemToPlay.SubtitleText;


        if(ItemToPlay.IncludeDrivingVehicle)
        {
            AddDrivingVehicle(ref ScannerList, ref Subtitles, ref Notification, ItemToPlay.VehicleToReport, ItemToPlay.ReportedBy);
        }




        ReportGenericEnd(ref ScannerList, NearType.Street, ref Subtitles, ref Notification, PositionToReport);
        bool CanBeInterrupted = ItemToPlay.ReportedBy == ReportType.Civilians ? true : false;
        PlayAudioList(new DispatchAudioEvent(ScannerList, Subtitles, Notification, CanBeInterrupted, !CanBeInterrupted));

        if (ItemToPlay.MarkVehicleAsStolen && ItemToPlay.VehicleToReport != null)
        {
            MarkVehicleAsStolen(ItemToPlay.VehicleToReport);
        }
    }

    private static bool CanRun(DispatchQueueItem ItemToPlay)
    {
        if (ItemToPlay.Type == AvailableDispatch.AssaultingOfficer && (ReportedLethalForceAuthorized || ReportedOfficerDown))
            return false;
        else
            return true;
    }
    private static void AttemptingSuicide(DispatchQueueItem ItemToPlay)
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        string NotificationTitle = GetNotificationSubtitle(ItemToPlay.ReportedBy);
        DispatchNotification Notification = new DispatchNotification("Police Scanner", NotificationTitle, "Suicide Attempt");
        ReportGenericStart(ref ScannerList, ref Subtitles, AttentionType.Nobody, ItemToPlay.ReportedBy, Game.LocalPlayer.Character.Position);
        ScannerList.Add(new List<string>() { crime_9_14a_attempted_suicide.Anattemptedsuicide.FileName, crime_9_14a_attempted_suicide.Apossibleattemptedsuicide.FileName }.PickRandom());
        Subtitles += " an ~r~Attempted Suicide~s~";
        ReportGenericEnd(ref ScannerList, NearType.Street, ref Subtitles, ref Notification, Game.LocalPlayer.Character.Position);
        PlayAudioList(new DispatchAudioEvent(ScannerList, Subtitles, Notification,ItemToPlay.CanBeInterrupted,false));
    }
    private static void AssaultingOfficer(DispatchQueueItem ItemToPlay)
    {
        if (ReportedLethalForceAuthorized || ReportedOfficerDown)
            return;
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        string NotificationTitle = GetNotificationSubtitle(ItemToPlay.ReportedBy);
        AttentionType WhoToNotifiy = GetWhoToNotify(ItemToPlay.ReportedBy);
        ReportGenericStart(ref ScannerList, ref Subtitles, WhoToNotifiy, ItemToPlay.ReportedBy, Game.LocalPlayer.Character.Position);
        Subtitles += " we have an ~r~Assault on an Officer~s~";
        DispatchNotification Notification = new DispatchNotification("Police Scanner", NotificationTitle, "Assault on an Officer");
        ScannerList.Add(new List<string>() { crime_assault_on_an_officer.Anassaultonanofficer.FileName, crime_assault_on_an_officer.Anofficerassault.FileName }.PickRandom());
        AddLethalForceAuthorized(ref ScannerList, ref Subtitles,ref Notification);
        ReportGenericEnd(ref ScannerList, NearType.Nothing, ref Subtitles, ref Notification, Game.LocalPlayer.Character.Position);
        PlayAudioList(new DispatchAudioEvent(ScannerList, Subtitles, Notification, ItemToPlay.CanBeInterrupted, true));
    }
    public static void CarryingWeapon(DispatchQueueItem ItemToPlay)
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        string NotificationTitle = GetNotificationSubtitle(ItemToPlay.ReportedBy);
        AttentionType WhoToNotifiy = GetWhoToNotify(ItemToPlay.ReportedBy);
        ReportGenericStart(ref ScannerList, ref Subtitles, WhoToNotifiy, ItemToPlay.ReportedBy, Game.LocalPlayer.Character.Position);
        
        DispatchNotification Notification = new DispatchNotification("Police Scanner", NotificationTitle, "Brandishing");
        if (ItemToPlay.ReportedBy == ReportType.Civilians)
        {
            if (ItemToPlay.WeaponToReport == null || ItemToPlay.WeaponToReport.Category == GTAWeapon.WeaponCategory.Melee)
            {
                ScannerList.Add(crime_suspect_armed_and_dangerous.Asuspectarmedanddangerous.FileName);
                Subtitles += " a suspect ~r~armed and dangerous~s~";
                Notification.Text += " Weapon: Melee";
            }
            else
            {
                ScannerList.Add(crime_firearms_possession.Afirearmspossession.FileName);
                Subtitles += " a ~r~firearms possession~s~";
                Notification.Text += " Weapon: Firearm";
            }
        }
        else
        {
            Notification.Text += "~n~Weapon:~s~";
            ScannerList.Add(suspect_is.SuspectIs.FileName);
            if (ItemToPlay.WeaponToReport == null)
            {
                ScannerList.Add(carrying_weapon.Carryingaweapon.FileName);
                Subtitles += " suspect is carrying a ~r~weapon~s~";
                Notification.Text += " Unknown";
            }
            else if (ItemToPlay.WeaponToReport.Name == "weapon_rpg")
            {
                ScannerList.Add(carrying_weapon.ArmedwithanRPG.FileName);
                Subtitles += " suspect is armed with an ~r~RPG~s~";
                Notification.Text += " RPG";
            }
            else if (ItemToPlay.WeaponToReport.Name == "weapon_bat")
            {
                ScannerList.Add(carrying_weapon.Armedwithabat.FileName);
                Subtitles += " suspect is armed with a ~r~bat~s~";
                Notification.Text += " Bat";
            }
            else if (ItemToPlay.WeaponToReport.Name == "weapon_grenadelauncher" || ItemToPlay.WeaponToReport.Name == "weapon_grenadelauncher_smoke" || ItemToPlay.WeaponToReport.Name == "weapon_compactlauncher")
            {
                ScannerList.Add(carrying_weapon.Armedwithagrenadelauncher.FileName);
                Subtitles += " suspect is armed with a ~r~grenade launcher~s~";
                Notification.Text += " Grenade Launcher";
            }
            else if (ItemToPlay.WeaponToReport.Name == "weapon_dagger" || ItemToPlay.WeaponToReport.Name == "weapon_knife" || ItemToPlay.WeaponToReport.Name == "weapon_switchblade")
            {
                ScannerList.Add(carrying_weapon.Armedwithaknife.FileName);
                Subtitles += " suspect is armed with a ~r~knife~s~";
                Notification.Text += " Knife";
            }
            else if (ItemToPlay.WeaponToReport.Name == "weapon_minigun")
            {
                ScannerList.Add(carrying_weapon.Armedwithaminigun.FileName);
                Subtitles += " suspect is armed with a ~r~minigun~s~";
                Notification.Text += " Minigun";
            }
            else if (ItemToPlay.WeaponToReport.Name == "weapon_sawnoffshotgun")
            {
                ScannerList.Add(carrying_weapon.Armedwithasawedoffshotgun.FileName);
                Subtitles += " suspect is armed with a ~r~sawed off shotgun~s~";
                Notification.Text += " Sawed Off Shotgun";
            }
            else if (ItemToPlay.WeaponToReport.Category == GTAWeapon.WeaponCategory.LMG)
            {
                ScannerList.Add(carrying_weapon.Armedwithamachinegun.FileName);
                Subtitles += " suspect is armed with a ~r~machine gun~s~";
                Notification.Text += " Machine Gun";
            }
            else if (ItemToPlay.WeaponToReport.Category == GTAWeapon.WeaponCategory.Pistol)
            {
                ScannerList.Add(carrying_weapon.Armedwithafirearm.FileName);
                Subtitles += " suspect is armed with a ~r~pistol~s~";
                Notification.Text += " Pistol";
            }
            else if (ItemToPlay.WeaponToReport.Category == GTAWeapon.WeaponCategory.Shotgun)
            {
                ScannerList.Add(carrying_weapon.Armedwithashotgun.FileName);
                Subtitles += " suspect is armed with a ~r~shotgun~s~";
                Notification.Text += " Shotgun";
            }
            else if (ItemToPlay.WeaponToReport.Category == GTAWeapon.WeaponCategory.SMG)
            {
                ScannerList.Add(carrying_weapon.Armedwithasubmachinegun.FileName);
                Subtitles += " suspect is armed with a ~r~submachine gun~s~";
                Notification.Text += " Submachine Gun";
            }
            else if (ItemToPlay.WeaponToReport.Category == GTAWeapon.WeaponCategory.AR)
            {
                ScannerList.Add(carrying_weapon.Carryinganassaultrifle.FileName);
                Subtitles += " suspect is carrying an ~r~assault rifle~s~";
                Notification.Text += " Assault Rifle";
            }
            else if (ItemToPlay.WeaponToReport.Category == GTAWeapon.WeaponCategory.Sniper)
            {
                ScannerList.Add(carrying_weapon.Armedwithasniperrifle.FileName);
                Subtitles += " suspect is armed with a ~r~sniper rifle~s~";
                Notification.Text += " Sniper Rifle";
            }
            else if (ItemToPlay.WeaponToReport.Category == GTAWeapon.WeaponCategory.Heavy)
            {
                ScannerList.Add(status_message.HeavilyArmed.FileName);
                Subtitles += " suspect is ~r~heaviy armed~s~";
                Notification.Text += " Heavy Weapon";
            }
            else if (ItemToPlay.WeaponToReport.Category == GTAWeapon.WeaponCategory.Melee)
            {
                ScannerList.Add(carrying_weapon.Carryingaweapon.FileName);
                Subtitles += " suspect is carrying a ~r~weapon~s~";
                Notification.Text += " melee weapon";
            }
            else
            {
                int Num = rnd.Next(1, 5);
                if (Num == 1)
                {
                    ScannerList.Add(carrying_weapon.Armedwithafirearm.FileName);
                    Subtitles += " suspect is armed with a ~r~firearm~s~";
                }
                else if (Num == 2)
                {
                    ScannerList.Add(carrying_weapon.Armedwithagat.FileName);
                    Subtitles += " suspect is armed with a ~r~gat~s~";
                }
                else if (Num == 3)
                {
                    ScannerList.Add(carrying_weapon.Carryingafirearm.FileName);
                    Subtitles += " suspect is carrying a ~r~firearm~s~";
                }
                else
                {
                    ScannerList.Add(carrying_weapon.Carryingagat.FileName);
                    Subtitles += " suspect is carrying a ~r~gat~s~";
                }
                Notification.Text += " Gat";
            }
        }
        Subtitles += "~s~";
        ReportGenericEnd(ref ScannerList, NearType.Nothing, ref Subtitles, ref Notification, Game.LocalPlayer.Character.Position);
        PlayAudioList(new DispatchAudioEvent(ScannerList, Subtitles, Notification, ItemToPlay.CanBeInterrupted, false));
    }
    public static void FelonySpeeding(DispatchQueueItem ItemToPlay)
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        string NotificationTitle = GetNotificationSubtitle(ItemToPlay.ReportedBy);
        AttentionType WhoToNotifiy = GetWhoToNotify(ItemToPlay.ReportedBy);
        DispatchNotification Notification = new DispatchNotification("Police Scanner", NotificationTitle, "Felony Speeding");
        ReportGenericStart(ref ScannerList, ref Subtitles, WhoToNotifiy, ItemToPlay.ReportedBy, Game.LocalPlayer.Character.Position);

        if(WantedLevelScript.HasBeenWantedFor <= 15000 || ItemToPlay.Speed < 40f)
            ScannerList.Add(new List<string>() { crime_speeding_felony.Aspeedingfelony.FileName }.PickRandom());

        Subtitles += " a ~r~Speeding Felony~s~";
        AddSpeed(ref ScannerList, ItemToPlay.Speed, ref Subtitles, ref Notification);
        AddVehicleDescription(ItemToPlay.VehicleToReport, ref ScannerList, false, ref Subtitles, ref Notification, true, false,true, ItemToPlay.ReportedBy == ReportType.Officers);
        ReportGenericEnd(ref ScannerList, NearType.Nothing, ref Subtitles, ref Notification, Game.LocalPlayer.Character.Position);
        PlayAudioList(new DispatchAudioEvent(ScannerList, Subtitles, Notification, ItemToPlay.CanBeInterrupted, false));
    }
    public static void OfficerDown(DispatchQueueItem ItemToPlay)
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        string NotificationTitle = GetNotificationSubtitle(ItemToPlay.ReportedBy);
        DispatchNotification Notification = new DispatchNotification("Police Scanner", NotificationTitle, "Officer Down");
        ReportGenericStart(ref ScannerList, ref Subtitles, AttentionType.AllUnits, ReportType.Nobody, Game.LocalPlayer.Character.Position);
        Subtitles += " we have an ~r~Officer Down~s~";
        bool addRespondCode = true;
        int Num = General.MyRand.Next(1, 7);
        if (Num == 1)
        {
            ScannerList.Add(we_have.We_Have_1.FileName);
            ScannerList.Add(crime_officer_down.AcriticalsituationOfficerdown.FileName);
        }
        else if (Num == 2)
        {
            ScannerList.Add(we_have.We_Have_1.FileName);
            ScannerList.Add(crime_officer_down.AnofferdownpossiblyKIA.FileName);
        }
        else if (Num == 3)
        {
            ScannerList.Add(we_have.We_Have_1.FileName);
            ScannerList.Add(crime_officer_down.Anofficerdown.FileName);
        }
        else if (Num == 4)
        {
            ScannerList.Add(we_have.We_Have_1.FileName);
            ScannerList.Add(custom_wanted_level_line.Officerdownsituationiscode99.FileName);
            addRespondCode = false;
        }
        else if (Num == 5)
        {
            ScannerList.Add(custom_wanted_level_line.Wehavea1099allavailableunitsrespond.FileName);
            addRespondCode = false;
        }
        else
        {
            ScannerList.Add(we_have.We_Have_2.FileName);
            ScannerList.Add(crime_officer_down.Anofficerdownconditionunknown.FileName);
        }
        if (addRespondCode)
        {
            ScannerList.Add(new List<string>() { dispatch_respond_code.AllunitsrespondCode99.FileName, dispatch_respond_code.AllunitsrespondCode99emergency.FileName, dispatch_respond_code.Code99allunitsrespond.FileName, custom_wanted_level_line.Code99allavailableunitsconvergeonsuspect.FileName, custom_wanted_level_line.Code99officersrequireimmediateassistance.FileName
                                        ,custom_wanted_level_line.Wehavea1099allavailableunitsrespond.FileName,dispatch_respond_code.Code99allunitsrespond.FileName,dispatch_respond_code.EmergencyallunitsrespondCode99.FileName}.PickRandom());
            Subtitles += " ~s~all units repond ~o~Code-99 Emergency~s~";
            Notification.Text += "~n~All units repond ~o~Code-99 Emergency~s~";
        }

        AddLethalForceAuthorized(ref ScannerList, ref Subtitles,ref Notification);
        ReportGenericEnd(ref ScannerList, NearType.Nothing, ref Subtitles, ref Notification, Game.LocalPlayer.Character.Position);
        PlayAudioList(new DispatchAudioEvent(ScannerList, Subtitles, Notification,false,true));
    }
    public static void PedestrianHitAndRun(DispatchQueueItem ItemToPlay)
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        string NotificationTitle = GetNotificationSubtitle(ItemToPlay.ReportedBy);
        AttentionType WhoToNotifiy = GetWhoToNotify(ItemToPlay.ReportedBy);

        DispatchNotification Notification = new DispatchNotification("Police Scanner", NotificationTitle, "Pedestrian Hit-and-Run");
        ReportGenericStart(ref ScannerList, ref Subtitles, WhoToNotifiy, ItemToPlay.ReportedBy, Game.LocalPlayer.Character.Position);
        ScannerList.Add(new List<string>() { crime_ped_struck_by_veh.Apedestrianstruck.FileName, crime_ped_struck_by_veh.Apedestrianstruck1.FileName, crime_ped_struck_by_veh.Apedestrianstruckbyavehicle.FileName, crime_ped_struck_by_veh.Apedestrianstruckbyavehicle1.FileName }.PickRandom());
        Subtitles += " a ~r~Pedestrian Struck~s~";
        AddVehicleDescription(ItemToPlay.VehicleToReport, ref ScannerList, false, ref Subtitles, ref Notification, true, false,true, ItemToPlay.ReportedBy == ReportType.Officers);
        ReportGenericEnd(ref ScannerList, NearType.HeadingAndStreet, ref Subtitles, ref Notification, Game.LocalPlayer.Character.Position);
        PlayAudioList(new DispatchAudioEvent(ScannerList, Subtitles, Notification, ItemToPlay.CanBeInterrupted, false));
    }
    public static void RecklessDriving(DispatchQueueItem ItemToPlay)
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        string NotificationTitle = GetNotificationSubtitle(ItemToPlay.ReportedBy);
        AttentionType WhoToNotifiy = GetWhoToNotify(ItemToPlay.ReportedBy);

        DispatchNotification Notification = new DispatchNotification("Police Scanner", NotificationTitle, "Reckless Driving");
        ReportGenericStart(ref ScannerList, ref Subtitles, WhoToNotifiy, ItemToPlay.ReportedBy, Game.LocalPlayer.Character.Position);
        ScannerList.Add(new List<string>() { crime_reckless_driver.Arecklessdriver.FileName }.PickRandom());
        Subtitles += " a ~r~Reckless Driver~s~";
        AddVehicleDescription(ItemToPlay.VehicleToReport, ref ScannerList, false, ref Subtitles, ref Notification, true, false,true, ItemToPlay.ReportedBy == ReportType.Officers);
        ReportGenericEnd(ref ScannerList, NearType.HeadingAndStreet, ref Subtitles, ref Notification, Game.LocalPlayer.Character.Position);
        PlayAudioList(new DispatchAudioEvent(ScannerList, Subtitles, Notification, ItemToPlay.CanBeInterrupted, false));
    }
    public static void ShootingAtPolice(DispatchQueueItem ItemToPlay)
    {
        if (ReportedLethalForceAuthorized || ReportedOfficerDown)
            return;
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        string NotificationTitle = GetNotificationSubtitle(ItemToPlay.ReportedBy);
        DispatchNotification Notification = new DispatchNotification("Police Scanner", NotificationTitle, "Shots Fired at an Officer");
        ReportGenericStart(ref ScannerList, ref Subtitles, AttentionType.AllUnits, ItemToPlay.ReportedBy, Game.LocalPlayer.Character.Position);
        ScannerList.Add(crime_shots_fired_at_an_officer.Shotsfiredatanofficer.FileName);
        Subtitles += " ~r~Shots Fired at an Officer~s~";
        AddLethalForceAuthorized(ref ScannerList, ref Subtitles,ref Notification);
        ReportGenericEnd(ref ScannerList, NearType.Nothing, ref Subtitles, ref Notification, Game.LocalPlayer.Character.Position);
        PlayAudioList(new DispatchAudioEvent(ScannerList, Subtitles, Notification, ItemToPlay.CanBeInterrupted, false));
    }
    public static void SpottedStolenCar(DispatchQueueItem ItemToPlay)
    {
        string Subtitles = "";
        string NotificationTitle = GetNotificationSubtitle(ItemToPlay.ReportedBy);
        AttentionType WhoToNotifiy = GetWhoToNotify(ItemToPlay.ReportedBy);
        DispatchNotification Notification = new DispatchNotification("Police Scanner", NotificationTitle, "Driving a Stolen Vehicle");
        List<string> ScannerList = new List<string>();
        ReportGenericStart(ref ScannerList, ref Subtitles, WhoToNotifiy, ItemToPlay.ReportedBy, Game.LocalPlayer.Character.Position);
        ScannerList.Add(new List<string>() { crime_person_in_a_stolen_car.Apersoninastolencar.FileName, crime_person_in_a_stolen_vehicle.Apersoninastolenvehicle.FileName, crime_person_in_a_stolen_car.Apersoninastolencar.FileName }.PickRandom());
        Subtitles += " a person in a ~r~Stolen Vehicle~s~";
        AddSpeed(ref ScannerList, ItemToPlay.Speed, ref Subtitles, ref Notification);
        AddVehicleDescription(ItemToPlay.VehicleToReport, ref ScannerList, false, ref Subtitles, ref Notification, false, false, false, false);
        ReportGenericEnd(ref ScannerList, NearType.HeadingAndStreet, ref Subtitles, ref Notification, Game.LocalPlayer.Character.Position);
        PlayAudioList(new DispatchAudioEvent(ScannerList, Subtitles, Notification, ItemToPlay.CanBeInterrupted, false));
    }
    public static void ReportStolenVehicle(DispatchQueueItem ItemToPlay)
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        string NotificationTitle = GetNotificationSubtitle(ItemToPlay.ReportedBy);
        AttentionType WhoToNotifiy = GetWhoToNotify(ItemToPlay.ReportedBy);

        DispatchNotification Notification = new DispatchNotification("Police Scanner", NotificationTitle, "Stolen Vehicle Reported");
        if (ItemToPlay.VehicleToReport.VehicleEnt.IsPoliceVehicle)
        {
            ReportGenericStart(ref ScannerList, ref Subtitles, AttentionType.AllUnits, ItemToPlay.ReportedBy, ItemToPlay.VehicleToReport.PositionOriginallyEntered);
            ScannerList.Add(new List<string>() { crime_stolen_cop_car.Astolenpolicecar.FileName, crime_stolen_cop_car.Astolenpolicevehicle.FileName, crime_stolen_cop_car.Astolenpolicevehicle1.FileName, crime_stolen_cop_car.Defectivepolicevehicle.FileName }.PickRandom());
            Subtitles += " a ~r~Stolen Police Vehicle~s~";
        }
        else
        {
            ReportGenericStart(ref ScannerList, ref Subtitles, WhoToNotifiy, ItemToPlay.ReportedBy, ItemToPlay.VehicleToReport.PositionOriginallyEntered);
            ScannerList.Add(new List<string>() { crime_stolen_vehicle.Apossiblestolenvehicle.FileName }.PickRandom());
            Subtitles += " a possible ~r~Stolen Vehicle~s~";
        }
        AddVehicleDescription(ItemToPlay.VehicleToReport, ref ScannerList, true, ref Subtitles, ref Notification, true, false,false,true);
        ReportGenericEnd(ref ScannerList, NearType.Nothing, ref Subtitles, ref Notification, ItemToPlay.VehicleToReport.PositionOriginallyEntered);
        PlayAudioList(new DispatchAudioEvent(ScannerList, Subtitles, Notification, ItemToPlay.CanBeInterrupted, false));
        MarkVehicleAsStolen(ItemToPlay.VehicleToReport);
    }
    public static void ThreateningOfficerWithFirearm(DispatchQueueItem ItemToPlay)
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        string NotificationTitle = GetNotificationSubtitle(ItemToPlay.ReportedBy);
        AttentionType WhoToNotifiy = GetWhoToNotify(ItemToPlay.ReportedBy);
        DispatchNotification Notification = new DispatchNotification("Police Scanner", NotificationTitle, "Threatening Officers with a Firearm");
        ReportGenericStart(ref ScannerList, ref Subtitles, WhoToNotifiy, ItemToPlay.ReportedBy, Game.LocalPlayer.Character.Position);
        ScannerList.Add(crime_suspect_threatening_an_officer_with_a_firearm.Asuspectthreateninganofficerwithafirearm.FileName);
        Subtitles += " we have a suspect ~r~Threatening an Officer with a Firearm~s~";
        AddLethalForceAuthorized(ref ScannerList, ref Subtitles,ref Notification);
        ReportGenericEnd(ref ScannerList, NearType.HeadingStreetAndZone, ref Subtitles, ref Notification, Game.LocalPlayer.Character.Position);
        PlayAudioList(new DispatchAudioEvent(ScannerList, Subtitles, Notification, ItemToPlay.CanBeInterrupted, true));
    }
    public static void VehicleHitAndRun(DispatchQueueItem ItemToPlay)
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        string NotificationTitle = GetNotificationSubtitle(ItemToPlay.ReportedBy);
        AttentionType WhoToNotifiy = GetWhoToNotify(ItemToPlay.ReportedBy);
        DispatchNotification Notification = new DispatchNotification("Police Scanner", NotificationTitle, "Motor Vehicle Accident");

        if (ItemToPlay.ReportedBy == ReportType.Officers && WantedLevelScript.HasBeenWantedFor > 15000)
        {
            ReportGenericStart(ref ScannerList, ref Subtitles, AttentionType.Nobody, ReportType.Nobody, Game.LocalPlayer.Character.Position);
            List<string> OfficerVariations = new List<string>() { s_m_y_cop_white_full_01.DispatchSuspectsVehicleInACollision.FileName, s_m_y_cop_black_full_01.SuspectsVehicleHasCrashed.FileName, s_m_y_cop_black_full_02.WeHaveACollision.FileName };
            ScannerList.Add(OfficerVariations.PickRandom());
            ReportGenericEnd(ref ScannerList, NearType.Nothing, ref Subtitles, ref Notification, Game.LocalPlayer.Character.Position);
        }
        else
        {
            ReportGenericStart(ref ScannerList, ref Subtitles, WhoToNotifiy, ItemToPlay.ReportedBy, Game.LocalPlayer.Character.Position);
            //if (ItemToPlay.ReportedBy == ReportType.Civilians)
            //    ScannerList.Add(new List<string>() { crime_dangerous_driving.Dangerousdriving.FileName, crime_dangerous_driving.Dangerousdriving1.FileName, crime_reckless_driver.Arecklessdriver.FileName, crime_traffic_felony.Atrafficfelony.FileName }.PickRandom());
            //else if (ItemToPlay.ReportedBy == ReportType.Officers)
                ScannerList.Add(new List<string>() { crime_motor_vehicle_accident.Amotorvehicleaccident.FileName, crime_motor_vehicle_accident.AnAEincident.FileName, crime_motor_vehicle_accident.AseriousMVA.FileName }.PickRandom());
        }
        Subtitles += " a ~r~Motor Vehicle Accident~s~";
        AddVehicleDescription(ItemToPlay.VehicleToReport, ref ScannerList, false, ref Subtitles, ref Notification, true, false,true, ItemToPlay.ReportedBy == ReportType.Officers);
        ReportGenericEnd(ref ScannerList, NearType.HeadingAndStreet, ref Subtitles, ref Notification, Game.LocalPlayer.Character.Position);
        PlayAudioList(new DispatchAudioEvent(ScannerList, Subtitles, Notification, ItemToPlay.CanBeInterrupted, false));
    }
    public static void SuspiciousActivity(DispatchQueueItem ItemToPlay)
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        string NotificationTitle = GetNotificationSubtitle(ItemToPlay.ReportedBy);
        AttentionType WhoToNotifiy = GetWhoToNotify(ItemToPlay.ReportedBy);
        DispatchNotification Notification = new DispatchNotification("Police Scanner", NotificationTitle, "Suspicious Activity");
        ReportGenericStart(ref ScannerList, ref Subtitles, WhoToNotifiy, ItemToPlay.ReportedBy, Game.LocalPlayer.Character.Position);
        ScannerList.Add(new List<string>() { crime_suspicious_activity.Suspiciousactivity.FileName, crime_theft.Apossibletheft.FileName }.PickRandom());
        Subtitles += " ~y~Suspicious Activity~s~";
        ReportGenericEnd(ref ScannerList, NearType.HeadingAndStreet, ref Subtitles, ref Notification, Game.LocalPlayer.Character.Position);
        PlayAudioList(new DispatchAudioEvent(ScannerList, Subtitles, Notification, ItemToPlay.CanBeInterrupted, false));
    }
    public static void SuspiciousVehicle(DispatchQueueItem ItemToPlay)
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        string NotificationTitle = GetNotificationSubtitle(ItemToPlay.ReportedBy);
        AttentionType WhoToNotifiy = GetWhoToNotify(ItemToPlay.ReportedBy);
        DispatchNotification Notification = new DispatchNotification("Police Scanner", NotificationTitle, "Suspicious Vehicle");
        ReportGenericStart(ref ScannerList, ref Subtitles, WhoToNotifiy, ItemToPlay.ReportedBy, Game.LocalPlayer.Character.Position);
        ScannerList.Add(new List<string>() { crime_suspicious_vehicle.Asuspiciousvehicle.FileName }.PickRandom());
        Subtitles += " a ~r~Suspicious Vehicle~s~";
        AddVehicleDescription(ItemToPlay.VehicleToReport, ref ScannerList, false, ref Subtitles, ref Notification, true, false,false, ItemToPlay.ReportedBy == ReportType.Officers);
        ReportGenericEnd(ref ScannerList, NearType.HeadingAndStreet, ref Subtitles, ref Notification, Game.LocalPlayer.Character.Position);
        PlayAudioList(new DispatchAudioEvent(ScannerList, Subtitles, Notification, ItemToPlay.CanBeInterrupted, false));
    }
    public static void GrandTheftAuto(DispatchQueueItem ItemToPlay)
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        string NotificationTitle = GetNotificationSubtitle(ItemToPlay.ReportedBy);
        AttentionType WhoToNotifiy = GetWhoToNotify(ItemToPlay.ReportedBy);
        DispatchNotification Notification = new DispatchNotification("Police Scanner", NotificationTitle, "Grand Theft Auto");
        ReportGenericStart(ref ScannerList, ref Subtitles, WhoToNotifiy, ItemToPlay.ReportedBy, Game.LocalPlayer.Character.Position);
        ScannerList.Add(new List<string>() { crime_grand_theft_auto.Agrandtheftauto.FileName, crime_grand_theft_auto.Agrandtheftautoinprogress.FileName, crime_grand_theft_auto.AGTAinprogress.FileName, crime_grand_theft_auto.AGTAinprogress1.FileName }.PickRandom());
        Subtitles += " a ~y~GTA~s~ in progress";
        if(ItemToPlay.VehicleToReport == null)
        {
            ItemToPlay.VehicleToReport = PlayerState.CurrentVehicle;
        }
        if (ItemToPlay.VehicleToReport != null && !ItemToPlay.VehicleToReport.HasBeenDescribedByDispatch)
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
            AddVehicleDescription(ItemToPlay.VehicleToReport, ref ScannerList, ItemToPlay.ReportedBy == ReportType.Civilians, ref Subtitles, ref Notification, false, true,false, false);
        }
        ReportGenericEnd(ref ScannerList, NearType.HeadingAndStreet, ref Subtitles, ref Notification, Game.LocalPlayer.Character.Position);
        PlayAudioList(new DispatchAudioEvent(ScannerList, Subtitles, Notification, ItemToPlay.CanBeInterrupted, false));
        if (ItemToPlay.VehicleToReport != null)
        {
            MarkVehicleAsStolen(ItemToPlay.VehicleToReport);
        }
    }
    private static void RunningARedLight(DispatchQueueItem ItemToPlay)
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        string NotificationTitle = GetNotificationSubtitle(ItemToPlay.ReportedBy);
        AttentionType WhoToNotifiy = GetWhoToNotify(ItemToPlay.ReportedBy);
        DispatchNotification Notification = new DispatchNotification("Police Scanner", NotificationTitle, "Running a Red Light");
        ReportGenericStart(ref ScannerList, ref Subtitles, WhoToNotifiy, ItemToPlay.ReportedBy, Game.LocalPlayer.Character.Position);
        ScannerList.Add(new List<string>() { crime_person_running_a_red_light.Apersonrunningaredlight.FileName }.PickRandom());
        Subtitles += " a person ~r~Running a Red Light~s~";
        AddVehicleDescription(ItemToPlay.VehicleToReport, ref ScannerList, false, ref Subtitles, ref Notification, true, false,true, ItemToPlay.ReportedBy == ReportType.Officers);
        ReportGenericEnd(ref ScannerList, NearType.HeadingAndStreet, ref Subtitles, ref Notification, Game.LocalPlayer.Character.Position);
        PlayAudioList(new DispatchAudioEvent(ScannerList, Subtitles, Notification, ItemToPlay.CanBeInterrupted, false));
    }
    public static void CriminalActivity(DispatchQueueItem ItemToPlay)
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        string NotificationTitle = GetNotificationSubtitle(ItemToPlay.ReportedBy);
        AttentionType WhoToNotifiy = GetWhoToNotify(ItemToPlay.ReportedBy);
        Vector3 PositionToReport = WantedLevelScript.LastWantedCenterPosition;
        if (Investigation.InInvestigationMode)
            PositionToReport = Investigation.InvestigationPosition;

        DispatchNotification Notification = new DispatchNotification("Police Scanner", NotificationTitle, "Criminal Activity");
        ReportGenericStart(ref ScannerList, ref Subtitles, WhoToNotifiy, ItemToPlay.ReportedBy, PositionToReport);
        ScannerList.Add(new List<string>() { crime_criminal_activity.Criminalactivity.FileName, crime_criminal_activity.Illegalactivity.FileName, crime_criminal_activity.Prohibitedactivity.FileName }.PickRandom());
        Subtitles += ", ~y~Criminal activity~s~";
        ReportGenericEnd(ref ScannerList, NearType.Street, ref Subtitles, ref Notification, PositionToReport);
        PlayAudioList(new DispatchAudioEvent(ScannerList, Subtitles, Notification, ItemToPlay.CanBeInterrupted, false));
    }
    public static void ShotsFired(DispatchQueueItem ItemToPlay)
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        string NotificationTitle = GetNotificationSubtitle(ItemToPlay.ReportedBy);
        AttentionType WhoToNotifiy = GetWhoToNotify(ItemToPlay.ReportedBy);
        Vector3 PositionToReport = WantedLevelScript.LastWantedCenterPosition;


        if (Investigation.InInvestigationMode)
            PositionToReport = Investigation.InvestigationPosition;

        DispatchNotification Notification = new DispatchNotification("Police Scanner", NotificationTitle, "Shots Fired");
        ReportGenericStart(ref ScannerList, ref Subtitles, WhoToNotifiy, ItemToPlay.ReportedBy, PositionToReport);


        ScannerList.Add(new List<String>() { crime_shooting.Afirearmssituationseveralshotsfired.FileName, crime_shooting.Aweaponsincidentshotsfired.FileName, crime_shoot_out.Ashootout.FileName, crime_firearm_discharged_in_a_public_place.Afirearmdischargedinapublicplace.FileName
            , crime_firearms_incident.AfirearmsincidentShotsfired.FileName, crime_firearms_incident.Anincidentinvolvingshotsfired.FileName, crime_firearms_incident.AweaponsincidentShotsfired.FileName }.PickRandom());
        Subtitles += " a ~y~Firearms Discharge~s~";



        ReportGenericEnd(ref ScannerList, NearType.Street, ref Subtitles, ref Notification, PositionToReport);
        PlayAudioList(new DispatchAudioEvent(ScannerList, Subtitles, Notification, ItemToPlay.CanBeInterrupted, false));
    }
    public static void TerroristActivity(DispatchQueueItem ItemToPlay)
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        string NotificationTitle = GetNotificationSubtitle(ItemToPlay.ReportedBy);
        AttentionType WhoToNotifiy = GetWhoToNotify(ItemToPlay.ReportedBy);
        Vector3 PositionToReport = WantedLevelScript.LastWantedCenterPosition;
        if (Investigation.InInvestigationMode)
            PositionToReport = Investigation.InvestigationPosition;

        DispatchNotification Notification = new DispatchNotification("Police Scanner", NotificationTitle, "Terrorist Activity");
        ReportGenericStart(ref ScannerList, ref Subtitles, WhoToNotifiy, ItemToPlay.ReportedBy, PositionToReport);
        ScannerList.Add(new List<string>() { crime_terrorist_activity.Possibleterroristactivity.FileName, crime_terrorist_activity.Possibleterroristactivity1.FileName, crime_terrorist_activity.Possibleterroristactivity2.FileName, crime_terrorist_activity.Terroristactivity.FileName }.PickRandom());
        Subtitles += " possible ~y~Terrorist Activity~s~ in progress";

        ReportGenericEnd(ref ScannerList, NearType.Street, ref Subtitles, ref Notification, PositionToReport);
        PlayAudioList(new DispatchAudioEvent(ScannerList, Subtitles, Notification, ItemToPlay.CanBeInterrupted, false));
    }
    private static void TrespassingOnGovernmentProperty(DispatchQueueItem ItemToPlay)
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        string NotificationTitle = GetNotificationSubtitle(ItemToPlay.ReportedBy);
        AttentionType WhoToNotifiy = GetWhoToNotify(ItemToPlay.ReportedBy);
        DispatchNotification Notification = new DispatchNotification("Police Scanner", NotificationTitle, "Trespassing on Government Property");
        ReportGenericStart(ref ScannerList, ref Subtitles, WhoToNotifiy, ItemToPlay.ReportedBy, Game.LocalPlayer.Character.Position);
        ScannerList.Add(crime_trespassing_on_government_property.Trespassingongovernmentproperty.FileName);
        Subtitles += " ~r~Trespassing on Government Property~s~";
        AddLethalForceAuthorized(ref ScannerList, ref Subtitles,ref Notification);
        ReportGenericEnd(ref ScannerList, NearType.Nothing, ref Subtitles, ref Notification, Game.LocalPlayer.Character.Position);
        PlayAudioList(new DispatchAudioEvent(ScannerList, Subtitles, Notification, ItemToPlay.CanBeInterrupted, false));
    }
    public static void CivlianFatality(DispatchQueueItem ItemToPlay)
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        string NotificationTitle = GetNotificationSubtitle(ItemToPlay.ReportedBy);
        AttentionType WhoToNotifiy = GetWhoToNotify(ItemToPlay.ReportedBy);
        Vector3 PositionToReport = WantedLevelScript.LastWantedCenterPosition;
        if (Investigation.InInvestigationMode)
            PositionToReport = Investigation.InvestigationPosition;

        DispatchNotification Notification = new DispatchNotification("Police Scanner", NotificationTitle, "Civilian Fatality");
        ReportGenericStart(ref ScannerList, ref Subtitles, WhoToNotifiy, ItemToPlay.ReportedBy, PositionToReport);
        ScannerList.Add(new List<string>() { crime_civilian_fatality.Acivilianfatality.FileName, crime_civilian_down.Aciviliandown.FileName }.PickRandom());
        Subtitles += " ~y~Civilian fatality~s~";

        ReportGenericEnd(ref ScannerList, NearType.Street, ref Subtitles, ref Notification, PositionToReport);
        PlayAudioList(new DispatchAudioEvent(ScannerList, Subtitles, Notification, ItemToPlay.CanBeInterrupted, false));
    }
    public static void CivilianInjury(DispatchQueueItem ItemToPlay)
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        string NotificationTitle = GetNotificationSubtitle(ItemToPlay.ReportedBy);
        AttentionType WhoToNotifiy = GetWhoToNotify(ItemToPlay.ReportedBy);
        Vector3 PositionToReport = WantedLevelScript.LastWantedCenterPosition;
        if (Investigation.InInvestigationMode)
            PositionToReport = Investigation.InvestigationPosition;

        DispatchNotification Notification = new DispatchNotification("Police Scanner", NotificationTitle, "Civilian Injury");
        ReportGenericStart(ref ScannerList, ref Subtitles, WhoToNotifiy, ItemToPlay.ReportedBy, PositionToReport);
        ScannerList.Add(new List<string>() { crime_injured_civilian.Aninjuredcivilian.FileName, crime_civilian_needing_assistance.Acivilianinneedofassistance.FileName, crime_civilian_needing_assistance.Acivilianrequiringassistance.FileName, crime_assault_on_a_civilian.Anassaultonacivilian.FileName }.PickRandom());
        Subtitles += " ~y~Civilian Injured~s~";

        ReportGenericEnd(ref ScannerList, NearType.Street, ref Subtitles, ref Notification, PositionToReport);
        PlayAudioList(new DispatchAudioEvent(ScannerList, Subtitles, Notification, ItemToPlay.CanBeInterrupted, false));
    }
    public static void StealingAirVehicle(DispatchQueueItem ItemToPlay)
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        string NotificationTitle = GetNotificationSubtitle(ItemToPlay.ReportedBy);
        DispatchNotification Notification = new DispatchNotification("Police Scanner", NotificationTitle, "Stolen Air Vehicle");
        ReportGenericStart(ref ScannerList, ref Subtitles, AttentionType.Nobody, ItemToPlay.ReportedBy, Game.LocalPlayer.Character.Position);

        if (ItemToPlay.VehicleToReport == null)
            ItemToPlay.VehicleToReport = PlayerState.CurrentVehicle;


        if (ItemToPlay.VehicleToReport == null || ItemToPlay.VehicleToReport.VehicleEnt == null)


            if (ItemToPlay.VehicleToReport != null && ItemToPlay.VehicleToReport.VehicleEnt != null && ItemToPlay.VehicleToReport.VehicleEnt.IsHelicopter)
            {
                ScannerList.Add(new List<string>() { crime_stolen_helicopter.Astolenhelicopter.FileName }.PickRandom());
                Subtitles += " a ~r~Stolen Helicopter~s~";
            }
            else //if (vehicle.VehicleEnt.IsPlane)
            {
                ScannerList.Add(new List<string>() { crime_stolen_aircraft.Astolenaircraft.FileName, crime_stolen_aircraft.Astolenaircraft.FileName, crime_hijacked_aircraft.Ahijackedaircraft.FileName, crime_theft_of_an_aircraft.Theftofanaircraft.FileName }.PickRandom());
                Subtitles += " a ~r~Stolen Aircraft~s~";
            }
        //else
        //    return;

        AddLethalForceAuthorized(ref ScannerList, ref Subtitles,ref Notification);
        ReportGenericEnd(ref ScannerList, NearType.Nothing, ref Subtitles, ref Notification, Game.LocalPlayer.Character.Position);
        PlayAudioList(new DispatchAudioEvent(ScannerList, Subtitles, Notification, ItemToPlay.CanBeInterrupted, false));
    }
    public static void AirSupportRequested(DispatchQueueItem ItemToPlay)
    {
        if (ReportedAirSupportRequested)
            return;
        ReportedAirSupportRequested = true;
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        string NotificationTitle = GetNotificationSubtitle(ItemToPlay.ReportedBy);
        Vector3 PositionToReport = WantedLevelScript.LastWantedCenterPosition;

        DispatchNotification Notification = new DispatchNotification("Police Scanner", NotificationTitle, "Air Support Requested");
        ReportGenericStart(ref ScannerList, ref Subtitles, AttentionType.Nobody, ReportType.Nobody, PositionToReport);
        ScannerList.Add(new List<string>() {
            officer_requests_air_support.Officersrequestinghelicoptersupport.FileName,
            officer_requests_air_support.Code99unitsrequestimmediateairsupport.FileName,
            officer_requests_air_support.Officersrequireaerialsupport.FileName,
            officer_requests_air_support.Officersrequireaerialsupport1.FileName,
            officer_requests_air_support.Officersrequireairsupport.FileName,
            officer_requests_air_support.Unitsrequestaerialsupport.FileName,
            officer_requests_air_support.Unitsrequestingairsupport.FileName,
            officer_requests_air_support.Unitsrequestinghelicoptersupport.FileName,

        }.PickRandom());
        Subtitles += "Officers require air support";

        ReportGenericEnd(ref ScannerList, NearType.Zone, ref Subtitles, ref Notification, PositionToReport);
        PlayAudioList(new DispatchAudioEvent(ScannerList, Subtitles, Notification, false, true));
    }
    public static void MilitaryDeployed(DispatchQueueItem ItemToPlay)
    {
        if (ReportedMilitaryDeployed)
            return;
        ReportedMilitaryDeployed = true;
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        string NotificationTitle = GetNotificationSubtitle(ItemToPlay.ReportedBy);
        Vector3 PositionToReport = WantedLevelScript.LastWantedCenterPosition;

        DispatchNotification Notification = new DispatchNotification("Police Scanner", NotificationTitle, "Military Units Requested");
        ReportGenericStart(ref ScannerList, ref Subtitles, AttentionType.Nobody, ReportType.Nobody, PositionToReport);
        ScannerList.Add(new List<string>() { custom_wanted_level_line.Code13militaryunitsrequested.FileName }.PickRandom());
        Subtitles += "Code-13 military units requested";

        ReportGenericEnd(ref ScannerList, NearType.Zone, ref Subtitles, ref Notification, PositionToReport);
        PlayAudioList(new DispatchAudioEvent(ScannerList, Subtitles, Notification, false, true));
    }
    public static void CivilianShot(DispatchQueueItem ItemToPlay)
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        string NotificationTitle = GetNotificationSubtitle(ItemToPlay.ReportedBy);
        AttentionType WhoToNotifiy = GetWhoToNotify(ItemToPlay.ReportedBy);
        Vector3 PositionToReport = WantedLevelScript.LastWantedCenterPosition;
        if (Investigation.InInvestigationMode)
            PositionToReport = Investigation.InvestigationPosition;

        DispatchNotification Notification = new DispatchNotification("Police Scanner", NotificationTitle, "Civilian Shot");
        ReportGenericStart(ref ScannerList, ref Subtitles, WhoToNotifiy, ItemToPlay.ReportedBy, PositionToReport);
        ScannerList.Add(new List<string>() { crime_civillian_gsw.AcivilianGSW.FileName, crime_civillian_gsw.Acivilianshot.FileName, crime_civillian_gsw.Agunshotwound.FileName }.PickRandom());
        Subtitles += " ~y~Civilian Shot~s~";

        ReportGenericEnd(ref ScannerList, NearType.Street, ref Subtitles, ref Notification, PositionToReport);
        PlayAudioList(new DispatchAudioEvent(ScannerList, Subtitles, Notification, ItemToPlay.CanBeInterrupted, false));
    }
    public static void CivilianMugged(DispatchQueueItem ItemToPlay)
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        string NotificationTitle = GetNotificationSubtitle(ItemToPlay.ReportedBy);
        AttentionType WhoToNotifiy = GetWhoToNotify(ItemToPlay.ReportedBy);
        Vector3 PositionToReport = WantedLevelScript.LastWantedCenterPosition;
        if (Investigation.InInvestigationMode)
            PositionToReport = Investigation.InvestigationPosition;

        DispatchNotification Notification = new DispatchNotification("Police Scanner", NotificationTitle, "Civilian Mugged");
        ReportGenericStart(ref ScannerList, ref Subtitles, WhoToNotifiy, ItemToPlay.ReportedBy, PositionToReport);
        ScannerList.Add(new List<string>() { crime_mugging.Apossiblemugging.FileName }.PickRandom());
        Subtitles += " ~y~Civilian Mugged~s~";

        ReportGenericEnd(ref ScannerList, NearType.Street, ref Subtitles, ref Notification, PositionToReport);
        PlayAudioList(new DispatchAudioEvent(ScannerList, Subtitles, Notification, ItemToPlay.CanBeInterrupted, false));
    }
    public static void SuspectEvadedOfficers(DispatchQueueItem ItemToPlay)
    {
        if (PedList.CopPeds.Any(x => x.DistanceToPlayer <= 100f))
            return;

        List<string> ScannerList = new List<string>();
        ScannerList.Add(new List<string>{ AudioBeeps.Radio_Start_1.FileName,AudioBeeps.Radio_Start_2.FileName }.PickRandom());

        ScannerList.Add(new List<string>() { suspect_eluded_pt_1.SuspectEvadedPursuingOfficiers.FileName, suspect_eluded_pt_1.OfficiersHaveLostVisualOnSuspect.FileName }.PickRandom());
        string Subtitles = "Suspect evaded pursuing officers,~s~";
        DispatchNotification Notification = new DispatchNotification("Police Scanner", "~g~Status~s~", "Suspect Evaded");
        ReportGenericEnd(ref ScannerList, NearType.Zone, ref Subtitles, ref Notification, WantedLevelScript.LastWantedCenterPosition);
        PlayAudioList(new DispatchAudioEvent(ScannerList, Subtitles, Notification, true, false));
    }
    public static void SuspectArrested(DispatchQueueItem ItemToPlay)
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        DispatchNotification Notification = new DispatchNotification("Police Scanner", "~g~Status~s~", "Suspect Arrested");
        ReportGenericStart(ref ScannerList, ref Subtitles, AttentionType.Nobody, ReportType.Nobody, Game.LocalPlayer.Character.Position);
        ScannerList.Add(new List<string>() { crook_arrested.Officershaveapprehendedsuspect.FileName, crook_arrested.Officershaveapprehendedsuspect1.FileName }.PickRandom());
        Subtitles += "Officers have apprehended suspect";
        ReportGenericEnd(ref ScannerList, NearType.Nothing, ref Subtitles, ref Notification, Game.LocalPlayer.Character.Position);
        PlayAudioList(new DispatchAudioEvent(ScannerList, Subtitles,null,true,false));
    }
    public static void SuspectWasted(DispatchQueueItem ItemToPlay)
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        DispatchNotification Notification = new DispatchNotification("Police Scanner", "~g~Status~s~", "Suspect Wasted");
        ReportGenericStart(ref ScannerList, ref Subtitles, AttentionType.Nobody, ReportType.Nobody, Game.LocalPlayer.Character.Position);
        ScannerList.Add(new List<string>() { crook_killed.Criminaldown.FileName, crook_killed.Suspectdown.FileName, crook_killed.Suspectneutralized.FileName, crook_killed.Suspectdownmedicalexaminerenroute.FileName, crook_killed.Suspectdowncoronerenroute.FileName, crook_killed.Officershavepacifiedsuspect.FileName }.PickRandom());
        Subtitles += "Criminal down";
        ReportGenericEnd(ref ScannerList, NearType.Nothing, ref Subtitles, ref Notification, Game.LocalPlayer.Character.Position);
        PlayAudioList(new DispatchAudioEvent(ScannerList, Subtitles,null,true,false));
    }
    public static void SuspectChangedVehicle(DispatchQueueItem ItemToPlay)
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        DispatchNotification Notification = new DispatchNotification("Police Scanner", "~g~Status~s~", "Suspect Changed Vehicle");
        ReportGenericStart(ref ScannerList, ref Subtitles, AttentionType.Nobody, ReportType.Nobody, Game.LocalPlayer.Character.Position);
        
        Subtitles += "Suspect spotted driving a";
        ScannerList.Add(suspect_last_seen.SuspectSpotted.FileName);
        ScannerList.Add(conjunctives.Drivinga.FileName);
        AddVehicleDescription(ItemToPlay.VehicleToReport, ref ScannerList, false, ref Subtitles, ref Notification, false, true,false, true);

        ReportGenericEnd(ref ScannerList, NearType.Nothing, ref Subtitles, ref Notification, Game.LocalPlayer.Character.Position);
        PlayAudioList(new DispatchAudioEvent(ScannerList, Subtitles, Notification,true,false));
    }
    public static void RequestBackup(DispatchQueueItem ItemToPlay)
    {
        RequestBackupPreamble();
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        DispatchNotification Notification = new DispatchNotification("Police Scanner", "~g~Status~s~", "Backup Required");
        ReportGenericStart(ref ScannerList, ref Subtitles, AttentionType.AllUnits, ReportType.Nobody, Game.LocalPlayer.Character.Position);
        List<string> PossibleVariations = new List<string>() { assistance_required.Assistanceneeded.FileName, assistance_required.Assistancerequired.FileName, assistance_required.Backupneeded.FileName, assistance_required.Backuprequired.FileName, assistance_required.Officersneeded.FileName, assistance_required.Officersrequired.FileName,
                                                                officer_requests_backup.Officersrequestingbackup.FileName,officer_requests_backup.Unitsrequirebackup.FileName,officer_requests_backup.Unitsrequireimmediateassistance.FileName,officer_requests_backup.Unitsrequestingbackup.FileName,officer_requests_backup.Officerneedsimmediateassistance.FileName };
        ScannerList.Add(PossibleVariations.PickRandom());
        Subtitles += " ~r~Assistance Needed~s~";

        if (!AddStreet(ref ScannerList, ref Subtitles, ref Notification))
            AddZone(ref ScannerList, ref Subtitles, Game.LocalPlayer.Character.Position, ref Notification);

        if (ItemToPlay.ResultsInLethalForce)
        {
            AddLethalForceAuthorized(ref ScannerList, ref Subtitles,ref Notification);
        }
        ScannerList.Add(dispatch_respond_code.UnitsrespondCode3.FileName);
        Subtitles += " ~s~Units Repond ~o~Code-3~s~";
        Notification.Text += "~n~Units Repond ~o~Code-3~s~";
        ReportGenericEnd(ref ScannerList, NearType.Nothing, ref Subtitles, ref Notification, Game.LocalPlayer.Character.Position);
        PlayAudioList(new DispatchAudioEvent(ScannerList, Subtitles, Notification, false, true));
    }
    private static void RequestBackupPreamble()
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        DispatchNotification Notification = new DispatchNotification("Police Scanner", "~g~Status~s~", "Backup Required");
        ReportGenericStart(ref ScannerList, ref Subtitles, AttentionType.Nobody, ReportType.Nobody, Game.LocalPlayer.Character.Position);
        List<string> OfficerVariations = new List<string>() { s_m_y_cop_white_full_01.RequestingBackup.FileName, s_m_y_cop_white_full_01.RequestingBackupWeNeedBackup.FileName, s_m_y_cop_white_full_01.WeNeedBackupNow.FileName, s_m_y_cop_white_full_02.MikeOscarSamInHotNeedOfBackup.FileName, s_m_y_cop_white_full_02.MikeOScarSamRequestingBackup.FileName
                            ,s_m_y_cop_white_mini_02.INeedSomeSeriousBackupHere.FileName,s_m_y_cop_white_mini_03.OfficerInNeedofSomeBackupHere.FileName};
        ScannerList.Add(OfficerVariations.PickRandom());
        ReportGenericEnd(ref ScannerList, NearType.Nothing, ref Subtitles, ref Notification, Game.LocalPlayer.Character.Position);
    }
    public static void WeaponsFree(DispatchQueueItem ItemToPlay)
    {
        if (ReportedWeaponsFree || PlayerState.IsBusted || PlayerState.IsDead)
            return;
        ReportedWeaponsFree = true;
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        DispatchNotification Notification = new DispatchNotification("Police Scanner", "~g~Status~s~", "Officers are ~r~Weapons Free~s~");
        ReportGenericStart(ref ScannerList, ref Subtitles, AttentionType.AllUnits, ReportType.Nobody, Game.LocalPlayer.Character.Position);
        ScannerList.Add(custom_wanted_level_line.Suspectisarmedanddangerousweaponsfree.FileName);
        Subtitles += " suspect is ~r~Armed and Dangerous~s~, you are ~r~Weapons Free~s~";
        ReportGenericEnd(ref ScannerList, NearType.Nothing, ref Subtitles, ref Notification, Game.LocalPlayer.Character.Position);
        PlayAudioList(new DispatchAudioEvent(ScannerList, Subtitles, Notification, false, true));
    }
    private static void SuspectResisitingArrest(DispatchQueueItem ItemToPlay)
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        DispatchNotification Notification = new DispatchNotification("Police Scanner", "~o~Crime Observed~s~", "Resisting Arrest");
        ReportGenericStart(ref ScannerList, ref Subtitles, AttentionType.Nobody, ReportType.Officers, Game.LocalPlayer.Character.Position);
        ScannerList.Add(new List<string>() { crime_person_resisting_arrest.Apersonresistingarrest.FileName, crime_suspect_resisting_arrest.Asuspectresistingarrest.FileName, crime_1_48_resist_arrest.Acriminalresistingarrest.FileName,
            crime_1_48_resist_arrest.Acriminalresistingarrest1.FileName, crime_1_48_resist_arrest.Asuspectfleeingacrimescene.FileName, crime_1_48_resist_arrest.Asuspectontherun.FileName }.PickRandom());
        Subtitles += " a ~r~Suspect Resisting Arrest~s~";

        ScannerList.Add(new List<string>() { suspect_last_seen.TargetIs.FileName, suspect_last_seen.TargetLastReported.FileName, suspect_last_seen.TargetSpotted.FileName }.PickRandom());

        if (!Police.WasPlayerLastSeenInVehicle)
        {    
            ScannerList.Add(new List<string>() { on_foot.Onfoot.FileName, on_foot.Onfoot.FileName }.PickRandom());
            Subtitles += " target last seen on foot";
            Notification.Text += "~n~Last Seen on Foot";
        }
        else
        {
            if (ItemToPlay.VehicleToReport != null && !ItemToPlay.VehicleToReport.HasBeenDescribedByDispatch)
            {
                Notification.Text += "~n~Last Seen driving a ";
                Subtitles += " driving a";
                ScannerList.Add(new List<string>() { conjunctives.Drivinga.FileName }.PickRandom());
                AddVehicleDescription(ItemToPlay.VehicleToReport, ref ScannerList, false, ref Subtitles, ref Notification, false, true, false, true);
            }
            else if(ItemToPlay.VehicleToReport != null && ItemToPlay.VehicleToReport.HasBeenDescribedByDispatch)
            {
                AddVehicleClass(ItemToPlay.VehicleToReport, ref ScannerList, ref Subtitles, ref Notification);
            }
            
        }
        ReportGenericEnd(ref ScannerList, NearType.Nothing, ref Subtitles, ref Notification, Game.LocalPlayer.Character.Position);
        PlayAudioList(new DispatchAudioEvent(ScannerList, Subtitles, Notification,true,false));
    }
    public static void LethalForceAuthorized(DispatchQueueItem ItemToPlay)
    {
        if (ReportedLethalForceAuthorized)
            return;

        ReportedLethalForceAuthorized = true;
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        DispatchNotification Notification = new DispatchNotification("Police Scanner", "~g~Status~s~", "~r~Lethal Force Authorized~s~");
        ReportGenericStart(ref ScannerList, ref Subtitles, AttentionType.AllUnits, ReportType.Nobody, Game.LocalPlayer.Character.Position);
        ScannerList.Add(new List<string>() { lethal_force.Useofdeadlyforceauthorized.FileName, lethal_force.Useofdeadlyforceisauthorized.FileName, lethal_force.Useofdeadlyforceisauthorized1.FileName, lethal_force.Useoflethalforceisauthorized.FileName, lethal_force.Useofdeadlyforcepermitted1.FileName }.PickRandom());
        Subtitles += " use of ~r~Deadly Force~s~ is authorized";
        ReportGenericEnd(ref ScannerList, NearType.Nothing, ref Subtitles, ref Notification, Game.LocalPlayer.Character.Position);
        PlayAudioList(new DispatchAudioEvent(ScannerList, Subtitles, Notification,false,true));
    }
    public static void SuspectLost(DispatchQueueItem ItemToPlay)
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        DispatchNotification Notification = new DispatchNotification("Police Scanner", "~g~Status~s~", "Suspect Lost");
        ReportGenericStart(ref ScannerList, ref Subtitles, AttentionType.Nobody, ReportType.Nobody, WantedLevelScript.LastWantedCenterPosition);
        ScannerList.Add(new List<string>() { attempt_to_find.AllunitsATonsuspects20.FileName, attempt_to_find.Allunitsattempttoreacquire.FileName, attempt_to_find.Allunitsattempttoreacquirevisual.FileName, attempt_to_find.RemainintheareaATL20onsuspect.FileName, attempt_to_find.RemainintheareaATL20onsuspect1.FileName }.PickRandom());
        Subtitles += "Remain in the area, ATL20 on suspect";
        ReportGenericEnd(ref ScannerList, NearType.Nothing, ref Subtitles, ref Notification, WantedLevelScript.LastWantedCenterPosition);

        ReportGenericStart(ref ScannerList, ref Subtitles, AttentionType.Nobody, ReportType.Nobody, WantedLevelScript.LastWantedCenterPosition);
        ScannerList.Add(new List<string>() { s_m_y_cop_white_full_02.Charlie4WellLookForThoseMaggots.FileName, s_m_y_cop_white_full_02.CopyThatDIspatchWellFindThoseAnimals.FileName, s_m_y_cop_white_full_02.CharlieFourRogerThatWereIntheArea.FileName
        ,s_m_y_cop_white_mini_03.AdamFourCopy.FileName,s_m_y_cop_white_mini_03.DispatchNeedSomeGuidanceHere.FileName}.PickRandom());
        ReportGenericEnd(ref ScannerList, NearType.Nothing, ref Subtitles, ref Notification, WantedLevelScript.LastWantedCenterPosition);

        PlayAudioList(new DispatchAudioEvent(ScannerList, Subtitles, Notification,true,false));
    }
    private static void NoFurtherUnitsNeeded(DispatchQueueItem ItemToPlay)
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        DispatchNotification Notification = new DispatchNotification("Police Scanner", "~g~Status~s~", "Officers On-Site, Code 4-ADAM");
        ReportGenericStart(ref ScannerList, ref Subtitles, AttentionType.Nobody, ReportType.Nobody, WantedLevelScript.LastWantedCenterPosition);
        ScannerList.Add(new List<string>() { officers_on_scene.Officersareatthescene.FileName, officers_on_scene.Officersarrivedonscene.FileName, officers_on_scene.Officershavearrived.FileName, officers_on_scene.Officersonscene.FileName, officers_on_scene.Officersonsite.FileName }.PickRandom());
        ScannerList.Add(new List<string>() { no_further_units.Noadditionalofficersneeded.FileName, no_further_units.Noadditionalofficersneeded1.FileName, no_further_units.Nofurtherunitsrequired.FileName, no_further_units.WereCode4Adam.FileName, no_further_units.Code4Adamnoadditionalsupportneeded.FileName
        , stand_down.ReturnToPatrol.FileName, stand_down.ReturnToPatrol1.FileName, stand_down.ReturnToPatrol2.FileName}.PickRandom());
        Subtitles += "Officers on site, we are Code 4-ADAM. No additional officers needed";
        ReportGenericEnd(ref ScannerList, NearType.Nothing, ref Subtitles, ref Notification, WantedLevelScript.LastWantedCenterPosition);
        PlayAudioList(new DispatchAudioEvent(ScannerList, Subtitles, Notification,true,false));
    }
    public static void LostVisualOnSuspect(DispatchQueueItem ItemToPlay)
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        DispatchNotification Notification = new DispatchNotification("Police Scanner", "~g~Status~s~", "Lost Visual");
        ReportGenericStart(ref ScannerList, ref Subtitles, AttentionType.Nobody, ReportType.Nobody, WantedLevelScript.LastWantedCenterPosition);
        ScannerList.Add(new List<string>() { suspect_eluded_pt_2.AllUnitsStayInTheArea.FileName, suspect_eluded_pt_2.AllUnitsRemainOnAlert.FileName, suspect_eluded_pt_2.AllUnitsStandby.FileName, suspect_eluded_pt_2.AllUnitsStayInTheArea.FileName, suspect_eluded_pt_2.AllUnitsRemainOnAlert.FileName }.PickRandom());
        Subtitles += "All units standby";
        ReportGenericEnd(ref ScannerList, NearType.Nothing, ref Subtitles, ref Notification, WantedLevelScript.LastWantedCenterPosition);
        PlayAudioList(new DispatchAudioEvent(ScannerList, Subtitles, Notification,true,false));
    }
    public static void ResumePatrol(DispatchQueueItem ItemToPlay)
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        DispatchNotification Notification = new DispatchNotification("Police Scanner", "~g~Status~s~", "Resume Patrol");
        ReportGenericStart(ref ScannerList, ref Subtitles, AttentionType.AllUnits, ReportType.Nobody, WantedLevelScript.LastWantedCenterPosition);
        ScannerList.Add(new List<string>() { officer_begin_patrol.Beginpatrol.FileName, officer_begin_patrol.Beginbeat.FileName, officer_begin_patrol.Assigntopatrol.FileName, officer_begin_patrol.Proceedtopatrolarea.FileName, officer_begin_patrol.Proceedwithpatrol.FileName }.PickRandom());
        Subtitles += " ~g~proceed with patrol~s~";
        ReportGenericEnd(ref ScannerList, NearType.Nothing, ref Subtitles, ref Notification, WantedLevelScript.LastWantedCenterPosition);
        PlayAudioList(new DispatchAudioEvent(ScannerList, Subtitles, Notification,true,false));
    }
    public static void SuspectReacquired(DispatchQueueItem ItemToPlay)
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        DispatchNotification Notification = new DispatchNotification("Police Scanner", string.Format("Suspect {0}", PedSwap.SuspectName), "");

        if (1==0)//WantedLevelScript.CurrentCrimes.KillingPolice.HasBeenWitnessedByPolice || WantedLevelScript.CurrentCrimes.KillingCivilians.HasBeenWitnessedByPolice)
        {
            ReportGenericStart(ref ScannerList, ref Subtitles, AttentionType.Nobody, ReportType.Officers, Game.LocalPlayer.Character.Position);
            ScannerList.Add(crime_wanted_felon_on_the_loose.Awantedfelonontheloose.FileName);
            Subtitles += " ~r~A Wanted Felon~s~ on the loose";
            ScannerList.Add(new List<string>() { proceed_with_caution.Approachwithcaution.FileName, proceed_with_caution.Officersproceedwithcaution.FileName, proceed_with_caution.Proceedwithcaution.FileName }.PickRandom());
            Subtitles += " ~s~proceed with caution~s~";
        }
        else
        {
            ReportGenericStart(ref ScannerList, ref Subtitles, AttentionType.Nobody, ReportType.Nobody, Game.LocalPlayer.Character.Position);
            ScannerList.Add(new List<string>() { suspect_last_seen.SuspectSpotted.FileName, suspect_last_seen.TargetSpotted.FileName, suspect_last_seen.SuspectSpotted.FileName, suspect_last_seen.TargetSpotted.FileName, suspect_last_seen.SuspectSpotted.FileName }.PickRandom());
            Subtitles += "~r~Suspect spotted~s~";
        }
        if (WantedLevelScript.CurrentCrimes.CommittedAnyCrimes)
            Notification.Text += "Wanted For:" + WantedLevelScript.CurrentCrimes.PrintCrimes();
        ReportGenericEnd(ref ScannerList, NearType.Zone, ref Subtitles, ref Notification, Game.LocalPlayer.Character.Position);


        PlayAudioList(new DispatchAudioEvent(ScannerList, Subtitles, Notification,true,false));
    }
    public static void SuspectSpotted(DispatchQueueItem ItemToPlay)
    {
        SuspectSpottedPreamble();
        List<string> ScannerList = new List<string>();   
        string Subtitles = "";  
        DispatchNotification Notification = new DispatchNotification("Police Scanner", "~g~Status~s~", "Suspect Spotted");
        ReportGenericStart(ref ScannerList, ref Subtitles, AttentionType.Nobody, ReportType.Nobody, Vector3.Zero);
        if (!ReportHeadingAndStreet(ref ScannerList, ref Subtitles, ref Notification) && ItemToPlay.ReportedBy == ReportType.Officers)
        {
            List<string> Possibilites = new List<string>() { spot_suspect_cop_01.HASH0601EE8E.FileName, spot_suspect_cop_01.HASH06A36FCF.FileName, spot_suspect_cop_01.HASH08E3F451.FileName, spot_suspect_cop_01.HASH0C703B6A.FileName, spot_suspect_cop_01.HASH13478918.FileName, spot_suspect_cop_01.HASH17551134.FileName, spot_suspect_cop_01.HASH1A3056EA.FileName, spot_suspect_cop_01.HASH1B3A58FF.FileName };
            ScannerList.Add(Possibilites.PickRandom());
            Subtitles += "Dispatch, we have ~r~eyes on~r~ the suspect";
        }
        ReportGenericEnd(ref ScannerList, NearType.Nothing, ref Subtitles, ref Notification, Vector3.Zero);

        PlayAudioList(new DispatchAudioEvent(ScannerList, Subtitles, Notification,true,false));
    }
    private static void SuspectSpottedPreamble()
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        DispatchNotification Notification = new DispatchNotification("Police Scanner", "~g~Status~s~", "Suspect Spotted");
        ReportGenericStart(ref ScannerList, ref Subtitles, AttentionType.Nobody, ReportType.Nobody, Vector3.Zero);
        if (PlayerState.IsInVehicle)
        {
            if (PlayerLocation.PlayerRecentlyGotOnFreeway)
            {
                List<string> Possibilites = new List<string>() { s_m_y_cop_white_full_01.SuspectEnteringTheFreeway.FileName, s_m_y_cop_white_full_01.EnteredFreeway.FileName };
                ScannerList.Add(Possibilites.PickRandom());
                Subtitles += "Dispatch, suspect has entered the freeway";
            }
            else if (PlayerLocation.PlayerRecentlyGotOffFreeway)
            {
                List<string> Possibilites = new List<string>() { s_m_y_cop_white_full_01.LeavingFreeway.FileName, s_m_y_cop_white_full_01.SuspectLeavingTheFreeway.FileName };
                ScannerList.Add(Possibilites.PickRandom());
                Subtitles += "Dispatch, suspect has left the freeway";
            }
            else
            {
                return;
            }
        }
        else
        {
            List<string> Possibilites = new List<string>() { s_m_y_cop_white_full_01.DispatchSuspectOnFoot.FileName, s_m_y_cop_white_full_01.GotEyesHesOnFoot.FileName, s_m_y_cop_white_full_01.HaveVisualSuspectOnFoot.FileName, s_m_y_cop_white_full_01.SuspectIsOnFoot.FileName };
            ScannerList.Add(Possibilites.PickRandom());
            Subtitles += "Dispatch, suspect on foot";
        }
        ReportGenericEnd(ref ScannerList, NearType.Nothing, ref Subtitles, ref Notification, Vector3.Zero);
        PlayAudioList(new DispatchAudioEvent(ScannerList, Subtitles, Notification, true, false));
    }

    public static void PopQuizHotShot(DispatchQueueItem ItemToPlay)
    {
        List<string> ScannerList = new List<string>
        {
            SpeedQuotes.PopQuizHotShot.FileName
        };
        PlayAudioList(new DispatchAudioEvent(ScannerList, "Pop quiz hot shot"));
    }

    //Starting
    private static void ReportGenericStart(ref List<string> ScannerList,ref string Subtitles, AttentionType WhoToNotify, ReportType ReportedBy, Vector3 PlaceToReport)
    {
        ScannerList.Add(new List<string> { AudioBeeps.Radio_Start_1.FileName/*, AudioBeeps.Radio_Start_2.FileName*/ }.PickRandom());
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
            if(SubsBlank)
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
    private static void ReportGenericEnd(ref List<string> ScannerList,NearType Near, ref string Subtitles, ref DispatchNotification Notification, Vector3 LocationToReport)
    {
        if(Near == NearType.Zone)
        {
            AddZone(ref ScannerList,ref Subtitles, LocationToReport,ref Notification);
        }
        else if(Near == NearType.HeadingAndZone)
        {
            AddHeading(ref ScannerList,ref Subtitles);
            AddZone(ref ScannerList, ref Subtitles, LocationToReport,ref Notification);
        }
        else if (Near == NearType.HeadingAndStreet)
        {
            AddHeading(ref ScannerList, ref Subtitles);  
            if (!AddStreet(ref ScannerList, ref Subtitles,ref Notification))
                AddZone(ref ScannerList,ref Subtitles, LocationToReport,ref Notification);
        }
        else if (Near == NearType.HeadingStreetAndZone)
        {
            AddHeading(ref ScannerList,ref Subtitles);
            AddStreet(ref ScannerList, ref Subtitles, ref Notification);
            AddZone(ref ScannerList,ref Subtitles, LocationToReport, ref Notification);
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
        ScannerList.Add(new List<string> { AudioBeeps.Radio_End_1.FileName/*, AudioBeeps.Radio_End_2.FileName*/ }.PickRandom());
    }
    public static bool AddHeading(ref List<string> ScannerList, ref string Subtitles)
    {
        if (Police.AnyRecentlySeenPlayer)
        {
            ScannerList.Add((new List<string>() { suspect_heading.TargetLastSeenHeading.FileName, suspect_heading.TargetReportedHeading.FileName, suspect_heading.TargetSeenHeading.FileName, suspect_heading.TargetSpottedHeading.FileName }).PickRandom());
            Subtitles += " ~s~suspect heading~s~";
            string heading = General.GetSimpleCompassHeading(Game.LocalPlayer.Character.Heading);
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

    public static bool ReportHeadingAndStreet(ref List<string> ScannerList, ref string Subtitles,ref DispatchNotification Notification)
    {
        if (PlayerLocation.PlayerCurrentStreet != null && PlayerLocation.PlayerCurrentStreet.DispatchFile != "")
        {
            ScannerList.Add((new List<string>() { suspect_heading.TargetLastSeenHeading.FileName, suspect_heading.TargetReportedHeading.FileName, suspect_heading.TargetSeenHeading.FileName, suspect_heading.TargetSpottedHeading.FileName }).PickRandom());
            Subtitles += "~r~Target spotted~s~ heading~s~";
            Notification.Text += "~n~Heading";
            string heading = General.GetSimpleCompassHeading(Game.LocalPlayer.Character.Heading);
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
            //Debugging.WriteToLog("StolenVehicles", String.Format("Vehicle {0} was just reported stolen", StolenCar.VehicleEnt.Handle));

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
    private static AttentionType GetWhoToNotify(ReportType ReportedBy)
    {
        AttentionType WhoToNotifiy;
        if (ReportedBy == ReportType.Officers)
            WhoToNotifiy = AttentionType.Nobody;
        else
            WhoToNotifiy = AttentionType.LocalUnits;

        return WhoToNotifiy;
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
    private static void AddLethalForceAuthorized(ref List<string> ScannerList, ref string Subtitles,ref DispatchNotification Notification)
    {
        if (!ReportedLethalForceAuthorized)
        {
            ScannerList.Add(new List<string>() { lethal_force.Useofdeadlyforceauthorized.FileName, lethal_force.Useofdeadlyforceisauthorized.FileName, lethal_force.Useofdeadlyforceisauthorized1.FileName, lethal_force.Useoflethalforceisauthorized.FileName, lethal_force.Useofdeadlyforcepermitted1.FileName }.PickRandom());
            Subtitles += " use of ~r~Deadly Force~s~ is authorized";
            Notification.Text += "~n~Use of ~r~Deadly Force~s~ is authorized";
            ReportedLethalForceAuthorized = true;
        }
    }
    public static void AddVehicleDescription(VehicleExt VehicleDescription, ref List<string> ScannerList, bool IncludeLicensePlate,ref string Subtitles,ref DispatchNotification Notification, bool IncludeAAudio, bool IncludePoliceDescription,bool IncludeIn, bool AddPossiblyStolenStolen)
    {
        //if (VehicleDescription == null)
        //    return;
        //if (VehicleDescription.HasBeenDescribedByDispatch)
        //    return;
        //else
        //    VehicleDescription.HasBeenDescribedByDispatch = true;

        //Notification.Text += "~n~Vehicle:~s~";

        //if (VehicleDescription.VehicleEnt.IsPoliceVehicle)
        //{
        //    if (IncludePoliceDescription)
        //    {
        //        Subtitles += " ~r~Stolen Police Car~s~";
        //        Notification.Text += " ~r~Stolen Police Car~s~";
        //        ScannerList.Add(vehicle_category.PoliceSedan01.FileName);
        //    }
        //}
        //else
        //{
        //    if (IncludeIn)
        //    {
        //        ScannerList.Add(new List<string>() { conjunctives.Inuhh2.FileName, conjunctives.Inuhh3.FileName }.PickRandom());
        //        if (VehicleDescription.IsStolen & AddPossiblyStolenStolen)
        //        {

        //            ScannerList.Add(crime_stolen_vehicle.Apossiblestolenvehicle.FileName);
        //            Subtitles += " ~s~in a possible stolen vehicle~s~";
        //            Notification.Text += " ~r~Stolen~s~";
        //        }
        //    }

        //   // Vehicles.VehicleInfo VehicleInformation = Vehicles.GetVehicleInfo(VehicleDescription);
        //    System.Drawing.Color BaseColor = GetBaseColor(VehicleDescription.DescriptionColor);
        //    ColorLookup LookupColor = ColorLookups.Where(x => x.BaseColor == BaseColor).PickRandom();
        //    string ManufacturerScannerFile;
        //    string VehicleClassScannerFile;
        //    if (VehicleInformation != null)
        //    {
        //       // Debugging.WriteToLog("Description", string.Format("VehicleInformation.ModelScannerFile {0}", VehicleInformation.ModelScannerFile.ToString()));
        //        ManufacturerScannerFile = GetManufacturerScannerFile(VehicleInformation.Manufacturer);
        //        VehicleClassScannerFile = GetVehicleClassScannerFile(VehicleInformation.VehicleClass);
        //        if (LookupColor != null && (VehicleInformation.ModelScannerFile != "" || VehicleInformation.ModelScannerFile != "" || VehicleClassScannerFile != ""))
        //        {
        //            if (IncludeAAudio)
        //            {
        //                Subtitles += " ~s~a~s~";
        //                ScannerList.Add(new List<string>() { conjunctives.A01.FileName, conjunctives.A02.FileName }.PickRandom());
        //            }
        //            if (VehicleInformation.VehicleClass != Vehicles.VehicleClass.Emergency)
        //            {
        //                Subtitles += " ~s~" + LookupColor.BaseColor.Name + "~s~";
        //                Notification.Text += " ~s~" + LookupColor.BaseColor.Name + "~s~";
        //                ScannerList.Add(LookupColor.ScannerFile);
        //            }
        //            if (ManufacturerScannerFile != "")
        //            {
        //                Subtitles += " ~p~" + VehicleInformation.Manufacturer + "~s~";
        //                Notification.Text += " ~p~" + VehicleInformation.Manufacturer + "~s~";
        //                ScannerList.Add(ManufacturerScannerFile);
        //            }
        //            if (VehicleInformation.ModelScannerFile != "")
        //            {
        //                string ModelName = VehicleInformation.Name.ToLower();
        //                unsafe
        //                {
        //                    IntPtr ptr = NativeFunction.CallByName<IntPtr>("GET_DISPLAY_NAME_FROM_VEHICLE_MODEL", VehicleDescription.VehicleEnt.Model.Hash);
        //                    ModelName = Marshal.PtrToStringAnsi(ptr);
        //                }
        //                unsafe
        //                {
        //                    IntPtr ptr2 = NativeFunction.CallByHash<IntPtr>(0x7B5280EBA9840C72, ModelName);
        //                    ModelName = Marshal.PtrToStringAnsi(ptr2);
        //                }
        //                if (ModelName == "CARNOTFOUND" || ModelName == "NULL")
        //                    ModelName = VehicleInformation.Name.ToLower();

        //                Subtitles += " ~g~" + ModelName + "~s~";
        //                Notification.Text += " ~g~" + ModelName + "~s~";
        //                ScannerList.Add(VehicleInformation.ModelScannerFile);
        //            }
        //            else if (VehicleClassScannerFile != "")
        //            {
        //                string subText = Vehicles.GetVehicleTypeSubtitle(VehicleInformation.VehicleClass);
        //                Subtitles += " ~b~" + subText + "~s~";
        //                Notification.Text += " ~b~" + subText + "~s~";
        //                ScannerList.Add(VehicleClassScannerFile);
        //            }
        //        }
        //    }        
        //}
        //if (IncludeLicensePlate)
        //{
        //    ScannerList.Add(suspect_license_plate.SuspectsLicensePlate.FileName);
        //    Subtitles += "~s~. Suspects License Plate: ~y~" + VehicleDescription.OriginalLicensePlate.PlateNumber.ToUpper() + "~s~";//VehicleDescription.VehicleEnt.LicensePlate.ToUpper() + "~s~";
        //    Notification.Text += "~n~License Plate: ~y~" + VehicleDescription.OriginalLicensePlate.PlateNumber.ToUpper() + "~s~";
        //    foreach (char c in VehicleDescription.OriginalLicensePlate.PlateNumber)
        //    {
        //        string DispatchFileName = LettersAndNumbersLookup.Where(x => x.AlphaNumeric == c).PickRandom().ScannerFile;
        //        ScannerList.Add(DispatchFileName);
        //    }
        //}
    }
    public static void AddVehicleClass(VehicleExt VehicleDescription, ref List<string> ScannerList, ref string Subtitles, ref DispatchNotification Notification)
    {
        //if(VehicleDescription != null)
        //{
        //    Vehicles.VehicleInfo VehicleInformation = Vehicles.GetVehicleInfo(VehicleDescription);
        //    if (VehicleInformation != null)
        //    {
        //        string VehicleClassScannerFile = GetVehicleClassScannerFile(VehicleInformation.VehicleClass);
        //        if (VehicleClassScannerFile != "")
        //        {
        //            string subText = Vehicles.GetVehicleTypeSubtitle(VehicleInformation.VehicleClass);
        //            Subtitles += " driving a ~b~" + subText + "~s~";
        //            Notification.Text += "~n~Vehicle: ~b~" + subText + "~s~";
        //            ScannerList.Add(conjunctives.Drivinga.FileName);
        //            ScannerList.Add(VehicleClassScannerFile);
        //            return;
        //        }
        //    }
        //}
        //Notification.Text += "~n~Vehicle: Unknown";
        //Subtitles += " driving a um";
        //ScannerList.Add(conjunctives.DrivingAUmmm.FileName);
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
    public static void AddSpeed(ref List<string> ScannerList,float Speed, ref string Subtitles,ref DispatchNotification Notification)
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

        public AvailableDispatch Type { get; set; }
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




        public bool ReportCharctersPosition { get; set; } = true;
        public AttentionType WhoToNotify { get; set; } = AttentionType.Nobody;
        public string NotificationTitle { get; set; } = "Police Scanner";
        public string NotificationSubtitle { get; set; }
        public string NotificationText { get; set; }
        public string SubtitleText { get; set; }
        public List<DispatchAudioList> PossibleAudioToPlay { get; set; } = new List<DispatchAudioList>();
        public bool MarkVehicleAsStolen { get; set; } = false;
        public bool IncludeDrivingVehicle { get; set; } = false;

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
        public DispatchAudioEvent(List<string> _SoundsToPlay,string _Subtitles)
        {
            SoundsToPlay = _SoundsToPlay;
            Subtitles = _Subtitles;
        }
        public DispatchAudioEvent(List<string> _SoundsToPlay,string _Subtitles, DispatchNotification _NotificationToDisplay)
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
        public string Title;
        public string Subtitle;
        public string Text;
        public string TextureDict = "CHAR_CALL911";
        public string TextureName = "CHAR_CALL911";
   
        public DispatchNotification(string _Title,string _Subtitle,string _Text)
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
        public AvailableDispatch Type { get; set; }
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
        public DispatchQueueItem(AvailableDispatch _Type,int _Priority)
        {
            Type = _Type;
            Priority = _Priority;
        }
        public DispatchQueueItem(AvailableDispatch _Type, int _Priority,bool _ResultsInLethalForce)
        {
            Type = _Type;
            Priority = _Priority;
            ResultsInLethalForce = _ResultsInLethalForce;
        }
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

