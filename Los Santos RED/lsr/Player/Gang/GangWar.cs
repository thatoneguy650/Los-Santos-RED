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
    public List<Zone> ZonesToAttack { get; set; }
    public void SetOutcome(bool isPlayerVictory)
    {
        IsWarEnded = true;
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
               
            };
        Player.CellPhone.AddPhoneResponse(Player.CurrentGang.Contact.Name, Player.CurrentGang.Contact.IconName, Replies.PickRandom());
    }
    private void SendWarWonMessage()
    {
        List<string> Replies = new List<string>() {
                                $"Looks like those {TargetGang.ShortName} fucks have been put in their place. We now own {ZonesToAttack.FirstOrDefault()?.DisplayName}",
                                };
        Player.CellPhone.AddScheduledText(Player.CurrentGang.Contact, Replies.PickRandom(), 1, false);
    }
    private void SendWarLostMessage()
    {
        List<string> Replies = new List<string>() {
                                $"How did you fuck that up? {TargetGang.ShortName} still own {ZonesToAttack.FirstOrDefault()?.DisplayName} and we look like pussies",
                                };
        Player.CellPhone.AddScheduledText(Player.CurrentGang.Contact, Replies.PickRandom(), 1, false);
    }
}

