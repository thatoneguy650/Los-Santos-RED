using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class PropComponent
{
    public int ComponentID { get; set; }
    public int DrawableID { get; set; }
    public int TextureID { get; set; }
    public int PaletteID { get; set; }
    public PropComponent()
    {

    }
    public PropComponent(int _ComponentID)
    {
        ComponentID = _ComponentID;
    }
    public PropComponent(int _ComponentID, int _DrawableID, int _TextureID, int _PaletteID)
    {
        ComponentID = _ComponentID;
        DrawableID = _DrawableID;
        TextureID = _TextureID;
        PaletteID = _PaletteID;
    }
}
