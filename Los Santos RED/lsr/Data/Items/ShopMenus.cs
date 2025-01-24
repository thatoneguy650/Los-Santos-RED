using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using NAudio.Gui;
using Rage;
using Rage.Native;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.LinkLabel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ExplorerBar;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;


public class ShopMenus : IShopMenus
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\ShopMenus.xml";
    public ShopMenuTypes PossibleShopMenus { get; private set; }
    public ShopMenus()
    {
        PossibleShopMenus = new ShopMenuTypes();
    }
    public void ReadConfig()
    {
        DirectoryInfo LSRDirectory = new DirectoryInfo("Plugins\\LosSantosRED");
        FileInfo ConfigFile = LSRDirectory.GetFiles("ShopMenus*.xml").OrderByDescending(x => x.Name).FirstOrDefault();
        if (ConfigFile != null)
        {
            EntryPoint.WriteToConsole($"Loaded Shop Menus config  {ConfigFile.FullName}",0);
            PossibleShopMenus = Serialization.DeserializeParam<ShopMenuTypes>(ConfigFile.FullName);
        }
        else if (File.Exists(ConfigFileName))
        {
            EntryPoint.WriteToConsole($"Loaded Shop Menus config  {ConfigFileName}",0);
            PossibleShopMenus = Serialization.DeserializeParam<ShopMenuTypes>(ConfigFileName);
        }
        else
        {
            EntryPoint.WriteToConsole($"No Shop Menus config found, creating default", 0);
            DefaultConfig();
            DefaultConfig_LosSantos2008();
            DefaultConfig_FullModernTraffic();
            DefaultConfig_FullExpandedExperience();
        }
    }

    private void DefaultConfig_LosSantos2008()
    {
        ShopMenuTypes oldPossibleShopMenus = PossibleShopMenus.Copy();
        List<string> toRemoveMenus = new List<string>() { "BenefactorGallavanterMenu", "VapidMenu", "LuxuryAutosMenu", "PremiumDeluxeMenu", "AlbanyMenu", "SunshineMenu", "NationalMenu", "PaletoExportMenu" };
        oldPossibleShopMenus.ShopMenuList.RemoveAll(x => toRemoveMenus.Contains(x.ID));
        oldPossibleShopMenus.ShopMenuList.AddRange(new List<ShopMenu>{
            new ShopMenu("BenefactorGallavanterMenu","Benefactor/Gallavanter",new List<MenuItem>() {       
                new MenuItem("Gallivanter Baller",67000,45000),
                new MenuItem("Benefactor Schafter",65000,34000),
               // new MenuItem("Benefactor Schafter (IV)",75000,52000),
                //new MenuItem("Benefactor Schafter GTR (IV)",112000,81000),
                new MenuItem("Benefactor Feltzer",145000,90500),
                //new MenuItem("Benefactor Feltzer (IV)",145000,90500),
                new MenuItem("Benefactor Serrano",60000,45000),
                new MenuItem("Benefactor Dubsta",110000,78000),
            }),
            new ShopMenu("VapidMenu","Vapid",new List<MenuItem>() {
                new MenuItem("Vapid Stanier",28000, 12000),
               // new MenuItem("Vapid Stanier (IV)",29000, 13000),
                new MenuItem("Vapid Uranus",105000,82000),
               // new MenuItem("Vapid Huntley Sport (IV)",35000,25000),
                new MenuItem("Vapid Fortune",15000,10000),
              //  new MenuItem("Vapid Coquette (IV)",56000,35000),
              //  new MenuItem("Vapid Contender",34000,28000),
                new MenuItem("Vapid Bobcat XL",31000,25000),
                new MenuItem("Vapid Minivan",29000, 12500),
                new MenuItem("Vapid Minivan Custom",30500,13000),
                new MenuItem("Vapid Speedo",31000,13500),
                new MenuItem("Vapid Speedo Custom",31500,14000),
                new MenuItem("Vapid Sadler",38000,15500),
                new MenuItem("Vapid Dominator",55000,33000),
                new MenuItem("Vapid Bullet",155000,105050),



            }),
            new ShopMenu("LuxuryAutosMenu","Luxury Autos",new List<MenuItem>() {//pegassi/grotti/enus/buckingham/pfiuster
                //new MenuItem("Schyster PMP 600",37000,28000),
               // new MenuItem("Declasse Premier (IV)",32000,27000),
               // new MenuItem("Albany Presidente (IV)",37000,31000),
               // new MenuItem("Albany Presidente 2 (IV)",39000,32000),
                //new MenuItem("Ubermacht Sentinel (IV)",35000,30000),
               // new MenuItem("Annis Pinnacle",45000,38000),
               // new MenuItem("Enus Super Diamond (IV)",655000,467000),
                //new MenuItem("Grotti Blade",28000,21000),
               // new MenuItem("Grotti Cheetah Classic",334500,259000),
               // new MenuItem("Grotti Cheetah (IV)",255000,167000),
              //  new MenuItem("Grotti Turismo (IV)",150000,86000),
                new MenuItem("Pfister Comet",100000,78000),
                new MenuItem("Pfister Comet Retro Custom",130000,65000),
                new MenuItem("Pegassi Bati 801",15000,7500),
                new MenuItem("Pegassi Bati 801RR",16000,7000),
            }),
            new ShopMenu("PremiumDeluxeMenu","PremiumDeluxe",new List<MenuItem>() {
                //new MenuItem("Dundreary Admiral",29000, 18000),
               // new MenuItem("Schyster Cabby",30000, 22000),
               // new MenuItem("Bravado Feroci",30500,21000),
                //new MenuItem("Emperor Lokus",31000, 25000),
                new MenuItem("Bravado Gauntlet",32000,28000),
                new MenuItem("Bravado Buffalo",35000,27000),
                new MenuItem("Declasse Merit",29000,21000),
                new MenuItem("Bravado Banshee",105000,78000),
                //new MenuItem("Dinka Perennial",15000,10000),

                new MenuItem("Karin Futo",12000,8000),
                new MenuItem("Karin Rebel",19000,9500),
                new MenuItem("Karin BeeJay XL",29000,17000),
                new MenuItem("Karin Dilettante",35000,18000),
                new MenuItem("Karin Asterope",36500,22000),
                new MenuItem("Karin Sultan Classic",36750,21500),
                new MenuItem("Karin Sultan",37000,28000),
                new MenuItem("Karin Sultan RS",38000,32000),
                new MenuItem("Karin Intruder",38000,32000),
                new MenuItem("Karin Previon",39000,34500),
                //new MenuItem("Karin Everon",44000,35000),
               // new MenuItem("Declasse Vigero (IV)",32000,28000),
                //new MenuItem("Maibatsu Vincent (IV)",22000,15000),
            }),
            new ShopMenu("AlbanyMenu","Albany",new List<MenuItem>() {

                new MenuItem("Albany Esperanto",25000,17500),


                //new MenuItem("Albany Emperor (IV)",25000,17500),
                new MenuItem("Albany Buccaneer",29000,19500),
                new MenuItem("Albany Buccaneer Custom",49000,29500),
                new MenuItem("Albany Cavalcade",45000,27500),
                //new MenuItem("Albany Cavalcade FXT",50000,25000),
                new MenuItem("Albany Emperor",25000,17500),
                new MenuItem("Albany Manana",28000,19500),
                new MenuItem("Albany Manana Custom",42400,21200),
                new MenuItem("Albany Primo",35000,17500),
                new MenuItem("Albany Primo Custom",55000,27500),
                new MenuItem("Albany Virgo",36000,18000),
                //new MenuItem("Albany Esperanto (IV)",22000,15000),
                new MenuItem("Albany Washington",48000,19000),
            }),
        });

        oldPossibleShopMenus.ShopMenuList.AddRange(new List<ShopMenu>{
        new ShopMenu("SunshineMenu", "Sunshine", new List<MenuItem>() {
                new MenuItem("Vapid Dominator",55000,10000),
                new MenuItem("Albany Cavalcade",0,9500),
                new MenuItem("Albany Washington",0,5000),
                new MenuItem("Bravado Buffalo",0,7000),
                new MenuItem("Bravado Banshee",0,25000),
                new MenuItem("Karin BeeJay XL",0,5500),
                new MenuItem("Benefactor Serrano",0,4500),
                new MenuItem("Benefactor Dubsta",0,21000),
                new MenuItem("Gallivanter Baller",0,17000),
                new MenuItem("Pfister Comet",0,27000),

                //new MenuItem("Albany Emperor (IV)",0,17500),
                //new MenuItem("Albany Cavalcade FXT",0,10000),
                //new MenuItem("Bravado Feroci",0,11000),
               // new MenuItem("Emperor Lokus",0, 12000),
               // new MenuItem("Schyster PMP 600",0,13000),
               // new MenuItem("Declasse Premier (IV)",0,12500),
               // new MenuItem("Albany Presidente (IV)",0,15000),
               // new MenuItem("Vapid Stanier (IV)",0, 10000),
              //  new MenuItem("Vapid Uranus",0,8500),
            }),

        new ShopMenu("NationalMenu", "National", new List<MenuItem>() {
                new MenuItem("Karin Sultan",0,5600),
                new MenuItem("Bravado Gauntlet",0,8600),
                new MenuItem("Bravado Buffalo",0,10500),
                new MenuItem("Bravado Banshee",0,22000),
                new MenuItem("Vapid Bullet",0,43000),
                new MenuItem("Gallivanter Baller",0,16000),
                new MenuItem("Benefactor Schafter",0,6800),
                new MenuItem("Benefactor Feltzer",0,19000),  
                
                //new MenuItem("Albany Virgo",0,5000),
                //new MenuItem("Declasse Vigero (IV)",0,12000),
                //new MenuItem("Dundreary Admiral",0, 8000),
                //new MenuItem("Schyster Cabby",0, 4000),
                //new MenuItem("Enus Super Diamond (IV)",0,12000),
                //new MenuItem("Grotti Cheetah Classic",0,15000),
                //new MenuItem("Grotti Cheetah (IV)",0,16500),
                //new MenuItem("Grotti Turismo (IV)",0,14500),
                //new MenuItem("Vapid Huntley Sport (IV)",0,18500),
                //new MenuItem("Vapid Fortune",0,4500),
            }),

        new ShopMenu("PaletoExportMenu", "Paleto Exports", new List<MenuItem>() {
                new MenuItem("Bravado Gauntlet",0,7800),
                new MenuItem("Bravado Buffalo",0,8900),
                new MenuItem("Karin Futo",0,2000),
                new MenuItem("Karin Rebel",0,5600),
                new MenuItem("Karin BeeJay XL",0,2800),
                new MenuItem("Karin Dilettante",0,2400),
                new MenuItem("Karin Asterope",0,3400),
                new MenuItem("Vapid Stanier",0, 3400),
                new MenuItem("Vapid Minivan",0, 2500),
                new MenuItem("Benefactor Schwartzer",0,4000),
                new MenuItem("BF Surfer",0, 1500),
                new MenuItem("BF Injection",0,2000),

                //new MenuItem("Albany Emperor (IV)",0,4000),
                //new MenuItem("Albany Esperanto (IV)",0,3500),
                //new MenuItem("Maibatsu Vincent (IV)",0,15000),
                //new MenuItem("Albany Presidente 2 (IV)",0,15500),
                //new MenuItem("Ubermacht Sentinel (IV)",0,14500),
                //new MenuItem("Annis Pinnacle",0,13000),
                //new MenuItem("Vapid Coquette (IV)",0,19000),
                //new MenuItem("Vapid Contender",0,6700),
                //new MenuItem("Vapid Bobcat",0,4500),
            }),

        });
        Serialization.SerializeParam(oldPossibleShopMenus, "Plugins\\LosSantosRED\\AlternateConfigs\\LosSantos2008\\ShopMenus_LosSantos2008.xml");
    }
    private void DefaultConfig_FullModernTraffic()
    {
        ShopMenuTypes fejPossibleShopMenus = PossibleShopMenus.Copy();
        List<string> toRemoveModItems = new List<string>() { "Weeny Issi", "Declasse Tornado 3" };
        foreach(ShopMenu shopMenu in fejPossibleShopMenus.ShopMenuList)
        {
            shopMenu.Items.RemoveAll(x => toRemoveModItems.Contains(x.ModItemName));
        }
        ShopMenu vapidMenu = fejPossibleShopMenus.ShopMenuList.Where(x => x.ID == "VapidMenu").FirstOrDefault();
        if(vapidMenu != null)
        {
            vapidMenu.Items.Add(new MenuItem("Vapid Interceptor", 35000, 15000));
            vapidMenu.Items.Add(new MenuItem("Vapid Stanier 2nd Gen", 25000, 12000));
            vapidMenu.Items.Add(new MenuItem("Vapid Caracara Service", 32000, 15000));
            vapidMenu.Items.Add(new MenuItem("Vapid Caracara 4x2", 30000, 14000));
            vapidMenu.Items.Add(new MenuItem("Vapid Bobcat 4x4", 22000, 5000));
            vapidMenu.Items.Add(new MenuItem("Vapid Bobcat Regular Bed", 12000, 4000));
        }
        //KarinMenu
        ShopMenu karinMenu = fejPossibleShopMenus.ShopMenuList.Where(x => x.ID == "KarinMenu").FirstOrDefault();
        if (karinMenu != null)
        {
            karinMenu.Items.Add(new MenuItem("Karin Everon V8", 58000, 25500));
        }
        ShopMenu albanyMenu = fejPossibleShopMenus.ShopMenuList.Where(x => x.ID == "AlbanyMenu").FirstOrDefault();
        if (albanyMenu != null)
        {
            albanyMenu.Items.Add(new MenuItem("Albany Esperanto", 22000, 10000));
        }
        ShopMenu premiumDeluxMenu = fejPossibleShopMenus.ShopMenuList.Where(x => x.ID == "PremiumDeluxeMenu").FirstOrDefault();
        if (premiumDeluxMenu != null)
        {
            premiumDeluxMenu.Items.Add(new MenuItem("Declasse Merit", 23000, 7800));
            premiumDeluxMenu.Items.Add(new MenuItem("Karin Everon V8", 58000, 25500));

            premiumDeluxMenu.Items.Add(new MenuItem("Albany Presidente", 26000, 12000));
            premiumDeluxMenu.Items.Add(new MenuItem("Schyster PMP 600", 36000, 17000));
            premiumDeluxMenu.Items.Add(new MenuItem("Canis Bodhi Mod", 28000, 14500));

        }
        ShopMenu elitasMenu = fejPossibleShopMenus.ShopMenuList.Where(x => x.ID == "ElitasMenu").FirstOrDefault();
        if (elitasMenu != null)
        {
            elitasMenu.Items.Add(new MenuItem("Buckingham Maverick 2nd Gen", 1800000));
        }
        foreach(ShopMenu menu in fejPossibleShopMenus.ShopMenuList)//swap some model names over
        {
            if (menu.ID != "KarinMenu")
            {
                foreach (MenuItem mi in menu.Items)
                {
                    if (mi.ModItemName == "Karin Kuruma")
                    {
                        mi.ModItemName = "Maibatsu Kuruma";
                    }
                }
            }
            menu.Items.RemoveAll(x => x.ModItemName == "Vapid Contender" || x.ModItemName == "Karin Kuruma");
        }
        Serialization.SerializeParam(fejPossibleShopMenus, "Plugins\\LosSantosRED\\AlternateConfigs\\FullModernTraffic\\Variations\\Full Modern Traffic\\ShopMenus_FullModernTraffic.xml");
    }
    private void DefaultConfig_FullExpandedExperience()
    {
        ShopMenuTypes fejPossibleShopMenus = PossibleShopMenus.Copy();
        List<string> beerMenus = new List<string>() { "ConvenienceStoreMenu","LiquorStoreMenu","PizzaMenu","GasStationMenu","DeliGroceryMenu","ItalianMenu","PizzaThisMenu","AlDentesMenu","TwentyFourSevenMenu","FruitVineMenu","RonMenu","XeroMenu","LTDMenu","BarMenu" };
        foreach (ShopMenu shopMenu in fejPossibleShopMenus.ShopMenuList.Where(x=> beerMenus.Contains(x.ID)))
        {
            shopMenu.Items.Add(new MenuItem("Can of PiBwasser", 3));
            shopMenu.Items.Add(new MenuItem("Can of PiBwasser ICE", 3));
        }
        //Serialization.SerializeParam(fejPossibleShopMenus, "Plugins\\LosSantosRED\\AlternateConfigs\\FullExpandedExperience\\ShopMenus_FullExpandedExperience.xml");
    }
    public ShopMenu GetSpecificMenu(string menuID)
    {
        return PossibleShopMenus.ShopMenuList.Where(x => x.ID == menuID).FirstOrDefault();// ShopMenuList.Where(x => x.ID == menuID).FirstOrDefault()?.Copy();
    }
    public ShopMenu GetWeightedRandomMenuFromGroup(string groupID)
    {
        return PossibleShopMenus.ShopMenuGroupList.Where(x => x.ID == groupID).FirstOrDefault()?.GetRandomMenu();
    }
    public ShopMenuGroup GetSpecificMenuGroup(string groupID)
    {
        return PossibleShopMenus.ShopMenuGroupList.Where(x => x.ID == groupID).FirstOrDefault();
    }
    public ShopMenu GetWeightedRandomMenuFromContainer(string containerID)
    {
       // EntryPoint.WriteToConsole($"GetWeightedRandomMenuFromOverallGroup {containerID}");
        ShopMenuGroupContainer smgc = PossibleShopMenus.ShopMenuGroupContainers.Where(x => x.ID == containerID).FirstOrDefault();
        if(smgc == null)
        {
            //EntryPoint.WriteToConsole($"GetWeightedRandomMenuFromOverallGroup NO GROUP CONTAINER FOUND");
            return null;
        }
        string weightedGroupID = smgc.GetRandomWeightedShopMenuGroupID();
        //EntryPoint.WriteToConsole($"GetWeightedRandomMenuFromOverallGroup GROUP CONTAINER FOUND {smgc.ID} {smgc.Name} weightedGroupID {weightedGroupID}");
        return GetWeightedRandomMenuFromGroup(weightedGroupID);
    }
    public ShopMenu GetRandomDrugCustomerMenu()
    {
        return PossibleShopMenus.ShopMenuGroupList.Where(x => x.CategoryID == StaticStrings.DrugCustomerMenuID).PickRandom()?.GetRandomMenu();
    }
    public ShopMenuGroupContainer GetSpecificGroupContainer(string containerID)
    {
        if(string.IsNullOrEmpty(containerID) || containerID == "")
        {
            return null;
        }
        return PossibleShopMenus.ShopMenuGroupContainers.Where(x => x.ID == containerID).FirstOrDefault();
    }
    public Tuple<int, int> GetPrices(string itemName)
    {
        int LowestPrice = 9999;
        int HighestPrice = 0;
        List<ShopMenu> AllShopMenus = AllMenus();
        foreach (ShopMenu shopMenu in AllShopMenus)
        {
            foreach (MenuItem menuItem in shopMenu.Items)
            {
                if (menuItem.Purchaseable && menuItem.ModItemName == itemName)
                {
                    if (menuItem.PurchasePrice < LowestPrice)
                    {
                        LowestPrice = menuItem.PurchasePrice;
                    }
                    if (menuItem.PurchasePrice > HighestPrice)
                    {
                        HighestPrice = menuItem.PurchasePrice;
                    }
                }
            }
        }
        return new Tuple<int, int>(LowestPrice, HighestPrice);
    }
    private List<ShopMenu> AllMenus()
    {
        List<ShopMenu> AllShopMenus = new List<ShopMenu>();
        foreach (ShopMenuGroup shopMenuGroup in PossibleShopMenus.ShopMenuGroupList)
        {
            foreach (PercentageSelectShopMenu dispatchableShopMenu in shopMenuGroup.PossibleShopMenus)
            {
                AllShopMenus.Add(dispatchableShopMenu.ShopMenu);
            }
        }
        AllShopMenus.AddRange(PossibleShopMenus.ShopMenuList);
        return AllShopMenus;
    }
    public ShopMenu GetVendingMenu(string propName)
    {
        string MenuID = "VendingMenu";
        PropShopMenu propShopMenu = null;
        if (NativeHelper.IsStringHash(propName, out uint modelHash))
        {
            propShopMenu = PossibleShopMenus.PropShopMenus.Where(x => x.ModelHash == modelHash).FirstOrDefault();
        }
        else
        {
            propShopMenu = PossibleShopMenus.PropShopMenus.Where(x => x.ModelName == propName).FirstOrDefault();
        }
        if(propShopMenu != null)
        {
            MenuID = propShopMenu.ShopMenuID;
        }
        return GetSpecificMenu(MenuID);
    }


    public PedVariationShopMenu GetPedVariationMenu(string pedVariationShopMenuID)
    {
        return PossibleShopMenus.PedVariationShopMenus.FirstOrDefault(x => x.ID == pedVariationShopMenuID);
        //public PedVariationShopMenu PedVariationShopMenu
    }


    private void DefaultConfig()
    {
        SetupPropMenus();
        GenericLocationsMenu();
        GenericPawnShopMenu();
        SpecificVendingMachines();
        SpecificRestaurants();
        SpecificConvenienceStores();
        SpecificHotels();
        SpecificSportingGoods();
        SpecificDealerships();
        SpecificVehicleExporters();
        SpecificWeaponsShops();
        DrugDealerMenus();
        DenList();
        GunShopList();
        MenuGroupList();
        DealerHangouts();
        SetupTreatments();
        SetupPedVariationMenus();
        Serialization.SerializeParam(PossibleShopMenus, ConfigFileName);
    }
    private void SetupPedVariationMenus()
    {
        PedVariationShopMenu pedVariationShopMenu = new PedVariationShopMenu();
        pedVariationShopMenu.ID = "GenericBarberShop";
        pedVariationShopMenu.PedVariationComponentMenu = new List<PedComponentShopMenu>()
        {
            new PedComponentShopMenu("player_one","Fade",2,0,0,25),
            new PedComponentShopMenu("player_one","Triple Rails",2,0,1,28),
            new PedComponentShopMenu("player_one","Wavy Siderows",2,0,2,30),
            new PedComponentShopMenu("player_one","Snakes",2,0,3,28),
            new PedComponentShopMenu("player_one","Tramlines",2,0,4,25),
            new PedComponentShopMenu("player_one","Star Kutz",2,0,5,35),
            new PedComponentShopMenu("player_one","Shutters",2,0,6,38),
            new PedComponentShopMenu("player_one","Berms",2,0,7,40),
            new PedComponentShopMenu("player_one","Mellowplex",2,0,8,45),
            new PedComponentShopMenu("player_one",2,0,9,25),
            new PedComponentShopMenu("player_one",2,0,10,35),
            new PedComponentShopMenu("player_one",2,0,11,33),
            new PedComponentShopMenu("player_one",2,0,12,30),
            new PedComponentShopMenu("player_one",2,0,13,23),
            new PedComponentShopMenu("player_one",2,0,14,45),
            new PedComponentShopMenu("player_one",2,0,15,22),
            new PedComponentShopMenu("player_one","The Feud",2,1,0,50),
            new PedComponentShopMenu("player_one","Lo Fro",2,2,0,95),
            new PedComponentShopMenu("player_one","Corn Rows",2,3,0,150),
            new PedComponentShopMenu("player_one","Shape Up",2,4,0,20),
        };
        PossibleShopMenus.PedVariationShopMenus.Add(pedVariationShopMenu);
    }
    private void SetupTreatments()
    {
        List<MedicalTreatment> DefaultMedicalTreatments = new List<MedicalTreatment>()
        {
            new MedicalTreatment("Regular Doctor Visit","One of our less qualified doctors will surely be able to help you out.",50,500),
            new MedicalTreatment("Decent Doctor Visit","Look at Mr. Rockefeller, shelling out for a ~r~real~s~ doctor.",75,750),
            new MedicalTreatment("Full Body Treatment","Our crack team will scan, poke, and prod you until you are like new!",100,1000),
        };
        PossibleShopMenus.TreatmentOptionsList.Add(new TreatmentOptions("DefaultMedicalTreatments", "DefaultMedicalTreatments", DefaultMedicalTreatments));
    }
    public List<MedicalTreatment> GetMedicalTreatments(string treatmentOptionsID)
    {
        return PossibleShopMenus.TreatmentOptionsList.Where(x=> x.ID == treatmentOptionsID).FirstOrDefault()?.MedicalTreatments;
    }
    private void SetupPropMenus()
    {
        PossibleShopMenus.PropShopMenus.AddRange(
        new List<PropShopMenu>() { 
            new PropShopMenu("prop_vend_snak_01", "CandyVendingMenu"),
            new PropShopMenu("prop_vend_water_01", "WaterVendingMenu"),
            new PropShopMenu(0x418f055a, "WaterVendingMenu"),
            new PropShopMenu("prop_vend_soda_01", "SprunkVendingMenu"),
            new PropShopMenu(0x426a547c, "SprunkVendingMenu"),
            new PropShopMenu("prop_vend_soda_02", "eColaVendingMenu"),
            new PropShopMenu(0x3b21c5e7, "eColaVendingMenu"),
            new PropShopMenu("prop_vend_coffe_01", "BeanMachineVendingMenu"),
            new PropShopMenu("prop_vend_fags_01", "CigVendingMenu"),
        });
    }
    private void GenericPawnShopMenu()
    {

        ShopMenu GenericPawnShopMenu = new ShopMenu("PawnShopMenuGeneric", "PawnShop", new List<MenuItem>() {
                //new MenuItem("GASH Black Umbrella", 50, 5),


                new VariablePriceMenuItem("GASH Black Umbrella", 25, 55, 1, 5) { NumberOfItemsToPurchaseFromPlayer = 1, NumberOfItemsToSellToPlayer = 1, },



                new VariablePriceMenuItem("TAG-HARD Flashlight", 110, 140, 5, 10) { NumberOfItemsToPurchaseFromPlayer = 1, NumberOfItemsToSellToPlayer = 1, },
                new VariablePriceMenuItem("Flint Handle Flashlight",90, 110, 15, 25) { NumberOfItemsToPurchaseFromPlayer = 1, NumberOfItemsToSellToPlayer = 1, },
                new VariablePriceMenuItem("G.E.S. Baseball Bat",95, 150,15, 25) { NumberOfItemsToPurchaseFromPlayer = 1, NumberOfItemsToSellToPlayer = 1, },
                new VariablePriceMenuItem("ProLaps Five Iron Golf Club",120, 190, 15,25) { NumberOfItemsToPurchaseFromPlayer = 1, NumberOfItemsToSellToPlayer = 1, },
                new VariablePriceMenuItem("Schmidt & Priss TL6 Scanner", 500,800,100, 150) { NumberOfItemsToPurchaseFromPlayer = 1, NumberOfItemsToSellToPlayer = 1, },
                new VariablePriceMenuItem("SCHEISS AS Binoculars", 500, 700,150, 200) { NumberOfItemsToPurchaseFromPlayer = 1, NumberOfItemsToSellToPlayer = 1, },
                new VariablePriceMenuItem("SCHEISS RP Binoculars", 900, 1500, 220, 400) { NumberOfItemsToPurchaseFromPlayer = 1, NumberOfItemsToSellToPlayer = 1, },
                new VariablePriceMenuItem("Schmidt & Priss RD4 Radar Detector", 300,500,25, 45) { NumberOfItemsToPurchaseFromPlayer = 1, NumberOfItemsToSellToPlayer = 1, },


                new VariablePriceMenuItem("Hawk & Little PTF092F",850, 1100, 100, 150) { NumberOfItemsToPurchaseFromPlayer = 1, NumberOfItemsToSellToPlayer = 1, },
                new VariablePriceMenuItem("Hawk & Little Thunder",745, 1250,110, 200) { NumberOfItemsToPurchaseFromPlayer = 1, NumberOfItemsToSellToPlayer = 1, },
                new VariablePriceMenuItem("Hawk & Little Combat Pistol",1200, 1500, 200, 300) { NumberOfItemsToPurchaseFromPlayer = 1, NumberOfItemsToSellToPlayer = 1, },
                new VariablePriceMenuItem("Hawk & Little Desert Slug",1500, 1900, 300, 500) { NumberOfItemsToPurchaseFromPlayer = 1, NumberOfItemsToSellToPlayer = 1, },
                new VariablePriceMenuItem("Hawk & Little 1919", 1200, 1600, 325, 350) { NumberOfItemsToPurchaseFromPlayer = 1, NumberOfItemsToSellToPlayer = 1, },
                new VariablePriceMenuItem("Hawk & Little Raging Mare",1500, 2300, 300, 450) { NumberOfItemsToPurchaseFromPlayer = 1, NumberOfItemsToSellToPlayer = 1, },
                new VariablePriceMenuItem("Hawk & Little Raging Mare Dx",1700, 2500, 375, 475) { NumberOfItemsToPurchaseFromPlayer = 1, NumberOfItemsToSellToPlayer = 1, },
                new VariablePriceMenuItem("Vom Feuer P69",855, 1050, 140, 170) { NumberOfItemsToPurchaseFromPlayer = 1, NumberOfItemsToSellToPlayer = 1, },
                new VariablePriceMenuItem("Shrewsbury S7",900, 1500, 220, 330) { NumberOfItemsToPurchaseFromPlayer = 1, NumberOfItemsToSellToPlayer = 1, },
                new VariablePriceMenuItem("Shrewsbury S7A",1200, 1750,325, 450) { NumberOfItemsToPurchaseFromPlayer = 1, NumberOfItemsToSellToPlayer = 1, },
                new VariablePriceMenuItem("Coil Tesla",950, 1400, 150, 250) { NumberOfItemsToPurchaseFromPlayer = 1, NumberOfItemsToSellToPlayer = 1, },
                new VariablePriceMenuItem("BS M1922",1400, 1900, 200, 250) { NumberOfItemsToPurchaseFromPlayer = 1, NumberOfItemsToSellToPlayer = 1, },

                new VariablePriceMenuItem("Flint Duct Tape", 5, 10,1,1) { NumberOfItemsToPurchaseFromPlayer = 1, NumberOfItemsToSellToPlayer = 5, },
                new VariablePriceMenuItem("Flint Multi-Bit Screwdriver", 35, 40, 4, 5) { NumberOfItemsToPurchaseFromPlayer = 1, NumberOfItemsToSellToPlayer = 5, },
                new VariablePriceMenuItem("Flint Rubber Mallet",45, 70, 10, 15){ NumberOfItemsToPurchaseFromPlayer = 1, NumberOfItemsToSellToPlayer = 5, },
                new VariablePriceMenuItem("Flint Hammer", 35, 60, 4, 5){ NumberOfItemsToPurchaseFromPlayer = 1, NumberOfItemsToSellToPlayer = 5, },
                new VariablePriceMenuItem("Flint Crowbar", 56, 85, 12, 20){ NumberOfItemsToPurchaseFromPlayer = 1, NumberOfItemsToSellToPlayer = 5, },
                new VariablePriceMenuItem("Flint Hatchet", 195, 290, 35, 50){ NumberOfItemsToPurchaseFromPlayer = 1, NumberOfItemsToSellToPlayer = 5, },
                new VariablePriceMenuItem("Vom Feuer Machete", 95, 150,25, 40){ NumberOfItemsToPurchaseFromPlayer = 1, NumberOfItemsToSellToPlayer = 5, },
                new VariablePriceMenuItem("Flint Pliers", 20, 45, 5, 10){ NumberOfItemsToPurchaseFromPlayer = 1, NumberOfItemsToSellToPlayer = 2, },
                new VariablePriceMenuItem("Flint Heavy Duty Pipe Wrench",30, 45, 15, 25){ NumberOfItemsToPurchaseFromPlayer = 1, NumberOfItemsToSellToPlayer = 2, },
                new VariablePriceMenuItem("Flint Shovel",95,  120, 20, 35){ NumberOfItemsToPurchaseFromPlayer = 1, NumberOfItemsToSellToPlayer = 2, },
                new VariablePriceMenuItem("Flint Cordless Drill", 98, 150, 18, 25){ NumberOfItemsToPurchaseFromPlayer = 1, NumberOfItemsToSellToPlayer = 2, },
                new VariablePriceMenuItem("Power Metal Cordless Drill",78,  110, 22, 30){ NumberOfItemsToPurchaseFromPlayer = 1, NumberOfItemsToSellToPlayer = 2, },

                new VariablePriceMenuItem("Power Metal Side Drill", 185,265,45,140){ NumberOfItemsToPurchaseFromPlayer = 1, NumberOfItemsToSellToPlayer = 2, },
                new VariablePriceMenuItem("Power Metal Custom Side Drill", 375,675,120,160){ NumberOfItemsToPurchaseFromPlayer = 1, NumberOfItemsToSellToPlayer = 2, },



                new VariablePriceMenuItem("Fake Gold Ring", 0,0,1,2){ NumberOfItemsToPurchaseFromPlayer = 5, NumberOfItemsToSellToPlayer = 5, },
                new VariablePriceMenuItem("Gold Ring", 0,0,85,155){ NumberOfItemsToPurchaseFromPlayer = 5, NumberOfItemsToSellToPlayer = 5, },
                new VariablePriceMenuItem("Fake Silver Ring", 0,0,1,1){ NumberOfItemsToPurchaseFromPlayer = 5, NumberOfItemsToSellToPlayer = 5, },
                new VariablePriceMenuItem("Silver Ring", 0,0,12,15){ NumberOfItemsToPurchaseFromPlayer = 5, NumberOfItemsToSellToPlayer = 5, },

                new VariablePriceMenuItem("iFruit Cellphone", 0,0,75, 95){ NumberOfItemsToPurchaseFromPlayer = 5, NumberOfItemsToSellToPlayer = 2, },
                new VariablePriceMenuItem("Facade Cellphone", 0,0,55, 75){ NumberOfItemsToPurchaseFromPlayer = 5, NumberOfItemsToSellToPlayer = 2, },
                new VariablePriceMenuItem("Badger Cellphone", 0, 0,45,55){ NumberOfItemsToPurchaseFromPlayer = 5, NumberOfItemsToSellToPlayer = 2, },
            });

        PossibleShopMenus.ShopMenuList.AddRange(new List<ShopMenu>{
            GenericPawnShopMenu,
        });
        //for (int i = 1; i <= 4; i++)
        //{
        //    ShopMenu newMenu = GenericPawnShopMenu.Copy();
        //    newMenu.Randomize();
        //    newMenu.ID += i.ToString();
        //    PossibleShopMenus.ShopMenuList.Add(newMenu);
        //}
    }
    private void GenericLocationsMenu()
    {
        PossibleShopMenus.ShopMenuList.AddRange(new List<ShopMenu>{
            new ShopMenu("ToolMenu","Tools",new List<MenuItem>() {

                new MenuItem("Flint Duct Tape", 4),

                new MenuItem("Flint Phillips Screwdriver", 14),
                new MenuItem("Flint Flathead Screwdriver", 16),
                new MenuItem("Flint Multi-Bit Screwdriver", 20),

                new MenuItem("Flint Rubber Mallet", 35),
                new MenuItem("Flint Hammer", 30),

                new MenuItem("Flint Crowbar", 65),

                new MenuItem("Flint Hatchet", 120),
                new MenuItem("Vom Feuer Machete", 130),

                new MenuItem("Flint Pliers", 35),
                new MenuItem("Flint Heavy Duty Pipe Wrench", 55),
                new MenuItem("Flint Shovel", 75),

                new MenuItem("TAG-HARD Flashlight", 80),
                new MenuItem("Flint Handle Flashlight", 65),

                new MenuItem("Flint Cordless Drill", 75),
                new MenuItem("Power Metal Cordless Drill", 90),
                new MenuItem("Power Metal Cordless Impact Driver", 150),




                new MenuItem("Power Metal Side Drill", 250),
                new MenuItem("Power Metal Custom Side Drill", 575),
            }),
            new ShopMenu("SportingGoodsMenu","SportingGoods",new List<MenuItem>() {
                new MenuItem("GASH Black Umbrella", 25),
                new MenuItem("GASH Blue Umbrella", 30),

                new MenuItem("TAG-HARD Flashlight", 85),
                new MenuItem("Flint Handle Flashlight", 60),

                new MenuItem("G.E.S. Baseball Bat", 95),
                new MenuItem("ProLaps Five Iron Golf Club", 85),
                new MenuItem("Schmidt & Priss TL6 Scanner", 400),


                new MenuItem("SCHEISS BS Binoculars", 150),
                new MenuItem("SCHEISS AS Binoculars", 350),
                new MenuItem("SCHEISS DS Binoculars", 500),
                new MenuItem("SCHEISS RP Binoculars", 650),

            }),
            new ShopMenu("CheapHotelMenu","Cheap Hotel",new List<MenuItem>() { new MenuItem("Room: Single Twin",99),new MenuItem("Room: Single Queen", 130),new MenuItem("Room: Double Queen", 150),new MenuItem("Room: Single King", 160), }),
            new ShopMenu("ExpensiveHotelMenu","Expensive Hotel",new List<MenuItem>() { new MenuItem("Room: Single Queen", 189),new MenuItem("Room: Double Queen", 220),new MenuItem("Room: Single King", 250),new MenuItem("Room: Delux", 280), }),
            new ShopMenu("ConvenienceStoreMenu","Convenience Store",new List<MenuItem>() {
                new MenuItem("Hot Dog", 5),
                new MenuItem("Burger",3),
                new MenuItem("Strawberry Rails Cereal", 7),
                new MenuItem("Crackles O' Dawn Cereal", 6),
                new MenuItem("White Bread", 3),
                new MenuItem("Carton of Milk", 4),
                new MenuItem("Sticky Rib Phat Chips", 2),
                new MenuItem("Habanero Phat Chips", 2),
                new MenuItem("Supersalt Phat Chips", 2),
                new MenuItem("Big Cheese Phat Chips", 2),
                new MenuItem("Ego Chaser Energy Bar", 2),
                new MenuItem("King Size P's & Q's", 3),
                new MenuItem("P's & Q's", 2),
                new MenuItem("Meteorite Bar", 2),
                new MenuItem("Donut", 1),
 


                //new MenuItem("Strawberry Rails Cereal", 7),
                //new MenuItem("Crackles O' Dawn Cereal", 6),
                //new MenuItem("White Bread", 3),
                //new MenuItem("Carton of Milk", 4),
                //new MenuItem("Bottle of JUNK Energy", 2),
                //new MenuItem("Can of Orang-O-Tang", 1),
                //new MenuItem("Bottle of GREY Water", 3),
                



                new MenuItem("Redwood Regular", 30),
                new MenuItem("Redwood Mild", 32),
                new MenuItem("Debonaire", 35),
                new MenuItem("Debonaire Menthol", 38),
                new MenuItem("Caradique", 35),
                new MenuItem("69 Brand", 40),
                new MenuItem("Estancia Cigar", 50),
                new MenuItem("Smoke Shop Rolling Papers",2),
                new MenuItem("DIC Lighter", 5),
                new MenuItem("DIC Lighter Ultra", 7),
                new MenuItem("Dippo Lighter", 20),
                new MenuItem("Can of eCola", 1),
                new MenuItem("Can of Sprunk", 1),
                new MenuItem("Can of Orang-O-Tang", 1),
                new MenuItem("Bottle of JUNK Energy", 2),
                new MenuItem("Bottle of PiBwasser", 3),
                new MenuItem("Bottle of Jakeys", 3),
                new MenuItem("Cup of Coffee", 2),
                new MenuItem("Bottle of Raine Water", 2),
                new MenuItem("Bottle of GREY Water", 3),
                new MenuItem("Flow Water ZERO", 3)
            }),
            new ShopMenu("SandwichMenu","Sanwiches",new List<MenuItem>() {
                new MenuItem("Ham and Cheese Sandwich", 2),
                new MenuItem("Turkey Sandwich", 2),
                new MenuItem("Tuna Sandwich", 2),
                new MenuItem("Supersalt Phat Chips", 2),
                new MenuItem("Big Cheese Phat Chips", 2),
                new MenuItem("Bottle of Raine Water", 2),
                new MenuItem("Can of eCola", 1),
                new MenuItem("Can of Sprunk", 1) }),
            new ShopMenu("HeadShopMenu","Head Shop",new List<MenuItem>() {
                new MenuItem("Redwood Regular", 30),
                new MenuItem("Redwood Mild", 32),
                new MenuItem("Debonaire", 35),
                new MenuItem("Debonaire Menthol", 38),
                new MenuItem("Caradique", 35),
                new MenuItem("69 Brand", 40),
                new MenuItem("Estancia Cigar", 50),
                new MenuItem("Smoke Shop Rolling Papers",2),
                new MenuItem("DIC Lighter", 5),
                new MenuItem("DIC Lighter Ultra", 7),
                new MenuItem("DIC Lighter Silver", 10),
                new MenuItem("DIC Lighter Gold", 15),
                new MenuItem("Dippo Lighter", 20),
                new MenuItem("Bottle of Raine Water", 2),
                new MenuItem("Flow Water ZERO", 3) }),
            new ShopMenu("LiquorStoreMenu","Liquor Store",new List<MenuItem>() {
                new MenuItem("Bottle of 40 oz", 5),
                new MenuItem("Bottle of Barracho", 3),
                new MenuItem("Bottle of PiBwasser", 3),
                new MenuItem("Bottle of Blarneys", 3),
                new MenuItem("Bottle of Logger", 3),
                new MenuItem("Bottle of Patriot", 3),
                new MenuItem("Bottle of Pride", 3),
                new MenuItem("Bottle of Stronzo", 4),
                new MenuItem("Bottle of A.M.", 4),
                new MenuItem("Bottle of Jakeys", 4),
                new MenuItem("Bottle of Dusche", 4),
                new MenuItem("Can of Hoplivion Double IPA", 4),
                new MenuItem("Can of Blarneys", 3),
                new MenuItem("Can of Logger", 3),
                new MenuItem("Bottle of Sinsimito Tequila", 30),
                new MenuItem("Bottle of Cazafortuna Tequila", 35),




                new MenuItem("The Mount Bourbon Whisky", 35),
                new MenuItem("Richard's Whisky Short", 55),
                new MenuItem("Macbeth Single Malt Short", 40),
                new MenuItem("Ragga Rum", 55),
                new MenuItem("This Worm Has Turned Tequilya", 40),
                new MenuItem("Cardiaque Brandy", 65),
                new MenuItem("Bourgeoix Cognac", 70),
                new MenuItem("Macbeth Single Malt", 45),
                new MenuItem("Richard's Whisky", 50),
                new MenuItem("NOGO Vodka", 30),
                new MenuItem("Bleuter'd Champagne", 95),
                new MenuItem("Cherenkov Red Label Vodka", 35),
                new MenuItem("Cherenkov Blue Label Vodka", 40),
                new MenuItem("Cherenkov Green Label Vodka", 45),
                new MenuItem("Cherenkov Purple Label Vodka", 50),
            }),
            new ShopMenu("BarMenu","Bar",new List<MenuItem>() { 
                //new MenuItem("Burger", 5),
                //new MenuItem("Hot Dog", 5),
                //new MenuItem("Bottle of Raine Water", 2),
                //new MenuItem("Cup of eCola", 2),
                //new MenuItem("Bottle of 40 oz", 5),
                new MenuItem("Bottle of Barracho", 4),
                new MenuItem("Bottle of PiBwasser", 4),
                new MenuItem("Bottle of Blarneys", 5),
                new MenuItem("Bottle of Logger", 5),
                new MenuItem("Bottle of Patriot", 5),
                new MenuItem("Bottle of Pride", 4),
                new MenuItem("Bottle of Stronzo", 5),
                new MenuItem("Bottle of A.M.", 4),
                new MenuItem("Bottle of Jakeys", 5),
                new MenuItem("Bottle of Dusche", 5),
                new MenuItem("Can of Hoplivion Double IPA", 4),
                new MenuItem("Can of Blarneys", 3),
                new MenuItem("Can of Logger", 3),
                new MenuItem("Bottle of Sinsimito Tequila", 30),
                new MenuItem("Bottle of Cazafortuna Tequila", 35),
                new MenuItem("The Mount Bourbon Whisky", 35),
                new MenuItem("Richard's Whisky Short", 55),
                new MenuItem("Macbeth Single Malt Short", 40),
                new MenuItem("Ragga Rum", 55),
                new MenuItem("This Worm Has Turned Tequilya", 40),
                new MenuItem("Cardiaque Brandy", 65),
                new MenuItem("Bourgeoix Cognac", 70),
                new MenuItem("Macbeth Single Malt", 45),
                new MenuItem("Richard's Whisky", 50),
                new MenuItem("NOGO Vodka", 30),
                new MenuItem("Bleuter'd Champagne", 95),
                new MenuItem("Cherenkov Red Label Vodka", 35),
                new MenuItem("Cherenkov Blue Label Vodka", 40),
                new MenuItem("Cherenkov Green Label Vodka", 45),
                new MenuItem("Cherenkov Purple Label Vodka", 50),


            }),
            new ShopMenu("CoffeeMenu","Coffee",new List<MenuItem>() {
                new MenuItem("Cup of Coffee", 2),
                new MenuItem("Donut", 5),
                new MenuItem("Bottle of Raine Water", 2) }),
            new ShopMenu("GenericMenu","Generic",new List<MenuItem>() {
                new MenuItem("Burger",3),
                new MenuItem("Big Cheese Phat Chips", 2),
                new MenuItem("Can of eCola", 1),
                new MenuItem("Can of Sprunk", 1),
                new MenuItem("Bottle of Raine Water", 2) }),
            new ShopMenu("PizzaMenu","Pizza",new List<MenuItem>() {




                new MenuItem("Small Cheese Pizza", 10),
                new MenuItem("Small Pepperoni Pizza", 12),
                new MenuItem("Small Supreme Pizza", 13),
                new MenuItem("Medium Cheese Pizza", 17),
                new MenuItem("Medium Pepperoni Pizza", 18),
                new MenuItem("Medium Supreme Pizza", 19),
                new MenuItem("Large Cheese Pizza", 23),
                new MenuItem("Large Pepperoni Pizza", 24),
                new MenuItem("Large Supreme Pizza", 25),


                new MenuItem("Slice of Pizza", 3),
                new MenuItem("Cup of Sprunk", 2),
                new MenuItem("Bottle of A.M.", 3),
                new MenuItem("Bottle of PiBwasser", 3),
                new MenuItem("Bottle of Barracho", 4),
                new MenuItem("Bottle of Blarneys", 4),
                new MenuItem("Bottle of Jakeys", 3),
                new MenuItem("Bottle of Stronzo", 4),
                new MenuItem("Bottle of Dusche", 3)








            }),
            new ShopMenu("DonutMenu","Donut",new List<MenuItem>() {
                new MenuItem("Donut", 1),
                new MenuItem("Can of eCola", 1),
                new MenuItem("Cup of eCola", 2),
                new MenuItem("Cup of Coffee", 3) }),
            new ShopMenu("FruitMenu","Fruit",new List<MenuItem>() {
                new MenuItem("Banana", 2),
                new MenuItem("Orange", 2),
                new MenuItem("Apple", 2),
                new MenuItem("Nuts", 2),
                new MenuItem("Bottle of Raine Water", 2),
                new MenuItem("Can of eCola", 1),
                new MenuItem("Can of Sprunk", 1) }),
            new ShopMenu("GasStationMenu","Gas Station",new List<MenuItem>() {
                new MenuItem("Hot Dog", 5),
                new MenuItem("Burger",3),
                new MenuItem("Sticky Rib Phat Chips", 2),
                new MenuItem("Habanero Phat Chips", 2),
                new MenuItem("Supersalt Phat Chips", 2),
                new MenuItem("Big Cheese Phat Chips", 2),
                new MenuItem("Ego Chaser Energy Bar", 2),
                new MenuItem("King Size P's & Q's", 3),
                new MenuItem("P's & Q's", 2),
                new MenuItem("Meteorite Bar", 2),
                new MenuItem("Donut", 1),
                new MenuItem("Redwood Regular", 30),
                new MenuItem("Redwood Mild", 32),
                new MenuItem("Debonaire", 35),
                new MenuItem("Estancia Cigar", 50),
                new MenuItem("Smoke Shop Rolling Papers",2),
                new MenuItem("DIC Lighter", 5),
                new MenuItem("DIC Lighter Ultra", 7),
                new MenuItem("DIC Lighter Silver", 10),
                new MenuItem("DIC Lighter Gold", 15),
                new MenuItem("Dippo Lighter", 20),

               // new MenuItem("Can of PiBwasser", 4),


                new MenuItem("Can of eCola", 1),
                new MenuItem("Can of Sprunk", 1),
                new MenuItem("Bottle of PiBwasser", 3),
                new MenuItem("Bottle of Jakeys", 3),
                new MenuItem("Can of Hoplivion Double IPA", 4),
                new MenuItem("Can of Blarneys", 3),
                new MenuItem("Can of Logger", 3),
                new MenuItem("Cup of Coffee", 2),
                new MenuItem("Bottle of Raine Water", 2),
                new MenuItem("Flow Water ZERO", 3)

            }),
            new ShopMenu("FancyDeliMenu","Deli",new List<MenuItem>() { new MenuItem("Chicken Club Salad",10),new MenuItem("Spicy Seafood Gumbo",14),new MenuItem("Muffaletta",8),new MenuItem("Zucchini Garden Pasta",9),new MenuItem("Pollo Mexicano",12),new MenuItem("Italian Cruz Po'boy",19),new MenuItem("Chipotle Chicken Panini",10)}),
            new ShopMenu("FancyFishMenu","Fish",new List<MenuItem>() { new MenuItem("Coconut Crusted Prawns",12),new MenuItem("Crab and Shrimp Louie",10),new MenuItem("Open-Faced Crab Melt",28),new MenuItem("King Salmon",48),new MenuItem("Ahi Tuna",44), }),
            new ShopMenu("FancyGenericMenu","Restaurant",new List<MenuItem>() { new MenuItem("Smokehouse Burger",10),new MenuItem("Chicken Critters Basket",7),new MenuItem("Prime Rib 16 oz",22),new MenuItem("Bone-In Ribeye",25),new MenuItem("Grilled Pork Chops",14),new MenuItem("Grilled Shrimp",15)}),
            new ShopMenu("NoodleMenu","Noodles",new List<MenuItem>() { new MenuItem("Juek Suk tong Mandu",8),new MenuItem("Hayan Jam Pong",9),new MenuItem("Sal Gook Su Jam Pong",12),new MenuItem("Chul Pan Bokkeum Jam Pong",20),new MenuItem("Deul Gae Udon",12),new MenuItem("Dakgogo Bokkeum Bap",9),}),            
            new ShopMenu("MexicanMenu","MexicanMenu",new List<MenuItem> {
                new MenuItem("Asada Plate", 12),
                new MenuItem("2 Tacos Combo", 10),
                new MenuItem("2 Enchiladas Combo", 10),
                new MenuItem("Quesadilla", 9),
                new MenuItem("San Andreas Burrito", 8),
                new MenuItem("Torta", 8),
                new MenuItem("Taco", 3),
                new MenuItem("Can of eCola", 1),
                new MenuItem("Can of Sprunk", 1),
            }),
            //Drugs
            new ShopMenu("WeedMenu","Marijuana",new List<MenuItem>() {

                new MenuItem("Marijuana",15),

                new MenuItem("Smoke Shop Rolling Papers",2),
                //new MenuItem("OG Kush",8),
                //new MenuItem("Northern Lights",9),


                new MenuItem("Bong",25),
                //new MenuItem("ElectroToke Vape", 25),
                new MenuItem("DIC Lighter",5),
                new MenuItem("DIC Lighter Ultra", 7),
            }),
            new ShopMenu("WeedAndCigMenu","Marijuana/Cigarette",new List<MenuItem>() { 
                //new MenuItem("White Widow",10),
                //new MenuItem("OG Kush",12),
                //new MenuItem("Northern Lights",13),
                new MenuItem("Marijuana",17),
                new MenuItem("Smoke Shop Rolling Papers",2),
                new MenuItem("Bong",35),
                new MenuItem("Redwood Regular", 30),
                new MenuItem("Redwood Mild", 32),
                new MenuItem("Debonaire", 35),
                new MenuItem("Debonaire Menthol", 38),
                new MenuItem("Caradique", 35),
                new MenuItem("69 Brand", 40),
                new MenuItem("Estancia Cigar", 50),
               // new MenuItem("ElectroToke Vape", 25),
                new MenuItem("DIC Lighter",5),
                new MenuItem("DIC Lighter Ultra", 7),
                new MenuItem("DIC Lighter Silver", 10),
                new MenuItem("DIC Lighter Gold", 15),
                new MenuItem("Dippo Lighter", 20), }),
            new ShopMenu("PharmacyMenu","Pharmacy",new List<MenuItem>() {
                new MenuItem("Chesty", 19, 0),
                new MenuItem("Pseudoephedrine", 25,0) { NumberOfItemsToSellToPlayer = 5},



                new MenuItem("Hingmyralgan", 45, 0),
                new MenuItem("Deludamol", 85, 0),
                new MenuItem("Delladamol", 55, 0),
                new MenuItem("Wach-Auf Caffeine Pills", 35, 0),
                new MenuItem("Lax to the Max", 24, 0),
                new MenuItem("Bull Shark Testosterone", 25, 0),
                new MenuItem("Alco Patch", 55, 0),
                new MenuItem("Equanox", 89, 0),
                new MenuItem("Mollis", 345, 0),
                new MenuItem("Zombix", 267, 0),
                new MenuItem("Diazepam", 150, 0),



            }),
            //New
            new ShopMenu("MallMenu","Mall",new List<MenuItem>() {
                new MenuItem("Flint Phillips Screwdriver", 14),
                new MenuItem("Flint Flathead Screwdriver", 16),
                new MenuItem("Flint Multi-Bit Screwdriver", 20),
                new MenuItem("Flint Pliers", 35),
                new MenuItem("TAG-HARD Flashlight", 85),
                new MenuItem("Flint Handle Flashlight", 60),
                new MenuItem("Schmidt & Priss TL6 Scanner", 400),
                new MenuItem("SCHEISS BS Binoculars", 150),
                new MenuItem("SCHEISS AS Binoculars", 350),
                new MenuItem("SCHEISS DS Binoculars", 500),
                new MenuItem("SCHEISS RP Binoculars", 650),

                new MenuItem("Bottle of Raine Water", 2),
                new MenuItem("Can of eCola", 1),
                new MenuItem("Can of Sprunk", 1),
                new MenuItem("Dippo Lighter", 20),

            }),
            new ShopMenu("DepartmentStoreMenu","DepartmentStore",new List<MenuItem>() {
               new MenuItem("GASH Black Umbrella", 25),
                new MenuItem("GASH Blue Umbrella", 30),
                new MenuItem("TAG-HARD Flashlight", 85),
                new MenuItem("Flint Handle Flashlight", 60),
                new MenuItem("G.E.S. Baseball Bat", 95),
                new MenuItem("ProLaps Five Iron Golf Club", 85),
                new MenuItem("Schmidt & Priss TL6 Scanner", 400),
                new MenuItem("SCHEISS BS Binoculars", 150),
                new MenuItem("SCHEISS AS Binoculars", 350),
                new MenuItem("SCHEISS DS Binoculars", 500),
                new MenuItem("SCHEISS RP Binoculars", 650),
                new MenuItem("Flint Duct Tape", 4),
                new MenuItem("Flint Phillips Screwdriver", 14),
                new MenuItem("Flint Flathead Screwdriver", 16),
                new MenuItem("Flint Multi-Bit Screwdriver", 20),
                new MenuItem("Flint Rubber Mallet", 35),
                new MenuItem("Flint Hammer", 30),
                new MenuItem("Flint Crowbar", 65),
                new MenuItem("Flint Hatchet", 120),
                new MenuItem("Vom Feuer Machete", 130),
                new MenuItem("Flint Pliers", 35),
                new MenuItem("Flint Heavy Duty Pipe Wrench", 55),
                new MenuItem("Flint Shovel", 75),
                new MenuItem("TAG-HARD Flashlight", 80),
                new MenuItem("Flint Handle Flashlight", 65),
                new MenuItem("Flint Cordless Drill", 75),
                new MenuItem("Power Metal Cordless Drill", 90),
                new MenuItem("Power Metal Cordless Impact Driver", 150),
                new MenuItem("Power Metal Side Drill", 250),
                new MenuItem("Power Metal Custom Side Drill", 575),
            }),
            new ShopMenu("InternetCafeMenu","InternetCafe",new List<MenuItem>() {
                new MenuItem("Donut", 1),
                new MenuItem("Can of eCola", 1),
                new MenuItem("Can of Sprunk", 1),
                new MenuItem("Cup of Coffee", 2),
                new MenuItem("Hot Pretzel", 2),
                new MenuItem("Bottle of GREY Water", 3),
                new MenuItem("Bagel Sandwich", 8),
            }),
            new ShopMenu("DeliGroceryMenu","Deli Grocery",new List<MenuItem>() {
                new MenuItem("Sticky Rib Phat Chips", 2),
                new MenuItem("Habanero Phat Chips", 2),
                new MenuItem("Supersalt Phat Chips", 2),
                new MenuItem("Big Cheese Phat Chips", 2),
                new MenuItem("Ego Chaser Energy Bar", 2),
                new MenuItem("King Size P's & Q's", 3),
                new MenuItem("P's & Q's", 2),
                new MenuItem("Meteorite Bar", 2),
                new MenuItem("Donut", 1),
                new MenuItem("Redwood Regular", 30),
                new MenuItem("Redwood Mild", 32),
                new MenuItem("Debonaire", 35),
                new MenuItem("Estancia Cigar", 50),
                new MenuItem("DIC Lighter", 5),
                new MenuItem("DIC Lighter Ultra", 7),
                new MenuItem("DIC Lighter Silver", 10),
                new MenuItem("DIC Lighter Gold", 15),
                new MenuItem("Dippo Lighter", 20),
                new MenuItem("Can of eCola", 1),
                new MenuItem("Can of Sprunk", 1),
                new MenuItem("Bottle of PiBwasser", 3),
                new MenuItem("Bottle of Jakeys", 3),
                new MenuItem("Can of Hoplivion Double IPA", 4),
                new MenuItem("Can of Blarneys", 3),
                new MenuItem("Can of Logger", 3),
                new MenuItem("Cup of Coffee", 2),
                new MenuItem("Bottle of Raine Water", 2),
                new MenuItem("Flow Water ZERO", 3)
            }),
            new ShopMenu("ItalianMenu","Italian",new List<MenuItem>() {
                new MenuItem("Slice of Pizza", 3),
                new MenuItem("Cup of Sprunk", 2),
                new MenuItem("Bottle of A.M.", 3),
                new MenuItem("Bottle of PiBwasser", 3),
                new MenuItem("Bottle of Barracho", 4),
                new MenuItem("Bottle of Blarneys", 4),
                new MenuItem("Bottle of Jakeys", 3),
                new MenuItem("Bottle of Stronzo", 4),
                new MenuItem("Bottle of Dusche", 3)
            }),
        });
    }
    private void SpecificRestaurants()
    {
        int ExpensiveComboPrice = 9;
        int MediumComboPrice = 8;
        int CheapComboPrice = 7;

        int SodaPrice = 2;
        PossibleShopMenus.ShopMenuList.AddRange(new List<ShopMenu>{
            //Burger
            new ShopMenu("BurgerShotMenu","BurgerShotMenu",new List<MenuItem> {
                new MenuItem("The Bleeder Meal", 9),
                new MenuItem("Torpedo Meal", 8),
                new MenuItem("Money Shot Meal", 7),
                new MenuItem("Meat Free Meal", 6),
                new MenuItem("Bleeder Burger", 5),
                new MenuItem("Money Shot Burger", 5),
                new MenuItem("Torpedo Sandwich", 4),
                new MenuItem("Freedom Fries", 2),
                new MenuItem("Liter of eCola", 2),
                new MenuItem("Liter of Sprunk", 2),
            }),
            new ShopMenu("UpNAtomMenu","UpNAtomMenu",new List<MenuItem> {
                new MenuItem("Trio Trio Combo", 8),
                new MenuItem("Dual Dual Combo", 7),
                new MenuItem("Solo Solo Combo", 6),
                new MenuItem("Trio Trio Burger", 5),
                new MenuItem("Dual Dual Burger", 5),
                new MenuItem("Solo Solo Burger", 4),
                new MenuItem("French Fries", 2),
                new MenuItem("Jumbo Shake", 5),
                new MenuItem("Large eCola", 2),
                new MenuItem("Large Sprunk", 2),
            }),
            new ShopMenu("BeefyBillsMenu","BeefyBillsMenu",new List<MenuItem> {
                new MenuItem("Kingsize Burger", 5),
                new MenuItem("Double Burger", 4),
                new MenuItem("Megacheese Burger", 3),
                new MenuItem("French Fries", 2),
                new MenuItem("Can of eCola", 1),
                new MenuItem("Can of Sprunk", 1),
                new MenuItem("Bottle of Raine Water", 2),
            }),
            new ShopMenu("WigwamMenu","WigwamMenu",new List<MenuItem> {
                new MenuItem("Big Wig Combo", 7),
                new MenuItem("Cheesie Wigwam Combo", 6),
                new MenuItem("Wigwam Classic Combo", 5),
                new MenuItem("Big Wig Burger", 4),
                new MenuItem("Cheesie Wigwam Burger", 3),
                new MenuItem("Wigwam Classic Burger", 3),
                new MenuItem("French Fries", 2),
                new MenuItem("Cup of eCola", 2),
                new MenuItem("Cup of Sprunk", 2),
            }),
            new ShopMenu("LaVacaLocaMenu","LaVacaLocaMenu",new List<MenuItem> {
                new MenuItem("Muy Loca Combo", 8),
                new MenuItem("Loca Combo", 7),
                new MenuItem("Locita Combo", 6),
                new MenuItem("Muy Loca Burger", 5),
                new MenuItem("Loca Burger", 5),
                new MenuItem("Locita Burger", 4),
                new MenuItem("French Fries", 2),
                new MenuItem("Cup of eCola", 2),
                new MenuItem("Cup of Sprunk", 2),
            }),
            new ShopMenu("SnrBunsMenu","SnrBunsMenu",new List<MenuItem> {
                new MenuItem("Snr. Combo", 9),
                new MenuItem("Soph. Combo", 8),
                new MenuItem("Jr. Combo", 7),
                new MenuItem("Snr. Burger", 6),
                new MenuItem("Soph. Burger", 5),
                new MenuItem("Jr. Burger", 4),
                new MenuItem("French Fries", 2),
                new MenuItem("Cup of eCola", 2),
                new MenuItem("Cup of Sprunk", 2),
            }),
            new ShopMenu("HornysBurgersMenu","HornysBurgersMenu",new List<MenuItem> {
                new MenuItem("Big Horny Combo", 8),
                new MenuItem("Horny Combo", 7),
                new MenuItem("Randy Combo", 6),
                new MenuItem("Big Horny Burger", 5),
                new MenuItem("Horny Burger", 5),
                new MenuItem("Randy Burger", 4),
                new MenuItem("French Fries", 2),
                new MenuItem("Cup of eCola", 2),
                new MenuItem("Cup of Sprunk", 2),
            }),

            new ShopMenu("ChihuahuaHotDogMenu","Chihuahua HotDogs",new List<MenuItem>() {
                new MenuItem("Hot Dog", 5),
                new MenuItem("Hot Sausage", 5),
                new MenuItem("Hot Pretzel", 2),
                new MenuItem("3 Mini Pretzels", 3),
                new MenuItem("Nuts", 2),
                new MenuItem("Can of Sprunk", 1),
                new MenuItem("Bottle of Raine Water", 2) }),

            //Sandwich 
            new ShopMenu("BiteMenu","BiteMenu",new List<MenuItem> {
                new MenuItem("Gut Buster Combo", 8),
                new MenuItem("Meat Tube Combo", 7),
                new MenuItem("Iceberg Salad Combo", 6),
                new MenuItem("Gut Buster Sandwich", 4),
                new MenuItem("Meat Tube Sandwich", 3),
                new MenuItem("Iceberg Salad", 2),
                new MenuItem("Sticky Rib Phat Chips", 2),
                new MenuItem("Habanero Phat Chips", 2),
                new MenuItem("Supersalt Phat Chips", 2),
                new MenuItem("Cup of eCola", 2),
                new MenuItem("Cup of Sprunk", 2),
            }),
            //Mexican
            new ShopMenu("TacoFarmerMenu","TacoFarmerMenu",new List<MenuItem> {
                new MenuItem("Asada Plate", 12),
                new MenuItem("2 Tacos Combo", 10),
                new MenuItem("2 Enchiladas Combo", 10),
                new MenuItem("Quesadilla", 9),
                new MenuItem("San Andreas Burrito", 8),
                new MenuItem("Torta", 8),
                new MenuItem("Taco", 3),
                new MenuItem("Can of eCola", 1),
                new MenuItem("Can of Sprunk", 1),
            }),
            new ShopMenu("TacoBombMenu","Taco Bomb",new List<MenuItem> {
                new MenuItem("Beef Bazooka",8),
                new MenuItem("Volcano Mudsplatter Nachos",7),
                new MenuItem("Deep Fried Salad",7),
                new MenuItem("Cheesy Meat Flappers",6),
                new MenuItem("Chimichingado Chiquito",5),
                new MenuItem("Breakfast Burrito",4),
                new MenuItem("Can of eCola", 1),
                new MenuItem("Can of Sprunk", 1), }),
            
            //Italian
            new ShopMenu("PizzaThisMenu","Pizza",new List<MenuItem>() {
                new MenuItem("10 inch Cheese Pizza", 11),
                new MenuItem("10 inch Pepperoni Pizza", 13),
                new MenuItem("10 inch Supreme Pizza", 14),
                new MenuItem("12 inch Cheese Pizza", 18),
                new MenuItem("12 inch Pepperoni Pizza", 19),
                new MenuItem("12 inch Supreme Pizza", 20),
                new MenuItem("18 inch Cheese Pizza", 25),
                new MenuItem("18 inch Pepperoni Pizza", 27),
                new MenuItem("18 inch Supreme Pizza", 30),
                new MenuItem("Cup of Sprunk", 2),
                new MenuItem("Bottle of A.M.", 3),
                new MenuItem("Bottle of PiBwasser", 3),
                new MenuItem("Bottle of Barracho", 4),
                new MenuItem("Bottle of Blarneys", 4),
                new MenuItem("Bottle of Jakeys", 3),
                new MenuItem("Bottle of Stronzo", 4),
                new MenuItem("Bottle of Dusche", 3)
            }),
            new ShopMenu("AlDentesMenu","AlDentesMenu",new List<MenuItem> {
                new MenuItem("Tour of Algonquin", 15),
                new MenuItem("Lasagna Cheesico", 14),
                new MenuItem("Seafood Ravioli", 13),
                new MenuItem("Spaghetti & 'Meat' Balls", 10),
                new MenuItem("Bottle of A.M.", 4),
                new MenuItem("Bottle of PiBwasser", 3),
                new MenuItem("Bottle of Blarneys", 4),
                new MenuItem("Bottle of Raine Water", 2),
            }),

            //Coffee
            new ShopMenu("BeanMachineMenu","Bean Machine",new List<MenuItem>() {
                new MenuItem("High Noon Coffee", 7),
                new MenuItem("The Eco-ffee", 4),
                new MenuItem("Speedball Coffee", 6),
                new MenuItem("Gunkacchino Coffee", 6),
                new MenuItem("Bratte Coffee", 19),
                new MenuItem("Flusher Coffee", 9),
                new MenuItem("Caffeagra Coffee", 12),
                new MenuItem("Big Fruit Smoothie", 14),
                new MenuItem("Donut", 3),
                new MenuItem("Bagel Sandwich", 8),
                new MenuItem("Bottle of Raine Water", 3),
                new MenuItem("Flow Water ZERO", 3)
            }),

            //Chicken
            new ShopMenu("CluckinBellMenu","CluckinBellMenu",new List<MenuItem> {
                new MenuItem("Cluckin' Huge Meal", 8),
                new MenuItem("Cluckin' Big Meal", 7),
                new MenuItem("Cluckin' Little Meal", 6),
                new MenuItem("Wing Piece", 4),
                new MenuItem("Little Peckers", 4),
                new MenuItem("Balls & Rings", 4),
                new MenuItem("French Fries", 2),
                new MenuItem("XXL eCola", 2),
                new MenuItem("XXL Sprunk", 2),
            }),
            new ShopMenu("BishopsChickenMenu","BishopsChickenMenu",new List<MenuItem> {
                new MenuItem("Pope Combo", 8),
                new MenuItem("Cardinal Combo", 7),
                new MenuItem("Bishop Combo", 6),
                new MenuItem("3 pc Chicken", 4),
                new MenuItem("2 pc Chicken ", 3),
                new MenuItem("1 pc Chicken", 2),
                new MenuItem("French Fries", 2),
                new MenuItem("Cup of eCola", 2),
                new MenuItem("Cup of Sprunk", 2),
            }),
            new ShopMenu("LuckyPluckerMenu","LuckyPluckerMenu",new List<MenuItem> {
                new MenuItem("Plucker Combo 1", 8),
                new MenuItem("Plucker Combo 2", 7),
                new MenuItem("Plucker Combo 3", 6),
                new MenuItem("Plucker Breast", 4),
                new MenuItem("Plucker Thigh", 4),
                new MenuItem("Plucker Salad", 4),
                new MenuItem("French Fries", 2),
                new MenuItem("Cup of eCola", 2),
                new MenuItem("Cup of Sprunk", 2),
            }),
            //Cherry Popper
            new ShopMenu("CherryPopperMenu","CherryPopperMenu",new List<MenuItem> {
                new MenuItem("Captain's Log", 4),
                new MenuItem("Uder Milken", 4),
                new MenuItem("Creamy Chufty", 3),
                new MenuItem("Chocolate Chufty", 3),
                new MenuItem("Zebrabar", 4),
                new MenuItem("Chilldo X-Treme", 3),
                new MenuItem("Fruity Streak", 3),
                new MenuItem("Chocco Streak", 3),
                new MenuItem("Earthquakes", 4),
                new MenuItem("Chocolate Starfish", 4),
            }),
            //Rusty Browns
            new ShopMenu("RustyBrownsMenu","RustyBrownsMenu",new List<MenuItem> {
                new MenuItem("Chocolate Donut", 2),
                new MenuItem("Sprinkles Donut", 2),
                new MenuItem("Rusty Ring Donut", 2),
                new MenuItem("Double Choc Whammy Donut", 3),
                new MenuItem("Cup of Coffee", 2),
            }),
            new ShopMenu("IceCreamMenu","IceCreamMenu",new List<MenuItem> {
                new MenuItem("Chocolate Cone", 3),
                new MenuItem("Vanilla Cone", 2),
                new MenuItem("Hot Fudge Sundae", 5),
                new MenuItem("Banana Split", 7),
                new MenuItem("Chocolate Shake", 9),
                new MenuItem("Vanilla Shake", 8),
            }),
        });
    }
    private void SpecificConvenienceStores()
    {
        PossibleShopMenus.ShopMenuList.AddRange(new List<ShopMenu>{
            new ShopMenu("TwentyFourSevenMenu","24/7",new List<MenuItem>() {
                new MenuItem("Hot Dog", 5),
                new MenuItem("White Bread", 3),
                new MenuItem("Carton of Milk", 4),
                new MenuItem("Strawberry Rails Cereal", 7),
                new MenuItem("Crackles O' Dawn Cereal", 6),
                new MenuItem("Sticky Rib Phat Chips", 2),
                new MenuItem("Habanero Phat Chips", 2),
                new MenuItem("Supersalt Phat Chips", 2),
                new MenuItem("Big Cheese Phat Chips", 2),
                new MenuItem("Ego Chaser Energy Bar", 2),
                new MenuItem("King Size P's & Q's", 3),
                new MenuItem("P's & Q's", 2),
                new MenuItem("Meteorite Bar", 2),
                new MenuItem("Donut", 1),
                new MenuItem("Redwood Regular", 30),
                new MenuItem("Redwood Mild", 32),
                new MenuItem("Debonaire", 35),
                new MenuItem("Debonaire Menthol", 38),
                new MenuItem("Caradique", 35),
                new MenuItem("69 Brand", 40),
                new MenuItem("Estancia Cigar", 50),
                new MenuItem("Smoke Shop Rolling Papers",2),
                new MenuItem("DIC Lighter", 5),
                new MenuItem("Can of eCola", 1),
                new MenuItem("Can of Sprunk", 1),
                new MenuItem("Can of Orang-O-Tang", 1),
                new MenuItem("Bottle of JUNK Energy", 2),
                new MenuItem("Bottle of PiBwasser", 3),
                new MenuItem("Bottle of Jakeys", 3),
                new MenuItem("Can of Hoplivion Double IPA", 4),
                new MenuItem("Can of Blarneys", 3),
                new MenuItem("Cup of Coffee", 2),
                new MenuItem("Bottle of Raine Water", 2),
                new MenuItem("Bottle of GREY Water", 3),}),
            new ShopMenu("GrainOfTruthMenu","Grain Of Truth",new List<MenuItem>() {
                new MenuItem("White Bread", 3),
                new MenuItem("Carton of Milk", 4),
                new MenuItem("Donut", 1),
                new MenuItem("Cup of Coffee", 2),
                new MenuItem("Bottle of Raine Water", 2),
                new MenuItem("Bottle of GREY Water", 3),
                new MenuItem("Flow Water ZERO", 3)}),
            new ShopMenu("FruitVineMenu","Fruit Of The Vine",new List<MenuItem>() {
                new MenuItem("Sticky Rib Phat Chips", 2),
                new MenuItem("Big Cheese Phat Chips", 2),
                new MenuItem("Ego Chaser Energy Bar", 2),
                new MenuItem("Meteorite Bar", 2),
                new MenuItem("Donut", 1),
                new MenuItem("Redwood Regular", 30),
                new MenuItem("Redwood Mild", 32),
                new MenuItem("Debonaire", 35),
                new MenuItem("Debonaire Menthol", 38),
                new MenuItem("Caradique", 35),
                new MenuItem("69 Brand", 40),
                new MenuItem("Estancia Cigar", 50),
                new MenuItem("Smoke Shop Rolling Papers",2),
                new MenuItem("DIC Lighter", 5),
                new MenuItem("Can of eCola", 1),
                new MenuItem("Can of Sprunk", 1),
                new MenuItem("Can of Orang-O-Tang", 1),
                new MenuItem("Bottle of JUNK Energy", 2),
                new MenuItem("Bottle of Barracho", 3),
                new MenuItem("Bottle of PiBwasser", 3),
                new MenuItem("Bottle of Blarneys", 3),
                new MenuItem("Bottle of Logger", 3),
                new MenuItem("Bottle of Patriot", 3),
                new MenuItem("Bottle of Pride", 3),
                new MenuItem("Bottle of Stronzo", 4),
                new MenuItem("Bottle of A.M.", 4),
                new MenuItem("Bottle of Jakeys", 4),
                new MenuItem("Bottle of Dusche", 4),
                new MenuItem("Can of Hoplivion Double IPA", 4),
                new MenuItem("Can of Blarneys", 3),
                new MenuItem("Can of Logger", 3),
                new MenuItem("Bottle of Sinsimito Tequila", 30),
                new MenuItem("Bottle of Cazafortuna Tequila", 35),                          
                new MenuItem("Cardiaque Brandy", 65),
                new MenuItem("Bourgeoix Cognac", 70),
                new MenuItem("Macbeth Single Malt", 45),
                new MenuItem("Richard's Whisky", 50),
                new MenuItem("Cherenkov Green Label Vodka", 45),
                new MenuItem("Cherenkov Purple Label Vodka", 50),
                new MenuItem("Cup of Coffee", 2),
                new MenuItem("Bottle of Raine Water", 2),
                new MenuItem("Bottle of GREY Water", 3),
                new MenuItem("Flow Water ZERO", 3)}),
            new ShopMenu("RonMenu","Ron",new List<MenuItem>() {
                new MenuItem("Habanero Phat Chips", 2),
                new MenuItem("Supersalt Phat Chips", 2),
                new MenuItem("King Size P's & Q's", 3),
                new MenuItem("P's & Q's", 2),
                new MenuItem("Meteorite Bar", 2),
                new MenuItem("Donut", 1),
                new MenuItem("Redwood Regular", 30),
                new MenuItem("Redwood Mild", 32),
                new MenuItem("Debonaire", 35),
                new MenuItem("Estancia Cigar", 50),
                new MenuItem("Smoke Shop Rolling Papers",2),
                new MenuItem("DIC Lighter", 5),
                new MenuItem("Can of eCola", 1),
                new MenuItem("Can of Sprunk", 1),
                new MenuItem("Can of Orang-O-Tang", 1),
                new MenuItem("Bottle of JUNK Energy", 2),
                new MenuItem("Bottle of PiBwasser", 3),
                new MenuItem("Bottle of Jakeys", 3),
                new MenuItem("Can of Hoplivion Double IPA", 4),
                new MenuItem("Can of Blarneys", 3),
                new MenuItem("Can of Logger", 3),
                new MenuItem("Cup of Coffee", 2),
                new MenuItem("Bottle of Raine Water", 2),
                new MenuItem("Bottle of GREY Water", 3),
                new MenuItem("Flow Water ZERO", 3)}),
            new ShopMenu("XeroMenu","Xero",new List<MenuItem>() {
                new MenuItem("Sticky Rib Phat Chips", 2),
                new MenuItem("Habanero Phat Chips", 2),
                new MenuItem("Supersalt Phat Chips", 2),
                new MenuItem("Big Cheese Phat Chips", 2),
                new MenuItem("P's & Q's", 2),
                new MenuItem("Meteorite Bar", 2),
                new MenuItem("Donut", 1),
                new MenuItem("Redwood Regular", 30),
                new MenuItem("Redwood Mild", 32),
                new MenuItem("Debonaire", 35),
                new MenuItem("Estancia Cigar", 50),
                new MenuItem("Smoke Shop Rolling Papers",2),
                new MenuItem("DIC Lighter", 5),
                new MenuItem("Can of eCola", 1),
                new MenuItem("Can of Sprunk", 1),
                new MenuItem("Can of Orang-O-Tang", 1),
                new MenuItem("Bottle of PiBwasser", 3),
                new MenuItem("Bottle of Jakeys", 3),
                new MenuItem("Can of Hoplivion Double IPA", 4),
                new MenuItem("Can of Logger", 3),
                new MenuItem("Bottle of Cazafortuna Tequila", 35),
                new MenuItem("The Mount Bourbon Whisky", 35),
                new MenuItem("Ragga Rum", 55),
                new MenuItem("This Worm Has Turned Tequilya", 40),
                new MenuItem("Richard's Whisky", 50),
                new MenuItem("NOGO Vodka", 30),
                new MenuItem("Cherenkov Red Label Vodka", 35),
                new MenuItem("Cherenkov Purple Label Vodka", 50),
                new MenuItem("Cup of Coffee", 2),
                new MenuItem("Bottle of Raine Water", 2),
                new MenuItem("Bottle of GREY Water", 3),
                new MenuItem("Flow Water ZERO", 3)}),
            new ShopMenu("LTDMenu","LTD",new List<MenuItem>() {
                new MenuItem("White Bread", 3),
                new MenuItem("Carton of Milk", 4),
                new MenuItem("Strawberry Rails Cereal", 7),
                new MenuItem("Crackles O' Dawn Cereal", 6),
                new MenuItem("Hot Dog", 5),
                new MenuItem("Sticky Rib Phat Chips", 2),
                new MenuItem("Habanero Phat Chips", 2),
                new MenuItem("Supersalt Phat Chips", 2),
                new MenuItem("Big Cheese Phat Chips", 2),
                new MenuItem("Ego Chaser Energy Bar", 2),
                new MenuItem("Meteorite Bar", 2),
                new MenuItem("Donut", 1),
                new MenuItem("Redwood Regular", 30),
                new MenuItem("Redwood Mild", 32),
                new MenuItem("Debonaire", 35),
                new MenuItem("Estancia Cigar", 50),
                new MenuItem("Smoke Shop Rolling Papers",2),
                new MenuItem("DIC Lighter", 5),
                new MenuItem("Can of eCola", 1),
                new MenuItem("Can of Sprunk", 1),
                new MenuItem("Can of Orang-O-Tang", 1),
                new MenuItem("Bottle of JUNK Energy", 2),
                new MenuItem("Bottle of PiBwasser", 3),
                new MenuItem("Bottle of Jakeys", 3),
                new MenuItem("Can of Blarneys", 3),
                new MenuItem("Can of Logger", 3),
                new MenuItem("Cup of Coffee", 2),
                new MenuItem("Bottle of Raine Water", 2),
                new MenuItem("Bottle of GREY Water", 3),
                new MenuItem("Flow Water ZERO", 3)}),
        });
    }
    private void SpecificHotels()
    {
        PossibleShopMenus.ShopMenuList.AddRange(new List<ShopMenu>{
            new ShopMenu("ViceroyMenu","Viceroy",new List<MenuItem>() {
                new MenuItem("City View King",354),
                new MenuItem("City View Deluxe King", 378),
                //new MenuItem("Partial Ocean View King", 392),
                new MenuItem("Ocean View King", 423),
               // new MenuItem("City View Two Bedded Room", 456),
                new MenuItem("Grande King", 534),
                new MenuItem("Grande Ocean View King", 647),
                new MenuItem("Empire Suite", 994),
                new MenuItem("Monarch Suite", 5000), }),
        });
    }
    private void SpecificSportingGoods()
    {
        PossibleShopMenus.ShopMenuList.AddRange(new List<ShopMenu>{
            new ShopMenu("VespucciSportsMenu", "VespucciSports", new List<MenuItem>() {
                    new MenuItem("GASH Black Umbrella", 25),
                    new MenuItem("GASH Blue Umbrella", 30),

                    new MenuItem("TAG-HARD Flashlight", 85),
                    new MenuItem("Flint Handle Flashlight", 60),

                    new MenuItem("G.E.S. Baseball Bat", 95),
                    new MenuItem("ProLaps Five Iron Golf Club", 85),
                    new MenuItem("Schmidt & Priss TL6 Scanner", 400),


                    new MenuItem("SCHEISS BS Binoculars", 150),
                    new MenuItem("SCHEISS AS Binoculars", 350),
                    new MenuItem("SCHEISS DS Binoculars", 500),
                    new MenuItem("SCHEISS RP Binoculars", 650),

                    //Bikes!
                    new MenuItem("BMX", 650),
                    new MenuItem("Cruiser", 1000),
                    new MenuItem("Scorcher", 1400),
                }),
            new ShopMenu("BourgeoisBicyclesMenu", "Bourgeois Bicycles", new List<MenuItem>() {
                    //Bikes!
                    new MenuItem("Cruiser", 1100),
                    new MenuItem("Fixter", 1200),
                    new MenuItem("Scorcher", 1500),
                    new MenuItem("Whippet Race Bike", 5100),
                    new MenuItem("Endurex Race Bike", 5200),
                    new MenuItem("Tri-Cycles Race Bike", 5300),
                    new MenuItem("Coil Inductor", 5900),
                    new MenuItem("Junk Energy Coil Inductor", 6500),
                }),
        });
    }
    private void SpecificDealerships()
    {
        PossibleShopMenus.ShopMenuList.AddRange(new List<ShopMenu>{
            new ShopMenu("BenefactorGallavanterMenu","Benefactor/Gallavanter",new List<MenuItem>() {
                new MenuItem("Gallivanter Baller",67000,45000),
                new MenuItem("Gallivanter Baller 2",90000,56000),
                new MenuItem("Gallivanter Baller LE",149000,76000),
                new MenuItem("Gallivanter Baller LE LWB",247000,125000),
                new MenuItem("Gallivanter Baller LE (Armored)",320000,160000),
                new MenuItem("Gallivanter Baller ST-D",145000,75000),

                new MenuItem("Benefactor Schafter",65000,34000),
                new MenuItem("Benefactor Schafter LWB",75000,52000),
                new MenuItem("Benefactor Schafter V12",112000,81000),
                new MenuItem("Benefactor Schafter V12 (Armored)",200000,100000),
                new MenuItem("Benefactor Feltzer",145000,90500),
                new MenuItem("Benefactor Schwartzer",48000,27000),
                new MenuItem("Benefactor Surano",110000,78000),
                new MenuItem("Benefactor Serrano",60000,45000),
                new MenuItem("Benefactor Dubsta",110000,78000),
                new MenuItem("Benefactor Dubsta 2",120000,89000),
                new MenuItem("Benefactor XLS",151000,11000),
                new MenuItem("Benefactor Stirling GT",250000,125000),
                new MenuItem("Benefactor Streiter",156000,105000),
                new MenuItem("Benefactor Schlagen GT",500000,250000),
                new MenuItem("Benefactor Krieger",750000,500000),
                new MenuItem("Benefactor Vorschlaghammer",55000,23000),
            }),
            new ShopMenu("VapidMenu","Vapid",new List<MenuItem>() {
                new MenuItem("Vapid Stanier",28000, 12000),
                new MenuItem("Vapid Minivan",29000, 12500),
                new MenuItem("Vapid Minivan Custom",30500,13000),
                new MenuItem("Vapid Speedo",31000,13500),
                new MenuItem("Vapid Speedo Custom",31500,14000),
                new MenuItem("Vapid Radius",32000,15000),
                new MenuItem("Vapid Sadler",38000,15500),
                new MenuItem("Vapid Sandking SWB",41000,22000),
                new MenuItem("Vapid Sandking XL",45000,23000),
                new MenuItem("Vapid Caracara 4x4",65000,34000),
                new MenuItem("Vapid Contender",82000,45000),
                new MenuItem("Vapid Guardian",85000,46000),
                new MenuItem("Vapid Trophy Truck",50000,36000),
                new MenuItem("Vapid Dominator",55000,33000),
                new MenuItem("Vapid Dominator ASP",75000,50000),
                new MenuItem("Vapid Dominator GTT",95000,70000),
                new MenuItem("Vapid Dominator GTX",105000,82000),
                new MenuItem("Vapid Flash GT",65000,47000),
                new MenuItem("Vapid FMJ",75000,58000),
                new MenuItem("Vapid Bullet",155000,105050),
                new MenuItem("Vapid Aleutian",55000,25000),
                new MenuItem("Vapid Dominator GT",85000,45000),
                new MenuItem("Vapid Dominator FX",19000,5000),

                new MenuItem("Vapid Firebolt ASP",45000,19000),
                new MenuItem("Vapid Uranus LozSpeed",15000,5000),

                //Dundreary
                new MenuItem("Dundreary Landstalker",34000,12000),
                new MenuItem("Dundreary Landstalker XL",49000,18000),
            }),
            new ShopMenu("HelmutMenu","Helmut",new List<MenuItem>() {
                new MenuItem("BF Surfer",11000, 6000),
                new MenuItem("BF Injection",16000,8000),
                new MenuItem("BF Bifta",29000,19000),
                new MenuItem("BF Raptor",32000,20000),
                new MenuItem("BF Weevil",35000,21000),
            }),
            new ShopMenu("SandersMenu","Sanders",new List<MenuItem>() {
                new MenuItem("Maibatsu Sanchez",7000,4500),
                new MenuItem("Maibatsu Sanchez Custom",7500,3200),
                new MenuItem("Maibatsu Manchez",9500,4600),
                new MenuItem("Maibatsu Manchez Scout",9600,4500),

                new MenuItem("Shitzu PCJ 600",9000,3900),
                new MenuItem("Shitzu Vader",9500,4200),
                new MenuItem("Shitzu Defiler",34000,15340),
                new MenuItem("Shitzu Hakuchou",19000,11200),
                new MenuItem("Shitzu Hakuchou Drag",25000,12500),

                new MenuItem("Dinka Enduro",6500,4300),
                new MenuItem("Dinka Akuma",10000,5000),
                new MenuItem("Dinka Double-T",12000,7500),
                new MenuItem("Dinka Thrust",13000,8000),
                new MenuItem("Dinka Vindicator",10000,5000),

                new MenuItem("Principe Nemesis",13500,6700),
                new MenuItem("Principe Diabolus",15000,8000),
                new MenuItem("Principe Diabolus Custom",17000,10500),
                new MenuItem("Principe Lectro",18000,12400),

                new MenuItem("Nagasaki Carbon RS",40000,22500),
                new MenuItem("Nagasaki BF400",12000,6200),

                //NEW
                new MenuItem("LCC Hexer",37000,23000),
                new MenuItem("LCC Innovation",35000,21000),

                new MenuItem("Western Zombie Bobber",40000,26000),
                new MenuItem("Western Zombie Chopper",35000,23000),
                new MenuItem("Western Daemon",26000,19000),
                new MenuItem("Western Daemon LOST",29000,20000),
                new MenuItem("Western Bagger",19000,9500),

            }),
            new ShopMenu("LuxuryAutosMenu","Luxury Autos",new List<MenuItem>() {//pegassi/grotti/enus/buckingham/pfiuster
                new MenuItem("Enus Huntley S",119000,65000),
                new MenuItem("Enus Cognoscenti Cabrio",185000,120000),
                new MenuItem("Enus Super Diamond",235000,189000),
                new MenuItem("Enus Cognoscenti 55",150000,105000),
                new MenuItem("Enus Cognoscenti",250000,125000),
                new MenuItem("Enus Cognoscenti (Armored)",500000,250000),
                new MenuItem("Enus Paragon R",256000,125000),
                new MenuItem("Enus Paragon R (Armored)",550000,340000),
                new MenuItem("Enus Windsor",634000,450000),
                new MenuItem("Enus Windsor Drop",655000,467000),
                new MenuItem("Enus Paragon S",295000,120000),

                new MenuItem("Grotti Carbonizzare",78000,56000),
                new MenuItem("Grotti Stinger",95000,76000),
                new MenuItem("Grotti Stinger GT",98000,77000),
                new MenuItem("Grotti Cheetah",240000,189000),
                new MenuItem("Grotti Bestia GTS",134000,98000),
                new MenuItem("Grotti Cheetah Classic",334500,259000),
                new MenuItem("Grotti Furia",255000,167000),
                new MenuItem("Grotti Itali GTO",342000,278000),
                new MenuItem("Grotti Itali RSX",545600,345000),
                new MenuItem("Grotti X80 Proto",567000,453000),
                new MenuItem("Grotti Turismo Classic",100000,75000),
                new MenuItem("Grotti Turismo R",150000,86000),
                new MenuItem("Grotti Visione",676500,450000),
                new MenuItem("Grotti Turismo Omaggio",325000,125000),

                new MenuItem("Pfister Comet",100000,78000),
                new MenuItem("Pfister Comet Retro Custom",130000,65000),
                new MenuItem("Pfister Comet Safari",135000,95000),
                new MenuItem("Pfister Comet SR",155000,115000),
                new MenuItem("Pfister Comet S2",165000,120000),
                new MenuItem("Pfister Growler",167000,98000),
                new MenuItem("Pfister Neon",177000,122000),
                new MenuItem("Pfister 811",189000,105000),

                new MenuItem("Pegassi Faggio",5000,1500),
                new MenuItem("Pegassi Faggio Sport",5500,2000),
                new MenuItem("Pegassi Faggio Mod",6000,2300),
                new MenuItem("Pegassi Ruffian",9900,3000),
                new MenuItem("Pegassi Bati 801",15000,7500),
                new MenuItem("Pegassi Bati 801RR",16000,7000),
                new MenuItem("Pegassi Esskey",12000,6000),
                new MenuItem("Pegassi Vortex",20000,13000),
                new MenuItem("Pegassi Monroe",21000,14000),
                new MenuItem("Pegassi Vacca",220000,100000),
                new MenuItem("Pegassi Infernus",340000,225000),
                new MenuItem("Pegassi Zentorno",725000,567000),
                new MenuItem("Pegassi Osiris",950000,700000),
                new MenuItem("Pegassi FCR 1000",15000,8000),
                new MenuItem("Pegassi FCR 1000 Custom",18000,12000),
                new MenuItem("Pegassi Reaper",956000,670000),
                new MenuItem("Pegassi Tempesta",1001000,780000),
                new MenuItem("Pegassi Tezeract",1200000,900000),
                new MenuItem("Pegassi Toros",89000,62000),
                new MenuItem("Pegassi Zentorno",725000,600000),
                new MenuItem("Pegassi Zorrusso",1250000,1000000),

                //Obey
                new MenuItem("Obey 10F",89000,45000),
                new MenuItem("Obey Tailgater",56000,24000),
                new MenuItem("Obey Tailgater S",66000,26000),
                new MenuItem("Obey Rocoto",34000,23000),
                new MenuItem("Obey Omnis e-GT",89000,45000),
                new MenuItem("Obey I-Wagen",99000,34000),
                new MenuItem("Obey 9F",45000,19000),
                new MenuItem("Obey 9F Cabrio",47000,20000),
                new MenuItem("Obey 8F Drafter",90000,34000),

                //Ocelot
                new MenuItem("Ocelot F620",89000,45000),
                new MenuItem("Ocelot Jackal",45000,22000),
                new MenuItem("Ocelot Jugular",95000,42000),
                new MenuItem("Ocelot Lynx",103000,34000),
                new MenuItem("Ocelot Pariah",150000,76000),
                new MenuItem("Ocelot Penetrator",160000,79000),
                new MenuItem("Ocelot Virtue",170000,74000),
                new MenuItem("Ocelot XA-21",190000,89000),
                new MenuItem("Ocelot Swinger",110000,49000),

                //Overflod
                new MenuItem("Overflod Pipistrello",1500000,324000),

                //Coil
                new MenuItem("Coil Voltic",90000,45000),
                new MenuItem("Coil Cyclone",120000,56000),
                new MenuItem("Coil Raiden",135000,67000),

                //Lampadati
                new MenuItem("Lampadati Cinquemila",135000,67000),
                new MenuItem("Lampadati Felon",67000,23000),
                new MenuItem("Lampadati Felon GT",75000,24000),
                new MenuItem("Lampadati Furore GT",156000,67000),
                new MenuItem("Lampadati Komoda",96000,23000),
                new MenuItem("Lampadati Novak",110000,45000),

                //Ubermacht
                new MenuItem("Ubermacht Oracle XS",45000,20000),
                new MenuItem("Ubermacht Oracle",40000,19000),
                new MenuItem("Ubermacht Revolter",150000,67000),
                new MenuItem("Ubermacht SC1",178000,100000),
                new MenuItem("Ubermacht Sentinel XS",37000,21000),
                new MenuItem("Ubermacht Sentinel 2",35000,19000),//regular
                new MenuItem("Ubermacht Sentinel 3",25000,12000),//classic
                new MenuItem("Ubermacht Zion",35000,17000),
                new MenuItem("Ubermacht Zion Cabrio",37000,19000),
                new MenuItem("Ubermacht Zion Classic",30000,20000),
                new MenuItem("Ubermacht Cypher",124000,78000),
                new MenuItem("Ubermacht Rebla GTS",87000,34000),
                new MenuItem("Ubermacht Rhinehart",95000,34000),
                new MenuItem("Ubermacht Sentinel Classic Widebody",50000),
                new MenuItem("Ubermacht Niobe",168000,101000),

            }),
            new ShopMenu("BravadoMenu","Bravado",new List<MenuItem>() {
                new MenuItem("Bravado Youga",26000),
                new MenuItem("Bravado Gresley",29000),
                new MenuItem("Bravado Bison",30000),
                new MenuItem("Bravado Bison 2",30500),
                new MenuItem("Bravado Bison 3",31000),
                new MenuItem("Bravado Gauntlet",32000),
                new MenuItem("Bravado Buffalo",35000),
                new MenuItem("Bravado Buffalo S",65000),
                new MenuItem("Bravado Banshee",105000),
                new MenuItem("Bravado Banshee 900R",150000),
                new MenuItem("Bravado Gauntlet Hellfire",120000,56000),
                new MenuItem("Bravado Buffalo EVX",178000,78000),
                new MenuItem("Bravado Gauntlet Classic",89000,45000),
                new MenuItem("Bravado Verlierer",250000,120000),
                new MenuItem("Bravado Dorado",24000,12000),
            }),
            new ShopMenu("KarinMenu","Karin",new List<MenuItem>() {
                new MenuItem("Karin Futo",12000,7800),
                new MenuItem("Karin Rebel",19000,12340),
                new MenuItem("Karin BeeJay XL",29000,17800),
                new MenuItem("Karin Dilettante",35000,22000),
                new MenuItem("Karin Asterope",36500,22500),
                new MenuItem("Karin Sultan Classic",36750),
                new MenuItem("Karin Sultan",37000,24000),
                new MenuItem("Karin Sultan RS",38000,25000),
                new MenuItem("Karin Intruder",38000,25000),
                new MenuItem("Karin Previon",39000,26000),
                new MenuItem("Karin Everon",44000,28000),
                new MenuItem("Karin Kuruma",45000,31000),
                new MenuItem("Karin Vivanite",37000,12000),
                new MenuItem("Karin Asterope GZ",38000,23500),
            }),
            new ShopMenu("PremiumDeluxeMenu","PremiumDeluxe",new List<MenuItem>() {
                new MenuItem("Bravado Youga",26000, 16000),
                new MenuItem("Bravado Gresley",29000, 18000),
                new MenuItem("Bravado Bison",30000, 22000),
                new MenuItem("Bravado Bison 2",30500,21000),
                new MenuItem("Bravado Bison 3",31000, 25000),
                new MenuItem("Bravado Gauntlet",32000,28000),
                new MenuItem("Bravado Buffalo",35000,27000),
                new MenuItem("Bravado Buffalo S",65000,55000),
                new MenuItem("Bravado Banshee",105000,78000),
                new MenuItem("Bravado Banshee 900R",150000,89000),
                new MenuItem("Bravado Gauntlet Hellfire",120000,56000),
                new MenuItem("Bravado Buffalo EVX",178000,78000),
                new MenuItem("Bravado Gauntlet Classic",89000,45000),
                new MenuItem("Bravado Verlierer",250000,120000),
                new MenuItem("Bravado Dorado",24000,12000),
                new MenuItem("Bravado Banshee GTS",95000,32000),

                new MenuItem("Karin Futo",12000,8000),
                new MenuItem("Karin Rebel",19000,9500),
                new MenuItem("Karin BeeJay XL",29000,17000),
                new MenuItem("Karin Dilettante",35000,18000),
                new MenuItem("Karin Asterope",36500,22000),
                new MenuItem("Karin Sultan Classic",36750,21500),
                new MenuItem("Karin Sultan",37000,28000),
                new MenuItem("Karin Sultan RS",38000,32000),
                new MenuItem("Karin Intruder",38000,32000),
                new MenuItem("Karin Previon",39000,34500),
                new MenuItem("Karin Everon",44000,35000),
                new MenuItem("Karin Kuruma",45000,36000),
                new MenuItem("Karin Vivanite",37000,12000),
                new MenuItem("Karin Asterope GZ",38000,23500),

                //Annis
                new MenuItem("Annis Euros X32",25000,14000),
                new MenuItem("Annis 300R",36000,16000),
                new MenuItem("Annis Elegy RH8",96000,46000),
                new MenuItem("Annis Euros",38000,19000),
                new MenuItem("Annis Hellion",23000,8000),
                new MenuItem("Annis ZR350",29000,9500),

                //Bollokan
                new MenuItem("Bollokan Prairie",15000,6000),
                new MenuItem("Bollokan Envisage",267000,124000),

                //Emperor
                new MenuItem("Emperor Habanero",19000,8000),
                new MenuItem("Emperor Vectre",85000,35000),

                //Imponte?
                new MenuItem("Imponte Deluxo",48000,23000),
                new MenuItem("Imponte Nightshade",45000,30000),
                new MenuItem("Imponte Dukes",27000,18000),
                new MenuItem("Imponte Phoenix",22000,10000),
                new MenuItem("Imponte Ruiner",25000,12000),
                new MenuItem("Imponte Ruiner ZZ-8",45000,13000),

                //Invetero
                new MenuItem("Invetero Coquette", 50000,20000),// { ModelName = "coquette" },
                new MenuItem("Invetero Coquette Classic", 70000,30000),// { ModelName = "coquette2" },
                new MenuItem("Invetero Coquette BlackFin", 80000,40000),// { ModelName = "coquette3" },
                new MenuItem("Invetero Coquette D10", 150000,80000),// { ModelName = "coquette4" },
                new MenuItem("Invetero Coquette D1",250000,95000),
                new MenuItem("Invetero Coquette D5",45000,23000),

                //Cheval
                new MenuItem("Cheval Fugitive",25000,12000),
                new MenuItem("Cheval Surge",29000,13000),
                new MenuItem("Cheval Picador",19000,10000),

                //Dinka
                new MenuItem("Dinka Jester RR Widebody",150000,45000),
                new MenuItem("Dinka Chavos V6",34000,19000),

                //Declasse
                new MenuItem("Declasse Asea",19000,8000),
                new MenuItem("Declasse Rancher XL",18000,9000),
                new MenuItem("Declasse Vigero",20000,7000),
                new MenuItem("Declasse Premier",23000,9000),
                new MenuItem("Declasse Granger",39000,19000),
                new MenuItem("Declasse Granger 3600LX",45000,23000),
                new MenuItem("Declasse Stallion",34000,12000),
                new MenuItem("Declasse Moonbeam",19000,3000),
                new MenuItem("Declasse Vigero ZX",96000,56000),
                new MenuItem("Declasse Walton L35",24000,12000),
                new MenuItem("Declasse Vigero ZX Convertible",75000,35000),
                new MenuItem("Declasse Impaler SZ",34000,15000),
                new MenuItem("Declasse Impaler LX",35000,16000),
                new MenuItem("Declasse Yosemite 1500",17000,8000),

                //Fathom
                new MenuItem("Fathom FR36",45000,23000),
                new MenuItem("Fathom FQ 2",26000,13000),

                //Canis
                new MenuItem("Canis Terminus",49000,25000),
                new MenuItem("Canis Castigator",24000,12000),
                new MenuItem("Canis Mesa",21000,8000),
                new MenuItem("Canis Kamacho",45000,23000),
                new MenuItem("Canis Seminole",29000,13000),
                new MenuItem("Canis Seminole Frontier",22000,12000),

                //Zirconium
                new MenuItem("Zirconium Stratum",21000, 11000),

                //Schyster
                new MenuItem("Schyster Fusilade",25000, 12000),
                new MenuItem("Schyster Deviant",29000, 11000),


                //Vulcar
                new MenuItem("Vulcar Fagaloa",17000, 5000),
                new MenuItem("Vulcar Ingot",19000, 7000),
                new MenuItem("Vulcar Nebula Turbo",21000, 11100),
                new MenuItem("Vulcar Warrener",22000, 11500),
                new MenuItem("Vulcar Warrener HKR",25000, 12000),
            }),
            new ShopMenu("AlbanyMenu","Albany",new List<MenuItem>() {
                new MenuItem("Albany Alpha",45000,27500),
                new MenuItem("Albany Buccaneer",29000,19500),
                new MenuItem("Albany Buccaneer Custom",49000,29500),
                new MenuItem("Albany Cavalcade",45000,27500),
                new MenuItem("Albany Cavalcade 2",50000,25000),
                new MenuItem("Albany Emperor",25000,17500),
                new MenuItem("Albany Manana",28000,19500),
                new MenuItem("Albany Manana Custom",42400,21200),
                new MenuItem("Albany Primo",35000,17500),
                new MenuItem("Albany Primo Custom",55000,27500),
                new MenuItem("Albany Virgo",36000,18000),
                new MenuItem("Albany V-STR",130000,65000),
                new MenuItem("Albany Washington",48000,19000),
                new MenuItem("Albany Cavalcade XL",65000,37500),
            }),
            new ShopMenu("LarrysRVMenu","Larry's RV",new List<MenuItem>() {
                new MenuItem("Zirconium Journey",25000, 15000),
                new MenuItem("Declasse Burrito",35000, 25000),
                new MenuItem("BF Surfer",65000, 45000),
                new MenuItem("Brute Camper",145000, 95000),


                new MenuItem("Vapid Tow Truck Full",34000,12000),
                new MenuItem("Vapid Tow Truck Utility",31000,10000),



                //new MenuItem("Vapid Sheriff Stanier",500,5),
                //new MenuItem("Declasse Sheriff Granger",500,5),
                //new MenuItem("Vapid Police Stanier",500,5),
                //new MenuItem("Bravado Police Buffalo",500,5),
                //

            }),
            new ShopMenu("GetAweighMenu","Get Aweigh",new List<MenuItem>() {

                //Boat
                new MenuItem("Dinka Marquis",240000),
                new MenuItem("Lampadati Toro",190000),
                new MenuItem("Nagasaki Dinghy",45000),
                new MenuItem("Nagasaki Dinghy 2",50000),
                new MenuItem("Pegassi Speeder",185000),

                new MenuItem("Shitzu Jetmax",250000),
                new MenuItem("Shitzu Longfin",275000),
                new MenuItem("Shitzu Squalo",85000),
                new MenuItem("Shitzu Suntrap",35000),
                new MenuItem("Shitzu Tropic",75000),

                //Jetski
                new MenuItem("Speedophile Seashark",5000),
                //new MenuItem("Speedophile Seashark 2",6000),
                new MenuItem("Speedophile Seashark 3",7000),
            }),
            new ShopMenu("ElitasMenu","PlaneMan",new List<MenuItem>() {

                //Heli

                new MenuItem("Buckingham Conada",2500000),

                new MenuItem("Sparrow",175000),
                new MenuItem("Sea Sparrow",185000),

                new MenuItem("Nagasaki Havok",155000),

                new MenuItem("Buckingham Volatus",1200000),

                new MenuItem("Buckingham SuperVolito Carbon",2100000),
                new MenuItem("Buckingham SuperVolito",2000000),

                new MenuItem("Buckingham Swift Deluxe",1600000),
                new MenuItem("Buckingham Swift",1500000),

                new MenuItem("Western Cargobob",5500000),
                new MenuItem("HVY Skylift",6500000),


                new MenuItem("Buckingham Maverick",1100000),




                new MenuItem("Maibatsu Frogger",1400000),
                new MenuItem("Nagasaki Buzzard",1250000),



                //Airplane
                new MenuItem("Buckingham Alpha-Z1",350000),
                new MenuItem("Buckingham Howard NX-25",650000),
                new MenuItem("Buckingham Luxor",5000000),
                new MenuItem("Buckingham Luxor Deluxe",5500000),
                new MenuItem("Buckingham Miljet",3200000),
                new MenuItem("Buckingham Nimbus",2900000), 
                new MenuItem("Buckingham Shamal",1800000),
                new MenuItem("Buckingham Vestra",650000),
                new MenuItem("Mammoth Dodo",145000),
                new MenuItem("Nagasaki Ultralight",45000),
                new MenuItem("Western Seabreeze",750000),



                new MenuItem("JoBuilt Mammatus",320000),
                new MenuItem("JoBuilt Velum",550000),
            }),
            new ShopMenu("LowriderMenu","Benny's Motorworks Menu",new List<MenuItem>() {
                new MenuItem("Albany Buccaneer Custom",55000, 25000),
                new MenuItem("Albany Manana Custom",45000, 27000),
                new MenuItem("Benefactor Glendale Custom",39000, 15000),
                new MenuItem("Declasse Tornado Custom",54000, 14000),
                new MenuItem("Declasse Voodoo Custom",43000, 20000),
                new MenuItem("Dundreary Virgo Classic Custom",55000, 25000),
                new MenuItem("Vapid Chino Custom",45000, 20000),
            }),
            new ShopMenu("OutlawMotorMenu","Outlaw Motors Menu",new List<MenuItem>() {
                new MenuItem("Western Bagger",25000, 12500),
                new MenuItem("LCC Hexer",37000, 23000),
                new MenuItem("LCC Innovation",35000, 21000),
                new MenuItem("Western Zombie Bobber",40000, 26000),
                new MenuItem("Western Zombie Chopper",35000, 23000),
                new MenuItem("Western Daemon",26000, 19000),
                new MenuItem("Vapid Chino Custom",45000, 20000),
            }),
        });
    }
    private void SpecificVehicleExporters()
    {
        PossibleShopMenus.ShopMenuList.AddRange(new List<ShopMenu>{
        new ShopMenu("SunshineMenu", "Sunshine", new List<MenuItem>() {
                new MenuItem("Vapid Dominator",55000,10000),

                new MenuItem("Albany Alpha",0,8500),
                new MenuItem("Albany Cavalcade",0,9500),
                new MenuItem("Albany Cavalcade 2",0,9000),
                new MenuItem("Albany Washington",0,5000),

                new MenuItem("Bravado Buffalo",0,7000),
                new MenuItem("Bravado Buffalo S",0,7900),
                new MenuItem("Bravado Banshee",0,25000),
                new MenuItem("Bravado Banshee 900R",0,30000),
                new MenuItem("Karin BeeJay XL",0,5500),
                new MenuItem("Karin Kuruma",0,12000),

                new MenuItem("Benefactor Surano",0,9500),
                new MenuItem("Benefactor Serrano",0,4500),
                new MenuItem("Benefactor Dubsta",0,21000),
                new MenuItem("Benefactor Dubsta 2",0,23000),

                new MenuItem("Gallivanter Baller",0,17000),

                new MenuItem("Enus Cognoscenti",0,55000),

                new MenuItem("Pfister Comet",0,27000),

                new MenuItem("Pfister Neon",0,57000),
                //new MenuItem("Pfister 811",0,34000),

                new MenuItem("Pegassi Osiris",0,18000),
                new MenuItem("Pegassi FCR 1000",0,2300),
                new MenuItem("Pegassi Reaper",0,17500),
                new MenuItem("Pegassi Tempesta",0,220000),
                new MenuItem("Pegassi Tezeract",0,230000),
            }),

        new ShopMenu("NationalMenu", "National", new List<MenuItem>() {
                new MenuItem("Karin Sultan",0,5600),
                new MenuItem("Karin Sultan RS",0,6700),

                new MenuItem("Bravado Gauntlet",0,8600),
                new MenuItem("Bravado Buffalo",0,10500),
                new MenuItem("Bravado Buffalo S",0,12500),
                new MenuItem("Bravado Banshee",0,22000),

                new MenuItem("Vapid Dominator ASP",0,14000),
                new MenuItem("Vapid Dominator GTT",0,19000),
                new MenuItem("Vapid Dominator GTX",0,26000),
                new MenuItem("Vapid Flash GT",0,12000),
                new MenuItem("Vapid FMJ",0,15000),
                new MenuItem("Vapid Bullet",0,43000),

                new MenuItem("Gallivanter Baller",0,16000),
                new MenuItem("Gallivanter Baller 2",0,1900),
                new MenuItem("Gallivanter Baller LE",0,22000),
                new MenuItem("Gallivanter Baller LE LWB",0,23000),


                new MenuItem("Benefactor Schafter",0,6800),
                new MenuItem("Benefactor Schafter V12",0,21500),
                new MenuItem("Benefactor Feltzer",0,19000),
                new MenuItem("Benefactor Schwartzer",0,5000),




                new MenuItem("Benefactor Streiter",0,35000),
                new MenuItem("Benefactor Schlagen GT",0,120000),
                new MenuItem("Benefactor Krieger",0,156000),

                new MenuItem("Enus Huntley S",0,21000),
                new MenuItem("Enus Cognoscenti Cabrio",0,46000),
                new MenuItem("Enus Super Diamond",0,50000),
                new MenuItem("Enus Cognoscenti 55",0,34000),
                new MenuItem("Enus Cognoscenti",0,23000),
                new MenuItem("Enus Paragon R",0,13000),
                new MenuItem("Enus Windsor",0,180000),
                new MenuItem("Enus Windsor Drop",0,170000),

                new MenuItem("Pegassi Bati 801",0,2500),
                new MenuItem("Pegassi Bati 801RR",0,2000),
                new MenuItem("Pegassi Esskey",0,1500),
                new MenuItem("Pegassi Infernus",0,67000),
                new MenuItem("Pegassi Zentorno",0,145000),

                new MenuItem("Pegassi Toros",0,12000),
                new MenuItem("Pegassi Zentorno",0,200000),
                new MenuItem("Pegassi Zorrusso",0,250000),

            }),

        new ShopMenu("PaletoExportMenu", "Paleto Exports", new List<MenuItem>() {
                new MenuItem("Bravado Youga",0, 5000),
                new MenuItem("Bravado Gresley",0, 5500),
                new MenuItem("Bravado Bison",0, 6500),
                new MenuItem("Bravado Gauntlet",0,7800),
                new MenuItem("Bravado Buffalo S",0,8900),


                new MenuItem("Karin Futo",0,2000),
                new MenuItem("Karin Rebel",0,5600),
                new MenuItem("Karin BeeJay XL",0,2800),
                new MenuItem("Karin Dilettante",0,2400),
                new MenuItem("Karin Asterope",0,3400),
                new MenuItem("Vapid Stanier",0, 3400),
                new MenuItem("Vapid Minivan",0, 2500),

                new MenuItem("Benefactor Schwartzer",0,4000),

                new MenuItem("BF Surfer",0, 1500),
                new MenuItem("BF Injection",0,2000),
            }),


        new ShopMenu("JDM-X", "JDMX", new List<MenuItem>() {
                new MenuItem("Karin Sultan RS Classic",0,18000),
                new MenuItem("Karin Sultan RS",0,16000),
                new MenuItem("Karin Sultan Classic",0,13000),
                new MenuItem("Karin Sultan",0,11000),
                new MenuItem("Karin Futo",0,5000),
                new MenuItem("Karin Futo GTX",0,13000),
                new MenuItem("Dinka Blista Kanjo",0,10000),
                new MenuItem("Dinka Blista Compact",0,5500),
                new MenuItem("Karin 190z",0,25000),
                new MenuItem("Maibatsu Penumbra",0,5000),
                new MenuItem("Maibatsu Penumbra FF",0,15000),
                new MenuItem("Annis ZR350",0,35000),
                new MenuItem("Dinka RT3000",0,18000),
                new MenuItem("Annis Remus",0,16000),
                new MenuItem("Karin Previon",0,10000),
                new MenuItem("Annis Elegy RH8",0,75000),
                new MenuItem("Annis Elegy Retro Custom",0,90000),
                new MenuItem("Annis Euros",0,10000),
                new MenuItem("Karin Kuruma",0,15000),
                new MenuItem("Dinka Jester",0,100000),
                new MenuItem("Dinka Jester (Racecar)",0,125000),
                new MenuItem("Emperor ETR1",0,200000),
                new MenuItem("Dinka Jester Classic",0,60000),
                new MenuItem("Dinka Jester RR",0,40000),
            }),


        });






    }
    private void SpecificVendingMachines()
    {
        PossibleShopMenus.ShopMenuList.AddRange(new List<ShopMenu>{
            new ShopMenu("CandyVendingMenu","Candybox Machine",new List<MenuItem>() {
                new MenuItem("Sticky Rib Phat Chips", 2),
                new MenuItem("Habanero Phat Chips", 2),
                new MenuItem("Supersalt Phat Chips", 2),
                new MenuItem("Big Cheese Phat Chips", 2),
                new MenuItem("Ego Chaser Energy Bar", 2),
                new MenuItem("King Size P's & Q's", 3),
                new MenuItem("P's & Q's", 2),
                new MenuItem("Meteorite Bar", 2),
            //new MenuItem("Hawk & Little PTF092F", 550),
            }) { BannerOverride = "candybox.png" },
            new ShopMenu("WaterVendingMenu","Raine Machine",new List<MenuItem>() {
                new MenuItem("Bottle of Raine Water", 2) }) { BannerOverride = "raine.png" },
            new ShopMenu("SprunkVendingMenu","Sprunk Machine",new List<MenuItem>() {
                new MenuItem("Can of Sprunk", 1),
                new MenuItem("Can of eCola", 1),
                new MenuItem("Can of Orang-O-Tang", 1),
                new MenuItem("Bottle of JUNK Energy", 2),
                new MenuItem("Bottle of Raine Water", 2),
                new MenuItem("Flow Water ZERO", 3) }) { BannerOverride = "sprunk.png" },
            new ShopMenu("eColaVendingMenu","eCola Machine",new List<MenuItem>() {
                new MenuItem("Can of Sprunk", 1),
                new MenuItem("Can of eCola", 1),
                new MenuItem("Can of Orang-O-Tang", 1),
                new MenuItem("Bottle of Raine Water", 2), }) { BannerOverride = "ecola.png" },
            new ShopMenu("BeanMachineVendingMenu","Bean Machine",new List<MenuItem>() {
                new MenuItem("High Noon Coffee", 2) }) { BannerOverride = "beanmachine.png" },
            new ShopMenu("CigVendingMenu","Cigarette Machine",new List<MenuItem>() {
                new MenuItem("Redwood Regular", 30),
                new MenuItem("Redwood Mild", 32),
                new MenuItem("Debonaire", 35),
                new MenuItem("Debonaire Menthol", 38),
                new MenuItem("Caradique", 35),
                new MenuItem("69 Brand", 40),
                new MenuItem("Estancia Cigar", 50),
                new MenuItem("DIC Lighter", 5), }) { BannerOverride = "redwood.png" },
        });
    }
    private void SpecificWeaponsShops()
    {
        PossibleShopMenus.ShopMenuList.AddRange(new List<ShopMenu>{
            new ShopMenu("AmmunationMenu","Ammunation",new List<MenuItem>() {
                new MenuItem("Hawk & Little PTF092F",550),
                new MenuItem("Hawk & Little Thunder",650),
                new MenuItem("Hawk & Little Combat Pistol",950),
                new MenuItem("Hawk & Little Desert Slug",1500),
                new MenuItem("Hawk & Little 1919",1200),
                new MenuItem("Hawk & Little Raging Mare",1700),
                new MenuItem("Hawk & Little Raging Mare Dx",1950),
                new MenuItem("Vom Feuer P69",790),
                new MenuItem("Vom Feuer SCRAMP",990),
                new MenuItem("Shrewsbury S7",1100),
                new MenuItem("Shrewsbury S7A",1200),
                new MenuItem("Coil Tesla",550),
                new MenuItem("BS M1922",995),

            }),
            new ShopMenu("GunVendorMenu", "Gun Vendor", new List<MenuItem>() {
                new MenuItem("Shrewsbury 420 Sawed-Off",340) { IsIllicilt = true },
                new MenuItem("Hawk & Little PTF092F",680) { IsIllicilt = true },
                new MenuItem("Shrewsbury Defender",1200) { IsIllicilt = true },
                new MenuItem("Toto 12 Guage Sawed-Off",430) { IsIllicilt = true },
                new MenuItem("M61 Grenade",1000) { IsIllicilt = true },
                new MenuItem("G.E.S. Baseball Bat",70) { IsIllicilt = true },
                new MenuItem("Flint Crowbar",35) { IsIllicilt = true },
                new MenuItem("ProLaps Five Iron Golf Club",150) { IsIllicilt = true },
                new MenuItem("Flint Hammer",25) { IsIllicilt = true },
                new MenuItem("Flint Hatchet",80) { IsIllicilt = true },
                new MenuItem("Brass Knuckles",200) { IsIllicilt = true },
                new MenuItem("Combat Knife",120) { IsIllicilt = true },
                new MenuItem("Vom Feuer Machete",29) { IsIllicilt = true },
                new MenuItem("Switchblade",300) { IsIllicilt = true },
                new MenuItem("Shrewsbury Luzi",956) { IsIllicilt = true },
                new MenuItem("Vom Feuer KEK-9",565) { IsIllicilt = true },
                new MenuItem("Shrewsbury A7-4K",856) { IsIllicilt = true } }),
        });
    }
    private void DrugDealerMenus()
    {






        ShopMenuGroup MarijuanaDealerMenuGroup = new ShopMenuGroup(StaticStrings.MarijuanaDealerMenuGroupID, "Marijuana Dealer Menus", StaticStrings.DrugDealerMenuID, new List<PercentageSelectShopMenu>()
        {
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugDealerMenuID, "Marijuana Dealer 1",  new List<MenuItem>() {
                    new MenuItem("Marijuana",20, 12) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 18, NumberOfItemsToSellToPlayer = 18  }}),10),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugDealerMenuID, "Marijuana Dealer 2", new List<MenuItem>() {
                    new MenuItem("Marijuana", 19, 11) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 13, NumberOfItemsToSellToPlayer = 13 } }),10),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugDealerMenuID, "Marijuana Dealer 3", new List<MenuItem>() {
                    new MenuItem("Marijuana", 18, 10) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 16, NumberOfItemsToSellToPlayer = 15 } }),10),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugDealerMenuID, "Marijuana Dealer 4", new List<MenuItem>() {
                    new MenuItem("Marijuana", 17, 12) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 13, NumberOfItemsToSellToPlayer = 17 } }),10),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugDealerMenuID, "Marijuana Dealer 5", new List<MenuItem>() {
                    new MenuItem("Marijuana", 16, 11) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 10, NumberOfItemsToSellToPlayer = 13 } }),10),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugDealerMenuID, "Marijuana Dealer 6", new List<MenuItem>() {
                    new MenuItem("Marijuana",15, 10) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 7, NumberOfItemsToSellToPlayer = 14 } }),10),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugDealerMenuID, "Marijuana Dealer 7", new List<MenuItem>() {
                    new MenuItem("Marijuana",14, 12) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 13, NumberOfItemsToSellToPlayer = 13 } }),10),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugDealerMenuID, "Marijuana Dealer 8", new List<MenuItem>() {
                    new MenuItem("Marijuana",13, 11) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 16, NumberOfItemsToSellToPlayer = 13 } }),10),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugDealerMenuID, "Marijuana Dealer 9", new List<MenuItem>() {
                    new MenuItem("Marijuana",20, 10) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 15, NumberOfItemsToSellToPlayer = 20 } }),10),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugDealerMenuID, "Marijuana Dealer 10", new List<MenuItem>() {
                    new MenuItem("Marijuana",19, 12) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 14, NumberOfItemsToSellToPlayer = 17 } }),10),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugDealerMenuID, "Marijuana Dealer 11", new List<MenuItem>() {
                    new MenuItem("Marijuana",18, 11) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 17, NumberOfItemsToSellToPlayer = 11 } }),10),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugDealerMenuID, "Marijuana Dealer 12", new List<MenuItem>() {
                    new MenuItem("Marijuana",17, 10) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 10, NumberOfItemsToSellToPlayer = 10 } }),10),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugDealerMenuID, "Marijuana Dealer 13", new List<MenuItem>() {
                    new MenuItem("Marijuana",16, 12) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 9, NumberOfItemsToSellToPlayer = 17 } }),10),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugDealerMenuID, "Marijuana Dealer 14", new List<MenuItem>() {
                    new MenuItem("Marijuana",15, 11) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 10, NumberOfItemsToSellToPlayer = 9 }}),10),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugDealerMenuID, "Marijuana Dealer 15", new List<MenuItem>() {
                    new MenuItem("Marijuana",14, 10) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 11, NumberOfItemsToSellToPlayer = 11 }}),10),
         });
        PossibleShopMenus.ShopMenuGroupList.Add(MarijuanaDealerMenuGroup);

        ShopMenuGroup ToiletCleanerDealerMenuGroup = new ShopMenuGroup(StaticStrings.ToiletCleanerDealerMenuGroupID, "Toilet Cleaner Dealer Menus", StaticStrings.DrugDealerMenuID, new List<PercentageSelectShopMenu>()
        {
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugDealerMenuID, "Toilet Dealer 1", new List<MenuItem>() {
                    new MenuItem("Toilet Cleaner",27, 17) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 12, NumberOfItemsToSellToPlayer = 15 } }),30),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugDealerMenuID, "Toilet Dealer 2", new List<MenuItem>() {
                    new MenuItem("Toilet Cleaner",26, 18) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 12, NumberOfItemsToSellToPlayer = 16 } }),30),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugDealerMenuID, "Toilet Dealer 3", new List<MenuItem>() {
                    new MenuItem("Toilet Cleaner",25, 16) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 15, NumberOfItemsToSellToPlayer = 12 },
                    new MenuItem("Shrewsbury Luzi",956) { IsIllicilt = true }, }),10),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugDealerMenuID, "Toilet Dealer 4", new List<MenuItem>() {
                    new MenuItem("Toilet Cleaner",24, 16) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 14, NumberOfItemsToSellToPlayer = 14 },}),20),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugDealerMenuID, "Toilet Dealer 53", new List<MenuItem>() {
                    new MenuItem("Toilet Cleaner",23, 18) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 10, NumberOfItemsToSellToPlayer = 12 },
                    new MenuItem("Shrewsbury Defender",1200) { IsIllicilt = true },}),10),
        });
        PossibleShopMenus.ShopMenuGroupList.Add(ToiletCleanerDealerMenuGroup);

        ShopMenuGroup SpankDealerMenuGroup = new ShopMenuGroup(StaticStrings.SPANKDealerMenuGroupID, "SPANK Dealer Menus", StaticStrings.DrugDealerMenuID, new List<PercentageSelectShopMenu>()
        {
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugDealerMenuID, "SPANK Dealer 1", new List<MenuItem>() {
                    new MenuItem("SPANK", 55, 25) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 10, NumberOfItemsToSellToPlayer = 14 } }),1),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugDealerMenuID, "SPANK Dealer 2", new List<MenuItem>() {
                    new MenuItem("SPANK", 52, 25) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 12, NumberOfItemsToSellToPlayer = 15 } }),1),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugDealerMenuID, "SPANK Dealer 3", new List<MenuItem>() {
                    new MenuItem("SPANK", 51, 20) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 13, NumberOfItemsToSellToPlayer = 16 },
                    new MenuItem("Toto 12 Guage Sawed-Off",430) { IsIllicilt = true },
                    new MenuItem("Combat Knife",120) { IsIllicilt = true },
                    new MenuItem("Hawk & Little Desert Slug",950) { IsIllicilt = true },}),1),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugDealerMenuID, "SPANK Dealer 4", new List<MenuItem>() {
                    new MenuItem("SPANK", 50, 25) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 11, NumberOfItemsToSellToPlayer = 9 },
                    new MenuItem("Vom Feuer KEK-9",565) { IsIllicilt = true },}),1),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugDealerMenuID, "SPANK Dealer 5", new List<MenuItem>() {
                    new MenuItem("SPANK", 48, 20) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 12, NumberOfItemsToSellToPlayer = 12 },
                    new MenuItem("Shrewsbury A7-4K",856) { IsIllicilt = true } }),1),
        });
        PossibleShopMenus.ShopMenuGroupList.Add(SpankDealerMenuGroup);


        ShopMenuGroup MethDealerMenuGroup = new ShopMenuGroup(StaticStrings.MethamphetamineDealerMenuGroupID, "Meth Dealer Menus", StaticStrings.DrugDealerMenuID, new List<PercentageSelectShopMenu>()
        {
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugDealerMenuID, "Meth Dealer 1", new List<MenuItem>() {
                    new MenuItem("Methamphetamine", 65, 40) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 8, NumberOfItemsToSellToPlayer = 14 },new MenuItem("Meth Pipe",5) { IsIllicilt = true, NumberOfItemsToSellToPlayer = 5 }, }),1),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugDealerMenuID, "Meth Dealer 2", new List<MenuItem>() {
                    new MenuItem("Methamphetamine", 55, 38) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 9, NumberOfItemsToSellToPlayer = 15 },new MenuItem("Meth Pipe",5) { IsIllicilt = true, NumberOfItemsToSellToPlayer = 5 }, }),1),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugDealerMenuID, "Meth Dealer 3", new List<MenuItem>() {
                    new MenuItem("Methamphetamine", 60, 36) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 10, NumberOfItemsToSellToPlayer = 13 },
                    new MenuItem("Meth Pipe",5) { IsIllicilt = true, NumberOfItemsToSellToPlayer = 5 },
                    new MenuItem("Hawk & Little PTF092F",200) { IsIllicilt = true },
                    new MenuItem("Switchblade",300) { IsIllicilt = true },}),1),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugDealerMenuID, "Meth Dealer 4", new List<MenuItem>() {
                    new MenuItem("Methamphetamine", 64, 35) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 14, NumberOfItemsToSellToPlayer = 19 },
                    new MenuItem("Meth Pipe",5) { IsIllicilt = true, NumberOfItemsToSellToPlayer = 5 },
                    new MenuItem("Vom Feuer KEK-9",565) { IsIllicilt = true },}),1),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugDealerMenuID, "Meth Dealer 5", new List<MenuItem>() {
                    new MenuItem("Methamphetamine", 62, 36) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 12, NumberOfItemsToSellToPlayer = 16 },
                    new MenuItem("Meth Pipe",5) { IsIllicilt = true, NumberOfItemsToSellToPlayer = 5 },
                    new MenuItem("Shrewsbury A7-4K",856) { IsIllicilt = true } }),1),
        });
        PossibleShopMenus.ShopMenuGroupList.Add(MethDealerMenuGroup);

        ShopMenuGroup HeroinDealerMenuGroup = new ShopMenuGroup(StaticStrings.HeroinDealerMenuGroupID, "Heroin Dealer Menus", StaticStrings.DrugDealerMenuID, new List<PercentageSelectShopMenu>()
        {
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugDealerMenuID, "Heroin Dealer 1", new List<MenuItem>() {
                    new MenuItem("Heroin", 150, 110) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 10, NumberOfItemsToSellToPlayer = 10 } }, StaticStrings.HeroinDealerMenuGroupID),1),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugDealerMenuID, "Heroin Dealer 2", new List<MenuItem>() {
                    new MenuItem("Heroin", 156, 108) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 9, NumberOfItemsToSellToPlayer = 11 } }, StaticStrings.HeroinDealerMenuGroupID),1),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugDealerMenuID, "Heroin Dealer 3", new List<MenuItem>() {
                    new MenuItem("Heroin", 160, 101) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 12, NumberOfItemsToSellToPlayer = 12 },
                    new MenuItem("Combat Knife",120) { IsIllicilt = true },
                    new MenuItem("Switchblade",300) { IsIllicilt = true },}, StaticStrings.HeroinDealerMenuGroupID),1),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugDealerMenuID, "Heroin Dealer 4", new List<MenuItem>() {
                    new MenuItem("Heroin", 158, 99) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 13, NumberOfItemsToSellToPlayer = 13 },
                    new MenuItem("Vom Feuer KEK-9",565) { IsIllicilt = true },}, StaticStrings.HeroinDealerMenuGroupID),1),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugDealerMenuID, "Heroin Dealer 5", new List<MenuItem>() {
                    new MenuItem("Heroin", 155, 105) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 12, NumberOfItemsToSellToPlayer = 11 }, }, StaticStrings.HeroinDealerMenuGroupID),1),
        });
        PossibleShopMenus.ShopMenuGroupList.Add(HeroinDealerMenuGroup);


        ShopMenuGroup CrackDealerMenuGroup = new ShopMenuGroup(StaticStrings.CrackDealerMenuGroupID, "Crack Dealer Menus", StaticStrings.DrugDealerMenuID, new List<PercentageSelectShopMenu>()
        {
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugDealerMenuID, "Crack Dealer 1", new List<MenuItem>() {
                    new MenuItem("Crack", 58, 40) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 9, NumberOfItemsToSellToPlayer = 10 },new MenuItem("Crack Pipe",5) { IsIllicilt = true, NumberOfItemsToSellToPlayer = 5 }, }, StaticStrings.CrackDealerMenuGroupID),1),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugDealerMenuID, "Crack Dealer 2", new List<MenuItem>() {
                    new MenuItem("Crack", 48, 38) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 11, NumberOfItemsToSellToPlayer = 12 },new MenuItem("Crack Pipe",5) { IsIllicilt = true, NumberOfItemsToSellToPlayer = 5 }, }, StaticStrings.CrackDealerMenuGroupID),1),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugDealerMenuID, "Crack Dealer 3", new List<MenuItem>() {
                    new MenuItem("Crack", 52, 36) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 12, NumberOfItemsToSellToPlayer = 8 },
                    new MenuItem("Crack Pipe",5) { IsIllicilt = true, NumberOfItemsToSellToPlayer = 5 },
                    new MenuItem("Toto 12 Guage Sawed-Off",430) { IsIllicilt = true },
                    new MenuItem("Combat Knife",120) { IsIllicilt = true },
                    new MenuItem("Hawk & Little Desert Slug",950) { IsIllicilt = true },}, StaticStrings.CrackDealerMenuGroupID),1),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugDealerMenuID, "Crack Dealer 4", new List<MenuItem>() {
                    new MenuItem("Crack", 53, 38) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 14, NumberOfItemsToSellToPlayer = 12 },
                    new MenuItem("Crack Pipe",5) { IsIllicilt = true, NumberOfItemsToSellToPlayer = 5 },
                    new MenuItem("Vom Feuer KEK-9",565) { IsIllicilt = true },}, StaticStrings.CrackDealerMenuGroupID),1),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugDealerMenuID, "Crack Dealer 5", new List<MenuItem>() {
                    new MenuItem("Crack", 50, 32) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 12, NumberOfItemsToSellToPlayer = 8 },
                    new MenuItem("Crack Pipe",5) { IsIllicilt = true, NumberOfItemsToSellToPlayer = 5 },
                    new MenuItem("Shrewsbury A7-4K",856) { IsIllicilt = true } }, StaticStrings.CrackDealerMenuGroupID),1),
        });
        PossibleShopMenus.ShopMenuGroupList.Add(CrackDealerMenuGroup);

        ShopMenuGroup CocaineDealerMenuGroup = new ShopMenuGroup(StaticStrings.CokeDealerMenuGroupID, "Cocaine Dealer Menus", StaticStrings.DrugDealerMenuID, new List<PercentageSelectShopMenu>()
        {
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugDealerMenuID, "Coke Dealer 1", new List<MenuItem>() {
                    new MenuItem("Cocaine", 180, 130) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 6, NumberOfItemsToSellToPlayer = 14 } }, StaticStrings.CokeDealerMenuGroupID),1),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugDealerMenuID, "Coke Dealer 2", new List<MenuItem>() {
                    new MenuItem("Cocaine", 175, 126) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 8, NumberOfItemsToSellToPlayer = 15 } }, StaticStrings.CokeDealerMenuGroupID),1),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugDealerMenuID, "Coke Dealer 3", new List<MenuItem>() {
                    new MenuItem("Cocaine", 170, 125) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 8, NumberOfItemsToSellToPlayer = 16 },
                    new MenuItem("Toto 12 Guage Sawed-Off",430) { IsIllicilt = true },
                    new MenuItem("Combat Knife",120) { IsIllicilt = true },
                    new MenuItem("Switchblade",300) { IsIllicilt = true },}, StaticStrings.CokeDealerMenuGroupID),1),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugDealerMenuID, "Coke Dealer 4", new List<MenuItem>() {
                    new MenuItem("Cocaine", 160, 120) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 10, NumberOfItemsToSellToPlayer = 12 },
                    new MenuItem("Vom Feuer KEK-9",565) { IsIllicilt = true },}, StaticStrings.CokeDealerMenuGroupID),1),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugDealerMenuID, "Coke Dealer 5", new List<MenuItem>() {
                    new MenuItem("Cocaine", 172, 128) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 9, NumberOfItemsToSellToPlayer = 11 },
                    new MenuItem("Hawk & Little PTF092F",250) { IsIllicilt = true } }, StaticStrings.CokeDealerMenuGroupID),1),
        });
        PossibleShopMenus.ShopMenuGroupList.Add(CocaineDealerMenuGroup);


        ShopMenuGroup MarijuanaCustomerMenuGroup = new ShopMenuGroup(StaticStrings.MarijuanaCustomerMenuGroupID, "Marijuana Customer Menus", StaticStrings.DrugCustomerMenuID, new List<PercentageSelectShopMenu>()
        {
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugCustomerMenuID, "Marijuana Customer 1", new List<MenuItem>() {
                    new MenuItem("Marijuana",0, 35) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 10 }}),1),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugCustomerMenuID, "Marijuana Customer 2", new List<MenuItem>() {
                    new MenuItem("Marijuana",0, 32) { IsIllicilt = true, NumberOfItemsToPurchaseFromPlayer = 9 }}),1),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugCustomerMenuID, "Marijuana Customer 3", new List<MenuItem>() {
                    new MenuItem("Marijuana",0, 30) { IsIllicilt = true, NumberOfItemsToPurchaseFromPlayer = 8 }}),1),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugCustomerMenuID, "Marijuana Customer 4", new List<MenuItem>() {
                    new MenuItem("Marijuana",0, 34) { IsIllicilt = true, NumberOfItemsToPurchaseFromPlayer = 11 }}),1),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugCustomerMenuID, "Marijuana Customer 5", new List<MenuItem>() {
                    new MenuItem("Marijuana",0, 33) { IsIllicilt = true, NumberOfItemsToPurchaseFromPlayer = 12 },}),1),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugCustomerMenuID, "Marijuana Customer 6", new List<MenuItem>() {
                    new MenuItem("Marijuana",0, 31) { IsIllicilt = true, NumberOfItemsToPurchaseFromPlayer = 10 },}),1),
        });
        PossibleShopMenus.ShopMenuGroupList.Add(MarijuanaCustomerMenuGroup);


        ShopMenuGroup ToiletCleanerCustomerMenuGroup = new ShopMenuGroup(StaticStrings.ToiletCleanerCustomerMenuGroupID, "Toilet Cleaner Customer Menus", StaticStrings.DrugCustomerMenuID, new List<PercentageSelectShopMenu>()
        {
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugCustomerMenuID, "Toilet Customer 1", new List<MenuItem>() {
                    new MenuItem("Toilet Cleaner",0, 45) { IsIllicilt = true, NumberOfItemsToPurchaseFromPlayer = 8 } }),1),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugCustomerMenuID, "Toilet Customer 2", new List<MenuItem>() {
                    new MenuItem("Toilet Cleaner",0, 42) { IsIllicilt = true, NumberOfItemsToPurchaseFromPlayer = 7 } }),1),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugCustomerMenuID, "Toilet Customer 3", new List<MenuItem>() {
                    new MenuItem("Toilet Cleaner",0, 38) { IsIllicilt = true, NumberOfItemsToPurchaseFromPlayer = 6 } }),1),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugCustomerMenuID, "Toilet Customer 4", new List<MenuItem>() {
                    new MenuItem("Toilet Cleaner",0, 39) { IsIllicilt = true, NumberOfItemsToPurchaseFromPlayer = 8 } }),1),
        });
        PossibleShopMenus.ShopMenuGroupList.Add(ToiletCleanerCustomerMenuGroup);


        ShopMenuGroup SPANKCustomerMenuGroup = new ShopMenuGroup(StaticStrings.SPANKCustomerMenuGroupID, "SPANK Customer Menus", StaticStrings.DrugCustomerMenuID, new List<PercentageSelectShopMenu>()
        {
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugCustomerMenuID, "SPANK Customer 1", new List<MenuItem>() {
                    new MenuItem("SPANK", 0, 62) { IsIllicilt = true, NumberOfItemsToPurchaseFromPlayer = 11 } }),1),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugCustomerMenuID, "SPANK Customer 2", new List<MenuItem>() {
                    new MenuItem("SPANK", 0, 67) { IsIllicilt = true, NumberOfItemsToPurchaseFromPlayer = 10 } }),1),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugCustomerMenuID, "SPANK Customer 3", new List<MenuItem>() {
                    new MenuItem("SPANK", 0, 65) { IsIllicilt = true, NumberOfItemsToPurchaseFromPlayer = 9 } }),1),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugCustomerMenuID, "SPANK Customer 4", new List<MenuItem>() {
                    new MenuItem("SPANK", 0, 70) { IsIllicilt = true, NumberOfItemsToPurchaseFromPlayer = 8 } }),1),
        });
        PossibleShopMenus.ShopMenuGroupList.Add(SPANKCustomerMenuGroup);


        ShopMenuGroup MethCustomerMenuGroup = new ShopMenuGroup(StaticStrings.MethamphetamineCustomerMenuGroupID, "Meth Customer Menus", StaticStrings.DrugCustomerMenuID, new List<PercentageSelectShopMenu>()
        {
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugCustomerMenuID, "Meth Customer 1", new List<MenuItem>() {
                    new MenuItem("Methamphetamine", 0, 85) { IsIllicilt = true, NumberOfItemsToPurchaseFromPlayer = 10 } }),1),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugCustomerMenuID, "Meth Customer 2", new List<MenuItem>() {
                    new MenuItem("Methamphetamine", 0, 80) { IsIllicilt = true, NumberOfItemsToPurchaseFromPlayer = 9 } }),1),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugCustomerMenuID, "Meth Customer 3", new List<MenuItem>() {
                    new MenuItem("Methamphetamine", 0, 75) { IsIllicilt = true, NumberOfItemsToPurchaseFromPlayer = 10 } }),1),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugCustomerMenuID, "Meth Customer 4", new List<MenuItem>() {
                    new MenuItem("Methamphetamine", 0, 77) { IsIllicilt = true, NumberOfItemsToPurchaseFromPlayer = 11} }),1),
        });
        PossibleShopMenus.ShopMenuGroupList.Add(MethCustomerMenuGroup);


        ShopMenuGroup CrackCustomerMenuGroup = new ShopMenuGroup(StaticStrings.CrackCustomerMenuGroupID, "Crack Customer Menus", StaticStrings.DrugCustomerMenuID, new List<PercentageSelectShopMenu>()
        {
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugCustomerMenuID, "Crack Customer 1", new List<MenuItem>() {
                    new MenuItem("Crack", 0, 70) { IsIllicilt = true, NumberOfItemsToPurchaseFromPlayer = 6 } }),1),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugCustomerMenuID, "Crack Customer 2", new List<MenuItem>() {
                    new MenuItem("Crack", 0, 68) { IsIllicilt = true, NumberOfItemsToPurchaseFromPlayer = 5 } }),1),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugCustomerMenuID, "Crack Customer 3", new List<MenuItem>() {
                    new MenuItem("Crack", 0, 65) { IsIllicilt = true, NumberOfItemsToPurchaseFromPlayer = 7 } }),1),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugCustomerMenuID, "Crack Customer 4", new List<MenuItem>() {
                    new MenuItem("Crack", 0, 66) { IsIllicilt = true, NumberOfItemsToPurchaseFromPlayer = 8 } }),1),
        });
        PossibleShopMenus.ShopMenuGroupList.Add(CrackCustomerMenuGroup);


        ShopMenuGroup CocaineCustomerMenuGroup = new ShopMenuGroup(StaticStrings.CokeCustomerMenuGroupID, "Cocaine Customer Menus", StaticStrings.DrugCustomerMenuID, new List<PercentageSelectShopMenu>()
        {
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugCustomerMenuID, "Coke Customer 1", new List<MenuItem>() {
                    new MenuItem("Cocaine", 0, 210) { IsIllicilt = true, NumberOfItemsToPurchaseFromPlayer = 6 } }),1),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugCustomerMenuID, "Coke Customer 2", new List<MenuItem>() {
                    new MenuItem("Cocaine", 0, 202) { IsIllicilt = true, NumberOfItemsToPurchaseFromPlayer = 7 } }),1),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugCustomerMenuID, "Coke Customer 3", new List<MenuItem>() {
                    new MenuItem("Cocaine", 0, 199) { IsIllicilt = true, NumberOfItemsToPurchaseFromPlayer = 5 } }),1),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugCustomerMenuID, "Coke Customer 4", new List<MenuItem>() {
                    new MenuItem("Cocaine", 0, 208) { IsIllicilt = true, NumberOfItemsToPurchaseFromPlayer = 8 } }),1),
        });
        PossibleShopMenus.ShopMenuGroupList.Add(CocaineCustomerMenuGroup);


        ShopMenuGroup HeroinCustomerMenuGroup = new ShopMenuGroup(StaticStrings.HeroinCustomerMenuGroupID, "Heroin Customer Menus", StaticStrings.DrugCustomerMenuID, new List<PercentageSelectShopMenu>()
        {
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugCustomerMenuID, "Heroin Customer 1", new List<MenuItem>() {
                    new MenuItem("Heroin", 0, 180) { IsIllicilt = true, NumberOfItemsToPurchaseFromPlayer = 6 } }),1),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugCustomerMenuID, "Heroin Customer 2", new List<MenuItem>() {
                    new MenuItem("Heroin", 0, 178) { IsIllicilt = true, NumberOfItemsToPurchaseFromPlayer = 7 } }),1),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugCustomerMenuID, "Heroin Customer 3", new List<MenuItem>() {
                    new MenuItem("Heroin", 0, 175) { IsIllicilt = true, NumberOfItemsToPurchaseFromPlayer = 8 } }),1),
            new PercentageSelectShopMenu(new ShopMenu(StaticStrings.DrugCustomerMenuID, "Heroin Customer 4", new List<MenuItem>() {
                    new MenuItem("Heroin", 0, 168) { IsIllicilt = true, NumberOfItemsToPurchaseFromPlayer = 6 } }),1),
        });
        PossibleShopMenus.ShopMenuGroupList.Add(HeroinCustomerMenuGroup);

    }
    private void DenList()
    {
        LostDenMenu();
        FamiliesDenMenu();
        VagosDenMenu();
        BallasDenMenu();
        VarriosDenMenu();
        MarabunteDenMenu();
        TriadsDenMenu();
        KkangpaeDenMenu();

        DiablosDenMenu();
        YardiesDenMenu();
        ArmenianDenMenu();
        MadrazoDenMenu();

        GambettiDenMenu(); 
        PavanoDenMenu();
        LupisellaDenMenu();
        MessinaDenMenu();
        AncelottiDenMenu();
        PossibleShopMenus.ShopMenuList.AddRange(new List<ShopMenu> { 

                new ShopMenu("GenericGangDenMenu","GenericGangDenMenu",new List<MenuItem>() {
                    new MenuItem("Marijuana",16,12),
                    new MenuItem("Smoke Shop Rolling Papers",2),
                    new MenuItem("Toilet Cleaner",22, 18) ,
                    new MenuItem("SPANK", 45, 20),
                    new MenuItem("Brass Knuckles",175),
                    new MenuItem("Combat Knife",150),
                    new MenuItem("Vom Feuer Machete",45),
                    new MenuItem("Switchblade",78),
                //Pistola
                new MenuItem("Hawk & Little PTF092F",500) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip",0),
                    new MenuItemExtra("Extended Clip", 75),
                    new MenuItemExtra("Suppressor", 500) },  },
                new MenuItem("Hawk & Little Combat Pistol",800) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 100),
                    new MenuItemExtra("Suppressor", 400), } },
                new MenuItem("Hawk & Little Desert Slug",1400) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120),
                    new MenuItemExtra("Suppressor", 700), } },
                new MenuItem("Hawk & Little 1919",1300) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 50),
                    new MenuItemExtra("Suppressor", 200), } },
                new MenuItem("Hawk & Little Raging Mare Dx",1600),
                new MenuItem("Vom Feuer P69",900) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 100),
                    new MenuItemExtra("Suppressor", 250), } },
                new MenuItem("Vom Feuer SCRAMP",850) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 85),
                    new MenuItemExtra("Suppressor", 300), } },
                new MenuItem("Shrewsbury S7A",1300) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 150),
                    new MenuItemExtra("Suppressor", 250), } },
                //Shotgun
                new MenuItem("Shrewsbury 420 Sawed-Off",350),
                new MenuItem("Shrewsbury 420",375),
                new MenuItem("Toto 12 Guage Sawed-Off",395),
                new MenuItem("Shrewsbury Defender",750),
                //SMG
                new MenuItem("Shrewsbury Luzi",550) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 150),
                    new MenuItemExtra("Suppressor", 275), } },
                new MenuItem("Vom Feuer KEK-9",300) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 150),
                    new MenuItemExtra("Drum Magazine", 180),
                    new MenuItemExtra("Suppressor", 450) } },
                new MenuItem("Hawk & Little Millipede",550) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 220), } },
                //AR
                new MenuItem("Shrewsbury A7-4K",650) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 200),
                    new MenuItemExtra("Drum Magazine", 250),
                    new MenuItemExtra("Suppressor", 600), } },
                new MenuItem("Shrewsbury A2-1K",850) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 200),
                    new MenuItemExtra("Suppressor", 250) } },
                new MenuItem("Vom Feuer A5-1R",950) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 125),
                    new MenuItemExtra("Box Magazine", 250),
                    new MenuItemExtra("Suppressor", 390),} },
                new MenuItem("Vom Feuer A5-1R MK2",1250) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 145),
                    new MenuItemExtra("Suppressor", 350),} },
                new MenuItem("Shrewsbury Stinkov",650) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 125),
                    new MenuItemExtra("Drum Magazine", 250),} },
                //LMG
                new MenuItem("Shrewsbury PDA",1500) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 200),} },
                new MenuItem("Vom Feuer BAT",1700) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 150),} },
                new MenuItem("Vom Feuer M70E1",2200) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 250),} },
                //SNIPER
                new MenuItem("Shrewsbury PWN",2200) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Suppressor", 340)} },
                //OTHER
                new MenuItem("M61 Grenade",500) { SubAmount = 1,SubPrice = 500 },
                new MenuItem("Improvised Incendiary",60) { SubAmount = 1,SubPrice = 60 },
                new MenuItem("BZ Gas Grenade",250) { SubAmount = 1,SubPrice = 250 },
                    }),
        });
    }
    private void LostDenMenu()
    {
        PossibleShopMenus.ShopMenuList.AddRange(new List<ShopMenu> {
               new ShopMenu("LostDenMenu","LostDenMenu",new List<MenuItem>() {


                   new MenuItem("Methamphetamine",50, 34),
                   new MenuItem("Methamphetamine",40) { MinimumPurchaseAmount = 500, MaximumPurchaseAmount = 1000, PurchaseIncrement = 100 },
                   new MenuItem("Methamphetamine",35) { MinimumPurchaseAmount = 1000, MaximumPurchaseAmount = 5000, PurchaseIncrement = 500 },
                   new MenuItem("Meth Pipe",5),

                    new MenuItem("G.E.S. Baseball Bat",45),
                    new MenuItem("Flint Crowbar",30),
                    new MenuItem("ProLaps Five Iron Golf Club",100),
                    new MenuItem("Flint Hammer",20),
                    new MenuItem("Flint Hatchet",75),
                    new MenuItem("Brass Knuckles",100),
                    new MenuItem("Combat Knife",100),
                    new MenuItem("Vom Feuer Machete",20),
                    new MenuItem("Switchblade",45),
                    new MenuItem("Nightstick",45),
                    new MenuItem("Flint Heavy Duty Pipe Wrench",20),
                    new MenuItem("Pool Cue",30),

                //Food
                new MenuItem("Bottle of Barracho", 0) { IsFree = true },
                new MenuItem("Bottle of PiBwasser", 0) { IsFree = true },
                new MenuItem("Bottle of Blarneys", 0) { IsFree = true },
                new MenuItem("Bottle of Logger", 0) { IsFree = true },
                new MenuItem("Bottle of Patriot", 0) { IsFree = true },
                new MenuItem("Bottle of Pride", 0) { IsFree = true },
                new MenuItem("Bottle of Stronzo", 0) { IsFree = true },
                new MenuItem("Bottle of A.M.", 0) { IsFree = true },
                new MenuItem("Bottle of Jakeys", 0) { IsFree = true },
                new MenuItem("Bottle of Dusche", 0) { IsFree = true },

                //Pistola
                new MenuItem("Hawk & Little PTF092F",400) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip",0),
                    new MenuItemExtra("Extended Clip", 75),
                    new MenuItemExtra("Flashlight", 70),
                    new MenuItemExtra("Suppressor", 850) },  },
                new MenuItem("Hawk & Little Thunder",450) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 105),
                    new MenuItemExtra("Tracer Rounds", 500),
                    new MenuItemExtra("Incendiary Rounds", 500),
                    new MenuItemExtra("Hollow Point Rounds", 500),
                    new MenuItemExtra("Full Metal Jacket Rounds",500),
                    new MenuItemExtra("Mounted Scope", 1200),
                    new MenuItemExtra("Flashlight", 75),
                    new MenuItemExtra("Suppressor", 699),
                    new MenuItemExtra("Compensator", 240), } },
                new MenuItem("Hawk & Little Combat Pistol",750) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 85),
                    new MenuItemExtra("Flashlight", 80),
                    new MenuItemExtra("Suppressor", 890), } },
                new MenuItem("Hawk & Little Desert Slug",1100) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120),
                    new MenuItemExtra("Flashlight", 95),
                    new MenuItemExtra("Suppressor", 1100), } },
                new MenuItem("Hawk & Little 1919",900) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 50),
                    new MenuItemExtra("Flashlight", 75),
                    new MenuItemExtra("Suppressor", 950), } },
                new MenuItem("Hawk & Little Raging Mare Dx",1500) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Rounds", 0),
                    new MenuItemExtra("Tracer Rounds", 500),
                    new MenuItemExtra("Incendiary Rounds", 500),
                    new MenuItemExtra("Hollow Point Rounds", 500),
                    new MenuItemExtra("Full Metal Jacket Rounds", 500),
                    new MenuItemExtra("Holographic Sight", 890),
                    new MenuItemExtra("Small Scope", 570),
                    new MenuItemExtra("Flashlight", 70), } },
                new MenuItem("Vom Feuer SCRAMP",700) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 85),
                    new MenuItemExtra("Flashlight", 80),
                    new MenuItemExtra("Suppressor", 890), } },
                new MenuItem("BS M1922",450) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 85),
                    new MenuItemExtra("Suppressor", 890), } },


                //Shotgun
                new MenuItem("Shrewsbury 420 Sawed-Off",200),
                new MenuItem("Shrewsbury 420",250) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Flashlight", 95),
                    new MenuItemExtra("Suppressor", 1100), } },
                new MenuItem("Vom Feuer 569",350) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Shells", 0),
                    new MenuItemExtra("Dragon's Breath Shells", 500),
                    new MenuItemExtra("Steel Buckshot Shells", 500),
                    new MenuItemExtra("Flechette Shells", 500),
                    new MenuItemExtra("Explosive Slugs", 500),
                    new MenuItemExtra("Holographic Sight", 670),
                    new MenuItemExtra("Small Scope", 560),
                    new MenuItemExtra("Medium Scope", 890),
                    new MenuItemExtra("Flashlight", 85),
                    new MenuItemExtra("Suppressor", 1890),
                    new MenuItemExtra("Squared Muzzle Brake", 200), } },
                new MenuItem("Vom Feuer IBS-12",540) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 85),
                    new MenuItemExtra("Flashlight", 95),
                    new MenuItemExtra("Suppressor", 1100), } },
                new MenuItem("Hawk & Little HLSG",670) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Flashlight", 95),
                    new MenuItemExtra("Suppressor", 1950),
                    new MenuItemExtra("Grip", 120),} },
                new MenuItem("Shrewsbury Taiga-12",560) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120),
                    new MenuItemExtra("Drum Magazine", 550),
                    new MenuItemExtra("Flashlight", 80),
                    new MenuItemExtra("Suppressor", 1945),
                    new MenuItemExtra("Grip", 224) } },
                new MenuItem("Toto 12 Guage Sawed-Off",300),
                new MenuItem("Shrewsbury Defender",700),
                new MenuItem("Leotardo SPAZ-11",1000) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Flashlight", 95),
                    new MenuItemExtra("Suppressor", 1100), } },
     
                //SMG
                new MenuItem("Shrewsbury Luzi",300) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120),
                    new MenuItemExtra("Flashlight", 80),
                    new MenuItemExtra("Suppressor", 1945),
                    new MenuItemExtra("Scope", 556) } },
                new MenuItem("Hawk & Little MP6",1000) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120),
                    new MenuItemExtra("Drum Magazine", 550),
                    new MenuItemExtra("Flashlight", 80),
                    new MenuItemExtra("Suppressor", 1945) } },
                new MenuItem("Hawk & Little XPM",1200) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120),
                    new MenuItemExtra("Tracer Rounds", 500),
                    new MenuItemExtra("Incendiary Rounds", 500),
                    new MenuItemExtra("Hollow Point Rounds", 500),
                    new MenuItemExtra("Full Metal Jacket Rounds", 500),
                    new MenuItemExtra("Flashlight", 120),
                    new MenuItemExtra("Holographic Sight", 760),
                    new MenuItemExtra("Small Scope", 525),
                    new MenuItemExtra("Medium Scope", 890),
                    new MenuItemExtra("Suppressor", 1400),
                    new MenuItemExtra("Flat Muzzle Brake", 255),
                    new MenuItemExtra("Tactical Muzzle Brake",265),
                    new MenuItemExtra("Fat-End Muzzle Brake", 200),
                    new MenuItemExtra("Precision Muzzle Brake", 276),
                    new MenuItemExtra("Heavy Duty Muzzle Brake", 345),
                    new MenuItemExtra("Slanted Muzzle Brake", 205),
                    new MenuItemExtra("Split-End Muzzle Brake", 200),
                    new MenuItemExtra("Default Barrel", 0),
                    new MenuItemExtra("Heavy Barrel", 500), } },
                new MenuItem("Vom Feuer KEK-9",200) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120),
                    new MenuItemExtra("Drum Magazine", 550),
                    new MenuItemExtra("Suppressor", 1945) } },
                new MenuItem("Hawk & Little Millipede",250) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120), } },

                //AR
                new MenuItem("Shrewsbury A7-4K",450) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 125),
                    new MenuItemExtra("Drum Magazine", 225),
                    new MenuItemExtra("Flashlight", 90),
                    new MenuItemExtra("Scope", 567),
                    new MenuItemExtra("Suppressor", 800),
                    new MenuItemExtra("Grip", 200), } },
                new MenuItem("Shrewsbury A2-1K",570) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 150),
                    new MenuItemExtra("Tracer Rounds", 500),
                    new MenuItemExtra("Incendiary Rounds", 500),
                    new MenuItemExtra("Armor Piercing Rounds", 500),
                    new MenuItemExtra("Full Metal Jacket Rounds", 500),
                    new MenuItemExtra("Grip", 120),
                    new MenuItemExtra("Flashlight", 90),
                    new MenuItemExtra("Holographic Sight", 780),
                    new MenuItemExtra("Small Scope", 667),
                    new MenuItemExtra("Large Scope", 989),
                    new MenuItemExtra("Suppressor", 1200),
                    new MenuItemExtra("Flat Muzzle Brake", 300),
                    new MenuItemExtra("Tactical Muzzle Brake", 123),
                    new MenuItemExtra("Fat-End Muzzle Brake", 150),
                    new MenuItemExtra("Precision Muzzle Brake", 224),
                    new MenuItemExtra("Heavy Duty Muzzle Brake", 330),
                    new MenuItemExtra("Slanted Muzzle Brake", 150),
                    new MenuItemExtra("Split-End Muzzle Brake", 175),
                    new MenuItemExtra("Default Barrel", 0),
                    new MenuItemExtra("Heavy Barrel", 800), } },
                new MenuItem("Vom Feuer A5-1R",550) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 125),
                    new MenuItemExtra("Box Magazine", 225),
                    new MenuItemExtra("Flashlight", 90),
                    new MenuItemExtra("Scope", 567),
                    new MenuItemExtra("Suppressor", 800),
                    new MenuItemExtra("Grip", 200), } },
                new MenuItem("Vom Feuer A5-1R MK2",780) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 145),
                    new MenuItemExtra("Tracer Rounds", 500),
                    new MenuItemExtra("Incendiary Rounds", 500),
                    new MenuItemExtra("Armor Piercing Rounds", 500),
                    new MenuItemExtra("Full Metal Jacket Rounds", 500),
                    new MenuItemExtra("Grip", 120),
                    new MenuItemExtra("Flashlight", 90),
                    new MenuItemExtra("Holographic Sight", 870),
                    new MenuItemExtra("Small Scope", 760),
                    new MenuItemExtra("Large Scope", 900),
                    new MenuItemExtra("Suppressor", 1200),
                    new MenuItemExtra("Flat Muzzle Brake", 250),
                    new MenuItemExtra("Tactical Muzzle Brake", 220),
                    new MenuItemExtra("Fat-End Muzzle Brake", 215),
                    new MenuItemExtra("Precision Muzzle Brake", 220),
                    new MenuItemExtra("Heavy Duty Muzzle Brake", 235),
                    new MenuItemExtra("Slanted Muzzle Brake", 230),
                    new MenuItemExtra("Split-End Muzzle Brake", 225),
                    new MenuItemExtra("Default Barrel", 0),
                    new MenuItemExtra("Heavy Barrel", 670), } },
                new MenuItem("Vom Feuer BFR",950) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 125),
                    new MenuItemExtra("Flashlight", 90),
                    new MenuItemExtra("Scope", 567),
                    new MenuItemExtra("Suppressor", 800), } },
                new MenuItem("Vom Feuer SL6",1200) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 125),
                    new MenuItemExtra("Drum Magazine", 225),
                    new MenuItemExtra("Flashlight", 90),
                    new MenuItemExtra("Scope", 567),
                    new MenuItemExtra("Suppressor", 800),
                    new MenuItemExtra("Grip", 200), } },
                new MenuItem("Vom Feuer SL6 MK2",1400) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 145),
                    new MenuItemExtra("Tracer Rounds", 500),
                    new MenuItemExtra("Incendiary Rounds", 500),
                    new MenuItemExtra("Armor Piercing Rounds", 500),
                    new MenuItemExtra("Full Metal Jacket Rounds", 500),
                    new MenuItemExtra("Grip", 120),
                    new MenuItemExtra("Flashlight", 90),
                    new MenuItemExtra("Holographic Sight", 870),
                    new MenuItemExtra("Small Scope", 760),
                    new MenuItemExtra("Large Scope", 900),
                    new MenuItemExtra("Suppressor", 1200),
                    new MenuItemExtra("Flat Muzzle Brake", 250),
                    new MenuItemExtra("Tactical Muzzle Brake", 220),
                    new MenuItemExtra("Fat-End Muzzle Brake", 215),
                    new MenuItemExtra("Precision Muzzle Brake", 220),
                    new MenuItemExtra("Heavy Duty Muzzle Brake", 235),
                    new MenuItemExtra("Slanted Muzzle Brake", 230),
                    new MenuItemExtra("Split-End Muzzle Brake", 225),
                    new MenuItemExtra("Default Barrel", 0),
                    new MenuItemExtra("Heavy Barrel", 670), } },
                new MenuItem("Shrewsbury Stinkov",400) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 125),
                    new MenuItemExtra("Drum Magazine", 225),} },
                new MenuItem("Vom Feuer POCK",1450),

                //LMG
                new MenuItem("Shrewsbury PDA",2000) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 125),
                    new MenuItemExtra("Scope", 567),} },
                new MenuItem("Vom Feuer M70E1",3000) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 145),
                    new MenuItemExtra("Tracer Rounds", 500),
                    new MenuItemExtra("Incendiary Rounds", 500),
                    new MenuItemExtra("Armor Piercing Rounds", 500),
                    new MenuItemExtra("Full Metal Jacket Rounds", 500),
                    new MenuItemExtra("Grip", 124),
                    new MenuItemExtra("Holographic Sight", 556),
                    new MenuItemExtra("Medium Scope", 760),
                    new MenuItemExtra("Large Scope", 890),
                    new MenuItemExtra("Flat Muzzle Brake", 120),
                    new MenuItemExtra("Tactical Muzzle Brake", 125),
                    new MenuItemExtra("Fat-End Muzzle Brake", 130),
                    new MenuItemExtra("Precision Muzzle Brake", 135),
                    new MenuItemExtra("Heavy Duty Muzzle Brake", 145),
                    new MenuItemExtra("Slanted Muzzle Brake", 155),
                    new MenuItemExtra("Split-End Muzzle Brake", 155),
                    new MenuItemExtra("Default Barrel", 0),
                    new MenuItemExtra("Heavy Barrel", 900),} },
                new MenuItem("Hawk & Little Kenan",700) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 125),} },

                //SNIPER
                new MenuItem("Shrewsbury PWN",1500) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Scope", 890),
                    new MenuItemExtra("Advanced Scope", 1400),
                    new MenuItemExtra("Suppressor", 1900),} },
                new MenuItem("Bartlett M92",2500) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Scope", 890),
                    new MenuItemExtra("Advanced Scope", 1400),} },
                new MenuItem("Bartlett M92 Mk2",4500) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 250),
                    new MenuItemExtra("Incendiary Rounds", 1000),
                    new MenuItemExtra("Armor Piercing Rounds", 1500),
                    new MenuItemExtra("Full Metal Jacket Rounds", 500),
                    new MenuItemExtra("Explosive Rounds", 2000),
                    new MenuItemExtra("Zoom Scope", 2500),
                    new MenuItemExtra("Advanced Scope", 1500),
                    new MenuItemExtra("Night Vision Scope", 3500),
                    new MenuItemExtra("Thermal Scope", 9500),
                    new MenuItemExtra("Suppressor", 1900),
                    new MenuItemExtra("Squared Muzzle Brake", 125),
                    new MenuItemExtra("Bell-End Muzzle Brake", 150),
                    new MenuItemExtra("Default Barrel", 0),
                    new MenuItemExtra("Heavy Barrel", 1800),} },
                new MenuItem("Vom Feuer M23 DBS Scout",1230) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 550),
                    new MenuItemExtra("Tracer Rounds",560),
                    new MenuItemExtra("Incendiary Rounds", 1000),
                    new MenuItemExtra("Armor Piercing Rounds", 1700),
                    new MenuItemExtra("Full Metal Jacket Rounds", 550),
                    new MenuItemExtra("Holographic Sight", 670),
                    new MenuItemExtra("Large Scope", 890),
                    new MenuItemExtra("Zoom Scope", 1200),
                    new MenuItemExtra("Flashlight", 78),
                    new MenuItemExtra("Suppressor", 1400),
                    new MenuItemExtra("Flat Muzzle Brake", 123),
                    new MenuItemExtra("Tactical Muzzle Brake", 125),
                    new MenuItemExtra("Fat-End Muzzle Brake", 150),
                    new MenuItemExtra("Precision Muzzle Brake", 125),
                    new MenuItemExtra("Heavy Duty Muzzle Brake", 134),
                    new MenuItemExtra("Slanted Muzzle Brake", 145),
                    new MenuItemExtra("Split-End Muzzle Brake", 134),
                    new MenuItemExtra("Default Barrel", 0),
                    new MenuItemExtra("Heavy Barrel", 1200),
                    new MenuItemExtra("Grip", 130),} },

                //OTHER
                new MenuItem("RPG-7",1000){ SubAmount = 1,SubPrice = 100 },
                new MenuItem("Hawk & Little MGL",1200){ Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Flashlight", 90),
                    new MenuItemExtra("Scope", 567),
                    new MenuItemExtra("Grip", 200), },SubAmount = 6,SubPrice = 100 },
                new MenuItem("M61 Grenade",400) { SubAmount = 1,SubPrice = 400 },
                new MenuItem("Improvised Incendiary",120) { SubAmount = 1,SubPrice = 120 },
                new MenuItem("BZ Gas Grenade",200) { SubAmount = 1,SubPrice = 200 },


                new MenuItem("Western Zombie Bobber",7000,5000),
                new MenuItem("Western Zombie Chopper",6200,4200),
                new MenuItem("Western Daemon",5500,3975),
                new MenuItem("Western Daemon LOST",6000,4000),
                new MenuItem("Western Bagger",3000,2000),
                new MenuItem("Vapid Lost Slamvan",12000,8000),
                new MenuItem("Declasse Gang Burrito",9000,7000),
               // new MenuItem("Tear Gas Grenade",125) { AmmoAmount = 1,AmmoPrice = 125 },
                    }),
        });;

    }
    private void FamiliesDenMenu()
    {
        PossibleShopMenus.ShopMenuList.AddRange(new List<ShopMenu> {
               new ShopMenu("FamiliesDenMenu","FamiliesDenMenu",new List<MenuItem>() {
                    new MenuItem("Marijuana", 15, 9),
                    new MenuItem("Marijuana",12) { MinimumPurchaseAmount = 500, MaximumPurchaseAmount = 500, PurchaseIncrement = 100 },
                    new MenuItem("Marijuana",10) { MinimumPurchaseAmount = 1000, MaximumPurchaseAmount = 1000, PurchaseIncrement = 100 },
                    new MenuItem("Smoke Shop Rolling Papers",2),
                    new MenuItem("Toilet Cleaner",20, 16) ,
                    new MenuItem("SPANK", 40, 25),
                    new MenuItem("Flint Hatchet",80),
                    new MenuItem("Brass Knuckles",150),
                    new MenuItem("Combat Knife",120),
                    new MenuItem("Vom Feuer Machete",29),
                    new MenuItem("Switchblade",55),
                //Pistola
                new MenuItem("Hawk & Little PTF092F",445) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip",0),
                    new MenuItemExtra("Extended Clip", 75),
                    new MenuItemExtra("Suppressor", 343) },  },
                new MenuItem("Hawk & Little Combat Pistol",780) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 85),
                    new MenuItemExtra("Suppressor", 345), } },
                new MenuItem("Hawk & Little Desert Slug",1245) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120),
                    new MenuItemExtra("Suppressor", 556), } },
                new MenuItem("Hawk & Little 1919",1134) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 50),
                    new MenuItemExtra("Suppressor", 125), } },
                new MenuItem("Hawk & Little Raging Mare Dx",1450),
                new MenuItem("Vom Feuer P69",790) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 85),
                    new MenuItemExtra("Suppressor", 125), } },
                new MenuItem("Vom Feuer SCRAMP",780) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 85),
                    new MenuItemExtra("Suppressor", 200), } },
                new MenuItem("Shrewsbury S7A",1140) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 68),
                    new MenuItemExtra("Mounted Scope", 890),
                    new MenuItemExtra("Suppressor", 145),
                    new MenuItemExtra("Compensator", 240), } },
                //Shotgun
                new MenuItem("Shrewsbury 420 Sawed-Off",200),
               // new MenuItem("Shrewsbury 420",200),
                new MenuItem("Toto 12 Guage Sawed-Off",250),
                //new MenuItem("Shrewsbury Defender",550),
                //SMG
                new MenuItem("Shrewsbury Luzi",455) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120),
                    new MenuItemExtra("Suppressor", 145), } },
                new MenuItem("Vom Feuer KEK-9",250) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120),
                    new MenuItemExtra("Drum Magazine", 123),
                    new MenuItemExtra("Suppressor", 356) } },
                new MenuItem("Hawk & Little Millipede",450) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120), } },
                //AR
                new MenuItem("Shrewsbury A7-4K",550) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 125),
                    new MenuItemExtra("Drum Magazine", 200),
                    new MenuItemExtra("Suppressor", 456), } },
                new MenuItem("Shrewsbury A2-1K",656) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 150),
                    new MenuItemExtra("Tracer Rounds", 500),
                    new MenuItemExtra("Incendiary Rounds", 500),
                    new MenuItemExtra("Armor Piercing Rounds", 500),
                    new MenuItemExtra("Full Metal Jacket Rounds", 500),
                    new MenuItemExtra("Suppressor", 245) } },
                //new MenuItem("Vom Feuer A5-1R",650) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 125),
                //    new MenuItemExtra("Box Magazine", 200),
                //    new MenuItemExtra("Suppressor", 225),} },
                //new MenuItem("Vom Feuer A5-1R MK2",950) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 145),
                //    new MenuItemExtra("Holographic Sight", 780),
                //    new MenuItemExtra("Suppressor", 225),} },
                new MenuItem("Shrewsbury Stinkov",450) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 125),
                    new MenuItemExtra("Drum Magazine", 200),} },
                ////LMG
                //new MenuItem("Shrewsbury PDA",1200) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 125),} },
                //new MenuItem("Vom Feuer BAT",1340) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 125),
                //    new MenuItemExtra("Grip", 90),
                //    new MenuItemExtra("Scope", 567),} },
                //new MenuItem("Vom Feuer M70E1",1680) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 145),
                //    new MenuItemExtra("Tracer Rounds", 500),
                //    new MenuItemExtra("Incendiary Rounds", 500),
                //    new MenuItemExtra("Armor Piercing Rounds", 500),
                //    new MenuItemExtra("Full Metal Jacket Rounds", 500),} },
                ////SNIPER
                //new MenuItem("Shrewsbury PWN",2200) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Suppressor", 340)} },
                //new MenuItem("Bartlett M92",4578),
                //new MenuItem("Bartlett M92 Mk2",3456),
                ////OTHER
                //new MenuItem("RPG-7",1800){ SubAmount = 1,SubPrice = 500 },
                //new MenuItem("Hawk & Little MGL",2500){ SubAmount = 6,SubPrice = 200 },
                new MenuItem("M61 Grenade",340) { SubAmount = 1,SubPrice = 340 },
                new MenuItem("Improvised Incendiary",30) { SubAmount = 1,SubPrice = 30 },
                new MenuItem("BZ Gas Grenade",100) { SubAmount = 1,SubPrice = 100 },
                    }),
        });
    }
    private void VagosDenMenu()
    {
        PossibleShopMenus.ShopMenuList.AddRange(new List<ShopMenu> {
               new ShopMenu("VagosDenMenu","VagosDenMenu",new List<MenuItem>() {
                    new MenuItem("Marijuana",15, 9),
                    new MenuItem("Marijuana",12) { MinimumPurchaseAmount = 500, MaximumPurchaseAmount = 500, PurchaseIncrement = 100 },
                    new MenuItem("Marijuana",10) { MinimumPurchaseAmount = 1000, MaximumPurchaseAmount = 1000, PurchaseIncrement = 100 },
                    new MenuItem("Smoke Shop Rolling Papers",2),
                //Pistola
                new MenuItem("Hawk & Little PTF092F",445) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip",0),
                    new MenuItemExtra("Extended Clip", 75),
                    new MenuItemExtra("Suppressor", 343) },  },
                new MenuItem("Hawk & Little Combat Pistol",780) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 85),
                    new MenuItemExtra("Suppressor", 345), } },
                new MenuItem("Vom Feuer P69",790) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 85),
                    new MenuItemExtra("Suppressor", 125), } },
                new MenuItem("Vom Feuer SCRAMP",780) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 85),
                    new MenuItemExtra("Suppressor", 200), } },
                //Shotgun
                new MenuItem("Shrewsbury 420 Sawed-Off",200),
                new MenuItem("Shrewsbury 420",200),
                new MenuItem("Toto 12 Guage Sawed-Off",250),
                //SMG
                new MenuItem("Shrewsbury Luzi",455) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120),
                    new MenuItemExtra("Suppressor", 145), } },
                new MenuItem("Vom Feuer KEK-9",250) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120),
                    new MenuItemExtra("Drum Magazine", 123),
                    new MenuItemExtra("Suppressor", 356) } },
                //AR
                new MenuItem("Shrewsbury A7-4K",550) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 125),
                    new MenuItemExtra("Drum Magazine", 200),
                    new MenuItemExtra("Suppressor", 456), } },
                new MenuItem("Shrewsbury A2-1K",656) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 150),
                    new MenuItemExtra("Tracer Rounds", 500),
                    new MenuItemExtra("Incendiary Rounds", 500),
                    new MenuItemExtra("Armor Piercing Rounds", 500),
                    new MenuItemExtra("Full Metal Jacket Rounds", 500),
                    new MenuItemExtra("Suppressor", 245) } },
                new MenuItem("Shrewsbury Stinkov",450) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 125),
                    new MenuItemExtra("Drum Magazine", 200),} },
                //OTHER
                new MenuItem("M61 Grenade",340) { SubAmount = 1,SubPrice = 340 },
                new MenuItem("Improvised Incendiary",30) { SubAmount = 1,SubPrice = 30 },
                new MenuItem("BZ Gas Grenade",100) { SubAmount = 1,SubPrice = 100 },
                    }),
        });
    }
    private void BallasDenMenu()
    {
        PossibleShopMenus.ShopMenuList.AddRange(new List<ShopMenu> {
               new ShopMenu("BallasDenMenu","BallasDenMenu",new List<MenuItem>() {
                    new MenuItem("Crack",46, 39),
                    new MenuItem("Crack",42) { MinimumPurchaseAmount = 500, MaximumPurchaseAmount = 500, PurchaseIncrement = 100 },
                    new MenuItem("Crack",40) { MinimumPurchaseAmount = 1000, MaximumPurchaseAmount = 1000, PurchaseIncrement = 100 },
                    new MenuItem("Crack Pipe",5),
                //Pistola
                new MenuItem("Hawk & Little PTF092F",445) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip",0),
                    new MenuItemExtra("Extended Clip", 75),
                    new MenuItemExtra("Suppressor", 343) },  },
                new MenuItem("Hawk & Little Combat Pistol",780) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 85),
                    new MenuItemExtra("Suppressor", 345), } },
                new MenuItem("Hawk & Little Desert Slug",1245) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120),
                    new MenuItemExtra("Suppressor", 556), } },
                new MenuItem("Vom Feuer SCRAMP",780) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 85),
                    new MenuItemExtra("Suppressor", 200), } },
                new MenuItem("Shrewsbury S7A",1140) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 68),
                    new MenuItemExtra("Mounted Scope", 890),
                    new MenuItemExtra("Suppressor", 145),
                    new MenuItemExtra("Compensator", 240), } },
                //Shotgun
                new MenuItem("Toto 12 Guage Sawed-Off",250),
                new MenuItem("Shrewsbury Defender",550),
                //SMG
                new MenuItem("Shrewsbury Luzi",455) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120),
                    new MenuItemExtra("Suppressor", 145), } },
                //AR
                new MenuItem("Shrewsbury A7-4K",550) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 125),
                    new MenuItemExtra("Drum Magazine", 200),
                    new MenuItemExtra("Suppressor", 456), } },
                new MenuItem("Shrewsbury A2-1K",656) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 150),
                    new MenuItemExtra("Tracer Rounds", 500),
                    new MenuItemExtra("Incendiary Rounds", 500),
                    new MenuItemExtra("Armor Piercing Rounds", 500),
                    new MenuItemExtra("Full Metal Jacket Rounds", 500),
                    new MenuItemExtra("Suppressor", 245) } },
                new MenuItem("M61 Grenade",340) { SubAmount = 1,SubPrice = 340 },
                new MenuItem("Improvised Incendiary",30) { SubAmount = 1,SubPrice = 30 },
                new MenuItem("BZ Gas Grenade",100) { SubAmount = 1,SubPrice = 100 },
                    }),
        });
    }
    private void VarriosDenMenu()
    {
        PossibleShopMenus.ShopMenuList.AddRange(new List<ShopMenu> {
               new ShopMenu("VarriosDenMenu","VarriosDenMenu",new List<MenuItem>() {
                    new MenuItem("Crack",45, 39),
                    new MenuItem("Crack",42) { MinimumPurchaseAmount = 500, MaximumPurchaseAmount = 500, PurchaseIncrement = 100 },
                    new MenuItem("Crack",40) { MinimumPurchaseAmount = 1000, MaximumPurchaseAmount = 1000, PurchaseIncrement = 100 },
                    new MenuItem("Crack Pipe",5),
                //Pistola
                new MenuItem("Hawk & Little PTF092F",445) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip",0),
                    new MenuItemExtra("Extended Clip", 75),
                    new MenuItemExtra("Suppressor", 343) },  },
                new MenuItem("Hawk & Little Combat Pistol",780) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 85),
                    new MenuItemExtra("Suppressor", 345), } },
                new MenuItem("Hawk & Little Desert Slug",1245) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120),
                    new MenuItemExtra("Suppressor", 556), } },
                new MenuItem("Vom Feuer SCRAMP",780) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 85),
                    new MenuItemExtra("Suppressor", 200), } },
                new MenuItem("Shrewsbury S7A",1140) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 68),
                    new MenuItemExtra("Mounted Scope", 890),
                    new MenuItemExtra("Suppressor", 145),
                    new MenuItemExtra("Compensator", 240), } },
                //Shotgun
                new MenuItem("Toto 12 Guage Sawed-Off",250),
                new MenuItem("Shrewsbury Defender",550),
                //SMG
                new MenuItem("Shrewsbury Luzi",455) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120),
                    new MenuItemExtra("Suppressor", 145), } },
                //AR
                new MenuItem("Shrewsbury A7-4K",550) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 125),
                    new MenuItemExtra("Drum Magazine", 200),
                    new MenuItemExtra("Suppressor", 456), } },
                new MenuItem("Shrewsbury A2-1K",656) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 150),
                    new MenuItemExtra("Tracer Rounds", 500),
                    new MenuItemExtra("Incendiary Rounds", 500),
                    new MenuItemExtra("Armor Piercing Rounds", 500),
                    new MenuItemExtra("Full Metal Jacket Rounds", 500),
                    new MenuItemExtra("Suppressor", 245) } },
                new MenuItem("M61 Grenade",340) { SubAmount = 1,SubPrice = 340 },
                new MenuItem("Improvised Incendiary",30) { SubAmount = 1,SubPrice = 30 },
                new MenuItem("BZ Gas Grenade",100) { SubAmount = 1,SubPrice = 100 },
                    }),
        });

    }
    private void MarabunteDenMenu()
    {
        PossibleShopMenus.ShopMenuList.AddRange(new List<ShopMenu> {
                new ShopMenu("MarabunteDenMenu", "MarabunteDenMenu", new List<MenuItem>() {
                    new MenuItem("Marijuana",14, 9),
                    new MenuItem("Marijuana",12) { MinimumPurchaseAmount = 500, MaximumPurchaseAmount = 500, PurchaseIncrement = 100 },
                    new MenuItem("Marijuana",10) { MinimumPurchaseAmount = 1000, MaximumPurchaseAmount = 1000, PurchaseIncrement = 100 },
                    new MenuItem("Smoke Shop Rolling Papers",2),
                    new MenuItem("Brass Knuckles",175),
                    new MenuItem("Combat Knife",150),
                    new MenuItem("Vom Feuer Machete",45),
                    new MenuItem("Switchblade",78),
                //Pistola
                new MenuItem("Hawk & Little PTF092F",500) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip",0),
                    new MenuItemExtra("Extended Clip", 75),
                    new MenuItemExtra("Suppressor", 500) },  },
                new MenuItem("Hawk & Little Combat Pistol",800) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 100),
                    new MenuItemExtra("Suppressor", 400), } },
                new MenuItem("Hawk & Little Desert Slug",1400) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120),
                    new MenuItemExtra("Suppressor", 700), } },
                new MenuItem("Hawk & Little 1919",1300) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 50),
                    new MenuItemExtra("Suppressor", 200), } },
                new MenuItem("Hawk & Little Raging Mare Dx",1600),
                new MenuItem("Vom Feuer P69",900) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 100),
                    new MenuItemExtra("Suppressor", 250), } },
                new MenuItem("Vom Feuer SCRAMP",850) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 85),
                    new MenuItemExtra("Suppressor", 300), } },
                new MenuItem("Shrewsbury S7A",1300) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 150),
                    new MenuItemExtra("Suppressor", 250), } },
                //Shotgun
                new MenuItem("Shrewsbury 420 Sawed-Off",350),
                new MenuItem("Shrewsbury 420",375),
                new MenuItem("Toto 12 Guage Sawed-Off",395),
                new MenuItem("Shrewsbury Defender",750),
                //SMG
                new MenuItem("Shrewsbury Luzi",550) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 150),
                    new MenuItemExtra("Suppressor", 275), } },
                new MenuItem("Vom Feuer KEK-9",300) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 150),
                    new MenuItemExtra("Drum Magazine", 180),
                    new MenuItemExtra("Suppressor", 450) } },
                new MenuItem("Hawk & Little Millipede",550) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 220), } },
                ////AR
                //new MenuItem("Shrewsbury A7-4K",650) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 200),
                //    new MenuItemExtra("Drum Magazine", 250),
                //    new MenuItemExtra("Suppressor", 600), } },
                //new MenuItem("Shrewsbury A2-1K",850) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 200),
                //    new MenuItemExtra("Suppressor", 250) } },
                //new MenuItem("Vom Feuer A5-1R",950) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 125),
                //    new MenuItemExtra("Box Magazine", 250),
                //    new MenuItemExtra("Suppressor", 390),} },
                //new MenuItem("Vom Feuer A5-1R MK2",1250) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 145),
                //    new MenuItemExtra("Suppressor", 350),} },
                //new MenuItem("Shrewsbury Stinkov",650) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 125),
                //    new MenuItemExtra("Drum Magazine", 250),} },
                ////LMG
                //new MenuItem("Shrewsbury PDA",1500) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 200),} },
                //new MenuItem("Vom Feuer BAT",1700) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 150),} },
                //new MenuItem("Vom Feuer M70E1",2200) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 250),} },
                ////SNIPER
                //new MenuItem("Shrewsbury PWN",2200) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Suppressor", 340)} },
                ////OTHER
                //new MenuItem("M61 Grenade",500) { SubAmount = 1,SubPrice = 500 },
                //new MenuItem("Improvised Incendiary",60) { SubAmount = 1,SubPrice = 60 },
                //new MenuItem("BZ Gas Grenade",250) { SubAmount = 1,SubPrice = 250 },
                    }),
        });
    }
    private void TriadsDenMenu()
    {
        PossibleShopMenus.ShopMenuList.AddRange(new List<ShopMenu> {
                new ShopMenu("TriadsDenMenu", "TriadsDenMenu", new List<MenuItem>() {
                    new MenuItem("Heroin",130, 100),
                    new MenuItem("Heroin",115) { MinimumPurchaseAmount = 500, MaximumPurchaseAmount = 500, PurchaseIncrement = 100 },
                    new MenuItem("Heroin",105) { MinimumPurchaseAmount = 1000, MaximumPurchaseAmount = 1000, PurchaseIncrement = 100 },
                    new MenuItem("Brass Knuckles",175),
                    new MenuItem("Combat Knife",150),
                    new MenuItem("Vom Feuer Machete",45),
                    new MenuItem("Switchblade",78),
                //Pistola
                new MenuItem("Hawk & Little PTF092F",500) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip",0),
                    new MenuItemExtra("Extended Clip", 75),
                    new MenuItemExtra("Suppressor", 500) },  },
                new MenuItem("Hawk & Little Combat Pistol",800) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 100),
                    new MenuItemExtra("Suppressor", 400), } },
                new MenuItem("Hawk & Little Desert Slug",1400) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120),
                    new MenuItemExtra("Suppressor", 700), } },
                new MenuItem("Hawk & Little 1919",1300) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 50),
                    new MenuItemExtra("Suppressor", 200), } },
                new MenuItem("Hawk & Little Raging Mare Dx",1600),
                new MenuItem("Vom Feuer P69",900) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 100),
                    new MenuItemExtra("Suppressor", 250), } },
                new MenuItem("Vom Feuer SCRAMP",850) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 85),
                    new MenuItemExtra("Suppressor", 300), } },
                new MenuItem("Shrewsbury S7A",1300) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 150),
                    new MenuItemExtra("Suppressor", 250), } },
                ////Shotgun
                //new MenuItem("Shrewsbury 420 Sawed-Off",350),
                //new MenuItem("Shrewsbury 420",375),
                //new MenuItem("Toto 12 Guage Sawed-Off",395),
                //new MenuItem("Shrewsbury Defender",750),
                ////SMG
                //new MenuItem("Shrewsbury Luzi",550) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 150),
                //    new MenuItemExtra("Suppressor", 275), } },
                //new MenuItem("Vom Feuer KEK-9",300) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 150),
                //    new MenuItemExtra("Drum Magazine", 180),
                //    new MenuItemExtra("Suppressor", 450) } },
                //new MenuItem("Hawk & Little Millipede",550) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 220), } },
                ////AR
                new MenuItem("Shrewsbury A7-4K",650) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 200),
                    new MenuItemExtra("Drum Magazine", 250),
                    new MenuItemExtra("Suppressor", 600), } },
                new MenuItem("Shrewsbury A2-1K",850) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 200),
                    new MenuItemExtra("Suppressor", 250) } },
                //new MenuItem("Vom Feuer A5-1R",950) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 125),
                //    new MenuItemExtra("Box Magazine", 250),
                //    new MenuItemExtra("Suppressor", 390),} },
                //new MenuItem("Vom Feuer A5-1R MK2",1250) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 145),
                //    new MenuItemExtra("Suppressor", 350),} },
                new MenuItem("Shrewsbury Stinkov",650) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 125),
                    new MenuItemExtra("Drum Magazine", 250),} },
                //LMG
                new MenuItem("Shrewsbury PDA",1500) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 200),} },
                new MenuItem("Vom Feuer BAT",1700) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 150),} },
                new MenuItem("Vom Feuer M70E1",2200) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 250),} },
                //SNIPER
                new MenuItem("Shrewsbury PWN",2200) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Suppressor", 340)} },
                //OTHER
                new MenuItem("M61 Grenade",500) { SubAmount = 1,SubPrice = 500 },
                new MenuItem("Improvised Incendiary",60) { SubAmount = 1,SubPrice = 60 },
                new MenuItem("BZ Gas Grenade",250) { SubAmount = 1,SubPrice = 250 },
                    }),
        });
    }
    private void KkangpaeDenMenu()
    {
        PossibleShopMenus.ShopMenuList.AddRange(new List<ShopMenu> {
                new ShopMenu("KkangpaeDenMenu", "KkangpaeDenMenu", new List<MenuItem>() {
                    new MenuItem("Heroin",125, 100),
                    new MenuItem("Heroin",115) { MinimumPurchaseAmount = 500, MaximumPurchaseAmount = 500, PurchaseIncrement = 100 },
                    new MenuItem("Heroin",105) { MinimumPurchaseAmount = 1000, MaximumPurchaseAmount = 1000, PurchaseIncrement = 100 },
                    new MenuItem("Brass Knuckles",175),
                    new MenuItem("Combat Knife",150),
                    new MenuItem("Vom Feuer Machete",45),
                    new MenuItem("Switchblade",78),
                //Pistola
                new MenuItem("Hawk & Little PTF092F",500) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip",0),
                    new MenuItemExtra("Extended Clip", 75),
                    new MenuItemExtra("Suppressor", 500) },  },
                new MenuItem("Hawk & Little Combat Pistol",800) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 100),
                    new MenuItemExtra("Suppressor", 400), } },
                //new MenuItem("Hawk & Little Desert Slug",1400) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 120),
                //    new MenuItemExtra("Suppressor", 700), } },
                //new MenuItem("Hawk & Little 1919",1300) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 50),
                //    new MenuItemExtra("Suppressor", 200), } },
                //new MenuItem("Hawk & Little Raging Mare Dx",1600),
                //new MenuItem("Vom Feuer P69",900) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 100),
                //    new MenuItemExtra("Suppressor", 250), } },
                new MenuItem("Vom Feuer SCRAMP",850) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 85),
                    new MenuItemExtra("Suppressor", 300), } },
                new MenuItem("Shrewsbury S7A",1300) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 150),
                    new MenuItemExtra("Suppressor", 250), } },
                ////Shotgun
                //new MenuItem("Shrewsbury 420 Sawed-Off",350),
                //new MenuItem("Shrewsbury 420",375),
                //new MenuItem("Toto 12 Guage Sawed-Off",395),
                //new MenuItem("Shrewsbury Defender",750),
                ////SMG
                //new MenuItem("Shrewsbury Luzi",550) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 150),
                //    new MenuItemExtra("Suppressor", 275), } },
                //new MenuItem("Vom Feuer KEK-9",300) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 150),
                //    new MenuItemExtra("Drum Magazine", 180),
                //    new MenuItemExtra("Suppressor", 450) } },
                //new MenuItem("Hawk & Little Millipede",550) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 220), } },
                //AR
                new MenuItem("Shrewsbury A7-4K",650) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 200),
                    new MenuItemExtra("Drum Magazine", 250),
                    new MenuItemExtra("Suppressor", 600), } },
                new MenuItem("Shrewsbury A2-1K",850) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 200),
                    new MenuItemExtra("Suppressor", 250) } },
                //new MenuItem("Vom Feuer A5-1R",950) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 125),
                //    new MenuItemExtra("Box Magazine", 250),
                //    new MenuItemExtra("Suppressor", 390),} },
                //new MenuItem("Vom Feuer A5-1R MK2",1250) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 145),
                //    new MenuItemExtra("Suppressor", 350),} },
                new MenuItem("Shrewsbury Stinkov",650) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 125),
                    new MenuItemExtra("Drum Magazine", 250),} },
                //LMG
                new MenuItem("Shrewsbury PDA",1500) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 200),} },
                //new MenuItem("Vom Feuer BAT",1700) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 150),} },
                //new MenuItem("Vom Feuer M70E1",2200) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 250),} },
                ////SNIPER
                //new MenuItem("Shrewsbury PWN",2200) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Suppressor", 340)} },
                ////OTHER
                //new MenuItem("M61 Grenade",500) { SubAmount = 1,SubPrice = 500 },
                //new MenuItem("Improvised Incendiary",60) { SubAmount = 1,SubPrice = 60 },
                //new MenuItem("BZ Gas Grenade",250) { SubAmount = 1,SubPrice = 250 },
                    }),
        });
    }
    private void DiablosDenMenu()
    {
        PossibleShopMenus.ShopMenuList.AddRange(new List<ShopMenu> {
               new ShopMenu("DiablosDenMenu","DiablosDenMenu",new List<MenuItem>() {
                    new MenuItem("SPANK", 45, 29),
                    new MenuItem("SPANK",40) { MinimumPurchaseAmount = 500, MaximumPurchaseAmount = 500, PurchaseIncrement = 100 },
                    new MenuItem("SPANK",30) { MinimumPurchaseAmount = 1000, MaximumPurchaseAmount = 1000, PurchaseIncrement = 100 },
                    new MenuItem("Flint Hatchet",80),
                    new MenuItem("Brass Knuckles",150),
                    new MenuItem("Combat Knife",120),
                    new MenuItem("Vom Feuer Machete",29),
                    new MenuItem("Switchblade",55),
                //Pistola
                new MenuItem("Hawk & Little PTF092F",445) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip",0),
                    new MenuItemExtra("Extended Clip", 75),
                    new MenuItemExtra("Suppressor", 343) },  },
                //new MenuItem("Hawk & Little Combat Pistol",780) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 85),
                //    new MenuItemExtra("Suppressor", 345), } },
                //new MenuItem("Hawk & Little Desert Slug",1245) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 120),
                //    new MenuItemExtra("Suppressor", 556), } },
                //new MenuItem("Hawk & Little 1919",1134) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 50),
                //    new MenuItemExtra("Suppressor", 125), } },
                //new MenuItem("Hawk & Little Raging Mare Dx",1450),
                new MenuItem("Vom Feuer P69",790) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 85),
                    new MenuItemExtra("Suppressor", 125), } },
                new MenuItem("Vom Feuer SCRAMP",780) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 85),
                    new MenuItemExtra("Suppressor", 200), } },
                new MenuItem("Shrewsbury S7A",1140) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 68),
                    new MenuItemExtra("Mounted Scope", 890),
                    new MenuItemExtra("Suppressor", 145),
                    new MenuItemExtra("Compensator", 240), } },
                //Shotgun
                //new MenuItem("Shrewsbury 420 Sawed-Off",200),
                //new MenuItem("Shrewsbury 420",200),
                //new MenuItem("Toto 12 Guage Sawed-Off",250),
                new MenuItem("Shrewsbury Defender",550),
                //SMG
                new MenuItem("Shrewsbury Luzi",455) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120),
                    new MenuItemExtra("Suppressor", 145), } },
                //new MenuItem("Vom Feuer KEK-9",250) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 120),
                //    new MenuItemExtra("Drum Magazine", 123),
                //    new MenuItemExtra("Suppressor", 356) } },
                //new MenuItem("Hawk & Little Millipede",450) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 120), } },
                ////AR
                //new MenuItem("Shrewsbury A7-4K",550) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 125),
                //    new MenuItemExtra("Drum Magazine", 200),
                //    new MenuItemExtra("Suppressor", 456), } },
                //new MenuItem("Shrewsbury A2-1K",656) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 150),
                //    new MenuItemExtra("Tracer Rounds", 500),
                //    new MenuItemExtra("Incendiary Rounds", 500),
                //    new MenuItemExtra("Armor Piercing Rounds", 500),
                //    new MenuItemExtra("Full Metal Jacket Rounds", 500),
                //    new MenuItemExtra("Suppressor", 245) } },
                //new MenuItem("Vom Feuer A5-1R",650) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 125),
                //    new MenuItemExtra("Box Magazine", 200),
                //    new MenuItemExtra("Suppressor", 225),} },
                //new MenuItem("Vom Feuer A5-1R MK2",950) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 145),
                //    new MenuItemExtra("Holographic Sight", 780),
                //    new MenuItemExtra("Suppressor", 225),} },
                //new MenuItem("Shrewsbury Stinkov",450) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 125),
                //    new MenuItemExtra("Drum Magazine", 200),} },
                ////LMG
                //new MenuItem("Shrewsbury PDA",1200) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 125),} },
                //new MenuItem("Vom Feuer BAT",1340) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 125),
                //    new MenuItemExtra("Grip", 90),
                //    new MenuItemExtra("Scope", 567),} },
                //new MenuItem("Vom Feuer M70E1",1680) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 145),
                //    new MenuItemExtra("Tracer Rounds", 500),
                //    new MenuItemExtra("Incendiary Rounds", 500),
                //    new MenuItemExtra("Armor Piercing Rounds", 500),
                //    new MenuItemExtra("Full Metal Jacket Rounds", 500),} },
                //SNIPER
                new MenuItem("Shrewsbury PWN",2200) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Suppressor", 340)} },
                new MenuItem("Bartlett M92",4578),
                new MenuItem("Bartlett M92 Mk2",3456),
                ////OTHER
                //new MenuItem("RPG-7",1800){ SubAmount = 1,SubPrice = 500 },
                //new MenuItem("Hawk & Little MGL",2500){ SubAmount = 6,SubPrice = 200 },
                //new MenuItem("M61 Grenade",340) { SubAmount = 1,SubPrice = 340 },
                //new MenuItem("Improvised Incendiary",30) { SubAmount = 1,SubPrice = 30 },
                //new MenuItem("BZ Gas Grenade",100) { SubAmount = 1,SubPrice = 100 },
                    }),
        });
    }
    private void YardiesDenMenu()
    {
        PossibleShopMenus.ShopMenuList.AddRange(new List<ShopMenu> {
               new ShopMenu("YardiesDenMenu","YardiesDenMenu",new List<MenuItem>() {
                    new MenuItem("Marijuana", 13, 9),
                    new MenuItem("Marijuana",12) { MinimumPurchaseAmount = 500, MaximumPurchaseAmount = 500, PurchaseIncrement = 100 },
                    new MenuItem("Marijuana",10) { MinimumPurchaseAmount = 1000, MaximumPurchaseAmount = 1000, PurchaseIncrement = 100 },
                    new MenuItem("Smoke Shop Rolling Papers",2),
                    new MenuItem("Flint Hatchet",80),
                    new MenuItem("Brass Knuckles",150),
                    new MenuItem("Combat Knife",120),
                    new MenuItem("Vom Feuer Machete",29),
                    new MenuItem("Switchblade",55),
                //Pistola
                new MenuItem("Hawk & Little PTF092F",445) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip",0),
                    new MenuItemExtra("Extended Clip", 75),
                    new MenuItemExtra("Suppressor", 343) },  },
                new MenuItem("Hawk & Little Combat Pistol",780) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 85),
                    new MenuItemExtra("Suppressor", 345), } },
                new MenuItem("Hawk & Little Desert Slug",1245) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120),
                    new MenuItemExtra("Suppressor", 556), } },
                //new MenuItem("Hawk & Little 1919",1134) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 50),
                //    new MenuItemExtra("Suppressor", 125), } },
                //new MenuItem("Hawk & Little Raging Mare Dx",1450),
                //new MenuItem("Vom Feuer P69",790) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 85),
                //    new MenuItemExtra("Suppressor", 125), } },
                //new MenuItem("Vom Feuer SCRAMP",780) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 85),
                //    new MenuItemExtra("Suppressor", 200), } },
                //new MenuItem("Shrewsbury S7A",1140) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 68),
                //    new MenuItemExtra("Mounted Scope", 890),
                //    new MenuItemExtra("Suppressor", 145),
                //    new MenuItemExtra("Compensator", 240), } },
                //Shotgun
                new MenuItem("Shrewsbury 420 Sawed-Off",200),
                new MenuItem("Shrewsbury 420",200),
                new MenuItem("Toto 12 Guage Sawed-Off",250),
                new MenuItem("Shrewsbury Defender",550),
                //SMG
                new MenuItem("Shrewsbury Luzi",455) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120),
                    new MenuItemExtra("Suppressor", 145), } },
                new MenuItem("Vom Feuer KEK-9",250) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120),
                    new MenuItemExtra("Drum Magazine", 123),
                    new MenuItemExtra("Suppressor", 356) } },
                new MenuItem("Hawk & Little Millipede",450) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120), } },
                ////AR
                //new MenuItem("Shrewsbury A7-4K",550) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 125),
                //    new MenuItemExtra("Drum Magazine", 200),
                //    new MenuItemExtra("Suppressor", 456), } },
                //new MenuItem("Shrewsbury A2-1K",656) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 150),
                //    new MenuItemExtra("Tracer Rounds", 500),
                //    new MenuItemExtra("Incendiary Rounds", 500),
                //    new MenuItemExtra("Armor Piercing Rounds", 500),
                //    new MenuItemExtra("Full Metal Jacket Rounds", 500),
                //    new MenuItemExtra("Suppressor", 245) } },
                //new MenuItem("Vom Feuer A5-1R",650) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 125),
                //    new MenuItemExtra("Box Magazine", 200),
                //    new MenuItemExtra("Suppressor", 225),} },
                //new MenuItem("Vom Feuer A5-1R MK2",950) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 145),
                //    new MenuItemExtra("Holographic Sight", 780),
                //    new MenuItemExtra("Suppressor", 225),} },
                //new MenuItem("Shrewsbury Stinkov",450) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 125),
                //    new MenuItemExtra("Drum Magazine", 200),} },
                ////LMG
                //new MenuItem("Shrewsbury PDA",1200) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 125),} },
                //new MenuItem("Vom Feuer BAT",1340) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 125),
                //    new MenuItemExtra("Grip", 90),
                //    new MenuItemExtra("Scope", 567),} },
                //new MenuItem("Vom Feuer M70E1",1680) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 145),
                //    new MenuItemExtra("Tracer Rounds", 500),
                //    new MenuItemExtra("Incendiary Rounds", 500),
                //    new MenuItemExtra("Armor Piercing Rounds", 500),
                //    new MenuItemExtra("Full Metal Jacket Rounds", 500),} },
                ////SNIPER
                //new MenuItem("Shrewsbury PWN",2200) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Suppressor", 340)} },
                //new MenuItem("Bartlett M92",4578),
                //new MenuItem("Bartlett M92 Mk2",3456),
                ////OTHER
                //new MenuItem("RPG-7",1800){ SubAmount = 1,SubPrice = 500 },
                //new MenuItem("Hawk & Little MGL",2500){ SubAmount = 6,SubPrice = 200 },
                //new MenuItem("M61 Grenade",340) { SubAmount = 1,SubPrice = 340 },
                //new MenuItem("Improvised Incendiary",30) { SubAmount = 1,SubPrice = 30 },
                //new MenuItem("BZ Gas Grenade",100) { SubAmount = 1,SubPrice = 100 },
                    }),
        });
    }
    private void ArmenianDenMenu()
    {
        PossibleShopMenus.ShopMenuList.AddRange(new List<ShopMenu> {
               new ShopMenu("ArmenianDenMenu","ArmenianDenMenu",new List<MenuItem>() {
                    new MenuItem("Heroin",135, 99),
                    new MenuItem("Heroin",120) { MinimumPurchaseAmount = 500, MaximumPurchaseAmount = 500, PurchaseIncrement = 100 },
                    new MenuItem("Heroin",100) { MinimumPurchaseAmount = 1000, MaximumPurchaseAmount = 1000, PurchaseIncrement = 100 },
                    new MenuItem("Flint Hatchet",80),
                    new MenuItem("Brass Knuckles",150),
                    new MenuItem("Combat Knife",120),
                    new MenuItem("Vom Feuer Machete",29),
                    new MenuItem("Switchblade",55),
                //Pistola
                new MenuItem("Hawk & Little PTF092F",445) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip",0),
                    new MenuItemExtra("Extended Clip", 75),
                    new MenuItemExtra("Suppressor", 343) },  },
                new MenuItem("Hawk & Little Combat Pistol",780) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 85),
                    new MenuItemExtra("Suppressor", 345), } },
                new MenuItem("Hawk & Little Desert Slug",1245) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120),
                    new MenuItemExtra("Suppressor", 556), } },
                new MenuItem("Hawk & Little 1919",1134) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 50),
                    new MenuItemExtra("Suppressor", 125), } },
                new MenuItem("Hawk & Little Raging Mare Dx",1450),
                new MenuItem("Vom Feuer P69",790) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 85),
                    new MenuItemExtra("Suppressor", 125), } },
                new MenuItem("Vom Feuer SCRAMP",780) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 85),
                    new MenuItemExtra("Suppressor", 200), } },
                new MenuItem("Shrewsbury S7A",1140) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 68),
                    new MenuItemExtra("Mounted Scope", 890),
                    new MenuItemExtra("Suppressor", 145),
                    new MenuItemExtra("Compensator", 240), } },
                ////Shotgun
                //new MenuItem("Shrewsbury 420 Sawed-Off",200),
                //new MenuItem("Shrewsbury 420",200),
                //new MenuItem("Toto 12 Guage Sawed-Off",250),
                //new MenuItem("Shrewsbury Defender",550),
                ////SMG
                //new MenuItem("Shrewsbury Luzi",455) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 120),
                //    new MenuItemExtra("Suppressor", 145), } },
                //new MenuItem("Vom Feuer KEK-9",250) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 120),
                //    new MenuItemExtra("Drum Magazine", 123),
                //    new MenuItemExtra("Suppressor", 356) } },
                //new MenuItem("Hawk & Little Millipede",450) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 120), } },
                //AR
                new MenuItem("Shrewsbury A7-4K",550) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 125),
                    new MenuItemExtra("Drum Magazine", 200),
                    new MenuItemExtra("Suppressor", 456), } },
                new MenuItem("Shrewsbury A2-1K",656) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 150),
                    new MenuItemExtra("Tracer Rounds", 500),
                    new MenuItemExtra("Incendiary Rounds", 500),
                    new MenuItemExtra("Armor Piercing Rounds", 500),
                    new MenuItemExtra("Full Metal Jacket Rounds", 500),
                    new MenuItemExtra("Suppressor", 245) } },
                new MenuItem("Vom Feuer A5-1R",650) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 125),
                    new MenuItemExtra("Box Magazine", 200),
                    new MenuItemExtra("Suppressor", 225),} },
                new MenuItem("Vom Feuer A5-1R MK2",950) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 145),
                    new MenuItemExtra("Holographic Sight", 780),
                    new MenuItemExtra("Suppressor", 225),} },
                new MenuItem("Shrewsbury Stinkov",450) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 125),
                    new MenuItemExtra("Drum Magazine", 200),} },
                new MenuItem("Vom Feuer LAR",550, 145) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 75),
                    new MenuItemExtra("Suppressor", 400),} },
                //LMG
                new MenuItem("Shrewsbury PDA",1200) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 125),} },
                new MenuItem("Vom Feuer BAT",1340) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 125),
                    new MenuItemExtra("Grip", 90),
                    new MenuItemExtra("Scope", 567),} },
                new MenuItem("Vom Feuer M70E1",1680) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 145),
                    new MenuItemExtra("Tracer Rounds", 500),
                    new MenuItemExtra("Incendiary Rounds", 500),
                    new MenuItemExtra("Armor Piercing Rounds", 500),
                    new MenuItemExtra("Full Metal Jacket Rounds", 500),} },
                //SNIPER
                new MenuItem("Shrewsbury PWN",2200) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Suppressor", 340)} },
                new MenuItem("Bartlett M92",4578),
                new MenuItem("Bartlett M92 Mk2",3456),
                //OTHER
                new MenuItem("RPG-7",1800){ SubAmount = 1,SubPrice = 500 },
                new MenuItem("Hawk & Little MGL",2500){ SubAmount = 6,SubPrice = 200 },
                //new MenuItem("M61 Grenade",340) { SubAmount = 1,SubPrice = 340 },
                //new MenuItem("Improvised Incendiary",30) { SubAmount = 1,SubPrice = 30 },
                //new MenuItem("BZ Gas Grenade",100) { SubAmount = 1,SubPrice = 100 },


                new MenuItem("Benefactor Schafter",10000,5000),
                new MenuItem("Benefactor Serrano",8000,5000),
                new MenuItem("Karin Futo",3000,1000),
                    }),
        });;
    }
    private void MadrazoDenMenu()
    {
        PossibleShopMenus.ShopMenuList.AddRange(new List<ShopMenu> {
               new ShopMenu("MadrazoDenMenu","MadrazoDenMenu",new List<MenuItem>() {
                    new MenuItem("Methamphetamine",45, 34),


                    new MenuItem("Methamphetamine",40) { MinimumPurchaseAmount = 500, MaximumPurchaseAmount = 1000, PurchaseIncrement = 100 },
                    new MenuItem("Methamphetamine",35) { MinimumPurchaseAmount = 1000, MaximumPurchaseAmount = 5000, PurchaseIncrement = 500 },
                    new MenuItem("Meth Pipe",5),

                    new MenuItem("Flint Hatchet",80),
                    new MenuItem("Brass Knuckles",150),
                    new MenuItem("Combat Knife",120),
                    new MenuItem("Vom Feuer Machete",29),
                    new MenuItem("Switchblade",55),
                //Pistola
                new MenuItem("Hawk & Little PTF092F",445) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip",0),
                    new MenuItemExtra("Extended Clip", 75),
                    new MenuItemExtra("Suppressor", 343) },  },
                new MenuItem("Hawk & Little Combat Pistol",780) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 85),
                    new MenuItemExtra("Suppressor", 345), } },
                new MenuItem("Hawk & Little Desert Slug",1245) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120),
                    new MenuItemExtra("Suppressor", 556), } },
                new MenuItem("Hawk & Little 1919",1134) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 50),
                    new MenuItemExtra("Suppressor", 125), } },
                new MenuItem("Hawk & Little Raging Mare Dx",1450),
                new MenuItem("Vom Feuer P69",790) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 85),
                    new MenuItemExtra("Suppressor", 125), } },
                new MenuItem("Vom Feuer SCRAMP",780) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 85),
                    new MenuItemExtra("Suppressor", 200), } },
                new MenuItem("Shrewsbury S7A",1140) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 68),
                    new MenuItemExtra("Mounted Scope", 890),
                    new MenuItemExtra("Suppressor", 145),
                    new MenuItemExtra("Compensator", 240), } },
                //Shotgun
                new MenuItem("Shrewsbury 420 Sawed-Off",200),
                new MenuItem("Shrewsbury 420",200),
                new MenuItem("Toto 12 Guage Sawed-Off",250),
                new MenuItem("Shrewsbury Defender",550),
                //SMG
                new MenuItem("Shrewsbury Luzi",455) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120),
                    new MenuItemExtra("Suppressor", 145), } },
                new MenuItem("Vom Feuer KEK-9",250) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120),
                    new MenuItemExtra("Drum Magazine", 123),
                    new MenuItemExtra("Suppressor", 356) } },
                new MenuItem("Hawk & Little Millipede",450) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120), } },
                //AR
                new MenuItem("Shrewsbury A7-4K",550) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 125),
                    new MenuItemExtra("Drum Magazine", 200),
                    new MenuItemExtra("Suppressor", 456), } },
                new MenuItem("Shrewsbury A2-1K",656) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 150),
                    new MenuItemExtra("Tracer Rounds", 500),
                    new MenuItemExtra("Incendiary Rounds", 500),
                    new MenuItemExtra("Armor Piercing Rounds", 500),
                    new MenuItemExtra("Full Metal Jacket Rounds", 500),
                    new MenuItemExtra("Suppressor", 245) } },
                //new MenuItem("Vom Feuer A5-1R",650) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 125),
                //    new MenuItemExtra("Box Magazine", 200),
                //    new MenuItemExtra("Suppressor", 225),} },
                //new MenuItem("Vom Feuer A5-1R MK2",950) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 145),
                //    new MenuItemExtra("Holographic Sight", 780),
                //    new MenuItemExtra("Suppressor", 225),} },
                new MenuItem("Shrewsbury Stinkov",450) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 125),
                    new MenuItemExtra("Drum Magazine", 200),} },
                //LMG
                new MenuItem("Shrewsbury PDA",1200) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 125),} },
                new MenuItem("Vom Feuer BAT",1340) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 125),
                    new MenuItemExtra("Grip", 90),
                    new MenuItemExtra("Scope", 567),} },
                new MenuItem("Vom Feuer M70E1",1680) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 145),
                    new MenuItemExtra("Tracer Rounds", 500),
                    new MenuItemExtra("Incendiary Rounds", 500),
                    new MenuItemExtra("Armor Piercing Rounds", 500),
                    new MenuItemExtra("Full Metal Jacket Rounds", 500),} },
                //SNIPER
                new MenuItem("Shrewsbury PWN",2200) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Suppressor", 340)} },
                new MenuItem("Bartlett M92",4578),
                new MenuItem("Bartlett M92 Mk2",3456),
                //OTHER
                new MenuItem("RPG-7",1800){ SubAmount = 1,SubPrice = 500 },
                new MenuItem("Hawk & Little MGL",2500){ SubAmount = 6,SubPrice = 200 },
                new MenuItem("M61 Grenade",340) { SubAmount = 1,SubPrice = 340 },
                new MenuItem("Improvised Incendiary",30) { SubAmount = 1,SubPrice = 30 },
                new MenuItem("BZ Gas Grenade",100) { SubAmount = 1,SubPrice = 100 },
                    }),
        });
    }
    private void GambettiDenMenu()
    {
        PossibleShopMenus.ShopMenuList.AddRange(new List<ShopMenu> {
                new ShopMenu("GambettiDenMenu", "GambettiDenMenu", new List<MenuItem>() {
                    new MenuItem("Cocaine",150, 109),
                    new MenuItem("Cocaine",130) { MinimumPurchaseAmount = 500, MaximumPurchaseAmount = 1000, PurchaseIncrement = 100 },
                    new MenuItem("Cocaine",110) { MinimumPurchaseAmount = 1000, MaximumPurchaseAmount = 5000, PurchaseIncrement = 500 },
                    new MenuItem("Brass Knuckles",175),
                    new MenuItem("Combat Knife",150),
                    new MenuItem("Vom Feuer Machete",45),
                    new MenuItem("Switchblade",78),
                //Pistola
                new MenuItem("Hawk & Little PTF092F",500) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip",0),
                    new MenuItemExtra("Extended Clip", 75),
                    new MenuItemExtra("Suppressor", 500) },  },
                //new MenuItem("Hawk & Little Combat Pistol",800) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 100),
                //    new MenuItemExtra("Suppressor", 400), } },
                new MenuItem("Hawk & Little Desert Slug",1400) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120),
                    new MenuItemExtra("Suppressor", 700), } },
                new MenuItem("Hawk & Little 1919",1300) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 50),
                    new MenuItemExtra("Suppressor", 200), } },
                new MenuItem("Hawk & Little Raging Mare Dx",1600),
                //new MenuItem("Vom Feuer P69",900) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 100),
                //    new MenuItemExtra("Suppressor", 250), } },
                //new MenuItem("Vom Feuer SCRAMP",850) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 85),
                //    new MenuItemExtra("Suppressor", 300), } },
                //new MenuItem("Shrewsbury S7A",1300) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 150),
                //    new MenuItemExtra("Suppressor", 250), } },
                ////Shotgun
                //new MenuItem("Shrewsbury 420 Sawed-Off",350),
                //new MenuItem("Shrewsbury 420",375),
                //new MenuItem("Toto 12 Guage Sawed-Off",395),
                //new MenuItem("Shrewsbury Defender",750),
                ////SMG
                //new MenuItem("Shrewsbury Luzi",550) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 150),
                //    new MenuItemExtra("Suppressor", 275), } },
                //new MenuItem("Vom Feuer KEK-9",300) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 150),
                //    new MenuItemExtra("Drum Magazine", 180),
                //    new MenuItemExtra("Suppressor", 450) } },
                //new MenuItem("Hawk & Little Millipede",550) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 220), } },
                //AR
                new MenuItem("Shrewsbury A7-4K",650) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 200),
                    new MenuItemExtra("Drum Magazine", 250),
                    new MenuItemExtra("Suppressor", 600), } },
                new MenuItem("Shrewsbury A2-1K",850) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 200),
                    new MenuItemExtra("Suppressor", 250) } },
                //new MenuItem("Vom Feuer A5-1R",950) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 125),
                //    new MenuItemExtra("Box Magazine", 250),
                //    new MenuItemExtra("Suppressor", 390),} },
                //new MenuItem("Vom Feuer A5-1R MK2",1250) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 145),
                //    new MenuItemExtra("Suppressor", 350),} },
                //new MenuItem("Shrewsbury Stinkov",650) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 125),
                //    new MenuItemExtra("Drum Magazine", 250),} },
                ////LMG
                //new MenuItem("Shrewsbury PDA",1500) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 200),} },
                //new MenuItem("Vom Feuer BAT",1700) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 150),} },
                //new MenuItem("Vom Feuer M70E1",2200) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 250),} },
                ////SNIPER
                //new MenuItem("Shrewsbury PWN",2200) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Suppressor", 340)} },
                ////OTHER
                //new MenuItem("M61 Grenade",500) { SubAmount = 1,SubPrice = 500 },
                //new MenuItem("Improvised Incendiary",60) { SubAmount = 1,SubPrice = 60 },
                //new MenuItem("BZ Gas Grenade",250) { SubAmount = 1,SubPrice = 250 },
                    }),
        });
    }
    private void PavanoDenMenu()
    {
        PossibleShopMenus.ShopMenuList.AddRange(new List<ShopMenu> {
                new ShopMenu("PavanoDenMenu", "PavanoDenMenu", new List<MenuItem>() {
                    new MenuItem("Cocaine",150, 109),
                    new MenuItem("Cocaine",130) { MinimumPurchaseAmount = 500, MaximumPurchaseAmount = 1000, PurchaseIncrement = 100 },
                    new MenuItem("Cocaine",110) { MinimumPurchaseAmount = 1000, MaximumPurchaseAmount = 5000, PurchaseIncrement = 500 },
                    new MenuItem("Brass Knuckles",175),
                    new MenuItem("Combat Knife",150),
                    new MenuItem("Vom Feuer Machete",45),
                    new MenuItem("Switchblade",78),
                //Pistola
                new MenuItem("Hawk & Little PTF092F",500) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip",0),
                    new MenuItemExtra("Extended Clip", 75),
                    new MenuItemExtra("Suppressor", 500) },  },
                //new MenuItem("Hawk & Little Combat Pistol",800) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 100),
                //    new MenuItemExtra("Suppressor", 400), } },
                //new MenuItem("Hawk & Little Desert Slug",1400) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 120),
                //    new MenuItemExtra("Suppressor", 700), } },
                new MenuItem("Hawk & Little 1919",1300) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 50),
                    new MenuItemExtra("Suppressor", 200), } },
                new MenuItem("Hawk & Little Raging Mare Dx",1600),
                new MenuItem("Vom Feuer P69",900) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 100),
                    new MenuItemExtra("Suppressor", 250), } },
                new MenuItem("Vom Feuer SCRAMP",850) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 85),
                    new MenuItemExtra("Suppressor", 300), } },
                new MenuItem("Shrewsbury S7A",1300) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 150),
                    new MenuItemExtra("Suppressor", 250), } },
                //Shotgun
                new MenuItem("Shrewsbury 420 Sawed-Off",350),
                new MenuItem("Shrewsbury 420",375),
                //new MenuItem("Toto 12 Guage Sawed-Off",395),
                //new MenuItem("Shrewsbury Defender",750),
                ////SMG
                //new MenuItem("Shrewsbury Luzi",550) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 150),
                //    new MenuItemExtra("Suppressor", 275), } },
                //new MenuItem("Vom Feuer KEK-9",300) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 150),
                //    new MenuItemExtra("Drum Magazine", 180),
                //    new MenuItemExtra("Suppressor", 450) } },
                //new MenuItem("Hawk & Little Millipede",550) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 220), } },
                ////AR
                //new MenuItem("Shrewsbury A7-4K",650) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 200),
                //    new MenuItemExtra("Drum Magazine", 250),
                //    new MenuItemExtra("Suppressor", 600), } },
                //new MenuItem("Shrewsbury A2-1K",850) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 200),
                //    new MenuItemExtra("Suppressor", 250) } },
                //new MenuItem("Vom Feuer A5-1R",950) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 125),
                //    new MenuItemExtra("Box Magazine", 250),
                //    new MenuItemExtra("Suppressor", 390),} },
                //new MenuItem("Vom Feuer A5-1R MK2",1250) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 145),
                //    new MenuItemExtra("Suppressor", 350),} },
                //new MenuItem("Shrewsbury Stinkov",650) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 125),
                //    new MenuItemExtra("Drum Magazine", 250),} },
                ////LMG
                //new MenuItem("Shrewsbury PDA",1500) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 200),} },
                //new MenuItem("Vom Feuer BAT",1700) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 150),} },
                //new MenuItem("Vom Feuer M70E1",2200) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 250),} },
                //SNIPER
                new MenuItem("Shrewsbury PWN",2200) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Suppressor", 340)} },
                ////OTHER
                //new MenuItem("M61 Grenade",500) { SubAmount = 1,SubPrice = 500 },
                //new MenuItem("Improvised Incendiary",60) { SubAmount = 1,SubPrice = 60 },
                //new MenuItem("BZ Gas Grenade",250) { SubAmount = 1,SubPrice = 250 },
                    }),
        });
    }
    private void LupisellaDenMenu()
    {
        PossibleShopMenus.ShopMenuList.AddRange(new List<ShopMenu> {
                new ShopMenu("LupisellaDenMenu", "LupisellaDenMenu", new List<MenuItem>() {
                    new MenuItem("Cocaine",150, 109),
                    new MenuItem("Cocaine",130) { MinimumPurchaseAmount = 500, MaximumPurchaseAmount = 1000, PurchaseIncrement = 100 },
                    new MenuItem("Cocaine",110) { MinimumPurchaseAmount = 1000, MaximumPurchaseAmount = 5000, PurchaseIncrement = 500 },
                    new MenuItem("Brass Knuckles",175),
                    new MenuItem("Combat Knife",150),
                    new MenuItem("Vom Feuer Machete",45),
                    new MenuItem("Switchblade",78),
                //Pistola
                new MenuItem("Hawk & Little PTF092F",500) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip",0),
                    new MenuItemExtra("Extended Clip", 75),
                    new MenuItemExtra("Suppressor", 500) },  },
                new MenuItem("Hawk & Little Combat Pistol",800) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 100),
                    new MenuItemExtra("Suppressor", 400), } },
                new MenuItem("Hawk & Little Desert Slug",1400) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120),
                    new MenuItemExtra("Suppressor", 700), } },
                //new MenuItem("Hawk & Little 1919",1300) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 50),
                //    new MenuItemExtra("Suppressor", 200), } },
                //new MenuItem("Hawk & Little Raging Mare Dx",1600),
                //new MenuItem("Vom Feuer P69",900) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 100),
                //    new MenuItemExtra("Suppressor", 250), } },
                //new MenuItem("Vom Feuer SCRAMP",850) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 85),
                //    new MenuItemExtra("Suppressor", 300), } },
                //new MenuItem("Shrewsbury S7A",1300) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 150),
                //    new MenuItemExtra("Suppressor", 250), } },
                //Shotgun
                new MenuItem("Shrewsbury 420 Sawed-Off",350),
                new MenuItem("Shrewsbury 420",375),
                new MenuItem("Toto 12 Guage Sawed-Off",395),
                new MenuItem("Shrewsbury Defender",750),
                ////SMG
                //new MenuItem("Shrewsbury Luzi",550) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 150),
                //    new MenuItemExtra("Suppressor", 275), } },
                //new MenuItem("Vom Feuer KEK-9",300) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 150),
                //    new MenuItemExtra("Drum Magazine", 180),
                //    new MenuItemExtra("Suppressor", 450) } },
                //new MenuItem("Hawk & Little Millipede",550) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 220), } },
                ////AR
                //new MenuItem("Shrewsbury A7-4K",650) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 200),
                //    new MenuItemExtra("Drum Magazine", 250),
                //    new MenuItemExtra("Suppressor", 600), } },
                //new MenuItem("Shrewsbury A2-1K",850) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 200),
                //    new MenuItemExtra("Suppressor", 250) } },
                //new MenuItem("Vom Feuer A5-1R",950) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 125),
                //    new MenuItemExtra("Box Magazine", 250),
                //    new MenuItemExtra("Suppressor", 390),} },
                //new MenuItem("Vom Feuer A5-1R MK2",1250) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 145),
                //    new MenuItemExtra("Suppressor", 350),} },
                //new MenuItem("Shrewsbury Stinkov",650) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 125),
                //    new MenuItemExtra("Drum Magazine", 250),} },
                ////LMG
                //new MenuItem("Shrewsbury PDA",1500) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 200),} },
                //new MenuItem("Vom Feuer BAT",1700) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 150),} },
                //new MenuItem("Vom Feuer M70E1",2200) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 250),} },
                ////SNIPER
                //new MenuItem("Shrewsbury PWN",2200) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Suppressor", 340)} },
                //OTHER
                new MenuItem("M61 Grenade",500) { SubAmount = 1,SubPrice = 500 },
                new MenuItem("Improvised Incendiary",60) { SubAmount = 1,SubPrice = 60 },
                new MenuItem("BZ Gas Grenade",250) { SubAmount = 1,SubPrice = 250 },
                    }),
        });
    }
    private void MessinaDenMenu()
    {
        PossibleShopMenus.ShopMenuList.AddRange(new List<ShopMenu> {
                new ShopMenu("MessinaDenMenu", "MessinaDenMenu", new List<MenuItem>() {
                    new MenuItem("Cocaine",150, 109),
                    new MenuItem("Cocaine",130) { MinimumPurchaseAmount = 500, MaximumPurchaseAmount = 1000, PurchaseIncrement = 100 },
                    new MenuItem("Cocaine",110) { MinimumPurchaseAmount = 1000, MaximumPurchaseAmount = 5000, PurchaseIncrement = 500 },
                    new MenuItem("Brass Knuckles",175),
                    new MenuItem("Combat Knife",150),
                    new MenuItem("Vom Feuer Machete",45),
                    new MenuItem("Switchblade",78),
                //Pistola
                new MenuItem("Hawk & Little PTF092F",500) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip",0),
                    new MenuItemExtra("Extended Clip", 75),
                    new MenuItemExtra("Suppressor", 500) },  },
                new MenuItem("Hawk & Little Combat Pistol",800) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 100),
                    new MenuItemExtra("Suppressor", 400), } },
                new MenuItem("Hawk & Little Desert Slug",1400) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120),
                    new MenuItemExtra("Suppressor", 700), } },
                new MenuItem("Hawk & Little 1919",1300) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 50),
                    new MenuItemExtra("Suppressor", 200), } },
                new MenuItem("Hawk & Little Raging Mare Dx",1600),
                //new MenuItem("Vom Feuer P69",900) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 100),
                //    new MenuItemExtra("Suppressor", 250), } },
                //new MenuItem("Vom Feuer SCRAMP",850) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 85),
                //    new MenuItemExtra("Suppressor", 300), } },
                //new MenuItem("Shrewsbury S7A",1300) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 150),
                //    new MenuItemExtra("Suppressor", 250), } },
                //Shotgun
                new MenuItem("Shrewsbury 420 Sawed-Off",350),
                new MenuItem("Shrewsbury 420",375),
                //new MenuItem("Toto 12 Guage Sawed-Off",395),
                //new MenuItem("Shrewsbury Defender",750),
                ////SMG
                //new MenuItem("Shrewsbury Luzi",550) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 150),
                //    new MenuItemExtra("Suppressor", 275), } },
                //new MenuItem("Vom Feuer KEK-9",300) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 150),
                //    new MenuItemExtra("Drum Magazine", 180),
                //    new MenuItemExtra("Suppressor", 450) } },
                //new MenuItem("Hawk & Little Millipede",550) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 220), } },
                ////AR
                //new MenuItem("Shrewsbury A7-4K",650) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 200),
                //    new MenuItemExtra("Drum Magazine", 250),
                //    new MenuItemExtra("Suppressor", 600), } },
                //new MenuItem("Shrewsbury A2-1K",850) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 200),
                //    new MenuItemExtra("Suppressor", 250) } },
                new MenuItem("Vom Feuer A5-1R",950) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 125),
                    new MenuItemExtra("Box Magazine", 250),
                    new MenuItemExtra("Suppressor", 390),} },
                //new MenuItem("Vom Feuer A5-1R MK2",1250) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 145),
                //    new MenuItemExtra("Suppressor", 350),} },
                //new MenuItem("Shrewsbury Stinkov",650) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 125),
                //    new MenuItemExtra("Drum Magazine", 250),} },
                ////LMG
                //new MenuItem("Shrewsbury PDA",1500) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 200),} },
                //new MenuItem("Vom Feuer BAT",1700) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 150),} },
                //new MenuItem("Vom Feuer M70E1",2200) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 250),} },
                ////SNIPER
                //new MenuItem("Shrewsbury PWN",2200) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Suppressor", 340)} },
                ////OTHER
                //new MenuItem("M61 Grenade",500) { SubAmount = 1,SubPrice = 500 },
                //new MenuItem("Improvised Incendiary",60) { SubAmount = 1,SubPrice = 60 },
                //new MenuItem("BZ Gas Grenade",250) { SubAmount = 1,SubPrice = 250 },
                    }),
        });
    }
    private void AncelottiDenMenu()
    {
        PossibleShopMenus.ShopMenuList.AddRange(new List<ShopMenu> {
                new ShopMenu("AncelottiDenMenu", "AncelottiDenMenu", new List<MenuItem>() {
                    new MenuItem("Cocaine",150, 109),
                    new MenuItem("Cocaine",130) { MinimumPurchaseAmount = 500, MaximumPurchaseAmount = 1000, PurchaseIncrement = 100 },
                    new MenuItem("Cocaine",110) { MinimumPurchaseAmount = 1000, MaximumPurchaseAmount = 5000, PurchaseIncrement = 500 },
                    new MenuItem("Brass Knuckles",175),
                    new MenuItem("Combat Knife",150),
                    new MenuItem("Vom Feuer Machete",45),
                    new MenuItem("Switchblade",78),
                //Pistola
                new MenuItem("Hawk & Little PTF092F",500) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip",0),
                    new MenuItemExtra("Extended Clip", 75),
                    new MenuItemExtra("Suppressor", 500) },  },
                new MenuItem("Hawk & Little Combat Pistol",800) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 100),
                    new MenuItemExtra("Suppressor", 400), } },
                new MenuItem("Hawk & Little Desert Slug",1400) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120),
                    new MenuItemExtra("Suppressor", 700), } },
                //new MenuItem("Hawk & Little 1919",1300) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 50),
                //    new MenuItemExtra("Suppressor", 200), } },
                new MenuItem("Hawk & Little Raging Mare Dx",1600),
                new MenuItem("Vom Feuer P69",900) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 100),
                    new MenuItemExtra("Suppressor", 250), } },
                new MenuItem("Vom Feuer SCRAMP",850) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 85),
                    new MenuItemExtra("Suppressor", 300), } },
                new MenuItem("Shrewsbury S7A",1300) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 150),
                    new MenuItemExtra("Suppressor", 250), } },
                //Shotgun
                new MenuItem("Shrewsbury 420 Sawed-Off",350),
                //new MenuItem("Shrewsbury 420",375),
                //new MenuItem("Toto 12 Guage Sawed-Off",395),
                //new MenuItem("Shrewsbury Defender",750),
                ////SMG
                //new MenuItem("Shrewsbury Luzi",550) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 150),
                //    new MenuItemExtra("Suppressor", 275), } },
                //new MenuItem("Vom Feuer KEK-9",300) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 150),
                //    new MenuItemExtra("Drum Magazine", 180),
                //    new MenuItemExtra("Suppressor", 450) } },
                //new MenuItem("Hawk & Little Millipede",550) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 220), } },
                //AR
                //new MenuItem("Shrewsbury A7-4K",650) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 200),
                //    new MenuItemExtra("Drum Magazine", 250),
                //    new MenuItemExtra("Suppressor", 600), } },
                //new MenuItem("Shrewsbury A2-1K",850) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 200),
                //    new MenuItemExtra("Suppressor", 250) } },
                //new MenuItem("Vom Feuer A5-1R",950) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 125),
                //    new MenuItemExtra("Box Magazine", 250),
                //    new MenuItemExtra("Suppressor", 390),} },
                new MenuItem("Vom Feuer A5-1R MK2",1250) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 145),
                    new MenuItemExtra("Suppressor", 350),} },
                //new MenuItem("Shrewsbury Stinkov",650) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 125),
                //    new MenuItemExtra("Drum Magazine", 250),} },
                ////LMG
                //new MenuItem("Shrewsbury PDA",1500) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 200),} },
                //new MenuItem("Vom Feuer BAT",1700) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 150),} },
                //new MenuItem("Vom Feuer M70E1",2200) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 250),} },
                ////SNIPER
                //new MenuItem("Shrewsbury PWN",2200) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Suppressor", 340)} },
                ////OTHER
                //new MenuItem("M61 Grenade",500) { SubAmount = 1,SubPrice = 500 },
                //new MenuItem("Improvised Incendiary",60) { SubAmount = 1,SubPrice = 60 },
                //new MenuItem("BZ Gas Grenade",250) { SubAmount = 1,SubPrice = 250 },
                    }),
        });
    }
    private void GunShopList()
    {
        GunShop1();
        GunShop2();
        GunShop3();
        GunShop4();
        GunShop5();
    }
    private void GunShop1()//general
    {
        PossibleShopMenus.ShopMenuList.AddRange(new List<ShopMenu>
        {
                new ShopMenu("GunShop1","GunShop1",new List<MenuItem>() {
                    new MenuItem("G.E.S. Baseball Bat",70),
                    new MenuItem("Flint Crowbar",35),
                    new MenuItem("ProLaps Five Iron Golf Club",150),
                    new MenuItem("Flint Hammer",25),

                //ArmorHealth
                new MenuItem("Light Body Armor",650),
                new MenuItem("Medium Body Armor",1250),
                new MenuItem("Heavy Body Armor",1500),
                new MenuItem("Full Body Armor",2000),
                new MenuItem("Health Pack",1550),

                //Pistola
                new MenuItem("Hawk & Little PTF092F",550,450) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip",0),
                    new MenuItemExtra("Extended Clip", 75),
                    new MenuItemExtra("Flashlight", 70),
                    new MenuItemExtra("Suppressor", 850) },  },
                
                new MenuItem("Hawk & Little Raging Mare Dx",1950,1245) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Rounds", 0),
                    new MenuItemExtra("Tracer Rounds", 500),
                    new MenuItemExtra("Incendiary Rounds", 500),
                    new MenuItemExtra("Hollow Point Rounds", 500),
                    new MenuItemExtra("Full Metal Jacket Rounds", 500),
                    new MenuItemExtra("Holographic Sight", 890),
                    new MenuItemExtra("Small Scope", 570),
                    new MenuItemExtra("Flashlight", 70), } },
                new MenuItem("Vom Feuer P69",790,656) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 85),
                    new MenuItemExtra("Suppressor", 890), } },
                new MenuItem("Shrewsbury S7A",1200, 950) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 68),
                    new MenuItemExtra("Tracer Rounds", 500),
                    new MenuItemExtra("Incendiary Rounds", 500),
                    new MenuItemExtra("Hollow Point Rounds", 500),
                    new MenuItemExtra("Full Metal Jacket Rounds", 500),
                    new MenuItemExtra("Flashlight", 75),
                    new MenuItemExtra("Mounted Scope", 890),
                    new MenuItemExtra("Suppressor", 780),
                    new MenuItemExtra("Compensator", 240), } },
                new MenuItem("Coil Tesla",550),
                new MenuItem("BS M1922",995, 750) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 85),
                    new MenuItemExtra("Suppressor", 890), } },
                new MenuItem("Vom Feuer Gruber",705, 400) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Suppressor", 950), } },


                //Shotgun
                new MenuItem("Shrewsbury 420 Sawed-Off",250,150),
                new MenuItem("Shrewsbury 420",400, 325) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Flashlight", 95),
                    new MenuItemExtra("Suppressor", 1100), } },
     
                //SMG
                new MenuItem("Shrewsbury Luzi",600,450) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120),
                    new MenuItemExtra("Flashlight", 80),
                    new MenuItemExtra("Suppressor", 1945),
                    new MenuItemExtra("Scope", 556) } },
                new MenuItem("Hawk & Little Millipede",450,320) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120), } },

                //AR
                new MenuItem("Shrewsbury A7-4K",650,550) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 125),
                    new MenuItemExtra("Drum Magazine", 225),
                    new MenuItemExtra("Flashlight", 90),
                    new MenuItemExtra("Scope", 567),
                    new MenuItemExtra("Suppressor", 800),
                    new MenuItemExtra("Grip", 200), } },
                new MenuItem("Vom Feuer SL6 MK2",2200,1600) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 145),
                    new MenuItemExtra("Tracer Rounds", 500),
                    new MenuItemExtra("Incendiary Rounds", 500),
                    new MenuItemExtra("Armor Piercing Rounds", 500),
                    new MenuItemExtra("Full Metal Jacket Rounds", 500),
                    new MenuItemExtra("Grip", 120),
                    new MenuItemExtra("Flashlight", 90),
                    new MenuItemExtra("Holographic Sight", 870),
                    new MenuItemExtra("Small Scope", 760),
                    new MenuItemExtra("Large Scope", 900),
                    new MenuItemExtra("Suppressor", 1200),
                    new MenuItemExtra("Flat Muzzle Brake", 250),
                    new MenuItemExtra("Tactical Muzzle Brake", 220),
                    new MenuItemExtra("Fat-End Muzzle Brake", 215),
                    new MenuItemExtra("Precision Muzzle Brake", 220),
                    new MenuItemExtra("Heavy Duty Muzzle Brake", 235),
                    new MenuItemExtra("Slanted Muzzle Brake", 230),
                    new MenuItemExtra("Split-End Muzzle Brake", 225),
                    new MenuItemExtra("Default Barrel", 0),
                    new MenuItemExtra("Heavy Barrel", 670), } },
                new MenuItem("Hawk & Little ZBZ-23",1200, 890) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 125),
                    new MenuItemExtra("Drum Magazine", 225),
                    new MenuItemExtra("Flashlight", 90),
                    new MenuItemExtra("Scope", 567),
                    new MenuItemExtra("Suppressor", 800),
                    new MenuItemExtra("Grip", 200), } },
                new MenuItem("Vom Feuer POCK",1700,1400),

                //LMG
                new MenuItem("Vom Feuer M70E1",5000, 3400) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 145),
                    new MenuItemExtra("Tracer Rounds", 500),
                    new MenuItemExtra("Incendiary Rounds", 500),
                    new MenuItemExtra("Armor Piercing Rounds", 500),
                    new MenuItemExtra("Full Metal Jacket Rounds", 500),
                    new MenuItemExtra("Grip", 124),
                    new MenuItemExtra("Holographic Sight", 556),
                    new MenuItemExtra("Medium Scope", 760),
                    new MenuItemExtra("Large Scope", 890),
                    new MenuItemExtra("Flat Muzzle Brake", 120),
                    new MenuItemExtra("Tactical Muzzle Brake", 125),
                    new MenuItemExtra("Fat-End Muzzle Brake", 130),
                    new MenuItemExtra("Precision Muzzle Brake", 135),
                    new MenuItemExtra("Heavy Duty Muzzle Brake", 145),
                    new MenuItemExtra("Slanted Muzzle Brake", 155),
                    new MenuItemExtra("Split-End Muzzle Brake", 155),
                    new MenuItemExtra("Default Barrel", 0),
                    new MenuItemExtra("Heavy Barrel", 900),} },

                //SNIPER
                new MenuItem("Shrewsbury PWN",2500, 1200) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Scope", 890),
                    new MenuItemExtra("Advanced Scope", 1400),
                    new MenuItemExtra("Suppressor", 1900),} },


                //new MenuItem("Shrewsbury BFD Dragmeout",1500, 1000) { Extras = new List<MenuItemExtra>() {
                //    new MenuItemExtra("Default Clip", 0),
                //    new MenuItemExtra("Extended Clip", 60),
                //    new MenuItemExtra("Scope", 500),
                //    new MenuItemExtra("Flashlight", 600),
                //    new MenuItemExtra("Suppressor", 700),} },


                //OTHER
                new MenuItem("RPG-7",12550){ SubAmount = 1,SubPrice = 500 },
                new MenuItem("M61 Grenade",800) { SubAmount = 1,SubPrice = 800 },
                new MenuItem("Improvised Incendiary",150) { SubAmount = 1,SubPrice = 150 },
                new MenuItem("BZ Gas Grenade",250) { SubAmount = 1,SubPrice = 150 },
               // new MenuItem("Tear Gas Grenade",125) { AmmoAmount = 1,AmmoPrice = 125 },


                new MenuItem("Schmidt & Priss TL6 Scanner", 85),
                new MenuItem("Schmidt & Priss RD4 Radar Detector",45),

                    }),
        });
    }
    private void GunShop2()//Pistol
    {
        PossibleShopMenus.ShopMenuList.AddRange(new List<ShopMenu>
        {
                new ShopMenu("GunShop2","GunShop2",new List<MenuItem>() {

                    new MenuItem("Flint Hammer",25),
                    new MenuItem("Flint Hatchet",80),
                    new MenuItem("Vom Feuer Machete",29),
                    new MenuItem("Switchblade",78),
                    new MenuItem("Nightstick",57),

                //ArmorHealth
                new MenuItem("Light Body Armor",650),
                new MenuItem("Medium Body Armor",1250),
                new MenuItem("Heavy Body Armor",1500),
                new MenuItem("Full Body Armor",2000),
                new MenuItem("Health Pack",1550),
                //Pistola
                new MenuItem("Hawk & Little PTF092F",550,450) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip",0),
                    new MenuItemExtra("Extended Clip", 75),
                    new MenuItemExtra("Flashlight", 70),
                    new MenuItemExtra("Suppressor", 850) },  },
                new MenuItem("Hawk & Little Thunder",650,550) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 105),
                    new MenuItemExtra("Tracer Rounds", 500),
                    new MenuItemExtra("Incendiary Rounds", 500),
                    new MenuItemExtra("Hollow Point Rounds", 500),
                    new MenuItemExtra("Full Metal Jacket Rounds",500),
                    new MenuItemExtra("Mounted Scope", 1200),
                    new MenuItemExtra("Flashlight", 75),
                    new MenuItemExtra("Suppressor", 699),
                    new MenuItemExtra("Compensator", 240), } },
                new MenuItem("Hawk & Little Combat Pistol",950,750) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 85),
                    new MenuItemExtra("Flashlight", 80),
                    new MenuItemExtra("Suppressor", 890), } },
                new MenuItem("Hawk & Little Desert Slug",1500,1200) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120),
                    new MenuItemExtra("Flashlight", 95),
                    new MenuItemExtra("Suppressor", 1100), } },
                new MenuItem("Hawk & Little 1919",1200,900) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 50),
                    new MenuItemExtra("Flashlight", 75),
                    new MenuItemExtra("Suppressor", 950), } },
                new MenuItem("Hawk & Little Raging Mare",1700,1250),
                new MenuItem("Hawk & Little Raging Mare Dx",1950, 1175) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Rounds", 0),
                    new MenuItemExtra("Tracer Rounds", 500),
                    new MenuItemExtra("Incendiary Rounds", 500),
                    new MenuItemExtra("Hollow Point Rounds", 500),
                    new MenuItemExtra("Full Metal Jacket Rounds", 500),
                    new MenuItemExtra("Holographic Sight", 890),
                    new MenuItemExtra("Small Scope", 570),
                    new MenuItemExtra("Flashlight", 70), } },
                new MenuItem("Vom Feuer P69",790, 500) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 85),
                    new MenuItemExtra("Suppressor", 890), } },
                new MenuItem("Vom Feuer SCRAMP",990, 450) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 85),
                    new MenuItemExtra("Flashlight", 80),
                    new MenuItemExtra("Suppressor", 890), } },
                new MenuItem("Shrewsbury S7",1100, 780) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 85), } },
                new MenuItem("Shrewsbury S7A",1200) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 68),
                    new MenuItemExtra("Tracer Rounds", 500),
                    new MenuItemExtra("Incendiary Rounds", 500),
                    new MenuItemExtra("Hollow Point Rounds", 500),
                    new MenuItemExtra("Full Metal Jacket Rounds", 500),
                    new MenuItemExtra("Flashlight", 75),
                    new MenuItemExtra("Mounted Scope", 890),
                    new MenuItemExtra("Suppressor", 780),
                    new MenuItemExtra("Compensator", 240), } },
                new MenuItem("Coil Tesla",550, 400),
                new MenuItem("BS M1922",995, 670) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 85),
                    new MenuItemExtra("Suppressor", 890), } },


                new MenuItem("Vom Feuer Gruber",675, 454) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Suppressor", 800), } },

                

                //Shotgun
                new MenuItem("Shrewsbury Taiga-12",670, 450) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120),
                    new MenuItemExtra("Drum Magazine", 550),
                    new MenuItemExtra("Flashlight", 80),
                    new MenuItemExtra("Suppressor", 1945),
                    new MenuItemExtra("Grip", 224) } },
                new MenuItem("Toto 12 Guage Sawed-Off",350, 200),
                new MenuItem("Shrewsbury Defender",990,600),
                new MenuItem("Leotardo SPAZ-11",2300, 1800) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Flashlight", 95),
                    new MenuItemExtra("Suppressor", 1100), } },
     
                //SMG
                new MenuItem("Shrewsbury Luzi",600, 400) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120),
                    new MenuItemExtra("Flashlight", 80),
                    new MenuItemExtra("Suppressor", 1945),
                    new MenuItemExtra("Scope", 556) } },
                new MenuItem("Vom Feuer KEK-9",250, 200) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120),
                    new MenuItemExtra("Drum Magazine", 550),
                    new MenuItemExtra("Suppressor", 1945) } },
                new MenuItem("Hawk & Little Millipede",450, 300) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120), } },

                //AR
                new MenuItem("Shrewsbury A7-4K",650, 440) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 125),
                    new MenuItemExtra("Drum Magazine", 225),
                    new MenuItemExtra("Flashlight", 90),
                    new MenuItemExtra("Scope", 567),
                    new MenuItemExtra("Suppressor", 800),
                    new MenuItemExtra("Grip", 200), } },
                new MenuItem("Shrewsbury A2-1K",790, 560) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 150),
                    new MenuItemExtra("Tracer Rounds", 500),
                    new MenuItemExtra("Incendiary Rounds", 500),
                    new MenuItemExtra("Armor Piercing Rounds", 500),
                    new MenuItemExtra("Full Metal Jacket Rounds", 500),
                    new MenuItemExtra("Grip", 120),
                    new MenuItemExtra("Flashlight", 90),
                    new MenuItemExtra("Holographic Sight", 780),
                    new MenuItemExtra("Small Scope", 667),
                    new MenuItemExtra("Large Scope", 989),
                    new MenuItemExtra("Suppressor", 1200),
                    new MenuItemExtra("Flat Muzzle Brake", 300),
                    new MenuItemExtra("Tactical Muzzle Brake", 123),
                    new MenuItemExtra("Fat-End Muzzle Brake", 150),
                    new MenuItemExtra("Precision Muzzle Brake", 224),
                    new MenuItemExtra("Heavy Duty Muzzle Brake", 330),
                    new MenuItemExtra("Slanted Muzzle Brake", 150),
                    new MenuItemExtra("Split-End Muzzle Brake", 175),
                    new MenuItemExtra("Default Barrel", 0),
                    new MenuItemExtra("Heavy Barrel", 800), } },

                //LMG
                new MenuItem("Shrewsbury PDA",4500, 3000) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 125),
                    new MenuItemExtra("Scope", 567),} },

                //SNIPER
                new MenuItem("Bartlett M92",5790, 2500) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Scope", 890),
                    new MenuItemExtra("Advanced Scope", 1400),} },
                new MenuItem("Vom Feuer M23 DBS Scout",1600, 1200) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 550),
                    new MenuItemExtra("Tracer Rounds",560),
                    new MenuItemExtra("Incendiary Rounds", 1000),
                    new MenuItemExtra("Armor Piercing Rounds", 1700),
                    new MenuItemExtra("Full Metal Jacket Rounds", 550),
                    new MenuItemExtra("Holographic Sight", 670),
                    new MenuItemExtra("Large Scope", 890),
                    new MenuItemExtra("Zoom Scope", 1200),
                    new MenuItemExtra("Flashlight", 78),
                    new MenuItemExtra("Suppressor", 1400),
                    new MenuItemExtra("Flat Muzzle Brake", 123),
                    new MenuItemExtra("Tactical Muzzle Brake", 125),
                    new MenuItemExtra("Fat-End Muzzle Brake", 150),
                    new MenuItemExtra("Precision Muzzle Brake", 125),
                    new MenuItemExtra("Heavy Duty Muzzle Brake", 134),
                    new MenuItemExtra("Slanted Muzzle Brake", 145),
                    new MenuItemExtra("Split-End Muzzle Brake", 134),
                    new MenuItemExtra("Default Barrel", 0),
                    new MenuItemExtra("Heavy Barrel", 1200),
                    new MenuItemExtra("Grip", 130),} },

                //OTHER
                new MenuItem("M61 Grenade",800) { SubAmount = 1,SubPrice = 800 },
                new MenuItem("Improvised Incendiary",150) { SubAmount = 1,SubPrice = 150 },
                new MenuItem("BZ Gas Grenade",250) { SubAmount = 1,SubPrice = 150 },

                new MenuItem("Schmidt & Priss TL6 Scanner", 85),
                new MenuItem("Schmidt & Priss RD4 Radar Detector",45),
               // new MenuItem("Tear Gas Grenade",125) { AmmoAmount = 1,AmmoPrice = 125 },
                    }),
        });
    }
    private void GunShop3()//SMG
    {
        PossibleShopMenus.ShopMenuList.AddRange(new List<ShopMenu>
        {
                new ShopMenu("GunShop3","GunShop3",new List<MenuItem>() {
                    new MenuItem("Combat Knife",120),
                    new MenuItem("Vom Feuer Machete",29),
                    new MenuItem("Switchblade",78),
                    new MenuItem("Nightstick",57),
                    new MenuItem("Flint Heavy Duty Pipe Wrench",24),
                    new MenuItem("Pool Cue",45),

                                    //ArmorHealth
                new MenuItem("Light Body Armor",650),
                new MenuItem("Medium Body Armor",1250),
                new MenuItem("Heavy Body Armor",1500),
                new MenuItem("Full Body Armor",2000),
                new MenuItem("Health Pack",1550),

                //Pistola
                new MenuItem("Hawk & Little Combat Pistol",950, 700) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 85),
                    new MenuItemExtra("Flashlight", 80),
                    new MenuItemExtra("Suppressor", 890), } },
                new MenuItem("Hawk & Little Desert Slug",1500, 1150) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120),
                    new MenuItemExtra("Flashlight", 95),
                    new MenuItemExtra("Suppressor", 1100), } },
                new MenuItem("Hawk & Little 1919",1200, 990) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 50),
                    new MenuItemExtra("Flashlight", 75),
                    new MenuItemExtra("Suppressor", 950), } },
                new MenuItem("Hawk & Little Raging Mare",1700, 1200),
                new MenuItem("Hawk & Little Raging Mare Dx",1950, 1300) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Rounds", 0),
                    new MenuItemExtra("Tracer Rounds", 500),
                    new MenuItemExtra("Incendiary Rounds", 500),
                    new MenuItemExtra("Hollow Point Rounds", 500),
                    new MenuItemExtra("Full Metal Jacket Rounds", 500),
                    new MenuItemExtra("Holographic Sight", 890),
                    new MenuItemExtra("Small Scope", 570),
                    new MenuItemExtra("Flashlight", 70), } },
                new MenuItem("Vom Feuer P69",790,599) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 85),
                    new MenuItemExtra("Suppressor", 890), } },
                new MenuItem("BS M1922",995, 656) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 85),
                    new MenuItemExtra("Suppressor", 890), } },


                //Shotgun
                new MenuItem("Shrewsbury 420 Sawed-Off",250, 200),
                new MenuItem("Shrewsbury Taiga-12",670) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120),
                    new MenuItemExtra("Drum Magazine", 550),
                    new MenuItemExtra("Flashlight", 80),
                    new MenuItemExtra("Suppressor", 1945),
                    new MenuItemExtra("Grip", 224) } },
                new MenuItem("Toto 12 Guage Sawed-Off",350, 225),
                new MenuItem("Shrewsbury Defender",990, 650),
                new MenuItem("Leotardo SPAZ-11",2300, 1500) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Flashlight", 95),
                    new MenuItemExtra("Suppressor", 1100), } },
     
                //SMG
                new MenuItem("Shrewsbury Luzi",600, 500) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120),
                    new MenuItemExtra("Flashlight", 80),
                    new MenuItemExtra("Suppressor", 1945),
                    new MenuItemExtra("Scope", 556) } },
                new MenuItem("Hawk & Little MP6",1500, 1200) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120),
                    new MenuItemExtra("Drum Magazine", 550),
                    new MenuItemExtra("Flashlight", 80),
                    new MenuItemExtra("Suppressor", 1945) } },
                new MenuItem("Hawk & Little XPM",1600, 1300) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120),
                    new MenuItemExtra("Tracer Rounds", 500),
                    new MenuItemExtra("Incendiary Rounds", 500),
                    new MenuItemExtra("Hollow Point Rounds", 500),
                    new MenuItemExtra("Full Metal Jacket Rounds", 500),
                    new MenuItemExtra("Flashlight", 120),
                    new MenuItemExtra("Holographic Sight", 760),
                    new MenuItemExtra("Small Scope", 525),
                    new MenuItemExtra("Medium Scope", 890),
                    new MenuItemExtra("Suppressor", 1400),
                    new MenuItemExtra("Flat Muzzle Brake", 255),
                    new MenuItemExtra("Tactical Muzzle Brake",265),
                    new MenuItemExtra("Fat-End Muzzle Brake", 200),
                    new MenuItemExtra("Precision Muzzle Brake", 276),
                    new MenuItemExtra("Heavy Duty Muzzle Brake", 345),
                    new MenuItemExtra("Slanted Muzzle Brake", 205),
                    new MenuItemExtra("Split-End Muzzle Brake", 200),
                    new MenuItemExtra("Default Barrel", 0),
                    new MenuItemExtra("Heavy Barrel", 500), } },
                new MenuItem("Vom Feuer Fisher",900, 650) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120),
                    new MenuItemExtra("Scope", 550),
                    new MenuItemExtra("Flashlight", 80),
                    new MenuItemExtra("Suppressor", 1945) } },
                new MenuItem("Coil PXM",1200, 900) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120),
                    new MenuItemExtra("Drum Magazine", 550),
                    new MenuItemExtra("Flashlight", 80),
                    new MenuItemExtra("Scope", 567),
                    new MenuItemExtra("Grip", 224) } },
                new MenuItem("Vom Feuer KEK-9",250, 150) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120),
                    new MenuItemExtra("Drum Magazine", 550),
                    new MenuItemExtra("Suppressor", 1945) } },

                new MenuItem("Vom Feuer PMP",600, 250) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 200),
                    new MenuItemExtra("Scope", 550),
                    new MenuItemExtra("Suppressor", 700) } },

                //"Vom Feuer PMP"



                new MenuItem("Hawk & Little Millipede",450, 250) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120), } },

                //AR
                new MenuItem("Vom Feuer A5-1R",700, 500) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 125),
                    new MenuItemExtra("Box Magazine", 225),
                    new MenuItemExtra("Flashlight", 90),
                    new MenuItemExtra("Scope", 567),
                    new MenuItemExtra("Suppressor", 800),
                    new MenuItemExtra("Grip", 200), } },
                new MenuItem("Shrewsbury Stinkov",750, 600) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 125),
                    new MenuItemExtra("Drum Magazine", 225),} },


                //OTHER
                new MenuItem("M61 Grenade",800) { SubAmount = 1,SubPrice = 800 },
                new MenuItem("Improvised Incendiary",150) { SubAmount = 1,SubPrice = 150 },
                new MenuItem("BZ Gas Grenade",250) { SubAmount = 1,SubPrice = 150 },

                new MenuItem("Schmidt & Priss TL6 Scanner", 85),
                new MenuItem("Schmidt & Priss RD4 Radar Detector",45),
               // new MenuItem("Tear Gas Grenade",125) { AmmoAmount = 1,AmmoPrice = 125 },
                    }),
        });
    }
    private void GunShop4()//AR
    {
        PossibleShopMenus.ShopMenuList.AddRange(new List<ShopMenu>
        {
                new ShopMenu("GunShop4","GunShop4",new List<MenuItem>() {
                                    //ArmorHealth
                new MenuItem("Light Body Armor",650),
                new MenuItem("Medium Body Armor",1250),
                new MenuItem("Heavy Body Armor",1500),
                new MenuItem("Full Body Armor",2000),
                new MenuItem("Health Pack",1550),

                //Pistola
                
                new MenuItem("Hawk & Little Combat Pistol",950, 650) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 85),
                    new MenuItemExtra("Flashlight", 80),
                    new MenuItemExtra("Suppressor", 890), } },
                new MenuItem("Hawk & Little Desert Slug",1500, 1200) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120),
                    new MenuItemExtra("Flashlight", 95),
                    new MenuItemExtra("Suppressor", 1100), } },

                //Shotgun
                new MenuItem("Shrewsbury 420 Sawed-Off",250, 200),
                new MenuItem("Shrewsbury 420",400) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Flashlight", 95),
                    new MenuItemExtra("Suppressor", 1100), } },
                new MenuItem("Shrewsbury Taiga-12",670, 550) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120),
                    new MenuItemExtra("Drum Magazine", 550),
                    new MenuItemExtra("Flashlight", 80),
                    new MenuItemExtra("Suppressor", 1945),
                    new MenuItemExtra("Grip", 224) } },
                new MenuItem("Toto 12 Guage Sawed-Off",350, 250),
                new MenuItem("Shrewsbury Defender",990, 700),
                new MenuItem("Leotardo SPAZ-11",2300, 1900) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Flashlight", 95),
                    new MenuItemExtra("Suppressor", 1100), } },
     
                //SMG
                new MenuItem("Shrewsbury Luzi",600, 450) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120),
                    new MenuItemExtra("Flashlight", 80),
                    new MenuItemExtra("Suppressor", 1945),
                    new MenuItemExtra("Scope", 556) } },
                new MenuItem("Vom Feuer Fisher",900, 750) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120),
                    new MenuItemExtra("Scope", 550),
                    new MenuItemExtra("Flashlight", 80),
                    new MenuItemExtra("Suppressor", 1945) } },

                //AR
                new MenuItem("Shrewsbury A7-4K",650, 550) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 125),
                    new MenuItemExtra("Drum Magazine", 225),
                    new MenuItemExtra("Flashlight", 90),
                    new MenuItemExtra("Scope", 567),
                    new MenuItemExtra("Suppressor", 800),
                    new MenuItemExtra("Grip", 200), } },
                new MenuItem("Shrewsbury A2-1K",790, 570) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 150),
                    new MenuItemExtra("Tracer Rounds", 500),
                    new MenuItemExtra("Incendiary Rounds", 500),
                    new MenuItemExtra("Armor Piercing Rounds", 500),
                    new MenuItemExtra("Full Metal Jacket Rounds", 500),
                    new MenuItemExtra("Grip", 120),
                    new MenuItemExtra("Flashlight", 90),
                    new MenuItemExtra("Holographic Sight", 780),
                    new MenuItemExtra("Small Scope", 667),
                    new MenuItemExtra("Large Scope", 989),
                    new MenuItemExtra("Suppressor", 1200),
                    new MenuItemExtra("Flat Muzzle Brake", 300),
                    new MenuItemExtra("Tactical Muzzle Brake", 123),
                    new MenuItemExtra("Fat-End Muzzle Brake", 150),
                    new MenuItemExtra("Precision Muzzle Brake", 224),
                    new MenuItemExtra("Heavy Duty Muzzle Brake", 330),
                    new MenuItemExtra("Slanted Muzzle Brake", 150),
                    new MenuItemExtra("Split-End Muzzle Brake", 175),
                    new MenuItemExtra("Default Barrel", 0),
                    new MenuItemExtra("Heavy Barrel", 800), } },
                new MenuItem("Vom Feuer A5-1R",700, 600) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 125),
                    new MenuItemExtra("Box Magazine", 225),
                    new MenuItemExtra("Flashlight", 90),
                    new MenuItemExtra("Scope", 567),
                    new MenuItemExtra("Suppressor", 800),
                    new MenuItemExtra("Grip", 200), } },
                new MenuItem("Vom Feuer A5-1R MK2",950, 750) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 145),
                    new MenuItemExtra("Tracer Rounds", 500),
                    new MenuItemExtra("Incendiary Rounds", 500),
                    new MenuItemExtra("Armor Piercing Rounds", 500),
                    new MenuItemExtra("Full Metal Jacket Rounds", 500),
                    new MenuItemExtra("Grip", 120),
                    new MenuItemExtra("Flashlight", 90),
                    new MenuItemExtra("Holographic Sight", 870),
                    new MenuItemExtra("Small Scope", 760),
                    new MenuItemExtra("Large Scope", 900),
                    new MenuItemExtra("Suppressor", 1200),
                    new MenuItemExtra("Flat Muzzle Brake", 250),
                    new MenuItemExtra("Tactical Muzzle Brake", 220),
                    new MenuItemExtra("Fat-End Muzzle Brake", 215),
                    new MenuItemExtra("Precision Muzzle Brake", 220),
                    new MenuItemExtra("Heavy Duty Muzzle Brake", 235),
                    new MenuItemExtra("Slanted Muzzle Brake", 230),
                    new MenuItemExtra("Split-End Muzzle Brake", 225),
                    new MenuItemExtra("Default Barrel", 0),
                    new MenuItemExtra("Heavy Barrel", 670), } },
                new MenuItem("Vom Feuer BFR",1200, 860) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 125),
                    new MenuItemExtra("Flashlight", 90),
                    new MenuItemExtra("Scope", 567),
                    new MenuItemExtra("Suppressor", 800), } },
                new MenuItem("Vom Feuer SL6",1900, 1200) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 125),
                    new MenuItemExtra("Drum Magazine", 225),
                    new MenuItemExtra("Flashlight", 90),
                    new MenuItemExtra("Scope", 567),
                    new MenuItemExtra("Suppressor", 800),
                    new MenuItemExtra("Grip", 200), } },
                new MenuItem("Vom Feuer SL6 MK2",2200, 1800) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 145),
                    new MenuItemExtra("Tracer Rounds", 500),
                    new MenuItemExtra("Incendiary Rounds", 500),
                    new MenuItemExtra("Armor Piercing Rounds", 500),
                    new MenuItemExtra("Full Metal Jacket Rounds", 500),
                    new MenuItemExtra("Grip", 120),
                    new MenuItemExtra("Flashlight", 90),
                    new MenuItemExtra("Holographic Sight", 870),
                    new MenuItemExtra("Small Scope", 760),
                    new MenuItemExtra("Large Scope", 900),
                    new MenuItemExtra("Suppressor", 1200),
                    new MenuItemExtra("Flat Muzzle Brake", 250),
                    new MenuItemExtra("Tactical Muzzle Brake", 220),
                    new MenuItemExtra("Fat-End Muzzle Brake", 215),
                    new MenuItemExtra("Precision Muzzle Brake", 220),
                    new MenuItemExtra("Heavy Duty Muzzle Brake", 235),
                    new MenuItemExtra("Slanted Muzzle Brake", 230),
                    new MenuItemExtra("Split-End Muzzle Brake", 225),
                    new MenuItemExtra("Default Barrel", 0),
                    new MenuItemExtra("Heavy Barrel", 670), } },
                new MenuItem("Hawk & Little ZBZ-23",1200, 950) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 125),
                    new MenuItemExtra("Drum Magazine", 225),
                    new MenuItemExtra("Flashlight", 90),
                    new MenuItemExtra("Scope", 567),
                    new MenuItemExtra("Suppressor", 800),
                    new MenuItemExtra("Grip", 200), } },
                new MenuItem("Hawk & Little ZBZ-25X",1300, 1100) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 145),
                    new MenuItemExtra("Tracer Rounds", 500),
                    new MenuItemExtra("Incendiary Rounds", 500),
                    new MenuItemExtra("Armor Piercing Rounds", 500),
                    new MenuItemExtra("Full Metal Jacket Rounds", 500),
                    new MenuItemExtra("Grip", 120),
                    new MenuItemExtra("Flashlight", 90),
                    new MenuItemExtra("Holographic Sight", 870),
                    new MenuItemExtra("Small Scope", 760),
                    new MenuItemExtra("Large Scope", 900),
                    new MenuItemExtra("Suppressor", 1200),
                    new MenuItemExtra("Flat Muzzle Brake", 250),
                    new MenuItemExtra("Tactical Muzzle Brake", 220),
                    new MenuItemExtra("Fat-End Muzzle Brake", 215),
                    new MenuItemExtra("Precision Muzzle Brake", 220),
                    new MenuItemExtra("Heavy Duty Muzzle Brake", 235),
                    new MenuItemExtra("Slanted Muzzle Brake", 230),
                    new MenuItemExtra("Split-End Muzzle Brake", 225),
                    new MenuItemExtra("Default Barrel", 0),
                    new MenuItemExtra("Heavy Barrel", 670), } },
                new MenuItem("Shrewsbury Stinkov",750, 550) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 125),
                    new MenuItemExtra("Drum Magazine", 225),} },
                new MenuItem("Vom Feuer GUH-B4",1400, 950) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 125),
                    new MenuItemExtra("Iron Sights", 0),
                    new MenuItemExtra("Flashlight", 90),
                    new MenuItemExtra("Scope", 567),
                    new MenuItemExtra("Suppressor", 800),} },
                new MenuItem("Vom Feuer POCK",1700, 1200),
                new MenuItem("Vom Feuer DP1 Carbine",855, 656) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 125),
                    new MenuItemExtra("Flashlight", 90),
                    new MenuItemExtra("Grip", 567),
                    new MenuItemExtra("Suppressor", 800),} },



                new MenuItem("Vom Feuer LAR",750, 345) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 125),
                    new MenuItemExtra("Suppressor", 800),} },
                //

                //LMG
                new MenuItem("Vom Feuer BAT",4340, 3400) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 125),
                    new MenuItemExtra("Grip", 90),
                    new MenuItemExtra("Scope", 567),} },
                new MenuItem("Vom Feuer M70E1",5000, 3700) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 145),
                    new MenuItemExtra("Tracer Rounds", 500),
                    new MenuItemExtra("Incendiary Rounds", 500),
                    new MenuItemExtra("Armor Piercing Rounds", 500),
                    new MenuItemExtra("Full Metal Jacket Rounds", 500),
                    new MenuItemExtra("Grip", 124),
                    new MenuItemExtra("Holographic Sight", 556),
                    new MenuItemExtra("Medium Scope", 760),
                    new MenuItemExtra("Large Scope", 890),
                    new MenuItemExtra("Flat Muzzle Brake", 120),
                    new MenuItemExtra("Tactical Muzzle Brake", 125),
                    new MenuItemExtra("Fat-End Muzzle Brake", 130),
                    new MenuItemExtra("Precision Muzzle Brake", 135),
                    new MenuItemExtra("Heavy Duty Muzzle Brake", 145),
                    new MenuItemExtra("Slanted Muzzle Brake", 155),
                    new MenuItemExtra("Split-End Muzzle Brake", 155),
                    new MenuItemExtra("Default Barrel", 0),
                    new MenuItemExtra("Heavy Barrel", 900),} },

                //OTHER
                new MenuItem("M61 Grenade",800) { SubAmount = 1,SubPrice = 800 },
                new MenuItem("Improvised Incendiary",150) { SubAmount = 1,SubPrice = 150 },
                new MenuItem("BZ Gas Grenade",250) { SubAmount = 1,SubPrice = 150 },

                new MenuItem("Schmidt & Priss TL6 Scanner", 85),
               new MenuItem("Schmidt & Priss RD4 Radar Detector",45),
               // new MenuItem("Tear Gas Grenade",125) { AmmoAmount = 1,AmmoPrice = 125 },
                    }),
        });
    }
    private void GunShop5()//heavy?
    {
        PossibleShopMenus.ShopMenuList.AddRange(new List<ShopMenu>
        {
                new ShopMenu("GunShop5","GunShop5",new List<MenuItem>() {
                    
                                    //ArmorHealth
                new MenuItem("Light Body Armor",650),
                new MenuItem("Medium Body Armor",1250),
                new MenuItem("Heavy Body Armor",1500),
                new MenuItem("Full Body Armor",2000),
                new MenuItem("Health Pack",1550),
                //Pistola
                new MenuItem("Vom Feuer SCRAMP",990, 770) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 85),
                    new MenuItemExtra("Flashlight", 80),
                    new MenuItemExtra("Suppressor", 890), } },
                new MenuItem("Shrewsbury S7A",1200,890) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 68),
                    new MenuItemExtra("Tracer Rounds", 500),
                    new MenuItemExtra("Incendiary Rounds", 500),
                    new MenuItemExtra("Hollow Point Rounds", 500),
                    new MenuItemExtra("Full Metal Jacket Rounds", 500),
                    new MenuItemExtra("Flashlight", 75),
                    new MenuItemExtra("Mounted Scope", 890),
                    new MenuItemExtra("Suppressor", 780),
                    new MenuItemExtra("Compensator", 240), } },
                new MenuItem("Coil Tesla",550),


                //Shotgun
                new MenuItem("Vom Feuer IBS-12",670) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 85),
                    new MenuItemExtra("Flashlight", 95),
                    new MenuItemExtra("Suppressor", 1100), } },
                new MenuItem("Hawk & Little HLSG",780) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Flashlight", 95),
                    new MenuItemExtra("Suppressor", 1950),
                    new MenuItemExtra("Grip", 120),} },
                new MenuItem("Shrewsbury Taiga-12",670) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120),
                    new MenuItemExtra("Drum Magazine", 550),
                    new MenuItemExtra("Flashlight", 80),
                    new MenuItemExtra("Suppressor", 1945),
                    new MenuItemExtra("Grip", 224) } },
                new MenuItem("Leotardo SPAZ-11",2300) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Flashlight", 95),
                    new MenuItemExtra("Suppressor", 1100), } },
     
                //SMG
                new MenuItem("Shrewsbury Luzi",600) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120),
                    new MenuItemExtra("Flashlight", 80),
                    new MenuItemExtra("Suppressor", 1945),
                    new MenuItemExtra("Scope", 556) } },
                new MenuItem("Coil PXM",1200) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120),
                    new MenuItemExtra("Drum Magazine", 550),
                    new MenuItemExtra("Flashlight", 80),
                    new MenuItemExtra("Scope", 567),
                    new MenuItemExtra("Grip", 224) } },
                new MenuItem("Vom Feuer KEK-9",250) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120),
                    new MenuItemExtra("Drum Magazine", 550),
                    new MenuItemExtra("Suppressor", 1945) } },
                new MenuItem("Hawk & Little Millipede",450) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120), } },

                //AR
                new MenuItem("Shrewsbury A2-1K",790) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 150),
                    new MenuItemExtra("Tracer Rounds", 500),
                    new MenuItemExtra("Incendiary Rounds", 500),
                    new MenuItemExtra("Armor Piercing Rounds", 500),
                    new MenuItemExtra("Full Metal Jacket Rounds", 500),
                    new MenuItemExtra("Grip", 120),
                    new MenuItemExtra("Flashlight", 90),
                    new MenuItemExtra("Holographic Sight", 780),
                    new MenuItemExtra("Small Scope", 667),
                    new MenuItemExtra("Large Scope", 989),
                    new MenuItemExtra("Suppressor", 1200),
                    new MenuItemExtra("Flat Muzzle Brake", 300),
                    new MenuItemExtra("Tactical Muzzle Brake", 123),
                    new MenuItemExtra("Fat-End Muzzle Brake", 150),
                    new MenuItemExtra("Precision Muzzle Brake", 224),
                    new MenuItemExtra("Heavy Duty Muzzle Brake", 330),
                    new MenuItemExtra("Slanted Muzzle Brake", 150),
                    new MenuItemExtra("Split-End Muzzle Brake", 175),
                    new MenuItemExtra("Default Barrel", 0),
                    new MenuItemExtra("Heavy Barrel", 800), } },
                new MenuItem("Vom Feuer A5-1R MK2",950) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 145),
                    new MenuItemExtra("Tracer Rounds", 500),
                    new MenuItemExtra("Incendiary Rounds", 500),
                    new MenuItemExtra("Armor Piercing Rounds", 500),
                    new MenuItemExtra("Full Metal Jacket Rounds", 500),
                    new MenuItemExtra("Grip", 120),
                    new MenuItemExtra("Flashlight", 90),
                    new MenuItemExtra("Holographic Sight", 870),
                    new MenuItemExtra("Small Scope", 760),
                    new MenuItemExtra("Large Scope", 900),
                    new MenuItemExtra("Suppressor", 1200),
                    new MenuItemExtra("Flat Muzzle Brake", 250),
                    new MenuItemExtra("Tactical Muzzle Brake", 220),
                    new MenuItemExtra("Fat-End Muzzle Brake", 215),
                    new MenuItemExtra("Precision Muzzle Brake", 220),
                    new MenuItemExtra("Heavy Duty Muzzle Brake", 235),
                    new MenuItemExtra("Slanted Muzzle Brake", 230),
                    new MenuItemExtra("Split-End Muzzle Brake", 225),
                    new MenuItemExtra("Default Barrel", 0),
                    new MenuItemExtra("Heavy Barrel", 670), } },
                new MenuItem("Vom Feuer BFR",1200) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 125),
                    new MenuItemExtra("Flashlight", 90),
                    new MenuItemExtra("Scope", 567),
                    new MenuItemExtra("Suppressor", 800), } },
                new MenuItem("Shrewsbury Stinkov",750) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 125),
                    new MenuItemExtra("Drum Magazine", 225),} },
                new MenuItem("Vom Feuer GUH-B4",1400) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 125),
                    new MenuItemExtra("Iron Sights", 0),
                    new MenuItemExtra("Flashlight", 90),
                    new MenuItemExtra("Scope", 567),
                    new MenuItemExtra("Suppressor", 800),} },
                new MenuItem("Vom Feuer POCK",1700),

                //LMG
                new MenuItem("Shrewsbury PDA",4500, 3800) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 125),
                    new MenuItemExtra("Scope", 567),} },
                new MenuItem("Vom Feuer BAT",4340, 4000) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 125),
                    new MenuItemExtra("Grip", 90),
                    new MenuItemExtra("Scope", 567),} },
                new MenuItem("Vom Feuer M70E1",5000, 4200) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 145),
                    new MenuItemExtra("Tracer Rounds", 500),
                    new MenuItemExtra("Incendiary Rounds", 500),
                    new MenuItemExtra("Armor Piercing Rounds", 500),
                    new MenuItemExtra("Full Metal Jacket Rounds", 500),
                    new MenuItemExtra("Grip", 124),
                    new MenuItemExtra("Holographic Sight", 556),
                    new MenuItemExtra("Medium Scope", 760),
                    new MenuItemExtra("Large Scope", 890),
                    new MenuItemExtra("Flat Muzzle Brake", 120),
                    new MenuItemExtra("Tactical Muzzle Brake", 125),
                    new MenuItemExtra("Fat-End Muzzle Brake", 130),
                    new MenuItemExtra("Precision Muzzle Brake", 135),
                    new MenuItemExtra("Heavy Duty Muzzle Brake", 145),
                    new MenuItemExtra("Slanted Muzzle Brake", 155),
                    new MenuItemExtra("Split-End Muzzle Brake", 155),
                    new MenuItemExtra("Default Barrel", 0),
                    new MenuItemExtra("Heavy Barrel", 900),} },
                new MenuItem("Hawk & Little Kenan",950) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 125),} },

                //SNIPER
                new MenuItem("Shrewsbury PWN",2500, 2000) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Scope", 890),
                    new MenuItemExtra("Advanced Scope", 1400),
                    new MenuItemExtra("Suppressor", 1900),} },
                new MenuItem("Bartlett M92",5790, 4500) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Scope", 890),
                    new MenuItemExtra("Advanced Scope", 1400),} },
                new MenuItem("Bartlett M92 Mk2",6780, 5600) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 250),
                    new MenuItemExtra("Incendiary Rounds", 1000),
                    new MenuItemExtra("Armor Piercing Rounds", 1500),
                    new MenuItemExtra("Full Metal Jacket Rounds", 500),
                    new MenuItemExtra("Explosive Rounds", 2000),
                    new MenuItemExtra("Zoom Scope", 2500),
                    new MenuItemExtra("Advanced Scope", 1500),
                    new MenuItemExtra("Night Vision Scope", 3500),
                    new MenuItemExtra("Thermal Scope", 9500),
                    new MenuItemExtra("Suppressor", 1900),
                    new MenuItemExtra("Squared Muzzle Brake", 125),
                    new MenuItemExtra("Bell-End Muzzle Brake", 150),
                    new MenuItemExtra("Default Barrel", 0),
                    new MenuItemExtra("Heavy Barrel", 1800),} },
                new MenuItem("Vom Feuer M23 DBS",1500,1145) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 125),
                    new MenuItemExtra("Flashlight", 90),
                    new MenuItemExtra("Scope", 567),
                    new MenuItemExtra("Suppressor", 800),
                    new MenuItemExtra("Grip", 200), } },
                new MenuItem("Vom Feuer M23 DBS Scout",1600, 1200) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 550),
                    new MenuItemExtra("Tracer Rounds",560),
                    new MenuItemExtra("Incendiary Rounds", 1000),
                    new MenuItemExtra("Armor Piercing Rounds", 1700),
                    new MenuItemExtra("Full Metal Jacket Rounds", 550),
                    new MenuItemExtra("Holographic Sight", 670),
                    new MenuItemExtra("Large Scope", 890),
                    new MenuItemExtra("Zoom Scope", 1200),
                    new MenuItemExtra("Flashlight", 78),
                    new MenuItemExtra("Suppressor", 1400),
                    new MenuItemExtra("Flat Muzzle Brake", 123),
                    new MenuItemExtra("Tactical Muzzle Brake", 125),
                    new MenuItemExtra("Fat-End Muzzle Brake", 150),
                    new MenuItemExtra("Precision Muzzle Brake", 125),
                    new MenuItemExtra("Heavy Duty Muzzle Brake", 134),
                    new MenuItemExtra("Slanted Muzzle Brake", 145),
                    new MenuItemExtra("Split-End Muzzle Brake", 134),
                    new MenuItemExtra("Default Barrel", 0),
                    new MenuItemExtra("Heavy Barrel", 1200),
                    new MenuItemExtra("Grip", 130),} },

                //OTHER
                new MenuItem("RPG-7",12550,10450){ SubAmount = 1,SubPrice = 500 },
                new MenuItem("Hawk & Little MGL",18889){ Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Flashlight", 90),
                    new MenuItemExtra("Scope", 567),
                    new MenuItemExtra("Grip", 200), },SubAmount = 6,SubPrice = 500 },
                new MenuItem("M61 Grenade",800) { SubAmount = 1,SubPrice = 800 },
                new MenuItem("Improvised Incendiary",150) { SubAmount = 1,SubPrice = 150 },
                new MenuItem("BZ Gas Grenade",250) { SubAmount = 1,SubPrice = 150 },

                new MenuItem("Schmidt & Priss TL6 Scanner", 85),
                new MenuItem("Schmidt & Priss RD4 Radar Detector",45),
               // new MenuItem("Tear Gas Grenade",125) { AmmoAmount = 1,AmmoPrice = 125 },
                    }),
        });
    }
    private void DealerHangouts()
    {
        PossibleShopMenus.ShopMenuList.AddRange(new List<ShopMenu> {
               new ShopMenu("DealerHangoutMenu1","DealerHangoutMenu1",new List<MenuItem>() {
                    new VariablePriceMenuItem("Marijuana", 13, 15, 11, 12) { NumberOfItemsToSellToPlayer = 15, NumberOfItemsToPurchaseFromPlayer = 10,IsIllicilt = true },
                    new VariablePriceMenuItem("Toilet Cleaner",17, 20, 14, 16) { NumberOfItemsToSellToPlayer = 15, NumberOfItemsToPurchaseFromPlayer = 10,IsIllicilt = true },
                    new MenuItem("Brass Knuckles",150) { IsIllicilt = true },
                    new MenuItem("Combat Knife",120) { IsIllicilt = true },
                    new MenuItem("Switchblade",55) { IsIllicilt = true },
                //Pistola
                new MenuItem("Hawk & Little PTF092F",445) { IsIllicilt = true, Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip",0),
                    new MenuItemExtra("Extended Clip", 75),
                    new MenuItemExtra("Suppressor", 343) },  },
                new MenuItem("Hawk & Little 1919",1134) { IsIllicilt = true, Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 50),
                    new MenuItemExtra("Suppressor", 125), } },
                new MenuItem("Vom Feuer P69",790) {IsIllicilt = true,  Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 85),
                    new MenuItemExtra("Suppressor", 125), } },
                //Shotgun
                new MenuItem("Shrewsbury 420 Sawed-Off",200) { IsIllicilt = true },
                new MenuItem("Toto 12 Guage Sawed-Off",250) { IsIllicilt = true },
                //SMG
                new MenuItem("Shrewsbury Luzi",455) { IsIllicilt = true, Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120),
                    new MenuItemExtra("Suppressor", 145), } },
                new MenuItem("Vom Feuer KEK-9",250) {IsIllicilt = true,  Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120),
                    new MenuItemExtra("Drum Magazine", 123),
                    new MenuItemExtra("Suppressor", 356) } },
                new MenuItem("Hawk & Little Millipede",450) { IsIllicilt = true, Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120), } },
                //AR
                new MenuItem("Shrewsbury Stinkov",450) { IsIllicilt = true, Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 125),
                    new MenuItemExtra("Drum Magazine", 200),} },
                //OTHER
                new MenuItem("Improvised Incendiary",30) { NumberOfItemsToSellToPlayer = 5, IsIllicilt = true, SubAmount = 1,SubPrice = 30 },
                new MenuItem("BZ Gas Grenade",100) { NumberOfItemsToSellToPlayer = 5,IsIllicilt = true, SubAmount = 1,SubPrice = 100 },
                    }),
        });
    }
    private void MenuGroupList()
    {
        ShopMenuGroupContainer PoorAreaDealerMenuGroup = new ShopMenuGroupContainer(StaticStrings.PoorAreaDrugDealerMenuGroupID, "Poor Area Dealer Menu",
        new List<PercentageSelectGroupMenuContainer>() {
                        new PercentageSelectGroupMenuContainer(StaticStrings.MethamphetamineDealerMenuGroupID, 15),
                        new PercentageSelectGroupMenuContainer(StaticStrings.ToiletCleanerDealerMenuGroupID, 15),
                        new PercentageSelectGroupMenuContainer(StaticStrings.MarijuanaDealerMenuGroupID, 15),
                        new PercentageSelectGroupMenuContainer(StaticStrings.CokeDealerMenuGroupID, 5),
                        new PercentageSelectGroupMenuContainer(StaticStrings.CrackDealerMenuGroupID, 15),
                        new PercentageSelectGroupMenuContainer(StaticStrings.HeroinDealerMenuGroupID, 5),
                        new PercentageSelectGroupMenuContainer(StaticStrings.SPANKDealerMenuGroupID, 15),
        });
        ShopMenuGroupContainer PoorAreaCustomerMenuGroup = new ShopMenuGroupContainer(StaticStrings.PoorAreaDrugCustomerMenuGroupID, "Poor Area Customer Menu",
            new List<PercentageSelectGroupMenuContainer>() {
                    new PercentageSelectGroupMenuContainer(StaticStrings.MethamphetamineCustomerMenuGroupID, 10),
                    new PercentageSelectGroupMenuContainer(StaticStrings.ToiletCleanerCustomerMenuGroupID, 10),
                    new PercentageSelectGroupMenuContainer(StaticStrings.MarijuanaCustomerMenuGroupID, 10),
                    new PercentageSelectGroupMenuContainer(StaticStrings.CokeCustomerMenuGroupID, 2),
                    new PercentageSelectGroupMenuContainer(StaticStrings.CrackCustomerMenuGroupID, 10),
                    new PercentageSelectGroupMenuContainer(StaticStrings.HeroinCustomerMenuGroupID, 2),
                    new PercentageSelectGroupMenuContainer(StaticStrings.SPANKCustomerMenuGroupID, 10),
            });

        ShopMenuGroupContainer MiddleAreaDealerMenuGroup = new ShopMenuGroupContainer(StaticStrings.MiddleAreaDrugDealerMenuGroupID, "Middle Area Dealer Menu",
            new List<PercentageSelectGroupMenuContainer>() {
                    new PercentageSelectGroupMenuContainer(StaticStrings.MethamphetamineDealerMenuGroupID, 5),
                    new PercentageSelectGroupMenuContainer(StaticStrings.ToiletCleanerDealerMenuGroupID, 5),
                    new PercentageSelectGroupMenuContainer(StaticStrings.MarijuanaDealerMenuGroupID, 40),
                    new PercentageSelectGroupMenuContainer(StaticStrings.CokeDealerMenuGroupID, 15),
                    new PercentageSelectGroupMenuContainer(StaticStrings.CrackDealerMenuGroupID, 5),
                    new PercentageSelectGroupMenuContainer(StaticStrings.HeroinDealerMenuGroupID, 5),
                    new PercentageSelectGroupMenuContainer(StaticStrings.SPANKDealerMenuGroupID, 5),
            });
        ShopMenuGroupContainer MiddleAreaCustomerMenuGroup = new ShopMenuGroupContainer(StaticStrings.MiddleAreaDrugCustomerMenuGroupID, "Middle Area Customer Menu",
            new List<PercentageSelectGroupMenuContainer>() {
                    new PercentageSelectGroupMenuContainer(StaticStrings.MethamphetamineCustomerMenuGroupID, 5),
                    new PercentageSelectGroupMenuContainer(StaticStrings.ToiletCleanerCustomerMenuGroupID, 5),
                    new PercentageSelectGroupMenuContainer(StaticStrings.MarijuanaCustomerMenuGroupID, 40),
                    new PercentageSelectGroupMenuContainer(StaticStrings.CokeCustomerMenuGroupID, 15),
                    new PercentageSelectGroupMenuContainer(StaticStrings.CrackCustomerMenuGroupID, 5),
                    new PercentageSelectGroupMenuContainer(StaticStrings.HeroinCustomerMenuGroupID, 5),
                    new PercentageSelectGroupMenuContainer(StaticStrings.SPANKCustomerMenuGroupID, 5),
            });

        ShopMenuGroupContainer RichAreaDealerMenuGroup = new ShopMenuGroupContainer(StaticStrings.RichAreaDrugDealerMenuGroupID, "Rich Area Dealer Menu",
            new List<PercentageSelectGroupMenuContainer>()
            {
                    new PercentageSelectGroupMenuContainer(StaticStrings.MethamphetamineDealerMenuGroupID, 2),
                    new PercentageSelectGroupMenuContainer(StaticStrings.ToiletCleanerDealerMenuGroupID, 1),
                    new PercentageSelectGroupMenuContainer(StaticStrings.MarijuanaDealerMenuGroupID, 30),
                    new PercentageSelectGroupMenuContainer(StaticStrings.CokeDealerMenuGroupID, 20),
                    new PercentageSelectGroupMenuContainer(StaticStrings.CrackDealerMenuGroupID, 2),
                    new PercentageSelectGroupMenuContainer(StaticStrings.HeroinDealerMenuGroupID, 15),
                    new PercentageSelectGroupMenuContainer(StaticStrings.SPANKDealerMenuGroupID, 1),
            });
        ShopMenuGroupContainer RichAreaCustomerMenuGroup = new ShopMenuGroupContainer(StaticStrings.RichAreaDrugCustomerMenuGroupID, "Rich Area Customer Menu",
            new List<PercentageSelectGroupMenuContainer>()
            {
                    new PercentageSelectGroupMenuContainer(StaticStrings.MethamphetamineCustomerMenuGroupID, 2),
                    new PercentageSelectGroupMenuContainer(StaticStrings.ToiletCleanerCustomerMenuGroupID, 1),
                    new PercentageSelectGroupMenuContainer(StaticStrings.MarijuanaCustomerMenuGroupID, 30),
                    new PercentageSelectGroupMenuContainer(StaticStrings.CokeCustomerMenuGroupID, 20),
                    new PercentageSelectGroupMenuContainer(StaticStrings.CrackCustomerMenuGroupID, 2),
                    new PercentageSelectGroupMenuContainer(StaticStrings.HeroinCustomerMenuGroupID, 15),
                    new PercentageSelectGroupMenuContainer(StaticStrings.SPANKCustomerMenuGroupID, 1),
            });

        ShopMenuGroupContainer MethamphetamineAreaDealerMenuGroup = new ShopMenuGroupContainer(StaticStrings.MethamphetamineAreaDrugDealerMenuGroupID, "Methamphetamine Area Dealer Menu",
            new List<PercentageSelectGroupMenuContainer>()
            {
                    new PercentageSelectGroupMenuContainer(StaticStrings.ToiletCleanerDealerMenuGroupID, 10),
                    new PercentageSelectGroupMenuContainer(StaticStrings.MethamphetamineDealerMenuGroupID, 90),
            });
        ShopMenuGroupContainer MethamphetamineAreaCustomerMenuGroup = new ShopMenuGroupContainer(StaticStrings.MethamphetamineAreaDrugCustomerMenuGroupID, "Methamphetamine Area Customer Menu",
            new List<PercentageSelectGroupMenuContainer>()
            {
                    new PercentageSelectGroupMenuContainer(StaticStrings.ToiletCleanerCustomerMenuGroupID, 15),
                    new PercentageSelectGroupMenuContainer(StaticStrings.MethamphetamineCustomerMenuGroupID, 85),
            });

        ShopMenuGroupContainer ToiletCleanerAreaDealerMenuGroup = new ShopMenuGroupContainer(StaticStrings.ToiletCleanerAreaDrugDealerMenuGroupID, "Toilet Cleaner Area Dealer Menu",
            new List<PercentageSelectGroupMenuContainer>()
            {
                    new PercentageSelectGroupMenuContainer(StaticStrings.MethamphetamineDealerMenuGroupID, 10),
                    new PercentageSelectGroupMenuContainer(StaticStrings.ToiletCleanerDealerMenuGroupID, 90),
            });
        ShopMenuGroupContainer ToiletCleanerAreaCustomerMenuGroup = new ShopMenuGroupContainer(StaticStrings.ToiletCleanerAreaDrugCustomerMenuGroupID, "Toilet Cleaner Area Customer Menu",
            new List<PercentageSelectGroupMenuContainer>()
            {
                    new PercentageSelectGroupMenuContainer(StaticStrings.MethamphetamineCustomerMenuGroupID, 15),
                    new PercentageSelectGroupMenuContainer(StaticStrings.ToiletCleanerCustomerMenuGroupID, 85),
            });

        ShopMenuGroupContainer MarijuanaAreaDealerMenuGroup = new ShopMenuGroupContainer(StaticStrings.MarijuanaAreaDrugDealerMenuGroupID, "Marijuana Area Dealer Menu",
            new List<PercentageSelectGroupMenuContainer>()
            {
                    new PercentageSelectGroupMenuContainer(StaticStrings.MarijuanaDealerMenuGroupID, 100),
            });
        ShopMenuGroupContainer MarijuanaAreaCustomerMenuGroup = new ShopMenuGroupContainer(StaticStrings.MarijuanaAreaDrugCustomerMenuGroupID, "Marijuana Area Customer Menu",
            new List<PercentageSelectGroupMenuContainer>()
            {
                    new PercentageSelectGroupMenuContainer(StaticStrings.MarijuanaCustomerMenuGroupID, 100),
            });

        ShopMenuGroupContainer CokeAreaDealerMenuGroup = new ShopMenuGroupContainer(StaticStrings.CokeAreaDrugDealerMenuGroupID, "Coke Area Dealer Menu",
            new List<PercentageSelectGroupMenuContainer>()
            {
                    new PercentageSelectGroupMenuContainer(StaticStrings.MarijuanaDealerMenuGroupID, 10),
                    new PercentageSelectGroupMenuContainer(StaticStrings.CokeDealerMenuGroupID, 90),
            });
        ShopMenuGroupContainer CokeAreaCustomerMenuGroup = new ShopMenuGroupContainer(StaticStrings.CokeAreaDrugCustomerMenuGroupID, "Coke Area Customer Menu",
            new List<PercentageSelectGroupMenuContainer>()
            {
                    new PercentageSelectGroupMenuContainer(StaticStrings.MarijuanaCustomerMenuGroupID, 15),
                    new PercentageSelectGroupMenuContainer(StaticStrings.CokeCustomerMenuGroupID, 85),
            });

        ShopMenuGroupContainer CrackAreaDealerMenuGroup = new ShopMenuGroupContainer(StaticStrings.CrackAreaDrugDealerMenuGroupID, "Crack Area Dealer Menu",
            new List<PercentageSelectGroupMenuContainer>()
            {
                    new PercentageSelectGroupMenuContainer(StaticStrings.MarijuanaDealerMenuGroupID, 10),
                    new PercentageSelectGroupMenuContainer(StaticStrings.CrackDealerMenuGroupID, 90),
            });
        ShopMenuGroupContainer CrackAreaCustomerMenuGroup = new ShopMenuGroupContainer(StaticStrings.CrackAreaDrugCustomerMenuGroupID, "Crack Area Customer Menu",
            new List<PercentageSelectGroupMenuContainer>()
            {
                    new PercentageSelectGroupMenuContainer(StaticStrings.MarijuanaCustomerMenuGroupID, 15),
                    new PercentageSelectGroupMenuContainer(StaticStrings.CrackCustomerMenuGroupID, 85),
            });

        ShopMenuGroupContainer HeroinAreaDealerMenuGroup = new ShopMenuGroupContainer(StaticStrings.HeroinAreaDrugDealerMenuGroupID, "Heroin Area Dealer Menu",
            new List<PercentageSelectGroupMenuContainer>()
            {
                    new PercentageSelectGroupMenuContainer(StaticStrings.MarijuanaDealerMenuGroupID, 10),
                    new PercentageSelectGroupMenuContainer(StaticStrings.HeroinDealerMenuGroupID, 90),
            });
        ShopMenuGroupContainer HeroinAreaCustomerMenuGroup = new ShopMenuGroupContainer(StaticStrings.HeroinAreaDrugCustomerMenuGroupID, "Heroin Area Customer Menu",
            new List<PercentageSelectGroupMenuContainer>()
            {
                    new PercentageSelectGroupMenuContainer(StaticStrings.MarijuanaCustomerMenuGroupID, 15),
                    new PercentageSelectGroupMenuContainer(StaticStrings.HeroinCustomerMenuGroupID, 85),
            });

        ShopMenuGroupContainer SPANKAreaDealerMenuGroup = new ShopMenuGroupContainer(StaticStrings.SPANKAreaDrugDealerMenuGroupID, "SPANK Area Dealer Menu",
            new List<PercentageSelectGroupMenuContainer>()
            {
                    new PercentageSelectGroupMenuContainer(StaticStrings.MarijuanaDealerMenuGroupID, 10),
                    new PercentageSelectGroupMenuContainer(StaticStrings.SPANKDealerMenuGroupID, 90),
            });
        ShopMenuGroupContainer SPANKAreaCustomerMenuGroup = new ShopMenuGroupContainer(StaticStrings.SPANKAreaDrugCustomerMenuGroupID, "SPANK Area Customer Menu",
            new List<PercentageSelectGroupMenuContainer>()
            {
                    new PercentageSelectGroupMenuContainer(StaticStrings.MarijuanaCustomerMenuGroupID, 15),
                    new PercentageSelectGroupMenuContainer(StaticStrings.SPANKCustomerMenuGroupID, 85),
            });

        PossibleShopMenus.ShopMenuGroupContainers.Add(PoorAreaDealerMenuGroup);
        PossibleShopMenus.ShopMenuGroupContainers.Add(PoorAreaCustomerMenuGroup);
        PossibleShopMenus.ShopMenuGroupContainers.Add(MiddleAreaDealerMenuGroup);
        PossibleShopMenus.ShopMenuGroupContainers.Add(MiddleAreaCustomerMenuGroup);
        PossibleShopMenus.ShopMenuGroupContainers.Add(RichAreaDealerMenuGroup);
        PossibleShopMenus.ShopMenuGroupContainers.Add(RichAreaCustomerMenuGroup);

        PossibleShopMenus.ShopMenuGroupContainers.Add(MethamphetamineAreaDealerMenuGroup);
        PossibleShopMenus.ShopMenuGroupContainers.Add(MethamphetamineAreaCustomerMenuGroup);

        PossibleShopMenus.ShopMenuGroupContainers.Add(ToiletCleanerAreaDealerMenuGroup);
        PossibleShopMenus.ShopMenuGroupContainers.Add(ToiletCleanerAreaCustomerMenuGroup);

        PossibleShopMenus.ShopMenuGroupContainers.Add(MarijuanaAreaDealerMenuGroup);
        PossibleShopMenus.ShopMenuGroupContainers.Add(MarijuanaAreaCustomerMenuGroup);

        PossibleShopMenus.ShopMenuGroupContainers.Add(CokeAreaDealerMenuGroup);
        PossibleShopMenus.ShopMenuGroupContainers.Add(CokeAreaCustomerMenuGroup);

        PossibleShopMenus.ShopMenuGroupContainers.Add(CrackAreaDealerMenuGroup);
        PossibleShopMenus.ShopMenuGroupContainers.Add(CrackAreaCustomerMenuGroup);

        PossibleShopMenus.ShopMenuGroupContainers.Add(HeroinAreaDealerMenuGroup);
        PossibleShopMenus.ShopMenuGroupContainers.Add(HeroinAreaCustomerMenuGroup);

        PossibleShopMenus.ShopMenuGroupContainers.Add(SPANKAreaDealerMenuGroup);
        PossibleShopMenus.ShopMenuGroupContainers.Add(SPANKAreaCustomerMenuGroup);
    }
    public void Setup(IModItems modItems)
    {
        foreach(ShopMenu sm in AllMenus())
        {
            int totalItems = sm.Items.Count;
            for (int i = totalItems - 1; i >= 0; i--)
            {
                MenuItem mi = sm.Items[i];
                if(mi != null)
                {
                    mi.ModItem = modItems.Get(mi.ModItemName);
                }
                if (mi.ModItem == null)
                {
                    EntryPoint.WriteToConsole($"Shop Menus ERROR Corresponding Item NOT FOUND {mi.ModItemName} in MENU {sm.Name} REMOVING FROM MENU",0);
                    sm.Items.RemoveAt(i);
                }
                mi.UpdatePrices();
                mi.UpdateStock();
            }
            if(totalItems == 0)
            {
                EntryPoint.WriteToConsole($"Shop Menus ERROR No Menu Items in MENU {sm.Name}", 0);
            }
        }
    }


}

