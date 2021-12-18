using Rage;
using System.Collections.Generic;

namespace LosSantosRED.lsr.Player.Activity
{
    public class DrinkingData
    {
        public DrinkingData(string animEnter, string animEnterDictionary, string animExit, string animExitDictionary, List<string> animIdle, string animIdleDictionary, int handBoneID, Vector3 handOffset, Rotator handRotator, string propModelName)
        {
            AnimEnter = animEnter;
            AnimEnterDictionary = animEnterDictionary;
            AnimExit = animExit;
            AnimExitDictionary = animExitDictionary;
            AnimIdle = animIdle;
            AnimIdleDictionary = animIdleDictionary;
            HandBoneID = handBoneID;
            HandOffset = handOffset;
            HandRotator = handRotator;
            PropModelName = propModelName;
        }
        public string AnimEnter { get; set; }
        public string AnimEnterDictionary { get; set; }
        public string AnimExit { get; set; }
        public string AnimExitDictionary { get; set; }
        public List<string> AnimIdle { get; set; } = new List<string>();
        public string AnimIdleDictionary { get; set; }
        public int HandBoneID { get; set; }
        public Vector3 HandOffset { get; set; }
        public Rotator HandRotator { get; set; }
        public string PropModelName { get; set; }
    }
}