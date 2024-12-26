using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Mod;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

public class Forger : GameLocation
{
    private UIMenu PlateSubMenu;
    private UIMenu CustomPlateSubMenu;
    private UIMenu SellPlateSubMenu;
    private UIMenu IDSubMenu;
    private UIMenu SellIDSubMenu;
    private UIMenu MarkedCashSubMenu;

    public Forger(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {

    }
    public Forger() : base()
    {

    }
    public override string TypeName { get; set; } = "Forger";
    public override int MapIcon { get; set; } = 438;//402 = car repair
    public override bool ShowsOnDirectory => false;
    public int RandomPlateCost { get; set; } = 300;
    public int CustomPlateCost { get; set; } = 500;
    public int WantedPlateSalesPrice { get; set; } = 50;
    public int CleanPlateSalesPrice { get; set; } = 200;


    public int PoliceIdentificationSalesPrice { get; set; } = 550;
    public int IdentificationSalesPrice { get; set; } = 45;

    public int IdentificationPurchasePrice { get; set; } = 450;


    public int MarkedBillsSalesPrice { get; set; } = 1500;

    public override bool CanCurrentlyInteract(ILocationInteractable player)
    {
        ButtonPromptText = $"Enter {Name}";
        return true;
    }
    public override void OnInteract()//ILocationInteractable player, IModItems modItems, IEntityProvideable world, ISettingsProvideable settings, IWeapons weapons, ITimeControllable time, IPlacesOfInterest placesOfInterest)
    {
        //Player = player;
        //ModItems = modItems;
        //World = world;
        //Settings = settings;
        //Weapons = weapons;
        //Time = time;
        if (IsLocationClosed())
        {
            return;
        }
        if (!CanInteract)
        {
            return;
        }
        if (Interior != null && Interior.IsTeleportEntry)
        {
            DoEntranceCamera(false);
            Interior.Teleport(Player, this, StoreCamera);
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
        GameFiber.StartNew(delegate
        {
            try
            {
                SetupLocationCamera(locationCamera, isInside, false);
                CreateInteractionMenu();
                Transaction = new Transaction(MenuPool, InteractionMenu, Menu, this);
                InteractionMenu.Visible = true;
                Interact();
                ProcessInteractionMenu();
                DisposeInteractionMenu();
                DisposeCamera(isInside);
                DisposeInterior();
                Player.IsTransacting = false;
                Player.ActivityManager.IsInteractingWithLocation = false;
                CanInteract = true;
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole("Location Interaction" + ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "ForgerInteract");
    }
    private void Interact()
    {
        AddLicensePlateItems();
        AddIdentificationItems();
        AddMarkedBillsItems();
    }

    private void AddMarkedBillsItems()
    {
        MarkedCashSubMenu = MenuPool.AddSubMenu(InteractionMenu, "Clean Money");
        InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1].Description = "Get cash for your marked bills";
        if (HasBannerImage)
        {
            BannerImage = Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\{BannerImagePath}");
            MarkedCashSubMenu.SetBannerType(BannerImage);
        }
        AddMoneyClean();
    }
    private void AddMoneyClean()
    {
        InventoryItem cashBundleItem = Player.Inventory.ItemsList.FirstOrDefault(x => x.ModItem != null && x.ModItem.ItemSubType == ItemSubType.Money && x.ModItem.Name.ToLower() == "marked cash stack");
        if (cashBundleItem != null)
        {
            MenuItem mi = new MenuItem("Marked Cash Stack", 0, MarkedBillsSalesPrice);
            cashBundleItem.ModItem.CreateSellMenuItem(Transaction, mi, MarkedCashSubMenu, Settings, Player, false, World);
        }
    }

    private void AddIdentificationItems()
    {
        IDSubMenu = MenuPool.AddSubMenu(InteractionMenu, "Identifications");
        InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1].Description = "Buy/Sell identifications.";
        if (HasBannerImage)
        {
            BannerImage = Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\{BannerImagePath}");
            IDSubMenu.SetBannerType(BannerImage);
        }
        AddIDSale();
        AddIdentificationBuy();
    }
    private void AddIDSale()
    {
        SellIDSubMenu = MenuPool.AddSubMenu(IDSubMenu, "Sell IDs");
        string sellRightLabel = IdentificationSalesPrice.ToString("C0");
        IDSubMenu.MenuItems[IDSubMenu.MenuItems.Count() - 1].Description = "Sell IDs.";
        IDSubMenu.MenuItems[IDSubMenu.MenuItems.Count() - 1].RightLabel = sellRightLabel;
        if (HasBannerImage)
        {
            BannerImage = Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\{BannerImagePath}");
            SellIDSubMenu.SetBannerType(BannerImage);
        }

        InventoryItem driversLivenseItems = Player.Inventory.ItemsList.FirstOrDefault(x => x.ModItem != null && x.ModItem.ItemSubType == ItemSubType.Identification && x.ModItem.Name.ToLower() == "drivers license");
        if(driversLivenseItems != null)
        {
            MenuItem mi = new MenuItem("Drivers License", 0,IdentificationSalesPrice);
            driversLivenseItems.ModItem.CreateSellMenuItem(Transaction, mi, SellIDSubMenu, Settings, Player, false, World);
        }


        InventoryItem policeIDItems = Player.Inventory.ItemsList.FirstOrDefault(x => x.ModItem != null && x.ModItem.ItemSubType == ItemSubType.Identification && x.ModItem.Name.ToLower() == "police id card");
        if (policeIDItems != null)
        {
            MenuItem mi2 = new MenuItem("Police ID Card", 0, PoliceIdentificationSalesPrice);
            policeIDItems.ModItem.CreateSellMenuItem(Transaction, mi2, SellIDSubMenu, Settings, Player, false, World);
        }
    }


    private void AddIdentificationBuy()
    {
        UIMenuItem buyIDMenu = new UIMenuItem("Buy ID", "Buy a clean ID.") { RightLabel = IdentificationPurchasePrice.ToString("C0") };
        IDSubMenu.AddItem(buyIDMenu);
        if(Player.Licenses.HasValidDriversLicense(Time))
        {
            buyIDMenu.Enabled = false;
        }
        buyIDMenu.Activated += (sender, e) =>
        {
            if (Player.BankAccounts.GetMoney(false) <= IdentificationPurchasePrice)
            {
                PlayErrorSound();
                DisplayMessage("~r~Cash Only", "You do not have enough cash on hand.");
                return;
            }
            Player.BankAccounts.GiveMoney(-1 * IdentificationPurchasePrice, false);
            Player.Licenses.DriversLicense = new DriversLicense();
            Player.Licenses.DriversLicense.IssueLicense(Time, 6, StateID);
            PlaySuccessSound();
            buyIDMenu.Enabled = false;
            DisplayMessage("~g~Purchased", $"Thank you for your purchase. ID added to inventory.");
        };
    }

    private void AddLicensePlateItems()
    {
        PlateSubMenu = MenuPool.AddSubMenu(InteractionMenu, "License Plates");
        InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1].Description = "Buy/Sell a license plate.";
        if (HasBannerImage)
        {
            BannerImage = Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\{BannerImagePath}");
            PlateSubMenu.SetBannerType(BannerImage);
        }
        AddPlateSale();
        AddRandomPlateBuy();
        AddCustomPlateBuy();
    }
    private void AddPlateSale()
    {
        SellPlateSubMenu = MenuPool.AddSubMenu(PlateSubMenu, "Sell Plates");
        string sellRightLabel = WantedPlateSalesPrice.ToString("C0") + " - " + CleanPlateSalesPrice.ToString("C0");
        PlateSubMenu.MenuItems[PlateSubMenu.MenuItems.Count() - 1].Description = "Sell stolen plates.";
        PlateSubMenu.MenuItems[PlateSubMenu.MenuItems.Count() - 1].RightLabel = sellRightLabel;
        if (HasBannerImage)
        {
            BannerImage = Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\{BannerImagePath}");
            SellPlateSubMenu.SetBannerType(BannerImage);
        }
        foreach (InventoryItem lpi in Player.Inventory.ItemsList.Where(x => x.ModItem != null && x.ModItem.ItemType == ItemType.LicensePlates))
        {
            lpi.ModItem.CreateSimpleSellMenu(Player, SellPlateSubMenu, this, CleanPlateSalesPrice, WantedPlateSalesPrice, false);
        }
    }
    private void AddRandomPlateBuy()
    {
        UIMenuItem randomPlateMenuItem = new UIMenuItem("Buy Random Plate", "Buy a clean random Plate.") { RightLabel = RandomPlateCost.ToString("C0") };
        PlateSubMenu.AddItem(randomPlateMenuItem);
        randomPlateMenuItem.Activated += (sender, e) =>
        {
            if (Player.BankAccounts.GetMoney(false) <= RandomPlateCost)
            {
                PlayErrorSound();
                DisplayMessage("~r~Cash Only", "You do not have enough cash on hand.");
                return;
            }
            Player.BankAccounts.GiveMoney(-1 * RandomPlateCost, false);
            string newText = RandomItems.RandomString(8);
            LicensePlate licensePlate = new LicensePlate(newText, 1, false);
            LicensePlateItem toAdd = new LicensePlateItem($"{newText}") { Description = licensePlate.GenerateDescription(), LicensePlate = licensePlate };
            Player.Inventory.Add(toAdd, 1.0f);
            PlaySuccessSound();
            DisplayMessage("~g~Purchased", $"Thank you for your purchase. Plate added to inventory.");
        };
    }
    private void AddCustomPlateBuy()
    {
        string CustomPlateRightLabe = CustomPlateCost.ToString("C0");
        CustomPlateSubMenu = MenuPool.AddSubMenu(PlateSubMenu, "Buy Custom Plates");
        PlateSubMenu.MenuItems[PlateSubMenu.MenuItems.Count() - 1].Description = "Buy a clean custom plate.";
        PlateSubMenu.MenuItems[PlateSubMenu.MenuItems.Count() - 1].RightLabel = CustomPlateRightLabe;
        if (HasBannerImage)
        {
            BannerImage = Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\{BannerImagePath}");
            PlateSubMenu.SetBannerType(BannerImage);
        }
        string customInitial = RandomItems.RandomString(8);
        UIMenuItem customPlateTextMenuItem = new UIMenuItem("Plate Text", "Current chosen plate text. Eight chracters Max. No Symbols.") { RightLabel = customInitial };
        CustomPlateSubMenu.AddItem(customPlateTextMenuItem);
        customPlateTextMenuItem.Activated += (sender, e) =>
        {
            string newplateText = NativeHelper.GetKeyboardInput(customPlateTextMenuItem.RightLabel);
            if (string.IsNullOrEmpty(newplateText))
            {
                return;
            }
            customPlateTextMenuItem.RightLabel = newplateText.Left(8);
        };
        UIMenuListScrollerItem<PlateType> customPlateTypeMenuItem = new UIMenuListScrollerItem<PlateType>("Plate Type", "Current chosen plate type.", PlateTypes.PlateTypeManager.PlateTypeList) { };
        CustomPlateSubMenu.AddItem(customPlateTypeMenuItem);
        UIMenuItem buyCustomPlateMenuItem = new UIMenuItem("Purchase", "Buy a the customized plate.") { RightLabel = CustomPlateRightLabe };
        CustomPlateSubMenu.AddItem(buyCustomPlateMenuItem);
        buyCustomPlateMenuItem.Activated += (sender, e) =>
        {
            if (Player.BankAccounts.GetMoney(false) <= CustomPlateCost)
            {
                PlayErrorSound();
                DisplayMessage("~r~Cash Only", "You do not have enough cash on hand.");
                return;
            }
            Player.BankAccounts.GiveMoney(-1 * CustomPlateCost, false);
            LicensePlate licensePlate = new LicensePlate(customPlateTextMenuItem.RightLabel, customPlateTypeMenuItem.SelectedItem.Index, false);
            LicensePlateItem toAdd = new LicensePlateItem($"{customPlateTextMenuItem.RightLabel}") { Description = licensePlate.GenerateDescription(), LicensePlate = licensePlate };
            Player.Inventory.Add(toAdd, 1.0f);
            PlaySuccessSound();
            DisplayMessage("~g~Purchased", $"Thank you for your purchase. Plate added to inventory.");
        };
    }
    public override void AddLocation(PossibleLocations possibleLocations)
    {
        possibleLocations.Forgers.Add(this);
        base.AddLocation(possibleLocations);
    }
}

