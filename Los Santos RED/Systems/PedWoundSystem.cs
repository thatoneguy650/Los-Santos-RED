using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class PedWoundSystem
{
    private static uint GameTimeLastHurtCivilian;
    private static uint GameTimeLastKilledCivilian;
    private static uint GameTimeLastHurtCop;
    private static uint GameTimeLastKilledCop;
    private static List<PedBone> PedBones = new List<PedBone>();
    private static List<PedHealthState> PedHealthStates = new List<PedHealthState>();
    public static List<GTAPed> PlayerKilledCops { get; set; } = new List<GTAPed>();
    public static List<GTAPed> PlayerKilledCivilians { get; set; } = new List<GTAPed>();
    public static bool RecentlyHurtCivilian(uint TimeSince)
    {
        if (GameTimeLastHurtCivilian == 0)
            return false;
        else if (Game.GameTime - GameTimeLastHurtCivilian <= TimeSince)
            return true;
        else
            return false;
    }
    public static bool RecentlyKilledCivilian(uint TimeSince)
    {
        if (GameTimeLastKilledCivilian == 0)
            return false;
        else if (Game.GameTime - GameTimeLastKilledCivilian <= TimeSince)
            return true;
        else
            return false;
    }
    public static bool RecentlyHurtCop(uint TimeSince)
    {
        if (GameTimeLastHurtCop == 0)
            return false;
        else if (Game.GameTime - GameTimeLastHurtCop <= TimeSince)
            return true;
        else
            return false;
    }
    public static bool RecentlyKilledCop(uint TimeSince)
    {
        if (GameTimeLastKilledCop == 0)
            return false;
        else if (Game.GameTime - GameTimeLastKilledCop <= TimeSince)
            return true;
        else
            return false;
    }
    public static bool NearCivilianMurderVictim(float Distance)
    {
        if (PlayerKilledCivilians.Any(x => x.Pedestrian.Exists() && x.Pedestrian.DistanceTo2D(Game.LocalPlayer.Character) <= Distance))
            return true;
        else
            return false;
    }
    public static bool NearCopMurderVictim(float Distance)
    {
        if (PlayerKilledCops.Any(x => x.Pedestrian.Exists() && x.Pedestrian.DistanceTo2D(Game.LocalPlayer.Character) <= Distance))
            return true;
        else
            return false;
    }
    public enum BodyLocation
    {
        Head = 0,
        Neck = 1,
        UpperTorso = 2,
        LowerTorso = 3,
        LeftArm = 4,
        RightArm = 5,
        LeftLeg = 6,
        RightLeg = 7,
        Unknown = 8,
    }
    public enum InjuryType
    {
        Vanilla = -1,
        Normal = 0,
        Graze = 1,
        Critical = 2,
        Fatal = 3,
    }
    public static bool IsRunning { get; set; }
    public static void Initialize()
    {
        IsRunning = true;
        SetupLists();

    }
    private static void SetupLists()
    {
        PlayerKilledCivilians = new List<GTAPed>();
        PlayerKilledCops = new List<GTAPed>();
        PedBones = new List<PedBone>();
        PedBones.Add(new PedBone("SKEL_ROOT", 4215, 0, BodyLocation.LowerTorso));
        PedBones.Add(new PedBone("SKEL_Pelvis", 4103, 11816, BodyLocation.LowerTorso));
        PedBones.Add(new PedBone("SKEL_L_Thigh", 4103, 58271, BodyLocation.LeftLeg));
        PedBones.Add(new PedBone("SKEL_L_Calf", 4103, 63931, BodyLocation.LeftLeg));
        PedBones.Add(new PedBone("SKEL_L_Foot", 4103, 14201, BodyLocation.LeftLeg));
        PedBones.Add(new PedBone("SKEL_L_Toe0", 7, 2108, BodyLocation.LeftLeg));
        PedBones.Add(new PedBone("IK_L_Foot", 119, 65245, BodyLocation.LeftLeg));
        PedBones.Add(new PedBone("PH_L_Foot", 119, 57717, BodyLocation.LeftLeg));
        PedBones.Add(new PedBone("MH_L_Knee", 119, 46078, BodyLocation.LeftLeg));
        PedBones.Add(new PedBone("SKEL_R_Thigh", 4103, 51826, BodyLocation.RightLeg));
        PedBones.Add(new PedBone("SKEL_R_Calf", 4103, 36864, BodyLocation.RightLeg));
        PedBones.Add(new PedBone("SKEL_R_Foot", 4103, 52301, BodyLocation.RightLeg));
        PedBones.Add(new PedBone("SKEL_R_Toe0", 7, 20781, BodyLocation.RightLeg));
        PedBones.Add(new PedBone("IK_R_Foot", 119, 35502, BodyLocation.RightLeg));
        PedBones.Add(new PedBone("PH_R_Foot", 119, 24806, BodyLocation.RightLeg));
        PedBones.Add(new PedBone("MH_R_Knee", 119, 16335, BodyLocation.RightLeg));
        PedBones.Add(new PedBone("RB_L_ThighRoll", 7, 23639, BodyLocation.LeftLeg));
        PedBones.Add(new PedBone("RB_R_ThighRoll", 7, 6442, BodyLocation.RightLeg));
        PedBones.Add(new PedBone("SKEL_Spine_Root", 4103, 57597, BodyLocation.LowerTorso));
        PedBones.Add(new PedBone("SKEL_Spine0", 4103, 23553, BodyLocation.LowerTorso));
        PedBones.Add(new PedBone("SKEL_Spine1", 4103, 24816, BodyLocation.LowerTorso));
        PedBones.Add(new PedBone("SKEL_Spine2", 4103, 24817, BodyLocation.UpperTorso));
        PedBones.Add(new PedBone("SKEL_Spine3", 4103, 24818, BodyLocation.UpperTorso));
        PedBones.Add(new PedBone("SKEL_L_Clavicle", 4103, 64729, BodyLocation.LeftArm));
        PedBones.Add(new PedBone("SKEL_L_UpperArm", 4103, 45509, BodyLocation.LeftArm));
        PedBones.Add(new PedBone("SKEL_L_Forearm", 4215, 61163, BodyLocation.LeftArm));
        PedBones.Add(new PedBone("SKEL_L_Hand", 4215, 18905, BodyLocation.LeftArm));
        PedBones.Add(new PedBone("SKEL_L_Finger00", 4103, 26610, BodyLocation.LeftArm));
        PedBones.Add(new PedBone("SKEL_L_Finger01", 4103, 4089, BodyLocation.LeftArm));
        PedBones.Add(new PedBone("SKEL_L_Finger02", 7, 4090, BodyLocation.LeftArm));
        PedBones.Add(new PedBone("SKEL_L_Finger10", 4103, 26611, BodyLocation.LeftArm));
        PedBones.Add(new PedBone("SKEL_L_Finger11", 4103, 4169, BodyLocation.LeftArm));
        PedBones.Add(new PedBone("SKEL_L_Finger12", 7, 4170, BodyLocation.LeftArm));
        PedBones.Add(new PedBone("SKEL_L_Finger20", 4103, 26612, BodyLocation.LeftArm));
        PedBones.Add(new PedBone("SKEL_L_Finger21", 4103, 4185, BodyLocation.LeftArm));
        PedBones.Add(new PedBone("SKEL_L_Finger22", 7, 4186, BodyLocation.LeftArm));
        PedBones.Add(new PedBone("SKEL_L_Finger30", 4103, 26613, BodyLocation.LeftArm));
        PedBones.Add(new PedBone("SKEL_L_Finger31", 4103, 4137, BodyLocation.LeftArm));
        PedBones.Add(new PedBone("SKEL_L_Finger32", 7, 4138, BodyLocation.LeftArm));
        PedBones.Add(new PedBone("SKEL_L_Finger40", 4103, 26614, BodyLocation.LeftArm));
        PedBones.Add(new PedBone("SKEL_L_Finger41", 4103, 4153, BodyLocation.LeftArm));
        PedBones.Add(new PedBone("SKEL_L_Finger42", 7, 4154, BodyLocation.LeftArm));
        PedBones.Add(new PedBone("PH_L_Hand", 119, 60309, BodyLocation.LeftArm));
        PedBones.Add(new PedBone("IK_L_Hand", 119, 36029, BodyLocation.LeftArm));
        PedBones.Add(new PedBone("RB_L_ForeArmRoll", 7, 61007, BodyLocation.LeftArm));
        PedBones.Add(new PedBone("RB_L_ArmRoll", 7, 5232, BodyLocation.LeftArm));
        PedBones.Add(new PedBone("MH_L_Elbow", 119, 22711, BodyLocation.LeftArm));
        PedBones.Add(new PedBone("SKEL_R_Clavicle", 4103, 10706, BodyLocation.RightArm));
        PedBones.Add(new PedBone("SKEL_R_UpperArm", 4103, 40269, BodyLocation.RightArm));
        PedBones.Add(new PedBone("SKEL_R_Forearm", 4215, 28252, BodyLocation.RightArm));
        PedBones.Add(new PedBone("SKEL_R_Hand", 4215, 57005, BodyLocation.RightArm));
        PedBones.Add(new PedBone("SKEL_R_Finger00", 4103, 58866, BodyLocation.RightArm));
        PedBones.Add(new PedBone("SKEL_R_Finger01", 4103, 64016, BodyLocation.RightArm));
        PedBones.Add(new PedBone("SKEL_R_Finger02", 7, 64017, BodyLocation.RightArm));
        PedBones.Add(new PedBone("SKEL_R_Finger10", 4103, 58867, BodyLocation.RightArm));
        PedBones.Add(new PedBone("SKEL_R_Finger11", 4103, 64096, BodyLocation.RightArm));
        PedBones.Add(new PedBone("SKEL_R_Finger12", 7, 64097, BodyLocation.RightArm));
        PedBones.Add(new PedBone("SKEL_R_Finger20", 4103, 58868, BodyLocation.RightArm));
        PedBones.Add(new PedBone("SKEL_R_Finger21", 4103, 64112, BodyLocation.RightArm));
        PedBones.Add(new PedBone("SKEL_R_Finger22", 7, 64113, BodyLocation.RightArm));
        PedBones.Add(new PedBone("SKEL_R_Finger30", 4103, 58869, BodyLocation.RightArm));
        PedBones.Add(new PedBone("SKEL_R_Finger31", 4103, 64064, BodyLocation.RightArm));
        PedBones.Add(new PedBone("SKEL_R_Finger32", 7, 64065, BodyLocation.RightArm));
        PedBones.Add(new PedBone("SKEL_R_Finger40", 4103, 58870, BodyLocation.RightArm));
        PedBones.Add(new PedBone("SKEL_R_Finger41", 4103, 64080, BodyLocation.RightArm));
        PedBones.Add(new PedBone("SKEL_R_Finger42", 7, 64081, BodyLocation.RightArm));
        PedBones.Add(new PedBone("PH_R_Hand", 119, 28422, BodyLocation.RightArm));
        PedBones.Add(new PedBone("IK_R_Hand", 119, 6286, BodyLocation.RightArm));
        PedBones.Add(new PedBone("RB_R_ForeArmRoll", 7, 43810, BodyLocation.RightArm));
        PedBones.Add(new PedBone("RB_R_ArmRoll", 7, 37119, BodyLocation.RightArm));
        PedBones.Add(new PedBone("MH_R_Elbow", 119, 2992, BodyLocation.RightArm));
        PedBones.Add(new PedBone("SKEL_Neck_1", 4103, 39317, BodyLocation.Neck));
        PedBones.Add(new PedBone("SKEL_Head", 4103, 31086, BodyLocation.Head));
        PedBones.Add(new PedBone("IK_Head", 119, 12844, BodyLocation.Head));
        PedBones.Add(new PedBone("FACIAL_facialRoot", 4103, 65068, BodyLocation.Head));
        PedBones.Add(new PedBone("FB_L_Brow_Out_000", 1799, 58331, BodyLocation.Head));
        PedBones.Add(new PedBone("FB_L_Lid_Upper_000", 1911, 45750, BodyLocation.Head));
        PedBones.Add(new PedBone("FB_L_Eye_000", 1799, 25260, BodyLocation.Head));
        PedBones.Add(new PedBone("FB_L_CheekBone_000", 1799, 21550, BodyLocation.Head));
        PedBones.Add(new PedBone("FB_L_Lip_Corner_000", 1911, 29868, BodyLocation.Head));
        PedBones.Add(new PedBone("FB_R_Lid_Upper_000", 1911, 43536, BodyLocation.Head));
        PedBones.Add(new PedBone("FB_R_Eye_000", 1799, 27474, BodyLocation.Head));
        PedBones.Add(new PedBone("FB_R_CheekBone_000", 1799, 19336, BodyLocation.Head));
        PedBones.Add(new PedBone("FB_R_Brow_Out_000", 1799, 1356, BodyLocation.Head));
        PedBones.Add(new PedBone("FB_R_Lip_Corner_000", 1911, 11174, BodyLocation.Head));
        PedBones.Add(new PedBone("FB_Brow_Centre_000", 1799, 37193, BodyLocation.Head));
        PedBones.Add(new PedBone("FB_UpperLipRoot_000", 5895, 20178, BodyLocation.Head));
        PedBones.Add(new PedBone("FB_UpperLip_000", 6007, 61839, BodyLocation.Head));
        PedBones.Add(new PedBone("FB_L_Lip_Top_000", 1911, 20279, BodyLocation.Head));
        PedBones.Add(new PedBone("FB_R_Lip_Top_000", 1911, 17719, BodyLocation.Head));
        PedBones.Add(new PedBone("FB_Jaw_000", 5895, 46240, BodyLocation.Head));
        PedBones.Add(new PedBone("FB_LowerLipRoot_000", 5895, 17188, BodyLocation.Head));
        PedBones.Add(new PedBone("FB_LowerLip_000", 6007, 20623, BodyLocation.Head));
        PedBones.Add(new PedBone("FB_L_Lip_Bot_000", 1911, 47419, BodyLocation.Head));
        PedBones.Add(new PedBone("FB_R_Lip_Bot_000", 1911, 49979, BodyLocation.Head));
        PedBones.Add(new PedBone("FB_Tongue_000", 1911, 47495, BodyLocation.Head));
        PedBones.Add(new PedBone("RB_Neck_1", 7, 35731, BodyLocation.Neck));
        PedBones.Add(new PedBone("IK_Root", 119, 56604, BodyLocation.LowerTorso));

        PedHealthStates = new List<PedHealthState>();
        AddPedsToTrack();
    }
    public static void Dispose()
    {
        IsRunning = false;
    }
    public static void Tick()
    {
        PedHealthStates.RemoveAll(x => !x.MyPed.Pedestrian.Exists());

        AddPedsToTrack();
        foreach (PedHealthState MyHealthState in PedHealthStates)
        {
            MyHealthState.CheckDamage();
        }
    }
    private static void AddPedsToTrack()
    {
        foreach (GTACop Cop in PedList.CopPeds)
        {
            if (Cop.Pedestrian.Exists() && !PedHealthStates.Any(x => x.MyPed.Pedestrian.Handle == Cop.Pedestrian.Handle))
            {
                PedHealthStates.Add(new PedHealthState(Cop));
            }
        }
        foreach (GTAPed Civilian in PedList.Civilians)
        {
            if (Civilian.Pedestrian.Exists() && !PedHealthStates.Any(x => x.MyPed.Pedestrian.Handle == Civilian.Pedestrian.Handle))
            {
                PedHealthStates.Add(new PedHealthState(Civilian));
            }
        }
    }
    private static InjuryType RandomType(bool CanBeFatal)
    {
        int RandomNumber = LosSantosRED.MyRand.Next(1, 101);
        if (RandomNumber <= 50)
            return InjuryType.Normal;
        else if (RandomNumber <= 75)
            return InjuryType.Graze;
        else if (RandomNumber <= 92)
            return InjuryType.Critical;
        else if (RandomNumber <= 100 && CanBeFatal)
            return InjuryType.Fatal;
        else
            return InjuryType.Normal;
    }
    private static GTAWeapon GetWeaponLastDamagedBy(Ped Pedestrian)
    {
        foreach (GTAWeapon MyWeapon in GTAWeapons.WeaponsList)
        {
            if (NativeFunction.CallByName<bool>("HAS_PED_BEEN_DAMAGED_BY_WEAPON", Pedestrian, MyWeapon.Hash, 0))
            {
                NativeFunction.CallByName<bool>("CLEAR_PED_LAST_WEAPON_DAMAGE", Pedestrian);
                return MyWeapon;
            }
        }

        if(NativeFunction.CallByName<bool>("HAS_PED_BEEN_DAMAGED_BY_WEAPON", Pedestrian, 0, 1))
            return new GTAWeapon("Generic Melee", 0, GTAWeapon.WeaponCategory.Melee, 0, 0, false, false, false, false);

        if (NativeFunction.CallByName<bool>("HAS_PED_BEEN_DAMAGED_BY_WEAPON", Pedestrian, 0, 2))
            return new GTAWeapon("Generic Weapon", 0, GTAWeapon.WeaponCategory.Melee, 0, 0, false, false, false, false);

        if (NativeFunction.CallByName<bool>("HAS_ENTITY_BEEN_DAMAGED_BY_ANY_VEHICLE", Pedestrian))
            return new GTAWeapon("Vehicle Injury", 0, GTAWeapon.WeaponCategory.Vehicle, 0, 0, false, false, false, false);
        else
            return new GTAWeapon("Unknown", 0, GTAWeapon.WeaponCategory.Unknown, 0, 0, false, false, false, false);
    }
    private static BodyLocation GetDamageLocation(Ped Pedestrian)
    {
        int outBone = 0;
        unsafe
        {
            NativeFunction.CallByName<bool>("GET_PED_LAST_DAMAGE_BONE", Pedestrian, &outBone);
        }

        if (outBone != 0)
        {
            NativeFunction.CallByName<bool>("CLEAR_PED_LAST_DAMAGE_BONE", Pedestrian);
            int DamagedBoneId = outBone;
            PedBone DamagedOne = PedBones.FirstOrDefault(x => x.Tag == DamagedBoneId);
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
    public class PedBone
    {
        public string Name { get; set; }
        public int Type { get; set; }
        public int Tag { get; set; }
        public BodyLocation Location { get; set; }
        public PedBone(string name, int type, int tag)
        {
            Name = name;
            Type = type;
            Tag = tag;
        }
        public PedBone(string name, int type, int tag, BodyLocation location)
        {
            Name = name;
            Type = type;
            Tag = tag;
            Location = location;
        }
    }
    public class PedHealthState
    {
        private uint GameTimeLastDamaged;
        private int CurrentHealth;
        private int CurrentArmor;
        private uint GameTimeLastCheckedDamage;
        public bool NeedDamageCheck
        {
            get
            {
                if (Game.GameTime - GameTimeLastCheckedDamage >= 300)
                    return true;
                else
                    return false;
            }
        }
        public GTAPed MyPed { get; set; }
        public int Health { get; set; }
        public int Armor { get; set; }
        public void CheckDamage()
        {
            if (NeedDamageCheck)
            {
                GameTimeLastCheckedDamage = Game.GameTime;
                CurrentHealth = MyPed.Pedestrian.Health;
                CurrentArmor = MyPed.Pedestrian.Armor;
                if (MyPed.Pedestrian.IsDead && !MyPed.KilledByPlayer)
                {
                    MyPed.CheckPlayerKilledPed();
                    if (MyPed.KilledByPlayer)
                    {
                        Debugging.WriteToLog("PedWoundSystem", string.Format("Player Killed {0}, IsCop: {1}", MyPed.Pedestrian.Handle, MyPed.IsCop));
                        if (MyPed.IsCop)
                        {
                            Police.CurrentCrimes.KillingPolice.CrimeObserved();
                            PlayerKilledCops.Add(MyPed);
                            GameTimeLastKilledCop = Game.GameTime;
                        }
                        else
                        {
                            PlayerKilledCivilians.Add(MyPed);
                            GameTimeLastKilledCivilian = Game.GameTime;
                        }
                    }
                }
                else if (MyPed.Pedestrian.IsAlive && (CurrentHealth < Health || CurrentArmor < Armor))
                {
                    if (!MyPed.HurtByPlayer)
                    {
                        MyPed.CheckPlayerHurtPed();
                        if (MyPed.HurtByPlayer)
                        {
                            Debugging.WriteToLog("PedWoundSystem", string.Format("Player Hurt {0}, IsCop: {1}", MyPed.Pedestrian.Handle, MyPed.IsCop));
                            if (MyPed.IsCop)
                            {
                                Police.CurrentCrimes.HurtingPolice.CrimeObserved();
                                GameTimeLastHurtCop = Game.GameTime;
                            }
                            else
                            {
                                GameTimeLastHurtCivilian = Game.GameTime;
                            }
                        }
                    }
                    ApplyDamage();
                    Health = CurrentHealth;
                    Armor = CurrentArmor;
                }
            }
        }
        private void ApplyDamage()
        {
            bool HurtByPed = NativeFunction.CallByName<bool>("HAS_ENTITY_BEEN_DAMAGED_BY_ANY_PED", MyPed.Pedestrian);
            bool HurtByVehicle = NativeFunction.CallByName<bool>("HAS_ENTITY_BEEN_DAMAGED_BY_ANY_VEHICLE", MyPed.Pedestrian);
            if (!HurtByPed && !HurtByVehicle)
            {
                return;
            }

            int TotalDamage = Health - CurrentHealth + Armor - CurrentArmor;
            int HealthDamage = Health - CurrentHealth;
            int ArmorDamage = Armor - CurrentArmor;
            BodyLocation DamagedLocation = GetDamageLocation(MyPed.Pedestrian);

            GTAWeapon DamagingWeapon = GetWeaponLastDamagedBy(MyPed.Pedestrian);

            bool CanBeFatal = false;
            if (DamagedLocation == BodyLocation.Head || DamagedLocation == BodyLocation.Neck || DamagedLocation == BodyLocation.UpperTorso)
                CanBeFatal = true;

            bool ArmorWillProtect = false;
            if (DamagedLocation == BodyLocation.UpperTorso)
                ArmorWillProtect = true;

            InjuryType HealthInjury = InjuryType.Vanilla; //RandomType(CanBeFatal);
            InjuryType ArmorInjury = InjuryType.Vanilla; //RandomType(false);

            if (DamagingWeapon.Category != GTAWeapon.WeaponCategory.Vehicle)
            {
                HealthInjury = RandomType(CanBeFatal);
                ArmorInjury = RandomType(false);
            }
            

            float HealthDamageModifier = 1.0f;

            if (HealthInjury == InjuryType.Fatal)
                HealthDamageModifier = 10.0f;
            else if (HealthInjury == InjuryType.Normal)
                HealthDamageModifier = 5.0f;//3.0f;//1.0f;
            else if (HealthInjury == InjuryType.Graze)
                HealthDamageModifier = 0.75f;//0.25f;
            else if (HealthInjury == InjuryType.Critical)
                HealthDamageModifier = 8.0f;// 6.0f;//2.0f;
            else if (HealthInjury == InjuryType.Vanilla)
                HealthDamageModifier = 1.0f;// 6.0f;//2.0f;


            if (MyPed.Pedestrian.Health == 0)//already dead, we are intercepting
                HealthInjury = InjuryType.Fatal;

            float ArmorDamageModifier = 1.0f;

            if (ArmorInjury == InjuryType.Normal)
                ArmorDamageModifier = 1.0f;
            else if (ArmorInjury == InjuryType.Graze)
                ArmorDamageModifier = 0.25f;
            else if (ArmorInjury == InjuryType.Critical)
                ArmorDamageModifier = 2.0f;
            else if (ArmorInjury == InjuryType.Vanilla)
                ArmorDamageModifier = 1.0f;

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

            Debugging.WriteToLog("Damage Detected", string.Format("Ped: {0}, Location: {1}, Weapon: {2}, Injury: {3}, PrevHealth/Armor: {4}/{5}, Health/Armor: {6}/{7}, Armor {8}, VanillaDamageHealthArmor {9}/{10}, New DamageHealthArmor {11}/{12}",
                                                                        MyPed.Pedestrian.Handle,DamagedLocation, DamagingWeapon.Name, HealthInjury, Health, Armor, MyPed.Pedestrian.Health, MyPed.Pedestrian.Armor, ArmorWillProtect, HealthDamage, ArmorDamage, NewHealthDamage, NewArmorDamage));

            GameTimeLastDamaged = Game.GameTime;
        }
        public void IsDamaged()
        {
            GameTimeLastDamaged = Game.GameTime;
        }
        public PedHealthState()
        {

        }
        public PedHealthState(GTAPed _MyPed)
        {
            MyPed = _MyPed;
            Health = _MyPed.Pedestrian.Health;
            Armor = _MyPed.Pedestrian.Armor;
            CurrentArmor = Armor;
            CurrentHealth = Health;
        }
    }
}

