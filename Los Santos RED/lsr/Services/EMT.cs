using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System.Collections.Generic;

public class EMT : PedExt
{
    private uint GameTimeSpawned;
    public EMT(Ped pedestrian, int health, Agency agency, bool wasModSpawned) : base(pedestrian)
    {
        Health = health;
        AssignedAgency = agency;
        WasModSpawned = wasModSpawned;
        if (WasModSpawned)
        {
            GameTimeSpawned = Game.GameTime;
        }
    }
    public Agency AssignedAgency { get; set; } = new Agency();
    public uint HasBeenSpawnedFor => Game.GameTime - GameTimeSpawned;
    public bool WasModSpawned { get; private set; }
}