using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class PedComponentShopMenu
{
    public PedComponentShopMenu()
    {

    }

    public PedComponentShopMenu(string modelName, int componentID, int drawableID,  int textureID, int price)
    {
        ModelName = modelName;
        ComponentID = componentID;
        DrawableID = drawableID;

        TextureID = textureID;
        Price = price;
    }
    public PedComponentShopMenu(string modelName,string debugName, int componentID, int drawableID, int textureID, int price)
    {
        ModelName = modelName;
        DebugName = debugName;
        ComponentID = componentID;
        DrawableID = drawableID;

        TextureID = textureID;
        Price = price;
    }
    public string ModelName { get; set; }
    public string DebugName { get; set; }
    public int ComponentID { get; set; }
    public int DrawableID { get; set; }
    public int TextureID { get; set; }
    public int Price { get; set; }
}

