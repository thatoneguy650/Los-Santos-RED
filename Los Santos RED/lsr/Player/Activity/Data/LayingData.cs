using Rage;
using System.Collections.Generic;

namespace LosSantosRED.lsr.Player.Activity
{
    public class LayingData
    {
        public LayingData()
        {
        }

        public LayingData(string animBase, string animBaseDictionary, string animEnter, string animEnterDictionary, string animExit, string animExitDictionary, List<string> animIdle, string animIdleDictionary)
        {
            AnimBase = animBase;
            AnimBaseDictionary = animBaseDictionary;
            AnimEnter = animEnter;
            AnimEnterDictionary = animEnterDictionary;
            AnimExit = animExit;
            AnimExitDictionary = animExitDictionary;
            AnimIdle = animIdle;
            AnimIdleDictionary = animIdleDictionary;

        }

        public string AnimBase { get; set; }
        public string AnimBaseDictionary { get; set; }

        public float AnimBaseBlendIn { get; set; } = 8.0f;
        public float AnimBaseBlendOut { get; set; } = -8.0f;
        public int AnimBaseFlag { get; set; } = 0;




        public string AnimEnter { get; set; }
        public string AnimEnterDictionary { get; set; }

        public float AnimEnterBlendIn { get; set; } = 8.0f;
        public float AnimEnterBlendOut { get; set; } = -8.0f;
        public int AnimEnterFlag { get; set; } = 0;



        public string AnimExit { get; set; }
        public string AnimExitDictionary { get; set; }



        public float AnimExitBlendIn { get; set; } = 8.0f;
        public float AnimExitBlendOut { get; set; } = -8.0f;
        public int AnimExitFlag { get; set; } = 0;

        public List<string> AnimIdle { get; set; } = new List<string>();
        public string AnimIdleDictionary { get; set; }


        public float AnimIdletBlendIn { get; set; } = 8.0f;
        public float AnimIdleBlendOut { get; set; } = -8.0f;
        public int AnimIdleFlag { get; set; } = 0;
        public bool AnimEnterIsReverse { get; set; } = false;
    }
}