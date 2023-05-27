using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class SecurityDog : SecurityGuard
{
    public SecurityDog(Ped pedestrian, ISettingsProvideable settings, int health, Agency agency, bool wasModSpawned, ICrimes crimes, IWeapons weapons, string name, string modelName, IEntityProvideable world) : base(pedestrian, settings, health, agency, wasModSpawned, crimes, weapons, name, modelName, world)
    {
    }

    public override bool IsAnimal { get; set; } = true;
    public override int DefaultCombatFlag { get; set; } = 134217728;//disable aim intro
    public override int DefaultEnterExitFlag { get; set; } = (int)eEnter_Exit_Vehicle_Flags.ECF_WARP_PED;
    public override bool IsTrustingOfPlayer { get; set; } = false;
    public override bool CanConverse => false;
    public override bool CanTransact => false;
    public override bool CanBeLooted { get; set; } = false;
    public override bool CanBeDragged { get; set; } = false;

}

