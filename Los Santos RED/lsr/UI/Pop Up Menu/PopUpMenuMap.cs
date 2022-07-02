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
    public PopUpMenuMap(int iD, string display, Action action, string description)
    {
        ID = iD;
        Display = display;
        Action = action;
        Description = description;
    }
    public PopUpMenuMap(int iD, string display, string childMenuID, string description)
    {
        ID = iD;
        Display = display;
        ChildMenuID = childMenuID;
        Description = description;
    }
    public int ID { get; set; }
    public string Display { get; set; }
    public Action Action { get; set; }
    public bool ClosesMenu { get; set; } = true;
    public string Description { get; set; }
    public string ChildMenuID { get; set; }


    public Func<bool> IsCurrentlyValid { get; set; } = new Func<bool>(() => true);
}

