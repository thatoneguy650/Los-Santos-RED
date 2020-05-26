
using ExtensionsMethods;
using Rage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Cop : PedExt
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
    public Agencies.Agency AssignedAgency { get; set; } = new Agencies.Agency("~s~", "UNK", "Unknown Agency", "White", Agencies.Agency.Classification.Other, null, null, "",null);
    public bool AtWantedCenterDuringSearchMode { get; set; } = false;
    public bool AtWantedCenterDuringChase { get; set; } = false;
    public Zone CurrentZone
    {
        get
        {
            return Zones.GetZoneAtLocation(Pedestrian.Position);
        }
    }

    public void SetAccuracyAndSightRange()
    {
        Pedestrian.VisionRange = 55f;
        Pedestrian.HearingRange = 25;
        if(General.MySettings.Police.OverridePoliceAccuracy)
            Pedestrian.Accuracy = General.MySettings.Police.PoliceGeneralAccuracy;
    }
    public void IssuePistol()
    {
        GTAWeapon Pistol;
        Agencies.Agency.IssuedWeapon PistolToPick = new Agencies.Agency.IssuedWeapon("weapon_pistol", true, null);
        if (AssignedAgency != null)
            PistolToPick = AssignedAgency.IssuedWeapons.Where(x => x.IsPistol).PickRandom();
        Pistol = Weapons.WeaponsList.Where(x => x.Name.ToLower() == PistolToPick.ModelName.ToLower() && x.Category == GTAWeapon.WeaponCategory.Pistol).PickRandom();
        IssuedPistol = Pistol;
        if (IssuedPistol == null)
            return;
        Pedestrian.Inventory.GiveNewWeapon(Pistol.Name, Pistol.AmmoAmount, false);
        if (General.MySettings.Police.AllowPoliceWeaponVariations)
        {
            GTAWeapon.WeaponVariation MyVariation = PistolToPick.MyVariation;
            PistolVariation = MyVariation;
            General.ApplyWeaponVariation(Pedestrian, (uint)Pistol.Hash, MyVariation);
        }
    }
    public void IssueHeavyWeapon()
    {
        GTAWeapon IssuedHeavy;

        if (General.MySettings.Police.OverridePoliceAccuracy)
            Pedestrian.Accuracy = General.MySettings.Police.PoliceHeavyAccuracy;

        Agencies.Agency.IssuedWeapon HeavyToPick = new Agencies.Agency.IssuedWeapon("weapon_shotgun", true, null);
        if (AssignedAgency != null)
            HeavyToPick = AssignedAgency.IssuedWeapons.Where(x => !x.IsPistol).PickRandom();

        IssuedHeavy = Weapons.WeaponsList.Where(x => x.Name.ToLower() == HeavyToPick.ModelName.ToLower() && x.Category != GTAWeapon.WeaponCategory.Pistol).PickRandom();
        IssuedHeavyWeapon = IssuedHeavy;

        if (IssuedHeavyWeapon == null)
            return;

        Pedestrian.Inventory.GiveNewWeapon(IssuedHeavy.Name, IssuedHeavy.AmmoAmount, true);
        if (General.MySettings.Police.AllowPoliceWeaponVariations)
        {
            GTAWeapon.WeaponVariation MyVariation = HeavyToPick.MyVariation;
            HeavyVariation = MyVariation;
            General.ApplyWeaponVariation(Pedestrian, (uint)IssuedHeavy.Hash, MyVariation);
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
            if (WasModSpawned && HasBeenSpawnedFor >= 15000)// && GameTimeSpawned > Police.GameTimeWantedStarted)
                return true;
            else
                return false;
        }
    }
    public int CountNearbyCops
    {
        get
        {
            return PedList.CopPeds.Count(x => x.Pedestrian.Exists() && Pedestrian.Handle != x.Pedestrian.Handle && x.Pedestrian.DistanceTo2D(Pedestrian) <= 50f);
        }
    }
    public Cop(Ped _Pedestrian, int _Health, Agencies.Agency _Agency) : base(_Pedestrian, _Health)
    {
        IsCop = true;
        Health = _Health;
        AssignedAgency = _Agency;
        SetAccuracyAndSightRange();
    }
}

