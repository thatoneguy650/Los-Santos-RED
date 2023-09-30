using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System.Collections.Generic;

public class Zombie : PedExt
{
    public Zombie(Ped pedestrian, ISettingsProvideable settings, int health, bool wasModSpawned, ICrimes crimes, IWeapons weapons, string name, string groupName, IEntityProvideable world) : base(pedestrian, settings, crimes, weapons, name, groupName, world)
    {
        Health = health;
        WasModSpawned = wasModSpawned;
        IsZombie = true;
    }
    public override bool KnowsDrugAreas => false;
    public override bool KnowsGangAreas => false;
}