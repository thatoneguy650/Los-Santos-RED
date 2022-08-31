using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

public class GangDen : InteractableLocation, ILocationGangAssignable
{
    private LocationCamera StoreCamera;
    private UIMenuItem dropoffCash;
    private UIMenuItem dropoffItem;
    private ILocationInteractable Player;
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
    private UIMenuItem dropoffKick;

    public GangDen() : base()
    {

    }
    public override bool ShowsOnDirectory { get; set; } = false;
    public override string TypeName { get; set; } = "Gang Den";
    public override int MapIcon { get; set; } = (int)BlipSprite.Snitch;
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
    [XmlIgnore]
    public bool IsDispatchFilled { get; set; } = false;
    public List<ConditionalLocation> PossiblePedSpawns { get; set; }
    public List<ConditionalLocation> PossibleVehicleSpawns { get; set; }
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
            bool isPlayerMember = player.RelationshipManager.GangRelationships.GetReputation(AssociatedGang)?.IsMember == true;
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



                CreateInteractionMenu();
                if (Player.IsNotWanted)
                {
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


                    if(isPlayerMember && player.RelationshipManager.GangRelationships.CurrentGangKickUp != null)
                    {
                        dropoffKick = new UIMenuItem("Pay Dues", "Drop of your member dues.") { RightLabel = $"${player.RelationshipManager.GangRelationships.CurrentGangKickUp.DueAmount}" };
                        InteractionMenu.AddItem(dropoffKick);
                    }



                    InteractionMenu.Visible = true;
                    InteractionMenu.OnItemSelect += InteractionMenu_OnItemSelect;
                    while (IsAnyMenuVisible || Time.IsFastForwarding || KeepInteractionGoing)
                    {
                        MenuPool.ProcessMenus();
                        Transaction?.PurchaseMenu?.Update();
                        Transaction?.SellMenu?.Update();
                        GameFiber.Yield();
                    }
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
            if(Player.BankAccounts.Money >= ExpectedMoney)
            {
                Player.BankAccounts.GiveMoney(-1*ExpectedMoney);
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
        else if (selectedItem == dropoffKick)
        {
            if(Player.RelationshipManager.GangRelationships.CurrentGangKickUp != null)
            {
                if(Player.BankAccounts.Money >= Player.RelationshipManager.GangRelationships.CurrentGangKickUp.DueAmount)
                {
                    Player.BankAccounts.GiveMoney(-1 * Player.RelationshipManager.GangRelationships.CurrentGangKickUp.DueAmount);
                    Player.RelationshipManager.GangRelationships.CurrentGangKickUp.PayDue();
                    InteractionMenu.Visible = false;
                }
                else
                {
                    Game.DisplayNotification(AssociatedGang.ContactIcon, AssociatedGang.ContactIcon, AssociatedGang.ContactName, "~r~Reply", "Come back when you actually have the cash.");
                }
            }
            
        }
    }
    public void Reset()
    {
        IsEnabled = false;
        ExpectedMoney = 0;
        ExpectedItem = null;
        ExpectedItemAmount = 0;
    }
    public void StoreData(IGangs gangs,IShopMenus shopMenus)
    {
        Menu = shopMenus.GetMenu(MenuID);
        AssociatedGang = gangs.GetGang(GangID);
        ButtonPromptText = $"Enter {AssociatedGang?.ShortName} {AssociatedGang?.DenName}";
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
                Player.IsResting = true;
                Player.IsSleeping = true;
                if (DateTime.Compare(Time.CurrentDateTime, TimeLastAddedItems) >= 0)
                {
                    if (Game.LocalPlayer.Character.Health < Game.LocalPlayer.Character.MaxHealth - 1)
                    {
                        Game.LocalPlayer.Character.Health++;
                    }
                    Player.RelationshipManager.GangRelationships.ChangeReputation(AssociatedGang, 2, false);
                    TimeLastAddedItems = TimeLastAddedItems.AddMinutes(30);
                }
                GameFiber.Yield();
            }
            Player.IsResting = false;
            Player.IsSleeping = false;
            InteractionMenu.Visible = true;
            KeepInteractionGoing = false;
        }, "RestWatcher");
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
            LayLowMenuItem.Enabled = false;
            KeepInteractionGoing = false;
            //RemoveLayLow();
        }, "LayLowWatcher");
    }
    //private void RemoveLayLow()
    //{
    //    if (InteractionMenu.MenuItems.IndexOf(LayLowMenuItem) >= 0)
    //    {
    //        InteractionMenu.RemoveItemAt(InteractionMenu.MenuItems.IndexOf(LayLowMenuItem));
    //        InteractionMenu.RefreshIndex();
    //    }
    //}
}

