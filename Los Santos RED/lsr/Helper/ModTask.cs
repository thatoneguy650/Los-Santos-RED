using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Helper
{
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
        public bool MissedInterval
        {
            get
            {
                if (Interval == 0)
                {
                    return false;
                }
                else if (Game.GameTime - GameTimeLastRan >= IntervalMissLength)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool RunningBehind
        {
            get
            {
                if (Interval == 0)
                {
                    return false;
                }
                else if (Game.GameTime - GameTimeLastRan >= (IntervalMissLength * 2))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool ShouldRun
        {
            get
            {
                if (GameTimeLastRan == 0)
                {
                    return true;
                }
                else if (Game.GameTime - GameTimeLastRan > Interval)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public void Run()
        {
            TickToRun();
            GameTimeLastRan = Game.GameTime;
            RanThisTick = true;
        }
    }
}
