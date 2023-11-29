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



        UIMenuItem RemoveButtonPrompts = new UIMenuItem("Remove Prompts", "Removes all the button prompts");
        RemoveButtonPrompts.Activated += (menu, item) =>
        {
            Player.ButtonPrompts.Clear();
            menu.Visible = false;
        };

        OtherItemsMenu.AddItem(RemoveButtonPrompts);


        UIMenuListScrollerItem<string> SetArrested = new UIMenuListScrollerItem<string>("Set Arrested", "Set the player ped as arrested.", new List<string>() { "Stay Standing", "Kneeling" });
        SetArrested.Activated += (menu, item) =>
        {
            bool stayStanding = SetArrested.SelectedItem == "Stay Standing";
            Player.Arrest();
            Game.TimeScale = 1.0f;
            Player.Surrendering.SetArrestedAnimation(stayStanding);
            menu.Visible = false;
        };

        UIMenuItem UnSetArrested = new UIMenuItem("UnSet Arrested", "Release the player from an arrest.");
        UnSetArrested.Activated += (menu, item) =>
        {
            Game.TimeScale = 1.0f;
            Player.Reset(true, false, true, true, true, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false);
            Player.Surrendering.UnSetArrestedAnimation();
            menu.Visible = false;
        };
        OtherItemsMenu.AddItem(SetArrested);
        OtherItemsMenu.AddItem(UnSetArrested);


    }

}

