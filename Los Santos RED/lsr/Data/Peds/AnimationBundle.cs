using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class AnimationBundle
{
    public AnimationBundle()
    {
    }

    public AnimationBundle(string dictionary, string name, int flags, float blendIn, float blendOut)
    {
        Dictionary = dictionary;
        Name = name;
        Flags = flags;
        BlendIn = blendIn;
        BlendOut = blendOut;
    }

    public string Dictionary { get; set; }
    public string Name { get; set; }
    public int Flags { get; set; } = 0;
    public float BlendIn { get; set; } = 8.0f;
    public float BlendOut { get; set; } = -8.0f;
    public int Time { get; set; } = -1;
    public string Gender { get; set; } = "U";
}

