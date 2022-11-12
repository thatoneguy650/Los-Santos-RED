using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System.Collections.Generic;

public class Cop : PedExt, IWeaponIssuable
{
    private uint GameTimeSpawned;
    private ISettingsProvideable Settings;
    private bool WasAlreadySetPersistent = false;
    public Cop(Ped pedestrian, ISettingsProvideable settings, int health, Agency agency, bool wasModSpawned, ICrimes crimes, IWeapons weapons, string name, string modelName, IEntityProvideable world) : base(pedestrian, settings, crimes, weapons, name, "Cop", world)
    {
        IsCop = true;
        Health = health;
        AssignedAgency = agency;
        WasModSpawned = wasModSpawned;
        ModelName = modelName;
        if (WasModSpawned)
        {
            GameTimeSpawned = Game.GameTime;
        }
        //Pedestrian.VisionRange = settings.SettingsManager.PoliceSettings.SightDistance;//55F
        //Pedestrian.HearingRange = 55;//25 not really used
        Settings = settings;
        if (Pedestrian.IsPersistent)
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

    }
    public IssuableWeapon GetRandomMeleeWeapon(IWeapons weapons) => AssignedAgency.GetRandomMeleeWeapon(weapons);
    public IssuableWeapon GetRandomWeapon(bool v, IWeapons weapons) => AssignedAgency.GetRandomWeapon(v, weapons);
    public Agency AssignedAgency { get; set; } = new Agency();
    public string CopDebugString => WeaponInventory.DebugWeaponState;
    public uint HasBeenSpawnedFor => Game.GameTime - GameTimeSpawned;
    public bool ShouldBustPlayer => !IsInVehicle && DistanceToPlayer > 0.1f && HeightToPlayer <= 2.5f && !IsUnconscious && !IsInWrithe && DistanceToPlayer <= Settings.SettingsManager.PoliceSettings.BustDistance && Pedestrian.Exists() && !Pedestrian.IsRagdoll;
    public bool IsIdleTaskable => WasModSpawned || !WasAlreadySetPersistent;
    public bool ShouldUpdateTarget => Game.GameTime - GameTimeLastUpdatedTarget >= Settings.SettingsManager.PoliceTaskSettings.TargetUpdateTime;
    public string ModelName { get; set; }
    public int ShootRate { get; set; } = 500;
    public int Accuracy { get; set; } = 40;
    public int CombatAbility { get; set; } = 1;
    public int TaserAccuracy { get; set; } = 30;
    public int TaserShootRate { get; set; } = 100;
    public int VehicleAccuracy { get; set; } = 10;
    public int VehicleShootRate { get; set; } = 20;
    public CopAssistManager AssistManager { get; private set;}
    public CopVoice Voice { get; private set; }
    public WeaponInventory WeaponInventory { get; private set; }
    public bool IsRespondingToInvestigation { get; set; }
    public bool IsRespondingToWanted { get; set; }
    public bool IsRespondingToCitizenWanted { get; set; }
    public bool HasTaser { get; set; } = false;
    public int Division { get; set; } = -1;
    public string UnityType { get; set; } = "Lincoln";
    public int BeatNumber { get; set; } = 1;
    public uint GameTimeLastUpdatedTarget { get; set; }


    public PedExt CurrentTarget { get; set; }

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

    public override void Update(IPerceptable perceptable, IPoliceRespondable policeRespondable, Vector3 placeLastSeen, IEntityProvideable world)
    {
        PlayerToCheck = policeRespondable;
        if (Pedestrian.Exists())
        {
            if (Pedestrian.IsAlive)
            {
                if (NeedsFullUpdate)
                {
                    IsInWrithe = Pedestrian.IsInWrithe;
                    UpdatePositionData();
                    PlayerPerception.Update(perceptable, placeLastSeen);
                    if(Settings.SettingsManager.PerformanceSettings.CopUpdatePerformanceMode1 && !PlayerPerception.RanSightThisUpdate)
                    {
                        GameFiber.Yield();//TR TEST 30
                    }
                    if (Settings.SettingsManager.PerformanceSettings.IsCopYield1Active)
                    {
                        GameFiber.Yield();//TR TEST 30
                    }
                    UpdateVehicleState();
                    if (Settings.SettingsManager.PerformanceSettings.IsCopYield2Active)
                    {
                        GameFiber.Yield();//TR TEST 30
                    }
                    if (Settings.SettingsManager.PerformanceSettings.CopUpdatePerformanceMode2 && !PlayerPerception.RanSightThisUpdate)
                    {
                        GameFiber.Yield();//TR TEST 30
                    }
                    if (Pedestrian.Exists() && Settings.SettingsManager.PoliceSettings.AllowPoliceToCallEMTsOnBodies && !IsUnconscious && !HasSeenDistressedPed && PlayerPerception.DistanceToTarget <= 150f)//only care in a bubble around the player, nothing to do with the player tho
                    {
                        LookForDistressedPeds(world);
                    }
                    if (Settings.SettingsManager.PerformanceSettings.IsCopYield3Active)
                    {
                        GameFiber.Yield();//TR TEST 30
                    }
                    if (HasSeenDistressedPed)
                    {
                        perceptable.AddMedicalEvent(PositionLastSeenDistressedPed);
                        HasSeenDistressedPed = false;
                    }
                    GameTimeLastUpdated = Game.GameTime;
                }
            }
            CurrentHealthState.Update(policeRespondable);//has a yield if they get damaged, seems ok
        }
    }

    public void UpdateSpeech(IPoliceRespondable currentPlayer)
    {
        Voice.Speak(currentPlayer);
        //if (Settings.SettingsManager.PoliceSettings.AllowRadioInAnimations)
        //{
        //    Voice.RadioIn(currentPlayer);
        //}
    }
    public void ForceSpeech(IPoliceRespondable currentPlayer)
    {
        Voice.ResetSpeech();
        Voice.Speak(currentPlayer);
    }

}