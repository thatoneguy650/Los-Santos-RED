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
       List<Crime> CrimesToCallIn = Police.CurrentCrimes.CurrentlyViolatingCanBeReportedByCivilians;
       if (LosSantosRED.PlayerIsNotWanted && CrimesToCallIn.Any())
        {
            foreach(GTAPed Snitch in PoliceScanning.Civilians)
            {
                if (Snitch.CanRecognizePlayer)
                {
                    foreach (Crime Bad in CrimesToCallIn)
                    {
                        Snitch.AddCrime(Bad);
                    }
                    if (!Snitch.isTasked && !Snitch.TaskIsQueued && Snitch.CanFlee)
                    {
                        Tasking.AddCivilianTaskToQueue(new CivilianTask(Snitch, Tasking.AssignableTasks.ReactToCrime));
                    }
                }
                else if (Snitch.CanHearPlayer)
                {
                    foreach (Crime Bad in CrimesToCallIn.Where(x => x.CanBeCalledInBySound))
                    {
                        Snitch.AddCrime(Bad);
                    }
                    if (!Snitch.isTasked && !Snitch.TaskIsQueued && Snitch.CanFlee)
                    {
                        Tasking.AddCivilianTaskToQueue(new CivilianTask(Snitch, Tasking.AssignableTasks.ReactToCrime));
                    }
                }
                else if(Snitch.isTasked && !Snitch.CanSeePlayer && !Snitch.TaskIsQueued && Snitch.DistanceToPlayer >= 100f)
                {
                    Tasking.AddCivilianTaskToQueue(new CivilianTask(Snitch, Tasking.AssignableTasks.UntaskCivilian));
                }
            }
        }
        int TotalCivisTasked = PoliceScanning.Civilians.Count(x => x.isTasked && x.TaskType == Tasking.AssignableTasks.ReactToCrime);
        UI.DebugLine = string.Format("CiviRec: {0},Violate: {1},Cnt: {2}", AnyCiviliansCanRecognizePlayer, string.Join(",",CrimesToCallIn.Select(x => x.DebugName)), TotalCivisTasked);
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
        AnyCiviliansCanSeePlayer = PoliceScanning.Civilians.Any(x => x.CanSeePlayer);
        AnyCiviliansCanRecognizePlayer = PoliceScanning.Civilians.Any(x => x.CanRecognizePlayer);
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
