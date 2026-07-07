using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class FightClubFighterPlayer : FightClubFighter
{
    
    public override bool IsPlayer => true;

    public void MoveToRing(FightClubArena fightClubArena, IEntityProvideable world, SpawnLocation SpawnLocation, IFightClubable player)
    {
        FightClubArena = fightClubArena;
        Player = player;
        Player.Character.Position = SpawnLocation.FinalPosition;
        Player.Character.Heading = SpawnLocation.Heading;
        FightClubArena = fightClubArena;
    }
    public override void Setup()
    {

    }
    public override void StartFight()
    {

    }
    public override void Update()
    {
        if(HasLost)
        {
            return;
        }
        float DistanceFromArena = Player.Character.DistanceTo(FightClubArena.ArenaCenter);
        CheckLoseConditions(DistanceFromArena);
        base.Update();
    }

    public void CheckLoseConditions(float distance)
    {
        if (Player.IsDead)
        {
            HasLost = true;
            return;
        }

        if (distance >= 20f)
        {
            if (!IsOutsideRing)
            {
                IsOutsideRing = true;
                GameTimeLeftRing = Game.GameTime;
                Game.DisplayHelp("YOU LEFT THE RING GET BACK");
                EntryPoint.WriteToConsole("YOU LEFT THE RING");
            }
        }
        else //if (distance < 20f && IsOutsideRing)
        {
            if (IsOutsideRing)
            {
                IsOutsideRing = false;
                GameTimeLeftRing = 0;
                EntryPoint.WriteToConsole("YOU WENT BACK INTO THE RING");
            }
        }


        if (GameTimeOutsideRing >= 5000 && !HasLost)
        {
            HasLost = true;
            Game.DisplayHelp("YOU LEFT THE RING TOO LONG");
            EntryPoint.WriteToConsole("YOU LEFT THE RING TOO LONG, FAIL");
        }




    }
    public override void Dispose()
    {

        base.Dispose();
    }
}

