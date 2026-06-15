using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GangWar
{
    private uint GameTimeStarted;

    public GangWar(Gang targetGang, List<Zone> zonesToAttack, int casualityLimit)
    {
        TargetGang = targetGang;
        ZonesToAttack = zonesToAttack;
        CasualityLimit = casualityLimit;
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
    }
    public void Start()
    {
        GameTimeStarted = Game.GameTime;
    }
    public void AddCasuality()
    {
        Casualites++;
    }
    public void Update()
    {
        if(IsWarEnded)
        {
            return;
        }
        if(Casualites > CasualityLimit)
        {
            EntryPoint.WriteToConsole("GANG WAR IS OVER THE CASUALITY LIMIT, SET PLAYER WINS");
            SetOutcome(true);
        }
    }
}

