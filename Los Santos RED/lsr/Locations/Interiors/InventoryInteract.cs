using ExtensionsMethods;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

public class InventoryInteract : InteriorInteract
{
    private AnimationBundle endBundle;
    private List<AnimationBundle> StartAnimations = new List<AnimationBundle>() { new AnimationBundle("amb@world_human_window_shop@male@enter", "enter",0,2.0f,-2.0f) };
    private List<AnimationBundle> LoopAnimations = new List<AnimationBundle>() { new AnimationBundle("amb@world_human_window_shop@male@idle_a", "browse_a", 1, 2.0f, -2.0f), new AnimationBundle("amb@world_human_window_shop@male@idle_a", "browse_b", 1, 2.0f, -2.0f), new AnimationBundle("amb@world_human_window_shop@male@idle_a", "browse_c", 1, 2.0f, -2.0f) };
    private List<AnimationBundle> EndAnimations = new List<AnimationBundle>() { new AnimationBundle("amb@world_human_window_shop@male@exit", "exit", 0, 2.0f, -2.0f) };
    public bool CanAccessItems { get; set; } = true;
    public bool CanAccessWeapons { get; set; } = true;
    public bool CanAccessCash { get; set; } = true;
    public List<ItemType> AllowedItemTypes { get; set; }
    public List<ItemType> DisallowedItemTypes { get; set; }
    public bool RemoveMenuBanner { get; set; } = true;
    public string Title { get; set; }
    public string Description { get; set; }
    [XmlIgnore]
    public IInventoryableLocation InventoryableLocation { get; set; }
    public InventoryInteract()
    {
    }

    public InventoryInteract(string name, Vector3 position, float heading, string buttonPromptText) : base(name, position, heading, buttonPromptText)
    {

    }
    public override void OnInteract()
    {
        Interior.IsMenuInteracting = true;
        Interior?.RemoveButtonPrompts();
        RemovePrompt();
        SetupCamera(false);
        if (!MoveToPosition())
        {
            Interior.IsMenuInteracting = false;
            Game.DisplayHelp("Access Failed");
            LocationCamera?.StopImmediately(true);
            return;
        }
        Player.InteriorManager.OnStartedInteriorInteract();
        PerformAnimation();
        InventoryableLocation.CreateInventoryMenu(CanAccessItems, CanAccessWeapons, CanAccessCash, AllowedItemTypes, DisallowedItemTypes, RemoveMenuBanner, Title, Description);
        StopPerformingAnimation();
        LocationCamera?.ReturnToGameplay(true);
        LocationCamera?.StopImmediately(true);
        Interior.IsMenuInteracting = false;
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
    private bool PerformAnimation()
    {
        Player.Character.Position = Position;
        Player.Character.Heading = Heading;
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
            //WaitForAnimation(endBundle.Dictionary, startBundle.Name);
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
            //WaitForAnimation(endBundle.Dictionary, endBundle.Name);
        }
        NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
        return true;
    }

}

