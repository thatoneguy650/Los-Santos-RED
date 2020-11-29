using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class PedDamageManager
{
    private enum BodyLocation
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
    private enum InjuryType
    {
        Vanilla = -1,
        Normal = 0,
        Graze = 1,
        Critical = 2,
        Fatal = 3,
    }
    private static uint GameTimeLastHurtCivilian;
    private static uint GameTimeLastKilledCivilian;
    private static uint GameTimeLastHurtCop;
    private static uint GameTimeLastKilledCop;
    private static List<PedBone> PedBones = new List<PedBone>();
    private static List<PedHealthState> PedHealthStates = new List<PedHealthState>();
    private static List<PedExt> PlayerKilledCivilians = new List<PedExt>();
    private static List<PedExt> PlayerKilledCops = new List<PedExt>();

    // public static List<string> AllPedDamageList { get; private set; } = new List<string>();
    public static bool RecentlyHurtCivilian
    {
        get
            {
            if (GameTimeLastHurtCivilian == 0)
                return false;
            else if (Game.GameTime - GameTimeLastHurtCivilian <= 5000)
                return true;
            else
                return false;
        }
    }
    public static bool RecentlyKilledCivilian
    {
        get
        {
            if (GameTimeLastKilledCivilian == 0)
                return false;
            else if (Game.GameTime - GameTimeLastKilledCivilian <= 5000)
                return true;
            else
                return false;
        }
    }
    public static bool RecentlyHurtPed
    {
        get
        {
            if (RecentlyHurtCivilian || RecentlyHurtCop)
                return true;
            else
                return false;
        }
    }
    public static bool RecentlyHurtCop
    {
        get
        {
            if (GameTimeLastHurtCop == 0)
                return false;
            else if (Game.GameTime - GameTimeLastHurtCop <= 5000)
                return true;
            else
                return false;
        }
    }
    public static bool RecentlyKilledCop
    {
        get
        {
            if (GameTimeLastKilledCop == 0)
                return false;
            else if (Game.GameTime - GameTimeLastKilledCop <= 5000)
                return true;
            else
                return false;
        }
    }
    public static bool KilledAnyCops
    {
        get
        {
            return PlayerKilledCops.Any();
        }
    }
    public static bool NearCivilianMurderVictim
    {
        get
        {
            if (PlayerKilledCivilians.Any(x => x.Pedestrian.Exists() && x.Pedestrian.DistanceTo2D(Game.LocalPlayer.Character) <= 9f))
                return true;
            else
                return false;
        }
    }
    public static bool NearCopMurderVictim
    {
        get
        {
            if (PlayerKilledCops.Any(x => x.Pedestrian.Exists() && x.Pedestrian.DistanceTo2D(Game.LocalPlayer.Character) <= 15f))
                return true;
            else
                return false;
        }
    }
    public static bool IsPlayerBleeding
    {
        get
        {
            return PedHealthStates.Any(x => x.IsBleeding && x.IsPlayerPed);
        }
    }
    public static bool IsRunning { get; set; }
    public static void Initialize()
    {
        IsRunning = true;
        SetupLists();
    }
    private static void SetupLists()
    {
        PlayerKilledCivilians = new List<PedExt>();
        PlayerKilledCops = new List<PedExt>();
        PedBones = new List<PedBone>
        {
            new PedBone("SKEL_ROOT", 4215, 0, BodyLocation.LowerTorso),
            new PedBone("SKEL_Pelvis", 4103, 11816, BodyLocation.LowerTorso),
            new PedBone("SKEL_L_Thigh", 4103, 58271, BodyLocation.LeftLeg),
            new PedBone("SKEL_L_Calf", 4103, 63931, BodyLocation.LeftLeg),
            new PedBone("SKEL_L_Foot", 4103, 14201, BodyLocation.LeftLeg),
            new PedBone("SKEL_L_Toe0", 7, 2108, BodyLocation.LeftLeg),
            new PedBone("IK_L_Foot", 119, 65245, BodyLocation.LeftLeg),
            new PedBone("PH_L_Foot", 119, 57717, BodyLocation.LeftLeg),
            new PedBone("MH_L_Knee", 119, 46078, BodyLocation.LeftLeg),
            new PedBone("SKEL_R_Thigh", 4103, 51826, BodyLocation.RightLeg),
            new PedBone("SKEL_R_Calf", 4103, 36864, BodyLocation.RightLeg),
            new PedBone("SKEL_R_Foot", 4103, 52301, BodyLocation.RightLeg),
            new PedBone("SKEL_R_Toe0", 7, 20781, BodyLocation.RightLeg),
            new PedBone("IK_R_Foot", 119, 35502, BodyLocation.RightLeg),
            new PedBone("PH_R_Foot", 119, 24806, BodyLocation.RightLeg),
            new PedBone("MH_R_Knee", 119, 16335, BodyLocation.RightLeg),
            new PedBone("RB_L_ThighRoll", 7, 23639, BodyLocation.LeftLeg),
            new PedBone("RB_R_ThighRoll", 7, 6442, BodyLocation.RightLeg),
            new PedBone("SKEL_Spine_Root", 4103, 57597, BodyLocation.LowerTorso),
            new PedBone("SKEL_Spine0", 4103, 23553, BodyLocation.LowerTorso),
            new PedBone("SKEL_Spine1", 4103, 24816, BodyLocation.LowerTorso),
            new PedBone("SKEL_Spine2", 4103, 24817, BodyLocation.UpperTorso),
            new PedBone("SKEL_Spine3", 4103, 24818, BodyLocation.UpperTorso),
            new PedBone("SKEL_L_Clavicle", 4103, 64729, BodyLocation.LeftArm),
            new PedBone("SKEL_L_UpperArm", 4103, 45509, BodyLocation.LeftArm),
            new PedBone("SKEL_L_Forearm", 4215, 61163, BodyLocation.LeftArm),
            new PedBone("SKEL_L_Hand", 4215, 18905, BodyLocation.LeftArm),
            new PedBone("SKEL_L_Finger00", 4103, 26610, BodyLocation.LeftArm),
            new PedBone("SKEL_L_Finger01", 4103, 4089, BodyLocation.LeftArm),
            new PedBone("SKEL_L_Finger02", 7, 4090, BodyLocation.LeftArm),
            new PedBone("SKEL_L_Finger10", 4103, 26611, BodyLocation.LeftArm),
            new PedBone("SKEL_L_Finger11", 4103, 4169, BodyLocation.LeftArm),
            new PedBone("SKEL_L_Finger12", 7, 4170, BodyLocation.LeftArm),
            new PedBone("SKEL_L_Finger20", 4103, 26612, BodyLocation.LeftArm),
            new PedBone("SKEL_L_Finger21", 4103, 4185, BodyLocation.LeftArm),
            new PedBone("SKEL_L_Finger22", 7, 4186, BodyLocation.LeftArm),
            new PedBone("SKEL_L_Finger30", 4103, 26613, BodyLocation.LeftArm),
            new PedBone("SKEL_L_Finger31", 4103, 4137, BodyLocation.LeftArm),
            new PedBone("SKEL_L_Finger32", 7, 4138, BodyLocation.LeftArm),
            new PedBone("SKEL_L_Finger40", 4103, 26614, BodyLocation.LeftArm),
            new PedBone("SKEL_L_Finger41", 4103, 4153, BodyLocation.LeftArm),
            new PedBone("SKEL_L_Finger42", 7, 4154, BodyLocation.LeftArm),
            new PedBone("PH_L_Hand", 119, 60309, BodyLocation.LeftArm),
            new PedBone("IK_L_Hand", 119, 36029, BodyLocation.LeftArm),
            new PedBone("RB_L_ForeArmRoll", 7, 61007, BodyLocation.LeftArm),
            new PedBone("RB_L_ArmRoll", 7, 5232, BodyLocation.LeftArm),
            new PedBone("MH_L_Elbow", 119, 22711, BodyLocation.LeftArm),
            new PedBone("SKEL_R_Clavicle", 4103, 10706, BodyLocation.RightArm),
            new PedBone("SKEL_R_UpperArm", 4103, 40269, BodyLocation.RightArm),
            new PedBone("SKEL_R_Forearm", 4215, 28252, BodyLocation.RightArm),
            new PedBone("SKEL_R_Hand", 4215, 57005, BodyLocation.RightArm),
            new PedBone("SKEL_R_Finger00", 4103, 58866, BodyLocation.RightArm),
            new PedBone("SKEL_R_Finger01", 4103, 64016, BodyLocation.RightArm),
            new PedBone("SKEL_R_Finger02", 7, 64017, BodyLocation.RightArm),
            new PedBone("SKEL_R_Finger10", 4103, 58867, BodyLocation.RightArm),
            new PedBone("SKEL_R_Finger11", 4103, 64096, BodyLocation.RightArm),
            new PedBone("SKEL_R_Finger12", 7, 64097, BodyLocation.RightArm),
            new PedBone("SKEL_R_Finger20", 4103, 58868, BodyLocation.RightArm),
            new PedBone("SKEL_R_Finger21", 4103, 64112, BodyLocation.RightArm),
            new PedBone("SKEL_R_Finger22", 7, 64113, BodyLocation.RightArm),
            new PedBone("SKEL_R_Finger30", 4103, 58869, BodyLocation.RightArm),
            new PedBone("SKEL_R_Finger31", 4103, 64064, BodyLocation.RightArm),
            new PedBone("SKEL_R_Finger32", 7, 64065, BodyLocation.RightArm),
            new PedBone("SKEL_R_Finger40", 4103, 58870, BodyLocation.RightArm),
            new PedBone("SKEL_R_Finger41", 4103, 64080, BodyLocation.RightArm),
            new PedBone("SKEL_R_Finger42", 7, 64081, BodyLocation.RightArm),
            new PedBone("PH_R_Hand", 119, 28422, BodyLocation.RightArm),
            new PedBone("IK_R_Hand", 119, 6286, BodyLocation.RightArm),
            new PedBone("RB_R_ForeArmRoll", 7, 43810, BodyLocation.RightArm),
            new PedBone("RB_R_ArmRoll", 7, 37119, BodyLocation.RightArm),
            new PedBone("MH_R_Elbow", 119, 2992, BodyLocation.RightArm),
            new PedBone("SKEL_Neck_1", 4103, 39317, BodyLocation.Neck),
            new PedBone("SKEL_Head", 4103, 31086, BodyLocation.Head),
            new PedBone("IK_Head", 119, 12844, BodyLocation.Head),
            new PedBone("FACIAL_facialRoot", 4103, 65068, BodyLocation.Head),
            new PedBone("FB_L_Brow_Out_000", 1799, 58331, BodyLocation.Head),
            new PedBone("FB_L_Lid_Upper_000", 1911, 45750, BodyLocation.Head),
            new PedBone("FB_L_Eye_000", 1799, 25260, BodyLocation.Head),
            new PedBone("FB_L_CheekBone_000", 1799, 21550, BodyLocation.Head),
            new PedBone("FB_L_Lip_Corner_000", 1911, 29868, BodyLocation.Head),
            new PedBone("FB_R_Lid_Upper_000", 1911, 43536, BodyLocation.Head),
            new PedBone("FB_R_Eye_000", 1799, 27474, BodyLocation.Head),
            new PedBone("FB_R_CheekBone_000", 1799, 19336, BodyLocation.Head),
            new PedBone("FB_R_Brow_Out_000", 1799, 1356, BodyLocation.Head),
            new PedBone("FB_R_Lip_Corner_000", 1911, 11174, BodyLocation.Head),
            new PedBone("FB_Brow_Centre_000", 1799, 37193, BodyLocation.Head),
            new PedBone("FB_UpperLipRoot_000", 5895, 20178, BodyLocation.Head),
            new PedBone("FB_UpperLip_000", 6007, 61839, BodyLocation.Head),
            new PedBone("FB_L_Lip_Top_000", 1911, 20279, BodyLocation.Head),
            new PedBone("FB_R_Lip_Top_000", 1911, 17719, BodyLocation.Head),
            new PedBone("FB_Jaw_000", 5895, 46240, BodyLocation.Head),
            new PedBone("FB_LowerLipRoot_000", 5895, 17188, BodyLocation.Head),
            new PedBone("FB_LowerLip_000", 6007, 20623, BodyLocation.Head),
            new PedBone("FB_L_Lip_Bot_000", 1911, 47419, BodyLocation.Head),
            new PedBone("FB_R_Lip_Bot_000", 1911, 49979, BodyLocation.Head),
            new PedBone("FB_Tongue_000", 1911, 47495, BodyLocation.Head),
            new PedBone("RB_Neck_1", 7, 35731, BodyLocation.Neck),
            new PedBone("IK_Root", 119, 56604, BodyLocation.LowerTorso)
        };
        PedHealthStates = new List<PedHealthState>();
        AddPedsToTrack();
    }
    public static void Dispose()
    {
        IsRunning = false;
    }
    public static void Tick()
    {
        if (IsRunning)
        {
            PedHealthStates.RemoveAll(x => !x.MyPed.Pedestrian.Exists());
            AddPedsToTrack();
            //AllPedDamageList.Clear();
            foreach (PedHealthState MyHealthState in PedHealthStates)
            {
                MyHealthState.Update();
                //AllPedDamageList.AddRange(MyHealthState.GetDamageList());
            }
            ResetDamageStats();
        }
    }
    public static void Reset()
    {
        GameTimeLastHurtCivilian = 0;
        GameTimeLastKilledCivilian = 0;
        GameTimeLastHurtCop = 0;
        GameTimeLastKilledCop = 0;
    }
    private static void AddPedsToTrack()
    {
        foreach (Cop Cop in PedManager.Cops)
        {
            if (Cop.Pedestrian.Exists() && !PedHealthStates.Any(x => x.MyPed.Pedestrian.Handle == Cop.Pedestrian.Handle))
            {
                PedHealthStates.Add(new PedHealthState(Cop));
            }
        }
        foreach (PedExt Civilian in PedManager.Civilians)
        {
            if (Civilian.Pedestrian.Exists() && !PedHealthStates.Any(x => x.MyPed.Pedestrian.Handle == Civilian.Pedestrian.Handle))
            {
                PedHealthStates.Add(new PedHealthState(Civilian));
            }
        }
        if (!PedHealthStates.Any(x => x.MyPed.Pedestrian.Handle == Game.LocalPlayer.Character.Handle))
        {
            PedHealthStates.Add(new PedHealthState(new PedExt(Game.LocalPlayer.Character)));
        }
    }
    private static void ResetDamageStats()
    {
        // NativeFunction.CallByName<bool>("SET_PLAYER_WEAPON_DAMAGE_MODIFIER", Game.LocalPlayer, 2.0f);
        //NativeFunction.CallByName<bool>("SET_AI_WEAPON_DAMAGE_MODIFIER", 1.0f);
        NativeFunction.CallByName<bool>("SET_PLAYER_HEALTH_RECHARGE_MULTIPLIER", Game.LocalPlayer, 0f);
    }
    private class PedBone
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
    private class PedHealthState
    {
        private uint GameTimeLastBled;
        private bool HurtByPed;
        private bool HurtByVehicle;
        private int CurrentHealth;
        private int CurrentArmor;
        private uint GameTimeLastCheckedDamage;
        private List<DamageDisplay> DamageList = new List<DamageDisplay>();
        public bool NeedDamageCheck
        {
            get
            {
                if (MyPed.Pedestrian == Game.LocalPlayer.Character)
                    return true;
                else if (Game.GameTime - GameTimeLastCheckedDamage >= 300)
                    return true;
                else
                    return false;
            }
        }
        public PedExt MyPed { get; private set; }
        public int Health { get; private set; }
        public int Armor { get; private set; }
        public bool IsBleeding { get; private set; }
        public bool IsBandaging { get; private set; }
        public bool IsPlayerPed
        {
            get
            {
                return MyPed.Pedestrian.Handle == Game.LocalPlayer.Character.Handle;
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
        public void Update()
        {
            if (NeedDamageCheck)
            {
                GameTimeLastCheckedDamage = Game.GameTime;
                CurrentHealth = MyPed.Pedestrian.Health;
                CurrentArmor = MyPed.Pedestrian.Armor;
                if (CurrentHealth < Health || CurrentArmor < Armor)
                {
                    FlagDamage();
                    ModifyDamage();
                    Health = CurrentHealth;
                    Armor = CurrentArmor;            
                }
                if(IsBleeding && MyPed.Pedestrian.IsAlive && ShouldBleed)
                {
                    MyPed.Pedestrian.Health -= 2;
                    CurrentHealth = MyPed.Pedestrian.Health;
                    CurrentArmor = MyPed.Pedestrian.Armor;
                    GameTimeLastBled = Game.GameTime;
                    Debugging.WriteToLog("PedWoundSystem", string.Format("Bleeding {0} {1}", MyPed.Pedestrian.Handle, CurrentHealth));
                }

                if (IsPlayerPed && IsBleeding && !IsBandaging && PlayerStateManager.IsStationary)
                {
                    BandagePed(Game.LocalPlayer.Character);
                }

                DamageList.RemoveAll(x => !x.RecentlyDealtDamage);
            }
        }
        public List<string> GetDamageList()
        {
            return DamageList.OrderByDescending(x => x.GameTimeDamageOccured).Select(x => x.DamageDealt).ToList();
        }
        private void FlagDamage()
        {
            if (MyPed.Pedestrian.IsDead && !MyPed.KilledByPlayer)
            {
                MyPed.CheckPlayerKilledPed();
                if (MyPed.KilledByPlayer)
                {
                    MyPed.HurtByPlayer = true;
                    Debugging.WriteToLog("PedWoundSystem", string.Format("Player Killed {0}, IsCop: {1}", MyPed.Pedestrian.Handle, MyPed.IsCop));
                    if (MyPed.IsCop)
                    {
                        PlayerKilledCops.Add(MyPed);
                        GameTimeLastKilledCop = Game.GameTime;
                        GameTimeLastHurtCop = Game.GameTime;
                    }
                    else
                    {
                        PlayerKilledCivilians.Add(MyPed);
                        GameTimeLastKilledCivilian = Game.GameTime;
                        GameTimeLastHurtCivilian = Game.GameTime;
                    }
                }
            }
            else if (MyPed.Pedestrian.IsAlive)
            {
                if (!MyPed.HurtByPlayer)
                {
                    MyPed.CheckPlayerHurtPed();
                    if (MyPed.HurtByPlayer)
                    {
                        Debugging.WriteToLog("PedWoundSystem", string.Format("Player Hurt {0}, IsCop: {1}", MyPed.Pedestrian.Handle, MyPed.IsCop));
                        if (MyPed.IsCop)
                        {
                            GameTimeLastHurtCop = Game.GameTime;
                        }
                        else
                        {
                            GameTimeLastHurtCivilian = Game.GameTime;
                        }
                    }
                }
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
                WeaponInformation DamagingWeapon = GetWeaponLastDamagedBy(MyPed.Pedestrian);

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

                if(HealthInjury == InjuryType.Critical || HealthInjury == InjuryType.Fatal)
                {
                    FlagAsBleeding();
                }



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
                if (IsPlayerPed)
                {
                    DisplayString = string.Format("      PLAYER: {0}-{1}-{2} Damage {3}/{4} Health {5}/{6}",
                         HealthInjury, DamagedLocation, DamagingWeapon.ModelName, NewHealthDamage, NewArmorDamage, MyPed.Pedestrian.Health, MyPed.Pedestrian.Armor);
                }
                else
                {
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
                }
                if(Health != CurrentHealth)
                {
                    SetRagdoll(CurrentHealth);
                }


                Debugging.WriteToLog("PedWoundSystem", DisplayString);
                DamageList.Add(new DamageDisplay(DisplayString));
                
            }
        } 
        private void SetRagdoll(int NewHealth)
        {
            if (Health - NewHealth >= 65)
            {
                NativeFunction.CallByName<bool>("SET_PED_TO_RAGDOLL", MyPed.Pedestrian, 3000, 3000, 0, false, false, false);
                IsBleeding = true;
                Debugging.WriteToLog("PlayerHealthChanged", string.Format("Critical Hit, Ragdoll"));
            }
            else if (Health - NewHealth >= 35 && General.RandomPercent(60))
            {
                NativeFunction.CallByName<bool>("SET_PED_TO_RAGDOLL", MyPed.Pedestrian, 1500, 1500, 1, false, false, false);
                IsBleeding = true;
                Debugging.WriteToLog("PlayerHealthChanged", string.Format("Critical Hit, Ragdoll"));
            }
            else if (Health - NewHealth >= 15 && General.RandomPercent(30))
            {
                NativeFunction.CallByName<bool>("SET_PED_TO_RAGDOLL", MyPed.Pedestrian, 1500, 1500, 1, false, false, false);
                IsBleeding = true;
                Debugging.WriteToLog("PlayerHealthChanged", string.Format("Critical Hit, Ragdoll"));
            }
            else if (Health - NewHealth >= 10)
            {
                IsBleeding = true;
                Debugging.WriteToLog("PlayerHealthChanged", string.Format("Normal Hit, Bleeding"));
            }
        }
        private void FlagAsBleeding()
        {
            //if(MyPed.Pedestrian.Exists())
            //    NativeFunction.CallByName<bool>("SET_PED_TO_RAGDOLL", MyPed.Pedestrian, 1500, 1500, 0, false, false, false);
            IsBleeding = true;

            if (!NativeFunction.CallByName<bool>("HAS_ANIM_SET_LOADED", "move_m@drunk@verydrunk"))
            {
                NativeFunction.CallByName<bool>("REQUEST_ANIM_SET", "move_m@drunk@verydrunk");
            }

            NativeFunction.CallByName<bool>("SET_PED_MOVEMENT_CLIPSET", MyPed.Pedestrian, "move_m@drunk@verydrunk", 0x3E800000);
        }
        private void BandagePed(Ped PedToBandage)
        {
            if (IsBandaging || !IsBleeding)
                return;

            if (PedToBandage.IsRagdoll || PedToBandage.IsSwimming || PedToBandage.IsInCover)//tons more should probably be checked
            {
                return;
            }

            GameFiber Bandaging = GameFiber.StartNew(delegate
            {
                IsBandaging = true;
                bool PlayingAnimation = false;
                //if (!PedToBandage.IsInAnyVehicle(false))
                //{
                //    General.RequestAnimationDictionay("move_p_m_two_idles@generic");
                //    NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", PedToBandage, (uint)2725352035, true);
                //    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToBandage, "move_p_m_two_idles@generic", "fidget_pick_at_face", 8.0f, -8.0f, 2000, 1, 0, false, false, false);
                //    PlayingAnimation = true;
                //    Debugging.WriteToLog("BandagePed", string.Format("Started Bandaging Animation"));
                //}
                uint GameTimeStartedBandaging = Game.GameTime;
                bool IsFinished = true;
                while (Game.GameTime - GameTimeStartedBandaging <= 2000)
                {
                    if (ExtensionsMethods.Extensions.IsMoveControlPressed() || Game.LocalPlayer.Character.IsDead)
                    {
                        IsFinished = false;
                        break;
                    }
                    GameFiber.Yield();
                }
                if (IsFinished)
                {
                    NativeFunction.CallByName<bool>("RESET_PED_MOVEMENT_CLIPSET", MyPed.Pedestrian, 0f);

                    Debugging.WriteToLog("BandagePed", string.Format("Finished, Not Bleeding"));
                    IsBleeding = false;

                    //NativeFunction.Natives.x2206BF9A37B7F724("DrugsDrivingOut", 3500, 0);//_START_SCREEN_EFFECT

                }
                else
                {
                    Debugging.WriteToLog("BandagePed", string.Format("Interrupted"));
                }

                if (PlayingAnimation)
                    PedToBandage.Tasks.Clear();

                IsBandaging = false;

            }, "Bandaging");
            Debugging.GameFibers.Add(Bandaging);
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
        private InjuryType RandomType(bool CanBeFatal)
        {
            int RandomNumber = General.MyRand.Next(1, 101);
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
        private WeaponInformation GetWeaponLastDamagedBy(Ped Pedestrian)
        {
            foreach (WeaponInformation MyWeapon in WeaponManager.WeaponsList)
            {
                if (NativeFunction.CallByName<bool>("HAS_PED_BEEN_DAMAGED_BY_WEAPON", Pedestrian, MyWeapon.Hash, 0))
                {
                    NativeFunction.CallByName<bool>("CLEAR_PED_LAST_WEAPON_DAMAGE", Pedestrian);
                    return MyWeapon;
                }
            }

            if (NativeFunction.CallByName<bool>("HAS_PED_BEEN_DAMAGED_BY_WEAPON", Pedestrian, 0, 1))
                return new WeaponInformation("Generic Melee", 0, WeaponCategory.Melee, 0, 0, false, false, false);

            if (NativeFunction.CallByName<bool>("HAS_PED_BEEN_DAMAGED_BY_WEAPON", Pedestrian, 0, 2))
                return new WeaponInformation("Generic Weapon", 0, WeaponCategory.Melee, 0, 0, false, false, false);

            if (NativeFunction.CallByName<bool>("HAS_ENTITY_BEEN_DAMAGED_BY_ANY_VEHICLE", Pedestrian))
                return new WeaponInformation("Vehicle Injury", 0, WeaponCategory.Vehicle, 0, 0, false, false, false);
            else
                return new WeaponInformation("Unknown", 0, WeaponCategory.Unknown, 0, 0, false, false, false);
        }
        public PedHealthState()
        {

        }
        public PedHealthState(PedExt _MyPed)
        {
            MyPed = _MyPed;
            Health = _MyPed.Pedestrian.Health;
            Armor = _MyPed.Pedestrian.Armor;
            CurrentArmor = Armor;
            CurrentHealth = Health;
        }
        private class DamageDisplay
        {    
            public DamageDisplay(string damageDealt)
            {
                DamageDealt = damageDealt;
                GameTimeDamageOccured = Game.GameTime;
            }
            public string DamageDealt { get; set; }
            public uint GameTimeDamageOccured { get; set; }
            public bool RecentlyDealtDamage
            {
                get
                {
                    if (Game.GameTime - GameTimeDamageOccured <= 3000)
                        return true;
                    else
                        return false;
                }
            }

        }
    }
}

