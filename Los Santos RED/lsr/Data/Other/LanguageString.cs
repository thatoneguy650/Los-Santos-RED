using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class LanguageString
{
    public LanguageString()
    {
    }

    public LanguageString(string iD, string text)
    {
        ID = iD;
        Text = text;
    }

    public string ID { get; set; }
    public string Text { get; set; }
}

