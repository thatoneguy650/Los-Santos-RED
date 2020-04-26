using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
[Serializable]
public class PropComponent
{
    public int PropID { get; set; }
    public int DrawableID { get; set; }
    public int TextureID { get; set; }
    public PropComponent()
    {

    }
    public PropComponent(int _PropID,int _DrawableID, int _TextureID)
    {
        PropID = _PropID;
        DrawableID = _DrawableID;
        TextureID = _TextureID;
    }
}
