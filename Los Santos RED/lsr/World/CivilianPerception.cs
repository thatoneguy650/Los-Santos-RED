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
        foreach (PedExt MyPed in Mod.World.Pedestrians.Civilians.Where(x => x.Pedestrian.Exists() && x.Pedestrian.IsAlive))
        {
            MyPed.Update();
        }
    }
    private void UpdateRecognition()
    {
        AnyCanSeePlayer = Mod.World.Pedestrians.Civilians.Any(x => x.CanSeePlayer);
        AnyCanHearPlayer = Mod.World.Pedestrians.Civilians.Any(x => x.WithinWeaponsAudioRange);
        AnyCanRecognizePlayer = Mod.World.Pedestrians.Civilians.Any(x => x.CanRecognizePlayer);
    }
}
