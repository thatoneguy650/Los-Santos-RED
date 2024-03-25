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

    private float IntervalIntensity = 0.0f;

    public Intoxicator(IIntoxicatable player, Intoxicant intoxicant)
    {
        Intoxicant = intoxicant;
        Player = player;
        IntervalIntensity = 0.0f;
    }
    public float CurrentIntensity
    {
        get
        {
            if (IsIntervalBased)
            {
                if (IsConsuming)
                {
                   // EntryPoint.WriteToConsole($"CurrentIntensity1: {IntervalIntensity}");
                    return IntervalIntensity;
                    
                }
                else
                {
                    //EntryPoint.WriteToConsole($"CurrentIntensity1: {IntervalIntensity * 1.0f - ((float)(Game.GameTime - GameTimeStoppedIntoxicating) / (float)Intoxicant.SoberingIntervalTime)} ST1:{TotalTimeSober} ST2:{Intoxicant.SoberingIntervalTime} StartedTime:{GameTimeStartedIntoxicating} StoppedTime:{GameTimeStoppedIntoxicating}");

                    return IntervalIntensity * 1.0f - ((float)(Game.GameTime - GameTimeStoppedIntoxicating) / (float)Intoxicant.SoberingIntervalTime);


                    EntryPoint.WriteToConsole($"CurrentIntensity1: {IntervalIntensity} SoberTimePerecent:{((float)TotalTimeSober / Intoxicant.SoberingIntervalTime)} ST1:{TotalTimeSober} ST2:{Intoxicant.SoberingIntervalTime} StartedTime:{GameTimeStartedIntoxicating} StoppedTime:{GameTimeStoppedIntoxicating}");
                    return (float)((float)IntervalIntensity - ((float)TotalTimeSober / Intoxicant.SoberingIntervalTime)).Clamp(0.0f, Intoxicant.MaxEffectAllowed);
                }
            }
            else
            {
                //EntryPoint.WriteToConsole($"CurrentIntensity3 WRONG: {IntervalIntensity}");
                return (float)((float)TotalTimeIntoxicated / Intoxicant.IntoxicatingIntervalTime - (float)TotalTimeSober / Intoxicant.SoberingIntervalTime).Clamp(0.0f, Intoxicant.MaxEffectAllowed);
            }
        }
    }
    private uint HasBeenIntoxicatedFor
    {
        get
        {
            return (!IsConsuming ? 0 : Game.GameTime - GameTimeStartedIntoxicating) + PreviousIntoxicationTime;
        }
    }
    private uint HasBeenNotIntoxicatedFor
    {
        get
        {
            return (IsConsuming ? 0 : Game.GameTime - GameTimeStoppedIntoxicating) + PreviousIntoxicationTime;
        }
    }
    private uint TotalTimeIntoxicated
    {
        get
        {
            return IsConsuming? HasBeenIntoxicatedFor : GameTimeStoppedIntoxicating - GameTimeStartedIntoxicating + PreviousIntoxicationTime;
        }
    }
    private uint TotalTimeSober
    {
        get
        {
            return IsConsuming ? 0 : HasBeenNotIntoxicatedFor;
        }
    }
    public Intoxicant Intoxicant { get; set; }
    public bool IsConsuming { get; private set; }
    public bool IsIntervalBased { get; set; }
    public float IntervalAddition { get; set; } = 0.4f;
    public void StopConsuming()
    {
        if (IsConsuming)
        {
            PreviousIntoxicationTime = Game.GameTime - GameTimeStartedIntoxicating;
            GameTimeStoppedIntoxicating = Game.GameTime;
            IsConsuming = false;

            EntryPoint.WriteToConsole($"STOPPED CONSUMING {Game.GameTime}");
        }
    }
    public void StartConsuming()
    {
        if (!IsConsuming)
        {
            GameTimeStartedIntoxicating = Game.GameTime;
            IsConsuming = true;
            EntryPoint.WriteToConsole($"STARTED CONSUMING {Game.GameTime}");
        }
    }
    public void SetConsumed()
    {
        GameTimeStartedIntoxicating = Game.GameTime - 30000;
        GameTimeStoppedIntoxicating = Game.GameTime;//2631372
    }
    public void OnIntervalConsumed()
    {
        IntervalIntensity = (IntervalIntensity + IntervalAddition).Clamp(0.0f, Intoxicant.MaxEffectAllowed);
        EntryPoint.WriteToConsole($"YOU CONSUMED INTOXICAN ON INTERVAL {IntervalIntensity} MAX {Intoxicant.MaxEffectAllowed}");
    }
}

