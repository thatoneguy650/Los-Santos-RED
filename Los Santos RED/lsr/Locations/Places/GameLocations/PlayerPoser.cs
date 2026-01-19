using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using RAGENativeUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


public class PlayerPoser
{
    private ISettingsProvideable Settings;
    private ILocationInteractable Player;
    private ePedFocusZone CurrentFocusZone;

    public PlayerPoser(ILocationInteractable player, ISettingsProvideable settings)
    {
        Player = player;
        Settings = settings;
    }

    public void Setup()
    {
        SetupAnimations();
    }
    public void Start()
    {
        SetDefaultAnimation();
    }
    public void Dispose()
    {
        NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
    }
    public void Reset()
    {
        SetDefaultAnimation();
    }
    public void SetHint(ePedFocusZone pedFocusZone)
    {
        CurrentFocusZone = pedFocusZone;
        if(CurrentFocusZone ==  ePedFocusZone.LeftWrist)
        {
            PoseLeftWrist();
        }
        else if(CurrentFocusZone ==  ePedFocusZone.Feet)
        {
            PoseFeet();
        }
        else if (CurrentFocusZone == ePedFocusZone.Head)
        {
            PoseHead();
        }
        else
        {
            SetDefaultAnimation();
        }
    }
    private void PoseLeftWrist()
    {
        string animIdle = "idle_a";
        animIdle = new List<string>() { "idle_a" , "idle_b" , "idle_c" , "idle_d" , "idle_e"  }.PickRandom();
        unsafe
        {
            int lol = 0;
            NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
            NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", 0, "anim@random@shop_clothes@watches", "intro", 4.0f, -4.0f, -1, 0, 0, false, false, false);
            NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", 0, "anim@random@shop_clothes@watches", animIdle, 4.0f, -4.0f, -1, 1, 0, false, false, false);
            NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
            NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
            NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Player.Character, lol);
            NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
        }
    }
    private void PoseFeet()
    {
        string animIdle = "try_shoes_neutral_a";
        animIdle = new List<string>() { "try_shoes_neutral_a", "try_shoes_neutral_d" }.PickRandom();
        unsafe
        {
            int lol = 0;
            NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
            NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", 0, "clothingshoes", "intro", 4.0f, -4.0f, -1, 0, 0, false, false, false);
            NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", 0, "clothingshoes", animIdle, 4.0f, -4.0f, -1, 1, 0, false, false, false);
            NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
            NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
            NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Player.Character, lol);
            NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
        }
    }
    private void PoseHead()
    {
        string animIdle = "try_glasses_neutral_a";
        animIdle = new List<string>() { "try_glasses_neutral_a", "try_glasses_neutral_b" }.PickRandom();
        unsafe
        {
            int lol = 0;
            NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
           // NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", 0, "clothingspecs", "intro", 4.0f, -4.0f, -1, 0, 0, false, false, false);
            NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", 0, "clothingspecs", animIdle, 4.0f, -4.0f, -1, 1, 0, false, false, false);
            NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
            NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
            NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Player.Character, lol);
            NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
        }
    }
    private void SetupAnimations()
    {
        if(!AnimationDictionary.RequestAnimationDictionayResult("anim@random@shop_clothes@watches"))
        {
            return;
        }
        if (!AnimationDictionary.RequestAnimationDictionayResult("move_clown@p_m_one_idles@"))
        {
            return;
        }
        if (!AnimationDictionary.RequestAnimationDictionayResult("clothingshoes"))
        {
            return;
        }
        if (!AnimationDictionary.RequestAnimationDictionayResult("clothingspecs"))
        {
            return;
        }
    }

    private void SetDefaultAnimation()
    {
        string dictionary = "move_clown@p_m_one_idles@";
        string anim = "fidget_look_at_outfit_01";
        NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, dictionary, anim, 2.0f, -2.0f, -1, 0, 0, false, false, false);
    }

}

