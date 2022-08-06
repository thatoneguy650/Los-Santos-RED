using Rage;
using System.Collections.Generic;

namespace LosSantosRED.lsr.Player.Activity
{
    public class SmokingData
    {
        public SmokingData(string animBase, string animBaseDictionary, string animEnter, string animEnterDictionary, string animExit, string animExitDictionary, List<string> animIdle, string animIdleDictionary, string handBoneName, Vector3 handOffset, Rotator handRotator, string mouthBoneName, Vector3 mouthOffset, Rotator mouthRotator, string propModelName)
        {
            AnimBase = animBase;
            AnimBaseDictionary = animBaseDictionary;
            AnimEnter = animEnter;
            AnimEnterDictionary = animEnterDictionary;
            AnimExit = animExit;
            AnimExitDictionary = animExitDictionary;
            AnimIdle = animIdle;
            AnimIdleDictionary = animIdleDictionary;
            HandBoneName = handBoneName;
            HandOffset = handOffset;
            HandRotator = handRotator;
            MouthBoneName = mouthBoneName;
            MouthOffset = mouthOffset;
            MouthRotator = mouthRotator;
            PropModelName = propModelName;
        }

        public string AnimBase { get; set; }
        public string AnimBaseDictionary { get; set; }
        public string AnimEnter { get; set; }
        public string AnimEnterDictionary { get; set; }
        public string AnimExit { get; set; }
        public string AnimExitDictionary { get; set; }
        public List<string> AnimIdle { get; set; } = new List<string>();
        public string AnimIdleDictionary { get; set; }
        public string HandBoneName { get; set; }
        public Vector3 HandOffset { get; set; }
        public Rotator HandRotator { get; set; }
        public string MouthBoneName { get; set; }
        public Vector3 MouthOffset { get; set; }
        public Rotator MouthRotator { get; set; }
        public string PropModelName { get; set; }
    }
}