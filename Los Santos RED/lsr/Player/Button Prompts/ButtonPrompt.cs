using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public class ButtonPrompt
{
    public ButtonPrompt(string text, string group, string identifier, Keys key, Keys modiferKey, int order)
    {
        HasKeyButtons = true;
        Text = text;
        Key = key;
        ModifierKey = modiferKey;
        Identifier = identifier;
        Group = group;
        Order = order;
    }
    public ButtonPrompt(string text, string group, string identifier, Keys key, int order)
    {
        HasKeyButtons = true;
        Text = text;
        Key = key;
        Identifier = identifier;
        Group = group;
        Order = order;
    }



    public ButtonPrompt(string text, string group, string identifier, ControllerButtons button, ControllerButtons modifierButton, int order)
    {
        HasControllerButtons = true;
        Text = text;
        ControllerButton = button;
        ControllerButtonModifier = modifierButton;
        Identifier = identifier;
        Group = group;
        Order = order;
    }
    public ButtonPrompt(string text, string group, string identifier, ControllerButtons button, int order)
    {
        HasControllerButtons = true;
        Text = text;
        ControllerButton = button;
        Identifier = identifier;
        Group = group;
        Order = order;
    }




    public ButtonPrompt(string text, string group, string identifier, GameControl gameControl, int order)
    {
        Text = text;
        GameControl = gameControl;
        HasGameControl = true;
        Identifier = identifier;
        Group = group;
        Order = order;
    }
    public ButtonPrompt(string text, string group, string identifier, GameControl gameControl, GameControl modifierGameControl, int order)
    {
        Text = text;
        GameControl = gameControl;
        GameControlModifier = modifierGameControl;
        HasGameControl = true;
        HasGameControlModifier = true;
        Identifier = identifier;
        Group = group;
        Order = order;
    }
    public bool HasGameControl { get; set; } = false;
    public GameControl GameControl { get; set; }
    public bool HasGameControlModifier { get; set; } = false;
    public GameControl GameControlModifier { get; set; }
    public string Identifier { get; set; }
    public string Text { get; set; }

    public bool HasKeyButtons { get; set; } = false;

    public Keys Key { get; set; }
    public Keys ModifierKey { get; set; } = Keys.None;


    public bool HasControllerButtons { get; set; } = false;
    public ControllerButtons ControllerButton { get; set; }
    public ControllerButtons ControllerButtonModifier { get; set; } = ControllerButtons.None;

    public bool IsPressedNow { get; set; }
    public string Group { get; set; }
    public int Order { get; set; }
    public bool IsHeldNow { get; set; }

    public bool IsFakePressed { get; set; }
    public uint GameTimeFakePressed { get; set; }
    public Action Action { get; set; }
    public void SetFakePressed()
    {
        IsFakePressed = true;
        GameTimeFakePressed = Game.GameTime;
    }

    public void UpdateState()
    {
        if (HasKeyButtons && Game.IsKeyDownRightNow(Key) && (ModifierKey == Keys.None || Game.IsKeyDownRightNow(ModifierKey)) && !IsHeldNow)
        {
            IsHeldNow = true;
        }
        else if (HasControllerButtons && Game.IsControllerButtonDownRightNow(ControllerButton) && (ModifierKey == Keys.None || Game.IsControllerButtonDown(ControllerButtonModifier)) && !IsHeldNow)
        {
            IsHeldNow = true;
        }
        else if (IsFakePressed)
        {
            IsHeldNow = true;
        }
        else
        {
            IsHeldNow = false;
        }

        if (HasKeyButtons && Game.IsKeyDown(Key) && (ModifierKey == Keys.None || Game.IsKeyDownRightNow(ModifierKey)) && !IsPressedNow)
        {
            //EntryPoint.WriteToConsole($"INPUT! Control :{Text}: Down 1");
            IsPressedNow = true;
        }


        else if(HasControllerButtons && Game.IsControllerButtonDown(ControllerButton) && (ControllerButtonModifier == ControllerButtons.None || Game.IsControllerButtonDown(ControllerButtonModifier)) && !IsPressedNow)
        {
            //EntryPoint.WriteToConsoleTestLong($"INPUT! Control :{Text}: Down 1.5");
            IsPressedNow = true;
        }


        else if (HasGameControl && !HasGameControlModifier && Game.IsControlJustPressed(2, GameControl) && !IsPressedNow)
        {
            //EntryPoint.WriteToConsoleTestLong($"INPUT! Control :{Text}: Down 2");
            IsPressedNow = true;
        }
        else if (HasGameControl && !HasGameControlModifier && NativeFunction.Natives.IS_DISABLED_CONTROL_JUST_PRESSED<bool>(2, (int)GameControl) && !IsPressedNow)
        {
            //EntryPoint.WriteToConsoleTestLong($"INPUT! Control :{Text}: Down 3");
            IsPressedNow = true;
        }

        else if (HasGameControl && HasGameControlModifier && Game.IsControlJustPressed(2, GameControlModifier) && Game.IsControlJustPressed(2, GameControl) && !IsPressedNow)
        {
            //EntryPoint.WriteToConsoleTestLong($"INPUT! Control :{Text}: Down 4");
            IsPressedNow = true;
        }
        else if (HasGameControl && HasGameControlModifier && NativeFunction.Natives.IS_DISABLED_CONTROL_JUST_PRESSED<bool>(2, (int)GameControlModifier) && NativeFunction.Natives.IS_DISABLED_CONTROL_JUST_PRESSED<bool>(2, (int)GameControl) && !IsPressedNow)
        {
            //EntryPoint.WriteToConsoleTestLong($"INPUT! Control :{Text}: Down 5");
            IsPressedNow = true;
        }


        else if (IsFakePressed)
        {
            //EntryPoint.WriteToConsoleTestLong($"INPUT! Control :{Text}: Down 6");
            IsPressedNow = true;
        }
        else
        {
            IsPressedNow = false;
        }
        if (Game.GameTime - GameTimeFakePressed >= 20)
        {
            IsFakePressed = false;
        }
    }
    public void Reset()
    {
        IsHeldNow = false;
        IsPressedNow = false;
        IsFakePressed = false;
    }

}


