using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Mod;
using Rage;
using Rage.Native;
using System;
using System.Drawing;
using System.Linq;

public class Cop : PedExt, IWeaponIssuable, IPlayerChaseable, IAIChaseable
{
    private bool IsSetStayInVehicle;
    private bool WasAlreadySetPersistent = false;
    private bool IsShootingCheckerActive;
    private uint GameTimeFirstSawPlayerViolating;
    private uint GameTimeLastRagdolled;
    private bool prevIsRespondingToInvestigation = false;


    public Cop(Ped pedestrian, ISettingsProvideable settings, int health, Agency agency, bool wasModSpawned, ICrimes crimes, IWeapons weapons, string name, string modelName, IEntityProvideable world) : base(pedestrian, settings, crimes, weapons, name, "Cop", world)
    {
        IsCop = true;
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
        AssistManager = new CopAssistManager(this);
        PedBrain = new CopBrain(this, Settings, world, weapons);
    }
    public IssuableWeapon GetRandomMeleeWeapon(IWeapons weapons) => AssignedAgency.GetRandomMeleeWeapon(weapons);
    public IssuableWeapon GetRandomWeapon(bool v, IWeapons weapons) => AssignedAgency.GetRandomWeapon(v, weapons);
    public Agency AssignedAgency { get; set; } = new Agency();
    public override Color BlipColor => AssignedAgency != null ? AssignedAgency.Color : base.BlipColor;
    public string CopDebugString => WeaponInventory.DebugWeaponState;
    public virtual bool ShouldBustPlayer => !IsPlayerControlled && !IsInVehicle && DistanceToPlayer > 0.1f && HeightToPlayer <= 2.5f && !IsUnconscious && !IsInWrithe && DistanceToPlayer <= Settings.SettingsManager.PoliceSettings.BustDistance && Pedestrian.Exists() && !Pedestrian.IsRagdoll;
    public bool IsIdleTaskable => WasModSpawned || !WasAlreadySetPersistent;
    public bool ShouldUpdateTarget => Game.GameTime - GameTimeLastUpdatedTarget >= Settings.SettingsManager.PoliceTaskSettings.TargetUpdateTime;
    public string ModelName { get; set; }
    public override ePedAlertType PedAlertTypes { get; set; } = ePedAlertType.UnconsciousBody | ePedAlertType.HelpCry | ePedAlertType.DeadBody | ePedAlertType.GunShot;
    public bool CanRadioInWanted => SawPlayerViolating &&!IsUnconscious && !IsDead && !IsInWrithe && !IsBeingHeldAsHostage && !RecentlyRagdolled && GameTimeFirstSawPlayerViolating > 0 && Game.GameTime - GameTimeFirstSawPlayerViolating >= Settings.SettingsManager.PoliceSettings.RadioInTime && Pedestrian.Exists() && Pedestrian.Exists() && !Pedestrian.IsRagdoll;
    public override bool CanPlayRadioInAnimation => WeaponInventory.CanRadioIn;
    public bool SawPlayerViolating { get; private set; }
    public override int ShootRate { get; set; } = 500;
    public override int Accuracy { get; set; } = 40;
    public override int CombatAbility { get; set; } = 1;
    public override int TaserAccuracy { get; set; } = 30;
    public override int TaserShootRate { get; set; } = 100;
    public override int VehicleAccuracy { get; set; } = 10;
    public override int VehicleShootRate { get; set; } = 20;
    public override int TurretAccuracy { get; set; } = 30;
    public override int TurretShootRate { get; set; } = 1000;
    public override bool AutoCallsInUnconsciousPeds { get; set; } = true;
    public override int CollideWithPlayerLimit => 1;
    public override int PlayerStandTooCloseLimit => 1;
    public override int InsultLimit => 2;
    public override string BlipName => "Police";
    public CopAssistManager AssistManager { get; private set;}
    public CopVoice Voice { get; private set; }
    public WeaponInventory WeaponInventory { get; private set; }
    public bool IsRespondingToInvestigation { get; set; }
    public bool IsRespondingToWanted { get; set; }
    public bool IsRespondingToCitizenWanted { get; set; }
    public bool IsRespondingToAPB { get; set; }
    public bool HasTaser { get; set; } = false;
    public int Division { get; set; } = -1;
    public virtual string UnitType { get; set; } = "Lincoln";
    public int BeatNumber { get; set; } = 1;
    public bool RecentlyRagdolled => GameTimeLastRagdolled > 0 && Game.GameTime - GameTimeLastRagdolled <= 3000;
    public uint GameTimeLastUpdatedTarget { get; set; }
    public override bool KnowsDrugAreas => false;
    public override bool KnowsGangAreas => true;
    public bool IsUsingMountedWeapon { get; set; } = false;
    public PedExt CurrentTarget { get; set; }
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
    public bool IsShooting { get; private set; }

    public bool IsMarshalTaskForceMember { get; set; } = false;
    public bool IsRoadblockSpawned { get; set; } = false;
    public bool IsOffDuty { get; set; } = false;


    public override void MatchPlayerPedType(IPedSwappable Player)
    {
        Player.RemoveAgencyStatus();
        Player.SetAgencyStatus(AssignedAgency);
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
                if (Pedestrian.IsRagdoll)
                {
                    GameTimeLastRagdolled = Game.GameTime;
                }
                IsInWrithe = Pedestrian.IsInWrithe;
                UpdatePositionData();
                PlayerPerception.Update(perceptable, placeLastSeen);
                UpdateVehicleState();
                UpdateCombatFlags();
                if (!IsUnconscious && PlayerPerception.DistanceToTarget <= 200f)
                {
                    if (!PlayerPerception.RanSightThisUpdate && !Settings.SettingsManager.PerformanceSettings.EnableIncreasedUpdateMode)
                    {
                        GameFiber.Yield();
                    }
                    if (Settings.SettingsManager.PoliceSettings.AllowShootingInvestigations && !IsShootingCheckerActive)//Need Frame Perfect checking on cops shooting for stealth
                    {
                        ShootingChecker();
                    }
                    if (Settings.SettingsManager.PoliceSettings.AllowAlerts)
                    {
                        PedAlerts.Update(policeRespondable, world);
                    }
                    //UpdateAlerts(perceptable, policeRespondable, world);
                    PlayerViolationChecker(policeRespondable, world);


                    if(prevIsRespondingToInvestigation != IsRespondingToInvestigation)
                    {
                        OnStartedRespondingToInvestigation();
                        prevIsRespondingToInvestigation = IsRespondingToInvestigation;
                    }

                }
                GameTimeLastUpdated = Game.GameTime;
            }
        }
        CurrentHealthState.Update(policeRespondable);//has a yield if they get damaged, seems ok 
    }
    private void OnStartedRespondingToInvestigation()
    {
        if (IsRespondingToInvestigation)
        {
            if (IsInVehicle && !WeaponInventory.HasHeavyWeaponOnPerson)
            {
                if (PlayerToCheck.Investigation.InvestigationWantedLevel >= 3 || (PlayerToCheck.Investigation.InvestigationWantedLevel == 2 && RandomItems.RandomPercent(45)))
                {
                    WeaponInventory.GiveHeavyWeapon();
                    EntryPoint.WriteToConsole("Responding to Investigation, Giving Heavy Weapon");
                }
            }
        }
    }
    public void UpdateSpeech(IPoliceRespondable currentPlayer)
    {
        Voice.Speak(currentPlayer);
    }
    //public void ForceSpeech(IPoliceRespondable currentPlayer)
    //{
    //    Voice.ResetSpeech();
    //    Voice.Speak(currentPlayer);
    //}
    public void SetStats(DispatchablePerson dispatchablePerson, IWeapons Weapons, bool addBlip, string forceGroupName, float sightDistance)
    {
        if (!Pedestrian.Exists())
        {
            return;
        }
        dispatchablePerson.SetPedExtPermanentStats(this, Settings.SettingsManager.PoliceSettings.OverrideHealth, Settings.SettingsManager.PoliceSettings.OverrideArmor, Settings.SettingsManager.PoliceSettings.OverrideAccuracy);
        if (!Pedestrian.Exists())
        {
            return;
        }
        if (!IsAnimal)
        {
            WeaponInventory.IssueWeapons(Weapons, true, true, true, dispatchablePerson);
            GameFiber.Yield();
        }
        if (!Pedestrian.Exists())
        {
            return;
        }
        if (AssignedAgency.Division != -1)
        {
            Division = AssignedAgency.Division;
            UnitType = forceGroupName;
            BeatNumber = AssignedAgency.GetNextBeatNumber();
            GroupName = $"{AssignedAgency.ID} {Division}-{UnitType}-{BeatNumber}";
        }
        else if (AssignedAgency.MemberName != "")
        {
            GroupName = AssignedAgency.MemberName;
        }
        else
        {
            GroupName = "Cop";
        }
        GameFiber.Yield();
        if (!Pedestrian.Exists())
        {
            return;
        }
        if (addBlip)
        {
            AddBlip();
        }
        if (IsAnimal)
        {
            return;
        }
        //return;
        if (Settings.SettingsManager.PoliceSettings.ForceDefaultWeaponAnimations)
        {
            NativeFunction.Natives.SET_WEAPON_ANIMATION_OVERRIDE(Pedestrian, Game.GetHashKey("Default"));
        }
        if(Settings.SettingsManager.PoliceTaskSettings.EnableCombatAttributeCanInvestigate)
        {
            NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Pedestrian, (int)eCombatAttributes.CA_CAN_INVESTIGATE, true);
        }
        if (Settings.SettingsManager.PoliceTaskSettings.EnableCombatAttributeCanChaseOnFoot)
        {
            NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Pedestrian, (int)eCombatAttributes.CA_CAN_CHASE_TARGET_ON_FOOT, true);
        }
        if (Settings.SettingsManager.PoliceTaskSettings.EnableCombatAttributeCanFlank)
        {
            NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Pedestrian, (int)eCombatAttributes.CA_CAN_FLANK, true);
        }
        if (Settings.SettingsManager.PoliceTaskSettings.EnableCombatAttributeDisableEntryReactions)
        {
            NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Pedestrian, (int)eCombatAttributes.CA_DISABLE_ENTRY_REACTIONS, true);
        }
        if(Settings.SettingsManager.PoliceTaskSettings.OverrrideTargetLossResponse)
        {
            NativeFunction.Natives.SET_PED_TARGET_LOSS_RESPONSE(Pedestrian, Settings.SettingsManager.PoliceTaskSettings.OverrrideTargetLossResponseValue);
        }
        if(Settings.SettingsManager.PoliceTaskSettings.EnableConfigFlagAlwaysSeeAproachingVehicles)
        {
            NativeFunction.Natives.SET_PED_CONFIG_FLAG(Pedestrian, (int)171, true);
        }
        if (Settings.SettingsManager.PoliceTaskSettings.EnableConfigFlagDiveFromApproachingVehicles)
        {
            NativeFunction.Natives.SET_PED_CONFIG_FLAG(Pedestrian, (int)172, true);
        }
        if(Settings.SettingsManager.PoliceTaskSettings.AllowMinorReactions)
        {
            NativeFunction.Natives.SET_PED_ALLOW_MINOR_REACTIONS_AS_MISSION_PED(Pedestrian, true);
        }
        if(Settings.SettingsManager.PoliceTaskSettings.StopWeaponFiringWhenDropped)
        {
            NativeFunction.Natives.STOP_PED_WEAPON_FIRING_WHEN_DROPPED(Pedestrian);
        }
        if (sightDistance > 60f)
        {
            NativeFunction.Natives.SET_PED_SEEING_RANGE(Pedestrian, sightDistance);
        }
    }
    private void UpdateCombatFlags()
    {
        if (StayInVehicle && IsUsingMountedWeapon)
        {
            if (!IsSetStayInVehicle)
            {
                NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Pedestrian, (int)eCombat_Attribute.CA_LEAVE_VEHICLES, false);
                IsSetStayInVehicle = true;
                //EntryPoint.WriteToConsoleTestLong($"COP {Handle} SET CA_LEAVE_VEHICLES FALSE");
            }
        }
        else
        {
            if(IsSetStayInVehicle)
            {
                NativeFunction.Natives.SET_PED_COMBAT_ATTRIBUTES(Pedestrian, (int)eCombat_Attribute.CA_LEAVE_VEHICLES, true);
                IsSetStayInVehicle = false;
                //EntryPoint.WriteToConsoleTestLong($"COP {Handle} SET CA_LEAVE_VEHICLES TRUE");
            }
        }
    }
    protected override void OnHitInsultLimit(IInteractionable player)
    {
        PlayerPerception.SetFakeSeen();
        player.SetAngeredCop();
        EntryPoint.WriteToConsole($"OnHitInsultLimit triggered {Handle}");
    }
    protected override void OnHitCollideWithPlayerLimit(IInteractionable player)
    {
        PlayerPerception.SetFakeSeen();
        player.SetAngeredCop();
        EntryPoint.WriteToConsole($"OnHitCollideWithPlayerLimit triggered {Handle}");
    }
    protected override void OnHitPlayerStoodTooCloseLimit(IInteractionable player)
    {
        PlayerPerception.SetFakeSeen();
        player.SetAngeredCop();
        EntryPoint.WriteToConsole($"OnHitPlayerStoodTooCloseLimit triggered {Handle}");
    }

    public override void OnPlayerDamagedCarOnFoot(IInteractionable player)
    {
        PlayerPerception.SetFakeSeen();
        player.SetAngeredCop();
        EntryPoint.WriteToConsole($"OnPlayerDamagedCarOnFoot triggered {Handle}");
    }
    public override void OnPlayerDidBodilyFunctionsNear(IInteractionable player)
    {
        player.SetAngeredCop();
        EntryPoint.WriteToConsole($"OnPlayerDidBodilyFunctionsNear triggered {Handle}");
    }
    public override void OnPlayerStoodOnCar(IInteractionable player)
    {
        PlayerPerception.SetFakeSeen();
        player.SetAngeredCop();
        EntryPoint.WriteToConsole($"OnPlayerStoodOnCar triggered {Handle}");
    }
    protected override string StealID(IInteractionable player, IModItems modItems)
    {
        EntryPoint.WriteToConsole($"STEAL COP ID START");
        if (!HasIdentification)
        {
            EntryPoint.WriteToConsole($"STEAL COP ID NO ID");
            return "";
        }
        HasIdentification = false;
        ValuableItem idITem = modItems.PossibleItems.ValuableItems.FirstOrDefault(x => x.Name.ToLower() == "police id card");
        if (idITem == null)
        {
            idITem = modItems.PossibleItems.ValuableItems.FirstOrDefault(x => x.Name.ToLower() == "drivers license");
            if (idITem == null)
            {
                EntryPoint.WriteToConsole($"STEAL COP ID NO MOD ITEM");
                return "";
            }
        }
        player.Inventory.Add(idITem, 1.0f);
        return $"~n~~p~{idITem.Name}~s~";
    }

    private void PlayerViolationChecker(IPoliceRespondable policeRespondable, IEntityProvideable world)
    {
        if(policeRespondable.IsNotWanted && policeRespondable.PoliceResponse.HasBeenNotWantedFor >= 2000 && SawPlayerViolating)
        {
            SawPlayerViolating = false;
            GameTimeFirstSawPlayerViolating = 0;
            return;
        }
        if(policeRespondable.IsNotWanted)
        {
            return;
        }
        if (CanSeePlayer && !SawPlayerViolating)
        {
            OnSawPlayerViolating();
        }
        if(policeRespondable.PoliceResponse.WantedLevelHasBeenRadioedIn)
        {
            return;
        }
        if (CanRadioInWanted)
        {
            Voice.RadioInWanted(policeRespondable);
            policeRespondable.PoliceResponse.RadioInWanted();
            EntryPoint.WriteToConsole($"I AM {Handle} AND I RADIOED IN THE WANTED LEVEL");
        }
    }
    private void OnSawPlayerViolating()
    {
        GameTimeFirstSawPlayerViolating = Game.GameTime;
        SawPlayerViolating = true;
        //EntryPoint.WriteToConsole($"I AM {Handle} AND I SAW PLAYER VIOLATING");
    }
    private void ShootingChecker()
    {
        if (!IsShootingCheckerActive)
        {
            GameFiber.Yield();//TR Yield add 1
            GameFiber.StartNew(delegate
            {
                try
                {
                    IsShootingCheckerActive = true;
                    //EntryPoint.WriteToConsole($"        Ped {PedExt.Pedestrian.Handle} IsShootingCheckerActive {IsShootingCheckerActive}", 5);
                    uint GameTimeLastShot = 0;
                    while (Pedestrian.Exists() && !IsDead && !IsUnconscious && IsShootingCheckerActive && EntryPoint.ModController?.IsRunning == true)// && !policeRespondable.PoliceResponse.WantedLevelHasBeenRadioedIn)// && CarryingWeapon && IsShootingCheckerActive && ObservedWantedLevel < 3)
                    {
                        if (Pedestrian.IsShooting && Pedestrian.Inventory.EquippedWeapon != null && (uint)Pedestrian.Inventory.EquippedWeapon.Hash != 911657153)
                        {
                            IsShooting = true;
                            GameTimeLastShot = Game.GameTime;
                        }
                        else if (Game.GameTime - GameTimeLastShot >= 5000)
                        {
                            IsShooting = false;
                        }
                        GameFiber.Yield();
                    }
                    IsShootingCheckerActive = false;
                }
                catch (Exception ex)
                {
                    EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                    //EntryPoint.ModController.CrashUnload();
                }
            }, "Ped Shooting Checker");
        }
    }

    protected override string GetPedInfoForDisplay()
    {
        string ExtraItems = base.GetPedInfoForDisplay();
        if (AssignedAgency != null)
        {
            ExtraItems += $"~n~Cop: {AssignedAgency.ShortName}";
        }
        return ExtraItems;
    }
    public override void OnDeath(IPoliceRespondable policeRespondable)
    {
        AddPossibleMIA(policeRespondable);
        base.OnDeath(policeRespondable);
    }
    public override void OnUnconscious(IPoliceRespondable policeRespondable)
    {
        AddPossibleMIA(policeRespondable);
        base.OnUnconscious(policeRespondable);
    }
    private void AddPossibleMIA(IPoliceRespondable policeRespondable)
    {
        if (policeRespondable.IsWanted && policeRespondable.PoliceResponse.WantedLevelHasBeenRadioedIn)
        {
            EntryPoint.WriteToConsole($"AddPossibleMIA {Handle} WANTED FAIL");
            return;
        }
        if(PedAlerts.IsAlerted)
        {
            if(!RandomItems.RandomPercent(Settings.SettingsManager.WorldSettings.OfficerMIAStartPercentage_Alterted))
            {
                EntryPoint.WriteToConsole($"AddPossibleMIA {Handle} IsAlerted{PedAlerts.IsAlerted} PERCENTAGE FAIL");
                return;
            }
        }
        else if (!RandomItems.RandomPercent(Settings.SettingsManager.WorldSettings.OfficerMIAStartPercentage_Regular))
        {
            EntryPoint.WriteToConsole($"AddPossibleMIA {Handle} IsAlerted{PedAlerts.IsAlerted} PERCENTAGE FAIL");
            return;
        }
        policeRespondable.OfficerMIAWatcher.AddMIA(this, Position);
        EntryPoint.WriteToConsole($"AddPossibleMIA {Handle} IsAlerted{PedAlerts.IsAlerted}");
    }
    public override string InteractPrompt(IButtonPromptable player)
    {
        return $"Talk to {FormattedName}";
    }
    public override void OnHeardGunfire(IPoliceRespondable policeRespondable)
    {
        if(policeRespondable.Investigation.IsActive)
        {
            policeRespondable.Investigation.ExtendPoliceTime();
        }
    }

    public override void OnSeenDeadBody(IPoliceRespondable policeRespondable)
    {
        if (policeRespondable.Investigation.IsActive)
        {
            policeRespondable.Investigation.ExtendPoliceTime();
        }
    }

    //public void AddDivision(string forceGroupName)
    //{
    //    if (AssignedAgency.Division != -1)
    //    {
    //        Division = AssignedAgency.Division;
    //        UnitType = forceGroupName;
    //        BeatNumber = AssignedAgency.GetNextBeatNumber();
    //        GroupName = $"{AssignedAgency.ID} {Division}-{UnitType}-{BeatNumber}";
    //    }
    //    else if (AssignedAgency.MemberName != "")
    //    {
    //        GroupName = AssignedAgency.MemberName;
    //    }
    //    else
    //    {
    //        GroupName = "Cop";
    //    }
    //}

    //public void SetPedExtPermanentStats(DispatchablePerson dispatchablePerson, bool overrideHealth, bool overrideArmor, bool overrideAccuracy)
    //{
    //    if(dispatchablePerson == null)
    //    {
    //        return;
    //    }
    //    Accuracy = RandomItems.GetRandomNumberInt(dispatchablePerson.AccuracyMin, dispatchablePerson.AccuracyMax);
    //    ShootRate = RandomItems.GetRandomNumberInt(dispatchablePerson.ShootRateMin, dispatchablePerson.ShootRateMax);
    //    CombatAbility = RandomItems.GetRandomNumberInt(dispatchablePerson.CombatAbilityMin, dispatchablePerson.CombatAbilityMax);
    //    CombatMovement = CombatMovement;
    //    CombatRange = CombatRange;
    //    TaserAccuracy = RandomItems.GetRandomNumberInt(dispatchablePerson.TaserAccuracyMin, dispatchablePerson.TaserAccuracyMax);
    //    TaserShootRate = RandomItems.GetRandomNumberInt(dispatchablePerson.TaserShootRateMin, dispatchablePerson.TaserShootRateMax);
    //    VehicleAccuracy = RandomItems.GetRandomNumberInt(dispatchablePerson.VehicleAccuracyMin, dispatchablePerson.VehicleAccuracyMax);
    //    VehicleShootRate = RandomItems.GetRandomNumberInt(dispatchablePerson.VehicleShootRateMin, dispatchablePerson.VehicleShootRateMax);
    //    TurretAccuracy = RandomItems.GetRandomNumberInt(dispatchablePerson.TurretAccuracyMin, dispatchablePerson.TurretAccuracyMax);
    //    TurretShootRate = RandomItems.GetRandomNumberInt(dispatchablePerson.TurretShootRateMin, dispatchablePerson.TurretShootRateMax);
    //    if (AlwaysHasLongGun)
    //    {
    //        EntryPoint.WriteToConsole($"COP {Handle} AlwaysHasLongGun");
    //        AlwaysHasLongGun = true;
    //    }
    //    if (dispatchablePerson.OverrideVoice != null && dispatchablePerson.OverrideVoice.Any())
    //    {
    //        EntryPoint.WriteToConsole($"COP {Handle} VoiceName");
    //        VoiceName = dispatchablePerson.OverrideVoice.PickRandom();
    //    }
    //    if (!Pedestrian.Exists())
    //    {
    //        return;
    //    }



    //    if (dispatchablePerson.DisableBulletRagdoll)
    //    {
    //        EntryPoint.WriteToConsole($"COP {Handle} DisableBulletRagdoll");
    //        NativeFunction.Natives.SET_PED_CONFIG_FLAG(Pedestrian, (int)107, true);//PCF_DontActivateRagdollFromBulletImpact		= 107,  // Blocks ragdoll activation when hit by a bullet
    //    }


    //    if (dispatchablePerson.DisableCriticalHits)
    //    {
    //        EntryPoint.WriteToConsole($"COP {Handle} DisableCriticalHits");
    //        NativeFunction.Natives.SET_PED_SUFFERS_CRITICAL_HITS(Pedestrian, false);
    //    }
    //    HasFullBodyArmor = HasFullBodyArmor;
    //    if (dispatchablePerson.FiringPatternHash != 0)
    //    {
    //        EntryPoint.WriteToConsole($"COP {Handle} FiringPatternHash");
    //        NativeFunction.Natives.SET_PED_FIRING_PATTERN(Pedestrian, dispatchablePerson.FiringPatternHash);
    //    }




    //    if (overrideHealth)
    //    {
    //        EntryPoint.WriteToConsole($"COP {Handle} health");
    //        int health = RandomItems.GetRandomNumberInt(dispatchablePerson.HealthMin, dispatchablePerson.HealthMax) + 100 + (IsAnimal ? 100 : 0);
    //        Pedestrian.MaxHealth = health;
    //        Pedestrian.Health = health;
    //    }
    //    if (overrideArmor)
    //    {
    //        EntryPoint.WriteToConsole($"COP {Handle} armor");
    //        int armor = RandomItems.GetRandomNumberInt(dispatchablePerson.ArmorMin, dispatchablePerson.ArmorMax);
    //        Pedestrian.Armor = armor;
    //    }



    //    if (overrideAccuracy)
    //    {
    //        EntryPoint.WriteToConsole($"COP {Handle} overrideAccuracy");
    //        Pedestrian.Accuracy = Accuracy;
    //        NativeFunction.Natives.SET_PED_SHOOT_RATE(Pedestrian, ShootRate);
    //        NativeFunction.Natives.SET_PED_COMBAT_ABILITY(Pedestrian, CombatAbility);
    //        if (CombatMovement != -1)
    //        {
    //            NativeFunction.Natives.SET_PED_COMBAT_MOVEMENT(Pedestrian, CombatMovement);
    //            EntryPoint.WriteToConsole($"SET COMBAT MOVEMENT {Handle} {CombatMovement}");
    //        }
    //        if (CombatRange != -1)
    //        {
    //            NativeFunction.Natives.SET_PED_COMBAT_RANGE(Pedestrian, CombatRange);
    //            EntryPoint.WriteToConsole($"SET COMBAT RANGE {Handle} {CombatRange}");
    //        }
    //    }

    //  //  return;




    //    //GameFiber.Yield();
    //    //if (!Pedestrian.Exists())
    //    //{
    //    //    return;
    //    //}


    //    //if (dispatchablePerson.PedConfigFlagsToSet != null && dispatchablePerson.PedConfigFlagsToSet.Any())
    //    //{
    //    //    dispatchablePerson.PedConfigFlagsToSet.ForEach(x => x.ApplyToPed(Pedestrian));
    //    //}
    //    //if (dispatchablePerson.CombatAttributesToSet != null && dispatchablePerson.CombatAttributesToSet.Any())
    //    //{
    //    //    dispatchablePerson.CombatAttributesToSet.ForEach(x => x.ApplyToPed(Pedestrian));
    //    //}
    //    //if (dispatchablePerson.CombatFloatsToSet != null && dispatchablePerson.CombatFloatsToSet.Any())
    //    //{
    //    //    dispatchablePerson.CombatFloatsToSet.ForEach(x => x.ApplyToPed(Pedestrian));
    //    //}
    //}
}