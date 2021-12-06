using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
[Serializable]
public class PedPropComponent
{
    public int PropID { get; set; }
    public int DrawableID { get; set; }
    public int TextureID { get; set; }
    public PedPropComponent()
    {

    }
    public PedPropComponent(int _PropID,int _DrawableID, int _TextureID)
    {
        PropID = _PropID;
        DrawableID = _DrawableID;
        TextureID = _TextureID;
    }
}
