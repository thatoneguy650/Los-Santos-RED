using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Flee : ComplexTask
{
    private ITargetable Target;
    public Flee(IComplexTaskable ped, ITargetable player) : base(player, ped, 0)
    {
        Name = "Flee";
        SubTaskName = "";
        Target = player;
    }
    public override void Start()
    {
        Ped.Pedestrian.Tasks.Flee(Target.Character, 100f, -1);
        GameTimeLastRan = Game.GameTime;
    }
    public override void Update()
    {

    }
    public override void Stop()
    {

    }
}

