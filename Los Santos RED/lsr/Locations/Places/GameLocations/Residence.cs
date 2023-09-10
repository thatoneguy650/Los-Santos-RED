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
using System.Runtime;
using System.Runtime.Remoting.Channels;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

public class Residence : GameLocation, ILocationSetupable
{
    private UIMenu OfferSubMenu;
    private UIMenuNumericScrollerItem<int> RestMenuItem;
    private UIMenuItem InventoryMenuItem;
    private UIMenuItem PurchaseResidenceMenuItem;
    private UIMenuItem RentResidenceMenuItem;
    private UIMenuItem RentDisplayItem;
    private bool KeepInteractionGoing;
    private UIMenu outfitsSubMenu;
    private IActivityPerformable ActivityPerformable;
    private UIMenuItem RentStopItem;
    private UIMenuItem SellHouseItem;
    private UIMenu cashStorageSubMenu;
    private UIMenuListScrollerItem<int> incrementScroller;
    private UIMenuNumericScrollerItem<int> storeCashScroller;
    private UIMenuNumericScrollerItem<int> removeCashScroller;
    private int MaxAccountValue = 5000000;
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
    [XmlIgnore]
    public SimpleInventory SimpleInventory { get; set; }
    [XmlIgnore]
    public WeaponStorage WeaponStorage { get; set; }
    [XmlIgnore]
    public int StoredCash { get; set; }

    public bool CanRent => !IsOwned && !IsRented && RentalFee > 0;
    public bool CanBuy => !IsOwned && PurchasePrice > 0;
    public bool IsOwnedOrRented => IsOwned || IsRented;
    public int RentalDays { get; set; }
    public int RentalFee { get; set; }
    public int PurchasePrice { get; set; }
    public int SalesPrice { get; set; }
    public override string TypeName => IsOwnedOrRented ? "Residence" : "For Sale/Rental";
    public override int MapIcon { get; set; } = (int)BlipSprite.PropertyForSale;
    public override string ButtonPromptText { get; set; }
    public override int SortOrder => IsOwnedOrRented ? 1 : 999;

    public Residence(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {
        ButtonPromptText = GetButtonPromptText();
        OpenTime = 0;
        CloseTime = 24;
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
        if(!CanInteract)
        {
            return;
        }
        Player.ActivityManager.IsInteractingWithLocation = true;
        CanInteract = false;
        Player.IsTransacting = true;
        GameFiber.StartNew(delegate
        {
            try
            {
                StoreCamera = new LocationCamera(this, Player, Settings);
                StoreCamera.SayGreeting = false;
                StoreCamera.Setup();
                CreateInteractionMenu();
                InteractionMenu.Visible = true;
                InteractionMenu.OnItemSelect += InteractionMenu_OnItemSelect;
                if (!HasBannerImage)
                {
                    InteractionMenu.SetBannerType(EntryPoint.LSRedColor);
                }
                GenerateResidenceMenu();
                while (IsAnyMenuVisible || Time.IsFastForwarding || KeepInteractionGoing)
                {
                    MenuPool.ProcessMenus();
                    GameFiber.Yield();
                }
                //EntryPoint.WriteToConsole($"PLAYER EVENT: RESIDENCE LOOP CLOSING IsAnyMenuVisible {IsAnyMenuVisible} Time.IsFastForwarding {Time.IsFastForwarding}");
                DisposeInteractionMenu();
                StoreCamera.Dispose();
                Player.ActivityManager.IsInteractingWithLocation = false;
                CanInteract = true;
                Player.IsTransacting = false;
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole("Location Interaction" + ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "ResidenceInteract");
        
    }
    public void RefreshUI()
    {
        UpdateStoredData();
    }
    public override void Reset()
    {
        IsOwned = false;
        IsRented = false;
        WeaponStorage.Reset();
        SimpleInventory.Reset();
        UpdateStoredData();
        StoredCash = 0;
    }
    public void ReRent()
    {
        try
        {
            if (Player.BankAccounts.GetMoney(true) >= RentalFee)
            {
                Player.BankAccounts.GiveMoney(-1 * RentalFee, true);
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
        catch(Exception ex)
        {
            EntryPoint.WriteToConsole($"{ex.Message} {ex.StackTrace}",0);
            Game.DisplayNotification($"ERROR RERENTING {ex.Message}");
        }
    }
    private void InteractionMenu_OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if(selectedItem == RestMenuItem)
        {
            Rest(RestMenuItem.Value);
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
                if (CanBuy)
                {
                    offerDescription += "buy ";
                }
                if (CanRent)
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
                if (!HasBannerImage)
                {
                    OfferSubMenu.SetBannerType(EntryPoint.LSRedColor);
                }
                PurchaseResidenceMenuItem = new UIMenuItem("Purchase", "Select to purchase this residence") { RightLabel = CanPurchaseRightLabel };
                if (CanBuy)
                {
                    PurchaseResidenceMenuItem.Activated += (sender, e) =>
                    {
                        if (Purchase())
                        {
                            MenuPool.CloseAllMenus();
                        }
                    };
                    OfferSubMenu.AddItem(PurchaseResidenceMenuItem);
                }
                RentResidenceMenuItem = new UIMenuItem("Rent", $"Select to rent this residence for {RentalDays} days") { RightLabel = CanRentRightLabel };
                if (CanRent)
                {
                    RentResidenceMenuItem.Activated += (sender, e) =>
                    {
                        if (Rent())
                        {
                            MenuPool.CloseAllMenus();
                        }
                    };
                    OfferSubMenu.AddItem(RentResidenceMenuItem);
                }
            }
        }
        AddInteractionItems();
    }
    private void AddInteractionItems()
    {
        if(!IsOwned && !IsRented)
        {
            return;
        }
        if (IsRented)
        {
            RentDisplayItem = new UIMenuItem("Rental Period", IsRentedDescription) { RightLabel = IsRentedRightLabel };
            InteractionMenu.AddItem(RentDisplayItem);

            RentStopItem = new UIMenuItem("Stop Renting", "Stop renting the current location.");
            RentStopItem.Activated += (sender, e) =>
            {
                StopRenting();
            };
            InteractionMenu.AddItem(RentStopItem);

        }
        if(IsOwned)
        {
            SellHouseItem = new UIMenuItem("Sell House", "Sell the current house.") { RightLabel = SalesPrice.ToString("C0") };
            SellHouseItem.Activated += (sender, e) =>
            {
                SellHouse();
            };
            InteractionMenu.AddItem(SellHouseItem);
        }

        RestMenuItem = new UIMenuNumericScrollerItem<int>("Rest", "Rest at your residence to recover health. Select up to 12 hours.", 1, 12, 1) { Formatter = v => v.ToString() + " hours" };
        InteractionMenu.AddItem(RestMenuItem);
        outfitsSubMenu = MenuPool.AddSubMenu(InteractionMenu, "Outfits");



        if (!HasBannerImage)
        {
            outfitsSubMenu.SetBannerType(EntryPoint.LSRedColor);
        }

        InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1].Description = "Set an outfit.";
        UpdateOutfits();
        UpdateInventory();
        UpdateStoredWeapons();


        cashStorageSubMenu = MenuPool.AddSubMenu(InteractionMenu, "Stored Cash");
        InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1].Description = "Manage stored cash.";

        if (!HasBannerImage)
        {
            cashStorageSubMenu.SetBannerType(EntryPoint.LSRedColor);
        }


        UpdateStoredCash();
    }
    private void SellHouse()
    {
        OnSold();
        MenuPool.CloseAllMenus();
    }
    private void StopRenting()
    {
        OnStopRenting();
        MenuPool.CloseAllMenus();
    }
    private void UpdateStoredWeapons()
    {
        WeaponStorage.CreateInteractionMenu(Player,MenuPool, InteractionMenu, Weapons, ModItems, false);
    }
    private void UpdateInventory()
    {
        SimpleInventory.CreateInteractionMenu(Player, MenuPool, InteractionMenu, false);
    }
    private void UpdateOutfits()
    {
        outfitsSubMenu.Clear();
        foreach (SavedOutfit so in Player.OutfitManager.CurrentPlayerOutfits)
        {
            UIMenuItem uIMenuItem = new UIMenuItem(so.Name);
            uIMenuItem.Activated += (sender, e) =>
            {
                Player.OutfitManager.SetOutfit(so);
            };
            outfitsSubMenu.AddItem(uIMenuItem);
        }
    }
    private void UpdateStoredCash()
    {
        cashStorageSubMenu.Clear();
        incrementScroller = new UIMenuListScrollerItem<int>("Increment","Set the scroll increment.",new List<int>() { 1,5,25,100,500,1000,10000,100000 }) { Formatter = v => v.ToString("N0") };
        storeCashScroller = new UIMenuNumericScrollerItem<int>("Store Cash", $"Store the selected amount of cash. Max of ${MaxAccountValue}", 0, GetOnHandCash(), 1) { Value = GetOnHandCash(), Formatter = v => "~r~$" + v + "~s~", };
        removeCashScroller = new UIMenuNumericScrollerItem<int>("Remove Cash", "Remove the selected amount of cash.", 0, StoredCash, 1) { Value = StoredCash, Formatter = v => "~g~$" + v + "~s~", };
        incrementScroller.IndexChanged += (sender, oldIndex, newIndex) =>
        {
            storeCashScroller.Step = incrementScroller.SelectedItem;
            removeCashScroller.Step = incrementScroller.SelectedItem;
        };
        storeCashScroller.Activated += (sender,selectedItem) =>
        {
            int onHandCash = GetOnHandCash();
            int newStoreValue = StoredCash + storeCashScroller.Value;
            if (newStoreValue >= Int32.MaxValue || newStoreValue >= MaxAccountValue)
            {
                Game.DisplaySubtitle("Account is at Maximum!");
                return;
            }
            if (storeCashScroller.Value <= GetOnHandCash())
            {
                Player.BankAccounts.GiveMoney(-1 * storeCashScroller.Value, false);
                StoredCash += storeCashScroller.Value;   
                storeCashScroller.Maximum = GetOnHandCash();
                removeCashScroller.Maximum = StoredCash;
                storeCashScroller.Value = GetOnHandCash();
                removeCashScroller.Value = StoredCash;

                DisplayMessage("~g~Stored~s~", $"You have stored ${storeCashScroller.Value}.~n~Current Balance: ${StoredCash}");

            }
        };

        removeCashScroller.Activated += (sender, selectedItem) =>
        {
            if (removeCashScroller.Value + Player.BankAccounts.GetMoney(false) >= Int32.MaxValue)
            {
                Game.DisplaySubtitle("Money is at Maximum!");
                return;
            }
            if (StoredCash >= removeCashScroller.Value)
            {
                Player.BankAccounts.GiveMoney(removeCashScroller.Value, false);
                StoredCash -= removeCashScroller.Value;
                storeCashScroller.Maximum = GetOnHandCash();
                removeCashScroller.Maximum = StoredCash;
                storeCashScroller.Value = GetOnHandCash();
                removeCashScroller.Value = StoredCash;
                DisplayMessage("~g~Removed~s~", $"You have removed ${removeCashScroller.Value}.~n~Current Balance: ${StoredCash}");
            }
        };
        cashStorageSubMenu.AddItem(incrementScroller);
        cashStorageSubMenu.AddItem(storeCashScroller);
        cashStorageSubMenu.AddItem(removeCashScroller);
    }

    private int GetOnHandCash()
    {
        int money = Player.BankAccounts.GetMoney(false);
        if (money >= 2147483647)
        {
            money = 2147483646;
        }
        return money;
    }



    private bool Rent()
    {
        if(CanRent && Player.BankAccounts.GetMoney(true) >= RentalFee)
        {
            OnRented();
            return true;
        }
        PlayErrorSound();
        DisplayMessage("~r~Rental Failed", "We are sorry, we are unable to complete this rental. Please make sure you have the funds.");
        return false;
    }
    private bool Purchase()
    {
        if (CanBuy && Player.BankAccounts.GetMoney(true) >= PurchasePrice)
        {
            OnPurchased();
            return true;
        }
        PlayErrorSound();
        DisplayMessage("~r~Purchased Failed", "We are sorry, we are unable to complete this purchase. Please make sure you have the funds.");
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
            try
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
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "FastForwardWatcher");
        //EntryPoint.WriteToConsole($"PLAYER EVENT: START REST ACTIVITY AT RESIDENCE");
    }
    private void OnRented()
    {
        Player.BankAccounts.GiveMoney(-1 * RentalFee, true);
        DateRentalPaymentPaid = Time.CurrentDateTime;
        IsRented = true;
        DateRentalPaymentDue = DateRentalPaymentPaid.AddDays(RentalDays);
        UpdateStoredData();
        Player.Properties.AddResidence(this);
        AddInteractionItems();
        OfferSubMenu.Close(true);
        PlaySuccessSound();
        DisplayMessage("~g~Rented", $"Thank you for renting {Name}");
    }
    private void OnPurchased()
    {
        Player.BankAccounts.GiveMoney(-1 * PurchasePrice, true);
        IsOwned = true;
        IsRented = false;
        UpdateStoredData();
        Player.Properties.AddResidence(this);
        if (!IsRented)
        {
            AddInteractionItems();
            OfferSubMenu.Close(true);
        }
        PlaySuccessSound();
        DisplayMessage("~g~Purchased", $"Thank you for purchasing {Name}");
    }
    private void OnStopRenting()
    {
        Reset();
        Player.Properties.RemoveResidence(this);
        PlaySuccessSound();
        DisplayMessage("~y~Rental", $"You have stopped renting {Name}");
    }
    private void OnSold()
    {
        Reset();
        Player.Properties.RemoveResidence(this);
        Player.BankAccounts.GiveMoney(SalesPrice, true);
        PlaySuccessSound();
        DisplayMessage("~g~Sold", $"You have sold {Name} for {SalesPrice.ToString("C0")}");
    }
    private void UpdateStoredData()
    {
        ButtonPromptText = GetButtonPromptText();
        if (IsOwned)
        {
            MapIcon = (int)BlipSprite.Garage;
            MapIconColorString = "Green";

            if (Blip.Exists())
            {
                Blip.Color = Color.Green;
                Blip.Sprite = BlipSprite.Garage;
            }
        }
        else if(IsRented)
        {
            MapIcon = (int)BlipSprite.Garage;
            MapIconColorString = "Yellow";

            if (Blip.Exists())
            {
                Blip.Color = Color.Yellow;
                Blip.Sprite = BlipSprite.Garage;
            }
        }
        else
        {
            MapIcon = (int)BlipSprite.PropertyForSale;
            MapIconColorString = "White";
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
    public void Setup()
    {
        if (SimpleInventory == null)
        {
            SimpleInventory = new SimpleInventory(Settings);
        }
        if (WeaponStorage == null)
        {
            WeaponStorage = new WeaponStorage(Settings);
        }
    }

}
