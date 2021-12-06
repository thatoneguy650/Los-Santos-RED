using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Intoxicator//THIS IS THE CURRENT INTOXICANTS VALUE INSIDE UR BODY
{
    private uint GameTimeStartedIntoxicating;
    private uint GameTimeStoppedIntoxicating;
    private bool IsConsuming;
    private IIntoxicatable Player;
    public Intoxicator(IIntoxicatable player, Intoxicant intoxicant)
    {
        Intoxicant = intoxicant;
        Player = player;
    }
    public float CurrentIntensity => (float)((float)TotalTimeIntoxicated / Intoxicant.IntoxicatingIntervalTime - (float)TotalTimeSober / Intoxicant.SoberingIntervalTime).Clamp(0.0f, Intoxicant.MaxEffectAllowed);
    private uint HasBeenIntoxicatedFor => !IsConsuming ? 0 : Game.GameTime - GameTimeStartedIntoxicating;
    private uint HasBeenNotIntoxicatedFor => IsConsuming ? 0 : Game.GameTime - GameTimeStoppedIntoxicating;
    private uint TotalTimeIntoxicated => IsConsuming ? HasBeenIntoxicatedFor : GameTimeStoppedIntoxicating - GameTimeStartedIntoxicating;
    private uint TotalTimeSober => IsConsuming ? 0 : HasBeenNotIntoxicatedFor;
    public Intoxicant Intoxicant { get; set; }
    public void Update(bool isConsuming)
    {
        if (IsConsuming != isConsuming)
        {

            if(IsConsuming)
            {
                if (CurrentIntensity == 0f)
                {
                    StartConsuming();
                }
            }
            else
            {
                StopConsuming();
            }
            IsConsuming = isConsuming;
        }
    }
    private void StopConsuming()
    {
        GameTimeStoppedIntoxicating = Game.GameTime;
    }
    private void StartConsuming()
    {
        GameTimeStartedIntoxicating = Game.GameTime;
    }
}

