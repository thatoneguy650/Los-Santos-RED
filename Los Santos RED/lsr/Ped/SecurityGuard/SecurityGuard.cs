using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;

public class SecurityGuard : PedExt, IWeaponIssuable, IPlayerChaseable, IAIChaseable
{
    private uint GameTimeSpawned;
    private ISettingsProvideable Settings;
    private bool WasAlreadySetPersistent = false;
    public SecurityGuard(Ped pedestrian, ISettingsProvideable settings, int health, Agency agency, bool wasModSpawned, ICrimes crimes, IWeapons weapons, string name, string modelName, IEntityProvideable world) : base(pedestrian, settings, true,true,false,false,name, crimes, weapons, "Security", world,false )
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
        Settings = settings;
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
    public SecurityGuardBrain SecurityGuardTaskManager { get; private set; }
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
    //public override void Update(IPerceptable perceptable, IPoliceRespondable policeRespondable, Vector3 placeLastSeen, IEntityProvideable world)
    //{
    //    PlayerToCheck = policeRespondable;
    //    if (Pedestrian.Exists())
    //    {
    //        if (Pedestrian.IsAlive)
    //        {
    //            if (NeedsFullUpdate)
    //            {
    //                IsInWrithe = Pedestrian.IsInWrithe;
    //                UpdatePositionData();
    //                PlayerPerception.Update(perceptable, placeLastSeen);
    //                if (Settings.SettingsManager.PerformanceSettings.CopUpdatePerformanceMode1 && !PlayerPerception.RanSightThisUpdate)
    //                {
    //                    GameFiber.Yield();//TR TEST 30
    //                }
    //                if (Settings.SettingsManager.PerformanceSettings.IsCopYield1Active)
    //                {
    //                    GameFiber.Yield();//TR TEST 30
    //                }
    //                UpdateVehicleState();
    //                if (Settings.SettingsManager.PerformanceSettings.IsCopYield2Active)
    //                {
    //                    GameFiber.Yield();//TR TEST 30
    //                }
    //                if (Settings.SettingsManager.PerformanceSettings.CopUpdatePerformanceMode2 && !PlayerPerception.RanSightThisUpdate)
    //                {
    //                    GameFiber.Yield();//TR TEST 30
    //                }
    //                if (Pedestrian.Exists() && Settings.SettingsManager.PoliceSettings.AllowPoliceToCallEMTsOnBodies && !IsUnconscious && !HasSeenDistressedPed && PlayerPerception.DistanceToTarget <= 150f)//only care in a bubble around the player, nothing to do with the player tho
    //                {
    //                    LookForDistressedPeds(world);
    //                }
    //                if (Settings.SettingsManager.PerformanceSettings.IsCopYield3Active)
    //                {
    //                    GameFiber.Yield();//TR TEST 30
    //                }
    //                if (HasSeenDistressedPed)
    //                {
    //                    perceptable.AddMedicalEvent(PositionLastSeenDistressedPed);
    //                    HasSeenDistressedPed = false;
    //                }
    //                GameTimeLastUpdated = Game.GameTime;
    //            }
    //        }
    //        CurrentHealthState.Update(policeRespondable);//has a yield if they get damaged, seems ok
    //    }
    //}
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
        dispatchablePerson.SetPedExtPermanentStats(this, Settings.SettingsManager.SecuritySettings.OverrideHealth, Settings.SettingsManager.SecuritySettings.OverrideArmor, Settings.SettingsManager.SecuritySettings.OverrideAccuracy);
        WeaponInventory.IssueWeapons(Weapons, true, true, true, dispatchablePerson);
        if (AssignedAgency.MemberName != "")
        {
            GroupName = AssignedAgency.MemberName;
        }
        if (!Pedestrian.Exists())
        {
            return;
        }
        if (addBlip)
        {
            Blip myBlip = Pedestrian.AttachBlip();
            NativeFunction.Natives.BEGIN_TEXT_COMMAND_SET_BLIP_NAME("STRING");
            NativeFunction.Natives.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME(GroupName);
            NativeFunction.Natives.END_TEXT_COMMAND_SET_BLIP_NAME(myBlip);
            myBlip.Color = AssignedAgency.Color;
            myBlip.Scale = 0.6f;
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
        Pedestrian.Money = 0;
    }
}