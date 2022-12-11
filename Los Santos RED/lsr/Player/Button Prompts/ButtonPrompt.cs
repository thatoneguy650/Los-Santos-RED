using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public class ButtonPrompt
{
    public ButtonPrompt(string text, string group, string identifier, Keys key, Keys modifer, int order)
    {
        Text = text;
        Key = key;
        Modifier = modifer;
        Identifier = identifier;
        Group = group;
        Order = order;
    }
    public ButtonPrompt(string text, string group, string identifier, Keys key, int order)
    {
        Text = text;
        Key = key;
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
    public bool HasGameControl { get; set; } = false;
    public GameControl GameControl { get; set; }
    public string Identifier { get; set; }
    public string Text { get; set; }
    public Keys Key { get; set; }
    public Keys Modifier { get; set; } = Keys.None;
    public bool IsPressedNow { get; set; }
    public string Group { get; set; }
    public int Order { get; set; }
    public bool IsHeldNow { get; set; }

    public bool IsAlternativePressed { get; set; }
    public uint GameTimeAlternativePressed { get; set; }

    public void SetAlternativePressed()
    {
        IsAlternativePressed = true;
        GameTimeAlternativePressed = Game.GameTime;
    }

}


