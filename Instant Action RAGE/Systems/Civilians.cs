using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Civilians
{
    private static int PrevCiviliansKilledByPlayer;
    private static bool PrevPlayerKilledCivilians;
    public static bool IsRunning;
    public static void Initialize()
    {
        IsRunning = true;
        PrevPlayerKilledCivilians = false;
        PrevCiviliansKilledByPlayer = 0;
    }
    public static void Dispose()
    {
        IsRunning = false;
    }
    public static void CivilianTick()
    {
        UpdateCivilians();
    }
    public static void UpdateCivilians()
    {
        PoliceScanning.Civilians.RemoveAll(x => !x.Pedestrian.Exists());
        foreach (GTAPed MyPed in PoliceScanning.Civilians)
        {
            if (MyPed.Pedestrian.IsDead)
            {
                CheckCivilianKilled(MyPed);
                continue;
            }
            int NewHealth = MyPed.Pedestrian.Health;
            if (NewHealth != MyPed.Health)
            {
                if (InstantAction.PlayerHurtPed(MyPed))
                {
                    MyPed.HurtByPlayer = true;
                }
                MyPed.Health = NewHealth;
            }
        }
        PoliceScanning.Civilians.RemoveAll(x => !x.Pedestrian.Exists() || x.Pedestrian.IsDead);

        if (PrevPlayerKilledCivilians != Police.PlayerKilledCivilians)
            PlayerKilledCiviliansChanged();

        if (PrevCiviliansKilledByPlayer != Police.CiviliansKilledByPlayer)
            CiviliansKilledChanged();
    }
    public static void CheckCivilianKilled(GTAPed MyPed)
    {
        if (InstantAction.PlayerHurtPed(MyPed))
        {
            MyPed.HurtByPlayer = true;
            Police.GameTimeLastHurtCivilian = Game.GameTime;
        }
        if (InstantAction.PlayerKilledPed(MyPed))
        {
            Police.GameTimeLastKilledCivilian = Game.GameTime;
            Police.CiviliansKilledByPlayer++;
            Police.PlayerKilledCivilians = true;
            LocalWriteToLog("CheckKilled", string.Format("PlayerKilled: {0}", MyPed.Pedestrian.Handle));
        }
    }
    private static void PlayerKilledCiviliansChanged()
    {
        LocalWriteToLog("ValueChecker", String.Format("PlayerKilledCivilians Changed to: {0}", Police.PlayerKilledCivilians));
        if (Police.PlayerKilledCivilians)
        {
            //SetWantedLevel(3, "You killed a police officer");
            //if (InstantAction.PlayerWantedLevel <= 3)
            //    DispatchAudio.AddDispatchToQueue(new DispatchAudio.DispatchQueueItem(DispatchAudio.ReportDispatch.ReportOfficerDown, 1) { ResultsInLethalForce = true });
        }
        PrevPlayerKilledCivilians = Police.PlayerKilledCivilians;
    }
    private static void CiviliansKilledChanged()
    {
        LocalWriteToLog("ValueChecker", String.Format("CiviliansKilledChanged Changed to: {0}", Police.CiviliansKilledByPlayer));
        PrevCiviliansKilledByPlayer = Police.CiviliansKilledByPlayer;
    }
    private static void LocalWriteToLog(string ProcedureString, string TextToLog)
    {
        if (Settings.PoliceLogging)
            Debugging.WriteToLog(ProcedureString, TextToLog);
    }
}
