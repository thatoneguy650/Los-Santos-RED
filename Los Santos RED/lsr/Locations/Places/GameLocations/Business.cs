using LosSantosRED.lsr.Interface;
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

public class Business : GameLocation, IInventoryableLocation, ILocationSetupable, IPayoutDisbursable
{
    private UIMenu OfferSubMenu;
    private string CanPurchaseRightLabel => $"{PurchasePrice:C0}";
    private UIMenuItem PurchaseBusinessMenuItem;
    private UIMenuItem SellBusinessItem;


    public Business() : base()
    {
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
        if (CashStorage == null)
        {
            CashStorage = new CashStorage();
        }
    }
    public override string TypeName { get; set; } = "Business";
    public override int MapIcon { get; set; } = (int)BlipSprite.BusinessForSale;
    public override string ButtonPromptText { get; set; }
    public bool IsPayoutInModItems { get; set; } = false;
    public List<string> PossibleModItemPayouts { get; set; }
    public int ModItemPayoutAmount { get; set; } = 1;
    public bool IsPayoutDepositedToBank { get; set; } = false;
    [XmlIgnore]
    public string ModItemToPayout { get; set; }
    [XmlIgnore]
    public SimpleInventory SimpleInventory { get; set; }
    [XmlIgnore]
    public WeaponStorage WeaponStorage { get; set; }
    [XmlIgnore]
    public CashStorage CashStorage { get; set; }
    [XmlIgnore]
    public BusinessInterior BusinessInterior { get; set; }
    public bool CanBuy => !IsOwned && PurchasePrice > 0;

    public GameLocation GameLocation => throw new NotImplementedException();

    public Business(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {
        ButtonPromptText = GetButtonPromptText();
    }
    public override bool CanCurrentlyInteract(ILocationInteractable player)
    {
        ButtonPromptText = GetButtonPromptText();
        return true;
    }
    public override List<Tuple<string, string>> DirectoryInfo(int currentHour, float distanceTo)
    {
        List<Tuple<string, string>> BaseList = base.DirectoryInfo(currentHour, distanceTo).ToList();
        return BaseList;
    }
    public override void AddLocation(PossibleLocations possibleLocations)
    {
        possibleLocations.Businesses.Add(this);
        base.AddLocation(possibleLocations);
    }
    public override void OnInteract()
    {
        if (BusinessInterior != null && BusinessInterior.IsTeleportEntry && IsOwned)
        {
            if(BusinessInterior.IsMPInterior && !World.IsMPMapLoaded)
            {
                StandardInteract(null, false);
                return;
            }
            DoEntranceCamera(false);
            BusinessInterior.SetBusiness(this);
            BusinessInterior.Teleport(Player, this, StoreCamera);
        }
        else
        {
            StandardInteract(null, false);
        }
    }
    public override void StoreData(IShopMenus shopMenus, IAgencies agencies, IGangs gangs, IZones zones, IJurisdictions jurisdictions, IGangTerritories gangTerritories, INameProvideable Names, ICrimes Crimes, IPedGroups PedGroups, IEntityProvideable world,
    IStreets streets, ILocationTypes locationTypes, ISettingsProvideable settings, IPlateTypes plateTypes, IOrganizations associations, IContacts contacts, IInteriors interiors,
        ILocationInteractable player, IModItems modItems, IWeapons weapons, ITimeControllable time, IPlacesOfInterest placesOfInterest, IIssuableWeapons issuableWeapons, IHeads heads, IDispatchablePeople dispatchablePeople)
    {
        base.StoreData(shopMenus, agencies, gangs, zones, jurisdictions, gangTerritories, Names, Crimes, PedGroups, world, streets, locationTypes, settings, plateTypes, associations, contacts, interiors, player, modItems, weapons, time, placesOfInterest, issuableWeapons, heads, dispatchablePeople);
        if (HasInterior)
        {
            BusinessInterior = interiors.PossibleInteriors.BusinessInteriors.Where(x => x.LocalID == InteriorID).FirstOrDefault();
            interior = BusinessInterior;
            if (BusinessInterior != null)
            {
                BusinessInterior.SetBusiness(this);
            }
        }
    }
    public override void StandardInteract(LocationCamera locationCamera, bool isInside)
    {
        Player.ActivityManager.IsInteractingWithLocation = true;
        CanInteract = false;
        Player.IsTransacting = true;
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
                GenerateBusinessMenu(isInside);
                while (IsAnyMenuVisible || Time.IsFastForwarding)
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
        }, "BusinessInteract");
    }
    private void GenerateBusinessMenu(bool isInside)
    {
        InteractionMenu.Clear();
        AddInquireItems();
        AddInteractionItems(isInside);
    }
    private void AddInquireItems()
    {
        if ((!IsOwned && CanBuy))
        {
            OfferSubMenu = MenuPool.AddSubMenu(InteractionMenu, "Make an Offer");
            string offerDescription = "";
            if (CanBuy)
            {
                offerDescription += "buy ";
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
            PurchaseBusinessMenuItem = new UIMenuItem("Purchase", "Select to purchase this business.") { RightLabel = CanPurchaseRightLabel };
            if (CanBuy)
            {
                PurchaseBusinessMenuItem.Activated += (sender, e) =>
                {
                    if (Purchase())
                    {
                        MenuPool.CloseAllMenus();
                        GenerateBusinessMenu(false);
                    }
                };
                OfferSubMenu.AddItem(PurchaseBusinessMenuItem);
            }
        }
    }
    public override void Payout(IPropertyOwnable player, ITimeReportable time)
    {
        try
        {
            if (player == null)
            {
                return;
            }
            int numberOfPaymentsToProcess = (time.CurrentDateTime - DatePayoutPaid).Days;
            DatePayoutPaid = time.CurrentDateTime;
            DatePayoutDue = DatePayoutPaid.AddDays(PayoutFrequency);

            if (IsPayoutInModItems)
            {
                ModItem itemToAdd = ModItems.Get(ModItemToPayout);
                SimpleInventory.Add(itemToAdd, numberOfPaymentsToProcess * ModItemPayoutAmount);
            }
            else
            {
                int payoutAmount = CalculatePayoutAmount(numberOfPaymentsToProcess);
                if (IsPayoutDepositedToBank && player.BankAccounts.BankAccountList.Any())
                {
                    player.BankAccounts.GiveMoney(payoutAmount, true);
                }
                else
                {
                    CashStorage.StoredCash = CashStorage.StoredCash + payoutAmount;
                }
            }
            int salesPriceToAdd = (int)(PurchasePrice * (GrowthPercentage / 100.0));
            CurrentSalesPrice = Math.Min(CurrentSalesPrice + salesPriceToAdd, MaxSalesPrice == 0? (int)(PurchasePrice * 1.2f):MaxSalesPrice);
        }
        catch (Exception ex)
        {
            EntryPoint.WriteToConsole($"{ex.Message} {ex.StackTrace}", 0);
            Game.DisplayNotification($"ERROR in Payout {ex.Message}");
        }
    }
    public override void Reset()
    {
        IsOwned = false;
        WeaponStorage.Reset();
        SimpleInventory.Reset();
        UpdateStoredData();
        CashStorage.Reset();
        IsPayoutInModItems = false;
        ModItemToPayout = String.Empty;
    }
    private void AddInteractionItems(bool isInside)
    {
        if (!IsOwned)
        {
            return;
        }
        CreateOwnershipInteractionMenu();

        bool withAnimations = Interior?.IsTeleportEntry == true;

        SimpleInventory.CreateInteractionMenu(Player, MenuPool, InteractionMenu, withAnimations, null, null, !isInside, null, null);
        WeaponStorage.CreateInteractionMenu(Player, MenuPool, InteractionMenu, Weapons, ModItems, withAnimations, !isInside);
        CashStorage.CreateInteractionMenu(Player, MenuPool, InteractionMenu, this, withAnimations, !isInside);
    }

    //Make a function for description rendering
    private void CreateOwnershipInteractionMenu()
    {
        if (IsOwned)
        {
            SellBusinessItem = new UIMenuItem("Sell Business", "Sell the current business.") { RightLabel = CurrentSalesPrice.ToString("C0") };
            SellBusinessItem.Activated += (sender, e) =>
            {
                OnSold();
                MenuPool.CloseAllMenus();
                Interior?.ForceExitPlayer(Player, this);
            };
            InteractionMenu.AddItem(SellBusinessItem);
            UIMenuItem payoutDescription = new UIMenuItem(IsPayoutInModItems ? ModItemToPayout : "Cash");
            UIMenuCheckboxItem payoutToBankMenuItem = new UIMenuCheckboxItem("Deposit to Bank?", IsPayoutDepositedToBank);
            payoutToBankMenuItem.CheckboxEvent += (s, c) =>
            {
                IsPayoutDepositedToBank = c;
            };
            if (PossibleModItemPayouts != null && PossibleModItemPayouts.Count > 0)
            {
                UIMenuListScrollerItem<string> payoutItemScrollerItem = new UIMenuListScrollerItem<string>("Produce", "Your payout option.");
                foreach (string modItem in PossibleModItemPayouts)
                {
                    payoutItemScrollerItem.Items.Add(modItem);
                }
                payoutItemScrollerItem.Activated += (sender, selectedItem) =>
                {
                    ModItemToPayout = payoutItemScrollerItem.SelectedItem;
                    UpdatePayoutDescription(payoutDescription);
                };
                UIMenuCheckboxItem payOutInModItems = new UIMenuCheckboxItem("Produce Items", IsPayoutInModItems);
                payOutInModItems.CheckboxEvent += (s, c) =>
                {
                    IsPayoutInModItems = c;
                    ModItemToPayout = payoutItemScrollerItem.SelectedItem;
                    UpdatePayoutDescription(payoutDescription);
                };
                InteractionMenu.AddItems(payoutDescription, payoutToBankMenuItem, payOutInModItems, payoutItemScrollerItem);
            }
        }
    }
    private void UpdatePayoutDescription(UIMenuItem payoutDescription)
    {
        if (IsPayoutInModItems)
        {
            payoutDescription.Text = ModItemToPayout;
            payoutDescription.Description = $"{ModItemPayoutAmount} every {PayoutFrequency} day(s)";
        }
        else
        {
            payoutDescription.Text = "Cash";
            payoutDescription.Description = $"Around ${PayoutMin} to ${PayoutMax} every {PayoutFrequency} day(s)";

        }
    }
    private void OnSold()
    {
        Reset();
        Player.Properties.RemovePayoutProperty(this);
        Player.BankAccounts.GiveMoney(CurrentSalesPrice, true);
        PlaySuccessSound();
        DisplayMessage("~g~Sold", $"You have sold {Name} for {CurrentSalesPrice.ToString("C0")}");
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
    private void OnPurchased()
    {
        Player.BankAccounts.GiveMoney(-1 * PurchasePrice, true);
        IsOwned = true;
        UpdateStoredData();
        Player.Properties.AddPayoutProperty(this);
        AddInteractionItems(false);
        PlaySuccessSound();
        DisplayMessage("~g~Purchased", $"Thank you for purchasing {Name}");
        DatePayoutPaid = Time.CurrentDateTime;
        DatePayoutDue = DatePayoutPaid.AddDays(PayoutFrequency);
        CurrentSalesPrice = SalesPrice;
    }
    private void UpdateStoredData()
    {
        ButtonPromptText = GetButtonPromptText();
        if (Blip.Exists())
        {
            Blip.Sprite = IsOwned ? BlipSprite.Business : BlipSprite.BusinessForSale;
        }
    }
    private string GetButtonPromptText()
    {
        if (IsOwned)
        {
            return $"Manage {Name}";
        }
        else
        {
            return $"Inquire About {Name}";
        }
    }
    public void RefreshUI()
    {
        UpdateStoredData();
    }
    public void CreateInventoryMenu(bool withItems, bool withWeapons, bool withCash, List<ItemType> AllowedItemTypes, List<ItemType> DisallowedItemTypes, bool removeBanner, string overrideItemTitle, string overrideItemDescription)
    {
        Player.ActivityManager.IsInteractingWithLocation = true;
        Player.IsTransacting = true;
        CreateInteractionMenu();
        if (removeBanner)
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
        while (IsAnyMenuVisible || Time.IsFastForwarding)
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
}