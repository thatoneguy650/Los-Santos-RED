using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
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



    private int TimesPlayerDefendedRetaliation;


    public GangRetaliation(IGangTerritoryManageable player, GangTerritoryManager gangTerritoryManager, uint gameTimeWarEnded, Gang targetGang, List<Zone> zonesToAttack, 
        ISettingsProvideable settings, Vector3 centerPoint)
    {
        Player = player;
        GameTimeRetaliationStarted = gameTimeWarEnded;
        TargetGang = targetGang;
        ZonesToAttack = zonesToAttack;
        GangTerritoryManager = gangTerritoryManager;
        Settings = settings;
        CenterPoint = centerPoint;
    }
    public GangRetaliation(IGangTerritoryManageable player, GangTerritoryManager gangTerritoryManager, uint gameTimeWarEnded, Gang targetGang, List<Zone> zonesToAttack, 
        ISettingsProvideable settings, int timesPlayerDefendedRetaliation, Vector3 centerPoint)
    {
        Player = player;
        GameTimeRetaliationStarted = gameTimeWarEnded;
        TargetGang = targetGang;
        ZonesToAttack = zonesToAttack;
        GangTerritoryManager = gangTerritoryManager;
        Settings = settings;
        TimesPlayerDefendedRetaliation = timesPlayerDefendedRetaliation;     
        CenterPoint = centerPoint;
    }
    public bool IsWarfareActive { get; private set; }
    public bool IsEnded { get; private set; }
    public bool HasPlayerEnteredArea { get; private set; }
    public Gang TargetGang { get; private set; }
    public Vector3 CenterPoint { get; private set; }
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
        if(TargetGang == null)
        {
            IsEnded = true;
            return;
        }
        //WinsNeeded = RandomItems.GetRandomNumberInt(TargetGang.TakeoverTerritoryRetaliationTimesMin, TargetGang.TakeoverTerritoryRetaliationTimesMax);
        ResetTimedItems();
        EntryPoint.WriteToConsole($"GANG RETALIATION SETUP: TimeToStartRetaliation:{TimeToStartRetaliation} RetaliationPercentAtIncrement:{RetaliationPercentAtIncrement} RetaliationTime:{RetaliationTime} TimeToReturnToZone:{TimeToReturnToZone}");
    }


    private void ResetTimedItems()
    {
        EntryPoint.WriteToConsole($"RESET GANG RETALIATION RESET TIMED ITEMS RAN");
        TimeToStartRetaliation = RandomItems.GetRandomNumber(Settings.SettingsManager.GangSettings.TerritoryRetaliationStartTimeMin, Settings.SettingsManager.GangSettings.TerritoryRetaliationStartTimeMax); //60000, 120000);
        
        RetaliationPercentAtIncrement = RandomItems.GetRandomNumber(Settings.SettingsManager.GangSettings.TerritoryRetaliationPercentageMin, Settings.SettingsManager.GangSettings.TerritoryRetaliationPercentageMax); //20f, 50f);
        EntryPoint.WriteToConsole($"RESET GANG RETALIATION RetaliationPercentAtIncrement{RetaliationPercentAtIncrement} Initial");
        RetaliationPercentAtIncrement -= (float)((float)TimesPlayerDefendedRetaliation * Settings.SettingsManager.GangSettings.TerritoryRetaliationPercentageDecreaseBasedOnTimesPlayerDefended);
        EntryPoint.WriteToConsole($"RESET GANG RETALIATION RetaliationPercentAtIncrement{RetaliationPercentAtIncrement} Adjusted");
        if (RetaliationPercentAtIncrement <= 1.0f)
        {
            RetaliationPercentAtIncrement = 1.0f;
        }
        EntryPoint.WriteToConsole($"RESET GANG RETALIATION RetaliationPercentAtIncrement{RetaliationPercentAtIncrement} Final");

        RetaliationTime = RandomItems.GetRandomNumber(Settings.SettingsManager.GangSettings.TerritoryRetaliationTimeMin, Settings.SettingsManager.GangSettings.TerritoryRetaliationTimeMax); //120000, 180000);
        
        
        TimeToReturnToZone = RandomItems.GetRandomNumber(Settings.SettingsManager.GangSettings.TerritoryRetaliationTimeToReturnMin, Settings.SettingsManager.GangSettings.TerritoryRetaliationTimeToReturnMax); //120000, 180000); //Settings.SettingsManager.GangSettings.TerritoryRetaliationTimeToReturnMin

        GameTimeReturnedToZone = 0;
        HasPlayerBeenWarnedNotToLeaveZone = false;

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


        EntryPoint.WriteToConsole($"GANG RETALIATION UPDATE RAN HasPlayerEnteredArea{HasPlayerEnteredArea} TimeAfterReturn{Game.GameTime - GameTimeReturnedToZone} NeededTime:{RetaliationTime}");


        if (Player.RecentlyRespawned)
        {
            OnPlayerLost();
            EntryPoint.WriteToConsole("PLAYER LOST RETALIATION SINCE THEY DIED OR GOT BUSTED");
            return;
        }
        if (!IsWarfareActive)
        {
            CheckRetaliationStart();
        }
        if (!IsWarfareActive)
        {
            return;
        }
        UpdateActive();
    }

    private void UpdateActive()
    {
        if(!HasPlayerEnteredArea)
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
            if(Player.RelationshipManager.GangRelationships.CurrentGangKickUp.MissedAmount > 0 || Player.RelationshipManager.GangRelationships.CurrentGangKickUp.MissedPeriods > 0)
            {
                OnPlayerLost();
            }
            else
            {
                if(RandomItems.RandomPercent(Settings.SettingsManager.GangSettings.TerritoryRetaliationAutoDefendPercentage))
                {
                    OnPlayerWonWithoutHelping();
                }
                else
                {
                    OnPlayerLost();
                }
            }

            
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
        IsWarfareActive = true;
        HasPlayerEnteredArea = false;
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
        
        EntryPoint.WriteToConsole("GANG RETALIATION EVENT: PLAYER LEFT ZONE AFTER ARRIVING");
    }
    private void OnPlayerLost()
    {
        IsEnded = true;
        GameTimeEnded = Game.GameTime;
        SendLostMessage();
        GangTerritoryManager.EndRetaliation(this, false);
        IsWarfareActive = false;
        EntryPoint.WriteToConsole("GANG RETALIATION EVENT: PLAYER LOST");
    }


    private void OnPlayerWonWithoutHelping()
    {
        IsWarfareActive = false;
        SendWonWithoutWorkMessage();
        TimesPlayerDefendedRetaliation++;
        GameTimeRetaliationStarted = Game.GameTime;
        ResetTimedItems();
        EntryPoint.WriteToConsole($"GANG RETALIATION EVENT: PLAYER WON WITHOUT DOING SHIT TimesPlayerDefendedRetaliation{TimesPlayerDefendedRetaliation}");
    }



    private void OnPlayerWon()
    {
        //GameTimeEnded = Game.GameTime;
        //IsEnded = true;
        IsWarfareActive = false;
        SendWonMessage();
        //GangTerritoryManager.EndRetaliation(this, true);
        TimesPlayerDefendedRetaliation++;

        //if(TimesWon >= WinsNeeded)
        //{
        //    GangTerritoryManager.EndRetaliation(this, false);
        //}
        //else
        //{
            GameTimeRetaliationStarted = Game.GameTime;
            ResetTimedItems();
        //}

        
        EntryPoint.WriteToConsole($"GANG RETALIATION EVENT: PLAYER WON TimesPlayerDefendedRetaliation{TimesPlayerDefendedRetaliation}");
    }

    private void OnPlayerReturnedToZoneFirstTime()
    {
        HasPlayerEnteredArea = true;
        GameTimeReturnedToZone = Game.GameTime;

        if(CenterPoint == Vector3.Zero)
        {
            CenterPoint = Player.Position;
        }

        SendReturnedMessage();
        EntryPoint.WriteToConsole("GANG RETALIATION EVENT: PLAYER RETRUNED TO ZONE FOR FIRST TIME");
    }

    private void SendReturnedMessage()
    {
        List<string> Replies = new List<string>() {
                                $"Took you long enough, {TargetGang.ColorPrefix}{TargetGang.ShortName}~s~ is running rampant. Hold Out for {NativeHelper.ConvertMSToTime(RetaliationTime)}.",
                                $"We are getting fucked by {TargetGang.ColorPrefix}{TargetGang.ShortName}~s~. Hold out for {NativeHelper.ConvertMSToTime(RetaliationTime)}",
                                $"We're you in Lemoyne? The fuckers at {TargetGang.ColorPrefix}{TargetGang.ShortName}~s~ are all over us! We need you to hold out for {NativeHelper.ConvertMSToTime(RetaliationTime)}",
                                };
        Player.CellPhone.AddScheduledText(Player.CurrentGang.Contact, Replies.PickRandom(), 0, true);
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
        Player.CellPhone.AddScheduledText(Player.CurrentGang.Contact, Replies.PickRandom(), 0, true);
    }
    private void SendWonMessage()
    {
        List<string> Replies = new List<string>() {
                                $"Good work holding off those {TargetGang.ColorPrefix}{TargetGang.ShortName}~s~ fucks. Was worried we were gonna lose {ZonesToAttack.FirstOrDefault()?.DisplayName}.",
            $"{TargetGang.ColorPrefix}{TargetGang.ShortName}~s~ has been beaten back. We still control {ZonesToAttack.FirstOrDefault()?.DisplayName}.",

            $"So many {TargetGang.ColorPrefix}{TargetGang.ShortName}~s~ bodies in {ZonesToAttack.FirstOrDefault()?.DisplayName}. They've got their tail between their legs.",
                                };
        Player.CellPhone.AddScheduledText(Player.CurrentGang.Contact, Replies.PickRandom(), 0, true);
    }
    private void SendWonWithoutWorkMessage()
    {
        List<string> Replies = new List<string>() {
                                $"We held off {TargetGang.ColorPrefix}{TargetGang.ShortName}~s~ without you in {ZonesToAttack.FirstOrDefault()?.DisplayName}. Where the fuck were you?",
            $"{TargetGang.ColorPrefix}{TargetGang.ShortName}~s~ has been beaten back despite you not helping. We still control {ZonesToAttack.FirstOrDefault()?.DisplayName}.",

            $"So many {TargetGang.ColorPrefix}{TargetGang.ShortName}~s~ bodies in {ZonesToAttack.FirstOrDefault()?.DisplayName}. Why weren't you there?.",
                                };
        Player.CellPhone.AddScheduledText(Player.CurrentGang.Contact, Replies.PickRandom(), 0, true);
    }
    private void SendLeftZoneMessage()
    {
        List<string> Replies = new List<string>() {
                                $"Get your ass back to {ZonesToAttack.FirstOrDefault()?.DisplayName}. These colors don't run.",
                                $"You need to stay in {ZonesToAttack.FirstOrDefault()?.DisplayName} or we are going to get fucked.",
                                $"GET BACK TO {ZonesToAttack.FirstOrDefault()?.DisplayName} NOW.",
                                };
        Player.CellPhone.AddScheduledText(Player.CurrentGang.Contact, Replies.PickRandom(), 0, true);
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

