using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

public class VehicleItem : ModItem
{
    private int PrimaryColor = 0;
    private int SecondaryColor = 0;
    private Color SellPrimaryColor = Color.Black;
    private Color SellSecondaryColor = Color.Black;
    private int Livery1 = -1;
    private UIMenu liveryFullMenu;
    private UIMenu VehicleMenu;
    private bool HasLivery1 = false;
    private bool HasLivery2 = false;

    private bool SetPrimaryColor = false;
    private bool SetSecondaryColor = false;
    private bool SetLivery1 = false;
    private bool SetLivery2 = false;

    private int FinalPrimaryColor => PrimaryColor == -1 ? 0 : PrimaryColor;
    private int FinalSecondaryColor => SecondaryColor == -1 ? 0 : SecondaryColor;
    public bool RequiresDLC { get; set; } = false;
    public string ModelName { get; set; }
    public uint ModelHash { get; set; }
    public override bool IsDLC => RequiresDLC;
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
    public override void Setup(PhysicalItems physicalItems, IWeapons weapons, IIntoxicants intoxicants)
    {
        //ModelItem = new PhysicalItem(ModelItemID, Game.GetHashKey(ModelItemID), ePhysicalItemType.Vehicle);
        ModelHash = ModelHash == 0 ? Game.GetHashKey(ModelName) : ModelHash;
        ModelItem = new PhysicalItem(ModelName, ModelHash == 0 ? Game.GetHashKey(ModelName) : ModelHash, ePhysicalItemType.Vehicle);
        MenuCategory = NativeHelper.VehicleClassName(Game.GetHashKey(ModelItem.ModelName));
    }
    public override void CreateSellMenuItem(Transaction Transaction, MenuItem menuItem, UIMenu sellMenuRNUI, ISettingsProvideable settings, ILocationInteractable player, bool isStealing, IEntityProvideable world)
    {
        PrimaryColor = -1;
        SecondaryColor = -1;
        SellPrimaryColor = Color.Black;
        SellSecondaryColor = Color.Black;
        string formattedSalesPrice = menuItem.SalesPrice.ToString("C0");
        string description = GetGeneralDescription(menuItem);
        bool enabled = false;
        VehicleExt ownedVersion = player.VehicleOwnership.OwnedVehicles.FirstOrDefault(x => x.Vehicle.Exists() && x.Vehicle.Model.Hash == Game.GetHashKey(ModelItem.ModelName));
        if (ownedVersion != null)
        {
            SellPrimaryColor = ownedVersion.Vehicle.PrimaryColor;
            SellSecondaryColor = ownedVersion.Vehicle.SecondaryColor;
            enabled = true;
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
            CategoryMenu.MenuItems[CategoryMenu.MenuItems.Count() - 1].Enabled = enabled;
            //EntryPoint.WriteToConsole($"Added Vehicle {Name} To SubMenu {CategoryMenu.SubtitleText}");
        }
        if (!FoundCategoryMenu && VehicleMenu == null)
        {
            VehicleMenu = Transaction.MenuPool.AddSubMenu(sellMenuRNUI, menuItem.ModItemName);
            sellMenuRNUI.MenuItems[sellMenuRNUI.MenuItems.Count() - 1].Description = description;
            sellMenuRNUI.MenuItems[sellMenuRNUI.MenuItems.Count() - 1].RightLabel = formattedSalesPrice;
            sellMenuRNUI.MenuItems[sellMenuRNUI.MenuItems.Count() - 1].Enabled = enabled;
            //EntryPoint.WriteToConsole($"Added Vehicle {Name} To Main Buy Menu");
        }
        if (Transaction.HasBannerImage)
        {
            VehicleMenu.SetBannerType(Transaction.BannerImage);
        }
        else if (Transaction.RemoveBanner)
        {
            VehicleMenu.RemoveBanner();
        }
        //description = Description;
        if (description == "")
        {
            description = $"List Price {formattedSalesPrice}";
        }
        UIMenuItem Sell = new UIMenuItem($"Sell", "Select to sell this vehicle") { RightLabel = formattedSalesPrice, Enabled = enabled };
        Sell.Activated += (sender, selectedItem) =>
        {
            Transaction.IsShowingConfirmDialog = true;
            sender.Visible = false;
            SimpleWarning popUpWarning = new SimpleWarning("Sell", $"Are you sure you want to sell this vehicle for ${menuItem.SalesPrice}", "", player.ButtonPrompts, settings);
            popUpWarning.Show();
            if (!popUpWarning.IsAccepted)
            {
                Transaction.IsShowingConfirmDialog = false;
                sender.Visible = true;
                return;
            }
            if (SellVehicle(Transaction, menuItem, player, settings, world))
            {
                player.BankAccounts.GiveMoney(menuItem.SalesPrice, Transaction.UseAccounts);
                Transaction.MoneySpent += menuItem.SalesPrice;
                sender.Visible = false;
            }
            Transaction.IsShowingConfirmDialog = false;
        };
        VehicleMenu.AddItem(Sell);
    }
    private bool SellVehicle(Transaction transaction, MenuItem CurrentMenuItem, ILocationInteractable player, ISettingsProvideable settings, IEntityProvideable world)
    {
        VehicleExt toSell = player.VehicleOwnership.OwnedVehicles.Where(x => x.Vehicle.Exists() && x.Vehicle.Model.Hash == Game.GetHashKey(ModelItem.ModelName)).OrderBy(x => x.Vehicle.DistanceTo2D(player.Position)).FirstOrDefault();
        if (toSell != null)
        {
            player.VehicleOwnership.RemoveOwnershipOfVehicle(toSell);
            if(toSell.Vehicle.Exists())
            {
                toSell.Vehicle.Delete();
            }
            transaction.OnItemSold(this, CurrentMenuItem, 1);
            return true;
        }
        else
        {
            transaction.PlayErrorSound();
            transaction.DisplayMessage("~r~Sale Failed", "We are sorry, we are unable to complete this transation");
            return false;
        }
    }
    public override void CreatePurchaseMenuItem(Transaction Transaction, MenuItem menuItem, UIMenu purchaseMenu, ISettingsProvideable settings, ILocationInteractable player, bool isStealing, IEntityProvideable world)
    {
        PrimaryColor = -1;
        SecondaryColor = -1;
        SetPrimaryColor = false;
        SetSecondaryColor = false;
        SetLivery1 = false;
        SetLivery2 = false;
        if(RequiresDLC && !settings.SettingsManager.PlayerOtherSettings.AllowDLCVehicles)
        {
            return;
        }

        string formattedPurchasePrice = menuItem.PurchasePrice.ToString("C0");
        if (menuItem.PurchasePrice == 0)
        {
            formattedPurchasePrice = "";
        }

        string description = GetGeneralDescription(menuItem);

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
            //EntryPoint.WriteToConsole($"Added Vehicle {Name} To SubMenu {CategoryMenu.SubtitleText}");
        }
        if (!FoundCategoryMenu && VehicleMenu == null)
        {
            VehicleMenu = Transaction.MenuPool.AddSubMenu(purchaseMenu, menuItem.ModItemName);
            purchaseMenu.MenuItems[purchaseMenu.MenuItems.Count() - 1].Description = description;
            purchaseMenu.MenuItems[purchaseMenu.MenuItems.Count() - 1].RightLabel = formattedPurchasePrice;
            //EntryPoint.WriteToConsole($"Added Vehicle {Name} To Main Buy Menu");
        }
        if (Transaction.HasBannerImage)
        {
            VehicleMenu.SetBannerType(Transaction.BannerImage);
        }
        else if (Transaction.RemoveBanner)
        {
            VehicleMenu.RemoveBanner();
        }


        //Purchase Stuff Here

        string PurchaseHeader = "Purchase";
        string PurchaseDescription = "Select to purchase this vehicle";

        if(!Transaction.IsPurchasing)
        {
            PurchaseHeader = "Acquire";
            PurchaseDescription = "Select to take possession of this vehicle";
        }





        UIMenuCheckboxItem StopRotation = new UIMenuCheckboxItem("Rotate Preview", Transaction.RotateVehiclePreview) { Description = "Toggle rotation of the preview" };
        StopRotation.CheckboxEvent += (sender, Checked) =>
        {
            Transaction.RotateVehiclePreview = StopRotation.Checked;
        };
        VehicleMenu.AddItem(StopRotation);



        //UIMenuItem RotateCamera = new UIMenuItem("Rotate Camera", "Rotate the camera around");
        //RotateCamera.Activated += (sender, selectedItem) =>
        //{
        //    //Transaction.RotatePreview = StopRotation.Checked;


        //    Transaction.LocationCamera?.RotateCameraByMouse();

        //};
        //VehicleMenu.AddItem(RotateCamera);








        VehicleMenu.OnMenuOpen += (sender) =>
        {
            StopRotation.Checked = Transaction.RotateVehiclePreview;
        };


        UIMenuItem Purchase = new UIMenuItem(PurchaseHeader, PurchaseDescription) { RightLabel = formattedPurchasePrice };
        Purchase.Activated += (sender, selectedItem) =>
        {
            if(menuItem == null)
            {
                return;
            }
            //EntryPoint.WriteToConsole($"Vehicle Purchase {menuItem.ModItemName} Player.Money {player.BankAccounts.Money} menuItem.PurchasePrice {menuItem.PurchasePrice}");
            if (player.BankAccounts.GetMoney(Transaction.UseAccounts) < menuItem.PurchasePrice)
            {
                Transaction.DisplayInsufficientFundsMessage();
                return;
            }
            Transaction.IsShowingConfirmDialog = true;
            sender.Visible = false;

            string PurchaseWarningHeader = "Purchase";
            string PurchaseWarningDescription = $"Are you sure you want to purchase this vehicle for ${menuItem.PurchasePrice}";
            if (!Transaction.IsPurchasing)
            {
                PurchaseWarningHeader = "Acquire";
                PurchaseWarningDescription = $"Are you sure you want to take possession of this vehicle";
            }
            SimpleWarning popUpWarning = new SimpleWarning(PurchaseWarningHeader, PurchaseWarningDescription, "", player.ButtonPrompts, settings);
            popUpWarning.Show();
            if (!popUpWarning.IsAccepted)
            {
                sender.Visible = true;
                Transaction.IsShowingConfirmDialog = false;
                return;
            }
            Transaction.IsShowingConfirmDialog = false;
            if (!PurchaseVehicle(Transaction, menuItem, player, settings, world))
            {
                return;
            }
            player.BankAccounts.GiveMoney(-1 * menuItem.PurchasePrice, Transaction.UseAccounts);
            Transaction.MoneySpent += menuItem.PurchasePrice;   
           // sender.Visible = false;
        };
        VehicleMenu.AddItem(Purchase);

        //Color Stuff Here
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
                    SetPrimaryColor = true;
                    PrimaryColor = cl.ColorID;
                    if (Transaction.SellingVehicle.Exists())
                    {
                        NativeFunction.Natives.SET_VEHICLE_COLOURS(Transaction.SellingVehicle, FinalPrimaryColor, FinalSecondaryColor);
                    }
                };
                primarycolorGroupMenu.AddItem(actualColorPrimary);
                UIMenuItem actualColorSecondary = new UIMenuItem(cl.ColorName, cl.FullColorName);
                actualColorSecondary.RightBadge = UIMenuItem.BadgeStyle.Heart;
                actualColorSecondary.RightBadgeInfo.Color = cl.RGBColor;
                actualColorSecondary.Activated += (sender, selectedItem) =>
                {
                    SetSecondaryColor = true;
                    SecondaryColor = cl.ColorID;
                    if (Transaction.SellingVehicle.Exists())
                    {
                        NativeFunction.Natives.SET_VEHICLE_COLOURS(Transaction.SellingVehicle, FinalPrimaryColor, FinalSecondaryColor);
                    }
                };
                secondarycolorGroupMenu.AddItem(actualColorSecondary);
            }
        }
        liveryFullMenu = null;     
    }



    private bool PurchaseVehicle(Transaction transaction, MenuItem CurrentMenuItem, ILocationInteractable player, ISettingsProvideable settings, IEntityProvideable world)
    {
        bool ItemInDeliveryBay = true;
        SpawnPlace ChosenSpawn = null;
        foreach (SpawnPlace sp in transaction.VehicleDeliveryLocations.OrderBy(x => RandomItems.GetRandomNumber(0f, 1f)))
        {
            ItemInDeliveryBay = false;// Rage.World.GetEntities(sp.Position, 7f, GetEntitiesFlags.ConsiderAllVehicles).Any();
            if (!ItemInDeliveryBay)
            {
                ChosenSpawn = sp;
                break;
            }
        }
        if (!ItemInDeliveryBay && ChosenSpawn != null)
        {
            world.Vehicles.CleanupAmbient();
            Vehicle NewVehicle = new Vehicle(ModelItem.ModelName, ChosenSpawn.Position, ChosenSpawn.Heading);
            if (NewVehicle.Exists())
            {
                //CurrentMenuItem.ItemsSoldToPlayer += 1;
                if(SetPrimaryColor || SetSecondaryColor || !SetLivery1)
                {
                    NativeFunction.Natives.SET_VEHICLE_COLOURS(NewVehicle, FinalPrimaryColor, FinalSecondaryColor);
                }
                if(SetLivery1 && Livery1 != -1)
                {
                    NativeFunction.Natives.SET_VEHICLE_LIVERY(NewVehicle, Livery1);
                }
                NewVehicle.Wash();
                //NewVehicle.LicensePlate = new PlateType(0, "", "San Andreas", 0, "12ABC345").GenerateNewLicensePlateNumber();
                VehicleExt MyNewCar = world.Vehicles.GetVehicleExt(NewVehicle);
                if (MyNewCar == null)
                {
                    MyNewCar = new VehicleExt(NewVehicle, settings);
                    MyNewCar.Setup();
                    MyNewCar.HasUpdatedPlateType = false;
                    MyNewCar.AllowVanityPlates = false;
                    MyNewCar.CanHaveRandomItems = false;
                    //EntryPoint.WriteToConsoleTestLong("New Vehicle Created in PurchaseVehicle");
                }
                MyNewCar.AddVehicleToList(world);
                //world.Vehicles.AddEntity(MyNewCar, ResponseType.None);
                player.VehicleOwnership.TakeOwnershipOfVehicle(MyNewCar, false);

                transaction.OnItemPurchased(this, CurrentMenuItem, 1);
                return true;
            }
            else
            {
                transaction.PlayErrorSound();
                transaction.DisplayMessage("~r~Delivery Failed", "We are sorry, we are unable to complete this delivery");
                return false;
            }
        }
        else
        {
            transaction.PlayErrorSound();
            transaction.DisplayMessage("~o~Blocked Delivery", "We are sorry, we are unable to complete this delivery, the bay is blocked");
            return false;
        }
    }
    public override void CreatePreview(Transaction Transaction, Camera StoreCam, bool isPurchase, IEntityProvideable world, ISettingsProvideable settings)
    {
        if (ModelItem != null && Transaction.VehiclePreviewPosition != null && Transaction.VehiclePreviewPosition.Position != Vector3.Zero && NativeFunction.Natives.IS_MODEL_VALID<bool>(Game.GetHashKey(ModelItem.ModelName)))
        {
            NativeFunction.Natives.CLEAR_AREA(Transaction.VehiclePreviewPosition.Position.X, Transaction.VehiclePreviewPosition.Position.Y, Transaction.VehiclePreviewPosition.Position.Z, 4f, true, false, false, false);
            Transaction.SellingVehicle = new Vehicle(ModelItem.ModelName, Transaction.VehiclePreviewPosition.Position, Transaction.VehiclePreviewPosition.Heading);
        }
        if (!Transaction.SellingVehicle.Exists())
        {
            return;
        }
        VehicleExt Car = new VehicleExt(Transaction.SellingVehicle, settings);
        Car.Setup();

        if(Car.IsBoat)
        {
            Transaction.RotatePreview = false;
        }
        Car.WasModSpawned = true;
        Car.WasSpawnedEmpty = true;
        Car.CanHaveRandomItems = false;
        Car.AddVehicleToList(world);
        //world.Vehicles.AddEntity(Car, ResponseType.None);
        Transaction.SellingVehicle.Wash();
        CreateLiveryMenuOne(Transaction);
        if (!HasLivery1)
        {
            if (isPurchase)
            {
                NativeFunction.Natives.SET_VEHICLE_COLOURS(Transaction.SellingVehicle, FinalPrimaryColor, FinalSecondaryColor);
            }
            else
            {
                Transaction.SellingVehicle.PrimaryColor = SellPrimaryColor;
                Transaction.SellingVehicle.SecondaryColor = SellSecondaryColor;
                //NativeFunction.Natives.SET_VEHICLE_COLOURS(Transaction.SellingVehicle, SellPrimaryColor, SellSecondaryColor);
            }
        }
        NativeFunction.Natives.SET_VEHICLE_ON_GROUND_PROPERLY<bool>(Transaction.SellingVehicle, 5.0f);
        Car.ForcePlateType(Transaction?.LicensePlatePreviewable?.LicensePlatePreviewText, 0);
    }
    private void CreateLiveryMenuOne(Transaction Transaction)
    {
        if(liveryFullMenu != null)
        {
            return;
        }
        int Livery1Count = NativeFunction.Natives.GET_VEHICLE_LIVERY_COUNT<int>(Transaction.SellingVehicle);
        if(Livery1Count == -1)
        {
            HasLivery1 = false;
            return;
        }
        HasLivery1 = true;

        if(VehicleMenu == null)
        {
            return;
        }

        liveryFullMenu = Transaction.MenuPool.AddSubMenu(VehicleMenu, "Liveries");
        liveryFullMenu.SubtitleText = "LIVERIES";
        VehicleMenu.MenuItems[VehicleMenu.MenuItems.Count() - 1].Description = "Pick Livery";

        if (Transaction.HasBannerImage) { liveryFullMenu.SetBannerType(Transaction.BannerImage); }
        for (int i = -1; i <= Livery1Count - 1; i++)
        {
            int selectedLivery = i;
            string MenuName = GetLiveryName(Transaction.SellingVehicle, i);
            UIMenuItemExt liveryOneMenu = new UIMenuItemExt(MenuName);// { RightLabel = i.ToString() };
            liveryOneMenu.Activated += (sender, selectedItem) =>
            {
                //int value = i;
                //if (!int.TryParse(liveryOneMenu.RightLabel, out int value))
                //{
                //    return;
                //}
                if (selectedLivery == -1)
                {
                    SetLivery1 = false;
                    Livery1 = -1;
                }
                else
                {
                    SetLivery1 = true;
                    Livery1 = selectedLivery;
                    //EntryPoint.WriteToConsoleTestLong($"Livery 1 Activated {Livery1}");
                    if (Transaction.SellingVehicle.Exists())
                    {
                        NativeFunction.Natives.SET_VEHICLE_LIVERY(Transaction.SellingVehicle, Livery1);
                    }
                }
            };
            liveryFullMenu.AddItem(liveryOneMenu);
        }
    }
    private string GetLiveryName(Vehicle vehicle, int Index)
    {
        if(Index == -1)
        {
            return "Default";
        }
        string LiveryName;
        unsafe
        {
            IntPtr ptr = NativeFunction.CallByName<IntPtr>("GET_LIVERY_NAME", vehicle, Index);
            LiveryName = Marshal.PtrToStringAnsi(ptr);
        }
        unsafe
        {
            IntPtr ptr2 = NativeFunction.CallByHash<IntPtr>(0x7B5280EBA9840C72, LiveryName);
            LiveryName = Marshal.PtrToStringAnsi(ptr2);
        }
        if(LiveryName == "NULL")
        {
            return $"Livery #{Index}";
        }
        return LiveryName;
    }
    private string GetGeneralDescription(MenuItem menuItem)
    {
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
        return description;
    }
}

