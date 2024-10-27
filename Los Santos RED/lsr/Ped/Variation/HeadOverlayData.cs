using Rage.Native;
using RAGENativeUI.Elements;
using RAGENativeUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
[Serializable]
public class HeadOverlayData
{
    public HeadOverlayData()
    {

    }
    public HeadOverlayData(int overlayID, string part)
    {
        OverlayID = overlayID;
        Part = part;
    }
    public int PrimaryColor { get; set; } = 0;
    public int SecondaryColor { get; set; } = 0;
    public int ColorType { get; set; } = 0;
    public int Index { get; set; } = 255;
    public float Opacity { get; set; } = 1.0f;
    public int OverlayID { get; set; }
    public string Part { get; set; }
    public override string ToString()
    {
        return Part;
    }

   


}