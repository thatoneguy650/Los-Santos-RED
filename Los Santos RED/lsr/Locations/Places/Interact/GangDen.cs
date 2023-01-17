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
    private UIMenuItem dropoffCash;
    private UIMenuItem dropoffItem;
    private UIMenuItem completeTask;

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


    public Vector3 ItemPreviewPosition { get; set; } = Vector3.Zero;
    public float ItemPreviewHeading { get; set; } = 0f;
    public List<SpawnPlace> ItemDeliveryLocations = new List<SpawnPlace>();


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
    public override bool CanCurrentlyInteract(ILocationInteractable player)
    {
        ButtonPromptText = $"Enter {AssociatedGang?.ShortName} {AssociatedGang?.DenName}";
        return true;
    }
    public GangDen(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description, string menuID, string _gangID) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {
        GangID = _gangID;
        MenuID = menuID;
    }
    public override void OnInteract(ILocationInteractable player, IModItems modItems, IEntityProvideable world, ISettingsProvideable settings, IWeapons weapons, ITimeControllable time, IPlacesOfInterest placesOfInterest)
    {
        Player = player;
        ModItems = modItems;
        World = world;
        Settings = settings;
        Weapons = weapons;
        Time = time;

        if (IsLocationClosed())
        {
            return;
        }

        if (CanInteract)
        {
            bool isPlayerMember = player.RelationshipManager.GangRelationships.GetReputation(AssociatedGang)?.IsMember == true;
            Player.ActivityManager.IsInteractingWithLocation = true;
            CanInteract = false;
            GameFiber.StartNew(delegate
            {
                try
                {
                    StoreCamera = new LocationCamera(this, Player);
                    StoreCamera.SayGreeting = false;

                    StoreCamera.ItemPreviewHeading = ItemPreviewHeading;
                    StoreCamera.ItemPreviewPosition = ItemPreviewPosition;

                    StoreCamera.Setup();

                    KeepInteractionGoing = false;
                    CreateInteractionMenu();
                    if (Player.IsWanted)
                    {
                        LayLowMenuItem = new UIMenuItem("Lay Low", "Wait out the cops.");
                        InteractionMenu.AddItem(LayLowMenuItem);
                        InteractionMenu.Visible = true;
                        InteractionMenu.OnItemSelect += InteractionMenu_OnItemSelect;
                        while (IsAnyMenuVisible || Time.IsFastForwarding || KeepInteractionGoing)// || Player.IsWanted)
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


                        if (Player.RelationshipManager.GangRelationships.CurrentGang != null && Player.RelationshipManager.GangRelationships.CurrentGang.ID == GangID)
                        {
                            Transaction.IsFreeVehicles = true;
                            Transaction.IsFreeWeapons = true;
                        }

                        Transaction.CreateTransactionMenu(Player, modItems, world, settings, weapons, time);

                        Transaction.ItemDeliveryLocations = ItemDeliveryLocations;
                        Transaction.ItemPreviewPosition = ItemPreviewPosition;
                        Transaction.ItemPreviewHeading = ItemPreviewHeading;

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


                        if (isPlayerMember && player.RelationshipManager.GangRelationships.CurrentGangKickUp != null)
                        {
                            dropoffKick = new UIMenuItem("Pay Dues", "Drop of your member dues.") { RightLabel = $"${player.RelationshipManager.GangRelationships.CurrentGangKickUp.DueAmount}" };
                            InteractionMenu.AddItem(dropoffKick);
                        }



                        InteractionMenu.Visible = true;
                        InteractionMenu.OnItemSelect += InteractionMenu_OnItemSelect;
                        while (IsAnyMenuVisible || Time.IsFastForwarding || KeepInteractionGoing)
                        {
                            MenuPool.ProcessMenus();

                            Transaction?.Update();

                            //Transaction?.PurchaseMenu?.Update();
                            //Transaction?.SellMenu?.Update();
                            GameFiber.Yield();
                        }
                        Transaction.DisposeTransactionMenu();
                        Player.IsTransacting = false;
                    }
                    DisposeInteractionMenu();
                    StoreCamera.Dispose();
                    Player.ActivityManager.IsInteractingWithLocation = false;
                    CanInteract = true;
                }
                catch (Exception ex)
                {
                    EntryPoint.WriteToConsole("Location Interaction" + ex.Message + " " + ex.StackTrace, 0);
                    EntryPoint.ModController.CrashUnload();
                }
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
                ExpectedMoney = 0;
                Player.PlayerTasks.CompleteTask(AssociatedGang.ContactName, true);
                InteractionMenu.Visible = false;
                PlaySuccessSound();
                DisplayMessage("~g~Reply", "Thanks for the cash. Here's your cut.");
            }
            else
            {
                PlayErrorSound();
                DisplayMessage("~r~Reply", "Come back when you actually have the cash.");
            }
        }
        else if (selectedItem == dropoffItem)
        {
            if (Player.Inventory.Get(ExpectedItem)?.Amount >= ExpectedItemAmount)
            {
                Player.Inventory.Remove(ExpectedItem, ExpectedItemAmount);
                PlaySuccessSound();
                DisplayMessage("~g~Reply", $"Thanks for bringing us {ExpectedItemAmount} {ExpectedItem.MeasurementName}(s) of {ExpectedItem.Name}. Have something for your time.");
                ExpectedItem = null;
                ExpectedItemAmount = 0;
                Player.PlayerTasks.CompleteTask(AssociatedGang.ContactName, true);
                InteractionMenu.Visible = false;
            }
            else
            {
                PlayErrorSound();
                DisplayMessage("~r~Reply", $"Come back when you actually have {ExpectedItemAmount} {ExpectedItem.MeasurementName}(s) of {ExpectedItem.Name}.");
            }

        }
        else if (selectedItem == completeTask)
        {
            PlaySuccessSound();
            DisplayMessage("~g~Reply", "Thanks for taking care of that thing. Here's your share.");
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
                    if (Player.RelationshipManager.GangRelationships.CurrentGangKickUp.CanPay)
                    {
                        Player.BankAccounts.GiveMoney(-1 * Player.RelationshipManager.GangRelationships.CurrentGangKickUp.DueAmount);
                        Player.RelationshipManager.GangRelationships.CurrentGangKickUp.PayDue();
                        InteractionMenu.Visible = false;
                        PlaySuccessSound();
                        DisplayMessage("~g~Reply", "Thanks for the kick.");
                    }
                    else
                    {
                        PlayErrorSound();
                        DisplayMessage("~r~Reply", "Not time yet, come back closer to the due date.");
                    }
                }
                else
                {
                    PlayErrorSound();
                    DisplayMessage("~r~Reply", "Come back when you actually have the cash.");
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
        Menu = shopMenus.GetSpecificMenu(MenuID);
        AssociatedGang = gangs.GetGang(GangID);
        ButtonPromptText = $"Enter {AssociatedGang?.ShortName} {AssociatedGang?.DenName}";
    }
    private void Rest(int Hours)
    {
        Time.FastForward(Time.CurrentDateTime.AddHours(Hours));//  new DateTime(Time.CurrentYear, Time.CurrentMonth, Time.CurrentDay, 11, 0, 0));
        InteractionMenu.Visible = false;
        KeepInteractionGoing = true;

        Player.ButtonPrompts.AddPrompt("GangDenRest", "Cancel Rest", "GangDenRest", Settings.SettingsManager.KeySettings.InteractCancel, 99);

        DateTime TimeLastAddedItems = Time.CurrentDateTime;
        GameFiber FastForwardWatcher = GameFiber.StartNew(delegate
        {
            while (Time.IsFastForwarding)
            {
                Player.IsResting = true;
                Player.IsSleeping = true;
                if (DateTime.Compare(Time.CurrentDateTime, TimeLastAddedItems) >= 0)
                {
                    if (!Settings.SettingsManager.NeedsSettings.ApplyNeeds)
                    {
                        Player.HealthManager.ChangeHealth(1);
                    }
                    Player.RelationshipManager.GangRelationships.ChangeReputation(AssociatedGang, 2, false);
                    TimeLastAddedItems = TimeLastAddedItems.AddMinutes(30);
                }
                if (Player.ButtonPrompts.IsPressed("GangDenRest"))
                {
                    Time.StopFastForwarding();
                }
                GameFiber.Yield();
            }
            Player.ButtonPrompts.RemovePrompts("GangDenRest");
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
    public override void DisplayMessage(string header, string message)
    {
        Game.RemoveNotification(NotificationHandle);
        NotificationHandle = Game.DisplayNotification(AssociatedGang.ContactIcon, AssociatedGang.ContactIcon, AssociatedGang.ContactName, header, message);
    }
}

