using ExtensionsMethods;
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
    private List<ColorLookup> ColorList;
    private MenuItem CurrentMenuItem;
    private ModItem CurrentModItem;
    private int CurrentTotalPrice;
    private WeaponInformation CurrentWeapon;
    private WeaponVariation CurrentWeaponVariation = new WeaponVariation();
    private bool hasAttachedProp;
    private bool IsActivelyConversing;
    private bool IsCancelled;
    private MenuPool MenuPool;
    private IModItems ModItems;
    private string PlateString = "";
    private ILocationInteractable Player;
    private string PlayingAnim;
    private string PlayingDict;
    private int PrimaryColor = 0;
    private float PropEntryHeading;
    private Vector3 PropEntryPosition;
    private UIMenu purchaseMenu;
    private int SecondaryColor = 0;
    private Ped SellingPed;
    private Rage.Object SellingProp;
    private Vehicle SellingVehicle;
    private ISettingsProvideable Settings;
    //private TransactableLocation Store;
    private Camera StoreCam;
    private ITimeControllable Time;
    private IWeapons Weapons;
    private IEntityProvideable World;
    private uint NotificationHandle;


    private Transaction Transaction;
    private ShopMenu ShopMenu;
    public PurchaseMenu(MenuPool menuPool, UIMenu parentMenu, ShopMenu shopMenu, Transaction transaction, IModItems modItems, ILocationInteractable player, IEntityProvideable world, ISettingsProvideable settings, IWeapons weapons, ITimeControllable time, Texture bannerImage, bool hasBannerImage, bool removeBanner, string storeName)// public StorePurchaseMenu(MenuPool menuPool, UIMenu parentMenu, TransactableLocation store, Camera storeCam, IModItems modItems, IActivityPerformable player, IEntityProvideable world, ISettingsProvideable settings, StoreTransaction storeTransaction, IWeapons weapons, ITimeControllable time)
    {
        ModItems = modItems;
        Player = player;
        World = world;
        Settings = settings;
        MenuPool = menuPool;
        Weapons = weapons;
        Time = time;
        //Store = store;

        ShopMenu = shopMenu;
        Transaction = transaction;
        BannerImage = bannerImage;
        RemoveBanner = removeBanner;
        HasBannerImage = hasBannerImage;
        StoreName = storeName;



        EntryPoint.WriteToConsole($"PurchaseMenu: HasBannerImage {HasBannerImage} RemoveBanner {RemoveBanner}");

        StoreCam = Camera.RenderingCamera;

        if (parentMenu != null)
        {
            purchaseMenu = MenuPool.AddSubMenu(parentMenu, "Buy");
            if (HasBannerImage)
            {
                purchaseMenu.SetBannerType(BannerImage);
            }
            else if (RemoveBanner)
            {
                purchaseMenu.RemoveBanner();
            }
            purchaseMenu.OnIndexChange += OnIndexChange;
            purchaseMenu.OnItemSelect += OnItemSelect;
        }
    }

    public bool HasBannerImage { get; set; } = false;
    public Texture BannerImage { get; set; }
    public bool RemoveBanner { get; set; } = false;
    public string StoreName { get; set; } = "";
    public int MoneySpent { get; private set; } = 0;
    private bool CanContinueConversation => Player.ActivityManager.CanConverse;
    public bool IsStealing { get; set; }
    public void ClearPreviews()
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
        //EntryPoint.WriteToConsole($"Purchase Menu ClearPreviews Ran", 5);
    }
    public void Dispose()
    {
        Hide();
        Game.RawFrameRender -= (s, e) => MenuPool.DrawBanners(e.Graphics);
    }
    public override void Hide()
    {
        ClearPreviews();
        if (purchaseMenu != null)
        {
            purchaseMenu.Visible = false;
        }
        Player.ButtonPrompts.Clear();
    }
    public void Setup()
    {
        if (Settings.SettingsManager.PlayerOtherSettings.GenerateStoreItemPreviews && Transaction.PreviewItems)
        {
            PreloadModels();
        }
        Transaction.ClearPreviews();
        ColorList = new List<ColorLookup>()
        {
            new ColorLookup(0,"Metallic Black","Metallic","Black",1)
            ,new ColorLookup(1,"Metallic Graphite Black","Metallic","Graphite Black",2)
            ,new ColorLookup(2,"Metallic Black Steal","Metallic","Black Steal",3)
            ,new ColorLookup(3,"Metallic Dark Silver","Metallic","Dark Silver",4)
            ,new ColorLookup(4,"Metallic Silver","Metallic","Silver",5)
            ,new ColorLookup(5,"Metallic Blue Silver","Metallic","Blue Silver",6)
            ,new ColorLookup(6,"Metallic Steel Gray","Metallic","Steel Gray",7)
            ,new ColorLookup(7,"Metallic Shadow Silver","Metallic","Shadow Silver",8)
            ,new ColorLookup(8,"Metallic Stone Silver","Metallic","Stone Silver",9)
            ,new ColorLookup(9,"Metallic Midnight Silver","Metallic","Midnight Silver",10)
            ,new ColorLookup(10,"Metallic Gun Metal","Metallic","Gun Metal",11)
            ,new ColorLookup(11,"Metallic Anthracite Grey","Metallic","Anthracite Grey",12)
            ,new ColorLookup(27,"Metallic Red","Metallic","Red",13)
            ,new ColorLookup(28,"Metallic Torino Red","Metallic","Torino Red",14)
            ,new ColorLookup(29,"Metallic Formula Red","Metallic","Formula Red",15)
            ,new ColorLookup(30,"Metallic Blaze Red","Metallic","Blaze Red",16)
            ,new ColorLookup(31,"Metallic Graceful Red","Metallic","Graceful Red",17)
            ,new ColorLookup(32,"Metallic Garnet Red","Metallic","Garnet Red",18)
            ,new ColorLookup(33,"Metallic Desert Red","Metallic","Desert Red",19)
            ,new ColorLookup(34,"Metallic Cabernet Red","Metallic","Cabernet Red",20)
            ,new ColorLookup(35,"Metallic Candy Red","Metallic","Candy Red",21)
            ,new ColorLookup(36,"Metallic Sunrise Orange","Metallic","Sunrise Orange",22)
            ,new ColorLookup(37,"Metallic Classic Gold","Metallic","Classic Gold",23)
            ,new ColorLookup(38,"Metallic Orange","Metallic","Orange",24)
            ,new ColorLookup(49,"Metallic Dark Green","Metallic","Dark Green",25)
            ,new ColorLookup(50,"Metallic Racing Green","Metallic","Racing Green",26)
            ,new ColorLookup(51,"Metallic Sea Green","Metallic","Sea Green",27)
            ,new ColorLookup(52,"Metallic Olive Green","Metallic","Olive Green",28)
            ,new ColorLookup(53,"Metallic Green","Metallic","Green",29)
            ,new ColorLookup(54,"Metallic Gasoline Blue Green","Metallic","Gasoline Blue Green",30)
            ,new ColorLookup(61,"Metallic Midnight Blue","Metallic","Midnight Blue",31)
            ,new ColorLookup(62,"Metallic Dark Blue","Metallic","Dark Blue",32)
            ,new ColorLookup(63,"Metallic Saxony Blue","Metallic","Saxony Blue",33)
            ,new ColorLookup(64,"Metallic Blue","Metallic","Blue",34)
            ,new ColorLookup(65,"Metallic Mariner Blue","Metallic","Mariner Blue",35)
            ,new ColorLookup(66,"Metallic Harbor Blue","Metallic","Harbor Blue",36)
            ,new ColorLookup(67,"Metallic Diamond Blue","Metallic","Diamond Blue",37)
            ,new ColorLookup(68,"Metallic Surf Blue","Metallic","Surf Blue",38)
            ,new ColorLookup(69,"Metallic Nautical Blue","Metallic","Nautical Blue",39)
            ,new ColorLookup(70,"Metallic Bright Blue","Metallic","Bright Blue",40)
            ,new ColorLookup(71,"Metallic Purple Blue","Metallic","Purple Blue",41)
            ,new ColorLookup(72,"Metallic Spinnaker Blue","Metallic","Spinnaker Blue",42)
            ,new ColorLookup(73,"Metallic Ultra Blue","Metallic","Ultra Blue",43)
            ,new ColorLookup(74,"Metallic Bright Blue","Metallic","Bright Blue",44)
            ,new ColorLookup(88,"Metallic Taxi Yellow","Metallic","Taxi Yellow",45)
            ,new ColorLookup(89,"Metallic Race Yellow","Metallic","Race Yellow",46)
            ,new ColorLookup(90,"Metallic Bronze","Metallic","Bronze",47)
            ,new ColorLookup(91,"Metallic Yellow Bird","Metallic","Yellow Bird",48)
            ,new ColorLookup(92,"Metallic Lime","Metallic","Lime",49)
            ,new ColorLookup(93,"Metallic Champagne","Metallic","Champagne",50)
            ,new ColorLookup(94,"Metallic Pueblo Beige","Metallic","Pueblo Beige",51)
            ,new ColorLookup(95,"Metallic Dark Ivory","Metallic","Dark Ivory",52)
            ,new ColorLookup(96,"Metallic Choco Brown","Metallic","Choco Brown",53)
            ,new ColorLookup(97,"Metallic Golden Brown","Metallic","Golden Brown",54)
            ,new ColorLookup(98,"Metallic Light Brown","Metallic","Light Brown",55)
            ,new ColorLookup(99,"Metallic Straw Beige","Metallic","Straw Beige",56)
            ,new ColorLookup(100,"Metallic Moss Brown","Metallic","Moss Brown",57)
            ,new ColorLookup(101,"Metallic Biston Brown","Metallic","Biston Brown",58)
            ,new ColorLookup(102,"Metallic Beechwood","Metallic","Beechwood",59)
            ,new ColorLookup(103,"Metallic Dark Beechwood","Metallic","Dark Beechwood",60)
            ,new ColorLookup(104,"Metallic Choco Orange","Metallic","Choco Orange",61)
            ,new ColorLookup(105,"Metallic Beach Sand","Metallic","Beach Sand",62)
            ,new ColorLookup(106,"Metallic Sun Bleeched Sand","Metallic","Sun Bleeched Sand",63)
            ,new ColorLookup(107,"Metallic Cream","Metallic","Cream",64)
            ,new ColorLookup(111,"Metallic White","Metallic","White",65)
            ,new ColorLookup(112,"Metallic Frost White","Metallic","Frost White",66)
            ,new ColorLookup(125,"Metallic Securicor Green","Metallic","Securicor Green",67)
            ,new ColorLookup(137,"Metallic Vermillion Pink","Metallic","Vermillion Pink",68)
            ,new ColorLookup(141,"Metallic Black Blue","Metallic","Black Blue",69)
            ,new ColorLookup(142,"Metallic Black Purple","Metallic","Black Purple",70)
            ,new ColorLookup(143,"Metallic Black Red","Metallic","Black Red",71)
            ,new ColorLookup(145,"Metallic Purple","Metallic","Purple",72)
            ,new ColorLookup(146,"Metallic V Dark Blue","Metallic","V Dark Blue",73)
            ,new ColorLookup(150,"Metallic Lava Red","Metallic","Lava Red",74)

            ,new ColorLookup(12,"Matte Black","Matte","Black",75)
            ,new ColorLookup(13,"Matte Gray","Matte","Gray",76)
            ,new ColorLookup(14,"Matte Light Grey","Matte","Light Grey",77)
            ,new ColorLookup(39,"Matte Red","Matte","Red",78)
            ,new ColorLookup(40,"Matte Dark Red","Matte","Dark Red",79)
            ,new ColorLookup(41,"Matte Orange","Matte","Orange",80)
            ,new ColorLookup(42,"Matte Yellow","Matte","Yellow",81)
            ,new ColorLookup(55,"Matte Lime Green","Matte","Lime Green",82)
            ,new ColorLookup(82,"Matte Dark Blue","Matte","Dark Blue",83)
            ,new ColorLookup(83,"Matte Blue","Matte","Blue",84)
            ,new ColorLookup(84,"Matte Midnight Blue","Matte","Midnight Blue",85)
            ,new ColorLookup(128,"Matte Green","Matte","Green",86)
            ,new ColorLookup(129,"Matte Brown","Matte","Brown",87)
            ,new ColorLookup(148,"Matte Purple","Matte","Purple",88)
            ,new ColorLookup(149,"Matte Dark Purple","Matte","Dark Purple",89)
            ,new ColorLookup(151,"Matte Forest Green","Matte","Forest Green",90)
            ,new ColorLookup(152,"Matte Olive Drab","Matte","Olive Drab",91)
            ,new ColorLookup(153,"Matte Desert Brown","Matte","Desert Brown",92)
            ,new ColorLookup(154,"Matte Desert Tan","Matte","Desert Tan",93)
            ,new ColorLookup(155,"Matte Foilage Green","Matte","Foilage Green",94)
            ,new ColorLookup(131,"Matte White","Matte","White",95)

            ,new ColorLookup(15,"Util Black","Util","Black",96)
            ,new ColorLookup(16,"Util Black Poly","Util","Black Poly",97)
            ,new ColorLookup(17,"Util Dark silver","Util","Dark silver",98)
            ,new ColorLookup(18,"Util Silver","Util","Silver",99)
            ,new ColorLookup(19,"Util Gun Metal","Util","Gun Metal",100)
            ,new ColorLookup(20,"Util Shadow Silver","Util","Shadow Silver",101)
            ,new ColorLookup(43,"Util Red","Util","Red",102)
            ,new ColorLookup(44,"Util Bright Red","Util","Bright Red",103)
            ,new ColorLookup(45,"Util Garnet Red","Util","Garnet Red",104)
            ,new ColorLookup(56,"Util Dark Green","Util","Dark Green",105)
            ,new ColorLookup(57,"Util Green","Util","Green",106)
            ,new ColorLookup(75,"Util Dark Blue","Util","Dark Blue",107)
            ,new ColorLookup(76,"Util Midnight Blue","Util","Midnight Blue",108)
            ,new ColorLookup(77,"Util Blue","Util","Blue",109)
            ,new ColorLookup(78,"Util Sea Foam Blue","Util","Sea Foam Blue",110)
            ,new ColorLookup(79,"Util Lightning blue","Util","Lightning blue",111)
            ,new ColorLookup(80,"Util Maui Blue Poly","Util","Maui Blue Poly",112)
            ,new ColorLookup(81,"Util Bright Blue","Util","Bright Blue",113)
            ,new ColorLookup(108,"Util Brown","Util","Brown",114)
            ,new ColorLookup(109,"Util Medium Brown","Util","Medium Brown",115)
            ,new ColorLookup(110,"Util Light Brown","Util","Light Brown",116)
            ,new ColorLookup(122,"Util Off White","Util","Off White",117)





            ,new ColorLookup(21,"Worn Black","Worn","Black",118)
            ,new ColorLookup(22,"Worn Graphite","Worn","Graphite",119)
            ,new ColorLookup(23,"Worn Silver Grey","Worn","Silver Grey",120)
            ,new ColorLookup(24,"Worn Silver","Worn","Silver",121)
            ,new ColorLookup(25,"Worn Blue Silver","Worn","Blue Silver",122)
            ,new ColorLookup(26,"Worn Shadow Silver","Worn","Shadow Silver",123)
            ,new ColorLookup(46,"Worn Red","Worn","Red",124)
            ,new ColorLookup(47,"Worn Golden Red","Worn","Golden Red",125)
            ,new ColorLookup(48,"Worn Dark Red","Worn","Dark Red",126)
            ,new ColorLookup(58,"Worn Dark Green","Worn","Dark Green",127)
            ,new ColorLookup(59,"Worn Green","Worn","Green",128)
            ,new ColorLookup(60,"Worn Sea Wash","Worn","Sea Wash",129)
            ,new ColorLookup(85,"Worn Dark blue","Worn","Dark blue",130)
            ,new ColorLookup(86,"Worn Blue","Worn","Blue",131)
            ,new ColorLookup(87,"Worn Light blue","Worn","Light blue",132)
            ,new ColorLookup(113,"Worn Honey Beige","Worn","Honey Beige",133)
            ,new ColorLookup(114,"Worn Brown","Worn","Brown",134)
            ,new ColorLookup(115,"Worn Dark Brown","Worn","Dark Brown",135)
            ,new ColorLookup(116,"Worn straw beige","Worn","straw beige",136)
            ,new ColorLookup(121,"Worn Off White","Worn","Off White",137)
            ,new ColorLookup(123,"Worn Orange","Worn","Orange",138)
            ,new ColorLookup(124,"Worn Light Orange","Worn","Light Orange",139)
            ,new ColorLookup(126,"Worn Taxi Yellow","Worn","Taxi Yellow",140)
            ,new ColorLookup(130,"Worn Orange","Worn","Orange",141)
            ,new ColorLookup(132,"Worn White","Worn","White",142)
            ,new ColorLookup(133,"Worn Olive Army Green","Worn","Olive Army Green",143)



            ,new ColorLookup(134,"Pure White","Standard","Pure White",150)
            ,new ColorLookup(135,"Hot Pink","Standard","Hot Pink",150)
            ,new ColorLookup(136,"Salmon pink","Standard","Salmon Pink",150)
            ,new ColorLookup(138,"Orange","Standard","Orange",150)
            ,new ColorLookup(139,"Green","Standard","Green",150)
            ,new ColorLookup(140,"Blue","Standard","Blue",150)

            ,new ColorLookup(120,"Chrome","Chrome","Chrome",199)


            ,new ColorLookup(117,"Brushed Steel","Metals","Brushed Steel",200)
            ,new ColorLookup(118,"Brushed Black Steel","Metals","Brushed Black Steel",201)
            ,new ColorLookup(119,"Brushed Aluminium","Metals","Brushed Aluminium",202)
            ,new ColorLookup(158,"Pure Gold","Metals","Pure Gold",203)
            ,new ColorLookup(159,"Brushed Gold","Metals","Brushed Gold",204)



            //,new ColorLookup(127,"police car blue","Unknown","police car blue",205)
            //,new ColorLookup(156,"DEFAULT ALLOY COLOR","Unknown","DEFAULT ALLOY COLOR",206)
            //,new ColorLookup(157,"Epsilon Blue","Unknown","Epsilon Blue",207)
            //,new ColorLookup(144,"hunter green","Unknown","hunter green",208)
            //,new ColorLookup(147,"MODSHOP BLACK1","Unknown","MODSHOP BLACK1",209)

        };
        CreatePurchaseMenu();
    }
    public override void Show()
    {
        //CreatePurchaseMenu();
        if (purchaseMenu != null)
        {
            purchaseMenu.Visible = true;
        }
    }
    public override void Toggle()
    {
        if (!purchaseMenu.Visible)
        {
            Show();
        }
        else
        {
            Hide();
        }
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
    public void OnAmountChanged(ModItem modItem)
    {
        if (modItem != null)
        {
            foreach (UIMenuItem uiMenuItem in purchaseMenu.MenuItems)
            {
                if (uiMenuItem.Text == modItem.Name && uiMenuItem.GetType() == typeof(UIMenuNumericScrollerItem<int>))
                {
                    MenuItem masdenuItem = ShopMenu.Items.Where(x => x.ModItemName == modItem.Name).FirstOrDefault();
                    UpdatePropEntryData(modItem, masdenuItem, (UIMenuNumericScrollerItem<int>)uiMenuItem);
                }
            }
        }
    }

    private void AddPropEntry(MenuItem menuItem, ModItem modItem)
    {
        UIMenuNumericScrollerItem<int> myscroller = new UIMenuNumericScrollerItem<int>(menuItem.ModItemName, "", 1, 99, 1) 
        { 
            Formatter = v => $"{(v == 1 && modItem.MeasurementName == "Item" ? "" : v.ToString() + " ")}" +
            $"{(modItem.MeasurementName != "Item" || v > 1 ? modItem.MeasurementName : "")}" +
            $"{(v > 1 ? "(s)" : "")}" +
            $"{(modItem.MeasurementName != "Item" || v > 1 ? " - " : "")}" +

            $"{(menuItem.PurchasePrice == 0 ? "FREE" : $"${(v * menuItem.PurchasePrice)}")}", 
            Value = 1 
        };
        UpdatePropEntryData(modItem, menuItem, myscroller);
        purchaseMenu.AddItem(myscroller);
    }
    private void AddVehicleEntry(MenuItem cii, ModItem myItem)
    {
        VehicleItem vehicleItem = (VehicleItem)myItem;
        if (vehicleItem != null && (!vehicleItem.RequiresDLC || Settings.SettingsManager.PlayerOtherSettings.AllowDLCVehiclesInStores))
        {
            //Setup Menu
            string formattedPurchasePrice = cii.PurchasePrice.ToString("C0");
            if (cii.PurchasePrice == 0)
            {
                formattedPurchasePrice = "FREE";
            }

            string MakeName = NativeHelper.VehicleMakeName(Game.GetHashKey(myItem.ModelItem.ModelName));
            string ClassName = NativeHelper.VehicleClassName(Game.GetHashKey(myItem.ModelItem.ModelName));
            string ModelName = NativeHelper.VehicleModelName(Game.GetHashKey(myItem.ModelItem.ModelName));


            string description;
            if (myItem.Description.Length >= 200)
            {
                description = myItem.Description.Substring(0, 200) + "...";//menu cant show more than 225?, need some for below
            }
            else
            {
                description = myItem.Description;
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
            if (vehicleItem.RequiresDLC)
            {
                description += $"~n~~b~DLC Vehicle";
            }


            UIMenu VehicleMenu = null;
            bool FoundCategoryMenu = false;
            foreach (UIMenu uimen in MenuPool.ToList())
            {
                if (uimen.SubtitleText == ClassName)
                {
                    FoundCategoryMenu = true;
                    VehicleMenu = MenuPool.AddSubMenu(uimen, cii.ModItemName);
                    uimen.MenuItems[uimen.MenuItems.Count() - 1].Description = description;
                    uimen.MenuItems[uimen.MenuItems.Count() - 1].RightLabel = formattedPurchasePrice;
                    EntryPoint.WriteToConsole($"Added Vehicle {myItem.Name} To SubMenu {uimen.SubtitleText}", 5);
                    break;
                }
            }
            if (!FoundCategoryMenu && VehicleMenu == null)
            {
                VehicleMenu = MenuPool.AddSubMenu(purchaseMenu, cii.ModItemName);
                purchaseMenu.MenuItems[purchaseMenu.MenuItems.Count() - 1].Description = description;
                purchaseMenu.MenuItems[purchaseMenu.MenuItems.Count() - 1].RightLabel = formattedPurchasePrice;
                EntryPoint.WriteToConsole($"Added Vehicle {myItem.Name} To Main Buy Menu", 5);
            }

            if (HasBannerImage)
            {
                VehicleMenu.SetBannerType(BannerImage);
            }
            else if (RemoveBanner)
            {
                VehicleMenu.RemoveBanner();
            }

            UIMenu colorFullMenu = MenuPool.AddSubMenu(VehicleMenu, "Colors");
            colorFullMenu.SubtitleText = "COLORS";
            VehicleMenu.MenuItems[VehicleMenu.MenuItems.Count() - 1].Description = "Pick Colors";
            if (HasBannerImage) { colorFullMenu.SetBannerType(BannerImage); }

            UIMenu primaryColorMenu = MenuPool.AddSubMenu(colorFullMenu, "Primary Color");
            primaryColorMenu.SubtitleText = "COLOR GROUPS";
            colorFullMenu.MenuItems[colorFullMenu.MenuItems.Count() - 1].Description = "Pick Primary Colors";
            if (HasBannerImage) { primaryColorMenu.SetBannerType(BannerImage); }

            UIMenu secondaryColorMenu = MenuPool.AddSubMenu(colorFullMenu, "Secondary Color");
            secondaryColorMenu.SubtitleText = "COLOR GROUPS";
            colorFullMenu.MenuItems[colorFullMenu.MenuItems.Count() - 1].Description = "Pick Secondary Colors";
            if (HasBannerImage) { secondaryColorMenu.SetBannerType(BannerImage); }


            //Add Color Sub Menu Here
            foreach (string colorGroupString in ColorList.GroupBy(x => x.ColorGroup).Select(x => x.Key).Distinct().OrderBy(x => x))
            {
                UIMenu primarycolorGroupMenu = MenuPool.AddSubMenu(primaryColorMenu, colorGroupString);
                primarycolorGroupMenu.SubtitleText = "PRIMARY COLORS";
                primaryColorMenu.MenuItems[primaryColorMenu.MenuItems.Count() - 1].Description = "Choose a color group";
                if (HasBannerImage) { primarycolorGroupMenu.SetBannerType(BannerImage); }

                UIMenu secondarycolorGroupMenu = MenuPool.AddSubMenu(secondaryColorMenu, colorGroupString);
                secondarycolorGroupMenu.SubtitleText = "SECONDARY COLORS";
                secondaryColorMenu.MenuItems[secondaryColorMenu.MenuItems.Count() - 1].Description = "Choose a color group";
                if (HasBannerImage) { secondarycolorGroupMenu.SetBannerType(BannerImage); }

                foreach (ColorLookup cl in ColorList.Where(x=>x.ColorGroup == colorGroupString))
                {
                    UIMenuItem actualColorPrimary = new UIMenuItem(cl.ColorName, cl.FullColorName);
                    actualColorPrimary.Activated += (sender, selectedItem) =>
                    {
                        PrimaryColor = cl.ColorID;
                        if (SellingVehicle.Exists())
                        {
                            NativeFunction.Natives.SET_VEHICLE_COLOURS(SellingVehicle, PrimaryColor, SecondaryColor);
                        }
                    };
                    primarycolorGroupMenu.AddItem(actualColorPrimary);

                    UIMenuItem actualColorSeconmdary = new UIMenuItem(cl.ColorName, cl.FullColorName);
                    actualColorSeconmdary.Activated += (sender, selectedItem) =>
                    {
                        SecondaryColor = cl.ColorID;
                        if (SellingVehicle.Exists())
                        {
                            NativeFunction.Natives.SET_VEHICLE_COLOURS(SellingVehicle, PrimaryColor, SecondaryColor);
                        }
                    };
                    secondarycolorGroupMenu.AddItem(actualColorSeconmdary);
                }
            }

            //Purchase Stuff Here
            UIMenuItem Purchase = new UIMenuItem($"Purchase", "Select to purchase this vehicle") { RightLabel = formattedPurchasePrice };
            Purchase.Activated += (sender, selectedItem) =>
            {
                if (cii != null)
                {
                    EntryPoint.WriteToConsole($"Vehicle Purchase {cii.ModItemName} Player.Money {Player.BankAccounts.Money} menuItem.PurchasePrice {cii.PurchasePrice}", 5);
                    if (Player.BankAccounts.Money < cii.PurchasePrice)
                    {
                        DisplayInsufficientFundsMessage();
                        return;
                    }
                    if (!PurchaseVehicle(vehicleItem))
                    {
                        return;
                    }
                    Player.BankAccounts.GiveMoney(-1 * cii.PurchasePrice);
                    MoneySpent += cii.PurchasePrice;
                }
                sender.Visible = false;
                Dispose();
            };
            VehicleMenu.AddItem(Purchase);
        }
    }



    private void AddWeaponEntry(MenuItem cii, ModItem myItem)
    {
        WeaponItem weaponItem = (WeaponItem)myItem;


        if (weaponItem != null)
        {
            EntryPoint.WriteToConsole($"Purchase Menu Add Weapon Entry ItemName: {myItem.Name}", 5);
            string description;
            if (myItem.Description.Length >= 200)
            {
                description = myItem.Description.Substring(0, 200) + "...";//menu cant show more than 225?, need some for below
            }
            else
            {
                description = myItem.Description;
            }
            description += "~n~~s~";
            if (weaponItem.RequiresDLC)
            {
                description += $"~n~~b~DLC Weapon";
            }
            string formattedPurchasePrice = cii.PurchasePrice.ToString("C0");

            if (cii.PurchasePrice == 0)
            {
                formattedPurchasePrice = "FREE";
            }

            WeaponInformation myWeapon = Weapons.GetWeapon(myItem.ModelItem.ModelName);
            UIMenu WeaponMenu = null;
            bool FoundCategoryMenu = false;
            if (myWeapon != null)
            {
                foreach (UIMenu uimen in MenuPool.ToList())
                {
                    if (uimen.SubtitleText == myWeapon.Category.ToString() && uimen.ParentMenu == purchaseMenu)
                    {
                        FoundCategoryMenu = true;
                        WeaponMenu = MenuPool.AddSubMenu(uimen, cii.ModItemName);
                        uimen.MenuItems[uimen.MenuItems.Count() - 1].Description = description;
                        uimen.MenuItems[uimen.MenuItems.Count() - 1].RightLabel = formattedPurchasePrice;
                        EntryPoint.WriteToConsole($"Added Weapon {myItem.Name} To SubMenu {uimen.SubtitleText}", 5);
                        break;
                    }
                }
            }
            if (!FoundCategoryMenu && WeaponMenu == null)
            {
                WeaponMenu = MenuPool.AddSubMenu(purchaseMenu, cii.ModItemName);
                purchaseMenu.MenuItems[purchaseMenu.MenuItems.Count() - 1].Description = description;
                purchaseMenu.MenuItems[purchaseMenu.MenuItems.Count() - 1].RightLabel = formattedPurchasePrice;
                EntryPoint.WriteToConsole($"Added Weapon {myItem.Name} To Main Buy Menu", 5);
            }
            // WeaponMenu.OnMenuOpen += OnWeaponMenuOpen;
            if (HasBannerImage)
            {
                WeaponMenu.SetBannerType(BannerImage);
            }
            else if (RemoveBanner)
            {
                WeaponMenu.RemoveBanner();
            }
            if (myWeapon != null)
            {
                foreach (ComponentSlot cs in myWeapon.PossibleComponents.Where(x => x.ComponentSlot != ComponentSlot.Coloring).GroupBy(x => x.ComponentSlot).Select(x => x.Key))
                {
                    List<MenuItemExtra> stuffList = new List<MenuItemExtra>() { new MenuItemExtra("Default", 0) };
                    bool AddedAny = false;
                    foreach (WeaponComponent mywc in myWeapon.PossibleComponents.Where(x => x.ComponentSlot == cs))
                    {
                        MenuItemExtra menuItemExtra = cii.Extras.FirstOrDefault(x => x.ExtraName == mywc.Name);
                        if (menuItemExtra != null)
                        {
                            AddedAny = true;
                            stuffList.Add(menuItemExtra);
                        }
                    }
                    if (AddedAny)
                    {
                        WeaponMenu.AddItem(new UIMenuListScrollerItem<MenuItemExtra>(cs.ToString(), cs.ToString(), stuffList)
                        { //Formatter = v => v.HasItem ? $"{v.ExtraName} - Equipped" : v.PurchasePrice == 0 ? v.ExtraName : $"{v.ExtraName} - ${v.PurchasePrice}",
                            Index = 0
                        });
                    }
                }
            }
            UIMenuNumericScrollerItem<int> PurchaseAmmo = new UIMenuNumericScrollerItem<int>($"Purchase Ammo", $"Select to purchase ammo for this weapon.", cii.SubAmount, 500, cii.SubAmount) { Index = 0, Formatter = v => $"{v} - ${cii.SubPrice * v}" };
            UIMenuItem Purchase = new UIMenuItem($"Purchase", "Select to purchase this Weapon") { RightLabel = formattedPurchasePrice };
            if (NativeFunction.Natives.HAS_PED_GOT_WEAPON<bool>(Player.Character, myWeapon.Hash, false))
            {
                Purchase.Enabled = false;
            }

            if (myWeapon.Category != WeaponCategory.Melee && myWeapon.Category != WeaponCategory.Throwable)
            {
                WeaponMenu.AddItem(PurchaseAmmo);
            }
            WeaponMenu.AddItem(Purchase);
            WeaponMenu.OnItemSelect += OnWeaponItemSelect;
            WeaponMenu.OnScrollerChange += OnWeaponScrollerChange;
            WeaponMenu.OnMenuOpen += OnWeaponMenuOpen;

            //WeaponMenu.OnMenuOpen += WeaponMenuOnMenuOpen;
        }
    }

    private void CreateCategories()
    {
        List<WeaponCategory> WeaponCategories = new List<WeaponCategory>();
        List<string> VehicleClasses = new List<string>();
        int TotalWeapons = ShopMenu.Items.Where(x => x.Purchaseable && ModItems.Get(x.ModItemName)?.ModelItem?.Type == ePhysicalItemType.Weapon).Count();
        int TotalVehicles = ShopMenu.Items.Where(x => x.Purchaseable && ModItems.Get(x.ModItemName)?.ModelItem?.Type == ePhysicalItemType.Vehicle).Count();

        foreach (MenuItem cii in ShopMenu.Items.Where(x => x.Purchaseable))
        {
            ModItem myItem = ModItems.Get(cii.ModItemName);
            if (myItem != null)
            {
                if (myItem.ModelItem?.Type == ePhysicalItemType.Weapon)
                {
                    if (TotalWeapons >= 7)
                    {
                        WeaponInformation myWeapon = Weapons.GetWeapon(myItem.ModelItem.ModelName);
                        if (myWeapon != null)
                        {
                            if (!WeaponCategories.Contains(myWeapon.Category))
                            {
                                WeaponCategories.Add(myWeapon.Category);
                                UIMenu WeaponMenu = MenuPool.AddSubMenu(purchaseMenu, myWeapon.Category.ToString());
                                if (HasBannerImage)
                                {
                                    WeaponMenu.SetBannerType(BannerImage);
                                }
                                else if (RemoveBanner)
                                {
                                    WeaponMenu.RemoveBanner();
                                }
                                WeaponMenu.OnIndexChange += OnIndexChange;
                                WeaponMenu.OnItemSelect += OnItemSelect;
                                WeaponMenu.OnMenuOpen += OnMenuOpen;
                                WeaponMenu.OnMenuClose += OnMenuClose;
                            }
                        }
                    }
                }
                else if (myItem.ModelItem?.Type == ePhysicalItemType.Vehicle)
                {
                    if (TotalVehicles >= 7)
                    {
                        string ClassName = NativeHelper.VehicleClassName(Game.GetHashKey(myItem.ModelItem.ModelName));
                        if (ClassName != "")
                        {
                            if (!VehicleClasses.Contains(ClassName))
                            {
                                VehicleClasses.Add(ClassName);
                                UIMenu VehicleMenu = MenuPool.AddSubMenu(purchaseMenu, ClassName);
                                if (HasBannerImage)
                                {
                                    VehicleMenu.SetBannerType(BannerImage);
                                }
                                else if (RemoveBanner)
                                {
                                    VehicleMenu.RemoveBanner();
                                }
                                VehicleMenu.OnIndexChange += OnIndexChange;
                                VehicleMenu.OnItemSelect += OnItemSelect;
                                VehicleMenu.OnMenuClose += OnMenuClose;
                                VehicleMenu.OnMenuOpen += OnMenuOpen;
                            }
                        }
                    }
                }
            }
        }
    }
    private void CreatePreview(UIMenuItem myItem)
    {
        ClearPreviews();
        // GameFiber.Yield();
        if (myItem != null && Transaction.PreviewItems && Settings.SettingsManager.PlayerOtherSettings.GenerateStoreItemPreviews)
        {
            EntryPoint.WriteToConsole($"SIMPLE TRANSACTION OnIndexChange Text: {myItem.Text}", 5);
            ModItem itemToShow = ModItems.AllItems().Where(x => x.Name == myItem.Text).FirstOrDefault();
            if (itemToShow != null)
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
                else if (itemToShow.ModelItem?.Type == ePhysicalItemType.Weapon)
                {
                    PreviewWeapon(itemToShow);
                }
            }
        }
    }
    private void CreatePurchaseMenu()
    {
        if (purchaseMenu != null)
        {
            EntryPoint.WriteToConsole($"CreatePurchaseMenu RAN!", 5);
            purchaseMenu.Clear();
            bool shouldCreateCategories = false;
            if (ShopMenu.Items.Where(x => x.Purchaseable).Count() >= 7)
            {
                shouldCreateCategories = true;
            }
            if (shouldCreateCategories)
            {
                CreateCategories();
            }
            foreach (MenuItem cii in ShopMenu.Items)
            {
                if (cii != null && cii.Purchaseable)
                {
                    ModItem myItem = ModItems.Get(cii.ModItemName);
                    if (myItem != null)
                    {
                        if (myItem.ModelItem?.Type == ePhysicalItemType.Vehicle)
                        {
                            AddVehicleEntry(cii, myItem);
                        }
                        else if (myItem.ModelItem?.Type == ePhysicalItemType.Weapon)
                        {
                            AddWeaponEntry(cii, myItem);
                        }
                        else
                        {
                            AddPropEntry(cii, myItem);
                        }
                    }
                }
            }
            // OnIndexChange(purchaseMenu, purchaseMenu.CurrentSelection);

            //LoopMenus();
        }
    }
    private void UpdatePropEntryData(ModItem modItem, MenuItem menuItem, UIMenuNumericScrollerItem<int> scrollerItem)
    {
        if (modItem != null && menuItem != null && scrollerItem != null)
        {
            InventoryItem PlayerInventoryItem = Player.Inventory.ItemsList.Where(x => x.ModItem.Name == menuItem.ModItemName).FirstOrDefault();
            int PlayerItems = 0;
            if (PlayerInventoryItem != null)
            {
                PlayerItems = PlayerInventoryItem.Amount;
            }

            string formattedPurchasePrice = menuItem.PurchasePrice.ToString("C0");




            if(IsStealing)
            {
                formattedPurchasePrice = "FREE";
            }
            if (menuItem.PurchasePrice == 0)
            {
                formattedPurchasePrice = "FREE";
            }

            string description = modItem.Description;
            //if (description == "")
            //{
            //    description = $"{menuItem.ModItemName} {formattedPurchasePrice}";
            //}
            description += "~n~~s~";

            description += $"~n~Price: ~r~{formattedPurchasePrice}~s~";

            description += $"~n~Type: ~p~{modItem.ItemType}~s~" + (modItem.ItemSubType != ItemSubType.None ? $" - ~p~{modItem.ItemSubType}~s~" : "");
            description += $"~n~~b~{modItem.AmountPerPackage}~s~ Item(s) per Package";
            if (modItem.AmountPerPackage > 1)
            {
                description += $"~n~~b~{((float)menuItem.PurchasePrice / (float)modItem.AmountPerPackage).ToString("C2")} ~s~per Item";
            }



            description += modItem.PurchaseMenuDescription(Settings);

            //if (modItem.ChangesHealth && !Settings.SettingsManager.NeedsSettings.ApplyNeeds)
            //{
            //    description += $"~n~{modItem.HealthChangeDescription}";
            //}
            //if (modItem.ChangesNeeds && Settings.SettingsManager.NeedsSettings.ApplyNeeds)
            //{
            //    description += $"~n~{modItem.NeedChangeDescription}";
            //}
            //if (modItem.ConsumeOnPurchase && (modItem.Type == eConsumableType.Eat || modItem.Type == eConsumableType.Drink))
            //{
            //    description += $"~n~~r~Dine-In Only~s~";
            //}




            bool enabled = true;
            int RemainingToBuy = 99;
            int MaxBuy = 99;
            if (menuItem.NumberOfItemsToSellToPlayer != -1)
            {
                RemainingToBuy = menuItem.NumberOfItemsToSellToPlayer - menuItem.ItemsSoldToPlayer;
                if (RemainingToBuy <= 0)
                {
                    MaxBuy = 0;
                    RemainingToBuy = 1;
                    enabled = false;
                }
                else
                {
                    MaxBuy = RemainingToBuy;
                }
                description += $"~n~{MaxBuy} {modItem.MeasurementName}(s) For Purchase~s~";
            }
            description += $"~n~Player Inventory: {PlayerItems}~s~ {modItem.MeasurementName}(s)";
            scrollerItem.Maximum = RemainingToBuy;
            scrollerItem.Enabled = enabled;
            scrollerItem.Description = description;
        }
    }
    private void OnIndexChange(UIMenu sender, int newIndex)
    {
        EntryPoint.WriteToConsole($"OnIndexChange {sender.SubtitleText} {newIndex}", 5);
        if (newIndex != -1)
        {
            CreatePreview(sender.MenuItems[newIndex]);
        }
    }
    private void OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        EntryPoint.WriteToConsole($"OnItemSelect {selectedItem.Text}", 5);
        ModItem ToAdd = ModItems.AllItems().Where(x => x.Name == selectedItem.Text).FirstOrDefault();
        MenuItem menuItem = ShopMenu.Items.Where(x => x.ModItemName == selectedItem.Text).FirstOrDefault();
        if (ToAdd != null && menuItem != null)
        {
            CurrentModItem = ToAdd;
            CurrentMenuItem = menuItem;
            if (ToAdd.ModelItem?.Type == ePhysicalItemType.Vehicle)//SubMenu
            {
                CurrentWeapon = null;
                CurrentWeaponVariation = new WeaponVariation();
                CreatePreview(selectedItem);
                EntryPoint.WriteToConsole($"Purchase Menu: {CurrentModItem.Name} OnItemSelect", 5);
            }
            else if (ToAdd.ModelItem?.Type == ePhysicalItemType.Weapon)//SubMenu
            {
                CurrentWeapon = Weapons.GetWeapon(CurrentModItem.ModelItem.ModelName);
                CurrentWeaponVariation = new WeaponVariation();
                CreatePreview(selectedItem);
                EntryPoint.WriteToConsole($"Purchase Menu: {CurrentModItem.Name} OnItemSelect", 5);
            }
            else
            {
                int TotalItems = 1;
                UIMenuNumericScrollerItem<int> scrollerItem = null;
                if (selectedItem.GetType() == typeof(UIMenuNumericScrollerItem<int>))
                {
                    scrollerItem = (UIMenuNumericScrollerItem<int>)selectedItem;
                    TotalItems = scrollerItem.Value;
                }
                CurrentWeapon = null;
                CurrentWeaponVariation = new WeaponVariation();

                PurchaseItem(CurrentModItem, CurrentMenuItem, TotalItems);
                Transaction.OnAmountChanged(CurrentModItem);
                //UpdatePropEntryData(CurrentModItem, CurrentMenuItem, scrollerItem);
            }
        }
        else
        {
            CurrentModItem = null;
            CurrentMenuItem = null;
        }
    }
    private void OnMenuClose(UIMenu sender)
    {
        EntryPoint.WriteToConsole($"OnMenuClose {sender.SubtitleText} {sender.CurrentSelection}", 5);
        ClearPreviews();
    }
    private void OnMenuOpen(UIMenu sender)
    {
        EntryPoint.WriteToConsole($"OnMenuOpen {sender.SubtitleText} {sender.CurrentSelection}", 5);
        if (sender.CurrentSelection != -1)
        {
            CreatePreview(sender.MenuItems[sender.CurrentSelection]);
        }
        foreach (UIMenuItem uimen in sender.MenuItems)
        {
            MenuItem menuItem = ShopMenu.Items.Where(x => x.ModItemName == uimen.Text).FirstOrDefault();
            if (menuItem != null)
            {
                EntryPoint.WriteToConsole($"    Sub Level: {menuItem.ModItemName}", 5);
            }
        }
    }
    private void OnVehicleScrollerChange(UIMenu sender, UIMenuScrollerItem item, int oldIndex, int newIndex)
    {
        if (item.Text == "Primary Color")
        {
            PrimaryColor = newIndex;
            if (SellingVehicle.Exists())
            {
                NativeFunction.Natives.SET_VEHICLE_COLOURS(SellingVehicle, PrimaryColor, SecondaryColor);
            }
        }
        else if (item.Text == "Secondary Color")
        {
            SecondaryColor = newIndex;
            if (SellingVehicle.Exists())
            {
                NativeFunction.Natives.SET_VEHICLE_COLOURS(SellingVehicle, PrimaryColor, SecondaryColor);
            }
        }
        else if (item.Text == "Color")
        {

        }
    }
    private void OnWeaponItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (selectedItem.Text == "Purchase" && CurrentModItem != null)
        {
            if (CurrentMenuItem != null)
            {
                int TotalPrice = CurrentMenuItem.PurchasePrice;
                foreach (WeaponComponent wc in CurrentWeaponVariation?.Components)
                {
                    MenuItemExtra mie = CurrentMenuItem.Extras.FirstOrDefault(x => x.ExtraName == wc.Name);
                    if (mie != null)
                    {
                        TotalPrice += mie.PurchasePrice;
                    }
                }
                EntryPoint.WriteToConsole($"Weapon Purchase {CurrentMenuItem.ModItemName} Player.Money {Player.BankAccounts.Money} menuItem.PurchasePrice {CurrentMenuItem.PurchasePrice}", 5);
                if (Player.BankAccounts.Money < TotalPrice)
                {
                    DisplayInsufficientFundsMessage();
                    return;
                }
                if (!PurchaseWeapon())
                {
                    return;
                }
                Player.BankAccounts.GiveMoney(-1 * TotalPrice);
                MoneySpent += TotalPrice;
                OnWeaponMenuOpen(sender);
            }
        }
        else if (selectedItem.Text == "Purchase Ammo" && CurrentModItem != null)
        {
            int TotalItems = 1;
            if (selectedItem.GetType() == typeof(UIMenuNumericScrollerItem<int>))
            {
                UIMenuNumericScrollerItem<int> myItem = (UIMenuNumericScrollerItem<int>)selectedItem;
                TotalItems = myItem.Value;
            }
            if (CurrentMenuItem != null)
            {
                int TotalPrice = CurrentMenuItem.SubPrice * TotalItems;
                EntryPoint.WriteToConsole($"Weapon Purchase {CurrentMenuItem.ModItemName} Player.Money {Player.BankAccounts.Money} menuItem.PurchasePrice {1}", 5);
                if (Player.BankAccounts.Money < TotalPrice)
                {
                    DisplayInsufficientFundsMessage();
                    return;
                }
                if (!PurchaseAmmo(TotalItems))
                {
                    return;
                }
                Player.BankAccounts.GiveMoney(-1 * TotalPrice);
                MoneySpent += TotalPrice;
                OnWeaponMenuOpen(sender);
            }
        }
        else if (selectedItem.GetType() == typeof(UIMenuListScrollerItem<MenuItemExtra>))
        {
            UIMenuListScrollerItem<MenuItemExtra> myItem = (UIMenuListScrollerItem<MenuItemExtra>)selectedItem;
            bool isComponentSlot = false;
            ComponentSlot selectedSlot;
            foreach (ComponentSlot cs in Enum.GetValues(typeof(ComponentSlot)).Cast<ComponentSlot>().ToList())
            {
                if (cs.ToString() == selectedItem.Text)
                {
                    selectedSlot = cs;
                    isComponentSlot = true;
                    if (myItem.SelectedItem.ExtraName == "Default")
                    {
                        CurrentWeapon.SetSlotDefault(Player.Character, selectedSlot);
                        Game.DisplayNotification("CHAR_BLOCKED", "CHAR_BLOCKED", StoreName, "Set Default", $"Set the {selectedSlot} slot to default");
                        OnWeaponMenuOpen(sender);
                        return;
                    }
                    break;
                }
            }
            WeaponComponent myComponent = CurrentWeapon.PossibleComponents.Where(x => x.Name == myItem.SelectedItem.ExtraName).FirstOrDefault();
            if (myComponent != null && CurrentMenuItem != null)
            {
                EntryPoint.WriteToConsole($"Weapon Component Purchase {CurrentMenuItem.ModItemName} Player.Money {Player.BankAccounts.Money} menuItem.PurchasePrice {CurrentMenuItem.PurchasePrice} myComponent {myComponent.Name}", 5);
                if (Player.BankAccounts.Money < myItem.SelectedItem.PurchasePrice)
                {
                    DisplayInsufficientFundsMessage();
                    return;
                }
                if (CurrentWeapon.HasComponent(Player.Character, myComponent))
                {
                    PlayErrorSound();
                    Game.DisplayNotification("CHAR_BLOCKED", "CHAR_BLOCKED", StoreName, "Already Owned", "We are sorry, we are unable to complete this transation, as the item is already owned");
                    return;
                }
                if (!PurchaseComponent(myComponent))
                {
                    return;
                }
                Player.BankAccounts.GiveMoney(-1 * myItem.SelectedItem.PurchasePrice);
                MoneySpent += myItem.SelectedItem.PurchasePrice;
                OnWeaponMenuOpen(sender);
            }
        }
    }
    private void OnWeaponMenuOpen(UIMenu sender)
    {
        EntryPoint.WriteToConsole($"OnWeaponMenuOpen RAN!", 5);
        foreach (UIMenuItem uimen in sender.MenuItems)
        {
            if (uimen.GetType() == typeof(UIMenuListScrollerItem<MenuItemExtra>))
            {
                UIMenuListScrollerItem<MenuItemExtra> myItem = (UIMenuListScrollerItem<MenuItemExtra>)(object)uimen;
                foreach (MenuItemExtra stuff in myItem.Items)
                {
                    WeaponComponent myComponent = CurrentWeapon.PossibleComponents.Where(x => x.Name == stuff.ExtraName).FirstOrDefault();
                    if (myComponent != null)
                    {
                        if (CurrentWeapon.HasComponent(Player.Character, myComponent))
                        {
                            myItem.SelectedItem = stuff;
                            stuff.HasItem = true;
                            EntryPoint.WriteToConsole($"OnWeaponMenuOpen RAN! {myComponent.Name} HAS COMPONENT {stuff.HasItem} {myItem.OptionText}", 5);
                        }
                        else
                        {
                            //myItem.SelectedItem = stuff;
                            stuff.HasItem = false;
                            EntryPoint.WriteToConsole($"OnWeaponMenuOpen RAN! {myComponent.Name} DOES NOT HAVE COMPONENT  {stuff.HasItem} {myItem.OptionText}", 5);
                        }
                        // myItem.Formatter = v => v.HasItem ? $"{v.ExtraName} - Equipped" : v.PurchasePrice == 0 ? v.ExtraName : $"{v.ExtraName} - ${v.PurchasePrice}";
                    }
                }
                myItem.Reformat();
            }
            else if (uimen.Text == "Purchase")
            {
                if (CurrentWeapon.HasWeapon(Player.Character) && CurrentWeapon.Category != WeaponCategory.Throwable)
                {
                    uimen.Enabled = false;
                    uimen.RightLabel = "Owned";
                }
                else
                {
                    uimen.Enabled = true;
                    // uimen.RightLabel = "Owned";
                }
            }
            else if (uimen.Text == "Purchase Ammo")
            {
                if (CurrentWeapon.HasWeapon(Player.Character))
                {
                    uimen.Enabled = true;
                }
                else
                {
                    uimen.Enabled = false;
                }
            }
            EntryPoint.WriteToConsole($"Full Below Level: {uimen.Text}", 5);
        }
    }
    private void OnWeaponScrollerChange(UIMenu sender, UIMenuScrollerItem item, int oldIndex, int newIndex)
    {
        try
        {
            if (CurrentWeapon != null)
            {
                //EntryPoint.WriteToConsole($"OnWeaponScrollerChange Weapon Start {CurrentModItem.ModelItem.ModelName} {CurrentWeapon.ModelName}", 5);
                string ExtraName = item.OptionText;
                int ExtraPrice = 0;
                if (item.GetType() == typeof(UIMenuListScrollerItem<MenuItemExtra>))
                {
                    UIMenuListScrollerItem<MenuItemExtra> test = (UIMenuListScrollerItem<MenuItemExtra>)item;
                    ExtraName = test.SelectedItem.ExtraName;
                    ExtraPrice = test.SelectedItem.PurchasePrice;
                }
                WeaponComponent myComponent = CurrentWeapon.PossibleComponents.Where(x => x.Name == ExtraName).FirstOrDefault();
                if (myComponent != null)
                {
                    uint ComponentHash = NativeFunction.Natives.GET_WEAPON_COMPONENT_TYPE_MODEL<uint>(myComponent.Hash);
                    NativeFunction.Natives.REQUEST_MODEL(ComponentHash);
                    uint GameTimeRequested = Game.GameTime;
                    while (!NativeFunction.Natives.HAS_MODEL_LOADED<bool>(ComponentHash))
                    {
                        if (Game.GameTime - GameTimeRequested >= 500)
                        {
                            break;
                        }
                        GameFiber.Yield();
                    }
                    foreach (WeaponComponent wc in CurrentWeapon.PossibleComponents.Where(x => x.ComponentSlot == myComponent.ComponentSlot))
                    {
                        if (NativeFunction.Natives.HAS_WEAPON_GOT_WEAPON_COMPONENT<bool>(SellingProp, wc.Hash))
                        {
                            NativeFunction.Natives.REMOVE_WEAPON_COMPONENT_FROM_WEAPON_OBJECT(SellingProp, wc.Hash);
                        }
                        if (CurrentWeaponVariation.Components.Contains(wc))
                        {
                            CurrentWeaponVariation.Components.Remove(wc);
                        }
                    }
                    if (item.Text != "Default")
                    {
                        if (NativeFunction.Natives.DOES_WEAPON_TAKE_WEAPON_COMPONENT<bool>(CurrentWeapon.Hash, myComponent.Hash))
                        {
                            if (!NativeFunction.Natives.HAS_WEAPON_GOT_WEAPON_COMPONENT<bool>(SellingProp, myComponent.Hash))
                            {
                                NativeFunction.Natives.GIVE_WEAPON_COMPONENT_TO_WEAPON_OBJECT(SellingProp, myComponent.Hash);
                            }
                            if (!CurrentWeaponVariation.Components.Contains(myComponent))
                            {
                                CurrentWeaponVariation.Components.Add(myComponent);
                            }
                        }
                    }
                }
                else
                {
                    foreach (WeaponComponent wc in CurrentWeapon.PossibleComponents.Where(x => x.ComponentSlot.ToString() == item.Text))
                    {
                        if (NativeFunction.Natives.HAS_WEAPON_GOT_WEAPON_COMPONENT<bool>(SellingProp, wc.Hash))
                        {
                            NativeFunction.Natives.REMOVE_WEAPON_COMPONENT_FROM_WEAPON_OBJECT(SellingProp, wc.Hash);
                        }
                        if (CurrentWeaponVariation.Components.Contains(wc))
                        {
                            CurrentWeaponVariation.Components.Remove(wc);
                        }
                    }
                }
            }
            if (CurrentWeapon != null)
            {
                if (!CurrentWeapon.HasWeapon(Player.Character))
                {
                    int TotalPrice = CurrentMenuItem.PurchasePrice;
                    EntryPoint.WriteToConsole($"Current Weapon: {CurrentWeapon.ModelName}", 5);
                    if (CurrentWeaponVariation != null)
                    {
                        foreach (WeaponComponent weaponComponents in CurrentWeaponVariation.Components)
                        {
                            MenuItemExtra mie = CurrentMenuItem.Extras.FirstOrDefault(x => x.ExtraName == weaponComponents.Name);
                            if (mie != null && !CurrentWeapon.HasComponent(Player.Character, weaponComponents))
                            {
                                TotalPrice += mie.PurchasePrice;
                            }
                            EntryPoint.WriteToConsole($"                Components On: {weaponComponents.Name}", 5);
                        }
                    }
                    foreach (UIMenuItem uimli in sender.MenuItems)
                    {
                        if (uimli.Text == "Purchase")
                        {
                            uimli.RightLabel = TotalPrice.ToString("C0");
                            break;
                        }
                    }
                    EntryPoint.WriteToConsole($"Current Weapon: {CurrentWeapon.ModelName} Total Price {TotalPrice}", 5);
                }
            }
        }
        catch (Exception ex)
        {
            EntryPoint.WriteToConsole($"Weapon Preview Error {ex.Message} {ex.StackTrace}", 0);
        }
    }
    private void PreloadModels()
    {
        foreach (MenuItem menuItem in ShopMenu.Items)//preload all item models so it doesnt bog the menu down
        {
            try
            {
                if (menuItem.Purchaseable)
                {
                    ModItem myItem = ModItems.AllItems().Where(x => x.Name == menuItem.ModItemName).FirstOrDefault();
                    if (myItem != null)
                    {
                        if (myItem.ModelItem != null && myItem.ModelItem.Type == ePhysicalItemType.Weapon && myItem.ModelItem.ModelName != "")
                        {
                            //if (NativeFunction.Natives.IS_MODEL_VALID<bool>(myItem.ModelItem.ModelHash))
                            //{
                                NativeFunction.Natives.REQUEST_WEAPON_ASSET(myItem.ModelItem.ModelHash, 31, 0);
                           // }
                        }
                        else if (myItem.ModelItem != null && myItem.ModelItem.Type == ePhysicalItemType.Vehicle && myItem.ModelItem.ModelName != "")
                        {
                            if (NativeFunction.Natives.IS_MODEL_VALID<bool>(Game.GetHashKey(myItem.ModelItem.ModelName.ToLower())))
                            {
                                Vehicle MyVehicle = new Vehicle(myItem.ModelItem.ModelName, Vector3.Zero, 0f);
                                if (MyVehicle.Exists())
                                {
                                    MyVehicle.Delete();
                                }
                            }
                        }
                        else if (myItem.PackageItem != null && myItem.PackageItem.Type == ePhysicalItemType.Prop && myItem.PackageItem.ModelName != "")
                        {
                            if (NativeFunction.Natives.IS_MODEL_VALID<bool>(Game.GetHashKey(myItem.PackageItem.ModelName.ToLower())))
                            {
                                new Model(myItem.PackageItem.ModelName).LoadAndWait();
                            }
                        }
                        else if (myItem.ModelItem != null && myItem.ModelItem.Type == ePhysicalItemType.Prop && myItem.ModelItem.ModelName != "")
                        {
                            if (NativeFunction.Natives.IS_MODEL_VALID<bool>(Game.GetHashKey(myItem.ModelItem.ModelName.ToLower())))
                            {
                                new Model(myItem.ModelItem.ModelName).LoadAndWait();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Game.DisplayNotification($"Error Preloading Model {ex.Message} {ex.StackTrace}");
            }
        }
    }


    private void PreviewPed(ModItem itemToShow)
    {
        //GameFiber.Yield();
    }
    private void PreviewProp(ModItem itemToShow)
    {
        try
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
            Vector3 Position = Vector3.Zero;
            if (StoreCam.Exists())
            {
                if (useClose)
                {
                    Position = StoreCam.Position + StoreCam.Direction;
                }
                else
                {
                    Position = StoreCam.Position + (StoreCam.Direction.ToNormalized() * 3f);
                }
            }
            else
            {
                Vector3 GPCamPos = NativeFunction.Natives.GET_GAMEPLAY_CAM_COORD<Vector3>();
                Vector3 GPCamDir = NativeHelper.GetGameplayCameraDirection();
                if (useClose)
                {
                    Position = GPCamPos + GPCamDir / 2;
                }
                else
                {
                    Position = GPCamPos + GPCamDir.ToNormalized() * 3f;
                }
            }
            if (ModelToSpawn != "" && NativeFunction.Natives.IS_MODEL_VALID<bool>(Game.GetHashKey(ModelToSpawn)))
            {
                try
                {
                    SellingProp = new Rage.Object(ModelToSpawn, Position);
                }
                catch (Exception ex)
                {
                    EntryPoint.WriteToConsole($"Error Spawning Model {ex.Message} {ex.StackTrace}");
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
        catch (Exception ex)
        {
            Game.DisplayNotification($"Error Displaying Model {ex.Message} {ex.StackTrace}");
        }
    }
    private void PreviewVehicle(ModItem itemToShow)
    {
        if (itemToShow != null && itemToShow.ModelItem != null && NativeFunction.Natives.IS_MODEL_VALID<bool>(Game.GetHashKey(itemToShow.ModelItem.ModelName)))
        {
            NativeFunction.Natives.CLEAR_AREA(Transaction.ItemPreviewPosition.X, Transaction.ItemPreviewPosition.Y, Transaction.ItemPreviewPosition.Z, 4f, true, false, false, false);
            SellingVehicle = new Vehicle(itemToShow.ModelItem.ModelName, Transaction.ItemPreviewPosition, Transaction.ItemPreviewHeading);
        }
        //GameFiber.Yield();
        if (SellingVehicle.Exists())
        {
            SellingVehicle.Wash();
            NativeFunction.Natives.SET_VEHICLE_COLOURS(SellingVehicle, PrimaryColor, SecondaryColor);
            NativeFunction.Natives.SET_VEHICLE_ON_GROUND_PROPERLY<bool>(SellingVehicle, 5.0f);
        }
    }
    private void PreviewWeapon(ModItem itemToShow)
    {
        try
        {
            if (itemToShow != null && itemToShow.ModelItem != null && itemToShow.ModelItem.ModelName != "")
            {
                Vector3 Position = Vector3.Zero;
                if (StoreCam.Exists())
                {
                    Position = StoreCam.Position + StoreCam.Direction / 2f;
                }
                else
                {
                    Vector3 GPCamPos = NativeFunction.Natives.GET_GAMEPLAY_CAM_COORD<Vector3>();
                    Vector3 GPCamDir = NativeHelper.GetGameplayCameraDirection();
                    Position = GPCamPos + GPCamDir / 2f;
                }
                if (NativeFunction.Natives.HAS_WEAPON_ASSET_LOADED<bool>(itemToShow.ModelItem.ModelHash))
                {
                    SellingProp = NativeFunction.Natives.CREATE_WEAPON_OBJECT<Rage.Object>(itemToShow.ModelItem.ModelHash, 60, Position.X, Position.Y, Position.Z, true, 1.0f, 0, 0, 1);
                }
                if (SellingProp.Exists())
                {
                    float length = SellingProp.Model.Dimensions.X;
                    float width = SellingProp.Model.Dimensions.Y;
                    float height = SellingProp.Model.Dimensions.Z;

                    float LargestSideLength = length;
                    if(width > LargestSideLength)
                    {
                        LargestSideLength = width;
                    }
                    if(height > LargestSideLength)
                    {
                        LargestSideLength = height;
                    }

                    if (StoreCam.Exists())
                    {
                        Position = StoreCam.Position + (StoreCam.Direction.ToNormalized() * 0.5f) + (StoreCam.Direction.ToNormalized() * LargestSideLength / 2f);//
                    }
                    else
                    {
                        Vector3 GPCamPos = NativeFunction.Natives.GET_GAMEPLAY_CAM_COORD<Vector3>();
                        Vector3 GPCamDir = NativeHelper.GetGameplayCameraDirection();
                        Position = GPCamPos + (GPCamDir.ToNormalized() * 0.5f) + (GPCamDir.ToNormalized() * LargestSideLength / 2f);
                    }
                    SellingProp.Position = Position;
                    SellingProp.SetRotationYaw(SellingProp.Rotation.Yaw + 45f);
                    if (SellingProp != null && SellingProp.Exists())
                    {
                        NativeFunction.Natives.SET_ENTITY_HAS_GRAVITY(SellingProp, false);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Game.DisplayNotification($"Error Displaying Model {ex.Message} {ex.StackTrace}");
        }
        //GameFiber.Yield();
    }

    private bool PurchaseAmmo(int TotalItems)
    {
        if (CurrentWeapon != null && CurrentWeapon.Category != WeaponCategory.Melee && NativeFunction.Natives.HAS_PED_GOT_WEAPON<bool>(Player.Character, CurrentWeapon.Hash, false))
        {
            NativeFunction.Natives.ADD_AMMO_TO_PED(Player.Character, CurrentWeapon.Hash, TotalItems);
            if(StoreName == "")
            {
                Game.DisplayNotification($"Thank you for your purchase of ~r~{TotalItems} ~s~rounds for ~o~{CurrentMenuItem.ModItemName}~s~");
            }
            else
            {
                Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", StoreName, "~g~Purchase", $"Thank you for your purchase of ~r~{TotalItems} ~s~rounds for ~o~{CurrentMenuItem.ModItemName}~s~");
            }
            return true;
        }
        PlayErrorSound();
        Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", StoreName, "~r~Purchase Failed", "We are sorry, we are unable to complete this transation");
        return false;
    }
    private bool PurchaseComponent(WeaponComponent myComponent)
    {
        if (CurrentWeapon != null && CurrentWeapon.AddComponent(Player.Character, myComponent))
        {
            if(StoreName == "")
            {
                Game.DisplayNotification($"Thank you for your purchase of ~r~{myComponent.Name}~s~ for ~o~{CurrentMenuItem.ModItemName}~s~");
            }
            else
            {
                Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", StoreName, "~g~Purchase", $"Thank you for your purchase of ~r~{myComponent.Name}~s~ for ~o~{CurrentMenuItem.ModItemName}~s~");
            }
            return true;
        }
        PlayErrorSound();
        Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", StoreName, "~r~Purchase Failed", "We are sorry, we are unable to complete this transation");
        return false;
    }
    private bool PurchaseItem(ModItem modItem, MenuItem menuItem, int TotalItems)
    {
        int TotalPrice = menuItem.PurchasePrice * TotalItems;
        CurrentTotalPrice = TotalPrice;
        if (Player.BankAccounts.Money >= TotalPrice || IsStealing)
        {
            Transaction.OnItemPurchased(modItem, menuItem, TotalItems);
            menuItem.ItemsSoldToPlayer += TotalItems;
            if (modItem.ConsumeOnPurchase)
            {
                Player.ActivityManager.UseInventoryItem(modItem, false);
            }
            else
            {
                Player.Inventory.Add(modItem, TotalItems * modItem.AmountPerPackage);
            }
            if (!IsStealing)
            {
                Player.BankAccounts.GiveMoney(-1 * TotalPrice);
                MoneySpent += TotalPrice;
            }
            return true;
        }
        DisplayInsufficientFundsMessage();
        return false;
    }
    private bool PurchaseVehicle(ModItem modItem)
    {


        bool ItemInDeliveryBay = true;
        SpawnPlace ChosenSpawn = null;
        foreach (SpawnPlace sp in Transaction.ItemDeliveryLocations.OrderBy(x => RandomItems.GetRandomNumber(0f,1f)))
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
            Vehicle NewVehicle = new Vehicle(modItem.ModelItem.ModelName, ChosenSpawn.Position, ChosenSpawn.Heading);
            if (NewVehicle.Exists())
            {
                Transaction.OnItemPurchased(modItem, CurrentMenuItem, 1);
                CurrentMenuItem.ItemsSoldToPlayer += 1;
                NativeFunction.Natives.SET_VEHICLE_COLOURS(NewVehicle, PrimaryColor, SecondaryColor);
                NewVehicle.Wash();
                VehicleExt MyNewCar = World.Vehicles.GetVehicleExt(NewVehicle);
                if (MyNewCar == null)
                {
                    MyNewCar = new VehicleExt(NewVehicle, Settings);
                    MyNewCar.Setup();
                    EntryPoint.WriteToConsole("New Vehicle Created in PurchaseVehicle");
                }
                World.Vehicles.AddEntity(MyNewCar, ResponseType.None);
                Player.VehicleOwnership.TakeOwnershipOfVehicle(MyNewCar, false);
                return true;
            }
            else
            {
                PlayErrorSound();
                Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", StoreName, "~r~Delivery Failed", "We are sorry, we are unable to complete this transation");
                return false;
            }
        }
        else
        {
            PlayErrorSound();
            Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", StoreName, "~o~Blocked Delivery", "We are sorry, we are unable to complete this transation, the delivery bay is blocked");
            return false;
        }
    }
    private bool PurchaseWeapon()
    {
        if (CurrentWeapon != null)
        {
            if (CurrentWeapon.Category == WeaponCategory.Throwable || !NativeFunction.Natives.HAS_PED_GOT_WEAPON<bool>(Player.Character, CurrentWeapon.Hash, false))
            {
                Transaction.OnItemPurchased(CurrentModItem, CurrentMenuItem, 1);
                NativeFunction.Natives.GIVE_WEAPON_TO_PED(Player.Character, CurrentWeapon.Hash, CurrentWeapon.AmmoAmount, false, false);
                if (CurrentWeaponVariation != null)
                {
                    CurrentWeapon.ApplyWeaponVariation(Player.Character, CurrentWeaponVariation);
                }
                Player.WeaponEquipment.SetUnarmed();
                return true;
            }
        }
        return false;
    }

    private void DisplayInsufficientFundsMessage()
    {
        PlayErrorSound();
        Game.RemoveNotification(NotificationHandle);
        NotificationHandle = Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", StoreName, "~r~Insufficient Funds", "We are sorry, we are unable to complete this transation.");
    }
    private void PlayErrorSound()
    {
        NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "ERROR", "HUD_LIQUOR_STORE_SOUNDSET", 0);
    }

    private class ColorLookup
    {
        public ColorLookup()
        {
        }
        public ColorLookup(int colorID, string fullColorName, string colorGroup, string colorName, int order)
        {
            ColorID = colorID;
            FullColorName = fullColorName;
            ColorGroup = colorGroup;
            ColorName = colorName;
            Order = order;
        }

        public int ColorID { get; set; }
        public string FullColorName { get; set; }
        public string ColorGroup { get; set; }
        public string ColorName { get; set; }
        public int Order { get; set; }
        public override string ToString()
        {
            return ColorName;
        }
    }
}