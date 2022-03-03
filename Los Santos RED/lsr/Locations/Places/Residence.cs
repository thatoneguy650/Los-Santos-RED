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
    private StoreCamera StoreCamera;

    private IActivityPerformable Player;
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

    public Residence() : base()
    {

    }

    [XmlIgnore]
    public bool IsOwnedByPlayer { get; set; } = false;
    [XmlIgnore]
    public bool IsRentedByPlayer { get; set; } = false;
    [XmlIgnore]
    public DateTime RentalPaymentDate { get; set; }
    [XmlIgnore]
    public DateTime DateOfLastRentalPayment { get; set; }

    public bool CanRent => !IsOwnedByPlayer && !IsRentedByPlayer && RentalFee > 0;
    public bool CanBuy => !IsOwnedByPlayer && PurchasePrice > 0;
    public bool IsOwnedOrRented => IsOwnedByPlayer || IsRentedByPlayer;

    public int RentalDays { get; set; }
    public int RentalFee { get; set; }
    public int PurchasePrice { get; set; }

    public override int MapIcon { get; set; } = (int)BlipSprite.GarageForSale;
    public override Color MapIconColor { get; set; } = Color.White;
    public override float MapIconScale { get; set; } = 1.0f;
    public override string ButtonPromptText { get; set; }




    public Residence(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {
        ButtonPromptText = GetButtonPromptText();
        OpenTime = 0;
        CloseTime = 24;
    }
    public override void OnInteract(IActivityPerformable player, IModItems modItems, IEntityProvideable world, ISettingsProvideable settings, IWeapons weapons, ITimeControllable time)
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
                StoreCamera = new StoreCamera(this, Player);
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


                Player.IsInteractingWithLocation = false;
                CanInteract = true;
            }, "ResidenceInteract");
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
        if(!IsOwnedByPlayer || !IsRentedByPlayer)
        {
            if ((!IsOwnedByPlayer && CanBuy) || (!IsRentedByPlayer && CanRent))
            {
                OfferSubMenu = MenuPool.AddSubMenu(InteractionMenu, "Make an Offer");
                string offerDescription = "";
                if (!IsOwnedByPlayer && CanBuy)
                {
                    offerDescription += "buy ";
                }
                if (!IsRentedByPlayer && CanRent)
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


                if (!IsOwnedByPlayer && CanBuy)
                {
                    PurchaseResidenceMenuItem = new UIMenuItem("Purchase", "Select to purchase this residence") { RightLabel = $"{PurchasePrice:C0}" };
                    OfferSubMenu.AddItem(PurchaseResidenceMenuItem);
                }
                if (!IsRentedByPlayer && CanRent)
                {
                    RentResidenceMenuItem = new UIMenuItem("Rent", $"Select to rent this residence for {RentalDays} days") { RightLabel = $"{RentalFee:C0} for {RentalDays} days" };
                    OfferSubMenu.AddItem(RentResidenceMenuItem);
                }
            }
        }
        AddInteractionItems();
    }
    private void AddInteractionItems()
    {
        if (IsOwnedByPlayer || IsRentedByPlayer)
        {
            if (IsRentedByPlayer)
            {
                RentDisplayItem = new UIMenuItem("Rental Period", $"Rental Days: {RentalDays}~n~Remaining Days: ~o~{Math.Round((RentalPaymentDate - Time.CurrentDateTime).TotalDays, 0)}~s~~n~Rental Fee: ~r~{RentalFee:C0}~s~") { RightLabel = "Remaing Days: " + Math.Round((RentalPaymentDate - Time.CurrentDateTime).TotalDays, 0).ToString() };
                InteractionMenu.AddItem(RentDisplayItem);
            }
            RestMenuItem = new UIMenuNumericScrollerItem<int>("Rest", "Rest at your residence to recover health. Select up to 12 hours.", 1, 12, 1) { Formatter = v => v.ToString() + " hours" };
            InventoryMenuItem = new UIMenuItem("Inventory", "Access the inventory at this location");
            InventoryMenuItem.RightBadge = UIMenuItem.BadgeStyle.Heart;
            InteractionMenu.AddItem(RestMenuItem);
            InteractionMenu.AddItem(InventoryMenuItem);

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
        if(CanRent && Player.Money >= RentalFee)
        {
            OnRented();
            return true;
        }
        Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", Name, "~r~Rental Failed", "We are sorry, we are unable to complete this rental. Please make sure you have the funds.");
        return false;
    }
    private bool Purchase()
    {
        if (CanBuy && Player.Money >= PurchasePrice)
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
        GameFiber FastForwardWatcher = GameFiber.StartNew(delegate
        {
            while (Time.IsFastForwarding)
            {
                if (Game.LocalPlayer.Character.Health < Game.LocalPlayer.Character.MaxHealth - 1)
                {
                    Game.LocalPlayer.Character.Health++;
                }
                GameFiber.Yield();
            }
            InteractionMenu.Visible = true;
            KeepInteractionGoing = false;
        }, "FastForwardWatcher");
        EntryPoint.WriteToConsole($"PLAYER EVENT: START REST ACTIVITY AT RESIDENCE", 3);
    }
    private void OnRented()
    {
        Player.GiveMoney(-1 * RentalFee);
        DateOfLastRentalPayment = Time.CurrentDateTime;
        IsRentedByPlayer = true;
        RentalPaymentDate = DateOfLastRentalPayment.AddDays(RentalDays);
        ButtonPromptText = GetButtonPromptText();

        MapIcon = (int)BlipSprite.Garage;
        MapIconColor = Color.Yellow;

        Blip.Color = Color.Yellow;
        Blip.Sprite = BlipSprite.Garage;


        AddInteractionItems();
        OfferSubMenu.Close(true);
        

        Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", Name, "~g~Rented", $"Thank you for renting {Name}");
    }
    private void OnPurchased()
    {
        Player.GiveMoney(-1 * PurchasePrice);
        IsOwnedByPlayer = true;
        ButtonPromptText = GetButtonPromptText();

        MapIcon = (int)BlipSprite.Garage;
        MapIconColor = Color.Green;

        Blip.Color = Color.Green;
        Blip.Sprite = BlipSprite.Garage;


        if(!IsRentedByPlayer)
        {
            AddInteractionItems();
            OfferSubMenu.Close(true);
        }

        Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", Name, "~g~Purchased", $"Thank you for purchasing {Name}");
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

}
