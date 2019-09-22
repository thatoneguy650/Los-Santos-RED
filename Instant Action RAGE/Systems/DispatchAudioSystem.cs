using Instant_Action_RAGE.Systems;
using NAudio.Wave;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Zones;

internal static class DispatchAudioSystem
    {
        private static WaveOutEvent outputDevice;
        private static AudioFileReader audioFile;
        private static Random rnd;

         public static bool IsRunning { get; set; } = true;
        static DispatchAudioSystem()
        {
            rnd = new Random();
        }
    public static void Initialize()
        {
            MainLoop();
        }
        public static void MainLoop()
        {
            GameFiber.StartNew(delegate
            {
                while (IsRunning)
                {

                    if (Game.IsKeyDown(Keys.NumPad5))
                    {
                        ReportThreateningWithFirearm(true);
                    }

                    GameFiber.Yield();
                }
            });
        }
    private static void PlayAudio(String _Audio)
    {
        if (outputDevice == null)
        {
            outputDevice = new WaveOutEvent();
            outputDevice.PlaybackStopped += OnPlaybackStopped;
        }
        if (audioFile == null)
        {
            //audioFile = new AudioFileReader(String.Format("Plugins\\InstantAction\\audio\\scanner\\{0}.wav", _Audio));
            audioFile = new AudioFileReader(String.Format("Plugins\\InstantAction\\scanner\\{0}", _Audio));
            audioFile.Volume = 0.4f;
            outputDevice.Init(audioFile);
        }
        outputDevice.Play();
    }
    private static void PlayAudioList(List<String> SoundsToPlay,bool CheckSight)
    {
        GameFiber.Sleep(rnd.Next(1000, 2000));
        if (CheckSight && !PoliceScanningSystem.CopPeds.Any(x => x.canSeePlayer))
            return;

        GameFiber.StartNew(delegate
        {            
            while (outputDevice != null)
                GameFiber.Yield();
            foreach (String audioname in SoundsToPlay)
            {
                PlayAudio(audioname);
                while (outputDevice != null)
                    GameFiber.Yield();
            }
        });
    }
    private static void OnPlaybackStopped(object sender, StoppedEventArgs args)
    {
        outputDevice.Dispose();
        outputDevice = null;
        audioFile.Dispose();
        audioFile = null;
    }
    private static void ReportGenericStart(List<string> myList)
    {
        if (!PoliceScanningSystem.CopPeds.Any(x => x.canSeePlayer))
            return;
        myList.Add(ScannerAudio.AudioBeeps.AudioStart());
        myList.Add(ScannerAudio.we_have.OfficersReport());
    }
    private static void ReportGenericEnd(List<string> myList,bool Near)
    {
        if(Near)
        {
            Vector3 Pos = Game.LocalPlayer.Character.Position;
            Zone MyZone = Zones.GetZoneName(Pos);
            if (MyZone != null)
            {
                myList.Add(ScannerAudio.conjunctives.NearGenericRandom());
                myList.Add(MyZone.ScannerValue);
            }
        }
        myList.Add(ScannerAudio.AudioBeeps.Radio_End_1.FileName);
    }
    public static void ReportShotsFired(bool Near)
    {
        List<string> ScannerList = new List<string>();
        ReportGenericStart(ScannerList);
        ScannerList.Add(ScannerAudio.crime_shots_fired_at_an_officer.Shotsfiredatanofficer.FileName);
        ReportGenericEnd(ScannerList, true);
        PlayAudioList(ScannerList, true);
    }
    public static void ReportCarryingWeapon(bool Near)
    {
        List<string> ScannerList = new List<string>();
        ReportGenericStart(ScannerList);
        ScannerList.Add(ScannerAudio.carrying_weapon.Carryingafirearm.FileName);
        ReportGenericEnd(ScannerList, true);
        PlayAudioList(ScannerList, true);
    }
    public static void ReportAssualtOnOfficer(bool Near)
    {
        List<string> ScannerList = new List<string>();
        ReportGenericStart(ScannerList);
        ScannerList.Add(ScannerAudio.crime_assault_on_an_officer.Anassaultonanofficer.FileName);
        ReportGenericEnd(ScannerList, true);

        PlayAudioList(ScannerList, true);
    }
    public static void ReportThreateningWithFirearm(bool Near)
    {
        List<string> ScannerList = new List<string>();
        ReportGenericStart(ScannerList);
        ScannerList.Add(ScannerAudio.crime_suspect_threatening_an_officer_with_a_firearm.Asuspectthreateninganofficerwithafirearm.FileName);
        ReportGenericEnd(ScannerList, true);
        PlayAudioList(ScannerList, true);
    }
    public static void ReportSuspectLastSeen(bool Near)
    {
        List<string> ScannerList = new List<string>();
        ReportGenericStart(ScannerList);
        ScannerList.Add(ScannerAudio.suspect_last_seen.TargetLastSeen.FileName);
        ReportGenericEnd(ScannerList, true);
        PlayAudioList(ScannerList, false);
    }
    public static void ReportSuspectArrested()
    {
        List<string> myList = new List<string>(new string[]
        {
            ScannerAudio.AudioBeeps.AudioStart(),
            ScannerAudio.crook_arrested.CrookArrestedRandom()
        }) ;
        myList.Add(ScannerAudio.AudioBeeps.AudioEnd());
        PlayAudioList(myList,true);
    }
    public static void ReportSuspectWasted()
    {
        List<string> myList = new List<string>(new string[]
        {
            ScannerAudio.AudioBeeps.AudioStart(),
            ScannerAudio.crook_killed.CrookKilledRandom()
        });
        myList.Add(ScannerAudio.AudioBeeps.AudioEnd());
        PlayAudioList(myList, true);
    }
}

