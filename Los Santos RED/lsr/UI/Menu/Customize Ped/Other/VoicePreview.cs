using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class VoicePreview
{
    public VoicePreview()
    {
    }

    public VoicePreview(string voiceName, string typeName, string gender, string ageName, string descriptor)
    {
        VoiceName = voiceName;
        TypeName = typeName;
        Gender = gender;
        AgeName = ageName;
        Descriptor = descriptor;
    }

    public string VoiceName { get; set; }
    public string TypeName { get; set; }
    public string Gender { get; set; }
    public string AgeName { get; set; }
    public string Descriptor { get; set; }
    public override string ToString()
    {
        return VoiceName.ToString();
    }

    public string GetFullDescription()
    {
        string toReturn = $"Type: {TypeName}";
        toReturn += $"~n~Gender: {Gender}";
        toReturn += $"~n~Age: {AgeName}";
        toReturn += $"~n~Desc: {Descriptor}";
        return toReturn;
    }
}

