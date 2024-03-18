using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DistanceChecker
{
    private float prevDistanceToPlayer;
    private uint GameTimeStartingMovingTowards;
    private uint GameTimeStartingMovingAway;
    public DistanceChecker()
    {

    }
    public bool IsMovingTowards => GameTimeStartingMovingTowards != 0 && Game.GameTime - GameTimeStartingMovingTowards >= 3000;
    public bool IsMovingAway => GameTimeStartingMovingAway != 0 && Game.GameTime - GameTimeStartingMovingAway >= 3000;
    public void UpdateMovement(float distanceToPlayer)
    {
        if(prevDistanceToPlayer == 0)//first update, cant tell
        {
            prevDistanceToPlayer = distanceToPlayer;
            return;
        }
        if(distanceToPlayer > prevDistanceToPlayer)//moving away!!
        {
            if(GameTimeStartingMovingAway == 0)
            {
                GameTimeStartingMovingAway = Game.GameTime;
            }
            GameTimeStartingMovingTowards = 0;
        }
        else if (distanceToPlayer < prevDistanceToPlayer)//moving toward!
        {
            if (GameTimeStartingMovingTowards == 0)
            {
                GameTimeStartingMovingTowards = Game.GameTime;
            }
            GameTimeStartingMovingAway = 0;
        }
        prevDistanceToPlayer = distanceToPlayer;
    }

}

