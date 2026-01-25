using LosSantosRED.lsr.Interface;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DebugOutfitSubMenu : DebugSubMenu
{
    public DebugOutfitSubMenu(UIMenu debug, MenuPool menuPool, IActionable player) : base(debug, menuPool, player)
    {

    }

    public override void AddItems()
    {
        SubMenu = MenuPool.AddSubMenu(Debug, "Outfit Menu");
        Debug.MenuItems[Debug.MenuItems.Count() - 1].Description = "Quick set a variation for the current character.";
        SubMenu.SetBannerType(EntryPoint.LSRedColor);
        //Player.OutfitManager.CreateOutfitMenu(MenuPool, SubMenu, false, false, false);
    }
    public override void Update()
    {
        Player.OutfitManager.CreateOutfitMenu(MenuPool, SubMenu, false, false, true);
    }
}

