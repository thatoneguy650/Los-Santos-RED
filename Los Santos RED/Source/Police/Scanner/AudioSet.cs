using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AudioSet
{
    public List<string> Sounds { get; set; }
    public string Subtitles { get; set; }
    public AudioSet(List<string> sounds, string subtitles)
    {
        Sounds = sounds;
        Subtitles = subtitles;
    }
}