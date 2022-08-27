using Rage;
using System;

public class ModTask
{
    public string DebugName;
    public uint GameTimeLastRan = 0;
    public uint Interval = 500;
    public uint IntervalMissLength;
    public bool RanThisTick = false;
    public int RunGroup;
    public int RunOrder;
    public Action TickToRun;
    public ModTask(uint _Interval, string _DebugName, Action _TickToRun, int _RunGroup, int _RunOrder)
    {
        GameTimeLastRan = 0;
        Interval = _Interval;
        IntervalMissLength = Interval * 2;
        DebugName = _DebugName;
        TickToRun = _TickToRun;
        RunGroup = _RunGroup;
        RunOrder = _RunOrder;
    }
    public ModTask(uint _Interval, string _DebugName, Action _TickToRun, int _RunOrder)
    {
        GameTimeLastRan = 0;
        Interval = _Interval;
        IntervalMissLength = Interval * 2;
        DebugName = _DebugName;
        TickToRun = _TickToRun;
        RunOrder = _RunOrder;
    }
    //public bool MissedInterval => Interval != 0 && Game.GameTime - GameTimeLastRan >= IntervalMissLength;
    //public bool RunningBehind => Interval != 0 && Game.GameTime - GameTimeLastRan >= (IntervalMissLength * 2);
    public bool ShouldRun => GameTimeLastRan == 0 || Game.GameTime - GameTimeLastRan > Interval; //public bool ShouldRun => GameTimeLastRan == 0 || Environment.TickCount - GameTimeLastRan > Interval;//CHANING IT TO TICKCOUNT WILL MURDER PERFORMANCE. IS THE TICK COUNT TOO HIGH?
    public void Run()
    {
        TickToRun();
        GameTimeLastRan = Game.GameTime;
        RanThisTick = true;
    }
}