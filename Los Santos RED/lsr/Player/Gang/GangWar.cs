using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GangWar
{
    private uint GameTimeStarted;
    
    private GangTerritoryManager GangTerritoryManager;
    private IGangTerritoryManageable Player;

    public GangWar(IGangTerritoryManageable player, Gang targetGang, List<Zone> zonesToAttack, int casualityLimit, GangTerritoryManager gangTerritoryManager)
    {
        Player = player;
        TargetGang = targetGang;
        ZonesToAttack = zonesToAttack;
        CasualityLimit = casualityLimit;
        GangTerritoryManager = gangTerritoryManager;
    }


    public Gang TargetGang { get; set; }
    public int Casualites { get; set; }
    public bool IsPlayerVictorius { get; private set; }
    public bool IsWarEnded { get; private set; }
    public int CasualityLimit { get; private set; }
    public uint GameTimeEnded { get; private set; }
    public List<Zone> ZonesToAttack { get; set; }
    public void SetOutcome(bool isPlayerVictory)
    {
        IsWarEnded = true;
        GameTimeEnded = Game.GameTime;
        IsPlayerVictorius = isPlayerVictory;
        GangTerritoryManager.EndGangWar(TargetGang, IsPlayerVictorius);
        if(isPlayerVictory)
        {
            SendWarWonMessage();
        }
        else
        {
            SendWarLostMessage(); 
        }
    }
    public void Start()
    {
        GameTimeStarted = Game.GameTime;

        SendWarStartedMessage();

    }
    public void AddCasuality()
    {
        Casualites++;
    }
    public void Update(IGangTerritoryManageable Player)
    {
        if(IsWarEnded)
        {
            return;
        }
        if(Player.RecentlyRespawned)
        {
            SetOutcome(false);
        }
        else if(Casualites > CasualityLimit)
        {
            EntryPoint.WriteToConsole("GANG WAR IS OVER THE CASUALITY LIMIT, SET PLAYER WINS");
            SetOutcome(true);
        }
        EntryPoint.WriteToConsole($"GangWar UPDATED {Casualites} OF {CasualityLimit} IsWarEnded{IsWarEnded}");
    }


    private void SendWarStartedMessage()
    {
        List<string> Replies = new List<string>() {
                $"Get your ass to {ZonesToAttack.FirstOrDefault()?.DisplayName} and take out ~r~{CasualityLimit}~s~ of those {TargetGang.ColorPrefix}{TargetGang.ShortName}~s~ motherfuckers. They will know you're coming",
                $"We are going to hit {ZonesToAttack.FirstOrDefault()?.DisplayName} and waste ~r~{CasualityLimit}~s~ of those {TargetGang.ColorPrefix}{TargetGang.ShortName}~s~ fuckers. Be careful, they will be on alert",
                $"Gonna takeover {ZonesToAttack.FirstOrDefault()?.DisplayName}. First we need to get rid of ~r~{CasualityLimit}~s~ of those {TargetGang.ColorPrefix}{TargetGang.ShortName}~s~ pussies. Keep your guard up",

            };
        Player.CellPhone.AddPhoneResponse(Player.CurrentGang.Contact.Name, Player.CurrentGang.Contact.IconName, Replies.PickRandom());
    }
    private void SendWarWonMessage()
    {
        List<string> Replies = new List<string>() {
                                $"Looks like those {TargetGang.ColorPrefix}{TargetGang.ShortName}~s~ fucks have been put in their place. We now own ~p~{ZonesToAttack.FirstOrDefault()?.DisplayName}~s~",
                                $"So many bodies in ~p~{ZonesToAttack.FirstOrDefault()?.DisplayName}~s~. Too bad those fucks at {TargetGang.ColorPrefix}{TargetGang.ShortName}~s~ couldn't hold onto it. What pussies.",
                                $"Those {TargetGang.ColorPrefix}{TargetGang.ShortName}~s~ fucks have been wiped. Can';'t wait to kick my feet up in ~p~{ZonesToAttack.FirstOrDefault()?.DisplayName}~s~",
                                };
        Player.CellPhone.AddScheduledText(Player.CurrentGang.Contact, Replies.PickRandom(), 1, false);
    }
    private void SendWarLostMessage()
    {
        List<string> Replies = new List<string>() {
                                $"How did you fuck that up? {TargetGang.ShortName} still own {ZonesToAttack.FirstOrDefault()?.DisplayName} and we look like pussies.",
                                $"Is fucking up what you do best? {TargetGang.ShortName} have made a fool of us. If you can't takeover {ZonesToAttack.FirstOrDefault()?.DisplayName} don't fucking try",
                                $"Good work shit for brains. {TargetGang.ShortName} are still in charge of {ZonesToAttack.FirstOrDefault()?.DisplayName}. Don't say you're gonna do shit you can't.",
        };
        Player.CellPhone.AddScheduledText(Player.CurrentGang.Contact, Replies.PickRandom(), 1, false);
    }
}

