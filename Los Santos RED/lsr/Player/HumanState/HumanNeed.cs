using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public abstract class HumanNeed
{
    private IHumanStateable Player;
    private float MaxValue;
    private float MinValue;
    private uint GameTimeLastUpdated;
    private uint GameTimeBetweenUpdates = 1000;
    private ITimeReportable Time;
    public HumanNeed(string name, float minValue, float maxValue, IHumanStateable humanStateable, ITimeReportable time)
    {
        Name = name;
        MaxValue = maxValue;
        MinValue = minValue;
        Player = humanStateable;
        CurrentValue = maxValue;
        Time = time;
    }
    public bool NeedsUpdate => Game.GameTime - GameTimeLastUpdated >= GameTimeBetweenUpdates;
    public string Name { get; set; }
    public float CurrentValue { get; private set; }
    public abstract void Update();
    public virtual void Change(float value)
    {
        if (CurrentValue + value < MinValue)
        {
            CurrentValue = MinValue;
        }
        else if (CurrentValue + value > MaxValue)
        {
            CurrentValue = MaxValue;
        }
        else
        {
            CurrentValue += value;
        }  
        if(CurrentValue == MinValue)
        {
            OnMinimum();
        }
        if (CurrentValue == MaxValue)
        {
            OnMaximum();
        }
    }
    public abstract void OnMinimum();
    public abstract void OnMaximum();
    public virtual void Reset()
    {
        CurrentValue = MaxValue;
        GameTimeLastUpdated = Game.GameTime;
    }
}

