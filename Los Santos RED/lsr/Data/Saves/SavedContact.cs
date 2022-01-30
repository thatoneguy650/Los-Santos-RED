using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class SavedContact
{
    public SavedContact()
    {

    }

    public SavedContact(string name, int index, string iconName)
    {
        Name = name;
        Index = index;
        IconName = iconName;
    }

    public string Name { get; set; } = "";
    public int Index { get; set; } = 0;
    public string IconName { get; set; } = "";

}

