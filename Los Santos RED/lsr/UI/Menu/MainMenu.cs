using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System.Collections.Generic;

public class MainMenu : Menu
{
    private ActionMenu ActionMenu;
    private List<DistanceSelect> Distances;
    private UIMenu Main;
    private IPedswappable PedSwap;
    private IActionable Player;
    private UIMenuItem ShowStatus;
    private UIMenuListItem TakeoverRandomPed;
    public MainMenu(MenuPool menuPool, IPedswappable pedSwap, IActionable player)
    {
        PedSwap = pedSwap;
        Player = player;
        Main = new UIMenu("Los Santos RED", "Select an Option");
        menuPool.Add(Main);
        CreateMainMenu();
        ActionMenu = new ActionMenu(menuPool, Main, Player);
    }
    public float SelectedTakeoverRadius { get; set; }
    public override void Hide()
    {
        Main.Visible = false;
        ActionMenu.Hide();
    }
    public override void Show()
    {
        if (!Main.Visible)
        {
            ActionMenu.Update();
            Main.Visible = true;
            ActionMenu.Hide();
        }
    }
    public override void Toggle()
    {
        if (!Main.Visible)
        {
            ActionMenu.Update();
            Main.Visible = true;
            ActionMenu.Hide();
        }
        else
        {
            Main.Visible = false;
            ActionMenu.Hide();
        }
    }
    private void CreateMainMenu()
    {
        Main.OnItemSelect += OnItemSelect;
        Main.OnListChange += OnListChange;
        Distances = new List<DistanceSelect> { new DistanceSelect("Closest", -1f), new DistanceSelect("20 M", 20f), new DistanceSelect("40 M", 40f), new DistanceSelect("100 M", 100f), new DistanceSelect("500 M", 500f), new DistanceSelect("Any", 1000f) };
        TakeoverRandomPed = new UIMenuListItem("Takeover Random Pedestrian", "Takes over a random pedestrian around the player.", Distances);
        ShowStatus = new UIMenuItem("Show Status", "Show the player status with a notification");
        Main.AddItem(TakeoverRandomPed);
        Main.AddItem(ShowStatus);
    }
    private void OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (selectedItem == TakeoverRandomPed)
        {
            if (SelectedTakeoverRadius == -1f)
            {
                PedSwap.TakeoverPed(500f, true, false, true);
            }
            else
            {
                PedSwap.TakeoverPed(SelectedTakeoverRadius, false, false, true);
            }
        }
        else if (selectedItem == ShowStatus)
        {
            Player.DisplayPlayerNotification();
        }
        Main.Visible = false;
    }
    private void OnListChange(UIMenu sender, UIMenuListItem list, int index)
    {
        if (list == TakeoverRandomPed)
        {
            SelectedTakeoverRadius = Distances[index].Distance;
            //EntryPoint.WriteToConsole($"Current Main Takeover Distance {SelectedTakeoverRadius}");
        }
    }
}