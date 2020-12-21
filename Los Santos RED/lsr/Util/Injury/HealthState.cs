using LosSantosRED.lsr;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class HealthState
{
    private uint GameTimeLastBled;
    private bool HurtByPed;
    private bool HurtByVehicle;
    private int CurrentHealth;
    private int CurrentArmor;
    private uint GameTimeLastCheckedDamage;
    private bool NeedDamageCheck
    {
        get
        {
            if (Game.GameTime - GameTimeLastCheckedDamage >= 300)
                return true;
            else
                return false;
        }
    }
    private bool ShouldBleed
    {
        get
        {
            if (GameTimeLastBled == 0)
                return true;
            else if (Game.GameTime - GameTimeLastBled >= 2500)
                return true;
            else
                return false;
        }
    }
    private PedExt MyPed;
    private int Health;
    private int Armor;
    private bool IsBleeding;
    public bool HasLoggedDeath { get; private set; }
    public void Update()
    {
        if (NeedDamageCheck && MyPed.Pedestrian.Exists() && !HasLoggedDeath)
        {
            GameTimeLastCheckedDamage = Game.GameTime;
            CurrentHealth = MyPed.Pedestrian.Health;
            CurrentArmor = MyPed.Pedestrian.Armor;

            if(CurrentHealth == 0)
            {
                HasLoggedDeath = true;//need to check once after the ped died to see who killed them, butr checking more is wasteful
            }
            if (CurrentHealth < Health || CurrentArmor < Armor)
            {
                FlagDamage();
                ModifyDamage();
                Health = CurrentHealth;
                Armor = CurrentArmor;
            }
            if (IsBleeding && MyPed.Pedestrian.IsAlive && ShouldBleed)
            {
                MyPed.Pedestrian.Health -= 2;
                CurrentHealth = MyPed.Pedestrian.Health;
                CurrentArmor = MyPed.Pedestrian.Armor;
                GameTimeLastBled = Game.GameTime;
                Mod.Debug.WriteToLog("PedWoundSystem", string.Format("Bleeding {0} {1}", MyPed.Pedestrian.Handle, CurrentHealth));
            }
        }
    }
    private void FlagDamage()
    {
        if (MyPed.Pedestrian.IsDead && MyPed.KilledBy(Game.LocalPlayer.Character))
        {
            Mod.Player.Killed(MyPed);
        }
        else if (MyPed.Pedestrian.IsAlive && MyPed.HurtBy(Game.LocalPlayer.Character))
        {
            Mod.Player.Injured(MyPed);
        }
    }
    private void ModifyDamage()
    {
        HurtByPed = NativeFunction.CallByName<bool>("HAS_ENTITY_BEEN_DAMAGED_BY_ANY_PED", MyPed.Pedestrian);
        HurtByVehicle = NativeFunction.CallByName<bool>("HAS_ENTITY_BEEN_DAMAGED_BY_ANY_VEHICLE", MyPed.Pedestrian);
        if (HurtByPed || HurtByVehicle)
        {
            int TotalDamage = Health - CurrentHealth + Armor - CurrentArmor;
            int HealthDamage = Health - CurrentHealth;
            int ArmorDamage = Armor - CurrentArmor;
            BodyLocation DamagedLocation = GetDamageLocation(MyPed.Pedestrian);
            WeaponInformation DamagingWeapon = Mod.DataMart.Weapons.GetDamagingWeapon(MyPed.Pedestrian);

            bool CanBeFatal = false;
            if (DamagedLocation == BodyLocation.Head || DamagedLocation == BodyLocation.Neck || DamagedLocation == BodyLocation.UpperTorso)
                CanBeFatal = true;

            bool ArmorWillProtect = false;
            if (DamagedLocation == BodyLocation.UpperTorso)
                ArmorWillProtect = true;

            InjuryType HealthInjury = InjuryType.Vanilla; //RandomType(CanBeFatal);
            InjuryType ArmorInjury = InjuryType.Vanilla; //RandomType(false);

            if (MyPed.Health == 0)//already dead, we are intercepting
            {
                HealthInjury = InjuryType.Fatal;
            }
            else if (DamagingWeapon.Category != WeaponCategory.Vehicle && DamagingWeapon.Category != WeaponCategory.Melee && DamagingWeapon.Category != WeaponCategory.Unknown)
            {
                HealthInjury = RandomType(CanBeFatal);
                ArmorInjury = RandomType(false);
            }

            //if(HealthInjury == InjuryType.Critical || HealthInjury == InjuryType.Fatal)
            //{
            //    FlagAsBleeding();
            //}



            float HealthDamageModifier = GetDamageModifier(HealthInjury, false);
            float ArmorDamageModifier = GetDamageModifier(ArmorInjury, true);

            int NewHealthDamage = Convert.ToInt32(HealthDamage * HealthDamageModifier);

            if (!ArmorWillProtect)
                NewHealthDamage = Convert.ToInt32((HealthDamage + ArmorDamage) * HealthDamageModifier);

            int NewArmorDamage = 0;
            if (ArmorWillProtect)
                NewArmorDamage = Convert.ToInt32(ArmorDamage * ArmorDamageModifier);




            if (Health - NewHealthDamage > 0)
                MyPed.Pedestrian.Health = Health - NewHealthDamage;
            else
                MyPed.Pedestrian.Health = 0;

            if (Armor - NewArmorDamage > 0)
                MyPed.Pedestrian.Armor = Armor - NewArmorDamage;
            else
                MyPed.Pedestrian.Armor = 0;

            string DisplayString = "";
            if (MyPed.IsCop)
            {
                DisplayString = string.Format("  Cop: {0}, {1}-{2}-{3} Damage {4}/{5} Health {6}/{7}",
                 MyPed.Pedestrian.Handle, HealthInjury, DamagedLocation, DamagingWeapon.ModelName, NewHealthDamage, NewArmorDamage, MyPed.Pedestrian.Health, MyPed.Pedestrian.Armor);
            }
            else
            {
                DisplayString = string.Format("  Ped: {0}, {1}-{2}-{3} Damage {4}/{5} Health {6}/{7}",
                  MyPed.Pedestrian.Handle, HealthInjury, DamagedLocation, DamagingWeapon.ModelName, NewHealthDamage, NewArmorDamage, MyPed.Pedestrian.Health, MyPed.Pedestrian.Armor);
            }
            Mod.Debug.WriteToLog("PedWoundSystem", DisplayString);
        }
        if (Health != CurrentHealth)
        {
            SetRagdoll(CurrentHealth);
        }

    }
    private void SetRagdoll(int NewHealth)
    {
        if (Health - NewHealth >= 65)
        {
            NativeFunction.CallByName<bool>("SET_PED_TO_RAGDOLL", MyPed.Pedestrian, 3000, 3000, 0, false, false, false);
            //IsBleeding = true;
            //Mod.Debugging.WriteToLog("PlayerHealthChanged", string.Format("Critical Hit, Ragdoll"));
        }
        else if (Health - NewHealth >= 35 && RandomItems.RandomPercent(60))
        {
            NativeFunction.CallByName<bool>("SET_PED_TO_RAGDOLL", MyPed.Pedestrian, 1500, 1500, 1, false, false, false);
            //IsBleeding = true;
            //Mod.Debugging.WriteToLog("PlayerHealthChanged", string.Format("Critical Hit, Ragdoll"));
        }
        else if (Health - NewHealth >= 15 && RandomItems.RandomPercent(30))
        {
            NativeFunction.CallByName<bool>("SET_PED_TO_RAGDOLL", MyPed.Pedestrian, 1500, 1500, 1, false, false, false);
            //IsBleeding = true;
            //Mod.Debugging.WriteToLog("PlayerHealthChanged", string.Format("Critical Hit, Ragdoll"));
        }
        else if (Health - NewHealth >= 10)
        {
            //IsBleeding = true;
            //Mod.Debugging.WriteToLog("PlayerHealthChanged", string.Format("Normal Hit, Bleeding"));
        }
    }
    private float GetDamageModifier(InjuryType injury, bool IsArmor)
    {
        if (IsArmor)
        {
            if (injury == InjuryType.Normal)
                return 1.0f;
            else if (injury == InjuryType.Graze)
                return 0.25f;
            else if (injury == InjuryType.Critical)
                return 2.0f;
            else if (injury == InjuryType.Vanilla)
                return 1.0f;
            else
                return 1.0f;
        }
        else
        {
            if (injury == InjuryType.Fatal)
                return 10.0f;
            else if (injury == InjuryType.Normal)
                return 2.0f;//5.0f;//3.0f;//1.0f;
            else if (injury == InjuryType.Graze)
                return 0.75f;//0.25f;
            else if (injury == InjuryType.Critical)
                return 3.0f;//8.0f;// 6.0f;//2.0f;
            else if (injury == InjuryType.Vanilla)
                return 1.0f;// 6.0f;//2.0f;
            else
                return 1.0f;
        }
    }
    private BodyLocation GetDamageLocation(Ped Pedestrian)
    {
        int outBone = 0;
        unsafe
        {
            NativeFunction.CallByName<bool>("GET_PED_LAST_DAMAGE_BONE", Pedestrian, &outBone);
        }

        if (outBone != 0)
        {
            NativeFunction.CallByName<bool>("CLEAR_PED_LAST_DAMAGE_BONE", Pedestrian);
            Bone DamagedOne = Mod.DataMart.Bones.GetBone(outBone);//    .FirstOrDefault(x => x.Tag == DamagedBoneId);
            if (DamagedOne != null)
            {
                return DamagedOne.Location;
            }
            else
            {
                return BodyLocation.Unknown;
            }
        }
        else
        {
            return BodyLocation.Unknown;
        }
    }
    private InjuryType RandomType(bool CanBeFatal)
    {
        int RandomNumber = RandomItems.MyRand.Next(1, 101);
        if (RandomNumber <= 60)
            return InjuryType.Normal;
        else if (RandomNumber <= 70)
            return InjuryType.Graze;
        else if (RandomNumber <= 92)
            return InjuryType.Critical;
        else if (RandomNumber <= 100 && CanBeFatal)
            return InjuryType.Fatal;
        else
            return InjuryType.Normal;
    }
    public HealthState()
    {

    }
    public HealthState(PedExt _MyPed)
    {
        MyPed = _MyPed;
        Health = _MyPed.Pedestrian.Health;
        Armor = _MyPed.Pedestrian.Armor;
        CurrentArmor = Armor;
        CurrentHealth = Health;
    }
}