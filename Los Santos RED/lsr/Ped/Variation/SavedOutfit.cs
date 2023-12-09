using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


public class SavedOutfit
{
    public string Name { get; set; }
    public string ModelName { get; set; }
    public string CharacterName { get; set; }
    public PedVariation PedVariation { get; set; }
    public SavedOutfit()
    {
    }

    //public SavedOutfit(string name, string modelName, PedVariation pedVariation)
    //{
    //    Name = name;
    //    ModelName = modelName;
    //    PedVariation = pedVariation;
    //}
    public SavedOutfit(string name, string modelName, string characterName, PedVariation pedVariation)
    {
        Name = name;
        ModelName = modelName;
        CharacterName = characterName;
        PedVariation = pedVariation;
    }
    [OnDeserialized()]
    private void SetValuesOnDeserialized(StreamingContext context)
    {
        CharacterName = "";
    }

}

