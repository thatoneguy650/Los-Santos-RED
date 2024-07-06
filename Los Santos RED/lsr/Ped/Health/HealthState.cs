using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

public class HealthState
{
    private int Armor;
    private int Total;
    private int CurrentArmor;
    private int CurrentTotal;
    private int CurrentHealth;
    private uint GameTimeLastBled;
    private uint GameTimeLastCheckedDamage;
    private uint GameTimeLastModifiedPlayerDamage;
    private int Health;
    private bool HurtByPed;
    private bool HurtByVehicle;
    private ISettingsProvideable Settings;
    private uint GameTimeLastSetRagDoll;
    private bool IsPlayer;
    private bool HasSetup = false;
    public HealthState()
    {

    }
    public HealthState(PedExt _MyPed, ISettingsProvideable settings, bool isPlayer)
    {
        MyPed = _MyPed;
        Settings = settings;
        IsPlayer = isPlayer;
    }
    public PedExt MyPed { get; set; }
    public bool HasLoggedDeath { get; private set; }
    private bool NeedDamageCheck
    {
        get
        {
            if(!HasLoggedDeath && MyPed.Pedestrian.Exists() && MyPed.Pedestrian.IsDead)
            {
                return true;
            }
            if(MyPed.DistanceToPlayer >= 400)
            {
                return false;
            }
            if (Game.GameTime - GameTimeLastCheckedDamage >= 300)
            {
                return true;
            }
            else
            {
                return false;
            }
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
    public bool WasShot { get; private set; }
    public bool WasMeleeAttacked { get; private set; }
    private uint GameTimeLastModifiedDamage;
    private uint GameTimeLastYelledInPain;
    public bool WasHitByVehicle { get; private set; }
    public void Setup()
    {
        if (!MyPed.Pedestrian.Exists())
        {
            return;
        }
        Health = MyPed.Pedestrian.Health;
        Armor = MyPed.Pedestrian.Armor;
        CurrentArmor = Armor;
        CurrentHealth = Health;
        HasSetup = true;
    }
    public void Update(IPoliceRespondable CurrentPlayer)
    {
        if(!HasSetup)
        {
            Setup();
        }
        if (NeedDamageCheck && MyPed.Pedestrian.Exists() && !HasLoggedDeath)
        {
            GameTimeLastCheckedDamage = Game.GameTime;
            CurrentHealth = MyPed.Pedestrian.Health;
            CurrentArmor = MyPed.Pedestrian.Armor;
            if(MyPed.Pedestrian.IsDead)
            {
                HasLoggedDeath = true;//need to check once after the ped died to see who killed them, butr checking more is wasteful
                MyPed.OnDeath(CurrentPlayer);
                FlagDamage(CurrentPlayer);
                return;
            }
            if (CurrentHealth < Health || CurrentArmor < Armor)
            {
                int prevHealth = Health;
                if (MyPed.Pedestrian.Exists() && MyPed.HasExistedFor >= 2000)//4000)//10000)
                {
                    GameFiber.Yield();
                    //EntryPoint.WriteToConsole($"HEALTHSTATE DAMAGE DETECTED {MyPed.Pedestrian.Handle} HasExistedFor {MyPed.HasExistedFor} CurrentHealth {CurrentHealth} CurrentArmor {CurrentArmor} Existing Health {Health} Existing Armor {Armor}", 5);
                    FlagDamage(CurrentPlayer);
                    if (Settings.SettingsManager.DamageSettings.ModifyAIDamage)
                    {
                        ModifyDamage();
                    }
                    Health = CurrentHealth;
                    Armor = CurrentArmor;
                }
                else
                {
                    Health = CurrentHealth;
                    Armor = CurrentArmor;
                }
                if(Settings.SettingsManager.DamageSettings.AllowAIUnconsciousOnDamage && Health > Settings.SettingsManager.DamageSettings.AIUnconsciousOnDamageAliveHealth && !MyPed.IsUnconscious && (Health <= Settings.SettingsManager.DamageSettings.AIUnconsciousOnDamageMinimumHealth || Health - prevHealth >= Settings.SettingsManager.DamageSettings.AIUnconsciousOnDamageMinimumHealthChange) && RandomItems.RandomPercent(Settings.SettingsManager.DamageSettings.AIUnconsciousOnDamagePercentage))// && RandomItems.RandomPercent(40))
                {
                    SetUnconscious(CurrentPlayer);
                }
                else if (Settings.SettingsManager.DamageSettings.AllowAIUnconsciousOnStun && MyPed.Pedestrian.Exists() && MyPed.Pedestrian.IsStunned 
                    && //!MyPed.PedViolations.IsCurrentlyViolatingAnyCrimes && 
                    RandomItems.RandomPercent(Settings.SettingsManager.DamageSettings.AIUnconsciousOnStunPercentage))
                {
                    SetUnconscious(CurrentPlayer);
                }
                if(Settings.SettingsManager.DamageSettings.AllowAIPainYells && (HurtByPed || HurtByVehicle) && !MyPed.IsDead && !MyPed.IsUnconscious && Health - prevHealth >= Settings.SettingsManager.DamageSettings.AIPainYellsDamageNeeded && MyPed.HasExistedFor >= 4000 && Game.GameTime - GameTimeLastYelledInPain >= 5000)
                {
                    MyPed.YellInPain(true);
                    MyPed.GameTimeLastInjured = Game.GameTime;
                    GameTimeLastYelledInPain = Game.GameTime;
                    //EntryPoint.WriteToConsole($"HEALTHSTATE DAMAGE DETECTED {MyPed.Pedestrian.Handle} YELLING! MyPed.GameTimeLastInjured {MyPed.GameTimeLastInjured}", 5);
                }
            }
        }
        if(!MyPed.IsUnconscious && !MyPed.IsDead && MyPed.IsInWrithe && Settings.SettingsManager.DamageSettings.AllowAIPainYells)
        {
            MyPed.YellInPain(false);
        }
        else if (MyPed.Pedestrian.Exists() && MyPed.IsUnconscious && (Game.GameTime - GameTimeLastSetRagDoll <= 1000 || !MyPed.Pedestrian.IsRagdoll))
        {
            MyPed.Pedestrian.BlockPermanentEvents = true;
            MyPed.Pedestrian.KeepTasks = true;
            MyPed.Pedestrian.IsRagdoll = true;
            //NativeFunction.Natives.SET_PED_TO_RAGDOLL(MyPed.Pedestrian, -1, -1, 0, true, true, false);
            MyPed.Pedestrian.BlockPermanentEvents = true;
            MyPed.Pedestrian.KeepTasks = true;
            GameTimeLastSetRagDoll = Game.GameTime;
        }
    }
    public void UpdatePlayer(IPoliceRespondable CurrentPlayer)
    {
        if (Game.GameTime - GameTimeLastCheckedDamage >= 300 && MyPed.Pedestrian.Exists())
        {
            GameTimeLastCheckedDamage = Game.GameTime;
            CurrentHealth = MyPed.Pedestrian.Health;
            CurrentArmor = MyPed.Pedestrian.Armor;
            CurrentTotal = CurrentHealth + CurrentArmor;
            if (!HasLoggedDeath && MyPed.Pedestrian.IsDead)
            {
                CurrentPlayer.GetKillingPed();
                HasLoggedDeath = true;//need to check once after the ped died to see who killed them, but checking more is wasteful
                return;
            }
            int prevHealth = Health;
            if (CurrentHealth < Health || CurrentArmor < Armor)
            {
                GameFiber.Yield(); 
                //EntryPoint.WriteToConsole($"HEALTHSTATE DAMAGE PLAYER DETECTED {MyPed.Pedestrian.Handle} CurrentHealth {CurrentHealth} CurrentArmor {CurrentArmor} Existing Health {Health} Existing Armor {Armor}");
                if (Settings.SettingsManager.DamageSettings.ModifyPlayerDamage && Game.GameTime - GameTimeLastModifiedPlayerDamage >= 500)
                {
                    if(CurrentTotal < Total)
                    {
                        ModifyDamage();
                    }
                    else
                    {
                        EntryPoint.WriteToConsole($"HEALTHSTATE DAMAGE PLAYER DETECTED {MyPed.Pedestrian.Handle} CurrentHealth {CurrentHealth} CurrentArmor {CurrentArmor} Existing Health {Health} Existing Armor {Armor} CurrentTotal {CurrentTotal} Total{Total}");
                    }
                }
                Health = CurrentHealth;
                Armor = CurrentArmor;
                Total = CurrentHealth + CurrentArmor;


                CheckPainYells(CurrentPlayer, prevHealth);

            }

        }
    }
    private void CheckPainYells(IPoliceRespondable CurrentPlayer, int prevHealth)
    {
        if (Settings.SettingsManager.DamageSettings.AllowPlayerPainYells && Health - prevHealth >= Settings.SettingsManager.DamageSettings.PlayerPainYellsDamageNeeded && MyPed.HasExistedFor >= 4000)
        {
            CurrentPlayer.ActivityManager.YellInPain();
            MyPed.GameTimeLastInjured = Game.GameTime;
            //EntryPoint.WriteToConsole($"HEALTHSTATE PLAYER DAMAGE DETECTED {MyPed.Pedestrian.Handle} YELLING! MyPed.GameTimeLastInjured {MyPed.GameTimeLastInjured}");
        }
    }

    public void SimpleRefresh(IPoliceRespondable CurrentPlayer)
    {
        if(MyPed == null || !MyPed.Pedestrian.Exists())
        {
            return;
        }

        int prevHealth = Health;


        CurrentHealth = MyPed.Pedestrian.Health;
        CurrentArmor = MyPed.Pedestrian.Armor;
        CurrentTotal = CurrentHealth + CurrentArmor;





        Health = CurrentHealth;
        Armor = CurrentArmor;
        Total = CurrentHealth + CurrentArmor;

        CheckPainYells(CurrentPlayer, prevHealth);

    }

    public void Reset()
    {
        HasLoggedDeath = false;
        GameTimeLastCheckedDamage = 0;
        if(MyPed.Pedestrian.Exists())
        {
            Health = MyPed.Pedestrian.Health;
            Armor = MyPed.Pedestrian.Armor;
            CurrentArmor = Armor;
            CurrentHealth = Health;
        }
    }
    public void ResurrectPed()
    {
        if (MyPed == null || !MyPed.Pedestrian.Exists())
        {
            return;
        }
        //MyPed.Pedestrian.Health = MyPed.Pedestrian.MaxHealth;
        //MyPed.Pedestrian.Resurrect();
        //MyPed.Pedestrian.Health = MyPed.Pedestrian.MaxHealth;
        NativeFunction.Natives.RESURRECT_PED(MyPed.Pedestrian);
       // MyPed.Pedestrian.Health = MyPed.Pedestrian.MaxHealth;
        NativeFunction.Natives.REVIVE_INJURED_PED(MyPed.Pedestrian);
       // MyPed.Pedestrian.Health = MyPed.Pedestrian.MaxHealth;
        NativeFunction.Natives.CLEAR_PED_TASKS_IMMEDIATELY(MyPed.Pedestrian);
    }


    public void SetUnconscious(IPoliceRespondable CurrentPlayer)
    {
        if (MyPed.Pedestrian.Exists())
        {
            MyPed.CanBeAmbientTasked = false;
            MyPed.CanBeTasked = false;
            MyPed.YellInPain(true);
            MyPed.IsUnconscious = true;
            MyPed.OnUnconscious(CurrentPlayer);
            NativeFunction.Natives.SET_PED_TO_RAGDOLL(MyPed.Pedestrian, -1, -1, 0, true, true, false);
            //EntryPoint.WriteToConsole($"HEALTHSTATE SetUnconscious {MyPed.Pedestrian.Handle} GameTimeLastInjured {MyPed.GameTimeLastInjured} Health {Health}");
        }
    }


    private void FlagDamage(IPoliceRespondable CurrentPlayer)
    {
        if(CurrentPlayer == null || !MyPed.Pedestrian.Exists())//only flag the player we want to have the damage
        {
            return;
        }
        if (MyPed.Pedestrian.IsDead)
        {
            MyPed.LogSourceOfDeath();
            if (MyPed.CheckKilledBy(CurrentPlayer.Character))
            {
                MyPed.WasKilledByPlayer = true;
                MyPed.HasBeenHurtByPlayer = true;
                CurrentPlayer.Violations.DamageViolations.AddKilled(MyPed, WasShot, WasMeleeAttacked, WasHitByVehicle);
            }
        }
        else
        {
            if (!Settings.SettingsManager.ViolationSettings.TreatAsCop && !MyPed.HasBeenHurtByPlayer && MyPed.CheckHurtBy(CurrentPlayer.Character,false))
            {
                if (Health - CurrentHealth + Armor - CurrentArmor > 5)
                {
                    MyPed.HasBeenHurtByPlayer = true;
                    CurrentPlayer.Violations.DamageViolations.AddInjured(MyPed, WasShot, WasMeleeAttacked, WasHitByVehicle);
                }
                EntryPoint.WriteToConsole($"YOU DAMAGED {MyPed.Handle} H1:{Health} H2:{CurrentHealth} HX:{Health - CurrentHealth} A1:{Armor} A2:{CurrentArmor} AX:{Armor - CurrentArmor}");
            }
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
            if (Settings.SettingsManager.DamageSettings.ClearDamage)
            {
                NativeFunction.CallByName<bool>("CLEAR_PED_LAST_DAMAGE_BONE", Pedestrian);
            }
            Bone DamagedOne = GetBone(outBone);//    .FirstOrDefault(x => x.Tag == DamagedBoneId);
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
    private float GetDamageModifier(InjuryType injury, bool IsArmor)
    {
        if (IsPlayer)
        {
            if (IsArmor)
            {
                if (injury == InjuryType.Normal)
                    return Settings.SettingsManager.DamageSettings.Armor_NormalDamageModifierPlayer;
                else if (injury == InjuryType.Graze)
                    return Settings.SettingsManager.DamageSettings.Armor_GrazeDamageModifierPlayer;
                else if (injury == InjuryType.Critical)
                    return Settings.SettingsManager.DamageSettings.Armor_CriticalDamageModifierPlayer;
                else if (injury == InjuryType.Vanilla)
                    return 1.0f;
                else
                    return 1.0f;
            }
            else
            {
                if (injury == InjuryType.Fatal)
                    return Settings.SettingsManager.DamageSettings.Health_FatalDamageModifierPlayer;
                else if (injury == InjuryType.Normal)
                    return Settings.SettingsManager.DamageSettings.Health_NormalDamageModifierPlayer;
                else if (injury == InjuryType.Graze)
                    return Settings.SettingsManager.DamageSettings.Health_GrazeDamageModifierPlayer;
                else if (injury == InjuryType.Critical)
                    return Settings.SettingsManager.DamageSettings.Health_CriticalDamageModifierPlayer;
                else if (injury == InjuryType.Vanilla)
                    return 1.0f;
                else
                    return 1.0f;
            }
        }
        else
        {
            if (IsArmor)
            {
                if (injury == InjuryType.Normal)
                    return Settings.SettingsManager.DamageSettings.Armor_NormalDamageModifierAI;
                else if (injury == InjuryType.Graze)
                    return Settings.SettingsManager.DamageSettings.Armor_GrazeDamageModifierAI;
                else if (injury == InjuryType.Critical)
                    return Settings.SettingsManager.DamageSettings.Armor_CriticalDamageModifierAI;
                else if (injury == InjuryType.Vanilla)
                    return 1.0f;
                else
                    return 1.0f;
            }
            else
            {
                if (injury == InjuryType.Fatal)
                    return Settings.SettingsManager.DamageSettings.Health_FatalDamageModifierAI;
                else if (injury == InjuryType.Normal)
                    return Settings.SettingsManager.DamageSettings.Health_NormalDamageModifierAI;
                else if (injury == InjuryType.Graze)
                    return Settings.SettingsManager.DamageSettings.Health_GrazeDamageModifierAI;
                else if (injury == InjuryType.Critical)
                    return Settings.SettingsManager.DamageSettings.Health_CriticalDamageModifierAI;
                else if (injury == InjuryType.Vanilla)
                    return 1.0f;
                else
                    return 1.0f;
            }
        }
    }
    private void ModifyDamage()
    {
        if (!Settings.SettingsManager.DamageSettings.ModifyDamage || !MyPed.Pedestrian.Exists() || Game.GameTime - GameTimeLastModifiedDamage <= 100)
        {
            return;
        }
        GameTimeLastModifiedDamage = Game.GameTime;
        WasHitByVehicle = false;
        WasShot = false;
        WasMeleeAttacked = false;
        HurtByPed = NativeFunction.CallByName<bool>("HAS_ENTITY_BEEN_DAMAGED_BY_ANY_PED", MyPed.Pedestrian);
        HurtByVehicle = NativeFunction.CallByName<bool>("HAS_ENTITY_BEEN_DAMAGED_BY_ANY_VEHICLE", MyPed.Pedestrian);
        if (HurtByPed || HurtByVehicle)
        {
            int HealthDamage = Health - CurrentHealth;
            int ArmorDamage = Armor - CurrentArmor;


            if(HealthDamage == 0 && ArmorDamage == 0)
            {
                return;
            }

            BodyLocation DamagedLocation = GetDamageLocation(MyPed.Pedestrian);
            WeaponCategory category = WeaponCategory.Unknown;
            if (NativeFunction.CallByName<bool>("HAS_PED_BEEN_DAMAGED_BY_WEAPON", MyPed.Pedestrian, 0, 1))
            {
                category = WeaponCategory.Melee;
                WasMeleeAttacked = true;
            }
            else if (NativeFunction.CallByName<bool>("HAS_PED_BEEN_DAMAGED_BY_WEAPON", MyPed.Pedestrian, 0, 2))
            {
                if(MyPed.Pedestrian.IsStunned)
                {
                    category = WeaponCategory.Melee;
                }
                else
                {
                    category = WeaponCategory.Pistol;
                    WasShot = true;
                }
            }
            else if (NativeFunction.CallByName<bool>("HAS_ENTITY_BEEN_DAMAGED_BY_ANY_VEHICLE", MyPed.Pedestrian))
            {
                category = WeaponCategory.Vehicle;
                WasHitByVehicle = true;
            }
   
            bool CanBeFatal = false;
            if (DamagedLocation == BodyLocation.Head || DamagedLocation == BodyLocation.Neck || DamagedLocation == BodyLocation.UpperTorso)
            {
                CanBeFatal = true;
            }

            bool ArmorWillProtect = false;
            if (MyPed.HasFullBodyArmor || DamagedLocation == BodyLocation.UpperTorso)
            {
                ArmorWillProtect = true;
            }
            InjuryType HealthInjury = InjuryType.Vanilla; //RandomType(CanBeFatal);
            InjuryType ArmorInjury = InjuryType.Vanilla; //RandomType(false);
            if (MyPed.Health == 0)//already dead, we are intercepting
            {
                HealthInjury = InjuryType.Fatal;
            }
            else if (category != WeaponCategory.Vehicle && category != WeaponCategory.Melee && category != WeaponCategory.Unknown)
            {
                HealthInjury = RandomType(CanBeFatal);
                ArmorInjury = RandomType(false);
            }
            float HealthDamageModifier = GetDamageModifier(HealthInjury, false);
            float ArmorDamageModifier = GetDamageModifier(ArmorInjury, true);

            int NewHealthDamage = Convert.ToInt32(HealthDamage * HealthDamageModifier);
            int NewArmorDamage = 0;
            if (ArmorWillProtect)
            {
                NewArmorDamage = Convert.ToInt32(ArmorDamage * ArmorDamageModifier);
            }
            else
            {
                NewHealthDamage = Convert.ToInt32((HealthDamage + ArmorDamage) * HealthDamageModifier);
            }

            Health = (Health - NewHealthDamage).Clamp(0, 99999);
            MyPed.Pedestrian.Health = Health;
            Armor = (Armor - NewArmorDamage).Clamp(0, 99999);
            MyPed.Pedestrian.Armor = Armor;
            //EntryPoint.WriteToConsole($"Player Damage Modify: Health{Health} NewHealthDamage{NewHealthDamage} Armor{Armor} NewArmorDamage{NewArmorDamage} CurrentHealth{CurrentHealth} CurrentArmor{CurrentArmor}");


        }
        if (Health != CurrentHealth && MyPed.Pedestrian.Health > 0)
        {
            SetRagdoll(CurrentHealth);
        }

    }
    private InjuryType RandomType(bool CanBeFatal)
    {
        var ToPickFrom = new List<(float, InjuryType)> { };
        if(IsPlayer)
        {
            ToPickFrom = new List<(float, InjuryType)>
            {
                (Settings.SettingsManager.DamageSettings.NormalDamagePercentPlayer, InjuryType.Normal),
                (Settings.SettingsManager.DamageSettings.GrazeDamagePercentPlayer, InjuryType.Graze),
                (Settings.SettingsManager.DamageSettings.CriticalDamagePercentPlayer, InjuryType.Critical),
                (Settings.SettingsManager.DamageSettings.FatalDamagePercentPlayer,InjuryType.Fatal)
            };
        }
        else
        {
            ToPickFrom = new List<(float, InjuryType)>
            {
                (Settings.SettingsManager.DamageSettings.NormalDamagePercentAI, InjuryType.Normal),
                (Settings.SettingsManager.DamageSettings.GrazeDamagePercentAI, InjuryType.Graze),
                (Settings.SettingsManager.DamageSettings.CriticalDamagePercentAI, InjuryType.Critical),
                (Settings.SettingsManager.DamageSettings.FatalDamagePercentAI,InjuryType.Fatal)
            };
        }




        float Total = ToPickFrom.Sum(x => x.Item1);
        float RandomPick = RandomItems.GetRandomNumber(0, Total);//--RandomItems.MyRand.Next(0, Total);
        foreach ((float, InjuryType) percentage in ToPickFrom)
        {
            float SpawnChance = percentage.Item1;
            if (RandomPick < SpawnChance)
            {     
                if (percentage.Item2 != InjuryType.Fatal)
                {
                    return percentage.Item2;
                }
                else if (percentage.Item2 == InjuryType.Fatal && CanBeFatal)
                {
                    return percentage.Item2;
                }
            }
            RandomPick -= SpawnChance;
        }
        return InjuryType.Normal;
    }
    private void SetRagdoll(int NewHealth)
    {
        if(!Settings.SettingsManager.DamageSettings.AllowRagdoll)
        {
            return;
        }
        if (Health - NewHealth >= 65)
        {
            //EntryPoint.WriteToConsole($"HEALTHSTATE: RAGDOLL 1 {MyPed.Pedestrian.Handle} Health - NewHealth = {Health - NewHealth} NewHealth {NewHealth} Health {Health} MyPed.Pedestrian.Health {MyPed.Pedestrian.Health}");
            //NativeFunction.CallByName<bool>("SET_PED_TO_RAGDOLL", MyPed.Pedestrian, 3000, 3000, 0, false, false, false);
        }
        //else if (Health - NewHealth >= 35 && RandomItems.RandomPercent(60))
        //{
        //    EntryPoint.WriteToConsole($"HEALTHSTATE: RAGDOLL 2 {MyPed.Pedestrian.Handle} Health - NewHealth = {Health - NewHealth} NewHealth {NewHealth} Health {Health} MyPed.Pedestrian.Health {MyPed.Pedestrian.Health}", 5);
        //   // NativeFunction.CallByName<bool>("SET_PED_TO_RAGDOLL", MyPed.Pedestrian, 1500, 1500, 1, false, false, false);
        //}
        //else if (Health - NewHealth >= 15 && RandomItems.RandomPercent(30))
        //{
        //    EntryPoint.WriteToConsole($"HEALTHSTATE: RAGDOLL 3 {MyPed.Pedestrian.Handle} Health - NewHealth = {Health - NewHealth} NewHealth {NewHealth} Health {Health} MyPed.Pedestrian.Health {MyPed.Pedestrian.Health}", 5);
        //    NativeFunction.CallByName<bool>("SET_PED_TO_RAGDOLL", MyPed.Pedestrian, 1500, 1500, 1, false, false, false);
        //}
        //else if (Health - NewHealth >= 10)
        //{
        //    EntryPoint.WriteToConsole($"HEALTHSTATE: RAGDOLL 4 {MyPed.Pedestrian.Handle} Health - NewHealth = {Health - NewHealth} NewHealth {NewHealth} Health {Health} MyPed.Pedestrian.Health {MyPed.Pedestrian.Health}", 5);
        //}
    }
    private Bone GetBone(int ID)
    {
        List<Bone>  PedBones = new List<Bone>
        {
            new Bone("SKEL_ROOT", 4215, 0, BodyLocation.LowerTorso),
            new Bone("SKEL_Pelvis", 4103, 11816, BodyLocation.LowerTorso),
            new Bone("SKEL_L_Thigh", 4103, 58271, BodyLocation.LeftLeg),
            new Bone("SKEL_L_Calf", 4103, 63931, BodyLocation.LeftLeg),
            new Bone("SKEL_L_Foot", 4103, 14201, BodyLocation.LeftLeg),
            new Bone("SKEL_L_Toe0", 7, 2108, BodyLocation.LeftLeg),
            new Bone("IK_L_Foot", 119, 65245, BodyLocation.LeftLeg),
            new Bone("PH_L_Foot", 119, 57717, BodyLocation.LeftLeg),
            new Bone("MH_L_Knee", 119, 46078, BodyLocation.LeftLeg),
            new Bone("SKEL_R_Thigh", 4103, 51826, BodyLocation.RightLeg),
            new Bone("SKEL_R_Calf", 4103, 36864, BodyLocation.RightLeg),
            new Bone("SKEL_R_Foot", 4103, 52301, BodyLocation.RightLeg),
            new Bone("SKEL_R_Toe0", 7, 20781, BodyLocation.RightLeg),
            new Bone("IK_R_Foot", 119, 35502, BodyLocation.RightLeg),
            new Bone("PH_R_Foot", 119, 24806, BodyLocation.RightLeg),
            new Bone("MH_R_Knee", 119, 16335, BodyLocation.RightLeg),
            new Bone("RB_L_ThighRoll", 7, 23639, BodyLocation.LeftLeg),
            new Bone("RB_R_ThighRoll", 7, 6442, BodyLocation.RightLeg),
            new Bone("SKEL_Spine_Root", 4103, 57597, BodyLocation.LowerTorso),
            new Bone("SKEL_Spine0", 4103, 23553, BodyLocation.LowerTorso),
            new Bone("SKEL_Spine1", 4103, 24816, BodyLocation.LowerTorso),
            new Bone("SKEL_Spine2", 4103, 24817, BodyLocation.UpperTorso),
            new Bone("SKEL_Spine3", 4103, 24818, BodyLocation.UpperTorso),
            new Bone("SKEL_L_Clavicle", 4103, 64729, BodyLocation.LeftArm),
            new Bone("SKEL_L_UpperArm", 4103, 45509, BodyLocation.LeftArm),
            new Bone("SKEL_L_Forearm", 4215, 61163, BodyLocation.LeftArm),
            new Bone("SKEL_L_Hand", 4215, 18905, BodyLocation.LeftArm),
            new Bone("SKEL_L_Finger00", 4103, 26610, BodyLocation.LeftArm),
            new Bone("SKEL_L_Finger01", 4103, 4089, BodyLocation.LeftArm),
            new Bone("SKEL_L_Finger02", 7, 4090, BodyLocation.LeftArm),
            new Bone("SKEL_L_Finger10", 4103, 26611, BodyLocation.LeftArm),
            new Bone("SKEL_L_Finger11", 4103, 4169, BodyLocation.LeftArm),
            new Bone("SKEL_L_Finger12", 7, 4170, BodyLocation.LeftArm),
            new Bone("SKEL_L_Finger20", 4103, 26612, BodyLocation.LeftArm),
            new Bone("SKEL_L_Finger21", 4103, 4185, BodyLocation.LeftArm),
            new Bone("SKEL_L_Finger22", 7, 4186, BodyLocation.LeftArm),
            new Bone("SKEL_L_Finger30", 4103, 26613, BodyLocation.LeftArm),
            new Bone("SKEL_L_Finger31", 4103, 4137, BodyLocation.LeftArm),
            new Bone("SKEL_L_Finger32", 7, 4138, BodyLocation.LeftArm),
            new Bone("SKEL_L_Finger40", 4103, 26614, BodyLocation.LeftArm),
            new Bone("SKEL_L_Finger41", 4103, 4153, BodyLocation.LeftArm),
            new Bone("SKEL_L_Finger42", 7, 4154, BodyLocation.LeftArm),
            new Bone("PH_L_Hand", 119, 60309, BodyLocation.LeftArm),
            new Bone("IK_L_Hand", 119, 36029, BodyLocation.LeftArm),
            new Bone("RB_L_ForeArmRoll", 7, 61007, BodyLocation.LeftArm),
            new Bone("RB_L_ArmRoll", 7, 5232, BodyLocation.LeftArm),
            new Bone("MH_L_Elbow", 119, 22711, BodyLocation.LeftArm),
            new Bone("SKEL_R_Clavicle", 4103, 10706, BodyLocation.RightArm),
            new Bone("SKEL_R_UpperArm", 4103, 40269, BodyLocation.RightArm),
            new Bone("SKEL_R_Forearm", 4215, 28252, BodyLocation.RightArm),
            new Bone("SKEL_R_Hand", 4215, 57005, BodyLocation.RightArm),
            new Bone("SKEL_R_Finger00", 4103, 58866, BodyLocation.RightArm),
            new Bone("SKEL_R_Finger01", 4103, 64016, BodyLocation.RightArm),
            new Bone("SKEL_R_Finger02", 7, 64017, BodyLocation.RightArm),
            new Bone("SKEL_R_Finger10", 4103, 58867, BodyLocation.RightArm),
            new Bone("SKEL_R_Finger11", 4103, 64096, BodyLocation.RightArm),
            new Bone("SKEL_R_Finger12", 7, 64097, BodyLocation.RightArm),
            new Bone("SKEL_R_Finger20", 4103, 58868, BodyLocation.RightArm),
            new Bone("SKEL_R_Finger21", 4103, 64112, BodyLocation.RightArm),
            new Bone("SKEL_R_Finger22", 7, 64113, BodyLocation.RightArm),
            new Bone("SKEL_R_Finger30", 4103, 58869, BodyLocation.RightArm),
            new Bone("SKEL_R_Finger31", 4103, 64064, BodyLocation.RightArm),
            new Bone("SKEL_R_Finger32", 7, 64065, BodyLocation.RightArm),
            new Bone("SKEL_R_Finger40", 4103, 58870, BodyLocation.RightArm),
            new Bone("SKEL_R_Finger41", 4103, 64080, BodyLocation.RightArm),
            new Bone("SKEL_R_Finger42", 7, 64081, BodyLocation.RightArm),
            new Bone("PH_R_Hand", 119, 28422, BodyLocation.RightArm),
            new Bone("IK_R_Hand", 119, 6286, BodyLocation.RightArm),
            new Bone("RB_R_ForeArmRoll", 7, 43810, BodyLocation.RightArm),
            new Bone("RB_R_ArmRoll", 7, 37119, BodyLocation.RightArm),
            new Bone("MH_R_Elbow", 119, 2992, BodyLocation.RightArm),
            new Bone("SKEL_Neck_1", 4103, 39317, BodyLocation.Neck),
            new Bone("SKEL_Head", 4103, 31086, BodyLocation.Head),
            new Bone("IK_Head", 119, 12844, BodyLocation.Head),
            new Bone("FACIAL_facialRoot", 4103, 65068, BodyLocation.Head),
            new Bone("FB_L_Brow_Out_000", 1799, 58331, BodyLocation.Head),
            new Bone("FB_L_Lid_Upper_000", 1911, 45750, BodyLocation.Head),
            new Bone("FB_L_Eye_000", 1799, 25260, BodyLocation.Head),
            new Bone("FB_L_CheekBone_000", 1799, 21550, BodyLocation.Head),
            new Bone("FB_L_Lip_Corner_000", 1911, 29868, BodyLocation.Head),
            new Bone("FB_R_Lid_Upper_000", 1911, 43536, BodyLocation.Head),
            new Bone("FB_R_Eye_000", 1799, 27474, BodyLocation.Head),
            new Bone("FB_R_CheekBone_000", 1799, 19336, BodyLocation.Head),
            new Bone("FB_R_Brow_Out_000", 1799, 1356, BodyLocation.Head),
            new Bone("FB_R_Lip_Corner_000", 1911, 11174, BodyLocation.Head),
            new Bone("FB_Brow_Centre_000", 1799, 37193, BodyLocation.Head),
            new Bone("FB_UpperLipRoot_000", 5895, 20178, BodyLocation.Head),
            new Bone("FB_UpperLip_000", 6007, 61839, BodyLocation.Head),
            new Bone("FB_L_Lip_Top_000", 1911, 20279, BodyLocation.Head),
            new Bone("FB_R_Lip_Top_000", 1911, 17719, BodyLocation.Head),
            new Bone("FB_Jaw_000", 5895, 46240, BodyLocation.Head),
            new Bone("FB_LowerLipRoot_000", 5895, 17188, BodyLocation.Head),
            new Bone("FB_LowerLip_000", 6007, 20623, BodyLocation.Head),
            new Bone("FB_L_Lip_Bot_000", 1911, 47419, BodyLocation.Head),
            new Bone("FB_R_Lip_Bot_000", 1911, 49979, BodyLocation.Head),
            new Bone("FB_Tongue_000", 1911, 47495, BodyLocation.Head),
            new Bone("RB_Neck_1", 7, 35731, BodyLocation.Neck),
            new Bone("IK_Root", 119, 56604, BodyLocation.LowerTorso)
        };
        return PedBones.FirstOrDefault(x => x.Tag == ID);
    }


}