using LosSantosRED.lsr.Interface;
using Rage;


public class PlayerFightClubFighter : FightClubFighter
{

    public override bool IsPlayer => true;

    public void MoveToRing(FightClubArena fightClubArena, IEntityProvideable world, SpawnPlace spawnPlace, IFightClubable player)
    {
        FightClubArena = fightClubArena;
        Player = player;
        SpawnPlace = spawnPlace;
        Player.Character.Position = spawnPlace.Position;
        Player.Character.Heading = spawnPlace.Heading;
        FightClubArena = fightClubArena;
    }
    public override void Setup()
    {
        
    }
    public override void StartFight(FightClubFight fightClubFight)
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

