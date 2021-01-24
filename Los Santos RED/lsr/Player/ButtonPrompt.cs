using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public class ButtonPrompt
{
    public ButtonPrompt(string text, Keys key, string group)
    {
        Text = text;
        Key = key;
        Group = group;
    }

    public string Text { get; set; }
    public Keys Key { get; set; }
    public bool IsPressedNow { get; set; }
    public string Group { get; set; }
}


