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
   // private bool IsConsuming;
    private IIntoxicatable Player;
    private uint PreviousIntoxicationTime = 0;
    public Intoxicator(IIntoxicatable player, Intoxicant intoxicant)
    {
        Intoxicant = intoxicant;
        Player = player;
    }
    public float CurrentIntensity => (float)((float)TotalTimeIntoxicated / Intoxicant.IntoxicatingIntervalTime - (float)TotalTimeSober / Intoxicant.SoberingIntervalTime).Clamp(0.0f, Intoxicant.MaxEffectAllowed);
    private uint HasBeenIntoxicatedFor => (!IsConsuming ? 0 : Game.GameTime - GameTimeStartedIntoxicating) + PreviousIntoxicationTime;
    private uint HasBeenNotIntoxicatedFor => (IsConsuming ? 0 : Game.GameTime - GameTimeStoppedIntoxicating) + PreviousIntoxicationTime;
    private uint TotalTimeIntoxicated => IsConsuming ? HasBeenIntoxicatedFor : GameTimeStoppedIntoxicating - GameTimeStartedIntoxicating + PreviousIntoxicationTime;
    private uint TotalTimeSober => IsConsuming ? 0 : HasBeenNotIntoxicatedFor;
    public Intoxicant Intoxicant { get; set; }
    public bool IsConsuming { get; private set; }
    public void StopConsuming()
    {
        if (IsConsuming)
        {
            PreviousIntoxicationTime = Game.GameTime - GameTimeStartedIntoxicating;
            GameTimeStoppedIntoxicating = Game.GameTime;
            IsConsuming = false;
        }
    }
    public void StartConsuming()
    {
        if (!IsConsuming)
        {
            GameTimeStartedIntoxicating = Game.GameTime;
            IsConsuming = true;
        }
    }
    public void SetConsumed()
    {
        GameTimeStartedIntoxicating = Game.GameTime - 30000;
        GameTimeStoppedIntoxicating = Game.GameTime;
    }
}

