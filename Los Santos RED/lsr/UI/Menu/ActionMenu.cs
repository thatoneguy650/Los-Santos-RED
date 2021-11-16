using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System.Collections.Generic;

public class ActionMenu : Menu
{
    private UIMenu Actions;
    private UIMenuListScrollerItem<LSR.Vehicles.LicensePlate> ChangePlate;
    private UIMenuItem Drink;
    private IActionable Player;
    private UIMenuItem RemovePlate;
    private UIMenuItem Smoke;
    private UIMenuItem SmokePot;
    private UIMenuItem StopConsuming;
    private UIMenuItem Suicide;
    public ActionMenu(MenuPool menuPool, UIMenu parentMenu, IActionable player)
    {
        Player = player;
        Actions = menuPool.AddSubMenu(parentMenu, "Actions");
        Actions.OnItemSelect += OnActionItemSelect;
        CreateActionsMenu();
    }
    public int SelectedPlateIndex { get; set; }

    public override void Hide()
    {
        Actions.Visible = false;
    }

    public override void Show()
    {
        Update();
        Actions.Visible = true;
    }
    public override void Toggle()
    {
        
        if (!Actions.Visible)
        {
            Actions.Visible = true;
        }
        else
        {
            Actions.Visible = false;
        }
    }
    public void Update()
    {
        CreateActionsMenu();
        if (Player.CanPerformActivities)
        {
            Suicide.Enabled = true;
            ChangePlate.Enabled = true;
            RemovePlate.Enabled = true;
            Drink.Enabled = true;
            Smoke.Enabled = true;
            SmokePot.Enabled = true;
        }
        else
        {
            Suicide.Enabled = false;
            ChangePlate.Enabled = false;
            RemovePlate.Enabled = false;
            Drink.Enabled = false;
            Smoke.Enabled = false;
            SmokePot.Enabled = false;
        }
        if (Player.IsPerformingActivity)
        {
            StopConsuming.Enabled = true;
        }
        else
        {
            StopConsuming.Enabled = false;
        }


        EntryPoint.WriteToConsole("ACTION MENU!-------------------------------", 3);
        foreach (LicensePlate lp in ChangePlate.Items)
        {

            EntryPoint.WriteToConsole($" PlateNumber: {lp.PlateNumber} Wanted: {lp.IsWanted} Type: {lp.PlateType}", 3);

        }
        EntryPoint.WriteToConsole("ACTION MENU!-------------------------------", 3);
    }
    private void CreateActionsMenu()
    {
        Actions.Clear();
        Suicide = new UIMenuItem("Suicide", "Commit Suicide");
        ChangePlate = new UIMenuListScrollerItem<LSR.Vehicles.LicensePlate>("Change Plate", "Change your license plate if you have spares.",Player.SpareLicensePlates);
        RemovePlate = new UIMenuItem("Remove Plate", "Remove the license plate.");
        Drink = new UIMenuItem("Drink", "Start Drinking");
        Smoke = new UIMenuItem("Smoke", "Start Smoking");
        SmokePot = new UIMenuItem("Smoke Pot", "Start Smoking Pot");
        StopConsuming = new UIMenuItem("Stop", "Stop Consuming Activity");

        Actions.AddItem(Suicide);
        Actions.AddItem(ChangePlate);
        Actions.AddItem(RemovePlate);
        Actions.AddItem(Drink);
        Actions.AddItem(Smoke);
        Actions.AddItem(SmokePot);
        Actions.AddItem(StopConsuming);

        
       // Actions.OnListChange += OnListChange;
    }
    private void OnActionItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (selectedItem == Suicide)
        {
            Player.CommitSuicide();
        }
        else if (selectedItem == ChangePlate)
        {
            Player.ChangePlate(ChangePlate.SelectedItem);
        }
        else if (selectedItem == RemovePlate)
        {
            Player.RemovePlate();
        }
        else if (selectedItem == Drink)
        {
            Player.StartDrinkingActivity();
        }
        else if (selectedItem == Smoke)
        {
            Player.StartSmoking();
        }
        else if (selectedItem == SmokePot)
        {
            Player.StartSmokingPot();
        }
        else if (selectedItem == StopConsuming)
        {
            Player.StopDynamicActivity();
        }
        Actions.Visible = false;
        ChangePlate.Items = Player.SpareLicensePlates;
    }
    //private void OnListChange(UIMenu sender, UIMenuListItem list, int index)
    //{
    //    if (list == ChangePlate)
    //    {
    //        SelectedPlateIndex = index;
    //    }
    //}
}