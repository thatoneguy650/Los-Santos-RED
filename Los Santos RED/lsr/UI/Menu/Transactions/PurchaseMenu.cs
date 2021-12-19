using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;

public class PurchaseMenu : Menu
{
    private UIMenu purchaseMenu;
    private IModItems ModItems;
    private GameLocation Store;
    private MenuPool MenuPool;
    private IInteractionable Player;
    private int ItemsBought;
    private Vehicle SellingVehicle;
    private Rage.Object SellingProp;
    private Ped SellingPed;
    private Camera StoreCam;
    private bool ShouldPreviewItem;
    private PedExt Ped;
    private bool IsActivelyConversing;
    private IEntityProvideable World;
    private ISettingsProvideable Settings;
    private List<ColorLookup> ColorList;
    private int PrimaryColor = 0;
    private int SecondaryColor = 0;
    private ModItem CurrentItem;
    private string PlateString = "";

    public bool Visible => purchaseMenu.Visible;
    private bool CanContinueConversation => Ped != null &&Ped.Pedestrian.Exists() && Player.Character.DistanceTo2D(Ped.Pedestrian) <= 6f && Ped.CanConverse && Player.CanConverse;
    public bool BoughtItem => ItemsBought > 0;
    public PurchaseMenu(MenuPool menuPool, UIMenu parentMenu, PedExt ped, GameLocation store, IModItems modItems, IInteractionable player, Camera storeCamera, bool shouldPreviewItem, IEntityProvideable world, ISettingsProvideable settings)
    {
        Ped = ped;
        ModItems = modItems;
        Store = store;
        Player = player;
        StoreCam = storeCamera;
        ShouldPreviewItem = shouldPreviewItem;
        World = world;
        Settings = settings;
        MenuPool = menuPool;
        purchaseMenu = MenuPool.AddSubMenu(parentMenu, "Buy");
        if (Store.BannerImage != "")
        {
            purchaseMenu.SetBannerType(Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\{Store.BannerImage}"));
            Game.RawFrameRender += (s, e) => MenuPool.DrawBanners(e.Graphics);
        }
        if (Store == null || Store.Name == "")
        {
            purchaseMenu.RemoveBanner();
        }
        purchaseMenu.OnIndexChange += OnIndexChange;
        purchaseMenu.OnItemSelect += OnItemSelect;
        purchaseMenu.OnListChange += OnListChange;
    }
    public void Setup()
    {
        PreloadModels();
        if (Ped != null)
        {
            AnimationDictionary.RequestAnimationDictionay("mp_safehousevagos@");
            AnimationDictionary.RequestAnimationDictionay("mp_common");
        }
        ColorList = new List<ColorLookup>()
        {
        new ColorLookup(0,"Metallic Black"),
        new ColorLookup(1,"Metallic Graphite Black"),
        new ColorLookup(2,"Metallic Black Steal"),
        new ColorLookup(3,"Metallic Dark Silver"),
        new ColorLookup(4,"Metallic Silver"),
        new ColorLookup(5,"Metallic Blue Silver"),
        new ColorLookup(6,"Metallic Steel Gray"),
        new ColorLookup(7,"Metallic Shadow Silver"),
        new ColorLookup(8,"Metallic Stone Silver"),
        new ColorLookup(9,"Metallic Midnight Silver"),
        new ColorLookup(10,"Metallic Gun Metal"),
        new ColorLookup(11,"Metallic Anthracite Grey"),
        new ColorLookup(12,"Matte Black"),
        new ColorLookup(13,"Matte Gray"),
        new ColorLookup(14,"Matte Light Grey"),
        new ColorLookup(15,"Util Black"),
        new ColorLookup(16,"Util Black Poly"),
        new ColorLookup(17,"Util Dark silver"),
        new ColorLookup(18,"Util Silver"),
        new ColorLookup(19,"Util Gun Metal"),
        new ColorLookup(20,"Util Shadow Silver"),
        new ColorLookup(21,"Worn Black"),
        new ColorLookup(22,"Worn Graphite"),
        new ColorLookup(23,"Worn Silver Grey"),
        new ColorLookup(24,"Worn Silver"),
        new ColorLookup(25,"Worn Blue Silver"),
        new ColorLookup(26,"Worn Shadow Silver"),
        new ColorLookup(27,"Metallic Red"),
        new ColorLookup(28,"Metallic Torino Red"),
        new ColorLookup(29,"Metallic Formula Red"),
        new ColorLookup(30,"Metallic Blaze Red"),
        new ColorLookup(31,"Metallic Graceful Red"),
        new ColorLookup(32,"Metallic Garnet Red"),
        new ColorLookup(33,"Metallic Desert Red"),
        new ColorLookup(34,"Metallic Cabernet Red"),
        new ColorLookup(35,"Metallic Candy Red"),
        new ColorLookup(36,"Metallic Sunrise Orange"),
        new ColorLookup(37,"Metallic Classic Gold"),
        new ColorLookup(38,"Metallic Orange"),
        new ColorLookup(39,"Matte Red"),
        new ColorLookup(40,"Matte Dark Red"),
        new ColorLookup(41,"Matte Orange"),
        new ColorLookup(42,"Matte Yellow"),
        new ColorLookup(43,"Util Red"),
        new ColorLookup(44,"Util Bright Red"),
        new ColorLookup(45,"Util Garnet Red"),
        new ColorLookup(46,"Worn Red"),
        new ColorLookup(47,"Worn Golden Red"),
        new ColorLookup(48,"Worn Dark Red"),
        new ColorLookup(49,"Metallic Dark Green"),
        new ColorLookup(50,"Metallic Racing Green"),
        new ColorLookup(51,"Metallic Sea Green"),
        new ColorLookup(52,"Metallic Olive Green"),
        new ColorLookup(53,"Metallic Green"),
        new ColorLookup(54,"Metallic Gasoline Blue Green"),
        new ColorLookup(55,"Matte Lime Green"),
        new ColorLookup(56,"Util Dark Green"),
        new ColorLookup(57,"Util Green"),
        new ColorLookup(58,"Worn Dark Green"),
        new ColorLookup(59,"Worn Green"),
        new ColorLookup(60,"Worn Sea Wash"),
        new ColorLookup(61,"Metallic Midnight Blue"),
        new ColorLookup(62,"Metallic Dark Blue"),
        new ColorLookup(63,"Metallic Saxony Blue"),
        new ColorLookup(64,"Metallic Blue"),
        new ColorLookup(65,"Metallic Mariner Blue"),
        new ColorLookup(66,"Metallic Harbor Blue"),
        new ColorLookup(67,"Metallic Diamond Blue"),
        new ColorLookup(68,"Metallic Surf Blue"),
        new ColorLookup(69,"Metallic Nautical Blue"),
        new ColorLookup(70,"Metallic Bright Blue"),
        new ColorLookup(71,"Metallic Purple Blue"),
        new ColorLookup(72,"Metallic Spinnaker Blue"),
        new ColorLookup(73,"Metallic Ultra Blue"),
        new ColorLookup(74,"Metallic Bright Blue"),
        new ColorLookup(75,"Util Dark Blue"),
        new ColorLookup(76,"Util Midnight Blue"),
        new ColorLookup(77,"Util Blue"),
        new ColorLookup(78,"Util Sea Foam Blue"),
        new ColorLookup(79,"Uil Lightning blue"),
        new ColorLookup(80,"Util Maui Blue Poly"),
        new ColorLookup(81,"Util Bright Blue"),
        new ColorLookup(82,"Matte Dark Blue"),
        new ColorLookup(83,"Matte Blue"),
        new ColorLookup(84,"Matte Midnight Blue"),
        new ColorLookup(85,"Worn Dark blue"),
        new ColorLookup(86,"Worn Blue"),
        new ColorLookup(87,"Worn Light blue"),
        new ColorLookup(88,"Metallic Taxi Yellow"),
        new ColorLookup(89,"Metallic Race Yellow"),
        new ColorLookup(90,"Metallic Bronze"),
        new ColorLookup(91,"Metallic Yellow Bird"),
        new ColorLookup(92,"Metallic Lime"),
        new ColorLookup(93,"Metallic Champagne"),
        new ColorLookup(94,"Metallic Pueblo Beige"),
        new ColorLookup(95,"Metallic Dark Ivory"),
        new ColorLookup(96,"Metallic Choco Brown"),
        new ColorLookup(97,"Metallic Golden Brown"),
        new ColorLookup(98,"Metallic Light Brown"),
        new ColorLookup(99,"Metallic Straw Beige"),
        new ColorLookup(100,"Metallic Moss Brown"),
        new ColorLookup(101,"Metallic Biston Brown"),
        new ColorLookup(102,"Metallic Beechwood"),
        new ColorLookup(103,"Metallic Dark Beechwood"),
        new ColorLookup(104,"Metallic Choco Orange"),
        new ColorLookup(105,"Metallic Beach Sand"),
        new ColorLookup(106,"Metallic Sun Bleeched Sand"),
        new ColorLookup(107,"Metallic Cream"),
        new ColorLookup(108,"Util Brown"),
        new ColorLookup(109,"Util Medium Brown"),
        new ColorLookup(110,"Util Light Brown"),
        new ColorLookup(111,"Metallic White"),
        new ColorLookup(112,"Metallic Frost White"),
        new ColorLookup(113,"Worn Honey Beige"),
        new ColorLookup(114,"Worn Brown"),
        new ColorLookup(115,"Worn Dark Brown"),
        new ColorLookup(116,"Worn straw beige"),
        new ColorLookup(117,"Brushed Steel"),
        new ColorLookup(118,"Brushed Black steel"),
        new ColorLookup(119,"Brushed Aluminium"),
        new ColorLookup(120,"Chrome"),
        new ColorLookup(121,"Worn Off White"),
        new ColorLookup(122,"Util Off White"),
        new ColorLookup(123,"Worn Orange"),
        new ColorLookup(124,"Worn Light Orange"),
        new ColorLookup(125,"Metallic Securicor Green"),
        new ColorLookup(126,"Worn Taxi Yellow"),
        new ColorLookup(127,"police car blue"),
        new ColorLookup(128,"Matte Green"),
        new ColorLookup(129,"Matte Brown"),
        new ColorLookup(130,"Worn Orange"),
        new ColorLookup(131,"Matte White"),
        new ColorLookup(132,"Worn White"),
        new ColorLookup(133,"Worn Olive Army Green"),
        new ColorLookup(134,"Pure White"),
        new ColorLookup(135,"Hot Pink"),
        new ColorLookup(136,"Salmon pink"),
        new ColorLookup(137,"Metallic Vermillion Pink"),
        new ColorLookup(138,"Orange"),
        new ColorLookup(139,"Green"),
        new ColorLookup(140,"Blue"),
        new ColorLookup(141,"Mettalic Black Blue"),
        new ColorLookup(142,"Metallic Black Purple"),
        new ColorLookup(143,"Metallic Black Red"),
        new ColorLookup(144,"hunter green"),
        new ColorLookup(145,"Metallic Purple"),
        new ColorLookup(146,"Metaillic V Dark Blue"),
        new ColorLookup(147,"MODSHOP BLACK1"),
        new ColorLookup(148,"Matte Purple"),
        new ColorLookup(149,"Matte Dark Purple"),
        new ColorLookup(150,"Metallic Lava Red"),
        new ColorLookup(151,"Matte Forest Green"),
        new ColorLookup(152,"Matte Olive Drab"),
        new ColorLookup(153,"Matte Desert Brown"),
        new ColorLookup(154,"Matte Desert Tan"),
        new ColorLookup(155,"Matte Foilage Green"),
        new ColorLookup(156,"DEFAULT ALLOY COLOR"),
        new ColorLookup(157,"Epsilon Blue"),
        new ColorLookup(158,"MP100 GOLD"),
        new ColorLookup(159,"MP100 GOLD SATIN"),
        new ColorLookup(160,"MP100 GOLD SPEC"),
        };
    }
    public void Dispose()
    {
        Hide();
        Game.RawFrameRender -= (s, e) => MenuPool.DrawBanners(e.Graphics);
    }
    public void Update()
    {
        if (MenuPool.IsAnyMenuOpen())
        {
            if (SellingProp.Exists())
            {
                SellingProp.SetRotationYaw(SellingProp.Rotation.Yaw + 1f);
            }
            if (SellingVehicle.Exists())
            {
                SellingVehicle.SetRotationYaw(SellingVehicle.Rotation.Yaw + 1f);
            }
        }
        else
        {
            ClearPreviews();
        }
    }
    public override void Hide()
    {
        ClearPreviews();     
        purchaseMenu.Visible = false;
        Player.ButtonPrompts.Clear();
        //EntryPoint.WriteToConsole($"Purchase Menu: {CurrentItem.Name} HIDE RAN", 5);
    }
    public override void Show()
    {
        CreatePurchaseMenu();
        purchaseMenu.Visible = true;
    }
    public override void Toggle()
    {
        if (!purchaseMenu.Visible)
        {
            CreatePurchaseMenu();
            purchaseMenu.Visible = true;
        }
        else
        {
            Hide();
            purchaseMenu.Visible = false;
        }
    }
    private void CreatePurchaseMenu()
    {
        purchaseMenu.Clear();
        foreach (MenuItem cii in Store.Menu)
        {
            if (cii != null && cii.Purchaseable)
            {
                ModItem myItem = ModItems.Get(cii.ModItemName);
                if (myItem != null)
                {
                    string formattedPurchasePrice = cii.PurchasePrice.ToString("C0");
                    if (myItem.ModelItem?.Type == ePhysicalItemType.Vehicle)
                    {
                        UIMenu VehicleMenu = MenuPool.AddSubMenu(purchaseMenu, cii.ModItemName);
                        VehicleMenu.SetBannerType(System.Drawing.Color.FromArgb(181, 48, 48));
                        if (Store?.BannerImage != "")
                        {
                            VehicleMenu.SetBannerType(Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\{Store.BannerImage}"));
                            Game.RawFrameRender += (s, e) => MenuPool.DrawBanners(e.Graphics);
                        }
                        if (Store == null || Store.Name == "")
                        {
                            VehicleMenu.RemoveBanner();
                        }
                        UIMenuItem SetPlate = new UIMenuItem($"Set Plate", $"Change License Plate");

                        UIMenuListScrollerItem<ColorLookup> ColorMenu = new UIMenuListScrollerItem<ColorLookup>("Color", "Select Color", ColorList);

                        UIMenuListScrollerItem<ColorLookup> PrimaryColorMenu = new UIMenuListScrollerItem<ColorLookup>("Primary Color", "Select Primary Color", ColorList);
                        UIMenuListScrollerItem<ColorLookup> SecondaryColorMenu = new UIMenuListScrollerItem<ColorLookup>("Secondary Color", "Select Secondary Color", ColorList);

                        UIMenuItem Purchase = new UIMenuItem($"Purchase", $"List Price: {formattedPurchasePrice}");
                        //VehicleMenu.AddItem(SetPlate);
                        VehicleMenu.AddItem(ColorMenu);
                        VehicleMenu.AddItem(PrimaryColorMenu);
                        VehicleMenu.AddItem(SecondaryColorMenu);
                        VehicleMenu.AddItem(Purchase);
                        VehicleMenu.OnItemSelect += OnVehicleItemSelect;
                        VehicleMenu.OnScrollerChange += OnVehicleScrollerChange;
                        VehicleMenu.OnIndexChange += OnVehicleIndexChange;
                        //purchaseMenu.AddItem(new UIMenuItem(cii.ModItemName, $"{cii.ModItemName} {formattedPurchasePrice}"));
                    }
                    else
                    {
                        purchaseMenu.AddItem(new UIMenuItem(cii.ModItemName, $"{cii.ModItemName} {formattedPurchasePrice}"));
                    }
                }
            }
        }
        OnIndexChange(purchaseMenu, purchaseMenu.CurrentSelection);
    }
    private void OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        ModItem ToAdd = ModItems.Items.Where(x => x.Name == selectedItem.Text).FirstOrDefault();
        MenuItem menuItem = Store.Menu.Where(x => x.ModItemName == selectedItem.Text).FirstOrDefault();
        bool ExitAfterPurchase = false;
        if (ToAdd != null && menuItem != null)
        {
            if (ToAdd.ModelItem?.Type == ePhysicalItemType.Vehicle)
            {
                CurrentItem = ToAdd;
                EntryPoint.WriteToConsole($"Purchase Menu: {CurrentItem.Name} OnItemSelect", 5);
                return;
            }
            if (Player.Money >= menuItem.PurchasePrice)
            {
                bool subtractCash = true;

                if (Ped != null && Ped.Pedestrian.Exists())
                {
                    StartBuyAnimation(ToAdd, menuItem.IsIllicilt);
                }
                else
                {
                    Hide();
                }
                ItemsBought++;
                if (ToAdd.Type == eConsumableType.Service)
                {
                    Player.StartServiceActivity(ToAdd, Store);
                }
                else //if (ToAdd.CanConsume)
                {
                    Player.AddToInventory(ToAdd, ToAdd.AmountPerPackage);
                    EntryPoint.WriteToConsole($"ADDED {ToAdd.Name} {ToAdd.GetType()}  Amount: {ToAdd.AmountPerPackage}", 5);
                }
                if (subtractCash)
                {
                    Player.GiveMoney(-1 * menuItem.PurchasePrice);
                }
            }
        }
        GameFiber.Sleep(500);
        while (Player.IsPerformingActivity)
        {
            GameFiber.Sleep(500);
        }
        if (ExitAfterPurchase)
        {
            Dispose();
        }
        else
        {
            Show();
        }
    }
    private void OnIndexChange(UIMenu sender, int newIndex)
    {
        if (newIndex != -1)
        {
            CreatePreview(sender.MenuItems[newIndex]);
        }
    }
    private void OnListChange(UIMenu sender, UIMenuListItem listItem, int newIndex)
    {

    }
    private void OnVehicleItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (selectedItem.Text == "Purchase" && CurrentItem != null)
        {
            if (!PurchaseVehicle(CurrentItem))
            {
                Game.DisplayNotification("CHAR_BLOCKED", "CHAR_BLOCKED", Store.Name, "Could Not Deliver", "We are sorry we are unable to complete this transation");
                return;
            }
            MenuItem menuItem = Store.Menu.Where(x => x.ModItemName == CurrentItem.Name).FirstOrDefault();
            if (menuItem != null)//otherwise its free i guess?
            {
                Player.GiveMoney(-1 * menuItem.PurchasePrice);
            }
            sender.Visible = false;
            Dispose();
        }
        if (selectedItem.Text == "Set Plate" && CurrentItem != null)
        {
            //PlateString = NativeHelper.GetKeyboardInput("");
            //if (SellingVehicle.Exists() && PlateString != "")
            //{
            //    SellingVehicle.LicensePlate = PlateString.Substring(0,8);
            //}
        }
    }
    private void OnVehicleIndexChange(UIMenu sender, int newIndex)
    {

    }
    private void OnVehicleScrollerChange(UIMenu sender, UIMenuScrollerItem item, int oldIndex, int newIndex)
    {
        if (item.Text == "Primary Color")
        {
            PrimaryColor = newIndex;
            if (SellingVehicle.Exists())
            {
                NativeFunction.Natives.SET_VEHICLE_COLOURS(SellingVehicle,PrimaryColor, SecondaryColor);
            }
        }
        else if (item.Text == "Secondary Color")
        {
            SecondaryColor = newIndex;
            if (SellingVehicle.Exists())
            {
                NativeFunction.Natives.SET_VEHICLE_COLOURS(SellingVehicle,PrimaryColor, SecondaryColor);
            }
        }
        else if (item.Text == "Color")
        {
            PrimaryColor = newIndex;
            SecondaryColor = newIndex;
            if (SellingVehicle.Exists())
            {
                NativeFunction.Natives.SET_VEHICLE_COLOURS(SellingVehicle, PrimaryColor, SecondaryColor);
            }
        }

    }
    private void CreatePreview(UIMenuItem myItem)
    {
        ClearPreviews();
       // GameFiber.Yield();
        if (myItem != null)
        {
            EntryPoint.WriteToConsole($"SIMPLE TRANSACTION OnIndexChange Text: {myItem.Text}", 5);
            ModItem itemToShow = ModItems.Items.Where(x => x.Name == myItem.Text).FirstOrDefault();
            if (itemToShow != null && ShouldPreviewItem)
            {
                if (itemToShow.PackageItem?.Type == ePhysicalItemType.Prop || itemToShow.ModelItem?.Type == ePhysicalItemType.Prop)
                {
                    PreviewProp(itemToShow);
                }
                else if (itemToShow.ModelItem?.Type == ePhysicalItemType.Vehicle)
                {
                    PreviewVehicle(itemToShow);
                }
                else if (itemToShow.ModelItem?.Type == ePhysicalItemType.Ped)
                {
                    PreviewPed(itemToShow);
                }
            }
        }
    }
    private void PreviewPed(ModItem itemToShow)
    {
        //if (itemToShow != null && itemToShow.ModelItem != null)
        //{
        //    SellingVehicle = new Vehicle(itemToShow.ModelItem.ModelName, Store.ItemPreviewPosition, Store.ItemPreviewHeading);
        //}
        GameFiber.Yield();
    }
    private void PreviewVehicle(ModItem itemToShow)
    {
        if (itemToShow != null && itemToShow.ModelItem != null)
        {
            SellingVehicle = new Vehicle(itemToShow.ModelItem.ModelName, Store.ItemPreviewPosition, Store.ItemPreviewHeading);
        }
       // GameFiber.Yield();
        if (SellingVehicle.Exists())
        {
            SellingVehicle.Wash();
            NativeFunction.Natives.SET_VEHICLE_COLOURS(SellingVehicle, PrimaryColor, SecondaryColor);
            NativeFunction.Natives.SET_VEHICLE_ON_GROUND_PROPERLY<bool>(SellingVehicle, 5.0f);
            //if (PlateString != "")
            //{
            //    SellingVehicle.LicensePlate = PlateString.Substring(0, 8);
            //}
        }
    }
    private void PreviewProp(ModItem itemToShow)
    {
        string ModelToSpawn = "";
        bool useClose = true;
        if (itemToShow.PackageItem != null)
        {
            ModelToSpawn = itemToShow.PackageItem.ModelName;
            useClose = !itemToShow.PackageItem.IsLarge;
        }
        if (ModelToSpawn == "")
        {
            ModelToSpawn = itemToShow.ModelItem.ModelName;
            useClose = !itemToShow.ModelItem.IsLarge;
        }
        if (ModelToSpawn != "")
        {
            if (useClose)
            {
                SellingProp = new Rage.Object(ModelToSpawn, StoreCam.Position + StoreCam.Direction);
            }
            else
            {
                SellingProp = new Rage.Object(ModelToSpawn, StoreCam.Position + (StoreCam.Direction.ToNormalized() * 3f));
            }
            if (SellingProp.Exists())
            {
                SellingProp.SetRotationYaw(SellingProp.Rotation.Yaw + 45f);
                if (SellingProp != null && SellingProp.Exists())
                {
                    NativeFunction.Natives.SET_ENTITY_HAS_GRAVITY(SellingProp, false);
                }
            }
            EntryPoint.WriteToConsole("SIMPLE TRANSACTION: PREVIEW ITEM RAN", 5);
        }
        else
        {
            if (SellingProp.Exists())
            {
                SellingProp.Delete();
            }
        }
    }
    private void ClearPreviews()
    {
        if (SellingProp.Exists())
        {
            SellingProp.Delete();
        }
        if (SellingVehicle.Exists())
        {
            SellingVehicle.Delete();
        }
        if (SellingPed.Exists())
        {
            SellingPed.Delete();
        }
    }
    private void PreloadModels()
    {
        foreach (MenuItem menuItem in Store.Menu)//preload all item models so it doesnt bog the menu down
        {
            if (menuItem.Purchaseable)
            {
                ModItem myItem = ModItems.Items.Where(x => x.Name == menuItem.ModItemName).FirstOrDefault();
                if (myItem != null)
                {
                    if (myItem.PackageItem != null && myItem.PackageItem.Type != ePhysicalItemType.Vehicle && myItem.PackageItem.ModelName != "")
                    {
                        new Model(myItem.PackageItem.ModelName).LoadAndWait();
                    }
                    else if (myItem.ModelItem != null && myItem.ModelItem.Type != ePhysicalItemType.Vehicle && myItem.ModelItem.ModelName != "")
                    {
                        new Model(myItem.ModelItem.ModelName).LoadAndWait();
                    }
                    else if (myItem.ModelItem != null && myItem.ModelItem.Type == ePhysicalItemType.Vehicle && myItem.ModelItem.ModelName != "")
                    {
                        Vehicle MyVehicle = new Vehicle(myItem.ModelItem.ModelName, Vector3.Zero, 0f);
                        if(MyVehicle.Exists())
                        {
                            MyVehicle.Delete();
                        }
                    }
                }
            }
        }
    }
    private bool PurchaseVehicle(ModItem modItem)
    {
        bool ItemInDeliveryBay = Rage.World.GetEntities(Store.ItemDeliveryPosition, 10f, GetEntitiesFlags.ConsiderAllVehicles).Any();
        if (!ItemInDeliveryBay)
        {
            Vehicle NewVehicle = new Vehicle(modItem.ModelItem.ModelName, Store.ItemDeliveryPosition, Store.ItemDeliveryHeading);
            if (NewVehicle.Exists())
            {
                //if (PlateString != "")
                //{
                //    NewVehicle.LicensePlate = PlateString.Substring(0, 8);
                //}
                NativeFunction.Natives.SET_VEHICLE_COLOURS(NewVehicle, PrimaryColor, SecondaryColor);
                NewVehicle.Wash();
                VehicleExt MyNewCar = new VehicleExt(NewVehicle, Settings);
                World.AddEntity(MyNewCar, ResponseType.None);
                Player.TakeOwnershipOfVehicle(MyNewCar);
                Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", Store.Name, "Purchase", "Thank you for your purchase");
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    private void StartBuyAnimation(ModItem item, bool isIllicit)
    {
        Hide();
        IsActivelyConversing = true;

        if(isIllicit)
        {
            Player.IsConductingIllicitTransaction = true;
            Ped.IsConductingIllicitTransaction = true;
        }



        Player.ButtonPrompts.Clear();
        SayAvailableAmbient(Player.Character, new List<string>() { "GENERIC_BUY", "GENERIC_YES", "BLOCKED_GENEIRC" }, true);
        if (Ped.Pedestrian.Exists())
        {
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Ped.Pedestrian, "mp_common", "givetake1_a", 1.0f, -1.0f, 5000, 50, 0, false, false, false);
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "mp_common", "givetake1_b", 1.0f, -1.0f, 5000, 50, 0, false, false, false);
        }
        GameFiber.Sleep(500);
        string modelName = "";
        bool HasProp = false;
        if(item.PackageItem != null && item.PackageItem.ModelName != "")
        {
            modelName = item.PackageItem.ModelName;
            HasProp = true;
        }
        else if (item.ModelItem != null && item.ModelItem.ModelName != "")
        {
            modelName = item.ModelItem.ModelName;
            HasProp = true;
        }
        if (Ped.Pedestrian.Exists() && HasProp && modelName != "")
        {
            SellingProp = new Rage.Object(modelName, Player.Character.GetOffsetPositionUp(50f));
            GameFiber.Yield();
            if (SellingProp.Exists())
            {
                SellingProp.AttachTo(Ped.Pedestrian, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Ped.Pedestrian, item.ModelItem.AttachBoneIndex), item.ModelItem.AttachOffset, item.ModelItem.AttachRotation);
            }
        }
        GameFiber.Sleep(500);
        if (Ped.Pedestrian.Exists())
        {
            if (SellingProp.Exists())
            {
                SellingProp.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_PED_BONE_INDEX", Player.Character, item.ModelItem.AttachBoneIndex), item.ModelItem.AttachOffset, item.ModelItem.AttachRotation);
            }
        }
        GameFiber.Sleep(1000);
        if (Ped.Pedestrian.Exists())
        {
            if (SellingProp.Exists())
            {
                SellingProp.Delete();
            }
            SayAvailableAmbient(Player.Character, new List<string>() { "GENERIC_THANKS", "GENERIC_BYE" }, true);
            SayAvailableAmbient(Ped.Pedestrian, new List<string>() { "GENERIC_BYE", "GENERIC_THANKS", "PED_RANT" }, true);
        }
        IsActivelyConversing = false;


        if (isIllicit)
        {
            Player.IsConductingIllicitTransaction = false;
            Ped.IsConductingIllicitTransaction = false;
        }
        //Show();     
    }
    private bool SayAvailableAmbient(Ped ToSpeak, List<string> Possibilities, bool WaitForComplete)
    {
        bool Spoke = false;
        if (CanContinueConversation)
        {
            foreach (string AmbientSpeech in Possibilities)
            {
                ToSpeak.PlayAmbientSpeech(null, AmbientSpeech, 0, SpeechModifier.Force);
                GameFiber.Sleep(100);
                if (ToSpeak.Exists() && ToSpeak.IsAnySpeechPlaying)
                {
                    Spoke = true;
                }
                //EntryPoint.WriteToConsole($"SAYAMBIENTSPEECH: {ToSpeak.Handle} Attempting {AmbientSpeech}, Result: {Spoke}");
                if (Spoke)
                {
                    break;
                }
            }
            GameFiber.Sleep(100);
            while (ToSpeak.Exists() && ToSpeak.IsAnySpeechPlaying && WaitForComplete && CanContinueConversation)
            {
                Spoke = true;
                GameFiber.Yield();
            }
            if (!Spoke)
            {
                // Game.DisplayNotification($"\"{Possibilities.FirstOrDefault()}\"");
            }
        }
        return Spoke;
    }
    private class ColorLookup
    {
        public ColorLookup()
        {

        }
        public ColorLookup(int colorID, string colorName)
        {
            ColorID = colorID;
            ColorName = colorName;
        }

        public int ColorID { get; set; }
        public string ColorName { get; set; }
        public override string ToString()
        {
            return ColorName;
        }
    }
}