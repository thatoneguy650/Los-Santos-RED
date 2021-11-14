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
    private bool WasAlreadySetPersistent = false;
    public Cop(Ped pedestrian, ISettingsProvideable settings, int health, Agency agency, bool wasModSpawned, ICrimes crimes, IWeapons weapons) : base(pedestrian, settings, crimes, weapons)
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
        Settings = settings;
        if(Pedestrian.IsPersistent)
        {
            WasAlreadySetPersistent = true;
        }
        WeaponInventory = new WeaponInventory(this, Settings);
        Voice = new Voice(this);
        AssistManager = new AssistManager(this);
        
    }
    public Agency AssignedAgency { get; set; } = new Agency();
    public string CopDebugString => WeaponInventory.DebugWeaponState;
    public uint HasBeenSpawnedFor => Game.GameTime - GameTimeSpawned;
    public bool ShouldBustPlayer => !IsInVehicle && DistanceToPlayer > 0.1f && DistanceToPlayer <= Settings.SettingsManager.PoliceSettings.BustDistance;
    public bool IsIdleTaskable => WasModSpawned || !WasAlreadySetPersistent;
    public bool WasModSpawned { get; private set; }
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