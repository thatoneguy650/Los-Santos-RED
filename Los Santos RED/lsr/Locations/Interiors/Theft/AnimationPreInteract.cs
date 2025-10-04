using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AnimationPreInteract : ItemUsePreInteract
{

    private TheftInteract TheftInteract;
    //private string AnimDict = "missheist_jewel";
    //private string AnimName = "fp_smash_case_necklace";
    //private int AnimationFlags = 0;


    private AnimationBundle endBundle;
    public List<AnimationBundle> StartAnimations { get; set; } = new List<AnimationBundle>() { };
    public List<AnimationBundle> LoopAnimations { get; set; } = new List<AnimationBundle>() { };
    public List<AnimationBundle> EndAnimations { get; set; } = new List<AnimationBundle>() { };


    public AnimationPreInteract()
    {

    }
    public override void Start(IInteractionable player, ILocationInteractable locationInteractable, ISettingsProvideable settings, IModItems modItems, TheftInteract theftInteract)
    {
        Player = player;
        LocationInteractable = locationInteractable;
        Settings = settings;
        ModItems = modItems;
        TheftInteract = theftInteract;

        if (PerformAnimation())
        {
            TheftInteract.SetUnlocked();
        }
        StopPerformingAnimation();
    }
    private bool PerformAnimation()
    {
        Player.Character.Position = TheftInteract.Position;
        Player.Character.Heading = TheftInteract.Heading;
        HashSet<string> dictionaryList = new HashSet<string>();
        AnimationBundle startBundle = StartAnimations.Where(x => x.Gender == "U" || x.Gender == Player.Gender).PickRandom();
        if (startBundle != null)
        {
            dictionaryList.Add(startBundle.Dictionary);
        }
        AnimationBundle loopBundle = LoopAnimations.Where(x => x.Gender == "U" || x.Gender == Player.Gender).PickRandom();
        if (loopBundle != null)
        {
            dictionaryList.Add(loopBundle.Dictionary);
        }
        endBundle = EndAnimations.Where(x => x.Gender == "U" || x.Gender == Player.Gender).PickRandom();
        if (endBundle != null)
        {
            dictionaryList.Add(endBundle.Dictionary);
        }
        foreach (string dictionary in dictionaryList)
        {
            if (!AnimationDictionary.RequestAnimationDictionayResult(dictionary))
            {
                return false;
            }
        }
        if (startBundle != null)
        {
            NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, startBundle.Dictionary, startBundle.Name, startBundle.BlendIn, startBundle.BlendOut, startBundle.Time, startBundle.Flags, 0, false, false, false);
            TheftInteract.WaitForAnimation(startBundle.Dictionary, startBundle.Name);
        }
        if (loopBundle != null)
        {
            NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, loopBundle.Dictionary, loopBundle.Name, loopBundle.BlendIn, loopBundle.BlendOut, loopBundle.Time, loopBundle.Flags, 0, false, false, false);
        }
        return true;
    }
    public bool StopPerformingAnimation()
    {
        if (endBundle != null)
        {
            NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, endBundle.Dictionary, endBundle.Name, endBundle.BlendIn, endBundle.BlendOut, endBundle.Time, endBundle.Flags, 0, false, false, false);
            TheftInteract.WaitForAnimation(endBundle.Dictionary, endBundle.Name);
        }
        NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
        return true;
    }

    public void Dispose()
    {

    }
  
}
