using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System.Collections.Generic;

public class Cop : PedExt
{
    private uint GameTimeSpawned;
    private WeaponInventory WeaponInventory;
    private Voice Voice;
    private AssistManager AssistManager;
    private ISettingsProvideable Settings;
    public Cop(Ped pedestrian, ISettingsProvideable settings, int health, Agency agency, bool wasModSpawned) : base(pedestrian, settings)
    {
        IsCop = true;
        Health = health;
        AssignedAgency = agency;
        WasModSpawned = wasModSpawned;
        if (WasModSpawned)
        {
            GameTimeSpawned = Game.GameTime;
        }
        Pedestrian.VisionRange = 90f;//55F
        Pedestrian.HearingRange = 55;//25
        WeaponInventory = new WeaponInventory(this);
        Voice = new Voice(this);
        AssistManager = new AssistManager(this);
        Settings = settings;
    }
    public Agency AssignedAgency { get; set; } = new Agency();
    public uint HasBeenSpawnedFor => Game.GameTime - GameTimeSpawned;
    public bool ShouldBustPlayer => !IsInVehicle && DistanceToPlayer > 0.1f && DistanceToPlayer <= Settings.SettingsManager.PoliceSettings.BustDistance;
    public bool WasModSpawned { get; private set; }
    public void IssueWeapons() => WeaponInventory.IssueWeapons();
    public void UpdateLoadout(bool IsDeadlyChase, int WantedLevel) => WeaponInventory.UpdateLoadout(IsDeadlyChase, WantedLevel);
    public void UpdateAssists(bool IsWanted) => AssistManager.UpdateCollision(IsWanted);
    public void RadioIn(IPoliceRespondable currentPlayer) => Voice.RadioIn(currentPlayer);
    public void Speak(IPoliceRespondable currentPlayer) => Voice.Speak(currentPlayer);
    public void UpdateSpeech(IPoliceRespondable currentPlayer)
    {
        Speak(currentPlayer);
        if (Settings.SettingsManager.PoliceSettings.AllowRadioInAnimations)
        {
            RadioIn(currentPlayer);
        }
    }
}