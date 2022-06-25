﻿using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

public class GangDen : InteractableLocation
{
    private LocationCamera StoreCamera;
    private UIMenuItem dropoffCash;
    private UIMenuItem dropoffItem;

    private IActivityPerformable Player;
    private IModItems ModItems;
    private IEntityProvideable World;
    private ISettingsProvideable Settings;
    private IWeapons Weapons;
    private ITimeControllable Time;
    private UIMenuItem completeTask;


    private Transaction Transaction;
    private UIMenuNumericScrollerItem<int> RestMenuItem;
    private bool KeepInteractionGoing;
    private UIMenuItem LayLowMenuItem;


    public GangDen() : base()
    {

    }
    public override bool ShowsOnDirectory { get; set; } = false;
    public override string TypeName { get; set; } = "Gang Den";
    public override int MapIcon { get; set; } = (int)BlipSprite.Shrink;
    public override Color MapIconColor { get; set; } = Color.White;
    public override float MapIconScale { get; set; } = 1.0f;
    public override string ButtonPromptText { get; set; }
    public string GangID { get; set; }

    //[XmlIgnore]
    //public ShopMenu Menu { get; set; }
    //public string MenuID { get; set; }
    [XmlIgnore]
    public int ExpectedMoney { get; set; }
    [XmlIgnore]
    public ModItem ExpectedItem { get; set; }
    [XmlIgnore]
    public int ExpectedItemAmount { get; set; }
    [XmlIgnore]
    public Gang AssociatedGang { get; set; }

    public GangDen(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description, string menuID, string _gangID) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {
        GangID = _gangID;
        MenuID = menuID;
        ButtonPromptText = $"Enter {Name}";
        CanInteractWhenWanted = true;
    }
    public override void OnInteract(ILocationInteractable player, IModItems modItems, IEntityProvideable world, ISettingsProvideable settings, IWeapons weapons, ITimeControllable time)
    {
        Player = player;
        ModItems = modItems;
        World = world;
        Settings = settings;
        Weapons = weapons;
        Time = time;

        if (CanInteract)
        {
            Player.IsInteractingWithLocation = true;
            CanInteract = false;
            GameFiber.StartNew(delegate
            {
                StoreCamera = new LocationCamera(this, Player);
                StoreCamera.SayGreeting = false;
                StoreCamera.Setup();
                CreateInteractionMenu();
                if(Player.IsWanted)
                {
                    LayLowMenuItem = new UIMenuItem("Lay Low", "Wait out the cops.");
                    InteractionMenu.AddItem(LayLowMenuItem);
                    InteractionMenu.Visible = true;
                    InteractionMenu.OnItemSelect += InteractionMenu_OnItemSelect;
                    while (IsAnyMenuVisible || Time.IsFastForwarding || KeepInteractionGoing || Player.IsWanted)
                    {
                        MenuPool.ProcessMenus();
                        GameFiber.Yield();
                    }
                    InteractionMenu.OnItemSelect -= InteractionMenu_OnItemSelect;
                }
                if (Player.IsNotWanted)
                {
                    RemoveLayLow();
                    KeepInteractionGoing = false;
                    Player.IsTransacting = true;
                    Transaction = new Transaction(MenuPool, InteractionMenu, Menu, this);
                    Transaction.CreateTransactionMenu(Player, modItems, world, settings, weapons, time);
                    PlayerTask pt = Player.PlayerTasks.GetTask(AssociatedGang.ContactName);
                    if (ExpectedMoney > 0 && pt.IsReadyForPayment)
                    {
                        dropoffCash = new UIMenuItem("Drop Cash", "Drop off the expected amount of cash.") { RightLabel = $"${ExpectedMoney}" };
                        InteractionMenu.AddItem(dropoffCash);
                    }
                    else if (ExpectedItem != null && pt.IsReadyForPayment)
                    {
                        dropoffItem = new UIMenuItem($"Drop off item", $"Drop off {ExpectedItem.Name} - {ExpectedItemAmount} {ExpectedItem.MeasurementName}(s).") { RightLabel = $"{ExpectedItem.Name} - {ExpectedItemAmount} {ExpectedItem.MeasurementName}(s)" };
                        InteractionMenu.AddItem(dropoffItem);
                    }
                    else if (pt != null && pt.IsActive && pt.IsReadyForPayment)
                    {
                        completeTask = new UIMenuItem($"Collect Money", $"Inform the higher ups that you have completed the assigment and collect your payment.") { RightLabel = $"${pt.PaymentAmountOnCompletion}" };
                        InteractionMenu.AddItem(completeTask);
                    }
                    RestMenuItem = new UIMenuNumericScrollerItem<int>("Relax", $"Relax at the {AssociatedGang?.DenName}. Recover ~g~health~s~ and increase ~s~rep~s~ a small amount. Select up to 12 hours.", 1, 12, 1) { Formatter = v => v.ToString() + " hours" };
                    InteractionMenu.Visible = true;
                    InteractionMenu.OnItemSelect += InteractionMenu_OnItemSelect;
                    while (IsAnyMenuVisible || Time.IsFastForwarding || KeepInteractionGoing)
                    {
                        MenuPool.ProcessMenus();
                        Transaction?.PurchaseMenu?.Update();
                        Transaction?.SellMenu?.Update();
                        GameFiber.Yield();
                    }
                    EntryPoint.WriteToConsole($"PLAYER EVENT: Gang Den LOOP CLOSING IsAnyMenuVisible {IsAnyMenuVisible} Time.IsFastForwarding {Time.IsFastForwarding}", 3);
                    //Transaction.ProcessTransactionMenu();
                    Transaction.DisposeTransactionMenu();
                    Player.IsTransacting = false;
                }
                DisposeInteractionMenu();
                StoreCamera.Dispose();
                Player.IsInteractingWithLocation = false;
                CanInteract = true;
            }, "GangDenInteract");       
        }
    }
    private void InteractionMenu_OnItemSelect(RAGENativeUI.UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (selectedItem.Text == "Buy")
        {
            Transaction?.SellMenu?.Dispose();
            Transaction?.PurchaseMenu?.Show();
        }
        else if (selectedItem.Text == "Sell")
        {
            Transaction?.PurchaseMenu?.Dispose();
            Transaction?.SellMenu?.Show();
        }
        else if (selectedItem == dropoffCash)
        {
            if(Player.Money >= ExpectedMoney)
            {
                Player.GiveMoney(-1*ExpectedMoney);
                Game.DisplayNotification(AssociatedGang.ContactIcon, AssociatedGang.ContactIcon, AssociatedGang.ContactName, "~g~Reply", "Thanks for the cash. Here's your cut.");
                ExpectedMoney = 0;
                Player.PlayerTasks.CompleteTask(AssociatedGang.ContactName, true);
                InteractionMenu.Visible = false;
            }
            else
            {
                Game.DisplayNotification(AssociatedGang.ContactIcon,AssociatedGang.ContactIcon,AssociatedGang.ContactName,"~r~Reply","Come back when you actually have the cash.");
            }
        }
        else if (selectedItem == dropoffItem)
        {
            if(Player.Inventory.Amount(ExpectedItem.Name) >= ExpectedItemAmount)
            {
                Player.Inventory.Remove(ExpectedItem, ExpectedItemAmount);
                Game.DisplayNotification(AssociatedGang.ContactIcon, AssociatedGang.ContactIcon, AssociatedGang.ContactName, "~g~Reply", $"Thanks for bringing us {ExpectedItemAmount} {ExpectedItem.MeasurementName}(s) of {ExpectedItem.Name}. Have something for your time.");
                ExpectedItem = null;
                ExpectedItemAmount = 0;
                Player.PlayerTasks.CompleteTask(AssociatedGang.ContactName, true);
                InteractionMenu.Visible = false;
            }
            else
            {
                Game.DisplayNotification(AssociatedGang.ContactIcon, AssociatedGang.ContactIcon, AssociatedGang.ContactName, "~r~Reply", $"Come back when you actually have {ExpectedItemAmount} {ExpectedItem.MeasurementName}(s) of {ExpectedItem.Name}.");
            }

        }
        else if (selectedItem == completeTask)
        {
            Game.DisplayNotification(AssociatedGang.ContactIcon, AssociatedGang.ContactIcon, AssociatedGang.ContactName, "~g~Reply", "Thanks for taking care of that thing. Here's your share.");
            ExpectedMoney = 0;
            Player.PlayerTasks.CompleteTask(AssociatedGang.ContactName, true);
            InteractionMenu.Visible = false;
        }
        else if (selectedItem == RestMenuItem)
        {
            Rest(RestMenuItem.Value);
        }
        else if (selectedItem == LayLowMenuItem)
        {
            LayLow();
        }
    }

    public void Reset()
    {
        IsEnabled = false;
        ExpectedMoney = 0;
        ExpectedItem = null;
        ExpectedItemAmount = 0;
    }
    private void Rest(int Hours)
    {
        Time.FastForward(Time.CurrentDateTime.AddHours(Hours));//  new DateTime(Time.CurrentYear, Time.CurrentMonth, Time.CurrentDay, 11, 0, 0));
        InteractionMenu.Visible = false;
        KeepInteractionGoing = true;
        DateTime TimeLastAddedItems = Time.CurrentDateTime;
        GameFiber FastForwardWatcher = GameFiber.StartNew(delegate
        {
            while (Time.IsFastForwarding)
            {
                if(DateTime.Compare(Time.CurrentDateTime, TimeLastAddedItems) >= 0)
                {
                    if (Game.LocalPlayer.Character.Health < Game.LocalPlayer.Character.MaxHealth - 1)
                    {
                        Game.LocalPlayer.Character.Health++;
                    }
                    Player.GangRelationships.ChangeReputation(AssociatedGang, 2, false);
                    TimeLastAddedItems = TimeLastAddedItems.AddMinutes(30);
                }
                GameFiber.Yield();
            }
            InteractionMenu.Visible = true;
            KeepInteractionGoing = false;
        }, "RestWatcher");
        EntryPoint.WriteToConsole($"PLAYER EVENT: START REST ACTIVITY AT GANG DEN", 3);
    }
    private void LayLow()
    {
        int TimeToWait = RandomItems.GetRandomNumberInt(8, 12);
        Time.FastForward(Time.CurrentDateTime.AddHours(TimeToWait));//  new DateTime(Time.CurrentYear, Time.CurrentMonth, Time.CurrentDay, 11, 0, 0));
        InteractionMenu.Visible = false;
        KeepInteractionGoing = true;
        GameFiber FastForwardWatcher = GameFiber.StartNew(delegate
        {
            while (Time.IsFastForwarding)
            {
                GameFiber.Yield();
            }
            Player.SetWantedLevel(0, "Gang Lay Low", true);
            RemoveLayLow();
            //InteractionMenu.Visible = true;
            //KeepInteractionGoing = false;
        }, "LayLowWatcher");
        EntryPoint.WriteToConsole($"PLAYER EVENT: START LAY LOW ACTIVITY AT GANG DEN", 3);
    }
    private void RemoveLayLow()
    {
        if (InteractionMenu.MenuItems.IndexOf(LayLowMenuItem) >= 0)
        {
            InteractionMenu.RemoveItemAt(InteractionMenu.MenuItems.IndexOf(LayLowMenuItem));
            InteractionMenu.RefreshIndex();
        }
    }
}
