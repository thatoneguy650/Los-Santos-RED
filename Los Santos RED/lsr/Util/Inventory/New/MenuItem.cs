using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class MenuItem
{
    public MenuItem()
    {

    }

    public MenuItem(string modItemName, int price)
    {
        ModItemName = modItemName;
        Price = price;
    }
    public string ModItemName { get; set; }
    public int Price { get; set;} = 5;
}

