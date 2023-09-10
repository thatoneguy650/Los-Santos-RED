using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DebugOtherSubMenu : DebugSubMenu
{
    public DebugOtherSubMenu(UIMenu debug, MenuPool menuPool, IActionable player) : base(debug, menuPool, player)
    {
    }
    public override void AddItems()
    {
        UIMenu OtherItemsMenu = MenuPool.AddSubMenu(Debug, "Other Menu");
        OtherItemsMenu.SetBannerType(EntryPoint.LSRedColor);
        Debug.MenuItems[Debug.MenuItems.Count() - 1].Description = "Change various other items.";

    }

}

