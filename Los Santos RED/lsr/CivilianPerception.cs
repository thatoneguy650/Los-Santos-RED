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
        Mod.PedManager.Civilians.RemoveAll(x => !x.Pedestrian.Exists());
        foreach (PedExt MyPed in Mod.PedManager.Civilians)
        {
            MyPed.Update();
        }
        Mod.PedManager.Civilians.RemoveAll(x => !x.Pedestrian.Exists()  || x.Pedestrian.IsDead);
        Mod.VehicleManager.CivilianVehicles.RemoveAll(x => !x.VehicleEnt.Exists());
    }
    private void UpdateRecognition()
    {
        AnyCanSeePlayer = Mod.PedManager.Civilians.Any(x => x.CanSeePlayer);
        AnyCanHearPlayer = Mod.PedManager.Civilians.Any(x => x.WithinWeaponsAudioRange);
        AnyCanRecognizePlayer = Mod.PedManager.Civilians.Any(x => x.CanRecognizePlayer);
    }
}
