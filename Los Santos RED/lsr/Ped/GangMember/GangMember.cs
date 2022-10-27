using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System.Collections.Generic;

public class GangMember : PedExt, IWeaponIssuable
{
    private uint GameTimeSpawned;
    
    public GangMember(Ped _Pedestrian, ISettingsProvideable settings, Gang gang, bool wasModSpawned, bool _WillFight, bool _WillCallPolice, string _Name, ICrimes crimes, IWeapons weapons, IEntityProvideable world, bool willFightPolice) : base(_Pedestrian, settings, _WillFight, _WillCallPolice, true, false, _Name, crimes, weapons, gang.MemberName, world, willFightPolice)
    {
        Gang = gang;
        WasModSpawned = wasModSpawned;
        WeaponInventory = new WeaponInventory(this, settings);
        if (WasModSpawned)
        {
            GameTimeSpawned = Game.GameTime;
        }
        IsTrustingOfPlayer = RandomItems.RandomPercent(Gang.PercentageTrustingOfPlayer);
        Money = RandomItems.GetRandomNumberInt(Gang.AmbientMemberMoneyMin, Gang.AmbientMemberMoneyMax);
    }
    public int ShootRate { get; set; } = 400;
    public int Accuracy { get; set; } = 5;
    public int CombatAbility { get; set; } = 0;
    public int TaserAccuracy { get; set; } = 10;
    public int TaserShootRate { get; set; } = 100;
    public int VehicleAccuracy { get; set; } = 10;
    public int VehicleShootRate { get; set; } = 100;

    public WeaponInventory WeaponInventory { get; private set; }
    public IssuableWeapon GetRandomMeleeWeapon(IWeapons weapons) => Gang.GetRandomMeleeWeapon(weapons);
    public IssuableWeapon GetRandomWeapon(bool v, IWeapons weapons) => Gang.GetRandomWeapon(v, weapons);
    public Gang Gang { get; set; } = new Gang();
    public uint HasBeenSpawnedFor => Game.GameTime - GameTimeSpawned;
    public bool HasTaser { get; set; } = false;
    public new string FormattedName => (PlayerKnownsName ? Name : GroupName);
    public override void OnBecameWanted()
    {
        if (Pedestrian.Exists())
        {
            if (Gang != null)
            {
                RelationshipGroup.Cop.SetRelationshipWith(Pedestrian.RelationshipGroup, Relationship.Hate);
                Pedestrian.RelationshipGroup.SetRelationshipWith(RelationshipGroup.Cop, Relationship.Hate);
                Gang.HasWantedMembers = true;
                EntryPoint.WriteToConsole($"{Pedestrian.Handle} BECAME WANTED (GANG MEMBER) SET {Gang.ID} TO HATES COPS");
            }
            EntryPoint.WriteToConsole($"{Pedestrian.Handle} BECAME WANTED (GANG MEMBER)");
        }
    }
    public override void OnLostWanted()
    {
        if(Pedestrian.Exists())
        {
            PedViolations.Reset();
            EntryPoint.WriteToConsole($"{Pedestrian.Handle} LOST WANTED (GANG MEMBER)");
        }
    }
}