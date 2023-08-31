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
using System.Windows.Forms;
using System.Xml.Serialization;

public class Forger : GameLocation
{
    private UIMenu PlateSubMenu;
    private UIMenu CustomPlateSubMenu;
    private UIMenu SellPlateSubMenu;

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
    public override bool CanCurrentlyInteract(ILocationInteractable player)
    {
        ButtonPromptText = $"Enter {Name}";
        return true;
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
        if (!CanInteract)
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
                StoreCamera.Setup();
                CreateInteractionMenu();
                InteractionMenu.Visible = true;
                Interact();
                ProcessInteractionMenu();
                DisposeInteractionMenu();
                StoreCamera.Dispose();
                Player.IsTransacting = false;
                Player.ActivityManager.IsInteractingWithLocation = false;
                CanInteract = true;
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole("Location Interaction" + ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "BarInteract");
    }
    private void Interact()
    {
        AddLicensePlateItems();
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
            lpi.ModItem.CreateSimpleSellMenu(Player, SellPlateSubMenu, this, CleanPlateSalesPrice, WantedPlateSalesPrice);
        }
    }
    private void AddRandomPlateBuy()
    {
        UIMenuItem randomPlateMenuItem = new UIMenuItem("Buy Random Plate", "Buy a clean random Plate.") { RightLabel = RandomPlateCost.ToString("C0") };
        PlateSubMenu.AddItem(randomPlateMenuItem);
        randomPlateMenuItem.Activated += (sender, e) =>
        {
            if (Player.BankAccounts.Money <= RandomPlateCost)
            {
                PlayErrorSound();
                DisplayMessage("~r~Insufficient Funds", "We are sorry, we are unable to complete this transation.");
                return;
            }
            Player.BankAccounts.GiveMoney(-1 * RandomPlateCost);
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
            if (Player.BankAccounts.Money <= CustomPlateCost)
            {
                PlayErrorSound();
                DisplayMessage("~r~Insufficient Funds", "We are sorry, we are unable to complete this transation.");
                return;
            }
            Player.BankAccounts.GiveMoney(-1 * CustomPlateCost);
            LicensePlate licensePlate = new LicensePlate(customPlateTextMenuItem.RightLabel, customPlateTypeMenuItem.SelectedItem.Index, false);
            LicensePlateItem toAdd = new LicensePlateItem($"{customPlateTextMenuItem.RightLabel}") { Description = licensePlate.GenerateDescription(), LicensePlate = licensePlate };
            Player.Inventory.Add(toAdd, 1.0f);
            PlaySuccessSound();
            DisplayMessage("~g~Purchased", $"Thank you for your purchase. Plate added to inventory.");
        };
    }
}

