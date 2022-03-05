using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System.Collections.Generic;

public class Cop : PedExt, IWeaponIssuable
{
    private AssistManager AssistManager;
    private uint GameTimeSpawned;
    private ISettingsProvideable Settings;
    private Voice Voice;
    private WeaponInventory WeaponInventory;
    private bool WasAlreadySetPersistent = false;
    public Cop(Ped pedestrian, ISettingsProvideable settings, int health, Agency agency, bool wasModSpawned, ICrimes crimes, IWeapons weapons, string name, string modelName) : base(pedestrian, settings, crimes, weapons, name, "Cop")
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
        if(Pedestrian.IsPersistent)
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
        Voice = new Voice(this, ModelName);
        AssistManager = new AssistManager(this); 
    }
    public IssuableWeapon GetRandomMeleeWeapon(IWeapons weapons) => AssignedAgency.GetRandomMeleeWeapon(weapons);
    public IssuableWeapon GetRandomWeapon(bool v, IWeapons weapons) => AssignedAgency.GetRandomWeapon(v, weapons);
    public Agency AssignedAgency { get; set; } = new Agency();
    public string CopDebugString => WeaponInventory.DebugWeaponState;
    public uint HasBeenSpawnedFor => Game.GameTime - GameTimeSpawned;
    public bool ShouldBustPlayer => !IsInVehicle && DistanceToPlayer > 0.1f && DistanceToPlayer <= Settings.SettingsManager.PoliceSettings.BustDistance;
    public bool IsIdleTaskable => WasModSpawned || !WasAlreadySetPersistent;
    public bool WasModSpawned { get; private set; }
    public void IssueWeapons(IWeapons weapons, bool issueMelee, bool issueSidearm, bool issueLongGun) => WeaponInventory.IssueWeapons(weapons, issueMelee, issueSidearm, issueLongGun);
    public IssuableWeapon Sidearm => WeaponInventory.Sidearm;
    public IssuableWeapon LongGun => WeaponInventory.LongGun;
    public string ModelName { get; set; }
    public int ShootRate { get; set; } = 500;
    public int Accuracy { get; set; } = 40;
    public int CombatAbility { get; set; } = 1;
    public int TaserAccuracy { get; set; } = 30;
    public int TaserShootRate { get; set; } = 100;
    public int VehicleAccuracy { get; set; } = 10;
    public int VehicleShootRate { get; set; } = 20;
    public void RadioIn(IPoliceRespondable currentPlayer) => Voice.RadioIn(currentPlayer);
    public void Speak(IPoliceRespondable currentPlayer) => Voice.Speak(currentPlayer);
    public void UpdateAssists(bool IsWanted) => AssistManager.UpdateCollision(IsWanted);
    public void UpdateLoadout(bool IsPlayerInvehicle, bool IsDeadlyChase, int WantedLevel, bool IsAttemptingToSurrender, bool IsBusted, bool IsWeaponsFree, bool HasShotAtPolice, bool LethalForceAuthorized) => WeaponInventory.UpdateLoadout(IsPlayerInvehicle, IsDeadlyChase, WantedLevel, IsAttemptingToSurrender, IsBusted, IsWeaponsFree, HasShotAtPolice, LethalForceAuthorized);
    public void UpdateSpeech(IPoliceRespondable currentPlayer)
    {
        Speak(currentPlayer);
        if (Settings.SettingsManager.PoliceSettings.AllowRadioInAnimations)
        {
            RadioIn(currentPlayer);
        }
    }
    public void SetCompletlyUnarmed() => WeaponInventory.SetCompletelyUnarmed();
    public void ResetWeaponsState()
    {
        WeaponInventory.Reset();
    }
    public void ForceSpeech(IPoliceRespondable currentPlayer)
    {
        Voice.ResetSpeech();
        Speak(currentPlayer);
    }
}