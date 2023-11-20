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

public class RestInteract : InteriorInteract
{
    protected LocationCamera LocationCamera;
    [XmlIgnore]
    public IRestableLocation RestableLocation {get;set;}
    public string AnimationStartDictionaryName { get; set; } = "savem_default@";
    public string AnimationStartName { get; set; } = "m_getin_l";
    public string AnimationLoopDictionaryName { get; set; } = "savem_default@";
    public string AnimationLoopName { get; set; } = "m_sleep_l_loop";
    public string AnimationEndDictionaryName { get; set; } = "savem_default@";
    public string AnimationEndName { get; set; } = "m_getout_l";

    public RestInteract()
    {
    }

    public RestInteract(Vector3 position, float heading, string buttonPromptText) : base(position, heading, buttonPromptText)
    {

    }

    public override void OnInteract()
    {
        Interior?.RemoveButtonPrompts();
        RemovePrompt();
        SetupCamera();
        if (!MoveToPosition())
        {
            Game.DisplayHelp("Resting Failed");
            return;
        }
        if (!DoRestAnimation())
        {
            Game.DisplayHelp("Resting Failed");
            return;
        }
        Interior.IsMenuInteracting = true;
        RestableLocation.CreateRestMenu();
        DoGetUpAnimation();
    }
    public override void AddPrompt()
    {
        ButtonPromptIndetifierText = "RestInteriorLocation";
        if (Player == null)
        {
            return;
        }
        Player.ButtonPrompts.AddPrompt(ButtonPromptIndetifierText, ButtonPromptText, ButtonPromptIndetifierText, Settings.SettingsManager.KeySettings.InteractStart, 999);
    }
    private void SetupCamera()
    {
        if (CameraPosition != Vector3.Zero)
        {
            if (LocationCamera == null)
            {
                LocationCamera = new LocationCamera(RestableLocation.GameLocation, LocationInteractable, Settings, true);
            }
            LocationCamera.MoveToPosition(CameraPosition, CameraDirection, CameraRotation);
        }
    }
    private bool DoRestAnimation()
    {
        Player.Character.Position = Position;
        Player.Character.Heading = Heading;
        if (!AnimationDictionary.RequestAnimationDictionayResult(AnimationStartDictionaryName) || !AnimationDictionary.RequestAnimationDictionayResult(AnimationLoopDictionaryName) || !AnimationDictionary.RequestAnimationDictionayResult(AnimationEndDictionaryName))
        {
            return false;
        }
        NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, AnimationStartDictionaryName, AnimationStartName, 4.0f, -4.0f, -1, (int)(eAnimationFlags.AF_HOLD_LAST_FRAME | eAnimationFlags.AF_TURN_OFF_COLLISION), 0, false, false, false);
        WaitForAnimation(AnimationStartDictionaryName, AnimationStartName);
        NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, AnimationLoopDictionaryName, AnimationLoopName, 4.0f, -4.0f, -1, (int)(eAnimationFlags.AF_LOOPING | eAnimationFlags.AF_TURN_OFF_COLLISION), 0, false, false, false);
        return true;
    }
    public bool DoGetUpAnimation()
    {
        NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, AnimationEndDictionaryName, AnimationEndName, 4.0f, -4.0f, -1, (int)(eAnimationFlags.AF_HOLD_LAST_FRAME | eAnimationFlags.AF_TURN_OFF_COLLISION), 0, false, false, false);
        WaitForAnimation(AnimationEndDictionaryName, AnimationEndName);
        NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
        return true;
    }

}

