using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DistanceChecker
{
    private float currentDistanceToPlayer;
    private float prevDistanceToPlayer;
    private uint GameTimeStartingMovingTowards;
    private uint GameTimeStartingMovingAway;
    private ISettingsProvideable Settings;

    public DistanceChecker(ISettingsProvideable settings)
    {
        Settings = settings;
    }

    public bool IsMovingTowards => GameTimeStartingMovingTowards != 0 && Game.GameTime - GameTimeStartingMovingTowards >= Settings.SettingsManager.PlayerOtherSettings.MovingTowardsTime;
    public bool IsMovingAway => GameTimeStartingMovingAway != 0 && Game.GameTime - GameTimeStartingMovingAway >= Settings.SettingsManager.PlayerOtherSettings.MovingAwayTime;
    public float DistanceToPlayer => currentDistanceToPlayer;
    public void UpdateMovement(float distanceToPlayer)
    {
        currentDistanceToPlayer = distanceToPlayer;
        if (prevDistanceToPlayer == 0)//first update, cant tell
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

