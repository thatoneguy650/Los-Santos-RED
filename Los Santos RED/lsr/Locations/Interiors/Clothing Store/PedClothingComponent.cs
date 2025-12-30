using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
[Serializable]
public class PedClothingComponent
{
    public int ComponentID { get; set; }
    public int DrawableID { get; set; }
    public List<int> PossibleTextures { get; set; }
    public PedClothingComponent()
    {

    }
    public PedClothingComponent(int _ComponentID)
    {
        ComponentID = _ComponentID;
    }
    public PedClothingComponent(int _ComponentID, int _DrawableID, List<int> _possibleTextures)
    {
        ComponentID = _ComponentID;
        DrawableID = _DrawableID;
        PossibleTextures = _possibleTextures;
    }

}

