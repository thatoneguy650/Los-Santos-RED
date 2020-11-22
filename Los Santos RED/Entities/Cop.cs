
using ExtensionsMethods;
using Rage;
using Rage.Native;
using System.Linq;


public class Cop : PedExt
{
    public bool WasModSpawned { get; set; }
    public bool WasRandomSpawnDriver { get; set; }
    public bool IsBikeCop { get; set; }
    public bool IsSetLessLethal { get; set; }
    public bool IsSetUnarmed { get; set; }
    public bool IsSetDeadly { get; set; }
    public uint GameTimeLastWeaponCheck { get; set; }
    public uint GameTimeLastSpoke { get; set; }
    public uint GameTimeLastRadioed { get; set; }
    public GTAWeapon IssuedPistol { get; set; } = new GTAWeapon("weapon_pistol", 60, GTAWeapon.WeaponCategory.Pistol, 1, 453432689, true,true,false,true);
    public GTAWeapon IssuedHeavyWeapon { get; set; }
    public GTAWeapon.WeaponVariation PistolVariation { get; set; }
    public GTAWeapon.WeaponVariation HeavyVariation { get; set; }
    public Agency AssignedAgency { get; set; } = new Agency();
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
        Agency.IssuedWeapon PistolToPick = new Agency.IssuedWeapon("weapon_pistol", true, null);
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

        Agency.IssuedWeapon HeavyToPick = new Agency.IssuedWeapon("weapon_shotgun", true, null);
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
    public void SetUnarmed()
    {
        if (!Pedestrian.Exists() || (IsSetUnarmed && !NeedsWeaponCheck))
            return;
        if (General.MySettings.Police.OverridePoliceAccuracy)
            Pedestrian.Accuracy = General.MySettings.Police.PoliceGeneralAccuracy;

        NativeFunction.CallByName<bool>("SET_PED_SHOOT_RATE", Pedestrian, 0);
        if (!(Pedestrian.Inventory.EquippedWeapon == null))
        {
            NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Pedestrian, 2725352035, true); //Unequip weapon so you don't get shot
            NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", Pedestrian, false);
        }
        NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", Pedestrian, 2, false);//cant do drivebys
        IsSetLessLethal = false;
        IsSetUnarmed = true;
        IsSetDeadly = false;
        GameTimeLastWeaponCheck = Game.GameTime;
    }
    public void SetDeadly()
    {
        if (!Pedestrian.Exists() || (IsSetDeadly && !NeedsWeaponCheck))
            return;
        if (General.MySettings.Police.OverridePoliceAccuracy)
            Pedestrian.Accuracy = General.MySettings.Police.PoliceGeneralAccuracy;
        NativeFunction.CallByName<bool>("SET_PED_SHOOT_RATE", Pedestrian, 30);
        if (!Pedestrian.Inventory.Weapons.Contains(IssuedPistol.Name))
            Pedestrian.Inventory.GiveNewWeapon(IssuedPistol.Name, -1, true);

        if ((Pedestrian.Inventory.EquippedWeapon == null || Pedestrian.Inventory.EquippedWeapon.Hash == WeaponHash.StunGun) && Game.LocalPlayer.WantedLevel >= 0)
            Pedestrian.Inventory.GiveNewWeapon(IssuedPistol.Name, -1, true);

        if (IssuedHeavyWeapon != null)
        {
            NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", Pedestrian, NativeFunction.CallByName<bool>("GET_BEST_PED_WEAPON", Pedestrian, 0), true);
        }

        if (General.MySettings.Police.AllowPoliceWeaponVariations)
            General.ApplyWeaponVariation(Pedestrian, (uint)IssuedPistol.Hash, PistolVariation);
        NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", Pedestrian, true);
        NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", Pedestrian, 2, true);//can do drivebys

        IsSetLessLethal = false;
        IsSetUnarmed = false;
        IsSetDeadly = true;
        GameTimeLastWeaponCheck = Game.GameTime;
    }
    public void SetLessLethal()
    {
        if (!Pedestrian.Exists() || (IsSetLessLethal && !NeedsWeaponCheck))
            return;

        if (General.MySettings.Police.OverridePoliceAccuracy)
            Pedestrian.Accuracy = General.MySettings.Police.PoliceTazerAccuracy;
        NativeFunction.CallByName<bool>("SET_PED_SHOOT_RATE", Pedestrian, 100);
        if (!Pedestrian.Inventory.Weapons.Contains(WeaponHash.StunGun))
        {
            Pedestrian.Inventory.GiveNewWeapon(WeaponHash.StunGun, 100, true);
        }
        else if (Pedestrian.Inventory.EquippedWeapon != WeaponHash.StunGun)
        {
            Pedestrian.Inventory.EquippedWeapon = WeaponHash.StunGun;
        }
        NativeFunction.CallByName<bool>("SET_PED_CAN_SWITCH_WEAPON", Pedestrian, false);
        NativeFunction.CallByName<bool>("SET_PED_COMBAT_ATTRIBUTES", Pedestrian, 2, false);//cant do drivebys
        IsSetLessLethal = true;
        IsSetUnarmed = false;
        IsSetDeadly = false;
        GameTimeLastWeaponCheck = Game.GameTime;
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
            if (WasModSpawned && HasBeenSpawnedFor >= 5000)//15000// && GameTimeSpawned > Police.GameTimeWantedStarted)
                return true;
            else
                return false;
        }
    }
    public int CountNearbyCops
    {
        get
        {
            return PedList.CopPeds.Count(x => Pedestrian.Exists() && x.Pedestrian.Exists() && Pedestrian.Handle != x.Pedestrian.Handle && x.Pedestrian.DistanceTo2D(Pedestrian) >= 3f && x.Pedestrian.DistanceTo2D(Pedestrian) <= 50f);
        }
    }

    public bool ShouldBustPlayer
    {
        get
        {
            if (PlayerState.IsBusted)
            {
                return false;
            }
            else if (!PlayerState.IsBustable)
            {
                return false;
            }
            else if (DistanceToPlayer < 0.1f) //weird cases where they are my same position
            {
                return false;
            }
            else if (PlayerState.HandsAreUp && DistanceToPlayer <= 5f)
            {
                return true;
            }
            if (PlayerState.IsInVehicle)
            {
                if (PlayerState.IsStationary && DistanceToPlayer <= 1f)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if ((Game.LocalPlayer.Character.IsStunned || Game.LocalPlayer.Character.IsRagdoll) && DistanceToPlayer <= 3f)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
    public Cop(Ped pedestrian, int health, Agency agency) : base(pedestrian, health)
    {
        IsCop = true;
        Health = health;
        AssignedAgency = agency;
        SetAccuracyAndSightRange();
    }
}

