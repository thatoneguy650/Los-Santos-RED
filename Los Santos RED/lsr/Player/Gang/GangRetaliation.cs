using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GangRetaliation
{
    private uint GameTimeStarted;
    private uint GameTimeEnded;
    private uint GameTimeReturnedToZone;
    private uint GameTimeWarEnded;
    private GangTerritoryManager GangTerritoryManager;
    private IGangTerritoryManageable Player;
    private uint TimeToStartRetaliation;
    private float RetaliationPercentAtIncrement;
    private uint RetaliationTime;
    private uint TimeToReturnToZone;

    public GangRetaliation(IGangTerritoryManageable player, GangTerritoryManager gangTerritoryManager, uint gameTimeWarEnded, Gang targetGang, List<Zone> zonesToAttack)
    {
        Player = player;
        GameTimeWarEnded = gameTimeWarEnded;
        TargetGang = targetGang;
        ZonesToAttack = zonesToAttack;
        GangTerritoryManager = gangTerritoryManager;
    }
    public bool HasRetaliationStarted { get; private set; }
    public bool IsEnded { get; private set; }
    public bool HasPlayerReturnedToZone { get; private set; }
    public Gang TargetGang { get; private set; }
    public List<Zone> ZonesToAttack { get; private set; } = new List<Zone>();

    private bool IsPlayerInZone()
    {
        if (Player.CurrentLocation.CurrentZone == null)
        {
            return false;
        }
        Zone zoneToReturnTo = ZonesToAttack.FirstOrDefault();
        if (zoneToReturnTo == null)
        {
            return false;
        }
        if (Player.CurrentLocation.CurrentZone.InternalGameName.ToLower() == zoneToReturnTo.InternalGameName.ToLower())
        {
            return true;
        }
        return false;
    }
    public void Setup()
    {
        TimeToStartRetaliation = RandomItems.GetRandomNumber(60000, 120000);
        RetaliationPercentAtIncrement = RandomItems.GetRandomNumber(20f, 50f);
        RetaliationTime = RandomItems.GetRandomNumber(120000, 180000);
        TimeToReturnToZone = RandomItems.GetRandomNumber(120000, 180000);
        EntryPoint.WriteToConsole($"GANG RETALIATION SETUP: TimeToStartRetaliation:{TimeToStartRetaliation} RetaliationPercentAtIncrement:{RetaliationPercentAtIncrement} RetaliationTime:{RetaliationTime} TimeToReturnToZone:{TimeToReturnToZone}");
    }
    private void CheckRetaliationStart()
    {
        if(TargetGang == null || ZonesToAttack == null || !ZonesToAttack.Any())
        {
            return;
        }
        if (Game.GameTime - GameTimeWarEnded < TimeToStartRetaliation)//what should this be?
        {
            return;
        }
        if (!RandomItems.RandomPercent(RetaliationPercentAtIncrement))
        {
            return;
        }
        OnRetaliationStarted();
    }
    public void Update()
    {
        if (IsEnded)
        {
            return;
        }
        if(Player.RecentlyRespawned)
        {
            OnPlayerLost();
            EntryPoint.WriteToConsole("PLAYER LOST RETALIATION SINCE THEY DIED OR GOT BUSTED");
            return;
        }
        if (!HasRetaliationStarted)
        {
            CheckRetaliationStart();
        }
        if (!HasRetaliationStarted)
        {
            return;
        }
        UpdateActive();
    }

    private void UpdateActive()
    {
        if(!HasPlayerReturnedToZone)
        {
            UpdateBeforeReturnedToZone();
        }
        else
        {
            UpdateAfterReturnedToZone();
        }
    }
    private void UpdateBeforeReturnedToZone()
    {
        if (Game.GameTime - GameTimeStarted >= TimeToReturnToZone)
        {
            OnPlayerLost();
            return;
        }
        if (IsPlayerInZone())
        {
            OnPlayerReturnedToZoneFirstTime();
        }
    }
    private void UpdateAfterReturnedToZone()
    {
        bool isPlayerInZone = IsPlayerInZone();
        if (!isPlayerInZone)
        {
            OnPlayerLeftZone();
            return;
        }
        if(GameTimeReturnedToZone == 0 || Game.GameTime - GameTimeReturnedToZone <= RetaliationTime)
        {
            return;
        }
        OnPlayerWon();
    }

    private void OnRetaliationStarted()
    {
        HasRetaliationStarted = true;
        GameTimeStarted = Game.GameTime;
        SendStartMessage();
        EntryPoint.WriteToConsole("GANG RETALIATION EVENT: RETALIATION STARTED");
    }
    private void OnPlayerLeftZone()
    {
        GameTimeReturnedToZone = 0;
        SendLeftZoneMessage();
        EntryPoint.WriteToConsole("GANG RETALIATION EVENT: PLAYER LKEFT ZONE AFTER ARRIVING");
    }
    private void OnPlayerLost()
    {
        IsEnded = true;
        GameTimeEnded = Game.GameTime;
        SendLostMessage();
        GangTerritoryManager.EndRetaliation(this, false);
        EntryPoint.WriteToConsole("GANG RETALIATION EVENT: PLAYER LOST");
    }
    private void OnPlayerWon()
    {
        GameTimeEnded = Game.GameTime;
        IsEnded = true;
        SendWonMessage();
        GangTerritoryManager.EndRetaliation(this, true);
        EntryPoint.WriteToConsole("GANG RETALIATION EVENT: PLAYER WON");
    }
    private void OnPlayerReturnedToZoneFirstTime()
    {
        HasPlayerReturnedToZone = true;
        GameTimeReturnedToZone = Game.GameTime;
        SendReturnedMessage();
        EntryPoint.WriteToConsole("GANG RETALIATION EVENT: PLAYER RETRUNED TO ZONE FOR FIRST TIME");
    }

    private void SendReturnedMessage()
    {
        List<string> Replies = new List<string>() {
                                $"Returned.",
                                };
        Player.CellPhone.AddScheduledText(Player.CurrentGang.Contact, Replies.PickRandom(), 1, false);
    }
    private void SendStartMessage()
    {
        List<string> Replies = new List<string>() {
                                $"Started.",
                                };
        Player.CellPhone.AddScheduledText(Player.CurrentGang.Contact, Replies.PickRandom(), 1, false);
    }
    private void SendLostMessage()
    {
        List<string> Replies = new List<string>() {
                                $"Lost.",
                                };
        Player.CellPhone.AddScheduledText(Player.CurrentGang.Contact, Replies.PickRandom(), 1, false);
    }
    private void SendWonMessage()
    {
        List<string> Replies = new List<string>() {
                                $"Won.",
                                };
        Player.CellPhone.AddScheduledText(Player.CurrentGang.Contact, Replies.PickRandom(), 1, false);
    }
    private void SendLeftZoneMessage()
    {
        List<string> Replies = new List<string>() {
                                $"Left Zone.",
                                };
        Player.CellPhone.AddScheduledText(Player.CurrentGang.Contact, Replies.PickRandom(), 1, false);
    }

    internal void LoadFromSave(uint timeToStartRetaliation, float retaliationPercentAtIncrement, uint retaliationTime, uint timeToReturnToZone, bool hasRetaliationStarted, bool isEnded, bool hasPlayerReturnedToZone)
    {
        TimeToStartRetaliation = timeToStartRetaliation;
        RetaliationPercentAtIncrement = retaliationPercentAtIncrement;
        RetaliationTime = retaliationTime;
        TimeToReturnToZone = timeToReturnToZone;
        HasRetaliationStarted = hasRetaliationStarted;
        IsEnded = isEnded;
        HasPlayerReturnedToZone = hasPlayerReturnedToZone;
        GameTimeStarted = Game.GameTime;
        if(HasPlayerReturnedToZone)
        {
            GameTimeReturnedToZone = Game.GameTime;
        }

    }
}

