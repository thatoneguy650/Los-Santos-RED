using LosSantosRED.lsr;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CivilianPerception
{
    public bool AnyCanSeePlayer { get; set; }
    public bool AnyCanHearPlayer { get; set; }
    public bool AnyCanRecognizePlayer { get; set; }
    public void Tick()
    {
        UpdateCivilians();
        UpdateRecognition();
    }
    private void UpdateCivilians()
    {
        PedManager.Civilians.RemoveAll(x => !x.Pedestrian.Exists());
        foreach (PedExt MyPed in PedManager.Civilians)
        {
            MyPed.Update();
        }
        PedManager.Civilians.RemoveAll(x => !x.Pedestrian.Exists()  || x.Pedestrian.IsDead);
        VehicleManager.CivilianVehicles.RemoveAll(x => !x.VehicleEnt.Exists());
    }
    private void UpdateRecognition()
    {
        AnyCanSeePlayer = PedManager.Civilians.Any(x => x.CanSeePlayer);
        AnyCanHearPlayer = PedManager.Civilians.Any(x => x.WithinWeaponsAudioRange);
        AnyCanRecognizePlayer = PedManager.Civilians.Any(x => x.CanRecognizePlayer);
    }
}
