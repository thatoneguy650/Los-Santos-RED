using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PopUpMenuMap
{
    public PopUpMenuMap()
    {

    }
    public PopUpMenuMap(int iD, string display, Action action)
    {
        ID = iD;
        Display = display;
        Action = action;
    }

    public int ID { get; set; }
    public string Display { get; set; }
    public Action Action { get; set; }
    public bool ClosesMenu { get; set; } = true;
}

