using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;


public class ZoneMenu
{
    public ZoneMenu()
    {
    }

    public ZoneMenu(string menuGroupID, int selectChance)
    {
        MenuGroupID = menuGroupID;
        SelectChance = selectChance;
    }
    public string MenuGroupID { get; set; }
    public int SelectChance { get; set; } = 0;
}

