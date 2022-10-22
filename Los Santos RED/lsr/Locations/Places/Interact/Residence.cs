using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

public class Residence : InteractableLocation
{
    private LocationCamera StoreCamera;

    private ILocationInteractable Player;
    private IModItems ModItems;
    private IEntityProvideable World;
    private ISettingsProvideable Settings;
    private IWeapons Weapons;
    private ITimeControllable Time;
    private UIMenu OfferSubMenu;
    private UIMenuNumericScrollerItem<int> RestMenuItem;
    private UIMenuItem InventoryMenuItem;
    private UIMenuItem PurchaseResidenceMenuItem;
    private UIMenuItem RentResidenceMenuItem;
    private UIMenuItem RentDisplayItem;
    private bool KeepInteractionGoing;
    private InventoryMenu InventoryMenu;
    private IActivityPerformable ActivityPerformable;

    private string IsRentedDescription => $"Rental Days: {RentalDays}~n~Remaining Days: ~o~{Math.Round((DateRentalPaymentDue - Time.CurrentDateTime).TotalDays, 0)}~s~~n~Rental Fee: ~r~{RentalFee:C0}~s~";
    private string IsRentedRightLabel => Time == null ? $"Due Date: {DateRentalPaymentDue}" : "Remaining Days: " + Math.Round((DateRentalPaymentDue - Time.CurrentDateTime).TotalDays, 0).ToString();
    private string CanRentRightLabel => $"{RentalFee:C0} for {RentalDays} days";
    private string CanPurchaseRightLabel => $"{PurchasePrice:C0}";
    public Residence() : base()
    {

    }

    [XmlIgnore]
    public bool IsOwned { get; set; } = false;
    [XmlIgnore]
    public bool IsRented { get; set; } = false;
    [XmlIgnore]
    public DateTime DateRentalPaymentDue { get; set; }
    [XmlIgnore]
    public DateTime DateRentalPaymentPaid { get; set; }

    public bool CanRent => !IsOwned && !IsRented && RentalFee > 0;
    public bool CanBuy => !IsOwned && PurchasePrice > 0;
    public bool IsOwnedOrRented => IsOwned || IsRented;

    public int RentalDays { get; set; }
    public int RentalFee { get; set; }
    public int PurchasePrice { get; set; }
    public override string TypeName => IsOwnedOrRented ? "Residence" : "For Sale/Rental";

    public override int MapIcon { get; set; } = (int)BlipSprite.PropertyForSale;
    public override Color MapIconColor { get; set; } = Color.White;
    public override float MapIconScale { get; set; } = 1.0f;
    public override string ButtonPromptText { get; set; }

    public override int SortOrder => IsOwnedOrRented ? 1 : 999;


    public Residence(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {
        ButtonPromptText = GetButtonPromptText();
        OpenTime = 0;
        CloseTime = 24;
    }
    public override void OnInteract(ILocationInteractable player, IModItems modItems, IEntityProvideable world, ISettingsProvideable settings, IWeapons weapons, ITimeControllable time)
    {
        Player = player;
        ModItems = modItems;
        World = world;
        Settings = settings;
        Weapons = weapons;
        Time = time;
        //ActivityPerformable = activityPerformable;



        if (CanInteract)
        {
            Player.ActivityManager.IsInteractingWithLocation = true;
            CanInteract = false;

            GameFiber.StartNew(delegate
            {
                StoreCamera = new LocationCamera(this, Player);
                StoreCamera.SayGreeting = false;
                StoreCamera.Setup();
                CreateInteractionMenu();
                InteractionMenu.Visible = true;
                InteractionMenu.OnItemSelect += InteractionMenu_OnItemSelect;

                GenerateResidenceMenu();





                //ProcessInteractionMenu();

                while (IsAnyMenuVisible || Time.IsFastForwarding || KeepInteractionGoing)
                {
                    MenuPool.ProcessMenus();
                    GameFiber.Yield();
                }
                EntryPoint.WriteToConsole($"PLAYER EVENT: RESIDENCE LOOP CLOSING IsAnyMenuVisible {IsAnyMenuVisible} Time.IsFastForwarding {Time.IsFastForwarding}", 3);


                DisposeInteractionMenu();
                StoreCamera.Dispose();


                Player.ActivityManager.IsInteractingWithLocation = false;
                CanInteract = true;
            }, "ResidenceInteract");
        }
    }
    public void RefreshUI()
    {
        UpdateStoredData();
    }
    public void Reset()
    {
        IsOwned = false;
        IsRented = false;
        UpdateStoredData();
    }
    public void ReRent()
    {
        if (Player.BankAccounts.Money >= RentalFee)
        {
            Player.BankAccounts.GiveMoney(-1 * RentalFee);
            DateRentalPaymentPaid = Time.CurrentDateTime;
            DateRentalPaymentDue = DateRentalPaymentPaid.AddDays(RentalDays);
            UpdateStoredData();
            Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", Name, "~g~Rent Paid", $"You have been charged the rental fee of {RentalFee:C0} for {Name}.~n~Next payment date: {DateRentalPaymentDue:d}");

        }
        else
        {
            Reset();
            if (MenuPool != null && MenuPool.IsAnyMenuOpen())
            {
                MenuPool.CloseAllMenus();
            }
            Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", Name, "~r~Evicted", $"You have been evicted from {Name} for non-payment.");
        }
    }
    private void InteractionMenu_OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if(selectedItem == RestMenuItem)
        {
            Rest(RestMenuItem.Value);
        }
        else if (selectedItem == InventoryMenuItem)
        {
            //Rest(RestMenuItem.Value);
        }
    }

    private void GenerateResidenceMenu()
    {
        if(!IsOwned || !IsRented)
        {
            if ((!IsOwned && CanBuy) || (!IsRented && CanRent))
            {
                OfferSubMenu = MenuPool.AddSubMenu(InteractionMenu, "Make an Offer");
                string offerDescription = "";
                if (!IsOwned && CanBuy)
                {
                    offerDescription += "buy ";
                }
                if (!IsRented && CanRent)
                {
                    if (offerDescription != "")
                    {
                        offerDescription += "or ";
                    }
                    offerDescription += "rent ";
                }
                InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1].Description = $"Select to {offerDescription.Trim()}";
                InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1].RightBadge = UIMenuItem.BadgeStyle.Lock;
                if (HasBannerImage)
                {
                    BannerImage = Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\{BannerImagePath}");
                    OfferSubMenu.SetBannerType(BannerImage);
                }
                OfferSubMenu.OnItemSelect += OfferMenu_OnItemSelect;
                OfferSubMenu.OnIndexChange += OfferMenu_OnIndexChange;
                OfferSubMenu.OnMenuOpen += OfferMenu_OnMenuOpen;
                OfferSubMenu.OnMenuClose += OfferMenu_OnMenuClose;


                if (!IsOwned && CanBuy)
                {
                    PurchaseResidenceMenuItem = new UIMenuItem("Purchase", "Select to purchase this residence") { RightLabel = CanPurchaseRightLabel };
                    OfferSubMenu.AddItem(PurchaseResidenceMenuItem);
                }
                if (!IsRented && CanRent)
                {
                    RentResidenceMenuItem = new UIMenuItem("Rent", $"Select to rent this residence for {RentalDays} days") { RightLabel = CanRentRightLabel };
                    OfferSubMenu.AddItem(RentResidenceMenuItem);
                }
            }
        }
        AddInteractionItems();
    }
    private void AddInteractionItems()
    {
        if (IsOwned || IsRented)
        {
            if (IsRented)
            {
                RentDisplayItem = new UIMenuItem("Rental Period", IsRentedDescription) { RightLabel = IsRentedRightLabel };
                InteractionMenu.AddItem(RentDisplayItem);
            }
            RestMenuItem = new UIMenuNumericScrollerItem<int>("Rest", "Rest at your residence to recover health. Select up to 12 hours.", 1, 12, 1) { Formatter = v => v.ToString() + " hours" };
            InteractionMenu.AddItem(RestMenuItem);
            InventoryMenu = new InventoryMenu(MenuPool, InteractionMenu, Player, ModItems, true);
        }
    }
    private void OfferMenu_OnMenuClose(UIMenu sender)
    {

    }
    private void OfferMenu_OnMenuOpen(UIMenu sender)
    {

    }
    private void OfferMenu_OnIndexChange(UIMenu sender, int newIndex)
    {


    }
    private void OfferMenu_OnItemSelect(RAGENativeUI.UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if(selectedItem == RentResidenceMenuItem)
        {
            if (Rent())
            {
                RentResidenceMenuItem.Enabled = false;
                //sender.Visible = false;
            }
        }
        else if (selectedItem == PurchaseResidenceMenuItem)
        {
            if(Purchase())
            {
                PurchaseResidenceMenuItem.Enabled = false;
                RentResidenceMenuItem.Enabled = false;
                //sender.Visible = false;
            }
        }
    }
    private bool Rent()
    {
        if(CanRent && Player.BankAccounts.Money >= RentalFee)
        {
            OnRented();
            return true;
        }
        Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", Name, "~r~Rental Failed", "We are sorry, we are unable to complete this rental. Please make sure you have the funds.");
        return false;
    }
    private bool Purchase()
    {
        if (CanBuy && Player.BankAccounts.Money >= PurchasePrice)
        {
            OnPurchased();
            return true;
        }
        Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", Name, "~r~Purchased Failed", "We are sorry, we are unable to complete this purchase. Please make sure you have the funds.");
        return false;
    }
    private void Rest(int Hours)
    {
        Time.FastForward(Time.CurrentDateTime.AddHours(Hours));//  new DateTime(Time.CurrentYear, Time.CurrentMonth, Time.CurrentDay, 11, 0, 0));
        InteractionMenu.Visible = false;
        KeepInteractionGoing = true;
        Player.IsResting = true;
        Player.IsSleeping = true;
        Player.ButtonPrompts.AddPrompt("ResidenceRest", "Cancel Rest", "ResidenceRest", Settings.SettingsManager.KeySettings.InteractCancel, 99);
        GameFiber FastForwardWatcher = GameFiber.StartNew(delegate
        {
            while (Time.IsFastForwarding)
            {
                if (!Settings.SettingsManager.NeedsSettings.ApplyNeeds)
                {
                    Player.HealthManager.ChangeHealth(1);
                }
                if (Player.ButtonPrompts.IsPressed("ResidenceRest"))
                {
                    Time.StopFastForwarding();
                }
                GameFiber.Yield();
            }
            Player.ButtonPrompts.RemovePrompts("ResidenceRest");
            Player.IsResting = false;
            Player.IsSleeping = false;
            if (RentDisplayItem != null)
            {
                RentDisplayItem.Description = $"Rental Days: {RentalDays}~n~Remaining Days: ~o~{Math.Round((DateRentalPaymentDue - Time.CurrentDateTime).TotalDays, 0)}~s~~n~Rental Fee: ~r~{RentalFee:C0}~s~";
                RentDisplayItem.RightLabel = "Remaing Days: " + Math.Round((DateRentalPaymentDue - Time.CurrentDateTime).TotalDays, 0).ToString();
            }
            InteractionMenu.Visible = true;
            KeepInteractionGoing = false;
        }, "FastForwardWatcher");
        EntryPoint.WriteToConsole($"PLAYER EVENT: START REST ACTIVITY AT RESIDENCE", 3);
    }
    private void OnRented()
    {
        Player.BankAccounts.GiveMoney(-1 * RentalFee);
        DateRentalPaymentPaid = Time.CurrentDateTime;
        IsRented = true;
        DateRentalPaymentDue = DateRentalPaymentPaid.AddDays(RentalDays);



        UpdateStoredData();

        Player.Properties.AddResidence(this);

        AddInteractionItems();
        OfferSubMenu.Close(true);
        

        Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", Name, "~g~Rented", $"Thank you for renting {Name}");
    }
    private void OnPurchased()
    {
        Player.BankAccounts.GiveMoney(-1 * PurchasePrice);
        IsOwned = true;
        


        UpdateStoredData();

        Player.Properties.AddResidence(this);



        if (!IsRented)
        {
            AddInteractionItems();
            OfferSubMenu.Close(true);
        }

        Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", Name, "~g~Purchased", $"Thank you for purchasing {Name}");
    }
    private void UpdateStoredData()
    {
        ButtonPromptText = GetButtonPromptText();
        if (IsOwned)
        {
            MapIcon = (int)BlipSprite.Garage;
            MapIconColor = Color.Green;

            if (Blip.Exists())
            {
                Blip.Color = Color.Green;
                Blip.Sprite = BlipSprite.Garage;
            }
        }
        else if(IsRented)
        {
            MapIcon = (int)BlipSprite.Garage;
            MapIconColor = Color.Yellow;

            if (Blip.Exists())
            {
                Blip.Color = Color.Yellow;
                Blip.Sprite = BlipSprite.Garage;
            }
        }
        else
        {
            MapIcon = (int)BlipSprite.PropertyForSale;
            MapIconColor = Color.White;
            if (Blip.Exists())
            {
                Blip.Color = Color.White;
                Blip.Sprite = BlipSprite.PropertyForSale;
            }
        }
    }
    private string GetButtonPromptText()
    {
        if (IsOwnedOrRented)
        {
            return $"Enter {Name}";
        }
        else
        {
            return $"Inquire About {Name}";
        }
    }

    public override List<Tuple<string, string>> DirectoryInfo(int currentHour, float distanceTo)
    {
        List<Tuple<string, string>> BaseList = base.DirectoryInfo(currentHour, distanceTo).ToList();
        if (IsOwnedOrRented)
        {
            if (IsRented)
            {
                BaseList.Add(Tuple.Create("Status:", IsRentedRightLabel));
            }
            else if (IsOwned)
            {
                BaseList.Add(Tuple.Create("Status:", "~g~Owned~s~"));
            }
        }
        else
        {
            if (CanRent)
            {
                BaseList.Add(Tuple.Create("Rent:", CanRentRightLabel));

            }
            if (CanBuy)
            {
                BaseList.Add(Tuple.Create("Buy:", CanPurchaseRightLabel));
            }
        }
        return BaseList;

    }


}
