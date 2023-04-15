using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class CanineUnit : Cop
{
    public override string UnitType { get; set; } = "K9";
    public override bool ShouldBustPlayer => false;
    public override bool IsAnimal { get; set; } = true;
    public CanineUnit(Ped pedestrian, ISettingsProvideable settings, int health, Agency agency, bool wasModSpawned, ICrimes crimes, IWeapons weapons, string name, string modelName, IEntityProvideable world) : base(pedestrian, settings, health, agency, wasModSpawned, crimes, weapons, name, modelName, world)
    {

    }
}

