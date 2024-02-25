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

public class SinkInteract : InteriorInteract
{
    public bool IsScenario { get; set; } = false;
    public SinkInteract()
    {

    }

    public SinkInteract(string name, Vector3 position, float heading, string buttonPromptText) : base(name, position, heading, buttonPromptText)
    {

    }

    public override void OnInteract()
    {
        Interior.IsMenuInteracting = true;
        Interior?.RemoveButtonPrompts();
        RemovePrompt();
        SetupCamera(false);
        if (!WithWarp)
        {
            if (!MoveToPosition())
            {
                Interior.IsMenuInteracting = false;
                Game.DisplayHelp("Interact Failed");
                LocationCamera?.StopImmediately(true);
                return;
            }
        }
        if (IsScenario)
        {
            if (!UseScenario())
            {
                Interior.IsMenuInteracting = false;
                Game.DisplayHelp("Interact Failed");
                LocationCamera?.StopImmediately(true);
                return;
            }
        }
        else
        {
            if (!PerformAnimation())
            {
                Interior.IsMenuInteracting = false;
                Game.DisplayHelp("Interact Failed");
                LocationCamera?.StopImmediately(true);
                return;
            }
        }
        uint GameTimeStartedWashing = Game.GameTime;
        GameFiber.Sleep(2000);
        bool HasCleaned = false;
        while (!Player.IsMoveControlPressed && Player.IsAliveAndFree)
        {
            if(!HasCleaned && Game.GameTime - GameTimeStartedWashing >= 5000)
            {
                Player.Character.ClearBlood();
                HasCleaned = true;
            }
            GameFiber.Yield();
        }
        Interior.IsMenuInteracting = false;
        NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
        LocationCamera?.ReturnToGameplay(true);
        LocationCamera?.StopImmediately(true);

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
        AnimationBundle LoopAnimation = new AnimationBundle()
        {
            Dictionary = "switch@michael@wash_face",
            Name = "loop_michael",
            Flags = 1,
            BlendIn = 2f,
            BlendOut = -2f,
        };
        if (!AnimationDictionary.RequestAnimationDictionayResult(LoopAnimation.Dictionary))
        {
            return false;
        }
        NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, LoopAnimation.Dictionary, LoopAnimation.Name, LoopAnimation.BlendIn, LoopAnimation.BlendOut, LoopAnimation.Time, LoopAnimation.Flags, 0, false, false, false);    
        return true;
    }
    public bool StopPerformingAnimation()
    {

        return true;
    }

}

