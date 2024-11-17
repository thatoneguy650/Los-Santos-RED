using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using RAGENativeUI;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

public class Transaction
{
    private UIMenu ParentMenu;
    private uint NotificationHandle;
    private uint GameTimeLastRotatedModel;
    public List<VehicleColorLookup> VehicleColors { get; private set; }
    public PurchaseMenu PurchaseMenu { get; private set; }
    public SellMenu SellMenu { get; private set; }
    public ShopMenu ShopMenu { get; set; }
    public GameLocation Store { get; private set; }
    public MenuPool MenuPool { get; private set; }
    public SpawnPlace VehiclePreviewPosition { get; set; }
    public List<SpawnPlace> VehicleDeliveryLocations { get; set; } = new List<SpawnPlace>();
    public bool PreviewItems { get; set; } = true;
    public PersonTransaction PersonTransaction { get; set; }


    public ItemDesires ItemDesires => PersonTransaction?.TransactionPed?.ItemDesires;// PersonTransaction?.TransactionPed?.ItemDesires != null ? PersonTransaction?.TransactionPed?.ItemDesires : Store.ItemDesires;
    public bool IsFreeVehicles { get; set; } = false;
    public bool IsFreeWeapons { get; set; } = false;
    public bool IsFreeItems { get; set; } = false;
    public Ped SellingPed { get; set; }
    public Rage.Object SellingProp { get; set; }
    public Vehicle SellingVehicle { get; set; }
    public bool HasBannerImage { get; set; } = false;
    public Texture BannerImage { get; set; }
    public bool RemoveBanner { get; set; } = false;
    public string StoreName { get; set; } = "";
    public int MoneySpent { get; set; } = 0;
    public bool IsStealing { get; set; }
    public bool IsShowingConfirmDialog { get; set; } = false;
    public bool RotatePreview { get; set; } = true;
    public bool RotateVehiclePreview { get; set; } = false;
    public ILicensePlatePreviewable LicensePlatePreviewable { get; set; }
    public bool IsPurchasing { get; set; } = true;
    public bool UseAccounts { get; set; } = true;
    public LocationCamera LocationCamera { get; set; }
    public bool IsInteriorInteract { get; internal set; }
    //public bool ShouldUpdateVariablePrices { get; set; } = false;

    public Transaction(MenuPool menuPool, UIMenu parentMenu, ShopMenu menu, GameLocation store)
    {
        MenuPool = menuPool;
        ParentMenu = parentMenu;
        ShopMenu = menu;
        Store = store;
    }
    public Transaction() : base()
    {

    }
    public void CreateTransactionMenu(ILocationInteractable player, IModItems modItems, IEntityProvideable world, ISettingsProvideable settings, IWeapons weapons, ITimeControllable time)
    {
        SetupLists();
        if (Store == null)
        {
            HasBannerImage = false;
            RemoveBanner = true;
            BannerImage = null;
        }
        else
        {
            HasBannerImage = Store.HasBannerImage;
            RemoveBanner = Store.RemoveBanner;
            BannerImage = Store.BannerImage;
        }
        if(ShopMenu == null || ShopMenu.Items == null || !ShopMenu.Items.Any())
        {
            return;
        }
        EntryPoint.WriteToConsole($"IsFreeVehicles {IsFreeVehicles} IsFreeWeapons {IsFreeWeapons} IsFreeItems {IsFreeItems}");
        foreach (MenuItem mi in ShopMenu.Items)
        {
            ModItem modItem = modItems.Get(mi.ModItemName);
            //if (ShouldUpdateVariablePrices)
            //{
            //    mi.UpdatePrices();
            //}
            if (modItem != null && ((IsFreeVehicles && modItem.ModelItem?.Type == ePhysicalItemType.Vehicle) || (IsFreeWeapons && modItem.ModelItem?.Type == ePhysicalItemType.Weapon) || (IsFreeItems && modItem.ModelItem?.Type != ePhysicalItemType.Weapon && modItem.ModelItem?.Type != ePhysicalItemType.Vehicle)))
            {
                mi.SetFree();
            }
            else
            {
                mi.ResetPrice();
            }
        }    
        if (ShopMenu.Items.Any(x => x.Purchaseable))
        {
            if (Store != null)
            {
                PurchaseMenu = new PurchaseMenu(MenuPool, ParentMenu, ShopMenu, this, modItems, player, world, settings, weapons, time);
                if(Store.VendorAbandoned)
                {
                    IsStealing = true;
                }
            }
            else
            {
                PurchaseMenu = new PurchaseMenu(MenuPool, ParentMenu, ShopMenu, this, modItems, player, world, settings, weapons, time);
            }
            PurchaseMenu.Setup();
        }
        if (ShopMenu.Items.Any(x => x.Sellable))
        {
            if (Store != null && !Store.VendorAbandoned)
            {
                SellMenu = new SellMenu(MenuPool, ParentMenu, ShopMenu, this, modItems, player, world, settings, weapons, time);//, Store.BannerImage, Store.HasBannerImage, Store.RemoveBanner, Store.Name);//was IsUsingCustomCam before
            }
            else
            {
                SellMenu = new SellMenu(MenuPool, ParentMenu, ShopMenu, this, modItems, player, world, settings, weapons, time);//, null,false, true, "");//was IsUsingCustomCam before
            }
            SellMenu.Setup();
        }
        //player.OnTransactionMenuCreated(Store, MenuPool, ParentMenu);
    }
    private void SetupLists()
    {
        VehicleColors = new List<VehicleColorLookup>()
        {
            new VehicleColorLookup(0,"Metallic Black","Metallic","Black",1) { RGBColor = System.Drawing.Color.FromArgb(13, 17, 22) }
            ,new VehicleColorLookup(1,"Metallic Graphite Black","Metallic","Graphite Black",2) { RGBColor = System.Drawing.Color.FromArgb(28, 29, 33) }
            ,new VehicleColorLookup(2,"Metallic Black Steal","Metallic","Black Steal",3) { RGBColor = System.Drawing.Color.FromArgb(50, 56, 61) }
            ,new VehicleColorLookup(3,"Metallic Dark Silver","Metallic","Dark Silver",4) { RGBColor = System.Drawing.Color.FromArgb(69, 75, 79) }
            ,new VehicleColorLookup(4,"Metallic Silver","Metallic","Silver",5) { RGBColor = System.Drawing.Color.FromArgb(153, 157, 160) }
            ,new VehicleColorLookup(5,"Metallic Blue Silver","Metallic","Blue Silver",6) { RGBColor = System.Drawing.Color.FromArgb(194, 196, 198) }
            ,new VehicleColorLookup(6,"Metallic Steel Gray","Metallic","Steel Gray",7) { RGBColor = System.Drawing.Color.FromArgb(151, 154, 151) }
            ,new VehicleColorLookup(7,"Metallic Shadow Silver","Metallic","Shadow Silver",8) { RGBColor = System.Drawing.Color.FromArgb(99, 115, 128) }
            ,new VehicleColorLookup(8,"Metallic Stone Silver","Metallic","Stone Silver",9) { RGBColor = System.Drawing.Color.FromArgb(99, 98, 92) }
            ,new VehicleColorLookup(9,"Metallic Midnight Silver","Metallic","Midnight Silver",10) { RGBColor = System.Drawing.Color.FromArgb(60, 63, 71) }
            ,new VehicleColorLookup(10,"Metallic Gun Metal","Metallic","Gun Metal",11) { RGBColor = System.Drawing.Color.FromArgb(68, 78, 84) }
            ,new VehicleColorLookup(11,"Metallic Anthracite Grey","Metallic","Anthracite Grey",12) { RGBColor = System.Drawing.Color.FromArgb(29, 33, 41) }
            ,new VehicleColorLookup(27,"Metallic Red","Metallic","Red",13) { RGBColor = System.Drawing.Color.FromArgb(192, 14, 26) }
            ,new VehicleColorLookup(28,"Metallic Torino Red","Metallic","Torino Red",14) { RGBColor = System.Drawing.Color.FromArgb(218, 25, 24) }
            ,new VehicleColorLookup(29,"Metallic Formula Red","Metallic","Formula Red",15) { RGBColor = System.Drawing.Color.FromArgb(182, 17, 27) }
            ,new VehicleColorLookup(30,"Metallic Blaze Red","Metallic","Blaze Red",16) { RGBColor = System.Drawing.Color.FromArgb(165, 30, 35) }
            ,new VehicleColorLookup(31,"Metallic Graceful Red","Metallic","Graceful Red",17) { RGBColor = System.Drawing.Color.FromArgb(123, 26, 34) }
            ,new VehicleColorLookup(32,"Metallic Garnet Red","Metallic","Garnet Red",18) { RGBColor = System.Drawing.Color.FromArgb(142, 27, 31) }
            ,new VehicleColorLookup(33,"Metallic Desert Red","Metallic","Desert Red",19) { RGBColor = System.Drawing.Color.FromArgb(111, 24, 24) }
            ,new VehicleColorLookup(34,"Metallic Cabernet Red","Metallic","Cabernet Red",20) { RGBColor = System.Drawing.Color.FromArgb(73, 17, 29) }
            ,new VehicleColorLookup(35,"Metallic Candy Red","Metallic","Candy Red",21) { RGBColor = System.Drawing.Color.FromArgb(182, 15, 37) }
            ,new VehicleColorLookup(36,"Metallic Sunrise Orange","Metallic","Sunrise Orange",22) { RGBColor = System.Drawing.Color.FromArgb(212, 74, 23) }
            ,new VehicleColorLookup(37,"Metallic Classic Gold","Metallic","Classic Gold",23) { RGBColor = System.Drawing.Color.FromArgb(194, 148, 79) }
            ,new VehicleColorLookup(38,"Metallic Orange","Metallic","Orange",24) { RGBColor = System.Drawing.Color.FromArgb(247, 134, 22) }
            ,new VehicleColorLookup(49,"Metallic Dark Green","Metallic","Dark Green",25) { RGBColor = System.Drawing.Color.FromArgb(19, 36, 40) }
            ,new VehicleColorLookup(50,"Metallic Racing Green","Metallic","Racing Green",26) { RGBColor = System.Drawing.Color.FromArgb(18, 46, 43) }
            ,new VehicleColorLookup(51,"Metallic Sea Green","Metallic","Sea Green",27) { RGBColor = System.Drawing.Color.FromArgb(18, 56, 60) }
            ,new VehicleColorLookup(52,"Metallic Olive Green","Metallic","Olive Green",28) { RGBColor = System.Drawing.Color.FromArgb(49, 66, 63) }
            ,new VehicleColorLookup(53,"Metallic Green","Metallic","Green",29) { RGBColor = System.Drawing.Color.FromArgb(21, 92, 45) }
            ,new VehicleColorLookup(54,"Metallic Gasoline Blue Green","Metallic","Gasoline Blue Green",30) { RGBColor = System.Drawing.Color.FromArgb(27, 103, 112) }
            ,new VehicleColorLookup(61,"Metallic Midnight Blue","Metallic","Midnight Blue",31) { RGBColor = System.Drawing.Color.FromArgb(34, 46, 70) }
            ,new VehicleColorLookup(62,"Metallic Dark Blue","Metallic","Dark Blue",32) { RGBColor = System.Drawing.Color.FromArgb(35, 49, 85) }
            ,new VehicleColorLookup(63,"Metallic Saxony Blue","Metallic","Saxony Blue",33) { RGBColor = System.Drawing.Color.FromArgb(48, 76, 126) }
            ,new VehicleColorLookup(64,"Metallic Blue","Metallic","Blue",34) { RGBColor = System.Drawing.Color.FromArgb(71, 87, 143) }
            ,new VehicleColorLookup(65,"Metallic Mariner Blue","Metallic","Mariner Blue",35) { RGBColor = System.Drawing.Color.FromArgb(99, 123, 167) }
            ,new VehicleColorLookup(66,"Metallic Harbor Blue","Metallic","Harbor Blue",36) { RGBColor = System.Drawing.Color.FromArgb(57, 71, 98) }
            ,new VehicleColorLookup(67,"Metallic Diamond Blue","Metallic","Diamond Blue",37) { RGBColor = System.Drawing.Color.FromArgb(214, 231, 241) }
            ,new VehicleColorLookup(68,"Metallic Surf Blue","Metallic","Surf Blue",38) { RGBColor = System.Drawing.Color.FromArgb(118, 175, 190) }
            ,new VehicleColorLookup(69,"Metallic Nautical Blue","Metallic","Nautical Blue",39) { RGBColor = System.Drawing.Color.FromArgb(52, 94, 114) }
            ,new VehicleColorLookup(70,"Metallic Bright Blue","Metallic","Bright Blue",40) { RGBColor = System.Drawing.Color.FromArgb(11, 156, 241) }
            ,new VehicleColorLookup(71,"Metallic Purple Blue","Metallic","Purple Blue",41) { RGBColor = System.Drawing.Color.FromArgb(47, 45, 82) }
            ,new VehicleColorLookup(72,"Metallic Spinnaker Blue","Metallic","Spinnaker Blue",42) { RGBColor = System.Drawing.Color.FromArgb(40, 44, 77) }
            ,new VehicleColorLookup(73,"Metallic Ultra Blue","Metallic","Ultra Blue",43) { RGBColor = System.Drawing.Color.FromArgb(35, 84, 161) }
            ,new VehicleColorLookup(74,"Metallic Bright Blue","Metallic","Bright Blue",44) { RGBColor = System.Drawing.Color.FromArgb(110, 163, 198) }
            ,new VehicleColorLookup(88,"Metallic Taxi Yellow","Metallic","Taxi Yellow",45) { RGBColor = System.Drawing.Color.FromArgb(255, 207, 32) }
            ,new VehicleColorLookup(89,"Metallic Race Yellow","Metallic","Race Yellow",46) { RGBColor = System.Drawing.Color.FromArgb(251, 226, 18) }
            ,new VehicleColorLookup(90,"Metallic Bronze","Metallic","Bronze",47) { RGBColor = System.Drawing.Color.FromArgb(145, 101, 50) }
            ,new VehicleColorLookup(91,"Metallic Yellow Bird","Metallic","Yellow Bird",48) { RGBColor = System.Drawing.Color.FromArgb(224, 225, 61) }
            ,new VehicleColorLookup(92,"Metallic Lime","Metallic","Lime",49) { RGBColor = System.Drawing.Color.FromArgb(152, 210, 35) }
            ,new VehicleColorLookup(93,"Metallic Champagne","Metallic","Champagne",50) { RGBColor = System.Drawing.Color.FromArgb(155, 140, 120) }
            ,new VehicleColorLookup(94,"Metallic Pueblo Beige","Metallic","Pueblo Beige",51) { RGBColor = System.Drawing.Color.FromArgb(80, 50, 24) }
            ,new VehicleColorLookup(95,"Metallic Dark Ivory","Metallic","Dark Ivory",52) { RGBColor = System.Drawing.Color.FromArgb(71, 63, 43) }
            ,new VehicleColorLookup(96,"Metallic Choco Brown","Metallic","Choco Brown",53) { RGBColor = System.Drawing.Color.FromArgb(34, 27, 25) }
            ,new VehicleColorLookup(97,"Metallic Golden Brown","Metallic","Golden Brown",54) { RGBColor = System.Drawing.Color.FromArgb(101, 63, 35) }
            ,new VehicleColorLookup(98,"Metallic Light Brown","Metallic","Light Brown",55) { RGBColor = System.Drawing.Color.FromArgb(119, 92, 62) }
            ,new VehicleColorLookup(99,"Metallic Straw Beige","Metallic","Straw Beige",56) { RGBColor = System.Drawing.Color.FromArgb(172, 153, 117) }
            ,new VehicleColorLookup(100,"Metallic Moss Brown","Metallic","Moss Brown",57) { RGBColor = System.Drawing.Color.FromArgb(108, 107, 75) }
            ,new VehicleColorLookup(101,"Metallic Biston Brown","Metallic","Biston Brown",58) { RGBColor = System.Drawing.Color.FromArgb(64, 46, 43) }
            ,new VehicleColorLookup(102,"Metallic Beechwood","Metallic","Beechwood",59) { RGBColor = System.Drawing.Color.FromArgb(164, 150, 95) }
            ,new VehicleColorLookup(103,"Metallic Dark Beechwood","Metallic","Dark Beechwood",60) { RGBColor = System.Drawing.Color.FromArgb(70, 35, 26) }
            ,new VehicleColorLookup(104,"Metallic Choco Orange","Metallic","Choco Orange",61) { RGBColor = System.Drawing.Color.FromArgb(117, 43, 25) }
            ,new VehicleColorLookup(105,"Metallic Beach Sand","Metallic","Beach Sand",62) { RGBColor = System.Drawing.Color.FromArgb(191, 174, 123) }
            ,new VehicleColorLookup(106,"Metallic Sun Bleeched Sand","Metallic","Sun Bleeched Sand",63) { RGBColor = System.Drawing.Color.FromArgb(223, 213, 178) }
            ,new VehicleColorLookup(107,"Metallic Cream","Metallic","Cream",64) { RGBColor = System.Drawing.Color.FromArgb(247, 237, 213) }
            ,new VehicleColorLookup(111,"Metallic White","Metallic","White",65) { RGBColor = System.Drawing.Color.FromArgb(255, 255, 246) }
            ,new VehicleColorLookup(112,"Metallic Frost White","Metallic","Frost White",66) { RGBColor = System.Drawing.Color.FromArgb(234, 234, 234) }
            ,new VehicleColorLookup(125,"Metallic Securicor Green","Metallic","Securicor Green",67) { RGBColor = System.Drawing.Color.FromArgb(131, 197, 102) }
            ,new VehicleColorLookup(137,"Metallic Vermillion Pink","Metallic","Vermillion Pink",68) { RGBColor = System.Drawing.Color.FromArgb(223, 88, 145) }
            ,new VehicleColorLookup(141,"Metallic Black Blue","Metallic","Black Blue",69) { RGBColor = System.Drawing.Color.FromArgb(10, 12, 23) }
            ,new VehicleColorLookup(142,"Metallic Black Purple","Metallic","Black Purple",70) { RGBColor = System.Drawing.Color.FromArgb(12, 13, 24) }
            ,new VehicleColorLookup(143,"Metallic Black Red","Metallic","Black Red",71) { RGBColor = System.Drawing.Color.FromArgb(14, 13, 20) }
            ,new VehicleColorLookup(145,"Metallic Purple","Metallic","Purple",72) { RGBColor = System.Drawing.Color.FromArgb(98, 18, 118) }
            ,new VehicleColorLookup(146,"Metallic V Dark Blue","Metallic","V Dark Blue",73) { RGBColor = System.Drawing.Color.FromArgb(11, 20, 33) }
            ,new VehicleColorLookup(150,"Metallic Lava Red","Metallic","Lava Red",74) { RGBColor = System.Drawing.Color.FromArgb(188, 25, 23) }

            ,new VehicleColorLookup(12,"Matte Black","Matte","Black",75) { RGBColor = System.Drawing.Color.FromArgb(19, 24, 31) }
            ,new VehicleColorLookup(13,"Matte Gray","Matte","Gray",76) { RGBColor = System.Drawing.Color.FromArgb(38, 40, 42) }
            ,new VehicleColorLookup(14,"Matte Light Grey","Matte","Light Grey",77) { RGBColor = System.Drawing.Color.FromArgb(81, 85, 84) }
            ,new VehicleColorLookup(39,"Matte Red","Matte","Red",78) { RGBColor = System.Drawing.Color.FromArgb(207, 31, 33) }
            ,new VehicleColorLookup(40,"Matte Dark Red","Matte","Dark Red",79) { RGBColor = System.Drawing.Color.FromArgb(115, 32, 33) }
            ,new VehicleColorLookup(41,"Matte Orange","Matte","Orange",80) { RGBColor = System.Drawing.Color.FromArgb(242, 125, 32) }
            ,new VehicleColorLookup(42,"Matte Yellow","Matte","Yellow",81) { RGBColor = System.Drawing.Color.FromArgb(255, 201, 31) }
            ,new VehicleColorLookup(55,"Matte Lime Green","Matte","Lime Green",82) { RGBColor = System.Drawing.Color.FromArgb(102, 184, 31) }
            ,new VehicleColorLookup(82,"Matte Dark Blue","Matte","Dark Blue",83) { RGBColor = System.Drawing.Color.FromArgb(31, 40, 82) }
            ,new VehicleColorLookup(83,"Matte Blue","Matte","Blue",84) { RGBColor = System.Drawing.Color.FromArgb(37, 58, 167) }
            ,new VehicleColorLookup(84,"Matte Midnight Blue","Matte","Midnight Blue",85) { RGBColor = System.Drawing.Color.FromArgb(28, 53, 81) }
            ,new VehicleColorLookup(128,"Matte Green","Matte","Green",86) { RGBColor = System.Drawing.Color.FromArgb(78, 100, 67) }
            ,new VehicleColorLookup(129,"Matte Brown","Matte","Brown",87) { RGBColor = System.Drawing.Color.FromArgb(188, 172, 143) }
            ,new VehicleColorLookup(148,"Matte Purple","Matte","Purple",88) { RGBColor = System.Drawing.Color.FromArgb(107, 31, 123) }
            ,new VehicleColorLookup(149,"Matte Dark Purple","Matte","Dark Purple",89) { RGBColor = System.Drawing.Color.FromArgb(30, 29, 34) }
            ,new VehicleColorLookup(151,"Matte Forest Green","Matte","Forest Green",90) { RGBColor = System.Drawing.Color.FromArgb(45, 54, 42) }
            ,new VehicleColorLookup(152,"Matte Olive Drab","Matte","Olive Drab",91) { RGBColor = System.Drawing.Color.FromArgb(105, 103, 72) }
            ,new VehicleColorLookup(153,"Matte Desert Brown","Matte","Desert Brown",92) { RGBColor = System.Drawing.Color.FromArgb(122, 108, 85) }
            ,new VehicleColorLookup(154,"Matte Desert Tan","Matte","Desert Tan",93) { RGBColor = System.Drawing.Color.FromArgb(195, 180, 146) }
            ,new VehicleColorLookup(155,"Matte Foilage Green","Matte","Foilage Green",94) { RGBColor = System.Drawing.Color.FromArgb(90, 99, 82) }
            ,new VehicleColorLookup(131,"Matte White","Matte","White",95) { RGBColor = System.Drawing.Color.FromArgb(252, 249, 241) }

            ,new VehicleColorLookup(15,"Util Black","Util","Black",96) { RGBColor = System.Drawing.Color.FromArgb(21, 25, 33) }
            ,new VehicleColorLookup(16,"Util Black Poly","Util","Black Poly",97) { RGBColor = System.Drawing.Color.FromArgb(30, 36, 41) }
            ,new VehicleColorLookup(17,"Util Dark silver","Util","Dark silver",98) { RGBColor = System.Drawing.Color.FromArgb(51, 58, 60) }
            ,new VehicleColorLookup(18,"Util Silver","Util","Silver",99) { RGBColor = System.Drawing.Color.FromArgb(140, 144, 149) }
            ,new VehicleColorLookup(19,"Util Gun Metal","Util","Gun Metal",100) { RGBColor = System.Drawing.Color.FromArgb(57, 67, 77) }
            ,new VehicleColorLookup(20,"Util Shadow Silver","Util","Shadow Silver",101) { RGBColor = System.Drawing.Color.FromArgb(80, 98, 114) }
            ,new VehicleColorLookup(43,"Util Red","Util","Red",102) { RGBColor = System.Drawing.Color.FromArgb(156, 16, 22) }
            ,new VehicleColorLookup(44,"Util Bright Red","Util","Bright Red",103) { RGBColor = System.Drawing.Color.FromArgb(222, 15, 24) }
            ,new VehicleColorLookup(45,"Util Garnet Red","Util","Garnet Red",104) { RGBColor = System.Drawing.Color.FromArgb(143, 30, 23) }
            ,new VehicleColorLookup(56,"Util Dark Green","Util","Dark Green",105) { RGBColor = System.Drawing.Color.FromArgb(34, 56, 62) }
            ,new VehicleColorLookup(57,"Util Green","Util","Green",106) { RGBColor = System.Drawing.Color.FromArgb(29, 90, 63) }
            ,new VehicleColorLookup(75,"Util Dark Blue","Util","Dark Blue",107) { RGBColor = System.Drawing.Color.FromArgb(17, 37, 82) }
            ,new VehicleColorLookup(76,"Util Midnight Blue","Util","Midnight Blue",108) { RGBColor = System.Drawing.Color.FromArgb(27, 32, 62) }
            ,new VehicleColorLookup(77,"Util Blue","Util","Blue",109) { RGBColor = System.Drawing.Color.FromArgb(39, 81, 144) }
            ,new VehicleColorLookup(78,"Util Sea Foam Blue","Util","Sea Foam Blue",110) { RGBColor = System.Drawing.Color.FromArgb(96, 133, 146) }
            ,new VehicleColorLookup(79,"Util Lightning blue","Util","Lightning blue",111) { RGBColor = System.Drawing.Color.FromArgb(36, 70, 168) }
            ,new VehicleColorLookup(80,"Util Maui Blue Poly","Util","Maui Blue Poly",112) { RGBColor = System.Drawing.Color.FromArgb(66, 113, 225) }
            ,new VehicleColorLookup(81,"Util Bright Blue","Util","Bright Blue",113) { RGBColor = System.Drawing.Color.FromArgb(59, 57, 224) }
            ,new VehicleColorLookup(108,"Util Brown","Util","Brown",114) { RGBColor = System.Drawing.Color.FromArgb(58, 42, 27) }
            ,new VehicleColorLookup(109,"Util Medium Brown","Util","Medium Brown",115) { RGBColor = System.Drawing.Color.FromArgb(120, 95, 51) }
            ,new VehicleColorLookup(110,"Util Light Brown","Util","Light Brown",116) { RGBColor = System.Drawing.Color.FromArgb(181, 160, 121) }
            ,new VehicleColorLookup(122,"Util Off White","Util","Off White",117) { RGBColor = System.Drawing.Color.FromArgb(223, 221, 208) }

            ,new VehicleColorLookup(21,"Worn Black","Worn","Black",118) { RGBColor = System.Drawing.Color.FromArgb(30, 35, 47) }
            ,new VehicleColorLookup(22,"Worn Graphite","Worn","Graphite",119) { RGBColor = System.Drawing.Color.FromArgb(54, 58, 63) }
            ,new VehicleColorLookup(23,"Worn Silver Grey","Worn","Silver Grey",120) { RGBColor = System.Drawing.Color.FromArgb(160, 161, 153) }
            ,new VehicleColorLookup(24,"Worn Silver","Worn","Silver",121) { RGBColor = System.Drawing.Color.FromArgb(211, 211, 211) }
            ,new VehicleColorLookup(25,"Worn Blue Silver","Worn","Blue Silver",122) { RGBColor = System.Drawing.Color.FromArgb(183, 191, 202) }
            ,new VehicleColorLookup(26,"Worn Shadow Silver","Worn","Shadow Silver",123) { RGBColor = System.Drawing.Color.FromArgb(119, 135, 148) }
            ,new VehicleColorLookup(46,"Worn Red","Worn","Red",124) { RGBColor = System.Drawing.Color.FromArgb(169, 71, 68) }
            ,new VehicleColorLookup(47,"Worn Golden Red","Worn","Golden Red",125) { RGBColor = System.Drawing.Color.FromArgb(177, 108, 81) }
            ,new VehicleColorLookup(48,"Worn Dark Red","Worn","Dark Red",126) { RGBColor = System.Drawing.Color.FromArgb(55, 28, 37) }
            ,new VehicleColorLookup(58,"Worn Dark Green","Worn","Dark Green",127) { RGBColor = System.Drawing.Color.FromArgb(45, 66, 63) }
            ,new VehicleColorLookup(59,"Worn Green","Worn","Green",128) { RGBColor = System.Drawing.Color.FromArgb(69, 89, 75) }
            ,new VehicleColorLookup(60,"Worn Sea Wash","Worn","Sea Wash",129) { RGBColor = System.Drawing.Color.FromArgb(101, 134, 127) }
            ,new VehicleColorLookup(85,"Worn Dark blue","Worn","Dark blue",130) { RGBColor = System.Drawing.Color.FromArgb(76, 95, 129) }
            ,new VehicleColorLookup(86,"Worn Blue","Worn","Blue",131) { RGBColor = System.Drawing.Color.FromArgb(88, 104, 142) }
            ,new VehicleColorLookup(87,"Worn Light blue","Worn","Light blue",132) { RGBColor = System.Drawing.Color.FromArgb(116, 181, 216) }
            ,new VehicleColorLookup(113,"Worn Honey Beige","Worn","Honey Beige",133) { RGBColor = System.Drawing.Color.FromArgb(176, 171, 148) }
            ,new VehicleColorLookup(114,"Worn Brown","Worn","Brown",134) { RGBColor = System.Drawing.Color.FromArgb(69, 56, 49) }
            ,new VehicleColorLookup(115,"Worn Dark Brown","Worn","Dark Brown",135) { RGBColor = System.Drawing.Color.FromArgb(42, 40, 43) }
            ,new VehicleColorLookup(116,"Worn straw beige","Worn","straw beige",136) { RGBColor = System.Drawing.Color.FromArgb(114, 108, 87) }
            ,new VehicleColorLookup(121,"Worn Off White","Worn","Off White",137) { RGBColor = System.Drawing.Color.FromArgb(234, 230, 222) }
            ,new VehicleColorLookup(123,"Worn Orange","Worn","Orange",138) { RGBColor = System.Drawing.Color.FromArgb(242, 173, 46) }
            ,new VehicleColorLookup(124,"Worn Light Orange","Worn","Light Orange",139) { RGBColor = System.Drawing.Color.FromArgb(249, 164, 88) }
            ,new VehicleColorLookup(126,"Worn Taxi Yellow","Worn","Taxi Yellow",140) { RGBColor = System.Drawing.Color.FromArgb(241, 204, 64) }
            ,new VehicleColorLookup(130,"Worn Orange","Worn","Orange",141) { RGBColor = System.Drawing.Color.FromArgb(248, 182, 88) }
            ,new VehicleColorLookup(132,"Worn White","Worn","White",142) { RGBColor = System.Drawing.Color.FromArgb(255, 255, 251) }
            ,new VehicleColorLookup(133,"Worn Olive Army Green","Worn","Olive Army Green",143) { RGBColor = System.Drawing.Color.FromArgb(129, 132, 76) }

            ,new VehicleColorLookup(134,"Pure White","Standard","Pure White",150) { RGBColor = System.Drawing.Color.FromArgb(255, 255, 255) }
            ,new VehicleColorLookup(135,"Hot Pink","Standard","Hot Pink",150) { RGBColor = System.Drawing.Color.FromArgb(242, 31, 153) }
            ,new VehicleColorLookup(136,"Salmon pink","Standard","Salmon Pink",150) { RGBColor = System.Drawing.Color.FromArgb(253, 214, 205) }
            ,new VehicleColorLookup(138,"Orange","Standard","Orange",150) { RGBColor = System.Drawing.Color.FromArgb(246, 174, 32) }
            ,new VehicleColorLookup(139,"Green","Standard","Green",150) { RGBColor = System.Drawing.Color.FromArgb(176, 238, 110) }
            ,new VehicleColorLookup(140,"Blue","Standard","Blue",150) { RGBColor = System.Drawing.Color.FromArgb(8, 233, 250) }
            ,new VehicleColorLookup(156,"DEFAULT ALLOY COLOR","Standard","DEFAULT ALLOY COLOR",150)

            ,new VehicleColorLookup(120,"Chrome","Chrome","Chrome",199) { RGBColor = System.Drawing.Color.FromArgb(88, 112, 161) }

            ,new VehicleColorLookup(117,"Brushed Steel","Metals","Brushed Steel",200) { RGBColor = System.Drawing.Color.FromArgb(106, 116, 124) }
            ,new VehicleColorLookup(118,"Brushed Black Steel","Metals","Brushed Black Steel",201) { RGBColor = System.Drawing.Color.FromArgb(53, 65, 88) }
            ,new VehicleColorLookup(119,"Brushed Aluminium","Metals","Brushed Aluminium",202) { RGBColor = System.Drawing.Color.FromArgb(155, 160, 168) }
            ,new VehicleColorLookup(158,"Pure Gold","Metals","Pure Gold",203) { RGBColor = System.Drawing.Color.FromArgb(122, 100, 64) }
            ,new VehicleColorLookup(159,"Brushed Gold","Metals","Brushed Gold",204) { RGBColor = System.Drawing.Color.FromArgb(127, 106, 72) }



            //,new ColorLookup(127,"police car blue","Unknown","police car blue",205)
            //
            //,new ColorLookup(157,"Epsilon Blue","Unknown","Epsilon Blue",207)
            //,new ColorLookup(144,"hunter green","Unknown","hunter green",208)
            //,new ColorLookup(147,"MODSHOP BLACK1","Unknown","MODSHOP BLACK1",209)

        };
    }
    public void ProcessTransactionMenu()
    {
        while (MenuPool.IsAnyMenuOpen() || IsShowingConfirmDialog)
        {
            MenuPool.ProcessMenus();
            Update();
            GameFiber.Yield();
        }
    }
    public void Update()
    {
        if (MenuPool.IsAnyMenuOpen())
        {
            if (RotatePreview)
            {

                if (SellingProp.Exists())
                {
                    SellingProp.SetRotationYaw(SellingProp.Rotation.Yaw + 1f);
                }
                if (SellingVehicle.Exists() && RotateVehiclePreview)
                {
                    SellingVehicle.SetRotationYaw(SellingVehicle.Rotation.Yaw + 1f);
                }
            }
        }
        else
        {
            ClearPreviews();
        }
    }
    public void DisposeTransactionMenu()
    {
        PurchaseMenu?.Dispose();
        SellMenu?.Dispose();
    }
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
    }
    public void OnAmountChanged(ModItem modItem)
    {
        //PurchaseMenu?.OnAmountChanged(modItem);
        //SellMenu?.OnAmountChanged(modItem);
    }
    public void OnItemPurchased(ModItem modItem, MenuItem menuItem, int TotalItems)
    {
        if (PersonTransaction != null)
        {
            PersonTransaction.OnItemPurchased(modItem, menuItem, TotalItems);//shows message too, but in the middle of the animation
        }
        else if (Store != null)
        {
            Store.OnItemPurchased(modItem, menuItem, TotalItems);
            DisplayItemPurchasedMessage(modItem, TotalItems);
        }
        menuItem.NumberOfItemsSoldToPlayer += TotalItems;
        SellMenu?.OnItemPurchased(menuItem);
    }
    public void OnItemSold(ModItem modItem, MenuItem menuItem, int TotalItems)
    {
        if (PersonTransaction != null)
        {
            PersonTransaction.OnItemSold(modItem, menuItem, TotalItems);
        }
        else if(Store != null)
        {
            Store.OnItemSold(modItem, menuItem, TotalItems);
            DisplayItemSoldMessage(modItem, TotalItems);
        }
        menuItem.NumberOfItemsPurchasedByPlayer += TotalItems;
        PurchaseMenu?.OnItemSold(menuItem); 
    }
    public void DisplayInsufficientFundsMessage()
    {
        PlayErrorSound();
        Game.RemoveNotification(NotificationHandle);
        string header = "~r~Insufficient Funds";
        string message = "We are sorry, we are unable to complete this transaction.";
        if (!UseAccounts)
        {
            header = "~r~Cash Only";
            message = "You do not have enough cash on hand.";
        }
        NotificationHandle = Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", Store?.Name, header, message);
    }
    public void DisplayItemSoldMessage(ModItem modItem, int TotalItems)
    {
        if(modItem == null)
        {
            return;
        }
        PlaySuccessSound();
        string Header = "~g~Sale";
        string Message = $"You have sold {TotalItems} ~r~{modItem.Name}(s)~s~";
        if (modItem.MeasurementName != "Item")
        {
            Message = $"You have sold {TotalItems} {modItem.MeasurementName}(s) of ~r~{modItem.Name}~s~";
        }
        DisplayMessage(Header, Message);   
    }
    public void DisplayItemPurchasedMessage(ModItem modItem, int TotalItems)
    {
        if (modItem == null)
        {
            return;
        }
        PlaySuccessSound();
        string Header = "~g~Purchase";
        string Message = $"You have purchased {TotalItems} ~r~{modItem.Name}(s)~s~";
        if (modItem.MeasurementName != "Item")
        {
            Message = $"You have purchased {TotalItems} {modItem.MeasurementName}(s) of ~r~{modItem.Name}~s~";
        }    
        if(!IsPurchasing)
        {
            Header = "~g~Acquired";
            Message = $"You have acquired {TotalItems} ~r~{modItem.Name}(s)~s~";
            if (modItem.MeasurementName != "Item")
            {
                Message = $"You have acquired {TotalItems} {modItem.MeasurementName}(s) of ~r~{modItem.Name}~s~";
            }
        }
        DisplayMessage(Header, Message);
    }
    public void PlayErrorSound()
    {
        NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "ERROR", "HUD_LIQUOR_STORE_SOUNDSET", 0);
    }
    public void PlaySuccessSound()
    {
        NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "PURCHASE", "HUD_LIQUOR_STORE_SOUNDSET", 0);
    }
    public void DisplayMessage(string header, string message)
    {
        Game.RemoveNotification(NotificationHandle);
        if (Store == null)
        {
            NotificationHandle = Game.DisplayNotification(message);
        }
        else
        {
            NotificationHandle = Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", Store?.Name, header, message);
        }
    }
}
