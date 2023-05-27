using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

public class SecurityGuard : PedExt, IWeaponIssuable, IPlayerChaseable, IAIChaseable
{
    private uint GameTimeSpawned;
    private bool WasAlreadySetPersistent = false;
    public SecurityGuard(Ped pedestrian, ISettingsProvideable settings, int health, Agency agency, bool wasModSpawned, ICrimes crimes, IWeapons weapons, string name, string modelName, IEntityProvideable world) : base(pedestrian, settings, crimes, weapons, name, "Security", world )
    {
        IsCop = false;
        Health = health;
        AssignedAgency = agency;
        WasModSpawned = wasModSpawned;
        ModelName = modelName;
        if (WasModSpawned)
        {
            GameTimeSpawned = Game.GameTime;
        }
        if (Pedestrian.Exists() && Pedestrian.IsPersistent)
        {
            WasAlreadySetPersistent = true;
        }
        if (modelName.ToLower() == "mp_m_freemode_01")
        {
            VoiceName = "S_M_Y_COP_01_WHITE_FULL_01";// "S_M_Y_COP_01";
        }
        else if (modelName.ToLower() == "mp_f_freemode_01")
        {
            VoiceName = "S_F_Y_COP_01_WHITE_FULL_01";// "S_F_Y_COP_01";
        }
        WeaponInventory = new WeaponInventory(this, Settings);
        Voice = new CopVoice(this, ModelName, Settings);
        PedBrain = new SecurityGuardBrain(this, Settings, world, weapons);
    }
    public IssuableWeapon GetRandomMeleeWeapon(IWeapons weapons) => AssignedAgency.GetRandomMeleeWeapon(weapons);
    public IssuableWeapon GetRandomWeapon(bool v, IWeapons weapons) => AssignedAgency.GetRandomWeapon(v, weapons);
    public Agency AssignedAgency { get; set; } = new Agency();
    public override Color BlipColor => AssignedAgency != null ? AssignedAgency.Color : base.BlipColor;
    public string CopDebugString => WeaponInventory.DebugWeaponState;
    public uint HasBeenSpawnedFor => Game.GameTime - GameTimeSpawned;
    public bool ShouldDetainPlayer => !IsInVehicle && DistanceToPlayer > 0.1f && HeightToPlayer <= 2.5f && !IsUnconscious && !IsInWrithe && DistanceToPlayer <= Settings.SettingsManager.SecuritySettings.DetainDistance && Pedestrian.Exists() && !Pedestrian.IsRagdoll;
    public bool IsIdleTaskable => WasModSpawned || !WasAlreadySetPersistent;
    public bool ShouldUpdateTarget => Game.GameTime - GameTimeLastUpdatedTarget >= Settings.SettingsManager.PoliceTaskSettings.TargetUpdateTime;
    public string ModelName { get; set; }
    public override int ShootRate { get; set; } = 500;
    public override int Accuracy { get; set; } = 40;
    public override int CombatAbility { get; set; } = 1;
    public override int TaserAccuracy { get; set; } = 30;
    public override int TaserShootRate { get; set; } = 100;
    public override int VehicleAccuracy { get; set; } = 10;
    public override int VehicleShootRate { get; set; } = 20;
    public override int TurretAccuracy { get; set; } = 10;
    public override int TurretShootRate { get; set; } = 1000;
    public CopVoice Voice { get; private set; }
    public WeaponInventory WeaponInventory { get; private set; }
    public bool IsRespondingToInvestigation { get; set; }
    public bool IsRespondingToWanted { get; set; }
    public bool IsRespondingToCitizenWanted { get; set; }
    public bool HasTaser { get; set; } = false;
    public override bool KnowsDrugAreas => false;
    public override bool KnowsGangAreas => true;
    public uint GameTimeLastUpdatedTarget { get; set; }
    public PedExt CurrentTarget { get; set; }
    public bool IsUsingMountedWeapon { get; set; } = false;
    public override bool WillCallPolice { get; set; } = true;
    public override bool WillCallPoliceIntense { get; set; } = true;
    public override bool NeedsFullUpdate
    {
        get
        {
            if (GameTimeLastUpdated == 0)
            {
                return true;
            }
            else if (Game.GameTime > GameTimeLastUpdated + FullUpdateInterval)// + UpdateJitter)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    private int FullUpdateInterval//dont forget distance and LOS in here
    {
        get
        {
            if (PlayerPerception?.DistanceToTarget >= 300)
            {
                return Settings.SettingsManager.PerformanceSettings.CopUpdateIntervalVeryFar;
            }
            else if (PlayerPerception?.DistanceToTarget >= 200)
            {
                return Settings.SettingsManager.PerformanceSettings.CopUpdateIntervalFar;
            }
            else if (PlayerPerception?.DistanceToTarget >= 50f)
            {
                return Settings.SettingsManager.PerformanceSettings.CopUpdateIntervalMedium;
            }
            else
            {
                return Settings.SettingsManager.PerformanceSettings.CopUpdateIntervalClose;
            }
        }
    }
    public void UpdateSpeech(IPoliceRespondable currentPlayer)
    {
        Voice.Speak(currentPlayer);
    }
    public void ForceSpeech(IPoliceRespondable currentPlayer)
    {
        Voice.ResetSpeech();
        Voice.Speak(currentPlayer);
    }
    public void SetStats(DispatchablePerson dispatchablePerson, IWeapons Weapons, bool addBlip)
    {
        if (!Pedestrian.Exists())
        {
            return;
        }
        Pedestrian.Money = 0;
        dispatchablePerson.SetPedExtPermanentStats(this, Settings.SettingsManager.SecuritySettings.OverrideHealth, Settings.SettingsManager.SecuritySettings.OverrideArmor, Settings.SettingsManager.SecuritySettings.OverrideAccuracy);
        if (!Pedestrian.Exists())
        {
            return;
        }
        WeaponInventory.IssueWeapons(Weapons, true, true, true, dispatchablePerson);
        GameFiber.Yield();
        if (!Pedestrian.Exists())
        {
            return;
        }
        if (string.IsNullOrEmpty(AssignedAgency.MemberName) && AssignedAgency.MemberName != "")
        {
            GroupName = AssignedAgency.MemberName;
        }
        if (!Pedestrian.Exists())
        {
            return;
        }
        if (addBlip)
        {
            AddBlip();
        }
        if (Settings.SettingsManager.SecuritySettings.ForceDefaultWeaponAnimations)
        {
            NativeFunction.Natives.SET_WEAPON_ANIMATION_OVERRIDE(Pedestrian, Game.GetHashKey("Default"));
        }
        if (Settings.SettingsManager.SecuritySettings.EnableCombatAttributeCanInvestigate)
        {
            NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Pedestrian, (int)eCombatAttributes.CA_CAN_INVESTIGATE, true);
        }
        if (Settings.SettingsManager.SecuritySettings.EnableCombatAttributeCanChaseOnFoot)
        {
            NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Pedestrian, (int)eCombatAttributes.CA_CAN_CHASE_TARGET_ON_FOOT, true);
        }
        if (Settings.SettingsManager.SecuritySettings.EnableCombatAttributeCanFlank)
        {
            NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Pedestrian, (int)eCombatAttributes.CA_CAN_FLANK, true);
        }
        if (Settings.SettingsManager.SecuritySettings.EnableCombatAttributeDisableEntryReactions)
        {
            NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Pedestrian, (int)eCombatAttributes.CA_DISABLE_ENTRY_REACTIONS, true);
        }
        if (Settings.SettingsManager.SecuritySettings.OverrrideTargetLossResponse)
        {
            NativeFunction.Natives.SET_PED_TARGET_LOSS_RESPONSE(Pedestrian, Settings.SettingsManager.PoliceTaskSettings.OverrrideTargetLossResponseValue);
        }
        if (Settings.SettingsManager.SecuritySettings.EnableConfigFlagAlwaysSeeAproachingVehicles)
        {
            NativeFunction.Natives.SET_PED_CONFIG_FLAG(Pedestrian, (int)171, true);
        }
        if (Settings.SettingsManager.SecuritySettings.EnableConfigFlagDiveFromApproachingVehicles)
        {
            NativeFunction.Natives.SET_PED_CONFIG_FLAG(Pedestrian, (int)172, true);
        }
        if (Settings.SettingsManager.SecuritySettings.AllowMinorReactions)
        {
            NativeFunction.Natives.SET_PED_ALLOW_MINOR_REACTIONS_AS_MISSION_PED(Pedestrian, true);
        }
        
    }
    public override void Update(IPerceptable perceptable, IPoliceRespondable policeRespondable, Vector3 placeLastSeen, IEntityProvideable world)
    {
        PlayerToCheck = policeRespondable;
        if (!Pedestrian.Exists())
        {
            return;
        }
        if (Pedestrian.IsAlive)
        {
            if (NeedsFullUpdate)
            {
                IsInWrithe = Pedestrian.IsInWrithe;
                UpdatePositionData();
                PlayerPerception.Update(perceptable, placeLastSeen);
                if (Settings.SettingsManager.PerformanceSettings.IsCivilianYield1Active)
                {
                    GameFiber.Yield();//TR TEST 28
                }
                UpdateVehicleState();
                if (!IsUnconscious)
                {
                    if (PlayerPerception.DistanceToTarget <= 200f)// && ShouldCheckCrimes)//was 150 only care in a bubble around the player, nothing to do with the player tho
                    {
                        if (Settings.SettingsManager.PerformanceSettings.IsCivilianYield2Active)//THIS IS THGE BEST ONE?
                        {
                            GameFiber.Yield();//TR TEST 28
                        }
                        if (Settings.SettingsManager.PerformanceSettings.CivilianUpdatePerformanceMode1 && (!PlayerPerception.RanSightThisUpdate || IsGangMember))
                        {
                            GameFiber.Yield();//TR TEST 28
                        }
                        if (ShouldCheckCrimes)
                        {
                            PedViolations.Update(policeRespondable);//possible yield in here!, REMOVED FOR NOW
                        }
                        if (Settings.SettingsManager.PerformanceSettings.IsCivilianYield3Active)
                        {
                            GameFiber.Yield();//TR TEST 28
                        }
                        PedPerception.Update();
                        if (Settings.SettingsManager.PerformanceSettings.IsCivilianYield4Active)
                        {
                            GameFiber.Yield();//TR TEST 28
                        }
                        if (Settings.SettingsManager.PerformanceSettings.CivilianUpdatePerformanceMode2 && (!PlayerPerception.RanSightThisUpdate || IsGangMember))
                        {
                            GameFiber.Yield();//TR TEST 28
                        }
                    }
                    if (Pedestrian.Exists() && policeRespondable.IsCop && !policeRespondable.IsIncapacitated)
                    {
                        CheckPlayerBusted();
                    }
                    if (Settings.SettingsManager.SecuritySettings.AllowCallEMTsOnBodies)
                    {
                        PedAlerts.LookForUnconsciousPeds(world);
                    }
                    if (Settings.SettingsManager.SecuritySettings.AllowReactionsToBodies)
                    {
                        PedAlerts.LookForBodiesAlert(world);
                    }//need to add shooting checker for police and or other security too!
                }
                GameTimeLastUpdated = Game.GameTime;
            }
        }
        CurrentHealthState.Update(policeRespondable);//has a yield if they get damaged, seems ok
    }

}