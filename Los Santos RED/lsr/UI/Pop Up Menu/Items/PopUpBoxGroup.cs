using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PopUpBoxGroup
{
    public PopUpBoxGroup()
    {

    }
    public PopUpBoxGroup(string iD, List<PopUpBox> popUpBoxes)
    {
        ID = iD;
        PopUpBoxes = popUpBoxes;
    }
    public bool IsChild { get; set; } = false;
    public string ID { get; set; } = "";
    public List<PopUpBox> PopUpBoxes { get; set; } = new List<PopUpBox>();
    public string Group { get; set; } = "";
}

