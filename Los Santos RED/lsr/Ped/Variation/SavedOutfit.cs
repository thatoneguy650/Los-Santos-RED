using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class SavedOutfit
{
    public string Name { get; set; }
    public string ModelName { get; set; }
    public PedVariation PedVariation { get; set; }
    public SavedOutfit()
    {
    }

    public SavedOutfit(string name, string modelName, PedVariation pedVariation)
    {
        Name = name;
        ModelName = modelName;
        PedVariation = pedVariation;
    }
}

