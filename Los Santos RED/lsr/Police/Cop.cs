using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System.Collections.Generic;

public class Cop : PedExt
{
    private AssistManager AssistManager;
    private uint GameTimeSpawned;
    private ISettingsProvideable Settings;
    private Voice Voice;
    private WeaponInventory WeaponInventory;
    public Cop(Ped pedestrian, ISettingsProvideable settings, int health, Agency agency, bool wasModSpawned, ICrimes crimes) : base(pedestrian, settings, crimes)
    {
        IsCop = true;
        Health = health;
        AssignedAgency = agency;
        WasModSpawned = wasModSpawned;
        if (WasModSpawned)
        {
            GameTimeSpawned = Game.GameTime;
        }
        Pedestrian.VisionRange = settings.SettingsManager.PoliceSettings.SightDistance;//55F
        Pedestrian.HearingRange = 55;//25 not really used
        WeaponInventory = new WeaponInventory(this);
        Voice = new Voice(this);
        AssistManager = new AssistManager(this);
        Settings = settings;
    }
    public Agency AssignedAgency { get; set; } = new Agency();
    public string CopDebugString => WeaponInventory.DebugWeaponState;
    public uint HasBeenSpawnedFor => Game.GameTime - GameTimeSpawned;
    public bool ShouldBustPlayer => !IsInVehicle && DistanceToPlayer > 0.1f && DistanceToPlayer <= Settings.SettingsManager.PoliceSettings.BustDistance;
    public bool WasModSpawned { get; private set; }
    //public bool ShouldAutoSetWeaponState => WeaponInventory.ShouldAutoSetWeaponState;
    //public void SetLessLethalWeapons() => WeaponInventory.SetLessLethal();
    //public void SetDeadlyWeapons() => WeaponInventory.SetDeadly();
    //public void SetDefaultWeapons() => WeaponInventory.SetDefault();
    public void IssueWeapons(IWeapons weapons) => WeaponInventory.IssueWeapons(weapons);
    public void RadioIn(IPoliceRespondable currentPlayer) => Voice.RadioIn(currentPlayer);
    public void Speak(IPoliceRespondable currentPlayer) => Voice.Speak(currentPlayer);
    public void UpdateAssists(bool IsWanted) => AssistManager.UpdateCollision(IsWanted);
    public void UpdateLoadout(bool IsDeadlyChase, int WantedLevel) => WeaponInventory.UpdateLoadout(IsDeadlyChase, WantedLevel);
    public void UpdateSpeech(IPoliceRespondable currentPlayer)
    {
        Speak(currentPlayer);
        if (Settings.SettingsManager.PoliceSettings.AllowRadioInAnimations)
        {
            RadioIn(currentPlayer);
        }
    }
}