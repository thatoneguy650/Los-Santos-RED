using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class FashionItemLookup
{
    public FashionItemLookup(bool isProp, int itemID, int drawableID, int textureID, string gender, string gXT, string name)
    {
        IsProp = isProp;
        ItemID = itemID;
        DrawableID = drawableID;
        TextureID = textureID;
        Gender = gender;
        GXT = gXT;
        Name = name;
    }

    public bool IsProp { get; set; }
    public int ItemID { get; set; }
    public int DrawableID { get; set; }
    public int TextureID { get; set; }
    public string Gender { get; set; }
    public string GXT { get; set; }
    public string Name { get; set; }



    public string Category { get; set; }
    public string DrawableName { get; set; }
    public string TextureName { get; set; }


    public string GetDrawableString()
    {
        if (DrawableName == "" || string.IsNullOrEmpty(DrawableName) || DrawableName is null)
        {
            return $"{Name} ({DrawableID})";
        }
        else
        {
            return $"{DrawableName} ({DrawableID})";
        }
    }
    public string GetTextureString()
    {
        if(TextureName == "" || string.IsNullOrEmpty(TextureName) || TextureName is null)
        {
            return $"{Name} ({TextureID})";
        }
        else
        {
            return $"{TextureName} ({TextureID})";
        }
    }
}

