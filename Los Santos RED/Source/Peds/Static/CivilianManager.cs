using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class CivilianManager
{
    public static bool IsRunning { get; set; }
    public static bool AnyCanSeePlayer { get; private set; }
    public static bool AnyCanHearPlayer { get; private set; }
    public static bool AnyCanRecognizePlayer { get; private set; }
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
        PedManager.Civilians.RemoveAll(x => !x.Pedestrian.Exists());
        foreach (PedExt MyPed in PedManager.Civilians)
        {
            MyPed.Update();
        }
        PedManager.Civilians.RemoveAll(x => !x.Pedestrian.Exists()  || x.Pedestrian.IsDead);
        VehicleManager.CivilianVehicles.RemoveAll(x => !x.VehicleEnt.Exists());
    }
    private static void UpdateRecognition()
    {
        AnyCanSeePlayer = PedManager.Civilians.Any(x => x.CanSeePlayer);
        AnyCanHearPlayer = PedManager.Civilians.Any(x => x.WithinWeaponsAudioRange);
        AnyCanRecognizePlayer = PedManager.Civilians.Any(x => x.CanRecognizePlayer);
    }
}
