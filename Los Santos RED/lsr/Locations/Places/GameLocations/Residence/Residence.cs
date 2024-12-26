using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Microsoft.VisualBasic;
using NAudio.Wave;
using Rage;
using Rage.Native;
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

public class Residence : GameLocation, ILocationSetupable, IRestableLocation, IInventoryableLocation, IOutfitableLocation
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
    private bool HasTeleported = false;

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
    public CashStorage CashStorage { get; set; }
    [XmlIgnore]
    public ResidenceInterior ResidenceInterior { get; set; }
    public bool CanRent => !IsOwned && !IsRented && RentalFee > 0;
    public bool CanBuy => !IsOwned && PurchasePrice > 0;
    public bool IsOwnedOrRented => IsOwned || IsRented;
    public int RentalDays { get; set; }
    public int RentalFee { get; set; }
    public int PurchasePrice { get; set; }
    public int SalesPrice { get; set; }
    public int ResidenceID { get; set; } = -999;
    public bool HasHeaderApartmentBuilding { get; set; } = false;
    public bool DisableInteractAfterPurchase { get; set; } = false;
    public override string TypeName => IsOwnedOrRented ? "Residence" : "For Sale/Rental";
    public override int MapIcon { get; set; } = (int)BlipSprite.PropertyForSale;
    [XmlIgnore]
    public override string ButtonPromptText { get; set; }
    public override int SortOrder => IsOwnedOrRented ? 1 : 999;
    public override bool ShowInteractPrompt => !IgnoreEntranceInteract && CanInteract && !HasHeaderApartmentBuilding && (!IsOwnedOrRented || (IsOwnedOrRented && !DisableInteractAfterPurchase));
    public override bool IsBlipEnabled => base.IsBlipEnabled && !HasHeaderApartmentBuilding;
    public GameLocation GameLocation => this;
    public Residence(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {
        ButtonPromptText = GetButtonPromptText();
        OpenTime = 0;
        CloseTime = 24;
    }
    public override void StoreData(IShopMenus shopMenus, IAgencies agencies, IGangs gangs, IZones zones, IJurisdictions jurisdictions, IGangTerritories gangTerritories, INameProvideable Names, ICrimes Crimes, IPedGroups PedGroups, IEntityProvideable world,
    IStreets streets, ILocationTypes locationTypes, ISettingsProvideable settings, IPlateTypes plateTypes, IOrganizations associations, IContacts contacts, IInteriors interiors,
        ILocationInteractable player, IModItems modItems, IWeapons weapons, ITimeControllable time, IPlacesOfInterest placesOfInterest, IIssuableWeapons issuableWeapons, IHeads heads, IDispatchablePeople dispatchablePeople)
    {
        base.StoreData(shopMenus, agencies, gangs, zones, jurisdictions, gangTerritories, Names, Crimes, PedGroups, world, streets, locationTypes, settings, plateTypes, associations, contacts, interiors, player, modItems, weapons, time, placesOfInterest, issuableWeapons, heads, dispatchablePeople);
        if (HasInterior)
        {
            ResidenceInterior = interiors.PossibleInteriors.ResidenceInteriors.Where(x => x.LocalID == InteriorID).FirstOrDefault();
            interior = ResidenceInterior;
            if (ResidenceInterior != null)
            {
                ResidenceInterior.SetResidence(this);
            }
        }
    }
    public void OnInteractFromApartment(ILocationInteractable player, IModItems modItems, IEntityProvideable world, ISettingsProvideable settings, IWeapons weapons, ITimeControllable time, IPlacesOfInterest placesOfInterest, LocationCamera storeCamera)
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
        if (!CanInteract)
        {
            return;
        }
        if (ResidenceInterior != null && ResidenceInterior.IsTeleportEntry && IsOwnedOrRented)
        {
            ResidenceInterior.SetResidence(this);
            ResidenceInterior.Teleport(Player, this, storeCamera);
        }
        else
        {
            StandardInteract(storeCamera, false);
        }
    }
    public override bool CanCurrentlyInteract(ILocationInteractable player)
    {
        ButtonPromptText = GetButtonPromptText();
        return true;
    }
    public override void OnInteract()
    {
        if (IsLocationClosed())
        {
            return;
        }
        if(!CanInteract)
        {
            return;
        }
        if (ResidenceInterior != null && ResidenceInterior.IsTeleportEntry && IsOwnedOrRented)
        {
            DoEntranceCamera(false);
            ResidenceInterior.SetResidence(this);
            ResidenceInterior.Teleport(Player, this, StoreCamera);
        }
        else
        {
            StandardInteract(null, false);
        }
    }
    public override void StandardInteract(LocationCamera locationCamera, bool isInside)
    {
        Player.ActivityManager.IsInteractingWithLocation = true;
        CanInteract = false;
        Player.IsTransacting = true;
        HasTeleported = false;
        GameFiber.StartNew(delegate
        {
            try
            {
                SetupLocationCamera(locationCamera, isInside, false);
                CreateInteractionMenu();
                InteractionMenu.Visible = true;
                if (!HasBannerImage)
                {
                    InteractionMenu.SetBannerType(EntryPoint.LSRedColor);
                }
                GenerateResidenceMenu(isInside);
                while (IsAnyMenuVisible || Time.IsFastForwarding || KeepInteractionGoing)
                {
                    MenuPool.ProcessMenus();
                    GameFiber.Yield();
                }
                DisposeInteractionMenu();
                DisposeCamera(isInside);
                DisposeInterior();
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
    protected override void DisposeCamera(bool isInside)
    {
        if (isInside)
        {
            StoreCamera.ReturnToGameplay(true);
            StoreCamera.StopImmediately(true);
        }
        else if (!HasTeleported)
        {
            StoreCamera.Dispose();
        }
    }
    public void CreateRestMenu(bool removeBanner)
    {
        Player.ActivityManager.IsInteractingWithLocation = true;
        Player.IsTransacting = true;
        CreateInteractionMenu();
        InteractionMenu.Visible = true;
        if (removeBanner)
        {
            InteractionMenu.RemoveBanner();
        }
        else if (!HasBannerImage)
        {
            InteractionMenu.SetBannerType(EntryPoint.LSRedColor);
        }
        InteractionMenu.Clear();
        CreateRestInteractionMenu();
        while (IsAnyMenuVisible || Time.IsFastForwarding || KeepInteractionGoing)
        {
            MenuPool.ProcessMenus();
            GameFiber.Yield();
        }
        DisposeInteractionMenu();
       // StoreCamera?.StopImmediately(false);
        Player.ActivityManager.IsInteractingWithLocation = false;
        Player.IsTransacting = false;
        if (Interior != null)
        {
            Interior.IsMenuInteracting = false;
        }
    }
    public void CreateInventoryMenu(bool withItems, bool withWeapons, bool withCash, List<ItemType> AllowedItemTypes, List<ItemType> DisallowedItemTypes, bool removeBanner, string overrideItemTitle, string overrideItemDescription)
    {
        Player.ActivityManager.IsInteractingWithLocation = true;
        Player.IsTransacting = true;
        CreateInteractionMenu();
        if(removeBanner)
        {
            InteractionMenu.RemoveBanner();
        }
        else if (!HasBannerImage)
        {
            InteractionMenu.SetBannerType(EntryPoint.LSRedColor);
        }
        InteractionMenu.Visible = true;
        InteractionMenu.Clear();
        bool withAnimations = Interior?.IsTeleportEntry == true;
        if (withItems)
        {
            SimpleInventory.CreateInteractionMenu(Player, MenuPool, InteractionMenu, withAnimations, AllowedItemTypes, DisallowedItemTypes, removeBanner, overrideItemTitle, overrideItemDescription);
        }
        if (withWeapons)
        {
            WeaponStorage.CreateInteractionMenu(Player, MenuPool, InteractionMenu, Weapons, ModItems, withAnimations, removeBanner);
        }
        if (withCash)
        {
            CashStorage.CreateInteractionMenu(Player, MenuPool, InteractionMenu, this, withAnimations, removeBanner);
        }
        while (IsAnyMenuVisible || Time.IsFastForwarding || KeepInteractionGoing)
        {
            MenuPool.ProcessMenus();
            GameFiber.Yield();
        }
        DisposeInteractionMenu();
        Player.ActivityManager.IsInteractingWithLocation = false;
        Player.IsTransacting = false;
        if (Interior != null)
        {
            Interior.IsMenuInteracting = false;
        }
    }
    public void CreateOutfitMenu(bool removeBanner, bool isInside)
    {
        Player.ActivityManager.IsInteractingWithLocation = true;
        Player.IsTransacting = true;
        CreateInteractionMenu();
        InteractionMenu.Visible = true;
        if(removeBanner)
        {
            InteractionMenu.RemoveBanner();
        }
        else if (!HasBannerImage)
        {
            InteractionMenu.SetBannerType(EntryPoint.LSRedColor);
        }
        InteractionMenu.Clear();
        CreateOutfitInteractionMenu(removeBanner, isInside);
        while (IsAnyMenuVisible || Time.IsFastForwarding || KeepInteractionGoing)
        {
            MenuPool.ProcessMenus();
            GameFiber.Yield();
        }
        DisposeInteractionMenu();
        //StoreCamera?.StopImmediately(true);
        Player.ActivityManager.IsInteractingWithLocation = false;
        Player.IsTransacting = false;
        if (Interior != null)
        {
            Interior.IsMenuInteracting = false;
        }
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
        CashStorage.Reset();

    }
    public void ReRent(IPropertyOwnable player, ITimeReportable time)
    {
        try
        {
            if(player == null)
            {
                return;
            }
            if (player.BankAccounts.GetMoney(true) >= RentalFee)
            {
                player.BankAccounts.GiveMoney(-1 * RentalFee, true);
                DateRentalPaymentPaid = time.CurrentDateTime;
                DateRentalPaymentDue = DateRentalPaymentPaid.AddDays(RentalDays);
                UpdateStoredData();

                EntryPoint.WriteToConsole($"IsRented{IsRented} IsOwned{IsOwned} DateRentalPaymentPaid{DateRentalPaymentPaid} DateRentalPaymentDue{DateRentalPaymentDue}");

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
    private void GenerateRestMenu()
    {
        if (!IsOwnedOrRented)
        {
            return;
        }
        InteractionMenu.Clear();
        CreateOwnershipInteractionMenu();
        CreateRestInteractionMenu();
    }
    private void GenerateResidenceMenu(bool isInside)
    {
        InteractionMenu.Clear();
        AddInquireItems();
        AddInteractionItems(isInside);
    }
    //private void GenerateSpecificInteractMenu(bool createOwnershipInteraction, bool createRestInteraction, bool createOutfitInteraction, bool createInventoryInteraction, bool createWeaponInteraction, bool createCashInteractionMenu)//needs toa lready be bought, some sort of restrict parameter to determine which it is?
    //{
    //    if(!IsOwnedOrRented)
    //    {
    //        return;
    //    }
    //    InteractionMenu.Clear();
    //    CreateOwnershipInteractionMenu();
    //    CreateRestInteractionMenu();
    //    CreateOutfitInteractionMenu();
    //    SimpleInventory.CreateInteractionMenu(Player, MenuPool, InteractionMenu, false);
    //    WeaponStorage.CreateInteractionMenu(Player, MenuPool, InteractionMenu, Weapons, ModItems, false);
    //    CashStorage.CreateInteractionMenu(Player, MenuPool, InteractionMenu, this, false);
    //}
    private void AddInquireItems()
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
                        OnRentedOrPurchased();
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
                        OnRentedOrPurchased();
                    }
                };
                OfferSubMenu.AddItem(RentResidenceMenuItem);
            }
        }
    }
    private void AddInteractionItems(bool isInside)
    {
        if(!IsOwned && !IsRented)
        {
            return;
        }
        CreateOwnershipInteractionMenu();
        if (!isInside)
        {
            CreateRestInteractionMenu();
        }
        CreateOutfitInteractionMenu(isInside, isInside);


        bool withAnimations = Interior?.IsTeleportEntry == true;

        SimpleInventory.CreateInteractionMenu(Player, MenuPool, InteractionMenu, withAnimations, null, null, !isInside, null, null);
        WeaponStorage.CreateInteractionMenu(Player, MenuPool, InteractionMenu, Weapons, ModItems, withAnimations, !isInside);
        CashStorage.CreateInteractionMenu(Player, MenuPool, InteractionMenu, this, withAnimations, !isInside);
    }
    private void CreateOwnershipInteractionMenu()
    {
        if (IsRented)
        {
            RentDisplayItem = new UIMenuItem("Rental Period", IsRentedDescription) { RightLabel = IsRentedRightLabel };
            InteractionMenu.AddItem(RentDisplayItem);

            RentStopItem = new UIMenuItem("Stop Renting", "Stop renting the current location.");
            RentStopItem.Activated += (sender, e) =>
            {
                OnStopRenting();
                MenuPool.CloseAllMenus();
                Interior?.ForceExitPlayer(Player, this);
            };
            InteractionMenu.AddItem(RentStopItem);
        }
        if (IsOwned)
        {
            SellHouseItem = new UIMenuItem("Sell House", "Sell the current house.") { RightLabel = SalesPrice.ToString("C0") };
            SellHouseItem.Activated += (sender, e) =>
            {
                OnSold();
                MenuPool.CloseAllMenus();
                Interior?.ForceExitPlayer(Player, this);
            };
            InteractionMenu.AddItem(SellHouseItem);
        }
    }
    private void CreateRestInteractionMenu()
    {
        RestMenuItem = new UIMenuNumericScrollerItem<int>("Rest", "Rest at your residence to recover health. Select up to 12 hours.", 1, 12, 1) { Formatter = v => v.ToString() + " hours" };
        RestMenuItem.Activated += (sender, selectedItem) =>
        {
            Rest(RestMenuItem.Value);
        };
        InteractionMenu.AddItem(RestMenuItem);
    }
    private void CreateOutfitInteractionMenu(bool removeBanner, bool isInside)
    {
        outfitsSubMenu = MenuPool.AddSubMenu(InteractionMenu, "Outfits");
        if (removeBanner)
        {
            outfitsSubMenu.RemoveBanner();
        }
        else if (!HasBannerImage)
        {
            outfitsSubMenu.SetBannerType(EntryPoint.LSRedColor);
        }
        InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1].Description = "Set an outfit.";
        Player.OutfitManager.CreateOutfitMenu(MenuPool, outfitsSubMenu, isInside, removeBanner);
    }
    private void OnRentedOrPurchased()
    {
        if (ResidenceInterior != null && ResidenceInterior.IsTeleportEntry)
        {
            ResidenceInterior.SetResidence(this);
            ResidenceInterior.Teleport(Player, this, StoreCamera);
            HasTeleported = true;
            MenuPool.CloseAllMenus();
        }
        else if (DisableInteractAfterPurchase)
        {
            ResidenceInterior?.Load(true);
            MenuPool.CloseAllMenus();
        }
        else
        {
            GenerateResidenceMenu(false);
        }
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
        AddInteractionItems(false);
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
            AddInteractionItems(false);
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
        EntryPoint.WriteToConsole($"UPDATE STORED DATA RAN FOR {Name} ");
        ButtonPromptText = GetButtonPromptText();
        if (Blip.Exists())
        {
            EntryPoint.WriteToConsole($"UPDATE STORED DATA RAN FOR {Name} BLIP SPRITE EXISTS IsOwned{IsOwned} IsRented{IsRented}");
            Blip.Sprite = IsOwned || IsRented ? BlipSprite.Garage : BlipSprite.PropertyForSale;
        }

        //UpdateBlip(Time);


        //if (IsOwned)
        //{
        //    EntryPoint.WriteToConsole("UPDATE STORED DATA OWNED");

        //    MapIcon = (int)BlipSprite.Garage;
        //    //MapIconColorString = "Green";

        //    if (Blip.Exists())
        //    {
        //        Blip.Color = Color.Green;
        //        Blip.Sprite = BlipSprite.Garage;


        //        EntryPoint.WriteToConsole("UPDATE STORED DATA OWNED BLIP EXISTS");
        //    }
        //}
        //else if(IsRented)
        //{
        //    //EntryPoint.WriteToConsole("UPDATE STORED DATA RENTED");
        //    MapIcon = (int)BlipSprite.Garage;
        //    //MapIconColorString = "Yellow";

        //    if (Blip.Exists())
        //    {
        //        Blip.Color = Color.Yellow;
        //        Blip.Sprite = BlipSprite.Garage;

        //        //EntryPoint.WriteToConsole("UPDATE STORED DATA RENTED BLIP EXISTS");
        //    }
        //}
        //else
        //{
        //    //EntryPoint.WriteToConsole("UPDATE STORED DATA NOT OWNED OR RENTED");
        //    MapIcon = (int)BlipSprite.PropertyForSale;
        //    //MapIconColorString = "White";
        //    if (Blip.Exists())
        //    {
        //        Blip.Color = Color.White;
        //        Blip.Sprite = BlipSprite.PropertyForSale;
        //        //EntryPoint.WriteToConsole("UPDATE STORED DATA NONE BLIP EXISTS");
        //    }
        //}
    }
    public override void UpdateBlip(ITimeReportable time)
    {
        if (Blip.Exists())
        {
            MapIconColorString = (IsOwned ? "Green" : IsRented ? "Yellow" : "White");
            Blip.Sprite = IsOwned || IsRented ? BlipSprite.Garage: BlipSprite.PropertyForSale;
            Blip.Color = Color.FromName(IsOwned ? "Green" : IsRented ? "Yellow" : "White");//NEED TO SET THE COLOR AFTER THE SPRITE OR IT DOESNT WORK!
        }
        if(IsOwnedOrRented)//shouldnt need blip updates?
        {
            return;
        }
        base.UpdateBlip(time);
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
        BaseList.Add(Tuple.Create("Has Interior:", $"{(ResidenceInterior != null ? "Yes" : "No")}"));    
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
        if(CashStorage == null)
        {
            CashStorage = new CashStorage();
        }
    }
    public override void AddLocation(PossibleLocations possibleLocations)
    {
        possibleLocations.Residences.Add(this);
        base.AddLocation(possibleLocations);
    }
}
