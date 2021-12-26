using Rage;
using System.Collections.Generic;

namespace LosSantosRED.lsr.Player.Activity
{
    public class SittingData
    {
        public SittingData(string animBase, string animBaseDictionary, string animEnter, string animEnterDictionary, string animExit, string animExitDictionary, List<string> animIdle, string animIdleDictionary, List<string> animIdle2, string animIdleDictionary2)
        {
            AnimBase = animBase;
            AnimBaseDictionary = animBaseDictionary;
            AnimEnter = animEnter;
            AnimEnterDictionary = animEnterDictionary;
            AnimExit = animExit;
            AnimExitDictionary = animExitDictionary;
            AnimIdle = animIdle;
            AnimIdleDictionary = animIdleDictionary;
            AnimIdle2 = animIdle2;
            AnimIdleDictionary2 = animIdleDictionary2;
        }

        public string AnimBase { get; set; }
        public string AnimBaseDictionary { get; set; }
        public string AnimEnter { get; set; }
        public string AnimEnterDictionary { get; set; }

        public string AnimExit { get; set; }
        public string AnimExitDictionary { get; set; }
        public List<string> AnimIdle { get; set; } = new List<string>();
        public string AnimIdleDictionary { get; set; }
        public List<string> AnimIdle2 { get; set; } = new List<string>();
        public string AnimIdleDictionary2 { get; set; }
    }
}