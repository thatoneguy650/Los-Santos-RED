using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System.Collections.Generic;

public class ActionMenu : Menu
{
    private UIMenu Actions;
    private UIMenuListItem ChangePlate;
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
        CreateActionsMenu();
    }
    public int SelectedPlateIndex { get; set; }

    public override void Hide()
    {
        Actions.Visible = false;
    }

    public override void Show()
    {
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
    }
    private void CreateActionsMenu()
    {
        Suicide = new UIMenuItem("Suicide", "Commit Suicide");
        ChangePlate = new UIMenuListItem("Change Plate", "Change your license plate if you have spares.",Player.SpareLicensePlates);
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

        Actions.OnItemSelect += OnActionItemSelect;
        Actions.OnListChange += OnListChange;
    }
    private void OnActionItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (selectedItem == Suicide)
        {
            Player.CommitSuicide();
        }
        else if (selectedItem == ChangePlate)
        {
            Player.ChangePlate(SelectedPlateIndex);
        }
        else if (selectedItem == RemovePlate)
        {
            Player.RemovePlate();
        }
        else if (selectedItem == Drink)
        {
            Player.DrinkBeer();
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
    }
    private void OnListChange(UIMenu sender, UIMenuListItem list, int index)
    {
        if (list == ChangePlate)
        {
            SelectedPlateIndex = index;
        }
    }
}