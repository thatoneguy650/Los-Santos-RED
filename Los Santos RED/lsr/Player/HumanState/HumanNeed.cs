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
    private uint GameTimeBetweenValueUpdates = 1000;
    private uint GameTimeBetweenResultUpdates = 15000;
    private ITimeReportable Time;
    private uint GameTimeLastChangedNeed;
    private int Digits;
    private string ColorPrefix => CurrentValue <= MaxValue * 0.25f ? "~r~" : CurrentValue <= MaxValue * 0.5f ? "~o~" : CurrentValue <= MaxValue * 0.75f ? "~y~" : "~s~";
    public HumanNeed(string name, float minValue, float maxValue, IHumanStateable humanStateable, ITimeReportable time, int digits)
    {
        Name = name;
        MaxValue = maxValue;
        MinValue = minValue;
        Player = humanStateable;
        CurrentValue = maxValue;
        Time = time;
        Digits = digits;
    }

    public bool IsAboveQuarter => CurrentValue >= MaxValue * 0.25f;
    public bool IsBelowQuarter => CurrentValue <= MaxValue * 0.25f;
    public bool IsBelowThreeQuarters => CurrentValue <= MaxValue * 0.75f;
    public bool IsAboveThreeQuarters => CurrentValue >= MaxValue * 0.75f;
    public bool IsAboveHalf => CurrentValue >= MaxValue * 0.5f;
    public bool IsBelowHalf => CurrentValue <= MaxValue * 0.5f;
    public bool IsNearMax => CurrentValue >= 0.9f * MaxValue;
    public bool IsMax => CurrentValue == MaxValue;
    public bool NeedsValueUpdate => Game.GameTime - GameTimeLastUpdated >= GameTimeBetweenValueUpdates;
    public bool NeedsResultUpdate => Game.GameTime - GameTimeLastUpdated >= GameTimeBetweenResultUpdates;
    public bool RecentlyChanged => GameTimeLastChangedNeed > 0 && Game.GameTime - GameTimeLastChangedNeed <= 5000;
    public string Name { get; set; }
    public float CurrentValue { get; private set; }
    public virtual string Display => $"{ColorPrefix}{Name}: {Math.Round(CurrentValue, Digits)}%";
    public abstract void Update();
    public virtual void Change(float value, bool updateRecent)
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
        if (updateRecent)
        {
            GameTimeLastChangedNeed = Game.GameTime;
        }
    }

    public virtual void Set(float value)
    {
        if (value < MinValue)
        {
            CurrentValue = MinValue;
        }
        else if (value > MaxValue)
        {
            CurrentValue = MaxValue;
        }
        else
        {
            CurrentValue = value;
        }
        if (CurrentValue == MinValue)
        {
            OnMinimum();
        }
        if (CurrentValue == MaxValue)
        {
            OnMaximum();
        }
    }
    public void SetRandom()
    {
        Set(RandomItems.GetRandomNumber(MinValue, MaxValue));
    }
    public abstract void OnMinimum();
    public abstract void OnMaximum();
    public virtual void Reset()
    {
        CurrentValue = MaxValue;
        GameTimeLastUpdated = Game.GameTime;
    }



}

