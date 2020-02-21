using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Civilians
{
    public static List<GTAPed> PlayerKilledCivilians;
    public static uint GameTimeLastHurtCivilian;
    public static uint GameTimeLastKilledCivilian;
    public static bool IsRunning;
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
            LocalWriteToLog("CheckKilled", string.Format("PlayerKilled: {0}", MyPed.Pedestrian.Handle));
        }
    }
    private static void LocalWriteToLog(string ProcedureString, string TextToLog)
    {
        if (Settings.PoliceLogging)
            Debugging.WriteToLog(ProcedureString, TextToLog);
    }
}
