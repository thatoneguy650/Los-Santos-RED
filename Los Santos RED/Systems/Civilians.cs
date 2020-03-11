using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Civilians
{
    private static uint GameTimeCiviliansLastReported;
    public static List<GTAPed> PlayerKilledCivilians { get; set; }
    public static uint GameTimeLastHurtCivilian { get; set; }
    public static uint GameTimeLastKilledCivilian { get; set; }
    public static bool IsRunning { get; set; }
    public static bool AnyCiviliansCanSeePlayer { get; set; }
    public static bool AnyCiviliansCanRecognizePlayer { get; set; }
    public static bool CivilianRecentlyReportedCrime(uint TimeSince)
    {
        if (GameTimeCiviliansLastReported == 0)
            return false;
        else if (Game.GameTime - GameTimeCiviliansLastReported <= TimeSince)
            return true;
        else
            return false;
    }
    public static bool RecentlyHurtCivilian(uint TimeSince)
    {
        if (GameTimeLastHurtCivilian == 0)
            return false;
        else if (Game.GameTime - GameTimeLastHurtCivilian <= TimeSince)
            return true;
        else
            return false;
    }
    public static bool RecentlyKilledCivilian(uint TimeSince)
    {
        if (GameTimeLastKilledCivilian == 0)
            return false;
        else if (Game.GameTime - GameTimeLastKilledCivilian <= TimeSince)
            return true;
        else
            return false;
    }
    public static bool NearMurderVictim(float Distance)
    {
        if (PoliceScanning.PlayerKilledCivilians.Any(x => x.Pedestrian.Exists() && x.Pedestrian.DistanceTo2D(Game.LocalPlayer.Character) <= Distance))
            return true;
        else
            return false;
    }
    public static void Initialize()
    {
        IsRunning = true;
        GameTimeLastHurtCivilian = 0;
        GameTimeLastKilledCivilian = 0;
    }
    public static void Dispose()
    {
        IsRunning = false;
    }
    public static void CivilianTick()
    {
        if (LosSantosRED.PlayerIsWanted)//for now dont do any work when we are wanted, the base game can do that for us
            return;

        UpdateCivilians();
        CheckRecognition();
        CheckSnitchCivilians();
    }
    private static void CheckSnitchCivilians()
    {

       bool CiviliansCanReport = Police.CurrentCrimes.CiviliansCanReport;
       if (LosSantosRED.PlayerIsNotWanted && AnyCiviliansCanRecognizePlayer && CiviliansCanReport)
        {
            foreach(GTAPed Snitch in PoliceScanning.Civilians.Where(x => (x.HasSeenPlayerFor >= 5000 && x.DistanceToPlayer <= 10f) || (x.DistanceToPlayer <= 5f && x.DistanceToPlayer > 0f)))
            {
                foreach(Crime Bad in Police.CurrentCrimes.CurrentlyViolating)
                {
                    Snitch.AddCrime(Bad);
                }

                if (!Snitch.CanFlee)
                    continue;
                if (Game.GameTime - Snitch.GameTimeLastTaskedFlee >= 15000 || Snitch.GameTimeLastTaskedFlee == 0)
                {
                    
                    if (Snitch.WillCallPolice)
                        CivilianReportCrime(Snitch);

                    Snitch.Pedestrian.Tasks.ReactAndFlee(Game.LocalPlayer.Character);
                    Snitch.GameTimeLastTaskedFlee = Game.GameTime;
                } 
            }
        }
        UI.DebugLine = string.Format("CiviSee: {0},CiviRec: {1},Violate: {2},RecentReport: {3}", AnyCiviliansCanSeePlayer, AnyCiviliansCanRecognizePlayer, CiviliansCanReport, CivilianRecentlyReportedCrime(25000));
    }
    private static void CivilianReportCrime(GTAPed CivilianToReport)
    {
        if (CivilianToReport == null)
            return;
        if (!CivilianToReport.Pedestrian.Exists())
            return;

        GameFiber CrimeReportedFiber = GameFiber.StartNew(delegate
        {
            uint GameTimeStarted = Game.GameTime;
            int TimeToWait = LosSantosRED.MyRand.Next(3000, 7000);
            while (Game.GameTime - GameTimeStarted <= TimeToWait)
            {
                if (!CivilianToReport.CanFlee)
                    return;
                if (PedSwapping.JustTakenOver(2000))
                    return;
                GameFiber.Sleep(200);
            }
            if (CivilianToReport == null)
                return;
            if (!CivilianToReport.Pedestrian.Exists())
                return;

            Debugging.WriteToLog("Check Snitches", string.Format("Civilian Reporting: {0},Crimes: {1}", CivilianToReport.Pedestrian.Handle, string.Join(",", CivilianToReport.CrimesWitnessed.Select(x => x.DebugName))));

            //Call It In
            Rage.Native.NativeFunction.CallByName<bool>("TASK_USE_MOBILE_PHONE_TIMED", CivilianToReport.Pedestrian, 10000);
            CivilianToReport.Pedestrian.PlayAmbientSpeech("JACKED_GENERIC");


            Crime WorstCrime = CivilianToReport.CrimesWitnessed.Where(x => !x.RecentlyReportedCrime(60000)).OrderBy(x => x.DispatchToPlay.Priority).FirstOrDefault();
            if (WorstCrime == null)
            {
                return;
            }

            Debugging.WriteToLog("Crime Pre Reported", WorstCrime.DebugName);
            GameTimeStarted = Game.GameTime;
            TimeToWait = LosSantosRED.MyRand.Next(3000, 7000);
            while (Game.GameTime - GameTimeStarted <= TimeToWait)
            {
                if (PedSwapping.JustTakenOver(2000))
                    return;
                GameFiber.Sleep(200);
            }

            if (LosSantosRED.PlayerIsWanted)
                return;

            WorstCrime.DispatchToPlay.ReportedBy = DispatchAudio.ReportType.Civilians;
            if (Game.LocalPlayer.Character.IsInAnyVehicle(false))
                WorstCrime.DispatchToPlay.VehicleToReport = LosSantosRED.GetPlayersCurrentTrackedVehicle();
            
            DispatchAudio.AddDispatchToQueue(WorstCrime.DispatchToPlay);
            Police.PoliceInInvestigationMode = true;
            Police.InvestigationPosition = Game.LocalPlayer.Character.Position;

        }, "CrimeCalledInByCivilians");
        Debugging.GameFibers.Add(CrimeReportedFiber);
    }
    public static void UpdateCivilians()
    {
        PoliceScanning.Civilians.RemoveAll(x => !x.Pedestrian.Exists());
        foreach (GTAPed MyPed in PoliceScanning.Civilians)
        {
            if (MyPed.Pedestrian.IsDead)
            {
                CheckCivilianKilled(MyPed);
                if(MyPed.KilledByPlayer)
                    PoliceScanning.PlayerKilledCivilians.Add(MyPed);
                //continue;
            }
            int NewHealth = MyPed.Pedestrian.Health;
            if (NewHealth != MyPed.Health)
            {
                if (LosSantosRED.PlayerHurtPed(MyPed))
                    MyPed.HurtByPlayer = true;
                MyPed.Health = NewHealth;
            }
            MyPed.UpdateDistance();
        }
        PoliceScanning.PlayerKilledCivilians.RemoveAll(x => !x.Pedestrian.Exists());
        PoliceScanning.Civilians.RemoveAll(x => !x.Pedestrian.Exists() || x.Pedestrian.IsDead);
    }
    private static void CheckRecognition()
    {
        AnyCiviliansCanSeePlayer = PoliceScanning.Civilians.Any(x => x.canSeePlayer);
        AnyCiviliansCanRecognizePlayer = PoliceScanning.Civilians.Any(x => (x.HasSeenPlayerFor >= 5000 && x.DistanceToPlayer <= 10f) || (x.DistanceToPlayer <= 5f && x.DistanceToPlayer > 0f));
        
    }
    public static void CheckCivilianKilled(GTAPed MyPed)
    {
        if (LosSantosRED.PlayerHurtPed(MyPed))
        {
            MyPed.HurtByPlayer = true;
            GameTimeLastHurtCivilian = Game.GameTime;
        }
        if (LosSantosRED.PlayerKilledPed(MyPed))
        {
            MyPed.KilledByPlayer = true;
            GameTimeLastKilledCivilian = Game.GameTime;
            Debugging.WriteToLog("CheckKilled", string.Format("PlayerKilled: {0}", MyPed.Pedestrian.Handle));
        }
    }
}
