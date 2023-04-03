using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class AnimationWatcher
{

    private float PrevAnimationTime;
    private uint GameTimeLastCheckedAnimation;
    public uint TimeBetweenCheck { get; set; } = 500;
    public bool HasStartedAnimation { get; private set; }

    public bool IsAnimationRunning(float AnimationTime)
    {
        if(AnimationTime > 0.0f)
        {
            HasStartedAnimation = true;
        }
        if (Game.GameTime - GameTimeLastCheckedAnimation >= TimeBetweenCheck)
        {
            if (PrevAnimationTime == AnimationTime && HasStartedAnimation)
            {
                return false;
            }
            PrevAnimationTime = AnimationTime;
            GameTimeLastCheckedAnimation = Game.GameTime;
        }
        return true;
    }

    public void Reset()
    {
        GameTimeLastCheckedAnimation = Game.GameTime;
        PrevAnimationTime = 0.0f;
    }
}

