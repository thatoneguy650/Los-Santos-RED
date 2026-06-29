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
    private uint GameTimeRetaliationStarted;
    private GangTerritoryManager GangTerritoryManager;
    private IGangTerritoryManageable Player;
    private uint TimeToStartRetaliation;
    private float RetaliationPercentAtIncrement;
    private uint RetaliationTime;
    private uint TimeToReturnToZone;
    private ISettingsProvideable Settings;
    private bool HasPlayerBeenWarnedNotToLeaveZone;
    private int TimesWon;
    private int WinsNeeded = 1;

    public GangRetaliation(IGangTerritoryManageable player, GangTerritoryManager gangTerritoryManager, uint gameTimeWarEnded, Gang targetGang, List<Zone> zonesToAttack, ISettingsProvideable settings)
    {
        Player = player;
        GameTimeRetaliationStarted = gameTimeWarEnded;
        TargetGang = targetGang;
        ZonesToAttack = zonesToAttack;
        GangTerritoryManager = gangTerritoryManager;
        Settings = settings;
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
        WinsNeeded = RandomItems.GetRandomNumberInt(TargetGang.TakeoverTerritoryRetaliationTimesMin, TargetGang.TakeoverTerritoryRetaliationTimesMax);
        ResetTimedItems();
        EntryPoint.WriteToConsole($"GANG RETALIATION SETUP: TimeToStartRetaliation:{TimeToStartRetaliation} RetaliationPercentAtIncrement:{RetaliationPercentAtIncrement} RetaliationTime:{RetaliationTime} TimeToReturnToZone:{TimeToReturnToZone}");
    }


    private void ResetTimedItems()
    {
        TimeToStartRetaliation = RandomItems.GetRandomNumber(Settings.SettingsManager.GangSettings.TerritoryRetaliationStartTimeMin, Settings.SettingsManager.GangSettings.TerritoryRetaliationStartTimeMax); //60000, 120000);
        RetaliationPercentAtIncrement = RandomItems.GetRandomNumber(Settings.SettingsManager.GangSettings.TerritoryRetaliationPercentageMin, Settings.SettingsManager.GangSettings.TerritoryRetaliationPercentageMax); //20f, 50f);
        RetaliationTime = RandomItems.GetRandomNumber(Settings.SettingsManager.GangSettings.TerritoryRetaliationTimeMin, Settings.SettingsManager.GangSettings.TerritoryRetaliationTimeMax); //120000, 180000);
        TimeToReturnToZone = RandomItems.GetRandomNumber(Settings.SettingsManager.GangSettings.TerritoryRetaliationTimeToReturnMin, Settings.SettingsManager.GangSettings.TerritoryRetaliationTimeToReturnMax); //120000, 180000); //Settings.SettingsManager.GangSettings.TerritoryRetaliationTimeToReturnMin
    }
    private void CheckRetaliationStart()
    {
        if(TargetGang == null || ZonesToAttack == null || !ZonesToAttack.Any())
        {
            return;
        }
        if (Game.GameTime - GameTimeRetaliationStarted < TimeToStartRetaliation)//what should this be?
        {
            return;
        }
        if (!RandomItems.RandomPercent(RetaliationPercentAtIncrement))
        {
            GameTimeRetaliationStarted = Game.GameTime;
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
        if(!HasPlayerBeenWarnedNotToLeaveZone)
        {
            SendLeftZoneMessage();
            HasPlayerBeenWarnedNotToLeaveZone = true;
        }
        
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
        //GameTimeEnded = Game.GameTime;
        //IsEnded = true;
        SendWonMessage();
        //GangTerritoryManager.EndRetaliation(this, true);
        TimesWon++;

        if(TimesWon >= WinsNeeded)
        {
            GangTerritoryManager.EndRetaliation(this, false);
        }
        else
        {
            GameTimeRetaliationStarted = Game.GameTime;
            ResetTimedItems();
        }

        
        EntryPoint.WriteToConsole($"GANG RETALIATION EVENT: PLAYER WON TimesWon{TimesWon} WinsNeeded{WinsNeeded}");
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
                                $"Took you long enough, {TargetGang.ColorPrefix}{TargetGang.ShortName}~s~ is running rampant. Waste those fucks!",
                                $"Did you stop for gas on the way back? We are getting fucked by {TargetGang.ColorPrefix}{TargetGang.ShortName}~s~",
                                $"We're you in Lemoyne? The fuckers at {TargetGang.ColorPrefix}{TargetGang.ShortName}~s~ are all over us!",
                                };
        Player.CellPhone.AddScheduledText(Player.CurrentGang.Contact, Replies.PickRandom(), 1, false);
    }
    private void SendStartMessage()
    {
        List<string> Replies = new List<string>() {
                                $"The fucks at {TargetGang.ColorPrefix}{TargetGang.ShortName}~s~ are making moves on {ZonesToAttack.FirstOrDefault()?.DisplayName}. Get your ass here NOW!",
        $"{TargetGang.ColorPrefix}{TargetGang.ShortName}~s~ is moving on {ZonesToAttack.FirstOrDefault()?.DisplayName}. We need backup ASAP",
        $"Word on the street is {TargetGang.ColorPrefix}{TargetGang.ShortName}~s~ are going to make a move on {ZonesToAttack.FirstOrDefault()?.DisplayName}. You need to be here yesterday.",
        };
        Player.CellPhone.AddScheduledText(Player.CurrentGang.Contact, Replies.PickRandom(), 0, true);
    }
    private void SendLostMessage()
    {
        List<string> Replies = new List<string>() {
                                $"Good work losing {ZonesToAttack.FirstOrDefault()?.DisplayName} to {TargetGang.ColorPrefix}{TargetGang.ShortName}~s~ aka the biggest pussies in town.",
                                $"Do you enjoy making us look bad? We just lost {ZonesToAttack.FirstOrDefault()?.DisplayName} to {TargetGang.ColorPrefix}{TargetGang.ShortName}~s~.",
                                $"So much for {ZonesToAttack.FirstOrDefault()?.DisplayName} the motherfuckers at {TargetGang.ColorPrefix}{TargetGang.ShortName}~s~ are back to running it.",
                                };
        Player.CellPhone.AddScheduledText(Player.CurrentGang.Contact, Replies.PickRandom(), 1, false);
    }
    private void SendWonMessage()
    {
        List<string> Replies = new List<string>() {
                                $"Good work holding off those {TargetGang.ColorPrefix}{TargetGang.ShortName}~s~ fucks. Was worried we were gonna lose {ZonesToAttack.FirstOrDefault()?.DisplayName}.",
            $"{TargetGang.ColorPrefix}{TargetGang.ShortName}~s~ has been beaten back. We still control {ZonesToAttack.FirstOrDefault()?.DisplayName}.",

            $"So many {TargetGang.ColorPrefix}{TargetGang.ShortName}~s~ bodies in {ZonesToAttack.FirstOrDefault()?.DisplayName}. They've got their tail between their legs.",
                                };
        Player.CellPhone.AddScheduledText(Player.CurrentGang.Contact, Replies.PickRandom(), 1, false);
    }
    private void SendLeftZoneMessage()
    {
        List<string> Replies = new List<string>() {
                                $"Get your ass back to {ZonesToAttack.FirstOrDefault()?.DisplayName}. These colors don't run.",
                                $"You need to stay in {ZonesToAttack.FirstOrDefault()?.DisplayName} or we are going to get fucked.",
                                $"GET BACK TO {ZonesToAttack.FirstOrDefault()?.DisplayName} NOW.",
                                };
        Player.CellPhone.AddScheduledText(Player.CurrentGang.Contact, Replies.PickRandom(), 1, false);
    }

    //public void LoadFromSave(uint timeToStartRetaliation, float retaliationPercentAtIncrement, uint retaliationTime, uint timeToReturnToZone, bool hasRetaliationStarted, bool isEnded, bool hasPlayerReturnedToZone)
    //{
    //    TimeToStartRetaliation = timeToStartRetaliation;
    //    RetaliationPercentAtIncrement = retaliationPercentAtIncrement;
    //    RetaliationTime = retaliationTime;
    //    TimeToReturnToZone = timeToReturnToZone;
    //    HasRetaliationStarted = hasRetaliationStarted;
    //    IsEnded = isEnded;
    //    HasPlayerReturnedToZone = hasPlayerReturnedToZone;
    //    GameTimeStarted = Game.GameTime;
    //    if(HasPlayerReturnedToZone)
    //    {
    //        GameTimeReturnedToZone = Game.GameTime;
    //    }

    //}
}

