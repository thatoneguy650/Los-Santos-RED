using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Civilians
{
    public static uint GameTimeLastHurtCivilian { get; set; }
    public static uint GameTimeLastKilledCivilian { get; set; }
    public static bool IsRunning { get; set; }
    public static bool AnyCiviliansCanSeePlayer { get; set; }
    public static bool AnyCiviliansCanRecognizePlayer { get; set; }
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
        if (PedScanning.PlayerKilledCivilians.Any(x => x.Pedestrian.Exists() && x.Pedestrian.DistanceTo2D(Game.LocalPlayer.Character) <= Distance))
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
        {
            AnyCiviliansCanSeePlayer = false;
            AnyCiviliansCanRecognizePlayer = false;
            UI.DebugLine = "";
            return;
        }
            
        UpdateCivilians();
        CheckRecognition();
        CheckSnitchCivilians();
    }
    private static void CheckSnitchCivilians()
    {
       List<Crime> CrimesToCallIn = Police.CurrentCrimes.CurrentlyViolatingCanBeReportedByCivilians;
       if (CrimesToCallIn.Any())
        {
            foreach(GTAPed Snitch in PedScanning.Civilians)
            {
                if (1==1)//LosSantosRED.PlayerIsNotWanted)
                {
                    if (Snitch.CanRecognizePlayer)
                    {
                        foreach (Crime Bad in CrimesToCallIn)
                        {
                            Snitch.AddCrime(Bad, Snitch.Pedestrian.Position);
                        }
                        if (!Snitch.IsTasked && !Snitch.TaskIsQueued && Snitch.CanFlee)
                        {
                            Tasking.AddCivilianTaskToQueue(new CivilianTask(Snitch, Tasking.AssignableTasks.ReactToCrime));
                        }
                    }
                    else if (Snitch.CanHearPlayer && CrimesToCallIn.Any(x => x.CanBeCalledInBySound))
                    {
                        foreach (Crime Bad in CrimesToCallIn.Where(x => x.CanBeCalledInBySound))
                        {
                            Snitch.AddCrime(Bad, Game.LocalPlayer.Character.Position);
                        }
                        if (!Snitch.IsTasked && !Snitch.TaskIsQueued && Snitch.CanFlee)
                        {
                            Tasking.AddCivilianTaskToQueue(new CivilianTask(Snitch, Tasking.AssignableTasks.ReactToCrime));
                        }
                    }
                    else if (Snitch.IsTasked && !Snitch.CanSeePlayer && !Snitch.TaskIsQueued && Snitch.DistanceToPlayer >= 100f)
                    {
                        Tasking.AddCivilianTaskToQueue(new CivilianTask(Snitch, Tasking.AssignableTasks.UntaskCivilian));
                    }
                }
                else
                {
                    if (Snitch.IsTasked && !Snitch.TaskIsQueued)
                    {
                        Tasking.AddCivilianTaskToQueue(new CivilianTask(Snitch, Tasking.AssignableTasks.UntaskCivilian));
                    }
                }
            }
        }
        //int TotalCivisTasked = PoliceScanning.Civilians.Count(x => x.IsTasked && x.TaskType == Tasking.AssignableTasks.ReactToCrime);

        //string CiviLine = "";
        //if (AnyCiviliansCanRecognizePlayer)
        //{
        //    if (CrimesToCallIn.Any())
        //        CiviLine += "~r~(SEEN)";
        //    else
        //        CiviLine += "~s~(SEEN)";
        //}

        UI.DebugLine = string.Format("{0}", string.Join(",", Police.CurrentCrimes.CurrentlyViolatingCanBeReportedByCivilians.Select(x => x.Name)));
    }
    public static void UpdateCivilians()
    {
        PedScanning.Civilians.RemoveAll(x => !x.Pedestrian.Exists());
        foreach (GTAPed MyPed in PedScanning.Civilians)
        {
            if (MyPed.Pedestrian.IsDead)
            {
                CheckCivilianKilled(MyPed);
                if(MyPed.KilledByPlayer)
                    PedScanning.PlayerKilledCivilians.Add(MyPed);
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
        PedScanning.PlayerKilledCivilians.RemoveAll(x => !x.Pedestrian.Exists());
        PedScanning.Civilians.RemoveAll(x => !x.Pedestrian.Exists() || x.Pedestrian.IsDead);
    }
    private static void CheckRecognition()
    {
        AnyCiviliansCanSeePlayer = PedScanning.Civilians.Any(x => x.CanSeePlayer);
        AnyCiviliansCanRecognizePlayer = PedScanning.Civilians.Any(x => x.CanRecognizePlayer);
    }
    public static void CheckCivilianKilled(GTAPed MyPed)
    {
        if (!MyPed.HurtByPlayer && LosSantosRED.PlayerHurtPed(MyPed))
        {
            MyPed.HurtByPlayer = true;
            GameTimeLastHurtCivilian = Game.GameTime;
        }
        if (!MyPed.KilledByPlayer && LosSantosRED.PlayerKilledPed(MyPed))
        {
            MyPed.KilledByPlayer = true;
            GameTimeLastKilledCivilian = Game.GameTime;
            Debugging.WriteToLog("CheckKilled", string.Format("PlayerKilled: {0}", MyPed.Pedestrian.Handle));
        }
    }
}
