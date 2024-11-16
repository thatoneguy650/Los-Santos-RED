using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class TaxiDriver : PedExt, IWeaponIssuable
{
    public TaxiFirm TaxiFirm { get; set; }
    public TaxiRide TaxiRide { get; set; }
    public WeaponInventory WeaponInventory { get; private set; }
    public bool HasTaser { get; set; } = false;
    public bool IsUsingMountedWeapon { get; set; } = false;
    public override bool HasWeapon => WeaponInventory.HasPistol || WeaponInventory.HasLongGun;
    public IssuableWeapon GetRandomMeleeWeapon(IWeapons weapons) => TaxiFirm.GetRandomMeleeWeapon(weapons);
    public IssuableWeapon GetRandomWeapon(bool v, IWeapons weapons) => TaxiFirm.GetRandomWeapon(v, weapons);
    public bool WillCancelRide => HasSeenPlayerCommitMajorCrime || (Pedestrian.Exists() && Pedestrian.IsFleeing);
    public TaxiDriver(Ped _Pedestrian, ISettingsProvideable settings, ICrimes crimes, IWeapons weapons, string _Name, string groupName, IEntityProvideable world, bool wasModSpawned) : base(_Pedestrian, settings, crimes, weapons, _Name, groupName, world)
    {
        WasModSpawned = wasModSpawned;
        PedBrain = new TaxiDriverBrain(this, Settings, world, weapons, this);
        WeaponInventory = new WeaponInventory(this, settings);
    }
    public void SetTaxiRide(TaxiRide taxiRide)
    {
        if(TaxiRide != null)
        {
            TaxiRide.Cancel();
        }
        TaxiRide = taxiRide;
        if (taxiRide != null)
        {
            SetPersistent();
        }
    }
    public override void SetPersistent()
    {
        if (!WasModSpawned && Pedestrian.Exists() && !Pedestrian.IsPersistent)
        {
            WasEverSetPersistent = true;
            Pedestrian.IsPersistent = true;
            EntryPoint.WriteToConsole($"STORING PED {Handle} MAKING PERSIS");
        }
    }
    public override void SetNonPersistent()
    {
        if (!WasModSpawned && Pedestrian.Exists() && Pedestrian.IsPersistent)
        {
            Pedestrian.IsPersistent = false;
            EntryPoint.WriteToConsole($"RELEASING PED {Handle} MAKING NON PERSIS");
        }
    }
}

