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
using static ScannerAudio;
using static Zones;

public static class DispatchAudio
{
    private static List<uint> NotificationHandles = new List<uint>();
    private static readonly Random rnd;
    private static WaveOutEvent outputDevice;
    private static AudioFileReader audioFile;
    private static bool ExecutingQueue = false;
    private static List<DispatchQueueItem> DispatchQueue = new List<DispatchQueueItem>();
    private static List<DispatchLettersNumber> LettersAndNumbersLookup = new List<DispatchLettersNumber>();
    private static List<ColorLookup> ColorLookups = new List<ColorLookup>();
    private static List<string> DamagedScannerAliases = new List<string>();
    public static bool CancelAudio;
    public static bool IsPlayingAudio;

    private static uint GameTimeLastDisplayedSubtitle;
    public static bool ReportedLethalForceAuthorized = false;
    public static bool ReportedWeaponsFree = false;

    public static bool AudioPlaying
    {
        get
        {
            return outputDevice != null;
        }
    }
    public static bool IsRunning { get; set; } = true;
    public enum ReportDispatch
    {
        ReportShotsFired = 0,
        ReportCarryingWeapon = 1,
        ReportOfficerDown = 2,
        ReportAssualtOnOfficer = 3,
        ReportThreateningWithFirearm = 4,
        ReportSuspectLastSeen = 5,
        ReportSuspectArrested = 6,
        ReportSuspectWasted = 7,
        ReportLethalForceAuthorized = 8,
        ReportStolenVehicle = 9,
        ReportSuspectLost = 10,
        ReportSpottedStolenCar = 11,
        ReportPedHitAndRun = 12,
        ReportVehicleHitAndRun = 13,
        ReportRecklessDriver = 14,
        ReportFelonySpeeding = 15,
        ReportWeaponsFree = 16,
        ReportSuspiciousActivity = 17,
        ReportSuspiciousVehicle = 18,
        ReportGrandTheftAuto = 19,
        ReportSuspectSpotted = 20,
        ReportIncreasedWanted = 21,
        ReportRunningRed = 22,
        ReportLocalSuspectSpotted = 23,
        ReportLowLevelCriminalActivity = 24,
        ReportLowLevelShotsFired = 25,
        ReportLowLevelGrandTheftAuto = 26,
        ReportPersonOfInterestExpire = 27,
        ReportSuspectLostVisual = 28,
        ReportLowLevelTerroristActivity = 29,
        ReportTrespassingOnGovernmentProperty = 30,
        ReportChangedVehicle = 31,
        ReportLowLevelCiviliansKilled = 32,
        ReportLowLevelCiviliansInjured = 33,
        ReportLowLevelCiviliansShot = 34,
        ReportCivilianKilled = 35,
        ReportStolenAirVehicle = 36,
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
    private enum ReportType
    {
        Nobody = -1,
        Civilians = 0,
        Officers = 1,
        WeHave = 2,
    }
    private enum AttentionType
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
        ReportedLethalForceAuthorized = false;
        ReportedWeaponsFree = false;
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

    }
    public static void PlayAudioList(DispatchAudioEvent MyAudioEvent)
    {
        if (MyAudioEvent.CheckSight && !PoliceScanning.CopPeds.Any(x => x.canSeePlayer))
            return;

        GameFiber PlayAudioList = GameFiber.StartNew(delegate
        {
            while (IsPlayingAudio)
                GameFiber.Yield();

            foreach (string audioname in MyAudioEvent.SoundsToPlay)
            {
                PlayAudio(audioname);
                if (MyAudioEvent.NotificationToDisplay != null && Settings.DispatchNotifications)
                {
                    NotificationHandles.Add(Game.DisplayNotification(MyAudioEvent.NotificationToDisplay.TextureDict, MyAudioEvent.NotificationToDisplay.TextureName, MyAudioEvent.NotificationToDisplay.Title, MyAudioEvent.NotificationToDisplay.Subtitle, MyAudioEvent.NotificationToDisplay.Text));
                }
                while (IsPlayingAudio)
                {
                    if (MyAudioEvent.Subtitles != "" && Settings.DispatchSubtitles && Game.GameTime - GameTimeLastDisplayedSubtitle >= 1500)
                    {
                        Game.DisplaySubtitle(MyAudioEvent.Subtitles, 2000);
                        GameTimeLastDisplayedSubtitle = Game.GameTime;
                    }
                    GameFiber.Yield();
                }
                if (CancelAudio)
                {
                    CancelAudio = false;
                    break;
                }
            }
        }, "PlayAudioList");
        Debugging.GameFibers.Add(PlayAudioList);
    }
    private static void PlayAudio(String _Audio)
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
                    Volume = Settings.DispatchAudioVolume
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
            Game.Console.Print(e.Message);
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
        if (!DispatchQueue.Any(x => x.Type == _ItemToAdd.Type))
            DispatchQueue.Add(_ItemToAdd);
    }
    public static void PlayDispatchQueue()
    {

        if (!Settings.DispatchAudio)
        {
            DispatchQueue.Clear();
            return;
        }


        if (DispatchQueue.Count > 0 && !ExecutingQueue)
        {
            LocalWriteToLog("PlayDispatchQueue", "Delegate Started");
            ExecutingQueue = true;
            GameFiber PlayDispatchQueue = GameFiber.StartNew(delegate
            {
                GameFiber.Sleep(rnd.Next(1500,2500));

                if (Settings.DispatchAudioOnlyHighPriority)
                {
                    DispatchQueue.RemoveAll(x => x.Priority > 3);
                }

                // Remove and order items, needs to be fixed up 
                if (Police.CurrentPoliceState == Police.PoliceState.DeadlyChase)
                {
                    DispatchQueue.RemoveAll(x => x.Priority > 3 && x.Type != ReportDispatch.ReportSuspectArrested && x.Type != ReportDispatch.ReportSuspectWasted && !x.IsAmbient);
                }

                if (DispatchQueue.Any(x => x.Priority <= 1) && DispatchQueue.Any(x => x.Priority > 1 && x.Type == ReportDispatch.ReportLethalForceAuthorized))
                {
                    DispatchQueue.RemoveAll(x => x.Priority > 1);
                }
                if (DispatchQueue.Any(x => x.ResultsInLethalForce && x.Type != ReportDispatch.ReportLethalForceAuthorized))
                {
                    DispatchQueue.RemoveAll(x => x.Type == ReportDispatch.ReportLethalForceAuthorized);
                }
                if (DispatchQueue.Any(x => x.ResultsInStolenCarSpotted && x.Type != ReportDispatch.ReportSpottedStolenCar))
                {
                    DispatchQueue.RemoveAll(x => x.Type == ReportDispatch.ReportSpottedStolenCar);
                }
                if (DispatchQueue.Any(x => x.ResultsInStolenCarSpotted && x.Type != ReportDispatch.ReportSuspiciousVehicle))
                {
                    DispatchQueue.RemoveAll(x => x.Type == ReportDispatch.ReportSuspiciousVehicle);
                }
                if (DispatchQueue.Any(x => !x.IsAmbient))
                {
                    DispatchQueue.RemoveAll(x => x.IsAmbient);
                }


                if (DispatchQueue.Any(x => x.Type == ReportDispatch.ReportChangedVehicle))
                {
                    DispatchQueue.RemoveAll(x => x.Type == ReportDispatch.ReportLocalSuspectSpotted || x.Type == ReportDispatch.ReportSuspectSpotted || x.Type == ReportDispatch.ReportSpottedStolenCar);
                }

                if (DispatchQueue.Where(x => x.IsTrafficViolation).Count() > 1)
                {
                    DispatchQueueItem HighestItem = DispatchQueue.Where(x => x.IsTrafficViolation).OrderBy(x => x.Priority).FirstOrDefault();
                    DispatchQueue.RemoveAll(x => x.IsTrafficViolation);
                    if (HighestItem != null)
                    {
                        DispatchQueue.Add(HighestItem);
                    }
                }

                while (DispatchQueue.Count > 0)
                {
                    DispatchQueueItem Item = DispatchQueue[0];
                    if (Item.Type == ReportDispatch.ReportAssualtOnOfficer)
                        ReportAssualtOnOfficer();
                    else if (Item.Type == ReportDispatch.ReportCarryingWeapon)
                        ReportCarryingWeapon(Item.WeaponToReport);
                    else if (Item.Type == ReportDispatch.ReportFelonySpeeding)
                        ReportFelonySpeeding(Item.VehicleToReport, Item.Speed);
                    else if (Item.Type == ReportDispatch.ReportLethalForceAuthorized)
                        ReportLethalForceAuthorized();
                    else if (Item.Type == ReportDispatch.ReportOfficerDown)
                        ReportOfficerDown();
                    else if (Item.Type == ReportDispatch.ReportPedHitAndRun)
                        ReportPedHitAndRun(Item.VehicleToReport);
                    else if (Item.Type == ReportDispatch.ReportRecklessDriver)
                        ReportRecklessDriver(Item.VehicleToReport);
                    else if (Item.Type == ReportDispatch.ReportShotsFired)
                        ReportShotsFired();
                    else if (Item.Type == ReportDispatch.ReportSpottedStolenCar)
                        ReportSpottedStolenCar(Item.Speed);
                    else if (Item.Type == ReportDispatch.ReportStolenVehicle)
                        ReportStolenVehicle(Item.VehicleToReport);
                    else if (Item.Type == ReportDispatch.ReportSuspectArrested)
                        ReportSuspectArrested();
                    else if (Item.Type == ReportDispatch.ReportSuspectLastSeen)
                        ReportSuspectLastSeen();
                    else if (Item.Type == ReportDispatch.ReportSuspectLost)
                        ReportSuspectLost();
                    else if (Item.Type == ReportDispatch.ReportSuspectWasted)
                        ReportSuspectWasted();
                    else if (Item.Type == ReportDispatch.ReportThreateningWithFirearm)
                        ReportThreateningWithFirearm();
                    else if (Item.Type == ReportDispatch.ReportVehicleHitAndRun)
                        ReportVehicleHitAndRun(Item.VehicleToReport);
                    else if (Item.Type == ReportDispatch.ReportWeaponsFree)
                        ReportWeaponsFree();
                    else if (Item.Type == ReportDispatch.ReportSuspiciousActivity)
                        ReportSuspiciousActivity();
                    else if (Item.Type == ReportDispatch.ReportSuspiciousVehicle)
                        ReportSuspiciousVehicle(Item.VehicleToReport);
                    else if (Item.Type == ReportDispatch.ReportGrandTheftAuto)
                        ReportGrandTheftAuto();
                    else if (Item.Type == ReportDispatch.ReportSuspectSpotted)
                        ReportSuspectSpotted();
                    else if (Item.Type == ReportDispatch.ReportIncreasedWanted)
                        ReportIncreasedWanted(Item.ResultsInLethalForce);
                    else if (Item.Type == ReportDispatch.ReportRunningRed)
                        ReportRunningRed(Item.VehicleToReport);
                    else if (Item.Type == ReportDispatch.ReportLocalSuspectSpotted)
                        ReportLocalSuspectSpotted();
                    else if (Item.Type == ReportDispatch.ReportLowLevelCriminalActivity)
                        ReportLowLevelCriminalActivity();
                    else if (Item.Type == ReportDispatch.ReportLowLevelShotsFired)
                        ReportLowLevelShotsFired();
                    else if (Item.Type == ReportDispatch.ReportLowLevelGrandTheftAuto)
                        ReportLowLevelGrandTheftAuto();
                    else if (Item.Type == ReportDispatch.ReportPersonOfInterestExpire)
                        ReportPersonOfInterestExpire();
                    else if (Item.Type == ReportDispatch.ReportSuspectLostVisual)
                        ReportSuspectLostVisual();
                    else if (Item.Type == ReportDispatch.ReportLowLevelTerroristActivity)
                        ReportLowLevelTerroristActivity();
                    else if (Item.Type == ReportDispatch.ReportTrespassingOnGovernmentProperty)
                        ReportTrespassingOnGovernmentProperty();
                    else if (Item.Type == ReportDispatch.ReportChangedVehicle)
                        ReportChangedVehicle(Item.VehicleToReport);
                    else if (Item.Type == ReportDispatch.ReportLowLevelCiviliansKilled)
                        ReportLowLevelCiviliansKilled();
                    else if (Item.Type == ReportDispatch.ReportLowLevelCiviliansInjured)
                        ReportLowLevelCiviliansInjured();
                    else if (Item.Type == ReportDispatch.ReportLowLevelCiviliansShot)
                        ReportLowLevelCiviliansShot();
                    else if (Item.Type == ReportDispatch.ReportCivilianKilled)
                        ReportCivilianKilled();
                    else if (Item.Type == ReportDispatch.ReportStolenAirVehicle)
                        ReportStolenAirVehicle(Item.VehicleToReport);

                    DispatchQueue.RemoveAt(0);

                    if(Item.ResultingWantedLevel > 0)
                    {
                        Police.SetWantedLevel(Item.ResultingWantedLevel, string.Format("Set Wanted After Dispatch: {0}",Item.Type));
                    }

                }
                ExecutingQueue = false;
            }, "PlayDispatchQueue");
            Debugging.GameFibers.Add(PlayDispatchQueue);
        }
    }
    public static void ClearDispatchQueue()
    {
        DispatchQueue.Clear();
    }

    //Starting
    private static void ReportGenericStart(ref List<string> ScannerList,ref string Subtitles, AttentionType WhoToNotify, ReportType ReportedBy, Vector3 PlaceToReport)
    {
        ScannerList.Add(AudioBeeps.AudioStart());
        Subtitles = "";
        if (WhoToNotify == AttentionType.AllUnits)
        {
            ScannerList.Add(new List<string>() { attention_all_units_gen.Attentionallunits.FileName, attention_all_units_gen.Attentionallunits1.FileName, attention_all_units_gen.Attentionallunits3.FileName, attention_all_units_gen.Attentionallunits3.FileName }.PickRandom());
            Subtitles += "Attention all units,";
        }
        else if (WhoToNotify == AttentionType.LocalUnits)
        {
            GetLocalUnitsStart(ref ScannerList, ref Subtitles, PlaceToReport);
        }
        else if (WhoToNotify == AttentionType.SpecificUnits)
        {
            GetLocalUnitsStart(ref ScannerList,ref Subtitles, PlaceToReport);
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
    private static bool GetLocalUnitsStart(ref List<string> ScannerList,ref string Subtitles,Vector3 PlaceToReport)
    {
        Zone MyZone = GetZoneAtLocation(PlaceToReport);
        if (MyZone != null && MyZone.DispatchUnitAudio.Any())
        {
            ScannerList.Add(MyZone.DispatchUnitAudio.PickRandom());
            Subtitles += "Attention any ~g~" + MyZone.DispatchUnitName + "~s~,";
            return true;
        }
        else
        {
            return false;
        }
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
        ScannerList.Add(AudioBeeps.Radio_End_1.FileName);
    }
    public static bool AddHeading(ref List<string> ScannerList, ref string Subtitles)
    {
        if (Police.AnyPoliceRecentlySeenPlayer)
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
            Notification.Text += " ~s~on ~HUD_COLOUR_YELLOWLIGHT~" + MyStreet.Name + "~s~";
            Subtitles += " ~s~on ~HUD_COLOUR_YELLOWLIGHT~" + MyStreet.Name + "~s~";

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
            Subtitles += " ~s~near ~p~" + MyZone.TextName + "~s~";
            Notification.Text += " ~s~near ~p~" + MyZone.TextName + "~s~";
            return true;
        }
        return false;
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
    //Traffic
    public static void ReportSpottedStolenCar(float Speed)
    {
        string Subtitles = "";
        DispatchNotification Notification = new DispatchNotification("Police Scanner", "~o~Crime Observed~s~", "Driving a Stolen Vehicle");
        List<string> ScannerList = new List<string>();
        ReportGenericStart(ref ScannerList,ref Subtitles,AttentionType.LocalUnits,ReportType.Officers, Game.LocalPlayer.Character.Position);
        ScannerList.Add(new List<string>() { crime_person_in_a_stolen_car.Apersoninastolencar.FileName, crime_person_in_a_stolen_vehicle.Apersoninastolenvehicle.FileName, crime_person_in_a_stolen_car.Apersoninastolencar.FileName}.PickRandom());
        Subtitles += " a person in a ~r~Stolen Vehicle~s~";
        AddSpeed(ref ScannerList, Speed,ref Subtitles,ref Notification);
        ReportGenericEnd(ref ScannerList, NearType.HeadingAndStreet, ref Subtitles,ref Notification,Game.LocalPlayer.Character.Position);
        PlayAudioList(new DispatchAudioEvent(ScannerList, false, Subtitles,Notification));
    }
    public static void ReportPedHitAndRun(GTAVehicle vehicle)
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        DispatchNotification Notification = new DispatchNotification("Police Scanner", "~o~Crime Observed~s~", "Pedestrian Hit-and-Run");
        ReportGenericStart(ref ScannerList,ref Subtitles,AttentionType.LocalUnits, ReportType.Officers, Game.LocalPlayer.Character.Position);
        ScannerList.Add(new List<string>() { crime_ped_struck_by_veh.Apedestrianstruck.FileName, crime_ped_struck_by_veh.Apedestrianstruck1.FileName, crime_ped_struck_by_veh.Apedestrianstruckbyavehicle.FileName, crime_ped_struck_by_veh.Apedestrianstruckbyavehicle1.FileName }.PickRandom());
        Subtitles += " a ~r~Pedestrian Struck~s~";
        if (vehicle.IsStolen)
        {
            AddStolenVehicle(ref ScannerList,vehicle,ref Subtitles);
        }
        ReportGenericEnd(ref ScannerList, NearType.HeadingAndStreet, ref Subtitles, ref Notification, Game.LocalPlayer.Character.Position);
        PlayAudioList(new DispatchAudioEvent(ScannerList, false, Subtitles, Notification));
    }
    public static void ReportVehicleHitAndRun(GTAVehicle vehicle)
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        DispatchNotification Notification = new DispatchNotification("Police Scanner", "~o~Crime Observed~s~", "Hit-and-Run");
        ReportGenericStart(ref ScannerList, ref Subtitles, AttentionType.LocalUnits, ReportType.Officers, Game.LocalPlayer.Character.Position);
        ScannerList.Add(new List<string>() { crime_dangerous_driving.Dangerousdriving.FileName, crime_dangerous_driving.Dangerousdriving1.FileName, crime_reckless_driver.Arecklessdriver.FileName, crime_traffic_felony.Atrafficfelony.FileName }.PickRandom());
        Subtitles += " a ~r~Hit and Run~s~";
        if (vehicle.IsStolen)
        {
            AddStolenVehicle(ref ScannerList, vehicle, ref Subtitles);
        }
        //AddVehicleDescription(vehicle, ref ScannerList, false, ref Subtitles,true,false);
        ReportGenericEnd(ref ScannerList, NearType.HeadingAndStreet, ref Subtitles, ref Notification, Game.LocalPlayer.Character.Position);
        PlayAudioList(new DispatchAudioEvent(ScannerList, false, Subtitles, Notification));
    }
    public static void ReportRecklessDriver(GTAVehicle vehicle)
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        DispatchNotification Notification = new DispatchNotification("Police Scanner", "~o~Crime Observed~s~", "Reckless Driving");
        ReportGenericStart(ref ScannerList, ref Subtitles, AttentionType.LocalUnits, ReportType.Officers, Game.LocalPlayer.Character.Position);
        ScannerList.Add(new List<string>() { crime_reckless_driver.Arecklessdriver.FileName }.PickRandom());
        Subtitles += " a ~r~Reckless Driver~s~";
        if (vehicle.IsStolen)
        {
            AddStolenVehicle(ref ScannerList, vehicle, ref Subtitles);
        }
        //AddVehicleDescription(vehicle, ref ScannerList, false, ref Subtitles,true,false);
        ReportGenericEnd(ref ScannerList, NearType.HeadingAndStreet, ref Subtitles, ref Notification, Game.LocalPlayer.Character.Position);
        PlayAudioList(new DispatchAudioEvent(ScannerList, false, Subtitles, Notification));
    }
    private static void ReportRunningRed(GTAVehicle vehicle)
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        DispatchNotification Notification = new DispatchNotification("Police Scanner", "~o~Crime Observed~s~", "Running a Red Light");
        ReportGenericStart(ref ScannerList, ref Subtitles, AttentionType.LocalUnits, ReportType.Officers, Game.LocalPlayer.Character.Position);
        ScannerList.Add(new List<string>() { crime_person_running_a_red_light.Apersonrunningaredlight.FileName }.PickRandom());
        Subtitles += " a person ~r~Running a Red Light~s~";
        if (vehicle.IsStolen)
        {
            AddStolenVehicle(ref ScannerList, vehicle, ref Subtitles);
        }
        //AddVehicleDescription(vehicle, ref ScannerList, false, ref Subtitles,true,false);
        ReportGenericEnd(ref ScannerList, NearType.HeadingAndStreet, ref Subtitles, ref Notification, Game.LocalPlayer.Character.Position);
        PlayAudioList(new DispatchAudioEvent(ScannerList, false, Subtitles, Notification));
    }
    public static void ReportFelonySpeeding(GTAVehicle vehicle,float Speed)
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        DispatchNotification Notification = new DispatchNotification("Police Scanner", "~o~Crime Observed~s~", "Felony Speeding");
        ReportGenericStart(ref ScannerList, ref Subtitles, AttentionType.LocalUnits, ReportType.Officers, Game.LocalPlayer.Character.Position);
        ScannerList.Add(new List<string>() { crime_speeding_felony.Aspeedingfelony.FileName }.PickRandom());
        Subtitles += " a ~r~Speeding Felony~s~";
        AddSpeed(ref ScannerList, Speed, ref Subtitles,ref Notification);
        if (vehicle.IsStolen)
        {
            AddStolenVehicle(ref ScannerList, vehicle, ref Subtitles);
        }
        //AddVehicleDescription(vehicle, ref ScannerList, false, ref Subtitles,true,false);
        ReportGenericEnd(ref ScannerList, NearType.Nothing, ref Subtitles, ref Notification, Game.LocalPlayer.Character.Position);
        PlayAudioList(new DispatchAudioEvent(ScannerList, false, Subtitles, Notification));
    }

    public static void ReportShotsFired()
    {
        if (ReportedLethalForceAuthorized)
            return;
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        DispatchNotification Notification = new DispatchNotification("Police Scanner", "~o~Crime Observed~s~", "Shots Fired at an Officer");
        ReportGenericStart(ref ScannerList, ref Subtitles, AttentionType.Nobody, ReportType.Officers, Game.LocalPlayer.Character.Position);
        ScannerList.Add(crime_shots_fired_at_an_officer.Shotsfiredatanofficer.FileName);
        Subtitles += " ~r~Shots Fired at an Officer~s~";
        AddLethalForceAuthorized(ref ScannerList, ref Subtitles);
        ReportGenericEnd(ref ScannerList, NearType.Nothing,ref Subtitles, ref Notification, Game.LocalPlayer.Character.Position);
        PlayAudioList(new DispatchAudioEvent(ScannerList, false, Subtitles, Notification));
    }
    private static void ReportTrespassingOnGovernmentProperty()
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        DispatchNotification Notification = new DispatchNotification("Police Scanner", "~o~Crime Observed~s~", "Trespassing on Government Property");
        ReportGenericStart(ref ScannerList, ref Subtitles, AttentionType.Nobody, ReportType.Officers, Game.LocalPlayer.Character.Position);
        ScannerList.Add(crime_trespassing_on_government_property.Trespassingongovernmentproperty.FileName);
        Subtitles += " ~r~Trespassing on Government Property~s~";
        AddLethalForceAuthorized(ref ScannerList, ref Subtitles);
        ReportGenericEnd(ref ScannerList, NearType.Nothing, ref Subtitles, ref Notification, Game.LocalPlayer.Character.Position);
        PlayAudioList(new DispatchAudioEvent(ScannerList, false, Subtitles, Notification));
    }
    public static void ReportCarryingWeapon(GTAWeapon CarryingWeapon)
    {
        string Subtitles = "";
        List<string> ScannerList = new List<string>();
        ReportGenericStart(ref ScannerList,ref Subtitles,AttentionType.Nobody,ReportType.Officers, Game.LocalPlayer.Character.Position);
        ScannerList.Add(suspect_is.SuspectIs.FileName);
        DispatchNotification Notification = new DispatchNotification("Police Scanner", "~o~Crime Observed~s~", "Brandishing");
        if (CarryingWeapon == null)
        {
            ScannerList.Add(carrying_weapon.Carryingaweapon.FileName);
            Subtitles += " suspect is carrying a ~r~weapon~s~";
            Notification.Text += " a Weapon";
        }
        else if (CarryingWeapon.Name == "weapon_rpg")
        {
            ScannerList.Add(carrying_weapon.ArmedwithanRPG.FileName);
            Subtitles += " suspect is armed with an ~r~RPG~s~";
            Notification.Text += " an RPG";
        }
        else if (CarryingWeapon.Name == "weapon_bat")
        {
            ScannerList.Add(carrying_weapon.Armedwithabat.FileName);
            Subtitles += " suspect is armed with a ~r~bat~s~";
            Notification.Text += " a Bat";
        }
        else if (CarryingWeapon.Name == "weapon_grenadelauncher" || CarryingWeapon.Name == "weapon_grenadelauncher_smoke" || CarryingWeapon.Name == "weapon_compactlauncher")
        {
            ScannerList.Add(carrying_weapon.Armedwithagrenadelauncher.FileName);
            Subtitles += " suspect is armed with a ~r~grenade launcher~s~";
            Notification.Text += " a Grenade Launcher";
        }
        else if (CarryingWeapon.Name == "weapon_dagger" || CarryingWeapon.Name == "weapon_knife" || CarryingWeapon.Name == "weapon_switchblade")
        {
            ScannerList.Add(carrying_weapon.Armedwithaknife.FileName);
            Subtitles += " suspect is armed with a ~r~knife~s~";
            Notification.Text += " a Knife";
        }
        else if (CarryingWeapon.Name == "weapon_minigun")
        {
            ScannerList.Add(carrying_weapon.Armedwithaminigun.FileName);
            Subtitles += " suspect is armed with a ~r~minigun~s~";
            Notification.Text += " a Minigun";
        }
        else if (CarryingWeapon.Name == "weapon_sawnoffshotgun")
        {
            ScannerList.Add(carrying_weapon.Armedwithasawedoffshotgun.FileName);
            Subtitles += " suspect is armed with a ~r~sawed off shotgun~s~";
            Notification.Text += " Sawed Off Shotgun";
        }
        else if (CarryingWeapon.Category == GTAWeapon.WeaponCategory.LMG)
        {
            ScannerList.Add(carrying_weapon.Armedwithamachinegun.FileName);
            Subtitles += " suspect is armed with a ~r~machine gun~s~";
            Notification.Text += " a Machine Gun";
        }
        else if (CarryingWeapon.Category == GTAWeapon.WeaponCategory.Pistol)
        {
            ScannerList.Add(carrying_weapon.Armedwithafirearm.FileName);
            Subtitles += " suspect is armed with a ~r~pistol~s~";
            Notification.Text += " a Pistol";
        }
        else if (CarryingWeapon.Category == GTAWeapon.WeaponCategory.Shotgun)
        {
            ScannerList.Add(carrying_weapon.Armedwithashotgun.FileName);
            Subtitles += " suspect is armed with a ~r~shotgun~s~";
            Notification.Text += " a Shotgun";
        }
        else if (CarryingWeapon.Category == GTAWeapon.WeaponCategory.SMG)
        {
            ScannerList.Add(carrying_weapon.Armedwithasubmachinegun.FileName);
            Subtitles += " suspect is armed with a ~r~submachine gun~s~";
            Notification.Text += " a Submachine Gun";
        }
        else if (CarryingWeapon.Category == GTAWeapon.WeaponCategory.AR)
        {
            ScannerList.Add(carrying_weapon.Carryinganassaultrifle.FileName);
            Subtitles += " suspect is carrying an ~r~assault rifle~s~";
            Notification.Text += " an Assault Rifle";
        }
        else if (CarryingWeapon.Category == GTAWeapon.WeaponCategory.Sniper)
        {
            ScannerList.Add(carrying_weapon.Armedwithasniperrifle.FileName);
            Subtitles += " suspect is armed with a ~r~sniper rifle~s~";
            Notification.Text += " a Sniper Rifle";
        }
        else if (CarryingWeapon.Category == GTAWeapon.WeaponCategory.Heavy)
        {
            ScannerList.Add(status_message.HeavilyArmed.FileName);
            Subtitles += " suspect is ~r~heaviy armed~s~";
            Notification.Text += " Heavy Weapons";
        }
        else if (CarryingWeapon.Category == GTAWeapon.WeaponCategory.Melee)
        {
            ScannerList.Add(carrying_weapon.Carryingaweapon.FileName);
            Subtitles += " suspect is carrying a ~r~weapon~s~";
            Notification.Text += " a melee weapon";
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
            Notification.Text += " a Gat";
        }
        Subtitles += "~s~";
        ReportGenericEnd(ref ScannerList, NearType.Nothing,ref Subtitles, ref Notification, Game.LocalPlayer.Character.Position);
        PlayAudioList(new DispatchAudioEvent(ScannerList, true, Subtitles, Notification));
    }
    public static void ReportOfficerDown()
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        DispatchNotification Notification = new DispatchNotification("Police Scanner", "~o~Crime Observed~s~", "Officer Down");
        ReportGenericStart(ref ScannerList,ref Subtitles,AttentionType.AllUnits,ReportType.Nobody, Game.LocalPlayer.Character.Position);
        Subtitles += " we have an ~r~Officer Down~s~";
        bool addRespondCode = true;
        int Num = rnd.Next(1, 6);
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
        else if (Num == 4)
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
        }
        AddLethalForceAuthorized(ref ScannerList, ref Subtitles);   
        ReportGenericEnd(ref ScannerList, NearType.Nothing,ref Subtitles, ref Notification, Game.LocalPlayer.Character.Position);
        PlayAudioList(new DispatchAudioEvent(ScannerList, false, Subtitles, Notification));
    }
    public static void ReportAssualtOnOfficer()
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";

        ReportGenericStart(ref ScannerList,ref Subtitles,AttentionType.Nobody,ReportType.Officers, Game.LocalPlayer.Character.Position);
        Subtitles += " we have an ~r~Assault on an Officer~s~";
        DispatchNotification Notification = new DispatchNotification("Police Scanner", "~o~Crime Observed~s~", "Assault on an Officer");
        ScannerList.Add(new List<string>() { crime_assault_on_an_officer.Anassaultonanofficer.FileName, crime_assault_on_an_officer.Anofficerassault.FileName }.PickRandom());
        AddLethalForceAuthorized(ref ScannerList,ref Subtitles);
        ReportGenericEnd(ref ScannerList, NearType.Nothing,ref Subtitles, ref Notification, Game.LocalPlayer.Character.Position);
        PlayAudioList(new DispatchAudioEvent(ScannerList, true, Subtitles, Notification));
    }
    public static void ReportThreateningWithFirearm()
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        DispatchNotification Notification = new DispatchNotification("Police Scanner", "~o~Crime Observed~s~", "Threatening Officers with a Firearm");
        ReportGenericStart(ref ScannerList,ref Subtitles,AttentionType.Nobody,ReportType.Officers, Game.LocalPlayer.Character.Position);
        ScannerList.Add(crime_suspect_threatening_an_officer_with_a_firearm.Asuspectthreateninganofficerwithafirearm.FileName);
        Subtitles += " we have a suspect ~r~Threatening an Officer with a Firearm~s~";
        AddLethalForceAuthorized(ref ScannerList,ref Subtitles);
        ReportGenericEnd(ref ScannerList, NearType.Nothing,ref Subtitles, ref Notification, Game.LocalPlayer.Character.Position);
        PlayAudioList(new DispatchAudioEvent(ScannerList, true, Subtitles, Notification));
    }
    public static void ReportSuspectLastSeen()
    {
        if (PoliceScanning.CopPeds.Any(x => x.DistanceToPlayer <= 100f))
            return;

        List<string> ScannerList = new List<string>() { AudioBeeps.AudioStart() };
        ScannerList.Add(new List<string>() { suspect_eluded_pt_1.SuspectEvadedPursuingOfficiers.FileName, suspect_eluded_pt_1.OfficiersHaveLostVisualOnSuspect.FileName }.PickRandom());
        string Subtitles = "Suspect evaded pursuing officers,~s~";
        DispatchNotification Notification = new DispatchNotification("Police Scanner", "~g~Status~s~", "Suspect Evaded");
        ReportGenericEnd(ref ScannerList, NearType.Zone,ref Subtitles, ref Notification, Police.LastWantedCenterPosition);
        PlayAudioList(new DispatchAudioEvent(ScannerList, false, Subtitles, Notification));
    }
    public static void ReportSuspectArrested()
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        DispatchNotification Notification = new DispatchNotification("Police Scanner", "~g~Status~s~", "Suspect Arrested");
        ReportGenericStart(ref ScannerList, ref Subtitles, AttentionType.Nobody, ReportType.Nobody, Game.LocalPlayer.Character.Position);
        ScannerList.Add(new List<string>() { crook_arrested.Officershaveapprehendedsuspect.FileName, crook_arrested.Officershaveapprehendedsuspect1.FileName }.PickRandom());
        Subtitles += "Officers have apprehended suspect";
        ReportGenericEnd(ref ScannerList, NearType.Nothing, ref Subtitles,ref Notification, Game.LocalPlayer.Character.Position);
        PlayAudioList(new DispatchAudioEvent(ScannerList, true, Subtitles));
    }
    public static void ReportSuspectWasted()
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        DispatchNotification Notification = new DispatchNotification("Police Scanner", "~g~Status~s~", "Suspect Wasted");
        ReportGenericStart(ref ScannerList,ref Subtitles,AttentionType.Nobody,ReportType.Nobody, Game.LocalPlayer.Character.Position);
        ScannerList.Add(new List<string>() { crook_killed.Criminaldown.FileName, crook_killed.Suspectdown.FileName, crook_killed.Suspectneutralized.FileName, crook_killed.Suspectdownmedicalexaminerenroute.FileName, crook_killed.Suspectdowncoronerenroute.FileName, crook_killed.Officershavepacifiedsuspect.FileName }.PickRandom());
        Subtitles += "Criminal down";
        ReportGenericEnd(ref ScannerList, NearType.Nothing, ref Subtitles,ref Notification, Game.LocalPlayer.Character.Position);
        PlayAudioList(new DispatchAudioEvent(ScannerList, true, Subtitles));
    }
    public static void ReportLethalForceAuthorized()
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
        ReportGenericEnd(ref ScannerList, NearType.Nothing,ref Subtitles, ref Notification, Game.LocalPlayer.Character.Position);
        PlayAudioList(new DispatchAudioEvent(ScannerList, false, Subtitles, Notification));
    }
    public static void ReportSuspiciousActivity()
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        DispatchNotification Notification = new DispatchNotification("Police Scanner", "~o~Crime Observed~s~", "Suspicious Activity");
        ReportGenericStart(ref ScannerList, ref Subtitles, AttentionType.LocalUnits, ReportType.Civilians, Game.LocalPlayer.Character.Position);
        ScannerList.Add(new List<string>() { crime_suspicious_activity.Suspiciousactivity.FileName, crime_theft.Apossibletheft.FileName }.PickRandom());
        Subtitles += " ~y~Suspicious Activity~s~";
        ReportGenericEnd(ref ScannerList, NearType.Nothing, ref Subtitles, ref Notification, Game.LocalPlayer.Character.Position);
        PlayAudioList(new DispatchAudioEvent(ScannerList, false, Subtitles, Notification));
    }
    public static void ReportGrandTheftAuto()
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        DispatchNotification Notification = new DispatchNotification("Police Scanner", "~o~Crime Observed~s~", "A GTA in progress");
        ReportGenericStart(ref ScannerList, ref Subtitles, AttentionType.LocalUnits, ReportType.Officers, Game.LocalPlayer.Character.Position);
        ScannerList.Add(new List<string>() { crime_grand_theft_auto.Agrandtheftauto.FileName, crime_grand_theft_auto.Agrandtheftautoinprogress.FileName, crime_grand_theft_auto.AGTAinprogress.FileName, crime_grand_theft_auto.AGTAinprogress1.FileName }.PickRandom());
        Subtitles += " a ~y~GTA~s~ in progress";
        ReportGenericEnd(ref ScannerList, NearType.Nothing, ref Subtitles, ref Notification, Game.LocalPlayer.Character.Position);
        PlayAudioList(new DispatchAudioEvent(ScannerList, false, Subtitles, Notification));
    }
    public static void ReportStolenAirVehicle(GTAVehicle vehicle)
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        DispatchNotification Notification = new DispatchNotification("Police Scanner", "~o~Crime Observed~s~", "Stolen Air Vehicle");
        ReportGenericStart(ref ScannerList, ref Subtitles, AttentionType.Nobody, ReportType.Officers, Game.LocalPlayer.Character.Position);

        if (vehicle.VehicleEnt.IsHelicopter)
        {
            ScannerList.Add(new List<string>() { crime_stolen_helicopter.Astolenhelicopter.FileName }.PickRandom());
            Subtitles += " a ~r~Stolen Helicopter~s~";
        }
        else if (vehicle.VehicleEnt.IsPlane)
        {
            ScannerList.Add(new List<string>() { crime_stolen_aircraft.Astolenaircraft.FileName, crime_stolen_aircraft.Astolenaircraft.FileName, crime_hijacked_aircraft.Ahijackedaircraft.FileName, crime_theft_of_an_aircraft.Theftofanaircraft.FileName }.PickRandom());
            Subtitles += " a ~r~Stolen Aircraft~s~";
        }
        else
            return;

        AddLethalForceAuthorized(ref ScannerList, ref Subtitles); 
        ReportGenericEnd(ref ScannerList, NearType.Nothing, ref Subtitles, ref Notification, Game.LocalPlayer.Character.Position);
        PlayAudioList(new DispatchAudioEvent(ScannerList, false, Subtitles, Notification));
    }
    public static void ReportChangedVehicle(GTAVehicle vehicle)
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        DispatchNotification Notification = new DispatchNotification("Police Scanner", "~g~Status~s~", "Suspect Updated Vehicle");
        ReportGenericStart(ref ScannerList, ref Subtitles, AttentionType.Nobody, ReportType.Nobody, Game.LocalPlayer.Character.Position);
        ScannerList.Add(suspect_last_seen.SuspectSpotted.FileName);
        Subtitles += "Suspect spotted driving a";
        ScannerList.Add(new List<string>() { conjunctives.Drivinga.FileName}.PickRandom());
        AddVehicleDescription(vehicle, ref ScannerList, false, ref Subtitles,ref Notification,false,true);

        ReportGenericEnd(ref ScannerList, NearType.Nothing, ref Subtitles, ref Notification, Game.LocalPlayer.Character.Position);
        PlayAudioList(new DispatchAudioEvent(ScannerList, false, Subtitles, Notification));
    }
    public static void ReportIncreasedWanted(bool ResultsInLethalForce)
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        DispatchNotification Notification = new DispatchNotification("Police Scanner", "~g~Status~s~", "Backup Required");
        ReportGenericStart(ref ScannerList, ref Subtitles, AttentionType.AllUnits, ReportType.Nobody, Game.LocalPlayer.Character.Position);
        List<string> PossibleVariations = new List<string>() { assistance_required.Assistanceneeded.FileName, assistance_required.Assistancerequired.FileName, assistance_required.Backupneeded.FileName, assistance_required.Backuprequired.FileName, assistance_required.Officersneeded.FileName, assistance_required.Officersrequired.FileName,
                                                                officer_requests_backup.Officersrequestingbackup.FileName,officer_requests_backup.Unitsrequirebackup.FileName,officer_requests_backup.Unitsrequireimmediateassistance.FileName,officer_requests_backup.Unitsrequestingbackup.FileName,officer_requests_backup.Officerneedsimmediateassistance.FileName };
        ScannerList.Add(PossibleVariations.PickRandom());
        Subtitles += " ~r~Assistance Needed~s~";
        AddZone(ref ScannerList, ref Subtitles, Game.LocalPlayer.Character.Position,ref Notification);
        if (ResultsInLethalForce)
        {
            AddLethalForceAuthorized(ref ScannerList,ref Subtitles);
        }
        ScannerList.Add(dispatch_respond_code.UnitsrespondCode3.FileName);
        Subtitles += " ~s~Units Repond ~o~Code-3~s~";
        ReportGenericEnd(ref ScannerList, NearType.Nothing, ref Subtitles, ref Notification, Game.LocalPlayer.Character.Position);
        PlayAudioList(new DispatchAudioEvent(ScannerList, false, Subtitles, Notification));
    }
    public static void ReportSuspiciousVehicle(GTAVehicle myCar)
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        DispatchNotification Notification = new DispatchNotification("Police Scanner", "~o~Crime Observed~s~", "Suspicious Vehicle");
        ReportGenericStart(ref ScannerList, ref Subtitles, AttentionType.LocalUnits, ReportType.Officers, Game.LocalPlayer.Character.Position);
        ScannerList.Add(new List<string>() { crime_suspicious_vehicle.Asuspiciousvehicle.FileName }.PickRandom());
        Subtitles += " a ~r~Suspicious Vehicle~s~";
        AddVehicleDescription(myCar, ref ScannerList, false, ref Subtitles, ref Notification, true,false);
        ReportGenericEnd(ref ScannerList, NearType.HeadingAndStreet, ref Subtitles, ref Notification, Game.LocalPlayer.Character.Position);
        PlayAudioList(new DispatchAudioEvent(ScannerList, false, Subtitles, Notification));
    }
    public static void ReportWeaponsFree()
    {
        if (ReportedWeaponsFree)
            return;
        ReportedWeaponsFree = true;
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        DispatchNotification Notification = new DispatchNotification("Police Scanner", "~g~Status~s~", "Officers are ~r~Weapons Free~s~");
        ReportGenericStart(ref ScannerList, ref Subtitles, AttentionType.AllUnits, ReportType.Nobody, Game.LocalPlayer.Character.Position);
        ScannerList.Add(custom_wanted_level_line.Suspectisarmedanddangerousweaponsfree.FileName);
        Subtitles += " suspect is ~r~Armed and Dangerous~s~, you are ~r~Weapons Free~s~";
        ReportGenericEnd(ref ScannerList, NearType.Nothing, ref Subtitles, ref Notification, Game.LocalPlayer.Character.Position);
        PlayAudioList(new DispatchAudioEvent(ScannerList, false, Subtitles, Notification));
    }
    public static void ReportCivilianKilled()
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        DispatchNotification Notification = new DispatchNotification("Police Scanner", "~o~Crime Observed~s~", "Civilian Fatality");
        ReportGenericStart(ref ScannerList, ref Subtitles, AttentionType.Nobody, ReportType.Officers, Game.LocalPlayer.Character.Position);
        ScannerList.Add(new List<string>() { crime_civilian_fatality.Acivilianfatality.FileName, crime_civilian_down.Aciviliandown.FileName }.PickRandom());
        Subtitles += " ~r~Civilian Down~s~";
        ReportGenericEnd(ref ScannerList, NearType.Street, ref Subtitles, ref Notification, Game.LocalPlayer.Character.Position);
        PlayAudioList(new DispatchAudioEvent(ScannerList, false, Subtitles, Notification));
    }
    public static void ResetReportedItems()
    {
        ReportedWeaponsFree = false;
        ReportedLethalForceAuthorized = false;
    }

    //Civilians Reporting
    public static void ReportLowLevelCriminalActivity()
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        DispatchNotification Notification = new DispatchNotification("Police Scanner", "~y~Crime Reported~s~", "Officers Needed");
        ReportGenericStart(ref ScannerList, ref Subtitles, AttentionType.Nobody, ReportType.Nobody, Police.LastWantedCenterPosition);
        int rnd = LosSantosRED.MyRand.Next(1, 11);
        if (rnd <= 2)
        {
            ScannerList.Add(new List<string>() { we_have.CitizensReport_1.FileName, we_have.CitizensReport_2.FileName, we_have.CitizensReport_3.FileName, we_have.CitizensReport_4.FileName }.PickRandom());
            ScannerList.Add(new List<string>() { crime_criminal_activity.Criminalactivity.FileName, crime_criminal_activity.Illegalactivity.FileName, crime_criminal_activity.Prohibitedactivity.FileName }.PickRandom());
            Subtitles += "Civilians report, ~y~Criminal activity~s~";
        }
        else if (rnd <= 5)
        {
            ScannerList.Add(new List<string>() { we_have.We_Have_1.FileName, we_have.We_Have_2.FileName }.PickRandom());
            ScannerList.Add(new List<string>() { crime_civilian_needing_assistance.Acivilianinneedofassistance.FileName }.PickRandom());
            Subtitles += "We have a ~y~Civilian in Need of Assistance~s~";
        }
        else if (rnd <= 8)
        {
            ScannerList.Add(new List<string>() { we_have.We_Have_1.FileName, we_have.We_Have_2.FileName }.PickRandom());
            ScannerList.Add(new List<string>() { crime_criminal_activity.Areportofacriminalactivity.FileName, crime_criminal_activity.Areportofcriminalactivity.FileName, crime_criminal_activity.Areportofillegalactivity.FileName }.PickRandom());
            Subtitles += "We have a report of ~y~Criminal Activity~s~";
        }
        else
        {
            ScannerList.Add(new List<string>() { assistance_required.Officersneeded.FileName, assistance_required.Officersrequired.FileName }.PickRandom());
            Subtitles += "~y~Officers required~s~";
        }
        ReportGenericEnd(ref ScannerList, NearType.Street, ref Subtitles, ref Notification, Police.LastWantedCenterPosition);
        PlayAudioList(new DispatchAudioEvent(ScannerList, false, Subtitles, Notification));
    }
    public static void ReportLowLevelShotsFired()
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        DispatchNotification Notification = new DispatchNotification("Police Scanner", "~y~Crime Reported~s~", "Shots Fired");
        ReportGenericStart(ref ScannerList, ref Subtitles, AttentionType.Nobody, ReportType.Civilians, Police.LastWantedCenterPosition);
        ScannerList.Add(new List<String>() { crime_shooting.Afirearmssituationseveralshotsfired.FileName, crime_shooting.Aweaponsincidentshotsfired.FileName, crime_shoot_out.Ashootout.FileName, crime_firearm_discharged_in_a_public_place.Afirearmdischargedinapublicplace.FileName
            , crime_firearms_incident.AfirearmsincidentShotsfired.FileName, crime_firearms_incident.Anincidentinvolvingshotsfired.FileName, crime_firearms_incident.AweaponsincidentShotsfired.FileName }.PickRandom());
        Subtitles += " a ~y~Firearms Discharge~s~";
        ReportGenericEnd(ref ScannerList, NearType.Street, ref Subtitles, ref Notification, Police.LastWantedCenterPosition);
        PlayAudioList(new DispatchAudioEvent(ScannerList, false, Subtitles, Notification));
    }
    public static void ReportLowLevelGrandTheftAuto()
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        DispatchNotification Notification = new DispatchNotification("Police Scanner", "~y~Crime Reported~s~", "GTA");
        ReportGenericStart(ref ScannerList, ref Subtitles, AttentionType.Nobody, ReportType.Civilians, Police.LastWantedCenterPosition);
        ScannerList.Add(new List<String>() { crime_grand_theft_auto.Agrandtheftauto.FileName, crime_grand_theft_auto.Agrandtheftautoinprogress.FileName, crime_grand_theft_auto.AGTAinprogress.FileName, crime_grand_theft_auto.AGTAinprogress1.FileName }.PickRandom());
        Subtitles += " a ~y~GTA~s~ in progress";
        ReportGenericEnd(ref ScannerList, NearType.Street, ref Subtitles, ref Notification, Police.LastWantedCenterPosition);
        PlayAudioList(new DispatchAudioEvent(ScannerList, false, Subtitles,Notification));
    }
    public static void ReportLowLevelTerroristActivity()
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        DispatchNotification Notification = new DispatchNotification("Police Scanner", "~y~Crime Reported~s~", "Terrorist Activity");
        ReportGenericStart(ref ScannerList, ref Subtitles, AttentionType.Nobody, ReportType.Civilians, Police.LastWantedCenterPosition);
        ScannerList.Add(new List<string>() { crime_terrorist_activity.Possibleterroristactivity.FileName, crime_terrorist_activity.Possibleterroristactivity1.FileName,crime_terrorist_activity.Possibleterroristactivity2.FileName, crime_terrorist_activity.Terroristactivity.FileName }.PickRandom());
        Subtitles += " possible ~y~Terrorist Activity~s~ in progress";
        ReportGenericEnd(ref ScannerList, NearType.Street, ref Subtitles, ref Notification, Police.LastWantedCenterPosition);
        PlayAudioList(new DispatchAudioEvent(ScannerList, false, Subtitles,Notification));
    }

    public static void ReportLowLevelCiviliansKilled()
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        DispatchNotification Notification = new DispatchNotification("Police Scanner", "~y~Crime Reported~s~", "Civilian Fatality");
        ReportGenericStart(ref ScannerList, ref Subtitles, AttentionType.Nobody, ReportType.Civilians, Police.LastWantedCenterPosition);
        ScannerList.Add(new List<string>() { crime_civilian_fatality.Acivilianfatality.FileName,crime_civilian_down.Aciviliandown.FileName}.PickRandom());
        Subtitles += " ~y~Civilian fatality~s~";
        ReportGenericEnd(ref ScannerList, NearType.Street, ref Subtitles, ref Notification, Police.LastWantedCenterPosition);
        PlayAudioList(new DispatchAudioEvent(ScannerList, false, Subtitles, Notification));
    }
    public static void ReportLowLevelCiviliansInjured()
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        DispatchNotification Notification = new DispatchNotification("Police Scanner", "~y~Crime Reported~s~", "Civilian Injury");
        ReportGenericStart(ref ScannerList, ref Subtitles, AttentionType.Nobody, ReportType.Civilians, Police.LastWantedCenterPosition);
        ScannerList.Add(new List<string>() { crime_injured_civilian.Aninjuredcivilian.FileName, crime_civilian_needing_assistance.Acivilianinneedofassistance.FileName, crime_civilian_needing_assistance.Acivilianrequiringassistance.FileName, crime_assault_on_a_civilian.Anassaultonacivilian.FileName }.PickRandom());
        Subtitles += " ~y~Civilian Injured~s~";
        ReportGenericEnd(ref ScannerList, NearType.Street, ref Subtitles, ref Notification, Police.LastWantedCenterPosition);
        PlayAudioList(new DispatchAudioEvent(ScannerList, false, Subtitles, Notification));
    }
    public static void ReportLowLevelCiviliansShot()
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        DispatchNotification Notification = new DispatchNotification("Police Scanner", "~y~Crime Reported~s~", "Civilian Shot");
        ReportGenericStart(ref ScannerList, ref Subtitles, AttentionType.Nobody, ReportType.Civilians, Police.LastWantedCenterPosition);
        ScannerList.Add(new List<string>() { crime_civillian_gsw.AcivilianGSW.FileName, crime_civillian_gsw.Acivilianshot.FileName, crime_civillian_gsw.Agunshotwound.FileName }.PickRandom());
        Subtitles += " ~y~Civilian Shot~s~";
        ReportGenericEnd(ref ScannerList, NearType.Street, ref Subtitles, ref Notification, Police.LastWantedCenterPosition);
        PlayAudioList(new DispatchAudioEvent(ScannerList, false, Subtitles, Notification));
    }
    //StolenCar
    public static void ReportStolenVehicle(GTAVehicle stolenVehicle)
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";

        DispatchNotification Notification = new DispatchNotification("Police Scanner", "~y~Crime Reported~s~", "Stolen Vehicle Reported");
        if (stolenVehicle.VehicleEnt.IsPoliceVehicle)
        {
            ReportGenericStart(ref ScannerList, ref Subtitles, AttentionType.AllUnits, ReportType.WeHave,stolenVehicle.PositionOriginallyEntered);
            ScannerList.Add(new List<string>() { crime_stolen_cop_car.Astolenpolicecar.FileName, crime_stolen_cop_car.Astolenpolicevehicle.FileName, crime_stolen_cop_car.Astolenpolicevehicle1.FileName, crime_stolen_cop_car.Defectivepolicevehicle.FileName }.PickRandom());
            Subtitles += " a ~r~Stolen Police Vehicle~s~";
        }
        else
        {
            ReportGenericStart(ref ScannerList, ref Subtitles, AttentionType.LocalUnits, ReportType.Civilians,stolenVehicle.PositionOriginallyEntered);
            ScannerList.Add(new List<string>() { crime_stolen_vehicle.Apossiblestolenvehicle.FileName }.PickRandom());
            Subtitles += " a possible ~r~Stolen Vehicle~s~";
        }
        AddVehicleDescription(stolenVehicle, ref ScannerList, true, ref Subtitles, ref Notification, true,false);
        ReportGenericEnd(ref ScannerList, NearType.Nothing, ref Subtitles, ref Notification, stolenVehicle.PositionOriginallyEntered);
        PlayAudioList(new DispatchAudioEvent(ScannerList, false, Subtitles, Notification));
        GameFiber ReportStolenVehicle = GameFiber.StartNew(delegate
        {
            GameFiber.Sleep(15000);
            stolenVehicle.WasReportedStolen = true;
            if (stolenVehicle.CarPlate.PlateNumber == stolenVehicle.OriginalLicensePlate.PlateNumber) //if you changed it between when it was reported, dont count it
                stolenVehicle.CarPlate.IsWanted = true;

            foreach(GTALicensePlate Plate in LicensePlateChanging.SpareLicensePlates)
            {
                if(Plate.PlateNumber == stolenVehicle.OriginalLicensePlate.PlateNumber)
                {
                    Plate.IsWanted = true;
                }
            }
            LocalWriteToLog("StolenVehicles", String.Format("Vehicle {0} was just reported stolen", stolenVehicle.VehicleEnt.Handle));

        }, "PlayDispatchQueue");
        Debugging.GameFibers.Add(ReportStolenVehicle);
    }

    //Visuals
    public static void ReportSuspectLost()//called when wanted level is removed
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        DispatchNotification Notification = new DispatchNotification("Police Scanner", "~g~Status~s~", "Suspect Lost");
        ReportGenericStart(ref ScannerList, ref Subtitles, AttentionType.Nobody, ReportType.Nobody, Police.LastWantedCenterPosition);
        ScannerList.Add(new List<string>() { attempt_to_find.AllunitsATonsuspects20.FileName, attempt_to_find.Allunitsattempttoreacquire.FileName, attempt_to_find.Allunitsattempttoreacquirevisual.FileName, attempt_to_find.RemainintheareaATL20onsuspect.FileName, attempt_to_find.RemainintheareaATL20onsuspect1.FileName }.PickRandom());
        Subtitles += "Remain in the area, ATL20 on suspect";
        ReportGenericEnd(ref ScannerList, NearType.Nothing, ref Subtitles, ref Notification, Police.LastWantedCenterPosition);
        PlayAudioList(new DispatchAudioEvent(ScannerList, false, Subtitles, Notification));
    }
    public static void ReportSuspectLostVisual()
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        DispatchNotification Notification = new DispatchNotification("Police Scanner", "~g~Status~s~", "Officers have lost visual contact with suspect");
        ReportGenericStart(ref ScannerList, ref Subtitles, AttentionType.Nobody, ReportType.Nobody, Police.LastWantedCenterPosition);
        ScannerList.Add(new List<string>() { suspect_eluded_pt_2.AllUnitsStayInTheArea.FileName, suspect_eluded_pt_2.AllUnitsRemainOnAlert.FileName, suspect_eluded_pt_2.AllUnitsStandby.FileName, suspect_eluded_pt_2.AllUnitsStayInTheArea.FileName, suspect_eluded_pt_2.AllUnitsRemainOnAlert.FileName }.PickRandom());
        Subtitles += "All units standby";
        ReportGenericEnd(ref ScannerList, NearType.Nothing, ref Subtitles, ref Notification, Police.LastWantedCenterPosition);
        PlayAudioList(new DispatchAudioEvent(ScannerList, false, Subtitles, Notification));
    }
    public static void ReportPersonOfInterestExpire()
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        DispatchNotification Notification = new DispatchNotification("Police Scanner", "~g~Status~s~", "Resume Patrol");
        ReportGenericStart(ref ScannerList, ref Subtitles, AttentionType.AllUnits, ReportType.Nobody, Police.LastWantedCenterPosition);
        ScannerList.Add(new List<string>() { officer_begin_patrol.Beginpatrol.FileName, officer_begin_patrol.Beginbeat.FileName, officer_begin_patrol.Assigntopatrol.FileName, officer_begin_patrol.Proceedtopatrolarea.FileName, officer_begin_patrol.Proceedwithpatrol.FileName }.PickRandom());
        Subtitles += " ~g~proceed with patrol~s~";
        ReportGenericEnd(ref ScannerList, NearType.Nothing, ref Subtitles, ref Notification, Police.LastWantedCenterPosition);
        PlayAudioList(new DispatchAudioEvent(ScannerList, false, Subtitles, Notification));
    }
    public static void ReportSuspectSpotted()
    {
        List<string> ScannerList = new List<string>();
        string Subtitles = "";
        DispatchNotification Notification = new DispatchNotification("Police Scanner", "~g~Status~s~", string.Format("Suspect {0} Reacquired", PedSwapping.SuspectName)) { TextureDict = PedSwapping.CurrentHeadshot.Txd, TextureName = PedSwapping.CurrentHeadshot.Txd };

        if (Police.CurrentCrimes.KillingPolice.HasBeenWitnessedByPolice || Police.CurrentCrimes.KillingCivilians.HasBeenWitnessedByPolice)
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
        ReportGenericEnd(ref ScannerList, NearType.Zone, ref Subtitles, ref Notification, Game.LocalPlayer.Character.Position);
        if(Police.CurrentCrimes.CommittedAnyCrimes)
            Notification.Text += ", Wanted For:" + Police.CurrentCrimes.PrintCrimes();

        PlayAudioList(new DispatchAudioEvent(ScannerList, false, Subtitles, Notification));
    }
    public static void ReportLocalSuspectSpotted()
    {
        List<string> ScannerList = new List<string>
        {
            AudioBeeps.AudioStart()
        };
        string Subtitles = "";
        DispatchNotification Notification = new DispatchNotification("Police Scanner", "~g~Status~s~", "Suspect Reacquired");
        if (!ReportHeadingAndStreet(ref ScannerList,ref Subtitles))
        {
            List<string> Possibilites = new List<string>() { spot_suspect_cop_01.HASH0601EE8E.FileName, spot_suspect_cop_01.HASH06A36FCF.FileName, spot_suspect_cop_01.HASH08E3F451.FileName, spot_suspect_cop_01.HASH0C703B6A.FileName, spot_suspect_cop_01.HASH13478918.FileName, spot_suspect_cop_01.HASH17551134.FileName, spot_suspect_cop_01.HASH1A3056EA.FileName, spot_suspect_cop_01.HASH1B3A58FF.FileName };
            ScannerList.Add(Possibilites.PickRandom());
            Subtitles += "Dispatch, we have ~r~eyes on~r~ the suspect";
        }
        ScannerList.Add(AudioBeeps.Radio_End_1.FileName);
        PlayAudioList(new DispatchAudioEvent(ScannerList, false, Subtitles, Notification));
    }
    public static bool ReportHeadingAndStreet(ref List<string> ScannerList, ref string Subtitles)
    {
        if (PlayerLocation.PlayerCurrentStreet != null && PlayerLocation.PlayerCurrentStreet.DispatchFile != "")
        {
            ScannerList.Add((new List<string>() { suspect_heading.TargetLastSeenHeading.FileName, suspect_heading.TargetReportedHeading.FileName, suspect_heading.TargetSeenHeading.FileName, suspect_heading.TargetSpottedHeading.FileName }).PickRandom());
            Subtitles += "~r~Target spotted~s~ heading~s~";
            string heading = GetSimpleCompassHeading();
            if (heading == "N")
            {
                ScannerList.Add(direction_heading.North.FileName);
                Subtitles += " ~g~North~s~";
            }
            else if (heading == "S")
            {
                ScannerList.Add(direction_heading.South.FileName);
                Subtitles += " ~g~South";
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

            if (PlayerLocation.PlayerCurrentStreet != null)
            {
                Street MyStreet = PlayerLocation.PlayerCurrentStreet;
                if (MyStreet != null && MyStreet.DispatchFile != "")
                {
                    ScannerList.Add((new List<string>() { conjunctives.On.FileName, conjunctives.On1.FileName, conjunctives.On2.FileName, conjunctives.On3.FileName, conjunctives.On4.FileName }).PickRandom());
                    ScannerList.Add(MyStreet.DispatchFile);
                    Subtitles += " ~s~on ~HUD_COLOUR_YELLOWLIGHT~" + MyStreet.Name + "~s~";

                    if (PlayerLocation.PlayerCurrentCrossStreet != null)
                    {
                        Street MyCrossStreet = PlayerLocation.PlayerCurrentCrossStreet;
                        if (MyCrossStreet != null && MyCrossStreet.DispatchFile != "")
                        {
                            ScannerList.Add((new List<string>() { conjunctives.AT01.FileName, conjunctives.AT02.FileName }).PickRandom());
                            ScannerList.Add(MyCrossStreet.DispatchFile);
                            Subtitles += " ~s~at ~HUD_COLOUR_YELLOWLIGHT~" + MyCrossStreet.Name + "~s~";
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
    public static void PopQuizHotShot()
    {
        List<string> ScannerList = new List<string>
        {
            SpeedQuotes.PopQuizHotShot.FileName
        };
        PlayAudioList(new DispatchAudioEvent(ScannerList, false, "Pop quiz hot shot"));
    }

    //Helper
    public static void AddStolenVehicle(ref List<string> ScannerList,GTAVehicle StolenCar, ref string Subtitles)
    {
        ScannerList.Add(new List<string>() { conjunctives.Inuhh2.FileName, conjunctives.Inuhh3.FileName }.PickRandom());
        if (StolenCar.VehicleEnt.IsPoliceVehicle)
        {
            ScannerList.Add(new List<string>() { crime_stolen_cop_car.Astolenpolicecar.FileName, crime_stolen_cop_car.Astolenpolicevehicle.FileName, crime_stolen_cop_car.Astolenpolicevehicle1.FileName, crime_stolen_cop_car.Defectivepolicevehicle.FileName }.PickRandom());
            Subtitles += " ~s~in a ~r~Stolen Police Vehicle~s~";
        }
        else
        {
            ScannerList.Add(crime_stolen_vehicle.Apossiblestolenvehicle.FileName);
            Subtitles += " ~s~in a possible stolen vehicle~s~";
        }
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
            return ScannerAudio.manufacturer.ALBANY01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Annis)
            return ScannerAudio.manufacturer.ANNIS01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Benefactor)
            return ScannerAudio.manufacturer.BENEFACTOR01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Burgerfahrzeug)
            return ScannerAudio.manufacturer.BF01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Bollokan)
            return ScannerAudio.manufacturer.BOLLOKAN01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Bravado)
            return ScannerAudio.manufacturer.BRAVADO01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Brute)
            return ScannerAudio.manufacturer.BRUTE01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Buckingham)
            return "";
        else if (manufacturer == Vehicles.Manufacturer.Canis)
            return ScannerAudio.manufacturer.CANIS01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Chariot)
            return ScannerAudio.manufacturer.CHARIOT01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Cheval)
            return ScannerAudio.manufacturer.CHEVAL01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Classique)
            return ScannerAudio.manufacturer.CLASSIQUE01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Coil)
            return ScannerAudio.manufacturer.COIL01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Declasse)
            return ScannerAudio.manufacturer.DECLASSE01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Dewbauchee)
            return ScannerAudio.manufacturer.DEWBAUCHEE01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Dinka)
            return ScannerAudio.manufacturer.DINKA01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.DUDE)
            return "";
        else if (manufacturer == Vehicles.Manufacturer.Dundreary)
            return ScannerAudio.manufacturer.DUNDREARY01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Emperor)
            return ScannerAudio.manufacturer.EMPEROR01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Enus)
            return ScannerAudio.manufacturer.ENUS01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Fathom)
            return ScannerAudio.manufacturer.FATHOM01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Gallivanter)
            return ScannerAudio.manufacturer.GALLIVANTER01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Grotti)
            return ScannerAudio.manufacturer.GROTTI01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.HVY)
            return ScannerAudio.manufacturer.HVY01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Hijak)
            return ScannerAudio.manufacturer.HIJAK01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Imponte)
            return ScannerAudio.manufacturer.IMPONTE01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Invetero)
            return ScannerAudio.manufacturer.INVETERO01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.JackSheepe)
            return ScannerAudio.manufacturer.JACKSHEEPE01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Jobuilt)
            return ScannerAudio.manufacturer.JOEBUILT01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Karin)
            return ScannerAudio.manufacturer.KARIN01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.KrakenSubmersibles)
            return "";
        else if (manufacturer == Vehicles.Manufacturer.Lampadati)
            return ScannerAudio.manufacturer.LAMPADATI01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.LibertyChopShop)
            return "";
        else if (manufacturer == Vehicles.Manufacturer.LibertyCityCycles)
            return "";
        else if (manufacturer == Vehicles.Manufacturer.MaibatsuCorporation)
            return ScannerAudio.manufacturer.MAIBATSU01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Mammoth)
            return ScannerAudio.manufacturer.MAMMOTH01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.MTL)
            return ScannerAudio.manufacturer.MTL01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Nagasaki)
            return ScannerAudio.manufacturer.NAGASAKI01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Obey)
            return ScannerAudio.manufacturer.OBEY01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Ocelot)
            return ScannerAudio.manufacturer.OCELOT01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Overflod)
            return ScannerAudio.manufacturer.OVERFLOD01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Pegassi)
            return ScannerAudio.manufacturer.PEGASI01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Pfister)
            return "";
        else if (manufacturer == Vehicles.Manufacturer.Principe)
            return ScannerAudio.manufacturer.PRINCIPE01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Progen)
            return ScannerAudio.manufacturer.PROGEN01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.ProLaps)
            return "";
        else if (manufacturer == Vehicles.Manufacturer.RUNE)
            return "";
        else if (manufacturer == Vehicles.Manufacturer.Schyster)
            return ScannerAudio.manufacturer.SCHYSTER01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Shitzu)
            return ScannerAudio.manufacturer.SHITZU01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Speedophile)
            return ScannerAudio.manufacturer.SPEEDOPHILE01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Stanley)
            return ScannerAudio.manufacturer.STANLEY01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.SteelHorse)
            return ScannerAudio.manufacturer.STEELHORSE01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Truffade)
            return ScannerAudio.manufacturer.TRUFFADE01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Ubermacht)
            return ScannerAudio.manufacturer.UBERMACHT01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Vapid)
            return ScannerAudio.manufacturer.VAPID01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Vulcar)
            return ScannerAudio.manufacturer.VULCAR01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Vysser)
            return "";
        else if (manufacturer == Vehicles.Manufacturer.Weeny)
            return ScannerAudio.manufacturer.WEENY01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.WesternCompany)
            return ScannerAudio.manufacturer.WESTERNCOMPANY01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.WesternMotorcycleCompany)
            return ScannerAudio.manufacturer.WESTERNMOTORCYCLECOMPANY01.FileName;
        else if (manufacturer == Vehicles.Manufacturer.Willard)
            return "";
        else if (manufacturer == Vehicles.Manufacturer.Zirconium)
            return ScannerAudio.manufacturer.ZIRCONIUM01.FileName;
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
    private static void AddLethalForceAuthorized(ref List<string> ScannerList, ref string Subtitles)
    {
        if (!ReportedLethalForceAuthorized)
        {
            ScannerList.Add(new List<string>() { lethal_force.Useofdeadlyforceauthorized.FileName, lethal_force.Useofdeadlyforceisauthorized.FileName, lethal_force.Useofdeadlyforceisauthorized1.FileName, lethal_force.Useoflethalforceisauthorized.FileName, lethal_force.Useofdeadlyforcepermitted1.FileName }.PickRandom());
            Subtitles += " use of ~r~Deadly Force~s~ is authorized";
            ReportedLethalForceAuthorized = true;
        }
    }
    public static void AddVehicleDescription(GTAVehicle VehicleDescription, ref List<string> ScannerList, bool IncludeLicensePlate,ref string Subtitles,ref DispatchNotification Notification, bool IncludeAAudio, bool IncludePoliceDescription)
    {
        if (VehicleDescription.HasBeenDescribedByDispatch)
            return;
        else
            VehicleDescription.HasBeenDescribedByDispatch = true;

        if (VehicleDescription.VehicleEnt.IsPoliceVehicle)
        {
            if (IncludePoliceDescription)
            {
                Subtitles += " ~r~Police Car~s~";
                Notification.Text += " ~r~Police Car~s~";
                ScannerList.Add(vehicle_category.PoliceSedan01.FileName);
            }
        }
        else
        {
            Vehicles.VehicleInfo VehicleInformation = Vehicles.GetVehicleInfo(VehicleDescription);
            System.Drawing.Color BaseColor = GetBaseColor(VehicleDescription.DescriptionColor);
            ColorLookup LookupColor = ColorLookups.Where(x => x.BaseColor == BaseColor).PickRandom();
            string ManufacturerScannerFile;
            string VehicleClassScannerFile;
            if (VehicleInformation != null)
            {
                LocalWriteToLog("Description", string.Format("VehicleInformation.ModelScannerFile {0}", VehicleInformation.ModelScannerFile.ToString()));
                ManufacturerScannerFile = GetManufacturerScannerFile(VehicleInformation.Manufacturer);
                VehicleClassScannerFile = GetVehicleClassScannerFile(VehicleInformation.VehicleClass);
                if (LookupColor != null && (VehicleInformation.ModelScannerFile != "" || VehicleInformation.ModelScannerFile != "" || VehicleClassScannerFile != ""))
                {
                    if (IncludeAAudio)
                    {
                        Subtitles += " ~s~a~s~";
                        Notification.Text += " ~s~a~s~";
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
            Notification.Text += " ~s~License Plate: ~y~" + VehicleDescription.OriginalLicensePlate.PlateNumber.ToUpper() + "~s~";
            foreach (char c in VehicleDescription.OriginalLicensePlate.PlateNumber)
            {
                string DispatchFileName = LettersAndNumbersLookup.Where(x => x.AlphaNumeric == c).PickRandom().ScannerFile;
                ScannerList.Add(DispatchFileName);
            }
        }
    }
    public static void AddSpeed(ref List<string> ScannerList,float Speed, ref string Subtitles,ref DispatchNotification Notification)
    {
        if (Speed >= 70f)
        {
            ScannerList.Add(suspect_last_seen.TargetLastReported.FileName);
            Subtitles += " ~s~target last reported~s~";
            if (Speed >= 70f && Speed < 80f)
            {
                ScannerList.Add(doing_speed.Doing70mph.FileName);
                Subtitles += " ~s~doing ~o~70 mph~s~";
                Notification.Text += " Exceeding ~o~70 mph~s~";
            }
            else if (Speed >= 80f && Speed < 90f)
            {
                ScannerList.Add(doing_speed.Doing80mph.FileName);
                Subtitles += " ~s~doing ~o~80 mph~s~";
                Notification.Text += " Exceeding ~o~80 mph~s~";
            }
            else if (Speed >= 90f && Speed < 100f)
            {
                ScannerList.Add(doing_speed.Doing90mph.FileName);
                Subtitles += " ~s~doing ~o~90 mph~s~";
                Notification.Text += " Exceeding ~o~90 mph~s~";
            }
            else if (Speed >= 100f && Speed < 104f)
            {
                ScannerList.Add(doing_speed.Doing100mph.FileName);
                Subtitles += " ~s~doing ~o~100 mph~s~";
                Notification.Text += " Exceeding ~o~100 mph~s~";
            }
            else if (Speed >= 105f)
            {
                ScannerList.Add(doing_speed.Doingover100mph.FileName);
                Subtitles += " ~s~doing ~o~over 100 mph~s~";
                Notification.Text += " Exceeding ~o~105 mph~s~";
            }
        }
    }
    private static void LocalWriteToLog(string ProcedureString, string TextToLog)
    {
        if (Settings.DispatchAudioLogging)
            Debugging.WriteToLog(ProcedureString, TextToLog);
    }
    public static void AbortAllAudio()
    {
        DispatchQueue.Clear();
        CancelAudio = true;
        if (AudioPlaying)
            outputDevice.Stop();
        DispatchQueue.Clear();
        if (AudioPlaying)
            outputDevice.Stop();
        DispatchQueue.Clear();
        CancelAudio = false;

        foreach(uint handles in NotificationHandles)
        {
            Game.RemoveNotification(handles);
        }
        NotificationHandles.Clear();
    }
    public class DispatchAudioEvent
    {
        public List<string> SoundsToPlay;
        public bool CheckSight = false;
        public string Subtitles = "";
        public DispatchNotification NotificationToDisplay;
        public DispatchAudioEvent(List<string> _SoundsToPlay, bool _CheckSight)
        {
            SoundsToPlay = _SoundsToPlay;
            CheckSight = _CheckSight;
        }
        public DispatchAudioEvent(List<string> _SoundsToPlay, bool _CheckSight,string _Subtitles)
        {
            SoundsToPlay = _SoundsToPlay;
            CheckSight = _CheckSight;
            Subtitles = _Subtitles;
        }
        public DispatchAudioEvent(List<string> _SoundsToPlay, bool _CheckSight,string _Subtitles, DispatchNotification _NotificationToDisplay)
        {
            SoundsToPlay = _SoundsToPlay;
            CheckSight = _CheckSight;
            Subtitles = _Subtitles;
            NotificationToDisplay = _NotificationToDisplay;
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
        public ReportDispatch Type { get; set; }
        public int Priority { get; set; } = 0;
        public bool ResultsInLethalForce { get; set; } = false;
        public bool ResultsInStolenCarSpotted { get; set; } = false;
        public bool IsTrafficViolation { get; set; } = false;
        public bool IsAmbient { get; set; } = false;
        public float Speed { get; set; }
        public int ResultingWantedLevel { get; set; }
        public GTAWeapon WeaponToReport { get; set; }
        public GTAVehicle VehicleToReport { get; set; }
        public DispatchQueueItem(ReportDispatch _Type,int _Priority)
        {
            Type = _Type;
            Priority = _Priority;
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

