
using Rage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GTACop : GTAPed
{
    public bool WasRandomSpawn { get; set; } = false;
    public bool WasRandomSpawnDriver { get; set; } = false;
    public bool WasInvestigationSpawn { get; set; } = false;
    public bool IsBikeCop { get; set; } = false;
    public bool IsSwat { get; set; } = false;
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
    public Agency AssignedAgency { get; set; } = new Agency("~s~", "UNK", "Unknown Agency", Color.White, Agency.Classification.Other, true, false, null, null, "");
    public bool AtWantedCenterDuringSearchMode { get; set; } = false;
    public bool AtWantedCenterDuringChase { get; set; } = false;
    public void SetAccuracyAndSightRange()
    {
        Pedestrian.VisionRange = 55f;
        Pedestrian.HearingRange = 25;
        if(LosSantosRED.MySettings.Police.OverridePoliceAccuracy)
            Pedestrian.Accuracy = LosSantosRED.MySettings.Police.PoliceGeneralAccuracy;
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
    public GTACop(Ped _Pedestrian, bool _canSeePlayer, int _Health, Agency _Agency) : base(_Pedestrian, _canSeePlayer, _Health)
    {
        AssignedAgency = _Agency;
        SetAccuracyAndSightRange();
        if (_Pedestrian.Model.Name.ToLower() == "s_m_y_swat_01")
            IsSwat = true;
    }
    public GTACop(Ped _Pedestrian, bool _canSeePlayer, uint _gameTimeLastSeenPlayer, Vector3 _positionLastSeenPlayer, int _Health, Agency _Agency) : base(_Pedestrian, _canSeePlayer, _Health)
    {
        Pedestrian = _Pedestrian;
        CanSeePlayer = _canSeePlayer;
        GameTimeLastSeenPlayer = _gameTimeLastSeenPlayer;
        PositionLastSeenPlayer = _positionLastSeenPlayer;
        Health = _Health;
        AssignedAgency = _Agency;
        SetAccuracyAndSightRange();
        if (_Pedestrian.Model.Name.ToLower() == "s_m_y_swat_01")
            IsSwat = true;
    }
}

