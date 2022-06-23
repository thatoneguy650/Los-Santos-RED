using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
[Serializable]
public class PedComponent
{
    public int ComponentID { get; set; }
    public int DrawableID { get; set; }
    public int TextureID { get; set; }
    public int PaletteID { get; set; }
    public PedComponent()
    {

    }
    public PedComponent(int _ComponentID)
    {
        ComponentID = _ComponentID;
    }
    public PedComponent(int _ComponentID,int _DrawableID,int _TextureID,int _PaletteID)
    {
        ComponentID = _ComponentID;
        DrawableID = _DrawableID;
        TextureID = _TextureID;
        PaletteID = _PaletteID;
    }
    public PedComponent(int _ComponentID, int _DrawableID, int _TextureID)
    {
        ComponentID = _ComponentID;
        DrawableID = _DrawableID;
        TextureID = _TextureID;
        PaletteID = 0;
    }
}

