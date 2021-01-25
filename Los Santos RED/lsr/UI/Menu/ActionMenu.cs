using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System.Collections.Generic;

public class ActionMenu
{
    private UIMenu Actions;
    private UIMenuItem ChangePlate;
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
    public void Update()
    {
        if (Player.CanPerformActivities)
        {
            ChangePlate.Enabled = true;
            RemovePlate.Enabled = true;
            Drink.Enabled = true;
            Smoke.Enabled = true;
            SmokePot.Enabled = true;


        }
        else
        {
            ChangePlate.Enabled = false;
            RemovePlate.Enabled = false;
            Drink.Enabled = false;
            Smoke.Enabled = false;
            SmokePot.Enabled = false;
        }
        if (Player.IsConsuming)
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
        ChangePlate = new UIMenuItem("Change Plate", "Change your license plate if you have spares.");
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
            Player.ChangePlate();
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
            Player.StopConsumingActivity();
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