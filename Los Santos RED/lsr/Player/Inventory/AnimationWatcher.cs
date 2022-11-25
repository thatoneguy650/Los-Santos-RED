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

    public bool HasStartedAnimation { get; private set; }

    public bool IsAnimationRunning(float AnimationTime)
    {
        if(AnimationTime > 0.0f)
        {
            HasStartedAnimation = true;
        }
        if (Game.GameTime - GameTimeLastCheckedAnimation >= 500)
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


}

