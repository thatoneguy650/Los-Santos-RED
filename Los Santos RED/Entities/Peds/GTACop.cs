
using ExtensionsMethods;
using Rage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GTACop : GTAPed
{
    public bool WasModSpawned { get; set; } = false;
    public bool WasRandomSpawnDriver { get; set; } = false;
    public bool WasInvestigationSpawn { get; set; } = false;
    public bool IsBikeCop { get; set; } = false;
    public bool IsPursuitPrimary { get; set; } = false;
    public bool SetTazer { get; set; } = false;
    public bool SetUnarmed { get; set; } = false;
    public bool SetDeadly { get; set; } = false;
    public uint GameTimeLastWeaponCheck { get; set; }
    public uint GameTimeLastTask { get; set; }
    public uint GameTimeLastSpoke { get; set; }
    public uint GameTimeLastRadioed { get; set; }
    public bool HasItemsToRadioIn { get; set; }
    public GTAWeapon IssuedPistol { get; set; } = new GTAWeapon("weapon_pistol", 60, GTAWeapon.WeaponCategory.Pistol, 1, 453432689, true,true,false,true);
    public GTAWeapon IssuedHeavyWeapon { get; set; }
    public GTAWeapon.WeaponVariation PistolVariation { get; set; }
    public GTAWeapon.WeaponVariation HeavyVariation { get; set; }
    public Agency AssignedAgency { get; set; } = new Agency("~s~", "UNK", "Unknown Agency", "White", Agency.Classification.Other, null, null, "",null);
    public bool AtWantedCenterDuringSearchMode { get; set; } = false;
    public bool AtWantedCenterDuringChase { get; set; } = false;
    public ChaseStatus CurrentChaseStatus { get; set; } = ChaseStatus.Idle;
    public enum ChaseStatus
    {
        Idle = 0,
        Investigation = 1,
        Active = 2,
    }
    public void SetAccuracyAndSightRange()
    {
        Pedestrian.VisionRange = 55f;
        Pedestrian.HearingRange = 25;
        if(LosSantosRED.MySettings.Police.OverridePoliceAccuracy)
            Pedestrian.Accuracy = LosSantosRED.MySettings.Police.PoliceGeneralAccuracy;
    }
    public void IssuePistol()
    {
        GTAWeapon Pistol;
        Agency.IssuedWeapon PistolToPick = new Agency.IssuedWeapon("weapon_pistol", true, null);
        if (AssignedAgency != null)
            PistolToPick = AssignedAgency.IssuedWeapons.Where(x => x.IsPistol).PickRandom();
        Pistol = GTAWeapons.WeaponsList.Where(x => x.Name.ToLower() == PistolToPick.ModelName.ToLower() && x.Category == GTAWeapon.WeaponCategory.Pistol).PickRandom();
        IssuedPistol = Pistol;
        if (IssuedPistol == null)
            return;
        Pedestrian.Inventory.GiveNewWeapon(Pistol.Name, Pistol.AmmoAmount, false);
        if (LosSantosRED.MySettings.Police.AllowPoliceWeaponVariations)
        {
            GTAWeapon.WeaponVariation MyVariation = PistolToPick.MyVariation;
            PistolVariation = MyVariation;
            LosSantosRED.ApplyWeaponVariation(Pedestrian, (uint)Pistol.Hash, MyVariation);
        }
    }
    public void IssueHeavyWeapon()
    {
        GTAWeapon IssuedHeavy;

        if (LosSantosRED.MySettings.Police.OverridePoliceAccuracy)
            Pedestrian.Accuracy = LosSantosRED.MySettings.Police.PoliceHeavyAccuracy;

        Agency.IssuedWeapon HeavyToPick = new Agency.IssuedWeapon("weapon_shotgun", true, null);
        if (AssignedAgency != null)
            HeavyToPick = AssignedAgency.IssuedWeapons.Where(x => !x.IsPistol).PickRandom();

        IssuedHeavy = GTAWeapons.WeaponsList.Where(x => x.Name.ToLower() == HeavyToPick.ModelName.ToLower() && x.Category != GTAWeapon.WeaponCategory.Pistol).PickRandom();
        IssuedHeavyWeapon = IssuedHeavy;

        if (IssuedHeavyWeapon == null)
            return;

        Pedestrian.Inventory.GiveNewWeapon(IssuedHeavy.Name, IssuedHeavy.AmmoAmount, true);
        if (LosSantosRED.MySettings.Police.AllowPoliceWeaponVariations)
        {
            GTAWeapon.WeaponVariation MyVariation = HeavyToPick.MyVariation;
            HeavyVariation = MyVariation;
            LosSantosRED.ApplyWeaponVariation(Pedestrian, (uint)IssuedHeavy.Hash, MyVariation);
        }
    }
    public bool NeedsWeaponCheck
    {
        get
        {
            if (GameTimeLastWeaponCheck == 0)
                return true;
            else if (Game.GameTime > GameTimeLastWeaponCheck + 500)
                return true;
            else
                return false;
        }       
    }
    public uint HasBeenSpawnedFor
    {
        get
        {
            return Game.GameTime - GameTimeSpawned;
        }
    }
    public bool CanSpeak
    {
        get
        {
            if (GameTimeLastSpoke == 0)
                return true;
            else if (Game.GameTime - GameTimeLastSpoke >= 15000)
                return true;
            else
                return false;
        }
    }
    public bool CanRadio
    {
        get
        {
            if (GameTimeLastRadioed == 0)
                return true;
            else if (Game.GameTime - GameTimeLastRadioed >= 15000)
                return true;
            else
                return false;
        }
    }
    public bool CanBeDeleted
    {
        get
        {
            if (WasModSpawned && HasBeenSpawnedFor >= 20000)
                return true;
            else
                return false;
        }
    }
    public GTACop(Ped _Pedestrian, bool _canSeePlayer, int _Health, Agency _Agency) : base(_Pedestrian, _canSeePlayer, _Health)
    {
        IsCop = true;
        AssignedAgency = _Agency;
        SetAccuracyAndSightRange();
    }
    public GTACop(Ped _Pedestrian, bool _canSeePlayer, uint _gameTimeLastSeenPlayer, Vector3 _positionLastSeenPlayer, int _Health, Agency _Agency) : base(_Pedestrian, _canSeePlayer, _Health)
    {
        IsCop = true;
        Pedestrian = _Pedestrian;
        CanSeePlayer = _canSeePlayer;
        GameTimeLastSeenPlayer = _gameTimeLastSeenPlayer;
        PositionLastSeenPlayer = _positionLastSeenPlayer;
        Health = _Health;
        AssignedAgency = _Agency;
        SetAccuracyAndSightRange();
    }
}

