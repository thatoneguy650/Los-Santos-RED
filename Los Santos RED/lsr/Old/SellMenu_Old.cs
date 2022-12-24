//using LosSantosRED.lsr.Helper;
//using LosSantosRED.lsr.Interface;
//using LSR.Vehicles;
//using Rage;
//using Rage.Native;
//using RAGENativeUI;
//using RAGENativeUI.Elements;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//public class SellMenu_Old : Menu
//{
//    private UIMenu sellMenuRNUI;
//    private IModItems ModItems;
//    private MenuPool MenuPool;
//    private ILocationInteractable Player;
//    private Vehicle SellingVehicle;
//    private Rage.Object SellingProp;
//    private Ped SellingPed;
//    private Camera StoreCam;
//    private bool ShouldPreviewItem;
//    //private TransactionOld Transaction;
//    private VehicleExt ToSellVehicle;
//    private IEntityProvideable World;
//    private ISettingsProvideable Settings;
//    //private TransactableLocation Store;
//    private ITimeControllable Time;
//    private IWeapons Weapons;
//    private ModItem CurrentModItem;
//    private MenuItem CurrentMenuItem;
//    private int CurrentTotalPrice;
//    private WeaponInformation CurrentWeapon;
//    private WeaponVariation CurrentWeaponVariation = new WeaponVariation();


//    private Transaction Transaction;
//    private ShopMenu ShopMenu;


//    public bool HasBannerImage { get; set; } = false;
//    public Texture BannerImage { get; set; }
//    public bool RemoveBanner { get; set; } = false;


//    public string StoreName { get; set; } = "";

//    public int MoneySpent { get; private set; } = 0;



//    public SellMenu_Old(MenuPool menuPool, UIMenu parentMenu, ShopMenu shopMenu, Transaction transaction, IModItems modItems, ILocationInteractable player, IEntityProvideable world, ISettingsProvideable settings, IWeapons weapons, ITimeControllable time, Texture bannerImage, bool hasBannerImage, bool removeBanner, string storeName)
//    {
//        ModItems = modItems;
//        Player = player;
//        World = world;
//        Settings = settings;
//        MenuPool = menuPool;


//        Transaction = transaction;
//        ShopMenu = shopMenu;
//        Time = time;
//        Weapons = weapons;
//        StoreCam = Camera.RenderingCamera;

//        BannerImage = bannerImage;
//        RemoveBanner = removeBanner;
//        StoreName = storeName;
//        HasBannerImage = hasBannerImage;


//        EntryPoint.WriteToConsole($"SellMenu: HasBannerImage {HasBannerImage} RemoveBanner {RemoveBanner}");

//        if (parentMenu != null)
//        {
//            sellMenuRNUI = menuPool.AddSubMenu(parentMenu, "Sell");
//            if (HasBannerImage)
//            {
//                sellMenuRNUI.SetBannerType(BannerImage);
//            }
//            else if (RemoveBanner)
//            {
//                sellMenuRNUI.RemoveBanner();
//            }
//            sellMenuRNUI.OnIndexChange += OnIndexChange;
//            sellMenuRNUI.OnItemSelect += OnItemSelect;
//            sellMenuRNUI.OnListChange += OnListChange;
//        }
//    }
//    public void Setup()
//    {
//        if (Settings.SettingsManager.PlayerOtherSettings.GenerateStoreItemPreviews && Transaction.PreviewItems)
//        {
//            PreloadModels();
//        }
//        Transaction.ClearPreviews();
//        CreateSellMenu();
//    }
//    public void Dispose()
//    {
//        Hide();
//        Game.RawFrameRender -= (s, e) => MenuPool.DrawBanners(e.Graphics);
//    }
//    public void Update()
//    {
//        if (MenuPool.IsAnyMenuOpen())//.Visible)
//        {
//            if (SellingProp.Exists())
//            {
//                SellingProp.SetRotationYaw(SellingProp.Rotation.Yaw + 1f);
//            }
//            if (SellingVehicle.Exists())
//            {
//                SellingVehicle.SetRotationYaw(SellingVehicle.Rotation.Yaw + 1f);
//            }
//        }
//        else
//        {
//            ClearPreviews();
//        }
//    }
//    public override void Hide()
//    {
//        ClearPreviews();
//        if (sellMenuRNUI != null)
//        {
//            sellMenuRNUI.Visible = false;
//            Player.ButtonPrompts.Clear();
//        }
//    }
//    public override void Show()
//    {
//        if (sellMenuRNUI != null)
//        {
//            if (sellMenuRNUI.CurrentSelection != -1)
//            {
//                CreatePreview(sellMenuRNUI.MenuItems[sellMenuRNUI.CurrentSelection]);
//            }
//            sellMenuRNUI.Visible = true;
//        }
//    }
//    public override void Toggle()
//    {
//        if (!sellMenuRNUI.Visible)
//        {
//            Show();
//        }
//        else
//        {
//            Hide();
//        }
//    }

//    public void OnAmountChanged(ModItem modItem)
//    {
//        if (modItem != null)
//        {
//            foreach (UIMenuItem uiMenuItem in sellMenuRNUI.MenuItems)
//            {
//                if (uiMenuItem.Text == modItem.Name && uiMenuItem.GetType() == typeof(UIMenuNumericScrollerItem<int>))
//                {
//                    MenuItem masdenuItem = ShopMenu.Items.Where(x => x.ModItemName == modItem.Name).FirstOrDefault();
//                    UpdatePropEntryData(modItem, masdenuItem, (UIMenuNumericScrollerItem<int>)uiMenuItem);
//                }
//            }
//        }
//    }


//    private void CreateSellMenu()
//    {
//        if (sellMenuRNUI != null)
//        {
//            sellMenuRNUI.Clear();
//            bool shouldCreateCategories = false;
//            if (ShopMenu.Items.Where(x => x.Sellable).Count() >= 7)
//            {
//                shouldCreateCategories = true;
//            }
//            if (shouldCreateCategories)
//            {
//                CreateCategories();
//            }
//            foreach (MenuItem cii in ShopMenu.Items)
//            {
//                if (cii != null && cii.Sellable)
//                {
//                    ModItem myItem = ModItems.Get(cii.ModItemName);
//                    if (myItem != null)
//                    {
//                        if (myItem.ModelItem?.Type == ePhysicalItemType.Vehicle)
//                        {
//                            AddVehicleEntry(cii, myItem);
//                        }
//                        else if (myItem.ModelItem?.Type == ePhysicalItemType.Weapon)
//                        {
//                            AddWeaponEntry(cii, myItem);
//                        }
//                        else
//                        {
//                            AddPropEntry(cii, myItem);
//                        }
//                    }
//                }
//            }
//        }
//        //OnIndexChange(sellMenu, sellMenu.CurrentSelection);
//    }
//    private void AddVehicleEntry(MenuItem cii, ModItem myItem)
//    {
//        VehicleItem vehicleItem = (VehicleItem)myItem;
//        if (vehicleItem != null)
//        {
//            string formattedSalesPrice = cii.SalesPrice.ToString("C0");
//            string MakeName = NativeHelper.VehicleMakeName(Game.GetHashKey(myItem.ModelItem.ModelName));
//            string ClassName = NativeHelper.VehicleClassName(Game.GetHashKey(myItem.ModelItem.ModelName));
//            string ModelName = NativeHelper.VehicleModelName(Game.GetHashKey(myItem.ModelItem.ModelName));
//            string description;
//            if (myItem.Description.Length >= 200)
//            {
//                description = myItem.Description.Substring(0, 200) + "...";//menu cant show more than 225?, need some for below
//            }
//            else
//            {
//                description = myItem.Description;
//            }
//            description += "~n~~s~";
//            if (MakeName != "")
//            {
//                description += $"~n~Manufacturer: ~b~{MakeName}~s~";
//            }
//            if (ModelName != "")
//            {
//                description += $"~n~Model: ~g~{ModelName}~s~";
//            }
//            if (ClassName != "")
//            {
//                description += $"~n~Class: ~p~{ClassName}~s~";
//            }
//            if (vehicleItem.RequiresDLC)
//            {
//                description += $"~n~~b~DLC Vehicle";
//            }
//            UIMenu VehicleMenu = null;
//            bool FoundCategoryMenu = false;
//            foreach (UIMenu uimen in MenuPool.ToList())
//            {
//                if (uimen.SubtitleText == ClassName && uimen.ParentMenu == sellMenuRNUI)
//                {
//                    FoundCategoryMenu = true;
//                    VehicleMenu = MenuPool.AddSubMenu(uimen, cii.ModItemName);
//                    uimen.MenuItems[uimen.MenuItems.Count() - 1].Description = description;
//                    uimen.MenuItems[uimen.MenuItems.Count() - 1].RightLabel = formattedSalesPrice;
//                    EntryPoint.WriteToConsole($"Added Vehicle {myItem.Name} To SubMenu {uimen.SubtitleText}", 5);
//                    break;
//                }
//            }
//            if (!FoundCategoryMenu && VehicleMenu == null)
//            {
//                VehicleMenu = MenuPool.AddSubMenu(sellMenuRNUI, cii.ModItemName);
//                sellMenuRNUI.MenuItems[sellMenuRNUI.MenuItems.Count() - 1].Description = description;
//                sellMenuRNUI.MenuItems[sellMenuRNUI.MenuItems.Count() - 1].RightLabel = formattedSalesPrice;
//                EntryPoint.WriteToConsole($"Added Vehicle {myItem.Name} To Main Buy Menu", 5);
//            }
//            if (HasBannerImage)
//            {
//                VehicleMenu.SetBannerType(BannerImage);
//            }
//            else if (RemoveBanner)
//            {
//                VehicleMenu.RemoveBanner();
//            }
//            description = myItem.Description;
//            if (description == "")
//            {
//                description = $"List Price {formattedSalesPrice}";
//            }
//            bool enabled = false;
//            if (Player.VehicleOwnership.OwnedVehicles.Any(x => x.Vehicle.Exists() && x.Vehicle.Model.Hash == Game.GetHashKey(myItem.ModelItem.ModelName)))
//            {
//                enabled = true;
//            }


//            UIMenuItem Sell = new UIMenuItem($"Sell", "Select to sell this vehicle") { RightLabel = formattedSalesPrice, Enabled = enabled };
//            VehicleMenu.AddItem(Sell);
//            VehicleMenu.OnItemSelect += OnVehicleItemSelect;
//            //VehicleMenu.OnScrollerChange += OnVehicleScrollerChange;
//        }
//    }


//    private void AddPropEntry(MenuItem cii, ModItem myItem)
//    {
//        UIMenuNumericScrollerItem<int> myScroller = new UIMenuNumericScrollerItem<int>(cii.ModItemName, "", 1, 1, 1) { Formatter = v => $"{(v == 1 && myItem.MeasurementName == "Item" ? "" : v.ToString() + " ")}{(myItem.MeasurementName != "Item" || v > 1 ? myItem.MeasurementName : "")}{(v > 1 ? "(s)" : "")}{(myItem.MeasurementName != "Item" || v > 1 ? " - " : "")}${(v * cii.SalesPrice)}", Value = 1 };
//        UpdatePropEntryData(myItem, cii, myScroller);
//        sellMenuRNUI.AddItem(myScroller);
//    }
//    private void OnVehicleItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
//    {
//        if (selectedItem.Text == "Sell" && CurrentModItem != null)
//        {
//            MenuItem menuItem = ShopMenu.Items.Where(x => x.ModItemName == CurrentModItem.Name).FirstOrDefault();
//            if (menuItem != null)
//            {
//                EntryPoint.WriteToConsole($"Vehicle Sell {menuItem.ModItemName} Player.Money {Player.BankAccounts.Money} menuItem.SalesPrice {menuItem.SalesPrice}", 5);
//                if (!SellVehicle(CurrentModItem))
//                {
//                    return;
//                }
//                Player.BankAccounts.GiveMoney(menuItem.SalesPrice);
//                MoneySpent += menuItem.SalesPrice;
//            }
//            sender.Visible = false;
//            Dispose();
//        }
//    }
//    private void UpdatePropEntryData(ModItem modItem, MenuItem menuItem, UIMenuNumericScrollerItem<int> scrollerItem)
//    {
//        if (modItem != null && menuItem != null && scrollerItem != null)
//        {
//            bool isEnabled = true;
//            InventoryItem PlayerInventoryItem = Player.Inventory.ItemsList.Where(x => x.ModItem.Name == menuItem.ModItemName).FirstOrDefault();
//            int MaxSell = 1;
//            int PlayerItems = 0;
//            if (PlayerInventoryItem != null)
//            {
//                PlayerItems = PlayerInventoryItem.Amount;
//                MaxSell = PlayerInventoryItem.Amount;
//            }
//            int RemainingToSell = MaxSell;
//            if (menuItem.NumberOfItemsToPurchaseFromPlayer != -1)
//            {
//                RemainingToSell = menuItem.NumberOfItemsToPurchaseFromPlayer - menuItem.ItemsBoughtFromPlayer;
//                if (RemainingToSell >= 1 && PlayerItems >= 1)
//                {
//                    MaxSell = Math.Min(MaxSell, RemainingToSell);
//                }
//                else
//                {
//                    MaxSell = 1;
//                    isEnabled = false;
//                }
//            }
//            else
//            {
//                if (PlayerItems <= 0)
//                {
//                    MaxSell = 1;
//                    isEnabled = false;
//                }
//            }
//            string formattedPurchasePrice = menuItem.SalesPrice.ToString("C0");
//            string description = modItem.Description;
//            description += "~n~~s~";
//            description += $"~n~Price: ~g~{formattedPurchasePrice}~s~";
//            description += $"~n~Type: ~p~{modItem.ItemType}~s~" + (modItem.ItemSubType != ItemSubType.None ? $" - ~p~{modItem.ItemSubType}~s~" : "");
//            description += $"~n~~b~{modItem.AmountPerPackage}~s~ Item(s) per Package";
//            if (modItem.AmountPerPackage > 1)
//            {
//                description += $"~n~~b~{((float)menuItem.SalesPrice / (float)modItem.AmountPerPackage).ToString("C2")} ~s~per Item";
//            }

//            description += modItem.SellMenuDescription(Settings);
//            //if (modItem.ChangesHealth && !Settings.SettingsManager.NeedsSettings.ApplyNeeds)
//            //{
//            //    description += $"~n~{modItem.HealthChangeDescription}";
//            //}
//            //if (modItem.ChangesNeeds && Settings.SettingsManager.NeedsSettings.ApplyNeeds)
//            //{
//            //    description += $"~n~{modItem.NeedChangeDescription}";
//            //}





//            description += $"~n~{RemainingToSell} {modItem.MeasurementName}(s) For Purchase~s~";
//            description += $"~n~Player Inventory: {PlayerItems}~s~ {modItem.MeasurementName}(s)";
//            scrollerItem.Maximum = MaxSell;
//            scrollerItem.Enabled = isEnabled;
//            scrollerItem.Description = description;
//            EntryPoint.WriteToConsole($"Item: {modItem.Name} formattedPurchasePrice {formattedPurchasePrice}");
//        }
//    }

//    private void CreateCategories()
//    {
//        List<WeaponCategory> WeaponCategories = new List<WeaponCategory>();
//        List<string> VehicleClasses = new List<string>();
//        int TotalWeapons = ShopMenu.Items.Where(x => x.Sellable && ModItems.Get(x.ModItemName)?.ModelItem?.Type == ePhysicalItemType.Weapon).Count();
//        int TotalVehicles = ShopMenu.Items.Where(x => x.Sellable && ModItems.Get(x.ModItemName)?.ModelItem?.Type == ePhysicalItemType.Vehicle).Count();
//        foreach (MenuItem cii in ShopMenu.Items.Where(x => x.Sellable))
//        {
//            ModItem myItem = ModItems.Get(cii.ModItemName);
//            if (myItem != null)
//            {
//                if (myItem.ModelItem?.Type == ePhysicalItemType.Weapon)
//                {
//                    if (TotalWeapons >= 7)
//                    {
//                        WeaponInformation myWeapon = Weapons.GetWeapon(myItem.ModelItem.ModelName);
//                        if (myWeapon != null)
//                        {
//                            if (!WeaponCategories.Contains(myWeapon.Category))
//                            {
//                                WeaponCategories.Add(myWeapon.Category);
//                                UIMenu WeaponMenu = MenuPool.AddSubMenu(sellMenuRNUI, myWeapon.Category.ToString());
//                                if (HasBannerImage)
//                                {
//                                    WeaponMenu.SetBannerType(BannerImage);
//                                }
//                                else if (RemoveBanner)
//                                {
//                                    WeaponMenu.RemoveBanner();
//                                }
//                                WeaponMenu.OnIndexChange += OnIndexChange;
//                                WeaponMenu.OnItemSelect += OnItemSelect;
//                                WeaponMenu.OnMenuOpen += OnMenuOpen;
//                                WeaponMenu.OnMenuClose += OnMenuClose;
//                            }
//                        }
//                    }
//                }
//                else if (myItem.ModelItem?.Type == ePhysicalItemType.Vehicle)
//                {
//                    if (TotalVehicles >= 7)
//                    {
//                        string ClassName = NativeHelper.VehicleClassName(Game.GetHashKey(myItem.ModelItem.ModelName));
//                        if (ClassName != "")
//                        {
//                            if (!VehicleClasses.Contains(ClassName))
//                            {
//                                VehicleClasses.Add(ClassName);
//                                UIMenu VehicleMenu = MenuPool.AddSubMenu(sellMenuRNUI, ClassName);
//                                if (HasBannerImage)
//                                {
//                                    VehicleMenu.SetBannerType(BannerImage);
//                                }
//                                else if (RemoveBanner)
//                                {
//                                    VehicleMenu.RemoveBanner();
//                                }
//                                VehicleMenu.OnIndexChange += OnIndexChange;
//                                VehicleMenu.OnItemSelect += OnItemSelect;
//                                VehicleMenu.OnMenuClose += OnMenuClose;
//                                VehicleMenu.OnMenuOpen += OnMenuOpen;
//                            }
//                        }
//                    }
//                }
//            }
//        }
//    }
//    private void OnMenuClose(UIMenu sender)
//    {
//        EntryPoint.WriteToConsole($"OnMenuClose {sender.SubtitleText} {sender.CurrentSelection}", 5);
//        ClearPreviews();
//    }
//    private void OnMenuOpen(UIMenu sender)
//    {
//        EntryPoint.WriteToConsole($"OnMenuOpen {sender.SubtitleText} {sender.CurrentSelection}", 5);
//        if (sender.CurrentSelection != -1)
//        {
//            CreatePreview(sender.MenuItems[sender.CurrentSelection]);
//        }

//        foreach (UIMenuItem uimen in sender.MenuItems)
//        {
//            MenuItem menuItem = ShopMenu.Items.Where(x => x.ModItemName == uimen.Text).FirstOrDefault();
//            if (menuItem != null)
//            {

//                EntryPoint.WriteToConsole($"    SELL ON MENU OPEN Sub Level: {menuItem.ModItemName} {uimen.Text}", 5);


//                ModItem currentItem = ModItems.Get(menuItem.ModItemName);
//                if (currentItem != null && currentItem.ModelItem?.Type == ePhysicalItemType.Weapon)
//                {
//                    WeaponInformation myGun = Weapons.GetWeapon(currentItem.ModelItem.ModelName);
//                    if (myGun != null)
//                    {
//                        if (NativeFunction.Natives.HAS_PED_GOT_WEAPON<bool>(Player.Character, myGun.Hash, false))
//                        {
//                            uimen.Enabled = true;
//                            EntryPoint.WriteToConsole($"    SELL ON MENU OPEN Sub Level: {myGun.ModelName} ENABLED", 5);
//                        }
//                        else
//                        {
//                            uimen.Enabled = false;
//                            EntryPoint.WriteToConsole($"    SELL ON MENU OPEN Sub Level: {myGun.ModelName} NOT ENABLED", 5);
//                        }
                        
//                    }
//                }
//            }
//        }
//    }
//    private void OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
//    {
//        ModItem ToAdd = ModItems.AllItems().Where(x => x.Name == selectedItem.Text).FirstOrDefault();
//        MenuItem menuItem = ShopMenu.Items.Where(x => x.ModItemName == selectedItem.Text).FirstOrDefault();
//        bool ExitAfterPurchase = false;
//        if (ToAdd != null && menuItem != null)
//        {
//            CurrentModItem = ToAdd;
//            CurrentMenuItem = menuItem;
//            if (ToAdd.ModelItem?.Type == ePhysicalItemType.Vehicle)//SubMenu
//            {
//                CurrentWeapon = null;
//                CurrentWeaponVariation = new WeaponVariation();

//                CreatePreview(selectedItem);
//                EntryPoint.WriteToConsole($"Purchase Menu: {CurrentModItem.Name} OnItemSelect", 5);
//            }
//            else if (ToAdd.ModelItem?.Type == ePhysicalItemType.Weapon)//SubMenu
//            {
//                CurrentWeapon = Weapons.GetWeapon(CurrentModItem.ModelItem.ModelName);
//                CurrentWeaponVariation = new WeaponVariation();
//                CreatePreview(selectedItem);
//                EntryPoint.WriteToConsole($"Purchase Menu: {CurrentModItem.Name} OnItemSelect", 5);
//            }
//            else
//            {
//                int TotalItems = 1;
//                UIMenuNumericScrollerItem<int> myItem = null;
//                if (selectedItem.GetType() == typeof(UIMenuNumericScrollerItem<int>))
//                {
//                    myItem = (UIMenuNumericScrollerItem<int>)selectedItem;
//                    TotalItems = myItem.Value;
//                }
//                CurrentWeapon = null;
//                CurrentWeaponVariation = new WeaponVariation();
//                SellItem(CurrentModItem, CurrentMenuItem, TotalItems, myItem);
//            }
//        }
//        else
//        {
//            CurrentModItem = null;
//            CurrentMenuItem = null;
//        }
//    }
//    private void OnIndexChange(UIMenu sender, int newIndex)
//    {
//        if (newIndex != -1)
//        {
//            CreatePreview(sender.MenuItems[newIndex]);
//        }
//    }
//    private void OnListChange(UIMenu sender, UIMenuListItem listItem, int newIndex)
//    {

//    }
//    private bool SellItem(ModItem modItem, MenuItem menuItem, int TotalItems, UIMenuNumericScrollerItem<int> myItem)
//    {
//        int TotalPrice = menuItem.SalesPrice * TotalItems;
//        CurrentTotalPrice = TotalPrice;
//        if (Player.Inventory.Remove(modItem, TotalItems))
//        {
//            Player.BankAccounts.GiveMoney(TotalPrice);
//            MoneySpent += TotalPrice;
//            menuItem.ItemsBoughtFromPlayer += TotalItems;
//            Transaction.OnAmountChanged(CurrentModItem);
//            Transaction.OnItemSold(modItem, menuItem, TotalItems);
//            //UpdatePropEntryData(modItem, menuItem, myItem);
//            while (Player.ActivityManager.IsPerformingActivity)
//            {
//                GameFiber.Sleep(500);
//            }
//            return true;
//        }
//        return false;
//    }

//    private void CreatePreview(UIMenuItem myItem)
//    {
//        ClearPreviews();
//        // GameFiber.Yield();
//        if (myItem != null && Transaction.PreviewItems && Settings.SettingsManager.PlayerOtherSettings.GenerateStoreItemPreviews)
//        {
//            EntryPoint.WriteToConsole($"SIMPLE TRANSACTION OnIndexChange Text: {myItem.Text}", 5);
//            ModItem itemToShow = ModItems.AllItems().Where(x => x.Name == myItem.Text).FirstOrDefault();
//            if (itemToShow != null)
//            {
//                if (itemToShow.PackageItem?.Type == ePhysicalItemType.Prop || itemToShow.ModelItem?.Type == ePhysicalItemType.Prop)
//                {
//                    PreviewProp(itemToShow);
//                }
//                else if (itemToShow.ModelItem?.Type == ePhysicalItemType.Vehicle)
//                {
//                    PreviewVehicle(itemToShow);
//                }
//                else if (itemToShow.ModelItem?.Type == ePhysicalItemType.Ped)
//                {
//                    PreviewPed(itemToShow);
//                }
//                else if (itemToShow.ModelItem?.Type == ePhysicalItemType.Weapon)
//                {
//                    PreviewWeapon(itemToShow);
//                }
//            }
//        }
//    }

//    private bool SellVehicle(ModItem modItem)
//    {
//        VehicleExt toSell = Player.VehicleOwnership.OwnedVehicles.Where(x => x.Vehicle.Exists() && x.Vehicle.Model.Hash == Game.GetHashKey(modItem.ModelItem.ModelName)).OrderBy(x => x.Vehicle.DistanceTo2D(Player.Position)).FirstOrDefault();
//        if (toSell != null)
//        {
//            Player.VehicleOwnership.RemoveOwnershipOfVehicle(toSell);
//            Transaction.OnItemSold(modItem, CurrentMenuItem, 1);
//            //Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", StoreName, "~g~Sale", "Thank you for your sale");
//            return true;
//        }
//        else
//        {
//            PlayErrorSound();
//            Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", StoreName, "~r~Sale Failed", "We are sorry, we are unable to complete this transation");
//            return false;
//        }
//    }
//    private void PreviewPed(ModItem itemToShow)
//    {
//        //GameFiber.Yield();
//    }
//    private void PreviewProp(ModItem itemToShow)
//    {
//        try
//        {
//            string ModelToSpawn = "";
//            bool useClose = true;
//            if (itemToShow.PackageItem != null)
//            {
//                ModelToSpawn = itemToShow.PackageItem.ModelName;
//                useClose = !itemToShow.PackageItem.IsLarge;
//            }
//            if (ModelToSpawn == "")
//            {
//                ModelToSpawn = itemToShow.ModelItem.ModelName;
//                useClose = !itemToShow.ModelItem.IsLarge;
//            }

//            Vector3 Position = Vector3.Zero;
//            if (StoreCam.Exists())
//            {
//                if (useClose)
//                {
//                    Position = StoreCam.Position + StoreCam.Direction;
//                }
//                else
//                {
//                    Position = StoreCam.Position + (StoreCam.Direction.ToNormalized() * 3f);
//                }
//            }
//            else
//            {
//                Vector3 GPCamPos = NativeFunction.Natives.GET_GAMEPLAY_CAM_COORD<Vector3>();
//                Vector3 GPCamDir = NativeHelper.GetGameplayCameraDirection();

//                if (useClose)
//                {
//                    Position = GPCamPos + GPCamDir / 2;
//                }
//                else
//                {
//                    Position = GPCamPos + GPCamDir.ToNormalized() * 3f;
//                }
//            }

//            if (ModelToSpawn != "" && NativeFunction.Natives.IS_MODEL_VALID<bool>(Game.GetHashKey(ModelToSpawn)))
//            {
//                try
//                {
//                    SellingProp = new Rage.Object(ModelToSpawn, Position);
//                }
//                catch (Exception ex)
//                {
//                    EntryPoint.WriteToConsole($"Error Spawning Model {ex.Message} {ex.StackTrace}");
//                }
//                //if (useClose)
//                //{
//                //    SellingProp = new Rage.Object(ModelToSpawn, StoreCam.Position + StoreCam.Direction);
//                //}
//                //else
//                //{
//                //    SellingProp = new Rage.Object(ModelToSpawn, StoreCam.Position + (StoreCam.Direction.ToNormalized() * 3f));
//                //}
//                //GameFiber.Yield();
//                if (SellingProp.Exists())
//                {
//                    SellingProp.SetRotationYaw(SellingProp.Rotation.Yaw + 45f);
//                    if (SellingProp != null && SellingProp.Exists())
//                    {
//                        NativeFunction.Natives.SET_ENTITY_HAS_GRAVITY(SellingProp, false);
//                    }
//                }
//                EntryPoint.WriteToConsole("SIMPLE TRANSACTION: PREVIEW ITEM RAN", 5);
//            }
//            else
//            {
//                if (SellingProp.Exists())
//                {
//                    SellingProp.Delete();
//                }
//            }
//        }
//        catch (Exception ex)
//        {
//            Game.DisplayNotification($"Error Displaying Model {ex.Message} {ex.StackTrace}");
//        }
//    }
//    private void PreviewVehicle(ModItem itemToShow)
//    {
//        if (itemToShow != null && itemToShow.ModelItem != null)
//        {
//            VehicleExt toSell = Player.VehicleOwnership.OwnedVehicles.Where(x => x.Vehicle.Exists() && x.Vehicle.Model.Hash == Game.GetHashKey(itemToShow.ModelItem.ModelName)).OrderBy(x => x.Vehicle.DistanceTo2D(Player.Position)).FirstOrDefault();
//            if (toSell != null && NativeFunction.Natives.IS_MODEL_VALID<bool>(Game.GetHashKey(itemToShow.ModelItem.ModelName)))
//            {
//                SellingVehicle = new Vehicle(itemToShow.ModelItem.ModelName, Transaction.ItemPreviewPosition, Transaction.ItemPreviewHeading);
//                if(SellingVehicle.Exists())
//                {
//                    SellingVehicle.PrimaryColor = toSell.Vehicle.PrimaryColor;
//                    SellingVehicle.SecondaryColor = toSell.Vehicle.SecondaryColor;
//                    SellingVehicle.LicensePlate = toSell.Vehicle.LicensePlate;
//                    SellingVehicle.LicensePlateStyle = toSell.Vehicle.LicensePlateStyle;
//                }

//            }




            
//        }
//        //GameFiber.Yield();
//        if (SellingVehicle.Exists())
//        {
//            SellingVehicle.Wash();
//            NativeFunction.Natives.SET_VEHICLE_COLOURS(SellingVehicle, 0, 0);
//            NativeFunction.Natives.SET_VEHICLE_ON_GROUND_PROPERLY<bool>(SellingVehicle, 5.0f);
//        }
//    }
//    private void PreviewWeapon(ModItem itemToShow)
//    {
//        try
//        {
//            EntryPoint.WriteToConsole($"SELL MENU PreviewWeapon RAN {itemToShow ?.Name}");
//            if (itemToShow != null && itemToShow.ModelItem != null && itemToShow.ModelItem.ModelName != "")
//            {
//                Vector3 Position = Vector3.Zero;
//                if (StoreCam.Exists())
//                {
//                    Position = StoreCam.Position + StoreCam.Direction / 2f;
//                }
//                else
//                {
//                    Vector3 GPCamPos = NativeFunction.Natives.GET_GAMEPLAY_CAM_COORD<Vector3>();
//                    Vector3 GPCamDir = NativeHelper.GetGameplayCameraDirection();
//                    Position = GPCamPos + GPCamDir / 2f;
//                }
//                if (NativeFunction.Natives.HAS_WEAPON_ASSET_LOADED<bool>(itemToShow.ModelItem.ModelHash))
//                {
//                    SellingProp = NativeFunction.Natives.CREATE_WEAPON_OBJECT<Rage.Object>(itemToShow.ModelItem.ModelHash, 60, Position.X, Position.Y, Position.Z, true, 1.0f, 0, 0, 1);
//                }
//                if (SellingProp.Exists())
//                {
//                    EntryPoint.WriteToConsole($"SELL MENU PreviewWeapon CREATED ITEM {itemToShow?.Name}");
//                    float length = SellingProp.Model.Dimensions.X;
//                    float width = SellingProp.Model.Dimensions.Y;

//                    float LargestSideLength = length;

//                    if (width > length)
//                    {
//                        LargestSideLength = width;
//                    }


//                    if (StoreCam.Exists())
//                    {
//                        Position = StoreCam.Position + (StoreCam.Direction.ToNormalized() * 0.5f) + (StoreCam.Direction.ToNormalized() * LargestSideLength / 2f);//
//                    }
//                    else
//                    {
//                        Vector3 GPCamPos = NativeFunction.Natives.GET_GAMEPLAY_CAM_COORD<Vector3>();
//                        Vector3 GPCamDir = NativeHelper.GetGameplayCameraDirection();
//                        Position = GPCamPos + (GPCamDir.ToNormalized() * 0.5f) + (GPCamDir.ToNormalized() * LargestSideLength / 2f);
//                    }
//                    SellingProp.Position = Position;
//                    SellingProp.SetRotationYaw(SellingProp.Rotation.Yaw + 45f);
//                    if (SellingProp != null && SellingProp.Exists())
//                    {
//                        NativeFunction.Natives.SET_ENTITY_HAS_GRAVITY(SellingProp, false);
//                    }
//                }
//            }
//        }
//        catch (Exception ex)
//        {
//            Game.DisplayNotification($"Error Displaying Model {ex.Message} {ex.StackTrace}");
//        }
//        //GameFiber.Yield();
//    }
//    public void ClearPreviews()
//    {
//        if (SellingProp.Exists())
//        {
//            SellingProp.Delete();
//        }
//        if (SellingVehicle.Exists())
//        {
//            SellingVehicle.Delete();
//        }
//        if (SellingPed.Exists())
//        {
//            SellingPed.Delete();
//        }
//        //EntryPoint.WriteToConsole($"Sell Menu ClearPreviews Ran", 5);
//    }
//    private void PreloadModels()
//    {
//        foreach (MenuItem menuItem in ShopMenu.Items)//preload all item models so it doesnt bog the menu down
//        {
//            try
//            {
//                if (menuItem.Sellable)
//                {
//                    ModItem myItem = ModItems.AllItems().Where(x => x.Name == menuItem.ModItemName).FirstOrDefault();
//                    if (myItem != null)
//                    {
//                        if (myItem.ModelItem != null && myItem.ModelItem.Type == ePhysicalItemType.Weapon && myItem.ModelItem.ModelName != "")
//                        {
//                            //if (NativeFunction.Natives.IS_MODEL_VALID<bool>(myItem.ModelItem.ModelHash))
//                            //{
//                                NativeFunction.Natives.REQUEST_WEAPON_ASSET(myItem.ModelItem.ModelHash, 31, 0);
//                            //}
//                        }
//                        else if (myItem.ModelItem != null && myItem.ModelItem.Type == ePhysicalItemType.Vehicle && myItem.ModelItem.ModelName != "")
//                        {
//                            if (NativeFunction.Natives.IS_MODEL_VALID<bool>(Game.GetHashKey(myItem.ModelItem.ModelName.ToLower())))
//                            {
//                                Vehicle MyVehicle = new Vehicle(myItem.ModelItem.ModelName, Vector3.Zero, 0f);
//                                if (MyVehicle.Exists())
//                                {
//                                    MyVehicle.Delete();
//                                }
//                            }
//                        }
//                        else if (myItem.PackageItem != null && myItem.PackageItem.Type == ePhysicalItemType.Prop && myItem.PackageItem.ModelName != "")
//                        {
//                            if (NativeFunction.Natives.IS_MODEL_VALID<bool>(Game.GetHashKey(myItem.PackageItem.ModelName.ToLower())))
//                            {
//                                new Model(myItem.PackageItem.ModelName).LoadAndWait();
//                            }
//                        }
//                        else if (myItem.ModelItem != null && myItem.ModelItem.Type == ePhysicalItemType.Prop && myItem.ModelItem.ModelName != "")
//                        {
//                            if (NativeFunction.Natives.IS_MODEL_VALID<bool>(Game.GetHashKey(myItem.ModelItem.ModelName.ToLower())))
//                            {
//                                new Model(myItem.ModelItem.ModelName).LoadAndWait();
//                            }
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                Game.DisplayNotification($"Error Preloading Model {ex.Message} {ex.StackTrace}");
//            }
//        }
//    }

//    private void AddWeaponEntry(MenuItem cii, ModItem myItem)
//    {
//        EntryPoint.WriteToConsole($"Sell Menu Add Weapon Entry ItemName: {myItem.Name}", 5);
//        WeaponItem weaponItem = (WeaponItem)myItem;

//        if (weaponItem != null)
//        {
//            string description;
//            if (myItem.Description.Length >= 200)
//            {
//                description = myItem.Description.Substring(0, 200) + "...";//menu cant show more than 225?, need some for below
//            }
//            else
//            {
//                description = myItem.Description;
//            }
//            description += "~n~~s~";
//            if (weaponItem.RequiresDLC)
//            {
//                description += $"~n~~b~DLC Weapon";
//            }
//            string formattedPurchasePrice = cii.SalesPrice.ToString("C0");
//            WeaponInformation myWeapon = Weapons.GetWeapon(myItem.ModelItem.ModelName);
//            bool hasPedGotWeapon = false;
//            if (NativeFunction.Natives.HAS_PED_GOT_WEAPON<bool>(Player.Character, myWeapon.Hash, false))
//            {
//                hasPedGotWeapon = true;
//            }
//            else
//            {
//                hasPedGotWeapon = false;
//            }


//            UIMenu WeaponMenu = null;
//            bool FoundCategoryMenu = false;
//            if (myWeapon != null)
//            {
//                foreach (UIMenu uimen in MenuPool.ToList())
//                {
//                    if (uimen.SubtitleText == myWeapon.Category.ToString() && uimen.ParentMenu == sellMenuRNUI)
//                    {
//                        FoundCategoryMenu = true;
//                        WeaponMenu = MenuPool.AddSubMenu(uimen, cii.ModItemName);
//                        uimen.MenuItems[uimen.MenuItems.Count() - 1].Description = description;
//                        uimen.MenuItems[uimen.MenuItems.Count() - 1].RightLabel = formattedPurchasePrice;
//                        uimen.MenuItems[uimen.MenuItems.Count() - 1].Enabled = hasPedGotWeapon;
//                        EntryPoint.WriteToConsole($"Added Weapon {myItem.Name} To SubMenu {uimen.SubtitleText}", 5);
//                        break;
//                    }
//                }
//            }
//            if (!FoundCategoryMenu && WeaponMenu == null)
//            {
//                WeaponMenu = MenuPool.AddSubMenu(sellMenuRNUI, cii.ModItemName);
//                sellMenuRNUI.MenuItems[sellMenuRNUI.MenuItems.Count() - 1].Description = description;
//                sellMenuRNUI.MenuItems[sellMenuRNUI.MenuItems.Count() - 1].RightLabel = formattedPurchasePrice;
//                sellMenuRNUI.MenuItems[sellMenuRNUI.MenuItems.Count() - 1].Enabled = hasPedGotWeapon;
//                EntryPoint.WriteToConsole($"Added Weapon {myItem.Name} To Main Buy Menu", 5);
//            }
//            //WeaponMenu.OnMenuOpen += OnWeaponMenuOpen;
//            if (HasBannerImage)
//            {
//                WeaponMenu.SetBannerType(BannerImage);
//            }
//            else if (RemoveBanner)
//            {
//                WeaponMenu.RemoveBanner();
//            }
//            UIMenuNumericScrollerItem<int> PurchaseAmmo = new UIMenuNumericScrollerItem<int>($"Sell Ammo", $"Select to sell ammo for this weapon.", cii.SubAmount, 500, cii.SubAmount) { Index = 0, Formatter = v => $"{v} - ${cii.SubPrice * v}" };
//            UIMenuItem Purchase = new UIMenuItem($"Sell", "Select to sell this Weapon") { RightLabel = formattedPurchasePrice };
//            if (hasPedGotWeapon)
//            {
//                Purchase.Enabled = true;
//            }
//            else
//            {
//                Purchase.Enabled = false;
//            }
//            if (myWeapon.Category != WeaponCategory.Melee && myWeapon.Category != WeaponCategory.Throwable)
//            {
//                WeaponMenu.AddItem(PurchaseAmmo);
//            }
//            WeaponMenu.AddItem(Purchase);
//            WeaponMenu.OnItemSelect += OnWeaponItemSelect;
//            WeaponMenu.OnMenuOpen += OnWeaponMenuOpen;
//        }
//    }
//    private void OnWeaponItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
//    {
//        if (selectedItem.Text == "Sell" && CurrentModItem != null)
//        {
//            if (CurrentMenuItem != null)
//            {
//                int TotalPrice = CurrentMenuItem.SalesPrice;
//                if (!SellWeapon())
//                {
//                    return;
//                }
//                Player.BankAccounts.GiveMoney(TotalPrice);
//                MoneySpent += TotalPrice;
//                OnWeaponMenuOpen(sender);
//            }
//        }
//        //else if (selectedItem.Text == "Purchase Ammo" && CurrentModItem != null)
//        //{
//        //    int TotalItems = 1;
//        //    if (selectedItem.GetType() == typeof(UIMenuNumericScrollerItem<int>))
//        //    {
//        //        UIMenuNumericScrollerItem<int> myItem = (UIMenuNumericScrollerItem<int>)selectedItem;
//        //        TotalItems = myItem.Value;
//        //    }
//        //    if (CurrentMenuItem != null)
//        //    {
//        //        int TotalPrice = CurrentMenuItem.SubPrice * TotalItems;
//        //        EntryPoint.WriteToConsole($"Weapon Purchase {CurrentMenuItem.ModItemName} Player.Money {Player.Money} menuItem.PurchasePrice {1}", 5);
//        //        if (Player.Money < TotalPrice)
//        //        {
//        //            Game.DisplayNotification("CHAR_BLOCKED", "CHAR_BLOCKED", Store.Name, "Insufficient Funds", "We are sorry, we are unable to complete this transation, as you do not have the required funds");
//        //            return;
//        //        }
//        //        if (!PurchaseAmmo(TotalItems))
//        //        {
//        //            return;
//        //        }
//        //        Player.GiveMoney(-1 * TotalPrice);
//        //        MoneySpent += TotalPrice;
//        //        OnWeaponMenuOpen(sender);
//        //    }
//        //}
//        //else if (selectedItem.GetType() == typeof(UIMenuListScrollerItem<MenuItemExtra>))
//        //{
//        //    UIMenuListScrollerItem<MenuItemExtra> myItem = (UIMenuListScrollerItem<MenuItemExtra>)selectedItem;
//        //    bool isComponentSlot = false;
//        //    ComponentSlot selectedSlot;
//        //    foreach (ComponentSlot cs in Enum.GetValues(typeof(ComponentSlot)).Cast<ComponentSlot>().ToList())
//        //    {
//        //        if (cs.ToString() == selectedItem.Text)
//        //        {
//        //            selectedSlot = cs;
//        //            isComponentSlot = true;
//        //            if (myItem.SelectedItem.ExtraName == "Default")
//        //            {
//        //                CurrentWeapon.SetSlotDefault(Player.Character, selectedSlot);
//        //                Game.DisplayNotification("CHAR_BLOCKED", "CHAR_BLOCKED", Store.Name, "Set Default", $"Set the {selectedSlot} slot to default");
//        //                OnWeaponMenuOpen(sender);
//        //                return;
//        //            }
//        //            break;
//        //        }
//        //    }
//        //    WeaponComponent myComponent = CurrentWeapon.PossibleComponents.Where(x => x.Name == myItem.SelectedItem.ExtraName).FirstOrDefault();
//        //    if (myComponent != null && CurrentMenuItem != null)
//        //    {
//        //        EntryPoint.WriteToConsole($"Weapon Component Purchase {CurrentMenuItem.ModItemName} Player.Money {Player.Money} menuItem.PurchasePrice {CurrentMenuItem.PurchasePrice} myComponent {myComponent.Name}", 5);
//        //        if (Player.Money < myItem.SelectedItem.PurchasePrice)
//        //        {
//        //            Game.DisplayNotification("CHAR_BLOCKED", "CHAR_BLOCKED", Store.Name, "Insufficient Funds", "We are sorry, we are unable to complete this transation, as you do not have the required funds");
//        //            return;
//        //        }
//        //        if (CurrentWeapon.HasComponent(Player.Character, myComponent))
//        //        {
//        //            Game.DisplayNotification("CHAR_BLOCKED", "CHAR_BLOCKED", Store.Name, "Already Owned", "We are sorry, we are unable to complete this transation, as the item is already owned");
//        //            return;
//        //        }
//        //        if (!PurchaseComponent(myComponent))
//        //        {
//        //            return;
//        //        }
//        //        Player.GiveMoney(-1 * myItem.SelectedItem.PurchasePrice);
//        //        MoneySpent += myItem.SelectedItem.PurchasePrice;
//        //        OnWeaponMenuOpen(sender);
//        //    }
//        //}
//    }
//    private void OnWeaponMenuOpen(UIMenu sender)
//    {
//        EntryPoint.WriteToConsole($"OnWeaponMenuOpen RAN!", 5);
//        foreach (UIMenuItem uimen in sender.MenuItems)
//        {
//            if (uimen.GetType() == typeof(UIMenuListScrollerItem<MenuItemExtra>))
//            {
//                UIMenuListScrollerItem<MenuItemExtra> myItem = (UIMenuListScrollerItem<MenuItemExtra>)(object)uimen;
//                foreach (MenuItemExtra stuff in myItem.Items)
//                {
//                    WeaponComponent myComponent = CurrentWeapon.PossibleComponents.Where(x => x.Name == stuff.ExtraName).FirstOrDefault();
//                    if (myComponent != null)
//                    {
//                        if (CurrentWeapon.HasComponent(Player.Character, myComponent))
//                        {
//                            myItem.SelectedItem = stuff;
//                            stuff.HasItem = true;
//                            EntryPoint.WriteToConsole($"OnWeaponMenuOpen RAN! {myComponent.Name} HAS COMPONENT {stuff.HasItem} {myItem.OptionText}", 5);
//                        }
//                        else
//                        {
//                            //myItem.SelectedItem = stuff;
//                            stuff.HasItem = false;
//                            EntryPoint.WriteToConsole($"OnWeaponMenuOpen RAN! {myComponent.Name} DOES NOT HAVE COMPONENT  {stuff.HasItem} {myItem.OptionText}", 5);
//                        }
//                        // myItem.Formatter = v => v.HasItem ? $"{v.ExtraName} - Equipped" : v.PurchasePrice == 0 ? v.ExtraName : $"{v.ExtraName} - ${v.PurchasePrice}";
//                    }
//                }
//                myItem.Reformat();
//            }
//            else if (uimen.Text == "Sell")
//            {
//                if (CurrentWeapon.HasWeapon(Player.Character) && CurrentWeapon.Category != WeaponCategory.Throwable)
//                {
//                    uimen.Enabled = true;
//                }
//                else
//                {
//                    uimen.Enabled = false;
//                    // uimen.RightLabel = "Owned";
//                }
//            }
//            else if (uimen.Text == "Sell Ammo")
//            {
//                if (CurrentWeapon.HasWeapon(Player.Character))
//                {
//                    uimen.Enabled = true;
//                }
//                else
//                {
//                    uimen.Enabled = false;
//                }
//            }
//            EntryPoint.WriteToConsole($"Full Below Level: {uimen.Text}", 5);
//        }
//    }
//    private bool SellWeapon()
//    {
//        if (CurrentWeapon != null)
//        {
//            if (NativeFunction.Natives.HAS_PED_GOT_WEAPON<bool>(Player.Character, CurrentWeapon.Hash, false))
//            {
//                NativeFunction.Natives.REMOVE_WEAPON_FROM_PED(Player.Character, CurrentWeapon.Hash);
//                Transaction.OnItemSold(CurrentModItem, CurrentMenuItem, 1);
//                //Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", StoreName, "~g~Sale", $"Thank you for your sale of ~r~{CurrentMenuItem.ModItemName}~s~");
//                Player.WeaponEquipment.SetUnarmed();
//                return true;
//            }
//        }
//        Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", StoreName, "~r~Sale Failed", "We are sorry, we are unable to complete this transation");
//        return false;
//    }
//    private void PlayErrorSound()
//    {
//        NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "ERROR", "HUD_LIQUOR_STORE_SOUNDSET", 0);
//    }
//}