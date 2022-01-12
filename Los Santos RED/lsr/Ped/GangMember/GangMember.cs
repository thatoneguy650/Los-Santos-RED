using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System.Collections.Generic;

public class GangMember : PedExt, IWeaponIssuable
{
    private uint GameTimeSpawned;
    private WeaponInventory WeaponInventory;
    public GangMember(Ped _Pedestrian, ISettingsProvideable settings, Gang gang, bool wasModSpawned, bool _WillFight, bool _WillCallPolice, string _Name, PedGroup gameGroup, ICrimes crimes, IWeapons weapons) : base(_Pedestrian, settings, _WillFight, _WillCallPolice, true, false, _Name, gameGroup, crimes, weapons)
    {
        Gang = gang;
        WasModSpawned = wasModSpawned;
        WeaponInventory = new WeaponInventory(this, settings);
        if (WasModSpawned)
        {
            GameTimeSpawned = Game.GameTime;
        }
    }
    public int ShootRate { get; set; } = 600;
    public int Accuracy { get; set; } = 10;
    public int CombatAbility { get; set; } = 0;

    public IssuableWeapon GetRandomWeapon(bool v, IWeapons weapons) => Gang.GetRandomWeapon(v, weapons);
    public void IssueWeapons(IWeapons weapons, uint meleeHash, bool issueSidearm, bool issueLongGun) => WeaponInventory.IssueWeapons(weapons, meleeHash, issueSidearm, issueLongGun);
    public Gang Gang { get; set; } = new Gang();
    public uint HasBeenSpawnedFor => Game.GameTime - GameTimeSpawned;
    public bool WasModSpawned { get; private set; }
}