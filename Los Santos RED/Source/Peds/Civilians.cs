using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Civilians
{
    public static bool IsRunning { get; set; }
    public static bool AnyCanSeePlayer { get; set; }
    public static bool AnyCanHearPlayer { get; set; }
    public static bool AnyCanRecognizePlayer { get; set; }
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
            UpdateRecognition();
        }
    }
    private static void UpdateCivilians()
    {
        PedList.Civilians.RemoveAll(x => !x.Pedestrian.Exists());
        foreach (PedExt MyPed in PedList.Civilians)
        {
            MyPed.Update();
        }
        PedList.Civilians.RemoveAll(x => !x.Pedestrian.Exists()  || x.Pedestrian.IsDead);   
    }
    private static void UpdateRecognition()
    {
        AnyCanSeePlayer = PedList.Civilians.Any(x => x.CanSeePlayer);
        AnyCanHearPlayer = PedList.Civilians.Any(x => x.WithinWeaponsAudioRange);
        AnyCanRecognizePlayer = PedList.Civilians.Any(x => x.CanRecognizePlayer);
    }
}
