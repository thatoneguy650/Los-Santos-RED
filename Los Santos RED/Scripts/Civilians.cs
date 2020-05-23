using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Civilians
{
    public static bool IsRunning { get; set; }
    public static bool AnyCiviliansCanSeePlayer { get; set; }
    public static bool AnyCiviliansCanRecognizePlayer { get; set; }

    public static void Initialize()
    {
        IsRunning = true;
    }
    public static void Dispose()
    {
        IsRunning = false;
    }
    public static void Tick()
    {
        if (IsRunning)
        {
            UpdateCivilians();
            CheckRecognition();
            CheckSnitchCivilians();
        }
    }
    private static void CheckSnitchCivilians()
    {
       List<Crime> CrimesToCallIn = Police.CurrentCrimes.CurrentlyViolatingCanBeReportedByCivilians;
       if (CrimesToCallIn.Any())
        {
            foreach(GTAPed Snitch in PedList.Civilians)
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
                        Snitch.AddCrime(Bad, Snitch.Pedestrian.Position);
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
        }
    }
    public static void UpdateCivilians()
    {
        PedList.Civilians.RemoveAll(x => !x.Pedestrian.Exists());
        foreach (GTAPed MyPed in PedList.Civilians)
        {
            MyPed.Update();
        }
        PedList.Civilians.RemoveAll(x => !x.Pedestrian.Exists() || x.Pedestrian.IsDead);
    }
    private static void CheckRecognition()
    {
        AnyCiviliansCanSeePlayer = PedList.Civilians.Any(x => x.CanSeePlayer);
        AnyCiviliansCanRecognizePlayer = PedList.Civilians.Any(x => x.CanRecognizePlayer);
    }
}
