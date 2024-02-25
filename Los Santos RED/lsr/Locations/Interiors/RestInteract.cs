using Rage.Native;
using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LosSantosRED.lsr.Interface;
using System.Xml.Serialization;
using RAGENativeUI;
using LosSantosRED.lsr;
using ExtensionsMethods;

public class RestInteract : InteriorInteract
{
    private AnimationBundle endBundle;
    [XmlIgnore]
    public IRestableLocation RestableLocation {get;set;}
    public List<AnimationBundle> StartAnimations { get;set;}
    public List<AnimationBundle> LoopAnimations { get; set; }
    public List<AnimationBundle> EndAnimations { get; set; }
    public bool UseDefaultAnimations { get; set; } = true;
    public RestInteract()
    {
    }

    public RestInteract(string name, Vector3 position, float heading, string buttonPromptText) : base(name, position, heading, buttonPromptText)
    {

    }

    public override void OnInteract()
    {
        EntryPoint.WriteToConsole($"ON INTERACT REST INTERACT {Position} {InteractableLocation?.Name}");
        Interior.IsMenuInteracting = true;
        Interior?.RemoveButtonPrompts();
        RemovePrompt();
        SetupCamera(false);
        if (!WithWarp)
        {
            if (!MoveToPosition())
            {
                Interior.IsMenuInteracting = false;
                Game.DisplayHelp("Resting Failed");
                LocationCamera?.StopImmediately(true);
                return;
            }
        }
        EntryPoint.WriteToConsole("REST INTERACT GOT TO THE ANIMATION START");
        if (!DoRestAnimation())
        {
            Interior.IsMenuInteracting = false;
            Game.DisplayHelp("Resting Failed");
            LocationCamera?.StopImmediately(true);
            return;
        }
        Player.InteriorManager.OnStartedInteriorInteract();
        RestableLocation.CreateRestMenu(true);
        Interior.IsMenuInteracting = false;
        DoGetUpAnimation();
        LocationCamera?.ReturnToGameplay(true);
        LocationCamera?.StopImmediately(true);
        Player.InteriorManager.OnEndedInteriorInteract();
    }
    public override void AddPrompt()
    {
        if (Player == null)
        {
            return;
        }
        Player.ButtonPrompts.AttemptAddPrompt(Name, ButtonPromptText, Name, Settings.SettingsManager.KeySettings.InteractStart, 999);
    }
    private bool DoRestAnimation()
    {
        Player.Character.Position = Position;
        Player.Character.Heading = Heading;
        HashSet<string> dictionaryList = new HashSet<string>();

        if (UseDefaultAnimations)
        {
            EntryPoint.WriteToConsole("UseDefaultAnimations");
            StartAnimations = new List<AnimationBundle>() { new AnimationBundle("savem_default@", "m_getin_l", (int)(eAnimationFlags.AF_HOLD_LAST_FRAME | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" } };
            LoopAnimations = new List<AnimationBundle>() { new AnimationBundle("savem_default@", "m_sleep_l_loop", (int)(eAnimationFlags.AF_LOOPING | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" } };
            EndAnimations = new List<AnimationBundle>() { new AnimationBundle("savem_default@", "m_getout_l", (int)(eAnimationFlags.AF_HOLD_LAST_FRAME | eAnimationFlags.AF_TURN_OFF_COLLISION), 4.0f, -4.0f) { Gender = "U" } };
        }

        AnimationBundle startBundle = StartAnimations.Where(x => x.Gender == "U" || x.Gender == Player.Gender).PickRandom();
        if(startBundle != null)
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
        foreach(string dictionary in dictionaryList)
        {
            if (!AnimationDictionary.RequestAnimationDictionayResult(dictionary))
            {
                return false;
            }
        }
        if(startBundle != null)
        {
            EntryPoint.WriteToConsole($"START: {startBundle.Dictionary}-{startBundle.Name}");
            NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, startBundle.Dictionary, startBundle.Name, startBundle.BlendIn, startBundle.BlendOut, startBundle.Time, startBundle.Flags, 0, false, false, false);
            WaitForAnimation(endBundle.Dictionary, startBundle.Name);
        }
        if(loopBundle != null)
        {
            EntryPoint.WriteToConsole($"LOOP: {loopBundle.Dictionary}-{loopBundle.Name}");
            NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, loopBundle.Dictionary, loopBundle.Name, loopBundle.BlendIn, loopBundle.BlendOut, loopBundle.Time, loopBundle.Flags, 0, false, false, false);
        }
        return true;
    }
    public bool DoGetUpAnimation()
    {
        if (endBundle != null)
        {
            EntryPoint.WriteToConsole($"END: {endBundle.Dictionary}-{endBundle.Name}");
            NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, endBundle.Dictionary, endBundle.Name, endBundle.BlendIn, endBundle.BlendOut, endBundle.Time, endBundle.Flags, 0, false, false, false);
            WaitForAnimation(endBundle.Dictionary, endBundle.Name);
        }
        NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
        return true;
    }

}

