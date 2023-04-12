using Rage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PopUpBox
{
    public PopUpBox()
    {

    }
    public PopUpBox(int iD, string display, Action action, string description)
    {
        ID = iD;
        Display = display;
        Action = action;
        Description = description;
    }
    public PopUpBox(int iD, string display, string childMenuID, string description)
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

    public bool HasIcon { get; private set; }
    public string IconNameDefault { get; set; }
    public Texture IconDefault { get; set; }
    public string IconNameSelected { get; set; }
    public Texture IconSelected { get; set; }
    public string IconNameInvalid { get; set; }
    public Texture IconInvalid { get; set; }
    public void MakeTexture()
    {
        if (File.Exists($"Plugins\\LosSantosRED\\images\\hudicons\\{IconNameDefault}")) 
        {
            IconDefault = Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\hudicons\\{IconNameDefault}");
            HasIcon = true;
            //EntryPoint.WriteToConsoleTestLong($"MADE TEXTURE {IconNameDefault}");
        }
        if (File.Exists($"Plugins\\LosSantosRED\\images\\hudicons\\{IconNameSelected}"))
        {
            IconSelected = Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\hudicons\\{IconNameSelected}");
            //EntryPoint.WriteToConsoleTestLong($"MADE TEXTURE {IconNameSelected}");
        }
        if (File.Exists($"Plugins\\LosSantosRED\\images\\hudicons\\{IconNameInvalid}"))
        {
            IconInvalid = Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\hudicons\\{IconNameInvalid}");
            //EntryPoint.WriteToConsoleTestLong($"MADE TEXTURE {IconNameInvalid}");
        }
        if(IconDefault == null || IconSelected == null || IconInvalid == null)
        {
            HasIcon = false;
            
        }
        else
        {
            //EntryPoint.WriteToConsoleTestLong($"HAS ALL ICONS: {Display}");
        }
    }
}

