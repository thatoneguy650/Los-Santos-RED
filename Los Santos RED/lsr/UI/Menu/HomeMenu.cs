//using LosSantosRED.lsr.Interface;
//using Rage;
//using RAGENativeUI;
//using RAGENativeUI.Elements;
//using System.Collections.Generic;

//public class HomeMenu : Menu
//{
//    private UIMenu Main;
//    private IActionable Player;
//    private UIMenuItem SleepMenu;
//    private UIMenuItem LeaveMenu;
//    private GameLocation Home;

//    public HomeMenu(MenuPool menuPool, IActionable player, GameLocation home)
//    {
//        Player = player;
//        Home = home;
//        Main = new UIMenu(Home.Name, "Select an Option");
//        Main.SetBannerType(EntryPoint.LSRedColor);
//        menuPool.Add(Main);
//        Main.OnItemSelect += OnItemSelect;
//        Main.OnListChange += OnListChange;
//        CreateMainMenu();
//    }
//    public override void Hide()
//    {
//        Main.Visible = false;
//    }
//    public override void Show()
//    {
//        if (!Main.Visible)
//        {
//            Main.Visible = true;
//        }
//    }
//    public override void Toggle()
//    {
//        if (!Main.Visible)
//        {
//            Main.Visible = true;
//        }
//        else
//        {
//            Main.Visible = false;
//        }
//    }
//    private void CreateMainMenu()
//    {
//        SleepMenu = new UIMenuItem("Sleep", "Sleep for a few hours to regain stamina.");
//        SleepMenu.RightBadge = UIMenuItem.BadgeStyle.Art;
//        LeaveMenu = new UIMenuItem("Exit", "Leave home.");
//        LeaveMenu.RightBadge = UIMenuItem.BadgeStyle.Star;
//        Main.AddItem(SleepMenu);
//        Main.AddItem(LeaveMenu);
//    }
//    private void OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
//    {
//        if (selectedItem == SleepMenu)
//        {
//            Player.DisplayPlayerNotification();
//        }
//        else if (selectedItem == LeaveMenu)
//        {
//            EntryPoint.ModController.Dispose();
//        }
//        Main.Visible = false;
//    }
//    private void OnListChange(UIMenu sender, UIMenuListItem list, int index)
//    {

//    }
//}