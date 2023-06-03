using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GTANotification
{
    public GTANotification(string title, string subtitle, string text)
    {
        Title = title;
        Subtitle = subtitle;
        Text = text;
    }

    public GTANotification(string textureDictionary, string texture, string title, string subtitle, string text)
    {
        TextureDictionary = textureDictionary;
        Texture = texture;
        Title = title;
        Subtitle = subtitle;
        Text = text;
    }

    public string TextureDictionary { get; set; } = "CHAR_BLANK_ENTRY";
    public string Texture { get; set; } = "CHAR_BLANK_ENTRY";
    public string Title { get; set; }
    public string Subtitle { get; set; }
    public string Text { get; set; }
    public void Display()
    {
        if(string.IsNullOrEmpty(TextureDictionary) || string.IsNullOrEmpty(Texture) || string.IsNullOrEmpty(Title) || string.IsNullOrEmpty(Subtitle))
        { 
            return; 
        }
        Game.DisplayNotification(TextureDictionary, Texture, Title, Subtitle, Text);
    }
}

