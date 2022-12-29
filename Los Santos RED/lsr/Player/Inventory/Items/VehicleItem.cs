using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;


public class VehicleItem : ModItem
{
    private int PrimaryColor = 0;
    private int SecondaryColor = 0;
    private Color SellPrimaryColor = Color.Black;
    private Color SellSecondaryColor = Color.Black;

    public bool RequiresDLC { get; set; } = false;
    public VehicleItem()
    {
    }

    public VehicleItem(string name, string description, ItemType itemType) : base(name, description, itemType)
    {

    }
    public VehicleItem(string name, ItemType itemType) : base(name, itemType)
    {

    }
    public VehicleItem(string name, bool requiresDLC, ItemType itemType) : base(name, itemType)
    {
        RequiresDLC = requiresDLC;
    }
    public VehicleItem(string name, string description, bool requiresDLC, ItemType itemType) : base(name, description, itemType)
    {
        RequiresDLC = requiresDLC;
    }
    public override void Setup(PhysicalItems physicalItems, IWeapons weapons)
    {
        ModelItem = physicalItems.Get(ModelItemID);
        if (ModelItem == null)
        {
            ModelItem = new PhysicalItem(ModelItemID, Game.GetHashKey(ModelItemID), ePhysicalItemType.Vehicle);
        }
        MenuCategory = NativeHelper.VehicleClassName(Game.GetHashKey(ModelItem.ModelName));
    }
    public override void CreateSellMenuItem(Transaction Transaction, MenuItem menuItem, UIMenu sellMenuRNUI, ISettingsProvideable settings, ILocationInteractable player, bool isStealing, IEntityProvideable world)
    {
        PrimaryColor = 0;
        SecondaryColor = 0;
        SellPrimaryColor = Color.Black;
        SellSecondaryColor = Color.Black;
        string formattedSalesPrice = menuItem.SalesPrice.ToString("C0");
        string MakeName = NativeHelper.VehicleMakeName(Game.GetHashKey(ModelItem.ModelName));
        string ClassName = NativeHelper.VehicleClassName(Game.GetHashKey(ModelItem.ModelName));
        string ModelName = NativeHelper.VehicleModelName(Game.GetHashKey(ModelItem.ModelName));
        string description;
        if (Description.Length >= 200)
        {
            description = Description.Substring(0, 200) + "...";//menu cant show more than 225?, need some for below
        }
        else
        {
            description = Description;
        }
        description += "~n~~s~";
        if (MakeName != "")
        {
            description += $"~n~Manufacturer: ~b~{MakeName}~s~";
        }
        if (ModelName != "")
        {
            description += $"~n~Model: ~g~{ModelName}~s~";
        }
        if (ClassName != "")
        {
            description += $"~n~Class: ~p~{ClassName}~s~";
        }
        if (RequiresDLC)
        {
            description += $"~n~~b~DLC Vehicle";
        }
        UIMenu VehicleMenu = null;
        bool FoundCategoryMenu = false;


        UIMenu VehicleSubMenu = sellMenuRNUI.Children.Where(x => x.Value.SubtitleText.ToLower() == "vehicles").FirstOrDefault().Value;
        UIMenu ToCheckFirst = sellMenuRNUI;
        if (VehicleSubMenu != null)
        {
            ToCheckFirst = VehicleSubMenu;
        }
        UIMenu CategoryMenu = ToCheckFirst.Children.Where(x => x.Value.SubtitleText == MenuCategory).FirstOrDefault().Value;
        if(CategoryMenu != null)
        {
            FoundCategoryMenu = true;
            VehicleMenu = Transaction.MenuPool.AddSubMenu(CategoryMenu, menuItem.ModItemName);
            CategoryMenu.MenuItems[CategoryMenu.MenuItems.Count() - 1].Description = description;
            CategoryMenu.MenuItems[CategoryMenu.MenuItems.Count() - 1].RightLabel = formattedSalesPrice;
            EntryPoint.WriteToConsole($"Added Vehicle {Name} To SubMenu {CategoryMenu.SubtitleText}", 5);
        }

        //foreach (UIMenu uimen in Transaction.MenuPool.ToList())
        //{
        //    if (uimen.SubtitleText == ClassName && uimen.ParentMenu == sellMenuRNUI)
        //    {
        //        FoundCategoryMenu = true;
        //        VehicleMenu = Transaction.MenuPool.AddSubMenu(uimen, menuItem.ModItemName);
        //        uimen.MenuItems[uimen.MenuItems.Count() - 1].Description = description;
        //        uimen.MenuItems[uimen.MenuItems.Count() - 1].RightLabel = formattedSalesPrice;
        //        EntryPoint.WriteToConsole($"Added Vehicle {Name} To SubMenu {uimen.SubtitleText}", 5);
        //        break;
        //    }
        //}






        if (!FoundCategoryMenu && VehicleMenu == null)
        {
            VehicleMenu = Transaction.MenuPool.AddSubMenu(sellMenuRNUI, menuItem.ModItemName);
            sellMenuRNUI.MenuItems[sellMenuRNUI.MenuItems.Count() - 1].Description = description;
            sellMenuRNUI.MenuItems[sellMenuRNUI.MenuItems.Count() - 1].RightLabel = formattedSalesPrice;
            EntryPoint.WriteToConsole($"Added Vehicle {Name} To Main Buy Menu", 5);
        }
        if (Transaction.HasBannerImage)
        {
            VehicleMenu.SetBannerType(Transaction.BannerImage);
        }
        else if (Transaction.RemoveBanner)
        {
            VehicleMenu.RemoveBanner();
        }
        description = Description;
        if (description == "")
        {
            description = $"List Price {formattedSalesPrice}";
        }
        bool enabled = false;
        VehicleExt ownedVersion = player.VehicleOwnership.OwnedVehicles.FirstOrDefault(x => x.Vehicle.Exists() && x.Vehicle.Model.Hash == Game.GetHashKey(ModelItem.ModelName));

        if (ownedVersion != null)
        {
            SellPrimaryColor = ownedVersion.Vehicle.PrimaryColor;
            SellSecondaryColor = ownedVersion.Vehicle.SecondaryColor;
            enabled = true;
        }
        UIMenuItem Sell = new UIMenuItem($"Sell", "Select to sell this vehicle") { RightLabel = formattedSalesPrice, Enabled = enabled };
        Sell.Activated += (sender, selectedItem) =>
        {
            if(SellVehicle(Transaction, menuItem, player, settings, world))
            {
                player.BankAccounts.GiveMoney(menuItem.SalesPrice);
                Transaction.MoneySpent += menuItem.SalesPrice;
                sender.Visible = false;
            }
        };
        VehicleMenu.AddItem(Sell);
    }
    private bool SellVehicle(Transaction transaction, MenuItem CurrentMenuItem, ILocationInteractable player, ISettingsProvideable settings, IEntityProvideable world)
    {
        VehicleExt toSell = player.VehicleOwnership.OwnedVehicles.Where(x => x.Vehicle.Exists() && x.Vehicle.Model.Hash == Game.GetHashKey(ModelItem.ModelName)).OrderBy(x => x.Vehicle.DistanceTo2D(player.Position)).FirstOrDefault();
        if (toSell != null)
        {
            player.VehicleOwnership.RemoveOwnershipOfVehicle(toSell);
            transaction.OnItemSold(this, CurrentMenuItem, 1);
            return true;
        }
        else
        {
            transaction.PlayErrorSound();
            Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", transaction.Store?.Name, "~r~Sale Failed", "We are sorry, we are unable to complete this transation");
            return false;
        }
    }
    public override void CreatePurchaseMenuItem(Transaction Transaction, MenuItem menuItem, UIMenu purchaseMenu, ISettingsProvideable settings, ILocationInteractable player, bool isStealing, IEntityProvideable world)
    {
        PrimaryColor = 0;
        SecondaryColor = 0;
        if(RequiresDLC && !settings.SettingsManager.PlayerOtherSettings.AllowDLCVehiclesInStores)
        {
            return;
        }

        string formattedPurchasePrice = menuItem.PurchasePrice.ToString("C0");
        if (menuItem.PurchasePrice == 0)
        {
            formattedPurchasePrice = "FREE";
        }

        string MakeName = NativeHelper.VehicleMakeName(Game.GetHashKey(ModelItem.ModelName));
        string ClassName = NativeHelper.VehicleClassName(Game.GetHashKey(ModelItem.ModelName));
        string ModelName = NativeHelper.VehicleModelName(Game.GetHashKey(ModelItem.ModelName));


        string description;
        if (Description.Length >= 200)
        {
            description = Description.Substring(0, 200) + "...";//menu cant show more than 225?, need some for below
        }
        else
        {
            description = Description;
        }

        description += "~n~~s~";
        if (MakeName != "")
        {
            description += $"~n~Manufacturer: ~b~{MakeName}~s~";
        }
        if (ModelName != "")
        {
            description += $"~n~Model: ~g~{ModelName}~s~";
        }
        if (ClassName != "")
        {
            description += $"~n~Class: ~p~{ClassName}~s~";
        }
        if (RequiresDLC)
        {
            description += $"~n~~b~DLC Vehicle";
        }


        UIMenu VehicleMenu = null;
        bool FoundCategoryMenu = false;

        UIMenu VehicleSubMenu = purchaseMenu.Children.Where(x => x.Value.SubtitleText.ToLower() == "vehicles").FirstOrDefault().Value;
        UIMenu ToCheckFirst = purchaseMenu;
        if (VehicleSubMenu != null)
        {
            ToCheckFirst = VehicleSubMenu;
        }
        UIMenu CategoryMenu = ToCheckFirst.Children.Where(x => x.Value.SubtitleText == MenuCategory).FirstOrDefault().Value;
        if (CategoryMenu != null)
        {
            FoundCategoryMenu = true;
            VehicleMenu = Transaction.MenuPool.AddSubMenu(CategoryMenu, menuItem.ModItemName);
            CategoryMenu.MenuItems[CategoryMenu.MenuItems.Count() - 1].Description = description;
            CategoryMenu.MenuItems[CategoryMenu.MenuItems.Count() - 1].RightLabel = formattedPurchasePrice;
            EntryPoint.WriteToConsole($"Added Vehicle {Name} To SubMenu {CategoryMenu.SubtitleText}", 5);
        }


        //foreach (UIMenu uimen in Transaction.MenuPool.ToList())
        //{
        //    if (uimen.SubtitleText == ClassName)
        //    {
        //        FoundCategoryMenu = true;
        //        VehicleMenu = Transaction.MenuPool.AddSubMenu(uimen, menuItem.ModItemName);
        //        uimen.MenuItems[uimen.MenuItems.Count() - 1].Description = description;
        //        uimen.MenuItems[uimen.MenuItems.Count() - 1].RightLabel = formattedPurchasePrice;
        //        EntryPoint.WriteToConsole($"Added Vehicle {Name} To SubMenu {uimen.SubtitleText}", 5);
        //        break;
        //    }
        //}





        if (!FoundCategoryMenu && VehicleMenu == null)
        {
            VehicleMenu = Transaction.MenuPool.AddSubMenu(purchaseMenu, menuItem.ModItemName);
            purchaseMenu.MenuItems[purchaseMenu.MenuItems.Count() - 1].Description = description;
            purchaseMenu.MenuItems[purchaseMenu.MenuItems.Count() - 1].RightLabel = formattedPurchasePrice;
            EntryPoint.WriteToConsole($"Added Vehicle {Name} To Main Buy Menu", 5);
        }

        if (Transaction.HasBannerImage)
        {
            VehicleMenu.SetBannerType(Transaction.BannerImage);
        }
        else if (Transaction.RemoveBanner)
        {
            VehicleMenu.RemoveBanner();
        }

        UIMenu colorFullMenu = Transaction.MenuPool.AddSubMenu(VehicleMenu, "Colors");
        colorFullMenu.SubtitleText = "COLORS";
        VehicleMenu.MenuItems[VehicleMenu.MenuItems.Count() - 1].Description = "Pick Colors";
        if (Transaction.HasBannerImage) { colorFullMenu.SetBannerType(Transaction.BannerImage); }

        UIMenu primaryColorMenu = Transaction.MenuPool.AddSubMenu(colorFullMenu, "Primary Color");
        primaryColorMenu.SubtitleText = "PRIMARY COLOR GROUPS";
        colorFullMenu.MenuItems[colorFullMenu.MenuItems.Count() - 1].Description = "Pick Primary Colors";
        if (Transaction.HasBannerImage) { primaryColorMenu.SetBannerType(Transaction.BannerImage); }

        UIMenu secondaryColorMenu = Transaction.MenuPool.AddSubMenu(colorFullMenu, "Secondary Color");
        secondaryColorMenu.SubtitleText = "SECONDARY COLOR GROUPS";
        colorFullMenu.MenuItems[colorFullMenu.MenuItems.Count() - 1].Description = "Pick Secondary Colors";
        if (Transaction.HasBannerImage) { secondaryColorMenu.SetBannerType(Transaction.BannerImage); }


        //Add Color Sub Menu Here
        foreach (string colorGroupString in Transaction.VehicleColors.GroupBy(x => x.ColorGroup).Select(x => x.Key).Distinct().OrderBy(x => x))
        {
            UIMenu primarycolorGroupMenu = Transaction.MenuPool.AddSubMenu(primaryColorMenu, colorGroupString);
            primarycolorGroupMenu.SubtitleText = "PRIMARY COLORS";
            primaryColorMenu.MenuItems[primaryColorMenu.MenuItems.Count() - 1].Description = "Choose a color group";
            if (Transaction.HasBannerImage) { primarycolorGroupMenu.SetBannerType(Transaction.BannerImage); }

            UIMenu secondarycolorGroupMenu = Transaction.MenuPool.AddSubMenu(secondaryColorMenu, colorGroupString);
            secondarycolorGroupMenu.SubtitleText = "SECONDARY COLORS";
            secondaryColorMenu.MenuItems[secondaryColorMenu.MenuItems.Count() - 1].Description = "Choose a color group";
            if (Transaction.HasBannerImage) { secondarycolorGroupMenu.SetBannerType(Transaction.BannerImage); }

            foreach (VehicleColorLookup cl in Transaction.VehicleColors.Where(x => x.ColorGroup == colorGroupString))
            {
                UIMenuItem actualColorPrimary = new UIMenuItem(cl.ColorName, cl.FullColorName);
                actualColorPrimary.RightBadge = UIMenuItem.BadgeStyle.Heart;
                actualColorPrimary.RightBadgeInfo.Color = cl.RGBColor;
                actualColorPrimary.Activated += (sender, selectedItem) =>
                {
                    PrimaryColor = cl.ColorID;
                    if (Transaction.SellingVehicle.Exists())
                    {
                        NativeFunction.Natives.SET_VEHICLE_COLOURS(Transaction.SellingVehicle, PrimaryColor, SecondaryColor);
                    }
                };
                primarycolorGroupMenu.AddItem(actualColorPrimary);

                UIMenuItem actualColorSeconmdary = new UIMenuItem(cl.ColorName, cl.FullColorName);

                actualColorSeconmdary.RightBadge = UIMenuItem.BadgeStyle.Heart;
                actualColorSeconmdary.RightBadgeInfo.Color = cl.RGBColor;

                actualColorSeconmdary.Activated += (sender, selectedItem) =>
                {
                    SecondaryColor = cl.ColorID;
                    if (Transaction.SellingVehicle.Exists())
                    {
                        NativeFunction.Natives.SET_VEHICLE_COLOURS(Transaction.SellingVehicle, PrimaryColor, SecondaryColor);
                    }
                };
                secondarycolorGroupMenu.AddItem(actualColorSeconmdary);
            }
        }

        //Purchase Stuff Here
        UIMenuItem Purchase = new UIMenuItem($"Purchase", "Select to purchase this vehicle") { RightLabel = formattedPurchasePrice };
        Purchase.Activated += (sender, selectedItem) =>
        {
            if (menuItem != null)
            {
                EntryPoint.WriteToConsole($"Vehicle Purchase {menuItem.ModItemName} Player.Money {player.BankAccounts.Money} menuItem.PurchasePrice {menuItem.PurchasePrice}", 5);
                if (player.BankAccounts.Money < menuItem.PurchasePrice)
                {
                    Transaction.DisplayInsufficientFundsMessage();
                    return;
                }
                if (!PurchaseVehicle(Transaction, menuItem, player,settings, world))
                {
                    return;
                }
                player.BankAccounts.GiveMoney(-1 * menuItem.PurchasePrice);
                Transaction.MoneySpent += menuItem.PurchasePrice;
            }
            sender.Visible = false;
            //Dispose();
        };
        VehicleMenu.AddItem(Purchase);
        
    }
    private bool PurchaseVehicle(Transaction transaction, MenuItem CurrentMenuItem, ILocationInteractable player, ISettingsProvideable settings, IEntityProvideable world)
    {
        bool ItemInDeliveryBay = true;
        SpawnPlace ChosenSpawn = null;
        foreach (SpawnPlace sp in transaction.ItemDeliveryLocations.OrderBy(x => RandomItems.GetRandomNumber(0f, 1f)))
        {
            ItemInDeliveryBay = Rage.World.GetEntities(sp.Position, 7f, GetEntitiesFlags.ConsiderAllVehicles).Any();
            if (!ItemInDeliveryBay)
            {
                ChosenSpawn = sp;
                break;
            }
        }
        if (!ItemInDeliveryBay && ChosenSpawn != null)
        {
            Vehicle NewVehicle = new Vehicle(ModelItem.ModelName, ChosenSpawn.Position, ChosenSpawn.Heading);
            if (NewVehicle.Exists())
            {  
                CurrentMenuItem.ItemsSoldToPlayer += 1;
                NativeFunction.Natives.SET_VEHICLE_COLOURS(NewVehicle, PrimaryColor, SecondaryColor);
                NewVehicle.Wash();
                VehicleExt MyNewCar = world.Vehicles.GetVehicleExt(NewVehicle);
                if (MyNewCar == null)
                {
                    MyNewCar = new VehicleExt(NewVehicle, settings);
                    MyNewCar.Setup();
                    EntryPoint.WriteToConsole("New Vehicle Created in PurchaseVehicle");
                }
                world.Vehicles.AddEntity(MyNewCar, ResponseType.None);
                player.VehicleOwnership.TakeOwnershipOfVehicle(MyNewCar, false);
                transaction.OnItemPurchased(this, CurrentMenuItem, 1);
                return true;
            }
            else
            {
                transaction.PlayErrorSound();
                Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", transaction.Store?.Name, "~r~Delivery Failed", "We are sorry, we are unable to complete this transation");
                return false;
            }
        }
        else
        {
            transaction.PlayErrorSound();
            Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", transaction.Store?.Name, "~o~Blocked Delivery", "We are sorry, we are unable to complete this transation, the delivery bay is blocked");
            return false;
        }
    }
    public override void CreatePreview(Transaction Transaction, Camera StoreCam, bool isPurchase)
    {
        if (ModelItem != null && NativeFunction.Natives.IS_MODEL_VALID<bool>(Game.GetHashKey(ModelItem.ModelName)))
        {
            NativeFunction.Natives.CLEAR_AREA(Transaction.ItemPreviewPosition.X, Transaction.ItemPreviewPosition.Y, Transaction.ItemPreviewPosition.Z, 4f, true, false, false, false);
            Transaction.SellingVehicle = new Vehicle(ModelItem.ModelName, Transaction.ItemPreviewPosition, Transaction.ItemPreviewHeading);
        }
        if (Transaction.SellingVehicle.Exists())
        {
            Transaction.SellingVehicle.Wash();
            if (isPurchase)
            {
                NativeFunction.Natives.SET_VEHICLE_COLOURS(Transaction.SellingVehicle, PrimaryColor, SecondaryColor);
            }
            else
            {
                Transaction.SellingVehicle.PrimaryColor = SellPrimaryColor;
                Transaction.SellingVehicle.SecondaryColor = SellSecondaryColor;
                //NativeFunction.Natives.SET_VEHICLE_COLOURS(Transaction.SellingVehicle, SellPrimaryColor, SellSecondaryColor);
            }
            NativeFunction.Natives.SET_VEHICLE_ON_GROUND_PROPERLY<bool>(Transaction.SellingVehicle, 5.0f);
        }
    }
}

