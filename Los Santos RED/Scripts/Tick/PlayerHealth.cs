using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class PlayerHealth
{
    private static uint GameTimeLastBled;
    private static List<PedBone> PedBones;
    private static bool PrevIsBleeding;
    private static uint GameTimeLastHealed;
    private static uint GameTimeLastDamaged;

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
        Normal = 0,
        Graze = 1,
        Critical = 2,
        Fatal = 3,
    }
    public static bool IsRunning { get; set; }
    public static int Health { get; set; }
    public static int Armor { get; set; }
    public static bool IsBleeding { get; set; }
    public static bool IsBandaging { get; set; }
    public static bool IsHealing { get; set; }
    public static void Initialize()
    {
        IsRunning = true;
        ResetDamageStats();
        SetupLists();
    }
    public static void ResetDamageStats()
    {
       // NativeFunction.CallByName<bool>("SET_PLAYER_WEAPON_DAMAGE_MODIFIER", Game.LocalPlayer, 2.0f);
        //NativeFunction.CallByName<bool>("SET_AI_WEAPON_DAMAGE_MODIFIER", 1.0f);
        NativeFunction.CallByName<bool>("SET_PLAYER_HEALTH_RECHARGE_MULTIPLIER", Game.LocalPlayer, 0f);
    }
    private static void SetupLists()
    {
        PedBones = new List<PedBone>();
        PedBones.Add(new PedBone("SKEL_ROOT", 4215, 0,BodyLocation.LowerTorso));
        PedBones.Add(new PedBone("SKEL_Pelvis", 4103, 11816,BodyLocation.LowerTorso));
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
        PedBones.Add(new PedBone("RB_Neck_1", 7, 35731,BodyLocation.Neck));
        PedBones.Add(new PedBone("IK_Root", 119, 56604,BodyLocation.LowerTorso));
    }
    public static void Dispose()
    {
        IsRunning = false;
        NativeFunction.CallByName<bool>("SET_PLAYER_WEAPON_DAMAGE_MODIFIER", Game.LocalPlayer, 1.0f);
        NativeFunction.CallByName<bool>("SET_AI_WEAPON_DAMAGE_MODIFIER", 1.0f);
        NativeFunction.CallByName<bool>("SET_PLAYER_HEALTH_RECHARGE_MULTIPLIER", Game.LocalPlayer, 1.0f);
    }
    public static void Tick()
    {
        if (IsRunning)
        {
            ResetDamageStats();


            int CurrentHealth = Game.LocalPlayer.Character.Health;
            int CurrentArmor = Game.LocalPlayer.Character.Armor;

            bool HasBeenDamaged = false;

            if (CurrentHealth < Health || CurrentArmor < Armor)
            {
                HasBeenDamaged = true;
            }

            if (HasBeenDamaged)
            {
                int TotalDamage = Health - CurrentHealth + Armor - CurrentArmor;
                int HealthDamage = Health - CurrentHealth;
                int ArmorDamage = Armor - CurrentArmor;
                BodyLocation DamagedLocation = GetDamageLocation(Game.LocalPlayer.Character);
                GTAWeapon DamagingWeapon = GetWeaponLastDamagedBy(Game.LocalPlayer.Character);

                bool CanBeFatal = false;
                if (DamagedLocation == BodyLocation.Head || DamagedLocation == BodyLocation.Neck || DamagedLocation == BodyLocation.UpperTorso)
                    CanBeFatal = true;

                bool ArmorWillProtect = false;
                if (DamagedLocation == BodyLocation.UpperTorso)
                    ArmorWillProtect = true;

                InjuryType HealthInjury = RandomType(CanBeFatal);
                InjuryType ArmorInjury = RandomType(false);

                float HealthDamageModifier = 1.0f;

                if (HealthInjury == InjuryType.Fatal)
                    HealthDamageModifier = 10.0f;
                else if (HealthInjury == InjuryType.Normal)
                    HealthDamageModifier = 1.0f;
                else if (HealthInjury == InjuryType.Graze)
                    HealthDamageModifier = 0.25f;
                else if (HealthInjury == InjuryType.Critical)
                    HealthDamageModifier = 2.0f;

                float ArmorDamageModifier = 1.0f;

                if (ArmorInjury == InjuryType.Normal)
                    ArmorDamageModifier = 1.0f;
                else if (ArmorInjury == InjuryType.Graze)
                    ArmorDamageModifier = 0.25f;
                else if (ArmorInjury == InjuryType.Critical)
                    ArmorDamageModifier = 2.0f;

                int NewHealthDamage = Convert.ToInt32(HealthDamage * HealthDamageModifier);

                if (!ArmorWillProtect)
                    NewHealthDamage = Convert.ToInt32((HealthDamage + ArmorDamage) * HealthDamageModifier);

                int NewArmorDamage = 0;
                if (ArmorWillProtect)
                    NewArmorDamage = Convert.ToInt32(ArmorDamage * ArmorDamageModifier);


                if (Health - NewHealthDamage > 0)
                    Game.LocalPlayer.Character.Health = Health - NewHealthDamage;
                else
                    Game.LocalPlayer.Character.Health = 0;

                if (Armor - NewArmorDamage > 0)
                    Game.LocalPlayer.Character.Armor = Armor - NewArmorDamage;
                else
                    Game.LocalPlayer.Character.Armor = 0;

                CurrentHealth = Game.LocalPlayer.Character.Health;
                CurrentArmor = Game.LocalPlayer.Character.Armor;

                Debugging.WriteToLog("Player Damage Detected", string.Format("Location: {0},Weapon: {1},{2}, Type: {3}, Total Damage: {4}, HealthDamage: {5}, ArmorDamage: {6},NewHealthDamage: {7}, NewArmorDamage: {8}, DamageModifier: {9},ArmorWillProtect: {10}",
                                                                            DamagedLocation, DamagingWeapon.Name, DamagingWeapon.Category, HealthInjury, TotalDamage, HealthDamage, ArmorDamage, NewHealthDamage, NewArmorDamage, HealthDamageModifier, ArmorWillProtect));
                //if(ArmorWillProtect)
                //    UI.DebugLine = string.Format("{0} ap hit at {1}", HealthInjury, DamagedLocation);
                //else
                //    UI.DebugLine = string.Format("{0} hit at {1}", HealthInjury, DamagedLocation);

                GameTimeLastDamaged = Game.GameTime;
            }

            if (Health != CurrentHealth)
            {
                PlayerHealthChanged(CurrentHealth);
            }

            if (Armor != CurrentArmor)
            {
                PlayerArmorChanged(CurrentArmor);
            }

            CheckBleeding();

            if (IsHealing)
            {
                if (Game.GameTime - GameTimeLastHealed >= 5000)
                {
                    int RandomNumber = General.MyRand.Next(1, 5);
                    if (Game.LocalPlayer.Character.Health < Game.LocalPlayer.Character.MaxHealth)

                        Game.LocalPlayer.Character.Health += RandomNumber;

                    GameTimeLastHealed = Game.GameTime;
                }
            }

            //if(Game.GameTime - GameTimeLastDamaged >= 15000)
            //{
            //    UI.DebugLine = "";
            //}
        }
    }
    private static void CheckBleeding()
    {
        if (IsBleeding && Game.GameTime - GameTimeLastBled >= 3500)
        {
            if (Game.LocalPlayer.Character.IsRunning)
                Health -= 10;
            else if (Game.LocalPlayer.Character.IsWalking)
                Health -= 4;
            else
                Health -= 2;

            Game.LocalPlayer.Character.Health = Health;
            GameTimeLastBled = Game.GameTime;

            Debugging.WriteToLog("IsBleeding", string.Format("Was {0},Now {1}", Health + 2, Health));
        }

        if (PrevIsBleeding != IsBleeding)
        {
            BleedingChanged();
        }
    }
    private static InjuryType RandomType(bool CanBeFatal)
    {
        int RandomNumber = General.MyRand.Next(1, 101);
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
        foreach(GTAWeapon MyWeapon in Weapons.WeaponsList)
        {
            if(NativeFunction.CallByName<bool>("HAS_PED_BEEN_DAMAGED_BY_WEAPON", Pedestrian, MyWeapon.Hash, 0))
            {
                NativeFunction.CallByName<bool>("CLEAR_PED_LAST_WEAPON_DAMAGE", Pedestrian);
                return MyWeapon;
            }
        }
        return new GTAWeapon("Unknown",0,GTAWeapon.WeaponCategory.Unknown,0,0,false,false,false,false);
    }
    private static void BleedingChanged()
    {
        if(IsBleeding) //started bleeding
        {
            //NativeFunction.Natives.x80C8B1846639BB19(1);
            //NativeFunction.Natives.x2206BF9A37B7F724("DrugsDrivingIn", 0, false);//_START_SCREEN_EFFECT
        }
        else
        {
            //NativeFunction.Natives.xB4EDDC19532BFB85();//ANIMPOSTFX_STOP_ALL
            //NativeFunction.Natives.x80C8B1846639BB19(0);

            //if(Health > 0)
            //    NativeFunction.Natives.x2206BF9A37B7F724("MP_corona_switch", 0, false);//_START_SCREEN_EFFECT
        }
        PrevIsBleeding = IsBleeding;
        Debugging.WriteToLog("BleedingChanged", string.Format("Bleeding: {0}", IsBleeding));
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
            if(DamagedOne != null)
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
    private static void PlayerHealthChanged(int NewHealth)
    {
        if(NewHealth == 0)
        {
            IsBleeding = false;
            Debugging.WriteToLog("PlayerHealthChanged", string.Format("Died"));
        }
        else if(NewHealth > Health)
        {
            //IsBleeding = false;
            Debugging.WriteToLog("PlayerHealthChanged", string.Format("Healed"));
        }
        else// Health went down, you got hurt
        {
            if (Health - NewHealth >= 65)
            {
                NativeFunction.CallByName<bool>("SET_PED_TO_RAGDOLL",Game.LocalPlayer.Character,3000,3000,0,false,false,false);
                IsBleeding = true;
                Debugging.WriteToLog("PlayerHealthChanged", string.Format("Critical Hit, Ragdoll"));
            }
            else if (Health - NewHealth >= 35 && General.RandomPercent(60))
            {
                NativeFunction.CallByName<bool>("SET_PED_TO_RAGDOLL", Game.LocalPlayer.Character, 1500, 1500, 1, false, false, false);
                IsBleeding = true;
                Debugging.WriteToLog("PlayerHealthChanged", string.Format("Critical Hit, Ragdoll"));
            }
            else if (Health - NewHealth >= 15 && General.RandomPercent(30))
            {
                NativeFunction.CallByName<bool>("SET_PED_TO_RAGDOLL", Game.LocalPlayer.Character, 1500, 1500, 1, false, false, false);
                IsBleeding = true;
                Debugging.WriteToLog("PlayerHealthChanged", string.Format("Critical Hit, Ragdoll"));
            }
            else if(Health - NewHealth >= 10)
            {
                IsBleeding = true;
                Debugging.WriteToLog("PlayerHealthChanged", string.Format("Normal Hit, Bleeding"));
            }
        }
        Debugging.WriteToLog("PlayerHealthChanged", string.Format("Was {0},Now {1}", Health, NewHealth));
        Health = NewHealth;
    }
    private static void PlayerArmorChanged(int NewArmor)
    {
        if (NewArmor > Armor)
        {
            Debugging.WriteToLog("PlayerArmorChanged", string.Format("Got Armor"));
        }
        else
        {
            if (Armor - NewArmor >= 50f)
            {
                NativeFunction.CallByName<bool>("SET_PED_TO_RAGDOLL", Game.LocalPlayer.Character, 1000, 1000, 3, false, false, false);
                Debugging.WriteToLog("PlayerArmorChanged", string.Format("Critical Armor Hit, Stumble"));
            }
            else if (Armor - NewArmor >= 25 && General.RandomPercent(40))
            {
                NativeFunction.CallByName<bool>("SET_PED_TO_RAGDOLL", Game.LocalPlayer.Character, 500, 500, 1, false, false, false);
                Debugging.WriteToLog("PlayerArmorChanged", string.Format("Critical Armor Hit, Stumble"));
            }

        }
        Debugging.WriteToLog("PlayerArmorChanged", string.Format("Was {0},Now {1}", Armor, NewArmor));
        Armor = NewArmor;

    }
    public static void BandagePed(Ped PedToBandage)
    {
        if (IsBandaging || !IsBleeding)
            return;

        if (Game.LocalPlayer.Character.IsRagdoll || Game.LocalPlayer.Character.IsSwimming || Game.LocalPlayer.Character.IsInCover)//tons more should probably be checked
        {
            return;
        }

        GameFiber Bandaging = GameFiber.StartNew(delegate
        {
            IsBandaging = true;
            bool PlayingAnimation = false;
            if (!PedToBandage.IsInAnyVehicle(false))
            {  
                General.RequestAnimationDictionay("move_p_m_two_idles@generic");
                NativeFunction.CallByName<bool>("SET_CURRENT_PED_WEAPON", PedToBandage, (uint)2725352035, true);
                NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", PedToBandage, "move_p_m_two_idles@generic", "fidget_pick_at_face", 8.0f, -8.0f, 2000, 1, 0, false, false, false);
                PlayingAnimation = true;
                Debugging.WriteToLog("BandagePed", string.Format("Started Bandaging Animation"));
            }
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
                Debugging.WriteToLog("BandagePed", string.Format("Finished, Not Bleeding"));
                IsBleeding = false;

                //NativeFunction.Natives.x2206BF9A37B7F724("DrugsDrivingOut", 3500, 0);//_START_SCREEN_EFFECT

            }
            else
            {
                Debugging.WriteToLog("BandagePed", string.Format("Interrupted"));
            }

            if(PlayingAnimation)
                PedToBandage.Tasks.Clear();

            IsBandaging = false;
            
        }, "Bandaging");
        Debugging.GameFibers.Add(Bandaging);
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
}

