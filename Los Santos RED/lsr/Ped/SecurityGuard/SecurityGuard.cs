using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Interface;
using Mod;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

public class SecurityGuard : PedExt, IWeaponIssuable, IPlayerChaseable, IAIChaseable
{
    private bool WasAlreadySetPersistent = false;
    public SecurityGuard(Ped pedestrian, ISettingsProvideable settings, int health, Agency agency, bool wasModSpawned, ICrimes crimes, IWeapons weapons, string name, string modelName, IEntityProvideable world) : base(pedestrian, settings, crimes, weapons, name, "Security", world )
    {
        IsCop = false;
        Health = health;
        AssignedAgency = agency;
        WasModSpawned = wasModSpawned;
        ModelName = modelName;
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
    public override ePedAlertType PedAlertTypes { get; set; } = ePedAlertType.UnconsciousBody | ePedAlertType.HelpCry | ePedAlertType.DeadBody | ePedAlertType.GunShot;
    public IssuableWeapon GetRandomMeleeWeapon(IWeapons weapons) => AssignedAgency.GetRandomMeleeWeapon(weapons);
    public IssuableWeapon GetRandomWeapon(bool v, IWeapons weapons) => AssignedAgency.GetRandomWeapon(v, weapons);
    public Agency AssignedAgency { get; set; } = new Agency();
    public override Color BlipColor => AssignedAgency != null ? AssignedAgency.Color : base.BlipColor;
    public string CopDebugString => WeaponInventory.DebugWeaponState;
    public bool ShouldDetainPlayer => !IsPlayerControlled && !IsInVehicle && DistanceToPlayer > 0.1f && HeightToPlayer <= 5f && !IsUnconscious && !IsInWrithe && DistanceToPlayer <= Settings.SettingsManager.SecuritySettings.DetainDistance && Pedestrian.Exists() && !Pedestrian.IsRagdoll;
    public bool IsIdleTaskable => WasModSpawned || !WasAlreadySetPersistent;
    public bool ShouldUpdateTarget => Game.GameTime - GameTimeLastUpdatedTarget >= Settings.SettingsManager.PoliceTaskSettings.TargetUpdateTime;
    public string ModelName { get; set; }
    public override int ShootRate { get; set; } = 500;
    public override string BlipName => "Security Guard";
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
    public override bool HasWeapon => WeaponInventory.HasPistol || WeaponInventory.HasLongGun;
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
        if (Pedestrian.Exists() && Settings.SettingsManager.CivilianSettings.SightDistance > 60f)
        {
            NativeFunction.Natives.SET_PED_SEEING_RANGE(Pedestrian, Settings.SettingsManager.CivilianSettings.SightDistance);
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
                UpdateVehicleState();
                if (!IsUnconscious && PlayerPerception.DistanceToTarget <= 200f)//was 150 only care in a bubble around the player, nothing to do with the player tho
                {
                    if (!PlayerPerception.RanSightThisUpdate && !Settings.SettingsManager.PerformanceSettings.EnableIncreasedUpdateMode)
                    {
                        GameFiber.Yield();
                    }
                    PedPerception.Update();
                    if (Settings.SettingsManager.SecuritySettings.AllowAlerts)
                    {
                        PedAlerts.Update(policeRespondable, world);
                    }
                    //UpdateAlerts(perceptable, policeRespondable, world);    
                }
                GameTimeLastUpdated = Game.GameTime;
            }
        }
        CurrentHealthState.Update(policeRespondable);//has a yield if they get damaged, seems ok
    }

    protected override string GetPedInfoForDisplay()
    {
        string ExtraItems = base.GetPedInfoForDisplay();
        if (AssignedAgency != null)
        {
            ExtraItems += $"~n~Security Guard: {AssignedAgency.ShortName}";
        }
        return ExtraItems;
    }
    public override string InteractPrompt(IButtonPromptable player)
    {
        return $"Talk to {FormattedName}";
    }
    //protected override void UpdateAlerts(IPerceptable perceptable, IPoliceRespondable policeRespondable, IEntityProvideable world)
    //{
    //    if (Settings.SettingsManager.SecuritySettings.AllowCallEMTsOnBodies)
    //    {
    //        PedAlerts.LookForUnconsciousPeds(world);
    //    }
    //    if (Settings.SettingsManager.SecuritySettings.AllowReactionsToBodies)
    //    {
    //        PedAlerts.LookForBodiesAlert(world);
    //    }
    //    if (PedAlerts.HasSeenUnconsciousPed)
    //    {
    //        perceptable.AddMedicalEvent(PedAlerts.PositionLastSeenUnconsciousPed);
    //        PedAlerts.HasSeenUnconsciousPed = false;
    //    }
    //    if (policeRespondable.Violations.WeaponViolations.RecentlyShot && WithinWeaponsAudioRange)
    //    {
    //        PedAlerts.AddHeardGunfire(policeRespondable.Position);
    //    }
    //}

}