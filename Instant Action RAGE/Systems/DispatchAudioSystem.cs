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
                        ReportShotsFired(true);
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
            audioFile = new AudioFileReader(String.Format("Plugins\\InstantAction\\audio\\scanner\\{0}.wav", _Audio));
            audioFile.Volume = 0.4f;
            outputDevice.Init(audioFile);
        }
        outputDevice.Play();
    }
    private static void PlayAudioList(List<String> SoundsToPlay,bool CheckSight)
    {
        GameFiber.Sleep(rnd.Next(1000, 2000));
        if(CheckSight && !PoliceScanningSystem.CopPeds.Any(x => x.canSeePlayer))
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
    public static void ReportShotsFired(bool Near)
    {
        if (!PoliceScanningSystem.CopPeds.Any(x => x.canSeePlayer))
            return;

        List<string> myList = new List<string>(new string[]
        {
            Scanner.Resident.DISPATCH_INTRO_01.Value,
            Scanner.WeHave.OfficersReportRandom(),
            Scanner.Crimes.CRIME_SHOTS_FIRED_AT_AN_OFFICER_01.Value,
        });

        Vector3 Pos = Game.LocalPlayer.Character.Position;
        Zone MyZone = Zones.GetZoneName(Pos);
        if(MyZone != null)
        {
            InstantAction.WriteToLog("ReportShotsFired", MyZone.TextName);
            myList.Add(Scanner.Conjunctives.NearAtCloseTo());
            myList.Add(MyZone.ScannerValue);
        }
        myList.Add(Scanner.Resident.OUTRO_01.Value);
        PlayAudioList(myList,true);
    }
    public static void ReportCarryingWeapon(bool Near)
    {
        if (!PoliceScanningSystem.CopPeds.Any(x => x.canSeePlayer))
            return;
        List<string> myList = new List<string>(new string[]
        {
            Scanner.Resident.DISPATCH_INTRO_01.Value,
            Scanner.WeHave.OfficersReportRandom(),
            Scanner.Crimes.CRIME_BRANDISHING_WEAPON_01.Value,
        });

        Vector3 Pos = Game.LocalPlayer.Character.Position;
        Zone MyZone = Zones.GetZoneName(Pos);
        if (MyZone != null)
        {
            InstantAction.WriteToLog("ReportCarryingWeapon", MyZone.TextName);
            myList.Add(Scanner.Conjunctives.NearAtCloseTo());
            myList.Add(MyZone.ScannerValue);
        }
        myList.Add(Scanner.Resident.OUTRO_01.Value);
        PlayAudioList(myList,true);
    }
    public static void ReportAssualtOnOfficer(bool Near)
    {
        if (!PoliceScanningSystem.CopPeds.Any(x => x.canSeePlayer))
            return;
        List<string> myList = new List<string>(new string[]
        {
            Scanner.Resident.DISPATCH_INTRO_01.Value,
            Scanner.WeHave.OfficersReportRandom(),
            Scanner.Crimes.CrimeAssaultPeaceOfficerRandom(),
        });

        Vector3 Pos = Game.LocalPlayer.Character.Position;
        Zone MyZone = Zones.GetZoneName(Pos);
        if (MyZone != null)
        {
            InstantAction.WriteToLog("ReportAssualtOnOfficer", MyZone.TextName);
            myList.Add(Scanner.Conjunctives.NearAtCloseTo());
            myList.Add(MyZone.ScannerValue);
        }
        myList.Add(Scanner.Resident.OUTRO_01.Value);
        PlayAudioList(myList,true);
    }
    public static void ReportThreateningWithFirearm(bool Near)
    {
        List<string> myList = new List<string>(new string[]
        {
            Scanner.Resident.DISPATCH_INTRO_01.Value,
            Scanner.WeHave.OfficersReportRandom(),
            Scanner.Crimes.CRIME_THREATEN_OFFICER_WITH_FIREARM_01.Value,
        });

        Vector3 Pos = Game.LocalPlayer.Character.Position;
        Zone MyZone = Zones.GetZoneName(Pos);
        if (MyZone != null)
        {
            InstantAction.WriteToLog("ReportAssualtOnOfficer", MyZone.TextName);
            myList.Add(Scanner.Conjunctives.NearAtCloseTo());
            myList.Add(MyZone.ScannerValue);
        }
        myList.Add(Scanner.Resident.OUTRO_01.Value);
        PlayAudioList(myList,true);
    }
    public static void ReportSuspectLastSeen(bool Near)
    {
        List<string> myList = new List<string>(new string[]
        {
            Scanner.Resident.DISPATCH_INTRO_01.Value,
            Scanner.Suspect.SUSPECT_LAST_SEEN_01.Value,
        });

        Vector3 Pos = Game.LocalPlayer.Character.Position;
        Zone MyZone = Zones.GetZoneName(Pos);
        if (MyZone != null)
        {
            InstantAction.WriteToLog("ReportAssualtOnOfficer", MyZone.TextName);
            myList.Add(Scanner.Conjunctives.NearAtCloseTo());
            myList.Add(MyZone.ScannerValue);
        }
        myList.Add(Scanner.Resident.OUTRO_01.Value);
        PlayAudioList(myList,false);
    }
    public static void ReportSuspectArrested()
    {
        List<string> myList = new List<string>(new string[]
        {
            Scanner.Resident.DISPATCH_INTRO_01.Value,
            Scanner.CrookArrested.CrookArrestedRandom()
        }) ;
        myList.Add(Scanner.Resident.OUTRO_01.Value);
        PlayAudioList(myList,false);
    }
}

