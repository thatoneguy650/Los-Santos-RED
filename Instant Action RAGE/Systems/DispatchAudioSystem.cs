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

    public static bool ReportedOfficerDown { get; set; } = false;
    public static bool ReportedShotsFired { get; set; } = false;
    public static bool ReportedAssaultOnOfficer { get; set; } = false;
    public static bool ReportedCarryingWeapon { get; set; } = false;
    public static bool ReportedLethalForceAuthorized { get; set; } = false;
    public static bool ReportedThreateningWithAFirearm { get; set; } = false;
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
                    GameFiber.Yield();
                }
            });
        }
    private static void PlayAudio(String _Audio)
    {
        try
        {
            if (outputDevice == null)
            {
                outputDevice = new WaveOutEvent();
                outputDevice.PlaybackStopped += OnPlaybackStopped;
            }
            if (audioFile == null)
            {
                audioFile = new AudioFileReader(String.Format("Plugins\\InstantAction\\scanner\\{0}", _Audio));
                audioFile.Volume = Settings.DispatchAudioVolume;
                outputDevice.Init(audioFile);
            }
            outputDevice.Play();
        }
        catch(Exception e)
        {
            Game.Console.Print(e.Message);
        }
    }
    private static void PlayAudioList(List<String> SoundsToPlay,bool CheckSight)
    {
        GameFiber.Sleep(rnd.Next(250, 670));
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
    public static void ReportShotsFired()
    {
        if (ReportedOfficerDown || ReportedLethalForceAuthorized)
            return;
        List<string> ScannerList = new List<string>();
        ReportGenericStart(ScannerList);
        ScannerList.Add(ScannerAudio.crime_shots_fired_at_an_officer.Shotsfiredatanofficer.FileName);
        ReportGenericEnd(ScannerList, true);
        ReportedShotsFired = true;
        PlayAudioList(ScannerList, true);
        
    }
    public static void ReportCarryingWeapon()
    {
        if (ReportedOfficerDown || ReportedLethalForceAuthorized || ReportedAssaultOnOfficer)
            return;
        List<string> ScannerList = new List<string>();
        ReportGenericStart(ScannerList);

        int Num = rnd.Next(1, 4);
        if(Num == 1)
        {
            ScannerList.Add(ScannerAudio.suspect_is.SuspectIs.FileName);
            ScannerList.Add(ScannerAudio.carrying_weapon.Armedwithafirearm.FileName);
        }
        else if (Num == 2)
        {
            ScannerList.Add(ScannerAudio.suspect_is.SuspectIs.FileName);
            ScannerList.Add(ScannerAudio.carrying_weapon.Armedwithagat.FileName);
        }
        else if (Num == 3)
        {
            ScannerList.Add(ScannerAudio.suspect_is.SuspectIs.FileName);
            ScannerList.Add(ScannerAudio.carrying_weapon.Carryingafirearm.FileName);
        }
        else
        {
            ScannerList.Add(ScannerAudio.suspect_is.SuspectIs.FileName);
            ScannerList.Add(ScannerAudio.carrying_weapon.Carryingagat.FileName);
        }
        
        ReportGenericEnd(ScannerList, true);
        ReportedCarryingWeapon = true;
        PlayAudioList(ScannerList, true);
    }
    public static void ReportOfficerDown()
    {
        List<string> ScannerList = new List<string>();
        ReportGenericStart(ScannerList);

        int Num = rnd.Next(1, 4);
        if (Num == 1)
        {
            //ScannerList.Add(ScannerAudio.we_have.We_Have_1.FileName);
            ScannerList.Add(ScannerAudio.crime_officer_down.AcriticalsituationOfficerdown.FileName);
        }
        else if (Num == 2)
        {
            //ScannerList.Add(ScannerAudio.we_have.We_Have_1.FileName);
            ScannerList.Add(ScannerAudio.crime_officer_down.AnofferdownpossiblyKIA.FileName);
        }
        else if (Num == 3)
        {
            //ScannerList.Add(ScannerAudio.we_have.We_Have_1.FileName);
            ScannerList.Add(ScannerAudio.crime_officer_down.Anofficerdown.FileName);
        }
        else
        {
            ScannerList.Add(ScannerAudio.we_have.We_Have_2.FileName);
            ScannerList.Add(ScannerAudio.crime_officer_down.Anofficerdownconditionunknown.FileName);
        }

        int Num2 = rnd.Next(1, 4);
        if (Num2 == 1)
        {
            ScannerList.Add(ScannerAudio.dispatch_respond_code.AllunitsrespondCode99.FileName);
        }
        else if (Num2 == 2)
        {
            ScannerList.Add(ScannerAudio.dispatch_respond_code.AllunitsrespondCode99emergency.FileName);
        }
        else if (Num2 == 3)
        {
            ScannerList.Add(ScannerAudio.dispatch_respond_code.Code99allunitsrespond.FileName);
        }
        else
        {
            ScannerList.Add(ScannerAudio.dispatch_respond_code.EmergencyallunitsrespondCode99.FileName);
        }

        ReportGenericEnd(ScannerList, false);
        ReportedOfficerDown = true;
        PlayAudioList(ScannerList, true);
    }
    public static void ReportAssualtOnOfficer()
    {
        if (ReportedOfficerDown || ReportedLethalForceAuthorized)
            return;
        List<string> ScannerList = new List<string>();
        ReportGenericStart(ScannerList);
        int Num = rnd.Next(1, 4);
        if (Num == 1)
        {
            ScannerList.Add(ScannerAudio.we_have.We_Have_1.FileName);
            ScannerList.Add(ScannerAudio.crime_assault.Apossibleassault1.FileName);
        }
        else if (Num == 2)
        {
            ScannerList.Add(ScannerAudio.we_have.We_Have_1.FileName);
            ScannerList.Add(ScannerAudio.crime_assault.Apossibleassault.FileName);
        }
        else if (Num == 3)
        {
            ScannerList.Add(ScannerAudio.we_have.We_Have_1.FileName);
            ScannerList.Add(ScannerAudio.crime_assault_on_an_officer.Anassaultonanofficer.FileName);
        }
        else
        {
            ScannerList.Add(ScannerAudio.we_have.We_Have_1.FileName);
            ScannerList.Add(ScannerAudio.crime_assault_on_an_officer.Anofficerassault.FileName);
        }
        ReportGenericEnd(ScannerList, true);
        ReportedAssaultOnOfficer = true;
        PlayAudioList(ScannerList, true);
    }
    public static void ReportThreateningWithFirearm()
    {
        if (ReportedOfficerDown || ReportedLethalForceAuthorized)
            return;

        List<string> ScannerList = new List<string>();
        ReportGenericStart(ScannerList);
        ScannerList.Add(ScannerAudio.we_have.We_Have_1.FileName);
        ScannerList.Add(ScannerAudio.crime_suspect_threatening_an_officer_with_a_firearm.Asuspectthreateninganofficerwithafirearm.FileName);
        ReportGenericEnd(ScannerList, true);
        ReportedThreateningWithAFirearm = true;
        PlayAudioList(ScannerList, true);
    }
    public static void ReportSuspectLastSeen(bool Near)
    {
        List<string> ScannerList = new List<string>();
        ReportGenericStart(ScannerList);

        int Num = rnd.Next(1, 4);
        if (Num == 1)
        {
            ScannerList.Add(ScannerAudio.suspect_eluded_pt_1.SuspectEvadedPursuingOfficiers.FileName);
        }
        else if (Num == 2)
        {
            ScannerList.Add(ScannerAudio.suspect_eluded_pt_1.OfficiersHaveLostVisualOnSuspect.FileName);
        }
        else if (Num == 3)
        {
            ScannerList.Add(ScannerAudio.suspect_last_seen.TargetLastReported.FileName);
        }
        else
        {
            ScannerList.Add(ScannerAudio.suspect_last_seen.TargetLastSeen.FileName);
        }
        
        ReportGenericEnd(ScannerList, true);
        PlayAudioList(ScannerList, false);
    }
    public static void ReportSuspectArrested()
    {
        List<string> myList = new List<string>(new string[]
        {
            ScannerAudio.AudioBeeps.AudioStart()
        }) ;

        int Num = rnd.Next(1, 4);
        if (Num == 1)
        {
            myList.Add(ScannerAudio.we_have.We_Have_1.FileName);
            myList.Add(ScannerAudio.crook_arrested.Asuspectincustody1.FileName);
        }
        else if (Num == 2)
        {
            myList.Add(ScannerAudio.we_have.We_Have_1.FileName);
            myList.Add(ScannerAudio.crook_arrested.Asuspectapprehended.FileName);
        }
        else if (Num == 3)
        {
            myList.Add(ScannerAudio.crook_arrested.Officershaveapprehendedsuspect.FileName);
        }
        else
        {
            myList.Add(ScannerAudio.crook_arrested.Officershaveapprehendedsuspect1.FileName);
        }

        myList.Add(ScannerAudio.AudioBeeps.AudioEnd());
        PlayAudioList(myList,true);
    }
    public static void ReportSuspectWasted()
    {
        List<string> myList = new List<string>(new string[]
        {
            ScannerAudio.AudioBeeps.AudioStart()
            
        });

        int Num = rnd.Next(1, 10);
        if (Num == 1)
        {
            myList.Add(ScannerAudio.we_have.We_Have_1.FileName);
            myList.Add(ScannerAudio.crook_killed.Acriminaldown.FileName);
        }
        else if (Num == 2)
        {
            myList.Add(ScannerAudio.we_have.We_Have_1.FileName);
            myList.Add(ScannerAudio.crook_killed.Asuspectdown.FileName);
        }
        else if (Num == 3)
        {
            myList.Add(ScannerAudio.we_have.We_Have_1.FileName);
            myList.Add(ScannerAudio.crook_killed.Asuspectdown2.FileName);
        }
        else if (Num == 4)
        {
            myList.Add(ScannerAudio.we_have.We_Have_2.FileName);
            myList.Add(ScannerAudio.crook_killed.Asuspectdown1.FileName);
        }
        else if (Num == 5)
        {
            myList.Add(ScannerAudio.crook_killed.Criminaldown.FileName);
        }
        else if (Num == 6)
        {
            myList.Add(ScannerAudio.crook_killed.Suspectdown.FileName);
        }
        else if (Num == 7)
        {
            myList.Add(ScannerAudio.crook_killed.Suspectneutralized.FileName);
        }
        else if (Num == 8)
        {
            myList.Add(ScannerAudio.crook_killed.Suspectdownmedicalexaminerenroute.FileName);
        }
        else if (Num == 9)
        {
            myList.Add(ScannerAudio.crook_killed.Suspectdowncoronerenroute.FileName);
        }
        else
        {
            myList.Add(ScannerAudio.crook_killed.Officershavepacifiedsuspect.FileName);
        }



        myList.Add(ScannerAudio.AudioBeeps.AudioEnd());
        PlayAudioList(myList, true);
    }
    public static void ReportLethalForceAuthorized()
    {
        List<string> ScannerList = new List<string>();
        // ReportGenericStart(ScannerList);

        ScannerList.Add(ScannerAudio.AudioBeeps.AudioStart());
        ScannerList.Add(ScannerAudio.attention_all_units_gen.Allunits.FileName);

        int Num = rnd.Next(1, 5);
        if (Num == 1)
        {
            ScannerList.Add(ScannerAudio.attention_all_units_gen.Allunits.FileName);
            ScannerList.Add(ScannerAudio.lethal_force.Useofdeadlyforceauthorized.FileName);
        }
        else if (Num == 2)
        {
            ScannerList.Add(ScannerAudio.attention_all_units_gen.Attentionallunits.FileName);
            ScannerList.Add(ScannerAudio.lethal_force.Useofdeadlyforceisauthorized.FileName);
        }
        else if (Num == 3)
        {
            ScannerList.Add(ScannerAudio.attention_all_units_gen.Attentionallunits1.FileName);
            ScannerList.Add(ScannerAudio.lethal_force.Useofdeadlyforceisauthorized1.FileName);
        }
        else if (Num == 4)
        {
            ScannerList.Add(ScannerAudio.attention_all_units_gen.Attentionallunits2.FileName);
            ScannerList.Add(ScannerAudio.lethal_force.Useoflethalforceisauthorized.FileName);
        }
        else
        {
            ScannerList.Add(ScannerAudio.attention_all_units_gen.Attentionallunits3.FileName);
            ScannerList.Add(ScannerAudio.lethal_force.Useofdeadlyforcepermitted1.FileName);
        }

        ReportGenericEnd(ScannerList, false);
        PlayAudioList(ScannerList, false);
    }
    public static void ReportSuspectLost()
    {
        List<string> ScannerList = new List<string>();
        // ReportGenericStart(ScannerList);

        ScannerList.Add(ScannerAudio.AudioBeeps.AudioStart());

        int Num = rnd.Next(1, 5);
        if (Num == 1)
        {
            ScannerList.Add(ScannerAudio.suspect_eluded_pt_2.AllUnitsRemainOnAlert.FileName);
        }
        else if (Num == 2)
        {
            ScannerList.Add(ScannerAudio.suspect_eluded_pt_2.AllUnitsStandby.FileName);
        }
        else if (Num == 3)
        {
            ScannerList.Add(ScannerAudio.suspect_eluded_pt_2.AllUnitsStayInTheArea.FileName);
        }
        else if (Num == 4)
        {
            ScannerList.Add(ScannerAudio.suspect_eluded_pt_2.AllUnitsRemainOnAlert.FileName);
        }
        else
        {
            ScannerList.Add(ScannerAudio.suspect_eluded_pt_2.AllUnitsStayInTheArea.FileName);
        }

        ReportGenericEnd(ScannerList, false);
        PlayAudioList(ScannerList, false);
    }
    public static void ResetReportedItems()
    {
        ReportedAssaultOnOfficer = false;
        ReportedCarryingWeapon = false;
        ReportedLethalForceAuthorized = false;
        ReportedOfficerDown = false;
        ReportedShotsFired = false;
        ReportedThreateningWithAFirearm = false;
    }
}

