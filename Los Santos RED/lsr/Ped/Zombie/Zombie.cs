using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System.Collections.Generic;

public class Zombie : PedExt
{
    private uint GameTimeSpawned;
    public Zombie(Ped pedestrian, ISettingsProvideable settings, int health, bool wasModSpawned, ICrimes crimes, IWeapons weapons, string name, string groupName) : base(pedestrian, settings, crimes, weapons, name, groupName)
    {
        Health = health;
        WasModSpawned = wasModSpawned;
        if (WasModSpawned)
        {
            GameTimeSpawned = Game.GameTime;
        }
        IsZombie = true;
    }
    public uint HasBeenSpawnedFor => Game.GameTime - GameTimeSpawned;
   // public bool WasModSpawned { get; private set; }
}