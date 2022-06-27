using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PopUpMenuGroup
{
    public PopUpMenuGroup()
    {

    }
    public PopUpMenuGroup(string iD, List<PopUpMenuMap> popUpMenuMaps)
    {
        ID = iD;
        PopUpMenuMaps = popUpMenuMaps;
    }
    public bool IsChild { get; set; } = false;
    public string ID { get; set; } = "";
    public List<PopUpMenuMap> PopUpMenuMaps { get; set; } = new List<PopUpMenuMap>();
    public string Group { get; set; } = "";
}

