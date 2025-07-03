using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    public class HideableObject
    {
    public HideableObject()
    {
    }

    public HideableObject(uint modelHash, string name, string buttonPrompt)
    {
        ModelHash = modelHash;
        Name = name;
        ButtonPrompt = buttonPrompt;
    }

    public uint ModelHash { get; set; }
    public string Name { get; set; }
    public string ButtonPrompt { get; set; }
    public bool IsDoor { get; set; }
}

