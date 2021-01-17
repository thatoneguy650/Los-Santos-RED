using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public class ButtonPrompt
{
    public ButtonPrompt(string text, Keys key, string name, string group)
    {
        Text = text;
        Key = key;
        Name = name;
        Group = group;
    }

    public string Text { get; set; }
    public Keys Key { get; set; }
    public bool IsPressedNow { get; set; }
    public string Name { get; set; }
    public string Group { get; set; }
}


