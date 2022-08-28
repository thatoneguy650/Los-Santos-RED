using ExtensionsMethods;
using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ShopMenus : IShopMenus
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\ShopMenus.xml";
    private List<ShopMenu> ShopMenuList;

    public ShopMenus()
    {

    }
    public void ReadConfig()
    {
        DirectoryInfo LSRDirectory = new DirectoryInfo("Plugins\\LosSantosRED");
        FileInfo ConfigFile = LSRDirectory.GetFiles("ShopMenus*.xml").OrderByDescending(x => x.Name).FirstOrDefault();
        if (ConfigFile != null)
        {
            EntryPoint.WriteToConsole($"Loaded Shop Menus config  {ConfigFile.FullName}",0);
            ShopMenuList = Serialization.DeserializeParams<ShopMenu>(ConfigFile.FullName);
        }
        else if (File.Exists(ConfigFileName))
        {
            EntryPoint.WriteToConsole($"Loaded Shop Menus config  {ConfigFileName}",0);
            ShopMenuList = Serialization.DeserializeParams<ShopMenu>(ConfigFileName);
        }
        else
        {
            EntryPoint.WriteToConsole($"No Shop Menus config found, creating default", 0);
            DefaultConfig();
        }
    }
    public ShopMenu GetMenu(string menuID)
    {
        return ShopMenuList.Where(x => x.ID == menuID).FirstOrDefault();
    }

    public Tuple<int,int> GetPrices(string itemName)
    {
        int LowestPrice = 9999;
        int HighestPrice = 0;
        foreach(ShopMenu shopMenu in ShopMenuList)
        {
            foreach(MenuItem menuItem in shopMenu.Items)
            {
                if(menuItem.Purchaseable && menuItem.ModItemName == itemName)
                {
                    if(menuItem.PurchasePrice < LowestPrice)
                    {
                        LowestPrice = menuItem.PurchasePrice;
                    }
                    if(menuItem.PurchasePrice > HighestPrice)
                    {
                        HighestPrice = menuItem.PurchasePrice;
                    }
                }
            }
        }
        return new Tuple<int, int>(LowestPrice,HighestPrice);
    }

    public ShopMenu GetRandomMenu(string menuGroup)
    {
        return ShopMenuList.Where(x => x.GroupName == menuGroup).PickRandom();
    }
    public ShopMenu GetRandomDrugDealerMenu()
    {
        ShopMenu Possible = ShopMenuList.Where(x => x.ID == "DealerMenu").PickRandom();
        return Possible;
    }
    public ShopMenu GetRandomDrugCustomerMenu()
    {
        ShopMenu Possible = ShopMenuList.Where(x => x.ID == "DrugCustomerMenu").PickRandom();
        return Possible;
    }
    public ShopMenu GetVendingMenu(string propName)
    {
        if (propName == "prop_vend_snak_01")
        {
            return ShopMenuList.Where(x => x.ID == "CandyVendingMenu").PickRandom();
        }
        else if (propName == "prop_vend_water_01" || propName == "0x418f055a")
        {
            return ShopMenuList.Where(x => x.ID == "WaterVendingMenu").PickRandom();
        }
        else if (propName == "prop_vend_soda_01" || propName == "0x426a547c")
        {
            return ShopMenuList.Where(x => x.ID == "SprunkVendingMenu").PickRandom();
        }
        else if (propName == "prop_vend_soda_02" || propName == "0x3b21c5e7")
        {
            return ShopMenuList.Where(x => x.ID == "eColaVendingMenu").PickRandom();
        }
        else if (propName == "prop_vend_coffe_01")
        {
            return ShopMenuList.Where(x => x.ID == "BeanMachineVendingMenu").PickRandom();
        }
        else if (propName == "prop_vend_fags_01")
        {
            return ShopMenuList.Where(x => x.ID == "CigVendingMenu").PickRandom();
        }
        else
        {
            return ShopMenuList.Where(x => x.ID == "VendingMenu").PickRandom();
        }
    }
    private void DefaultConfig()
    {
        ShopMenuList = new List<ShopMenu>();
        GenericLocationsMenu();

        SpecificVendingMachines();
        SpecificRestaurants();
        SpecificConvenienceStores();
        SpecificHotels();
        SpecificDealerships();
        SpecificWeaponsShops();

        DrugDealerMenus();
        DenList();
        GunShopList();
        Serialization.SerializeParams(ShopMenuList, ConfigFileName);
    }
    private void GenericLocationsMenu()
    {
        ShopMenuList.AddRange(new List<ShopMenu>{
            //Generic
            new ShopMenu("ToolMenu","Tools",new List<MenuItem>() { new MenuItem("Screwdriver",19),new MenuItem("Hammer", 15),new MenuItem("Drill", 50),new MenuItem("Pliers", 20),new MenuItem("Shovel", 60),new MenuItem("Wrench", 24),}),
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
                new MenuItem("Bottle of GREY Water", 3),}),
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
                new MenuItem("DIC Lighter", 5),
                new MenuItem("DIC Lighter Ultra", 7),
                new MenuItem("DIC Lighter Silver", 10),
                new MenuItem("DIC Lighter Gold", 15),
                new MenuItem("Dippo Lighter", 20),
                new MenuItem("Bottle of Raine Water", 2) }),
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
                new MenuItem("Bottle of Cazafortuna Tequila", 35),}),
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
                new MenuItem("Bottle of Raine Water", 2) }),
            new ShopMenu("FancyDeliMenu","Deli",new List<MenuItem>() { new MenuItem("Chicken Club Salad",10),new MenuItem("Spicy Seafood Gumbo",14),new MenuItem("Muffaletta",8),new MenuItem("Zucchini Garden Pasta",9),new MenuItem("Pollo Mexicano",12),new MenuItem("Italian Cruz Po'boy",19),new MenuItem("Chipotle Chicken Panini",10),new MenuItem("Bottle of Raine Water",2),new MenuItem("Cup of eCola",2),new MenuItem("Cup of Sprunk",2),}),
            new ShopMenu("FancyFishMenu","Fish",new List<MenuItem>() { new MenuItem("Coconut Crusted Prawns",12),new MenuItem("Crab and Shrimp Louie",10),new MenuItem("Open-Faced Crab Melt",28),new MenuItem("King Salmon",48),new MenuItem("Ahi Tuna",44),new MenuItem("Key Lime Pie",13),new MenuItem("Bottle of Raine Water",2), }),
            new ShopMenu("FancyGenericMenu","Restaurant",new List<MenuItem>() { new MenuItem("Smokehouse Burger",10),new MenuItem("Chicken Critters Basket",7),new MenuItem("Prime Rib 16 oz",22),new MenuItem("Bone-In Ribeye",25),new MenuItem("Grilled Pork Chops",14),new MenuItem("Grilled Shrimp",15),new MenuItem("Bottle of Raine Water",2),new MenuItem("Cup of eCola",2),new MenuItem("Cup of Sprunk",2),}),
            new ShopMenu("NoodleMenu","Noodles",new List<MenuItem>() { new MenuItem("Juek Suk tong Mandu",8),new MenuItem("Hayan Jam Pong",9),new MenuItem("Sal Gook Su Jam Pong",12),new MenuItem("Chul Pan Bokkeum Jam Pong",20),new MenuItem("Deul Gae Udon",12),new MenuItem("Dakgogo Bokkeum Bap",9),new MenuItem("Bottle of Raine Water",2),new MenuItem("Cup of eCola",2),new MenuItem("Cup of Sprunk",2),}),
                   
            //Drugs
            new ShopMenu("WeedMenu","Marijuana",new List<MenuItem>() {
                new MenuItem("White Widow",7),
                new MenuItem("OG Kush",8),
                new MenuItem("Northern Lights",9),
                new MenuItem("Bong",25),
                //new MenuItem("ElectroToke Vape", 25),
                new MenuItem("DIC Lighter",5),
                new MenuItem("DIC Lighter Ultra", 7),
            }),
            new ShopMenu("WeedAndCigMenu","Marijuana/Cigarette",new List<MenuItem>() { new MenuItem("White Widow",10),
                new MenuItem("OG Kush",12),
                new MenuItem("Northern Lights",13),
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
                new MenuItem("Lax to the Max", 24, 0),
                new MenuItem("Bull Shark Testosterone", 25, 0),
                new MenuItem("Alco Patch", 55, 0),
                new MenuItem("Equanox", 89, 0),
                new MenuItem("Mollis", 345, 0),
                new MenuItem("Zombix", 267, 0),
            }),

            //Cars
            new ShopMenu("CarMenu","Cars",new List<MenuItem>() {
                new MenuItem("Albany Alpha",80000),
                new MenuItem("Albany Roosevelt",70000),
                new MenuItem("Albany Fränken Stange",70000),
                new MenuItem("Albany Roosevelt Valor",70000),
                new MenuItem("Albany Buccaneer",150000),
                new MenuItem("Albany Buccaneer Custom",150000),
                new MenuItem("Albany Cavalcade",50000),
                new MenuItem("Albany Cavalcade 2",50000),
                new MenuItem("Albany Emperor",25000),
                new MenuItem("Albany Emperor 2",25000),
                new MenuItem("Albany Emperor 3",25000),
                new MenuItem("Albany Hermes",25000),
                new MenuItem("Albany Lurcher",150000),
                new MenuItem("Albany Manana",25000),
                new MenuItem("Albany Manana Custom",25000),
                new MenuItem("Albany Primo",35000),
                new MenuItem("Albany Primo Custom",35000),
                new MenuItem("Albany Virgo",150000),
                new MenuItem("Albany V-STR",150000),
                new MenuItem("Albany Washington",35000),
                new MenuItem("Annis Elegy Retro Custom",100000),
                new MenuItem("Annis Elegy RH8",100000),
                new MenuItem("Annis Euros",80000),
                new MenuItem("Annis Hellion",50000),
                new MenuItem("Annis RE-7B",35000),
                new MenuItem("Annis Remus",100000),
                new MenuItem("Annis S80RR",35000),
                new MenuItem("Annis Savestra",150000),
                new MenuItem("Annis ZR350",150000),
                new MenuItem("Annis Apocalypse ZR380",150000),
                new MenuItem("Annis Future Shock ZR380",150000),
                new MenuItem("Annis Nightmare ZR380",150000),
                new MenuItem("Benefactor Apocalypse Bruiser",40000),
                new MenuItem("Benefactor Future Shock Bruiser",40000),
                new MenuItem("Benefactor Nightmare Bruiser",40000),
                new MenuItem("Benefactor Dubsta",50000),
                new MenuItem("Benefactor Dubsta 2",50000),
                new MenuItem("Benefactor Dubsta 6x6",50000),
                new MenuItem("Benefactor Feltzer",30000),
                new MenuItem("Benefactor Stirling GT",30000),
                new MenuItem("Benefactor Glendale",100000),
                new MenuItem("Benefactor Glendale Custom",100000),
                new MenuItem("Benefactor Turreted Limo",35000),
                new MenuItem("Benefactor BR8",35000),
                new MenuItem("Benefactor Panto",35000),
                new MenuItem("Benefactor Schafter",35000),
                new MenuItem("Benefactor Schafter V12",35000),
                new MenuItem("Benefactor Schafter LWB",35000),
                new MenuItem("Benefactor Schafter V12 (Armored)",35000),
                new MenuItem("Benefactor Schafter LWB (Armored)",35000),
                new MenuItem("Benefactor Schwartzer",35000),
                new MenuItem("Benefactor Serrano",50000),
                new MenuItem("Benefactor Surano",80000),
                new MenuItem("Benefactor XLS",50000),
                new MenuItem("Benefactor XLS (Armored)",50000),
                new MenuItem("Benefactor Krieger",35000),
                new MenuItem("Benefactor Schlagen GT",30000),
                new MenuItem("Benefactor Streiter",150000),
                new MenuItem("Benefactor Terrorbyte",25000),
                new MenuItem("BF Injection",50000),
                new MenuItem("BF Bifta",50000),
                new MenuItem("BF Club",150000),
                new MenuItem("BF Dune Buggy",50000),
                new MenuItem("BF Dune FAV",50000),
                new MenuItem("BF Raptor",12000),
                //new MenuItem("BF Surfer",22000),
                new MenuItem("BF Surfer",22000),
                new MenuItem("BF Weevil",35000),
                new MenuItem("Bollokan Prairie",35000),
                new MenuItem("Bravado Banshee",150000),
                new MenuItem("Bravado Banshee 900R",150000),
                new MenuItem("Bravado Bison",25000),
                new MenuItem("Bravado Bison 2",25000),
                new MenuItem("Bravado Bison 3",25000),
                new MenuItem("Bravado Buffalo",25000),
                new MenuItem("Bravado Buffalo S",25000),
                new MenuItem("Bravado Sprunk Buffalo",25000),
                new MenuItem("Bravado Duneloader",25000),
                new MenuItem("Bravado Gauntlet",25000),
                new MenuItem("Bravado Redwood Gauntlet",25000),
                new MenuItem("Bravado Gauntlet Classic",25000),
                new MenuItem("Bravado Gauntlet Hellfire",25000),
                new MenuItem("Bravado Gauntlet Classic Custom",25000),
                new MenuItem("Bravado Gresley",50000),
                new MenuItem("Bravado Half-track",40000),
                new MenuItem("Bravado Apocalypse Sasquatch",500000),
                new MenuItem("Bravado Future Shock Sasquatch",500000),
                new MenuItem("Bravado Nightmare Sasquatch",500000),
                new MenuItem("Bravado Paradise",22000),
                new MenuItem("Bravado Rat-Truck",50000),
                new MenuItem("Bravado Rumpo",22000),
                new MenuItem("Bravado Rumpo 2",22000),
                new MenuItem("Bravado Rumpo Custom",50000),
                new MenuItem("Bravado Verlierer",150000),
                new MenuItem("Bravado Youga",22000),
                new MenuItem("Bravado Youga Classic",22000),
                new MenuItem("Bravado Youga Classic 4x4",22000),
                new MenuItem("Brute Boxville",25000),
                new MenuItem("Brute Boxville 3",25000),
                new MenuItem("Brute Boxville 4",25000),
                new MenuItem("Brute Camper",25000),
                new MenuItem("Brute Pony",22000),
                new MenuItem("Brute Pony 2",22000),
                new MenuItem("Brute Stockade",40000),
                new MenuItem("Brute Stockade 3",40000),
                new MenuItem("Brute Tipper",25000),
                new MenuItem("Canis Bodhi",50000),
                new MenuItem("Canis Crusader",50000),
                new MenuItem("Canis Freecrawler",50000),
                new MenuItem("Canis Kalahari",50000),
                new MenuItem("Canis Kamacho",50000),
                new MenuItem("Canis Mesa",50000),
                new MenuItem("Canis Mesa 2",50000),
                new MenuItem("Canis Mesa 3",50000),
                new MenuItem("Canis Seminole",50000),
                new MenuItem("Canis Seminole Frontier",50000),
                new MenuItem("Chariot Romero Hearse",35000),
                new MenuItem("Cheval Fugitive",35000),
                new MenuItem("Cheval Marshall",500000),
                new MenuItem("Cheval Picador",35000),
                new MenuItem("Cheval Surge",35000),
                new MenuItem("Cheval Taipan",35000),
                new MenuItem("Coil Brawler",50000),
                new MenuItem("Coil Cyclone",35000),
                new MenuItem("Coil Raiden",35000),
                new MenuItem("Coil Voltic",80000),
                new MenuItem("Coil Rocket Voltic",80000),
                new MenuItem("Declasse Asea",35000),
                new MenuItem("Declasse Asea",35000),
                new MenuItem("Declasse Apocalypse Brutus",50000),
                new MenuItem("Declasse Future Shock Brutus",50000),
                new MenuItem("Declasse Nightmare Brutus",50000),
                new MenuItem("Declasse Burrito",22000),
                new MenuItem("Declasse Bugstars Burrito",22000),
                new MenuItem("Declasse Burrito 3",22000),
                new MenuItem("Declasse Burrito 4",22000),
                new MenuItem("Declasse Burrito 5",22000),
                new MenuItem("Declasse Gang Burrito",22000),
                new MenuItem("Declasse Gang Burrito 2",22000),
                new MenuItem("Declasse Granger",50000),
                new MenuItem("Declasse Hotring Sabre",100000),
                new MenuItem("Declasse Impaler",30000),
                new MenuItem("Declasse Apocalypse Impaler",30000),
                new MenuItem("Declasse Future Shock Impaler",30000),
                new MenuItem("Declasse Nightmare Impaler",30000),
                new MenuItem("Declasse Lifeguard",50000),
                new MenuItem("Declasse Mamba",150000),
                new MenuItem("Declasse Moonbeam",50000),
                new MenuItem("Declasse Moonbeam Custom",50000),
                new MenuItem("Declasse DR1",35000),
                new MenuItem("Declasse Premier",35000),
                new MenuItem("Declasse Rancher XL",50000),
                new MenuItem("Declasse Rancher XL 2",50000),
                new MenuItem("Declasse Rhapsody",35000),
                new MenuItem("Declasse Sabre Turbo",150000),
                new MenuItem("Declasse Sabre Turbo Custom",150000),
                new MenuItem("Declasse Scramjet",130000),
                new MenuItem("Declasse Stallion",150000),
                new MenuItem("Declasse Burger Shot Stallion",150000),
                new MenuItem("Declasse Tampa",150000),
                new MenuItem("Declasse Drift Tampa",150000),
                new MenuItem("Declasse Weaponized Tampa",150000),
                new MenuItem("Declasse Tornado",25000),
                new MenuItem("Declasse Tornado 2",25000),
                new MenuItem("Declasse Tornado 3",25000),
                new MenuItem("Declasse Tornado 4",25000),
                new MenuItem("Declasse Tornado Custom",25000),
                new MenuItem("Declasse Tornado Rat Rod",25000),
                new MenuItem("Declasse Tulip",30000),
                new MenuItem("Declasse Vamos",150000),
                new MenuItem("Declasse Vigero",50000),
                new MenuItem("Declasse Voodoo Custom",25000),
                new MenuItem("Declasse Voodoo",25000),
                new MenuItem("Declasse Yosemite",25000),
                new MenuItem("Declasse Drift Yosemite",25000),
                new MenuItem("Declasse Yosemite Rancher",50000),
                new MenuItem("Dewbauchee Exemplar",35000),
                new MenuItem("Dewbauchee JB 700",150000),
                new MenuItem("Dewbauchee JB 700W",150000),
                new MenuItem("Dewbauchee Massacro",35000),
                new MenuItem("Dewbauchee Massacro (Racecar)",35000),
                new MenuItem("Dewbauchee Rapid GT",35000),
                new MenuItem("Dewbauchee Rapid GT 2",35000),
                new MenuItem("Dewbauchee Rapid GT Classic",100000),
                new MenuItem("Dewbauchee Seven-70",150000),
                new MenuItem("Dewbauchee Specter",150000),
                new MenuItem("Dewbauchee Specter Custom",150000),
                new MenuItem("Dewbauchee Vagner",35000),
                new MenuItem("Dinka Akuma",20000),
                new MenuItem("Dinka Blista",35000),
                new MenuItem("Dinka Blista Compact",35000),
                new MenuItem("Dinka Go Go Monkey Blista",35000),
                new MenuItem("Dinka Double-T",20000),
                new MenuItem("Dinka Enduro",12000),
                new MenuItem("Dinka Jester",80000),
                new MenuItem("Dinka Jester (Racecar)",80000),
                new MenuItem("Dinka Jester Classic",80000),
                new MenuItem("Dinka Jester RR",80000),
                new MenuItem("Dinka Blista Kanjo",150000),
                new MenuItem("Dinka RT3000",80000),
                new MenuItem("Dinka Sugoi",150000),
                new MenuItem("Dinka Thrust",12000),
                new MenuItem("Dinka Veto Classic",50000),
                new MenuItem("Dinka Veto Modern",50000),
                new MenuItem("Dinka Vindicator",12000),
                new MenuItem("Dundreary Landstalker",50000),
                new MenuItem("Dundreary Landstalker XL",50000),
                new MenuItem("Dundreary Regina",25000),
                new MenuItem("Dundreary Stretch",35000),
                new MenuItem("Dundreary Virgo Classic Custom",150000),
                new MenuItem("Dundreary Virgo Classic",150000),
                new MenuItem("Emperor Habanero",35000),
                new MenuItem("Emperor ETR1",35000),
                new MenuItem("Emperor Vectre",150000),
                new MenuItem("Enus Cognoscenti 55",25000),
                new MenuItem("Enus Cognoscenti 55 (Armored)",25000),
                new MenuItem("Enus Cognoscenti Cabrio",35000),
                new MenuItem("Enus Cognoscenti",25000),
                new MenuItem("Enus Cognoscenti (Armored)",25000),
                new MenuItem("Enus Huntley S",50000),
                new MenuItem("Enus Paragon R",150000),
                new MenuItem("Enus Paragon R (Armored)",150000),
                new MenuItem("Enus Stafford",70000),
                new MenuItem("Enus Super Diamond",25000),
                new MenuItem("Enus Windsor",25000),
                new MenuItem("Enus Windsor Drop",25000),
                new MenuItem("Fathom FQ 2",35000),
                new MenuItem("Gallivanter Baller",50000),
                new MenuItem("Gallivanter Baller 2",50000),
                new MenuItem("Gallivanter Baller LE",50000),
                new MenuItem("Gallivanter Baller LE LWB",50000),
                new MenuItem("Gallivanter Baller LE (Armored)",50000),
                new MenuItem("Gallivanter Baller LE LWB (Armored)",50000),
                new MenuItem("Grotti Bestia GTS",150000),
                new MenuItem("Grotti Brioso R/A",35000),
                new MenuItem("Grotti Brioso 300",35000),
                new MenuItem("Grotti Carbonizzare",100000),
                new MenuItem("Grotti Cheetah",150000),
                new MenuItem("Grotti Cheetah Classic",150000),
                new MenuItem("Grotti Furia",35000),
                new MenuItem("Grotti GT500",130000),
                new MenuItem("Grotti Itali GTO",35000),
                new MenuItem("Grotti Itali RSX",35000),
                new MenuItem("Grotti X80 Proto",35000),
                new MenuItem("Grotti Stinger",130000),
                new MenuItem("Grotti Stinger GT",130000),
                new MenuItem("Grotti Turismo Classic",100000),
                new MenuItem("Grotti Turismo R",150000),
                new MenuItem("Grotti Visione",35000),
                new MenuItem("Hijak Khamelion",35000),
                new MenuItem("Hijak Ruston",150000),
                new MenuItem("HVY Barracks Semi",40000),
                new MenuItem("HVY Biff",40000),
                new MenuItem("HVY Dozer",40000),
                new MenuItem("HVY Cutter",25000),
                new MenuItem("HVY Dump",40000),
                new MenuItem("HVY Forklift",25000),
                new MenuItem("HVY Insurgent Pick-Up",50000),
                new MenuItem("HVY Insurgent",50000),
                new MenuItem("HVY Insurgent Pick-Up Custom",50000),
                new MenuItem("HVY Menacer",50000),
                new MenuItem("HVY Mixer",40000),
                new MenuItem("HVY Mixer 2",40000),
                new MenuItem("HVY Nightshark",50000),
                new MenuItem("HVY Apocalypse Scarab",40000),
                new MenuItem("HVY Future Shock Scarab",40000),
                new MenuItem("HVY Nightmare Scarab",40000),
                new MenuItem("Imponte Deluxo",150000),
                new MenuItem("Imponte Dukes",150000),
                new MenuItem("Imponte Duke O'Death",150000),
                new MenuItem("Imponte Beater Dukes",150000),
                new MenuItem("Imponte Nightshade",130000),
                new MenuItem("Imponte Phoenix",20000),
                new MenuItem("Imponte Ruiner",150000),
                new MenuItem("Imponte Ruiner 2000",150000),
                new MenuItem("Imponte Ruiner",150000),
                new MenuItem("Invetero Coquette",150000),
                new MenuItem("Invetero Coquette Classic",150000),
                new MenuItem("Invetero Coquette BlackFin",150000),
                new MenuItem("Invetero Coquette D10",150000),
                new MenuItem("JoBuilt Hauler",40000),
                new MenuItem("JoBuilt Hauler Custom",40000),
                new MenuItem("JoBuilt Phantom",40000),
                new MenuItem("JoBuilt Phantom Wedge",40000),
                new MenuItem("JoBuilt Phantom Custom",40000),
                new MenuItem("JoBuilt Rubble",25000),
                new MenuItem("Karin Asterope",35000),
                new MenuItem("Karin BeeJay XL",50000),
                new MenuItem("Karin Calico GTF",150000),
                new MenuItem("Karin Dilettante",35000),
                new MenuItem("Karin Dilettante 2",35000),
                new MenuItem("Karin Everon",50000),
                new MenuItem("Karin Futo",100000),
                new MenuItem("Karin Futo GTX",100000),
                new MenuItem("Karin Intruder",35000),
                new MenuItem("Karin Kuruma",35000),
                new MenuItem("Karin Kuruma (armored)",35000),
                new MenuItem("Karin Previon",150000),
                new MenuItem("Karin Rusty Rebel",50000),
                new MenuItem("Karin Rebel",50000),
                new MenuItem("Karin Sultan",35000),
                new MenuItem("Karin Sultan Classic",35000),
                new MenuItem("Karin Sultan RS Classic",35000),
                new MenuItem("Karin Sultan RS",35000),
                new MenuItem("Karin Technical",50000),
                new MenuItem("Karin Technical Custom",50000),
                new MenuItem("Karin 190z",130000),
                new MenuItem("Lampadati Casco",130000),
                new MenuItem("Lampadati Felon",35000),
                new MenuItem("Lampadati Felon GT",35000),
                new MenuItem("Lampadati Furore GT",150000),
                new MenuItem("Lampadati Michelli GT",130000),
                new MenuItem("Lampadati Pigalle",100000),
                new MenuItem("Lampadati Tropos Rallye",150000),
                new MenuItem("Lampadati Komoda",150000),
                new MenuItem("Lampadati Novak",50000),
                new MenuItem("Lampadati Tigon",35000),
                new MenuItem("Lampadati Viseris",150000),
                new MenuItem("LCC Avarus",12000),
                new MenuItem("LCC Hexer",12000),
                new MenuItem("LCC Innovation",12000),
                new MenuItem("LCC Sanctus",12000),
                new MenuItem("Maibatsu Manchez",12000),
                new MenuItem("Maibatsu Manchez Scout",12000),
                new MenuItem("Maibatsu Mule",25000),
                new MenuItem("Maibatsu Mule",25000),
                new MenuItem("Maibatsu Mule",25000),
                new MenuItem("Maibatsu Mule Custom",25000),
                new MenuItem("Maibatsu Penumbra",150000),
                new MenuItem("Maibatsu Penumbra FF",150000),
                new MenuItem("Maibatsu Sanchez (livery)",12000),
                new MenuItem("Maibatsu Sanchez",12000),
                new MenuItem("Mammoth Patriot",50000),
                new MenuItem("Mammoth Patriot Stretch",50000),
                new MenuItem("Mammoth Squaddie",50000),
                new MenuItem("Maxwell Asbo",150000),
                new MenuItem("Maxwell Vagrant",50000),
                new MenuItem("MTL Brickade",40000),
                new MenuItem("MTL Apocalypse Cerberus",25000),
                new MenuItem("MTL Future Shock Cerberus",25000),
                new MenuItem("MTL Nightmare Cerberus",25000),
                new MenuItem("MTL Fire Truck",40000),
                new MenuItem("MTL Flatbed",25000),
                new MenuItem("MTL Packer",40000),
                new MenuItem("MTL Pounder",25000),
                new MenuItem("MTL Pounder Custom",25000),
                new MenuItem("MTL Dune",40000),
                new MenuItem("MTL Wastelander",40000),
                new MenuItem("Nagasaki BF400",12000),
                new MenuItem("Nagasaki Carbon RS",20000),
                new MenuItem("Nagasaki Outlaw",50000),
                new MenuItem("Nagasaki Shotaro",20000),
                new MenuItem("Obey 8F Drafter",150000),
                new MenuItem("Obey 9F",80000),
                new MenuItem("Obey 9F Cabrio",80000),
                new MenuItem("Obey Omnis",35000),
                new MenuItem("Obey Rocoto",35000),
                new MenuItem("Obey Tailgater",35000),
                new MenuItem("Obey Tailgater S",35000),
                new MenuItem("Ocelot Ardent",150000),
                new MenuItem("Ocelot F620",35000),
                new MenuItem("Ocelot R88",35000),
                new MenuItem("Ocelot Jackal",35000),
                new MenuItem("Ocelot Jugular",0),
                new MenuItem("Ocelot Locust",35000),
                new MenuItem("Ocelot Lynx",150000),
                new MenuItem("Ocelot Pariah",150000),
                new MenuItem("Ocelot Penetrator",35000),
                new MenuItem("Ocelot Swinger",130000),
                new MenuItem("Ocelot XA-21",35000),
                new MenuItem("Overflod Autarch",35000),
                new MenuItem("Overflod Entity XXR",35000),
                new MenuItem("Overflod Entity XF",150000),
                new MenuItem("Overflod Imorgon",35000),
                new MenuItem("Overflod Tyrant",35000),
                new MenuItem("Pegassi Bati 801",20000),
                new MenuItem("Pegassi Bati 801RR",20000),
                new MenuItem("Pegassi Esskey",12000),
                new MenuItem("Pegassi Faggio Sport",12000),
                new MenuItem("Pegassi Faggio",12000),
                new MenuItem("Pegassi Faggio Mod",12000),
                new MenuItem("Pegassi FCR 1000",20000),
                new MenuItem("Pegassi FCR 1000 Custom",20000),
                new MenuItem("Pegassi Infernus",100000),
                new MenuItem("Pegassi Infernus Classic",100000),
                new MenuItem("Pegassi Monroe",70000),
                new MenuItem("Pegassi Oppressor",20000),
                new MenuItem("Pegassi Oppressor Mk II",20000),
                new MenuItem("Pegassi Osiris",35000),
                new MenuItem("Pegassi Reaper",35000),
                new MenuItem("Pegassi Ruffian",20000),
                new MenuItem("Pegassi Tempesta",35000),
                new MenuItem("Pegassi Tezeract",35000),
                new MenuItem("Pegassi Torero",100000),
                new MenuItem("Pegassi Toros",50000),
                new MenuItem("Pegassi Vacca",35000),
                new MenuItem("Pegassi Vortex",20000),
                new MenuItem("Pegassi Zentorno",35000),
                new MenuItem("Pegassi Zorrusso",35000),
                new MenuItem("Pfister Comet",150000),
                new MenuItem("Pfister Comet Retro Custom",150000),
                new MenuItem("Pfister Comet Safari",150000),
                new MenuItem("Pfister Comet SR",150000),
                new MenuItem("Pfister Comet S2",150000),
                new MenuItem("Pfister Growler",150000),
                new MenuItem("Pfister Neon",35000),
                new MenuItem("Pfister 811",150000),
                new MenuItem("Principe Deveste Eight",35000),
                new MenuItem("Principe Diabolus",12000),
                new MenuItem("Principe Diabolus Custom",12000),
                new MenuItem("Principe Lectro",20000),
                new MenuItem("Principe Nemesis",20000),
                new MenuItem("Progen Emerus",35000),
                new MenuItem("Progen PR4",35000),
                new MenuItem("Progen GP1",35000),
                new MenuItem("Progen Itali GTB",35000),
                new MenuItem("Progen Itali GTB Custom",35000),
                new MenuItem("Progen T20",35000),
                new MenuItem("Progen Tyrus",35000),
                new MenuItem("RUNE Cheburek",150000),
                new MenuItem("Schyster Deviant",150000),
                new MenuItem("Schyster Fusilade",35000),
                new MenuItem("Shitzu Defiler",20000),
                new MenuItem("Shitzu Hakuchou",20000),
                new MenuItem("Shitzu Hakuchou Drag",20000),
                new MenuItem("Shitzu PCJ 600",20000),
                new MenuItem("Shitzu Vader",12000),
                new MenuItem("Stanley Fieldmaster",50000),
                new MenuItem("Stanley Fieldmaster",50000),
                new MenuItem("Truffade Adder",80000),
                new MenuItem("Truffade Nero",80000),
                new MenuItem("Truffade Nero Custom",80000),
                new MenuItem("Truffade Thrax",80000),
                new MenuItem("Truffade Z-Type",70000),
                new MenuItem("Ubermacht Oracle XS",35000),
                new MenuItem("Ubermacht Oracle",35000),
                new MenuItem("Ubermacht Revolter",150000),
                new MenuItem("Ubermacht SC1",35000),
                new MenuItem("Ubermacht Sentinel XS",35000),
                new MenuItem("Ubermacht Sentinel 2",35000),
                new MenuItem("Ubermacht Sentinel 3",150000),
                new MenuItem("Ubermacht Zion",35000),
                new MenuItem("Ubermacht Zion Cabrio",35000),
                new MenuItem("Ubermacht Zion Classic",30000),
                new MenuItem("Ubermacht Cypher",0),
                new MenuItem("Ubermacht Rebla GTS",50000),
                new MenuItem("Vapid Benson",25000),
                new MenuItem("Vapid Blade",150000),
                new MenuItem("Vapid Bobcat XL",50000),
                new MenuItem("Vapid Bullet",150000),
                new MenuItem("Vapid Caracara",50000),
                new MenuItem("Vapid Caracara 4x4",50000),
                new MenuItem("Vapid Chino",150000),
                new MenuItem("Vapid Chino Custom",150000),
                new MenuItem("Vapid Clique",30000),
                new MenuItem("Vapid Contender",25000),
                new MenuItem("Vapid Dominator",150000),
                new MenuItem("Vapid Pisswasser Dominator",150000),
                new MenuItem("Vapid Dominator GTX",100000),
                new MenuItem("Vapid Apocalypse Dominator",150000),
                new MenuItem("Vapid Future Shock Dominator",150000),
                new MenuItem("Vapid Nightmare Dominator",150000),
                new MenuItem("Vapid Dominator ASP",35000),
                new MenuItem("Vapid Dominator GTT",25000),
                new MenuItem("Vapid Ellie",100000),
                new MenuItem("Vapid Flash GT",35000),
                new MenuItem("Vapid FMJ",35000),
                new MenuItem("Vapid GB200",35000),
                new MenuItem("Vapid Guardian",50000),
                new MenuItem("Vapid Hotknife",50000),
                new MenuItem("Vapid Hustler",50000),
                new MenuItem("Vapid Apocalypse Imperator",150000),
                new MenuItem("Vapid Future Shock Imperator",150000),
                new MenuItem("Vapid Nightmare Imperator",150000),
                new MenuItem("Vapid Minivan",50000),
                new MenuItem("Vapid Minivan Custom",50000),
                new MenuItem("Vapid Monster",500000),
                new MenuItem("Vapid Peyote",25000),
                new MenuItem("Vapid Peyote Gasser",25000),
                new MenuItem("Vapid Peyote Custom",25000),
                new MenuItem("Vapid Radius",50000),
                new MenuItem("Vapid Retinue",150000),
                new MenuItem("Vapid Retinue Mk II",150000),
                new MenuItem("Vapid Riata",50000),
                new MenuItem("Vapid Sadler",25000),
                new MenuItem("Vapid Sadler",25000),
                new MenuItem("Vapid Sandking XL",50000),
                new MenuItem("Vapid Sandking SWB",50000),
                new MenuItem("Vapid Slamtruck",25000),
                new MenuItem("Vapid Slamvan",25000),
                new MenuItem("Vapid Lost Slamvan",25000),
                new MenuItem("Vapid Slamvan Custom",25000),
                new MenuItem("Vapid Apocalypse Slamvan",30000),
                new MenuItem("Vapid Future Shock Slamvan",30000),
                new MenuItem("Vapid Nightmare Slamvan",30000),
                new MenuItem("Vapid Speedo",22000),
                new MenuItem("Vapid Clown Van",22000),
                new MenuItem("Vapid Speedo Custom",22000),
                new MenuItem("Vapid Stanier",35000),
                new MenuItem("Vapid Trophy Truck",50000),
                new MenuItem("Vapid Desert Raid",50000),
                new MenuItem("Vapid Winky",50000),
                new MenuItem("Vulcar Fagaloa",25000),
                new MenuItem("Vulcar Ingot",35000),
                new MenuItem("Vulcar Nebula Turbo",150000),
                new MenuItem("Vulcar Warrener",100000),
                new MenuItem("Vulcar Warrener HKR",100000),
                new MenuItem("Vysser Neo",35000),
                new MenuItem("Weeny Dynasty",70000),
                new MenuItem("Weeny Issi",35000),
                new MenuItem("Weeny Issi Classic",52000),
                new MenuItem("Weeny Apocalypse Issi",52000),
                new MenuItem("Weeny Future Shock Issi",52000),
                new MenuItem("Weeny Nightmare Issi",52000),
                new MenuItem("Weeny Issi Sport",35000),
                new MenuItem("Western Bagger",12000),
                new MenuItem("Western Cliffhanger",12000),
                new MenuItem("Western Daemon",12000),
                new MenuItem("Western Daemon 2",12000),
                new MenuItem("Western Apocalypse Deathbike",12000),
                new MenuItem("Western Future Shock Deathbike",12000),
                new MenuItem("Western Nightmare Deathbike",12000),
                new MenuItem("Western Gargoyle",12000),
                new MenuItem("Western Nightblade",12000),
                new MenuItem("Western Rat Bike",12000),
                new MenuItem("Western Sovereign",12000),
                new MenuItem("Western Wolfsbane",12000),
                new MenuItem("Western Zombie Bobber",12000),
                new MenuItem("Western Zombie Chopper",12000),
                new MenuItem("Willard Faction",150000),
                new MenuItem("Willard Faction Custom",150000),
                new MenuItem("Willard Faction Custom Donk",150000),
                new MenuItem("Zirconium Journey",25000),
                new MenuItem("Zirconium Stratum",35000),
            }),
            new ShopMenu("HeliMenu","Heli",new List<MenuItem>() {
                new MenuItem("Buckingham SuperVolito",52000),
                new MenuItem("Buckingham SuperVolito Carbon",52000),
                new MenuItem("Buckingham Swift",52000),
                new MenuItem("Buckingham Swift Deluxe",52000),
                new MenuItem("Buckingham Volatus",52000),
                new MenuItem("Mammoth Thruster",52000),
                new MenuItem("Nagasaki Havok",52000),
            }),
            new ShopMenu("PlaneMenu","Planes",new List<MenuItem>() {
                new MenuItem("Buckingham Alpha-Z1",45000),
                new MenuItem("Buckingham Howard NX-25",45000),
                new MenuItem("Buckingham Luxor",45000),
                new MenuItem("Buckingham Luxor Deluxe",45000),
                new MenuItem("Buckingham Miljet",45000),
                new MenuItem("Buckingham Nimbus",45000),
                new MenuItem("Buckingham Pyro",45000),
                new MenuItem("Buckingham Shamal",45000),
                new MenuItem("Buckingham Vestra",45000),
                new MenuItem("Mammoth Avenger",45000),
                new MenuItem("Mammoth Avenger 2",45000),
                new MenuItem("Mammoth Dodo",45000),
                new MenuItem("Mammoth Hydra",45000),
                new MenuItem("Mammoth Mogul",45000),
                new MenuItem("Mammoth Tula",45000),
                new MenuItem("Nagasaki Ultralight",45000),
                new MenuItem("Western Besra",45000),
                new MenuItem("Western Rogue",45000),
                new MenuItem("Western Seabreeze",45000),
            }),
            new ShopMenu("BoatMenu","Boats",new List<MenuItem>() {
                new MenuItem("Dinka Marquis",40000),
                new MenuItem("Lampadati Toro",40000),
                new MenuItem("Lampadati Toro",40000),
                new MenuItem("Nagasaki Dinghy",40000),
                new MenuItem("Nagasaki Dinghy 2",40000),
                new MenuItem("Nagasaki Dinghy 3",40000),
                new MenuItem("Nagasaki Dinghy 4",40000),
                new MenuItem("Nagasaki Weaponized Dinghy",40000),
                new MenuItem("Pegassi Speeder",40000),
                new MenuItem("Pegassi Speeder",40000),
                new MenuItem("Shitzu Jetmax",40000),
                new MenuItem("Shitzu Longfin",40000),
                new MenuItem("Shitzu Squalo",40000),
                new MenuItem("Shitzu Suntrap",40000),
                new MenuItem("Shitzu Tropic",40000),
                new MenuItem("Shitzu Tropic",40000),
                new MenuItem("Speedophile Seashark",40000),
                new MenuItem("Speedophile Seashark 2",40000),
                new MenuItem("Speedophile Seashark 3",40000),
            }),

        });
    }
    private void SpecificRestaurants()
    {
        ShopMenuList.AddRange(new List<ShopMenu>{
            //Specific
            new ShopMenu("BurgerShotMenu","Burger Shot",new List<MenuItem> {
                new MenuItem("Money Shot Meal", 7),
                new MenuItem("The Bleeder Meal", 4),
                new MenuItem("Torpedo Meal", 6),
                new MenuItem("Meat Free Meal", 5),


                new MenuItem("Freedom Fries", 2),
                new MenuItem("Liter of eCola", 2),
                new MenuItem("Liter of Sprunk", 2),
                new MenuItem("Bottle of Raine Water", 2),
                new MenuItem("Double Shot Coffee", 2) }),
            new ShopMenu("UpNAtomMenu","Up-N-Atom",new List<MenuItem>() {
                new MenuItem("Triple Burger", 4),
                new MenuItem("Bacon Triple Cheese Melt", 3),
                new MenuItem("Jumbo Shake", 5),
                new MenuItem("Bacon Burger", 2),
                new MenuItem("French Fries", 2),
                new MenuItem("Cup of eCola", 2),
                new MenuItem("Cup of Sprunk", 2),
                new MenuItem("Cup of Coffee", 3),
                new MenuItem("Bottle of Raine Water", 5) }),
            new ShopMenu("BeefyBillsMenu","Beefy Bills",new List<MenuItem>() {
                new MenuItem("Burger", 3),
                new MenuItem("Megacheese Burger", 2),
                new MenuItem("Double Burger", 2),
                new MenuItem("Kingsize Burger", 2),
                new MenuItem("Bacon Burger", 2),
                new MenuItem("French Fries", 2),
                new MenuItem("Can of eCola", 1),
                new MenuItem("Can of Sprunk", 1),
                new MenuItem("Bottle of Raine Water", 2) }),
            new ShopMenu("ChihuahuaHotDogMenu","Chihuahua HotDogs",new List<MenuItem>() {
                new MenuItem("Hot Dog", 5),
                new MenuItem("Hot Sausage", 5),
                new MenuItem("Hot Pretzel", 2),
                new MenuItem("3 Mini Pretzels", 3),
                new MenuItem("Nuts", 2),
                new MenuItem("Can of Sprunk", 1),
                new MenuItem("Bottle of Raine Water", 2) }),
            new ShopMenu("TacoFarmerMenu","Taco Farmer",new List<MenuItem>() {
                new MenuItem("Taco", 2),
                new MenuItem("Can of eCola", 1),
                new MenuItem("Can of Sprunk", 1),
                new MenuItem("Cup of Coffee", 3),
                new MenuItem("Bottle of Raine Water", 2) }),
            new ShopMenu("BiteMenu","Bite!",new List<MenuItem>() {
                new MenuItem("Gut Buster Sandwich", 9),
                new MenuItem("Ham and Tuna Sandwich", 7),
                new MenuItem("Chef's Salad", 4),
                new MenuItem("Cup of eCola", 1),
                new MenuItem("Cup of Sprunk", 1),
                new MenuItem("Bottle of Raine Water", 2) }),
            new ShopMenu("TacoBombMenu","Taco Bomb",new List<MenuItem> {
                new MenuItem("Breakfast Burrito",4),
                new MenuItem("Deep Fried Salad",7),
                new MenuItem("Beef Bazooka",8),
                new MenuItem("Chimichingado Chiquito",5),
                new MenuItem("Cheesy Meat Flappers",6),
                new MenuItem("Volcano Mudsplatter Nachos",7),
                new MenuItem("Can of eCola", 1),
                new MenuItem("Can of Sprunk", 1),
                new MenuItem("Bottle of Raine Water", 2) }),
            new ShopMenu("WigwamMenu","Wigwam",new List<MenuItem>() {
                new MenuItem("Wigwam Burger", 3),
                new MenuItem("Wigwam Cheeseburger", 2),
                new MenuItem("Big Wig Burger", 5),
                new MenuItem("French Fries", 2),
                new MenuItem("Cup of eCola", 1),
                new MenuItem("Cup of Sprunk", 1),
                new MenuItem("Bottle of Raine Water", 2) }),
            new ShopMenu("CluckinBellMenu","Cluckin' Bell",new List<MenuItem>() {
                new MenuItem("Cluckin' Little Meal",2),
                new MenuItem("Cluckin' Big Meal",6),
                new MenuItem("Cluckin' Huge Meal",12),
                new MenuItem("Wing Piece",7),
                new MenuItem("Little Peckers",8),
                new MenuItem("Balls & Rings",4),
                new MenuItem("Fries",2),
                new MenuItem("Fowlburger",5),
                new MenuItem("Cup Of Coffee",3),
                new MenuItem("Cup of eCola",2),
                new MenuItem("Cup of Sprunk",2), }),
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
            new ShopMenu("AlDentesMenu","Al Dentes",new List<MenuItem>() {
                new MenuItem("Small Cheese Pizza", 9),
                new MenuItem("Small Pepperoni Pizza", 11),
                new MenuItem("Small Supreme Pizza", 12),
                new MenuItem("Medium Cheese Pizza", 16),
                new MenuItem("Medium Pepperoni Pizza", 17),
                new MenuItem("Medium Supreme Pizza", 18),
                new MenuItem("Large Cheese Pizza", 22),
                new MenuItem("Large Pepperoni Pizza", 23),
                new MenuItem("Large Supreme Pizza", 24),
                new MenuItem("Cup of Sprunk", 2),
                new MenuItem("Bottle of A.M.", 3),
                new MenuItem("Bottle of PiBwasser", 3),
                new MenuItem("Bottle of Barracho", 4),
                new MenuItem("Bottle of Blarneys", 4),
                new MenuItem("Bottle of Jakeys", 3),
                new MenuItem("Bottle of Stronzo", 4),
                new MenuItem("Bottle of Dusche", 3)


            }),
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
                new MenuItem("Bottle of Raine Water", 3)
            }),
        });
    }
    private void SpecificConvenienceStores()
    {
        ShopMenuList.AddRange(new List<ShopMenu>{
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
                new MenuItem("Bottle of GREY Water", 3),}),
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
                new MenuItem("Cup of Coffee", 2),
                new MenuItem("Bottle of Raine Water", 2),
                new MenuItem("Bottle of GREY Water", 3),}),
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
                new MenuItem("Bottle of GREY Water", 3),}),
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
                new MenuItem("DIC Lighter", 5),
                new MenuItem("Can of eCola", 1),
                new MenuItem("Can of Sprunk", 1),
                new MenuItem("Can of Orang-O-Tang", 1),
                new MenuItem("Bottle of PiBwasser", 3),
                new MenuItem("Bottle of Jakeys", 3),
                new MenuItem("Can of Hoplivion Double IPA", 4),
                new MenuItem("Can of Logger", 3),
                new MenuItem("Bottle of Cazafortuna Tequila", 35),
                new MenuItem("Cup of Coffee", 2),
                new MenuItem("Bottle of Raine Water", 2),
                new MenuItem("Bottle of GREY Water", 3),}),
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
                new MenuItem("Bottle of GREY Water", 3),}),
        });
    }
    private void SpecificHotels()
    {
        ShopMenuList.AddRange(new List<ShopMenu>{
            new ShopMenu("ViceroyMenu","Viceroy",new List<MenuItem>() {
                new MenuItem("City View King",354),
                new MenuItem("City View Deluxe King", 378),
                new MenuItem("Partial Ocean View King", 392),
                new MenuItem("Ocean View King", 423),
                new MenuItem("City View Two Bedded Room", 456),
                new MenuItem("Grande King", 534),
                new MenuItem("Grande Ocean View King", 647),
                new MenuItem("Empire Suite", 994),
                new MenuItem("Monarch Suite", 1327), }),
        });
    }
    private void SpecificDealerships()
    {
        ShopMenuList.AddRange(new List<ShopMenu>{
            new ShopMenu("BenefactorGallavanterMenu","Benefactor/Gallavanter",new List<MenuItem>() {
                new MenuItem("Gallivanter Baller",67000,45000),
                new MenuItem("Gallivanter Baller 2",90000,56000),
                new MenuItem("Gallivanter Baller LE",149000,76000),
                new MenuItem("Gallivanter Baller LE LWB",247000,125000),
                new MenuItem("Benefactor Schafter",65000,34000),
                new MenuItem("Benefactor Schafter LWB",75000,52000),
                new MenuItem("Benefactor Schafter V12",112000,81000),

                new MenuItem("Benefactor Feltzer",145000,90500),
                new MenuItem("Benefactor Schwartzer",48000,27000),
                new MenuItem("Benefactor Surano",110000,78000),
                new MenuItem("Benefactor Serrano",60000,45000),
                new MenuItem("Benefactor Dubsta",110000,78000),
                new MenuItem("Benefactor Dubsta 2",120000,89000),
                new MenuItem("Benefactor XLS",151000,11000),
                new MenuItem("Benefactor Streiter",156000,105000),
                new MenuItem("Benefactor Schlagen GT",500000,250000),
                new MenuItem("Benefactor Krieger",750000,500000),
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
                new MenuItem("Vapid Pisswasser Dominator",65000,45000),
                new MenuItem("Vapid Dominator ASP",75000,50000),
                new MenuItem("Vapid Dominator GTT",95000,70000),
                new MenuItem("Vapid Dominator GTX",105000,82000),
                new MenuItem("Vapid Flash GT",65000,47000),
                new MenuItem("Vapid FMJ",75000,58000),
                new MenuItem("Vapid Bullet",155000,105050),
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
                new MenuItem("Shitzu Hakuchou",45000,22340),
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
                new MenuItem("Pfister Comet",100000,78000),
                new MenuItem("Pfister Comet Retro Custom",13000,12000),
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
            }),
            new ShopMenu("AlbanyMenu","Albany",new List<MenuItem>() {
                new MenuItem("Albany Alpha",80000),
                new MenuItem("Albany Roosevelt",70000),
                new MenuItem("Albany Fränken Stange",70000),
                new MenuItem("Albany Roosevelt Valor",70000),
                new MenuItem("Albany Buccaneer",29000),
                new MenuItem("Albany Buccaneer Custom",150000),
                new MenuItem("Albany Cavalcade",50000),
                new MenuItem("Albany Cavalcade 2",50000),
                new MenuItem("Albany Emperor",25000),
                new MenuItem("Albany Emperor 2",25000),
                new MenuItem("Albany Emperor 3",25000),
                new MenuItem("Albany Hermes",25000),
                new MenuItem("Albany Lurcher",150000),
                new MenuItem("Albany Manana",25000),
                new MenuItem("Albany Manana Custom",25000),
                new MenuItem("Albany Primo",35000),
                new MenuItem("Albany Primo Custom",35000),
                new MenuItem("Albany Virgo",150000),
                new MenuItem("Albany V-STR",150000),
                new MenuItem("Albany Washington",35000),
            }),
            new ShopMenu("LarrysRVMenu","Larry's RV",new List<MenuItem>() {
                new MenuItem("Zirconium Journey",25000, 15000),
                new MenuItem("Declasse Burrito",35000, 25000),
                new MenuItem("BF Surfer",65000, 45000),
                new MenuItem("Brute Camper",145000, 95000),
            }),
        });
    }
    private void SpecificVendingMachines()
    {
        ShopMenuList.AddRange(new List<ShopMenu>{
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
                new MenuItem("Bottle of Raine Water", 2), }) { BannerOverride = "sprunk.png" },
            new ShopMenu("eColaVendingMenu","eCola Machine",new List<MenuItem>() {
                new MenuItem("Can of Sprunk", 1),
                new MenuItem("Can of eCola", 1),
                new MenuItem("Can of Orang-O-Tang", 1),
                new MenuItem("Can of Squirter", 1),
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
        ShopMenuList.AddRange(new List<ShopMenu>{
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
        });
    }
    private void DrugDealerMenus()
    {
        ShopMenuList.AddRange(new List<ShopMenu>() {
        new ShopMenu("DealerMenu", "Marijuana Dealer 1", new List<MenuItem>() {
                    new MenuItem("Marijuana",20, 12) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 20, NumberOfItemsToSellToPlayer = 20  }}, "MarijuanaDealerMenu"),
            new ShopMenu("DealerMenu", "Marijuana Dealer 2", new List<MenuItem>() {
                    new MenuItem("Marijuana", 19, 11) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 15, NumberOfItemsToSellToPlayer = 5 } }, "MarijuanaDealerMenu"),
            new ShopMenu("DealerMenu", "Marijuana Dealer 3", new List<MenuItem>() {
                    new MenuItem("Marijuana", 18, 10) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 18, NumberOfItemsToSellToPlayer = 7 } }, "MarijuanaDealerMenu"),
            new ShopMenu("DealerMenu", "Marijuana Dealer 4", new List<MenuItem>() {
                    new MenuItem("Marijuana", 17, 12) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 15, NumberOfItemsToSellToPlayer = 9 } }, "MarijuanaDealerMenu"),
            new ShopMenu("DealerMenu", "Marijuana Dealer 5", new List<MenuItem>() {
                    new MenuItem("Marijuana", 16, 11) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 12, NumberOfItemsToSellToPlayer = 15 } }, "MarijuanaDealerMenu"),
            new ShopMenu("DealerMenu", "Marijuana Dealer 6", new List<MenuItem>() {
                    new MenuItem("Marijuana",15, 10) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 5, NumberOfItemsToSellToPlayer = 16 } }, "MarijuanaDealerMenu"),
            new ShopMenu("DealerMenu", "Marijuana Dealer 7", new List<MenuItem>() {
                    new MenuItem("Marijuana",14, 12) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 19, NumberOfItemsToSellToPlayer = 15 } }, "MarijuanaDealerMenu"),
            new ShopMenu("DealerMenu", "Marijuana Dealer 8", new List<MenuItem>() {
                    new MenuItem("Marijuana",13, 11) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 22, NumberOfItemsToSellToPlayer = 15 } }, "MarijuanaDealerMenu"),
            new ShopMenu("DealerMenu", "Marijuana Dealer 9", new List<MenuItem>() {
                    new MenuItem("Marijuana",20, 10) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 34, NumberOfItemsToSellToPlayer = 22 } }, "MarijuanaDealerMenu"),
            new ShopMenu("DealerMenu", "Marijuana Dealer 10", new List<MenuItem>() {
                    new MenuItem("Marijuana",19, 12) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 10, NumberOfItemsToSellToPlayer = 9 } }, "MarijuanaDealerMenu"),
            new ShopMenu("DealerMenu", "Marijuana Dealer 11", new List<MenuItem>() {
                    new MenuItem("Marijuana",18, 11) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 12, NumberOfItemsToSellToPlayer = 13 } }, "MarijuanaDealerMenu"),
            new ShopMenu("DealerMenu", "Marijuana Dealer 12", new List<MenuItem>() {
                    new MenuItem("Marijuana",17, 10) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 15, NumberOfItemsToSellToPlayer = 12 } }, "MarijuanaDealerMenu"),
            new ShopMenu("DealerMenu", "Marijuana Dealer 13", new List<MenuItem>() {
                    new MenuItem("Marijuana",16, 12) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 10, NumberOfItemsToSellToPlayer = 19 } }, "MarijuanaDealerMenu"),
            new ShopMenu("DealerMenu", "Marijuana Dealer 14", new List<MenuItem>() {
                    new MenuItem("Marijuana",15, 11) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 11, NumberOfItemsToSellToPlayer = 11 }}, "MarijuanaDealerMenu"),
            new ShopMenu("DealerMenu", "Marijuana Dealer 15", new List<MenuItem>() {
                    new MenuItem("Marijuana",14, 10) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 12, NumberOfItemsToSellToPlayer = 13 }}, "MarijuanaDealerMenu"),

            new ShopMenu("DealerMenu", "Toilet Dealer 1", new List<MenuItem>() {
                    new MenuItem("Toilet Cleaner",27, 17) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 12, NumberOfItemsToSellToPlayer = 12 } }, "ToiletCleanerDealerMenu"),
            new ShopMenu("DealerMenu", "Toilet Dealer 2", new List<MenuItem>() {
                    new MenuItem("Toilet Cleaner",26, 18) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 2, NumberOfItemsToSellToPlayer = 15 } }, "ToiletCleanerDealerMenu"),
            new ShopMenu("DealerMenu", "Toilet Dealer 3", new List<MenuItem>() {
                    new MenuItem("Toilet Cleaner",25, 16) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 5, NumberOfItemsToSellToPlayer = 11 },
                    new MenuItem("Shrewsbury Luzi",956) { IsIllicilt = true }, }, "ToiletCleanerDealerMenu"),
            new ShopMenu("DealerMenu", "Toilet Dealer 4", new List<MenuItem>() {
                    new MenuItem("Toilet Cleaner",24, 16) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 4, NumberOfItemsToSellToPlayer = 16 },}, "ToiletCleanerDealerMenu"),
            new ShopMenu("DealerMenu", "Toilet Dealer 53", new List<MenuItem>() {
                    new MenuItem("Toilet Cleaner",23, 18) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 10, NumberOfItemsToSellToPlayer = 7 },
                    new MenuItem("Shrewsbury Defender",1200) { IsIllicilt = true },}, "ToiletCleanerDealerMenu"),

            new ShopMenu("DealerMenu", "SPANK Dealer 1", new List<MenuItem>() {
                    new MenuItem("SPANK", 55, 25) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 8, NumberOfItemsToSellToPlayer = 13 } }, "SPANKDealerMenu"),
            new ShopMenu("DealerMenu", "SPANK Dealer 2", new List<MenuItem>() {
                    new MenuItem("SPANK", 52, 25) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 10, NumberOfItemsToSellToPlayer = 14 } }, "SPANKDealerMenu"),
            new ShopMenu("DealerMenu", "SPANK Dealer 3", new List<MenuItem>() {
                    new MenuItem("SPANK", 51, 20) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 6, NumberOfItemsToSellToPlayer = 15 },
                    new MenuItem("Toto 12 Guage Sawed-Off",430) { IsIllicilt = true },
                    new MenuItem("Combat Knife",120) { IsIllicilt = true },
                    new MenuItem("Hawk & Little Desert Slug",950) { IsIllicilt = true },}, "SPANKDealerMenu"),
            new ShopMenu("DealerMenu", "SPANK Dealer 4", new List<MenuItem>() {
                    new MenuItem("SPANK", 50, 25) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 8, NumberOfItemsToSellToPlayer = 9 },
                    new MenuItem("Vom Feuer KEK-9",565) { IsIllicilt = true },}, "SPANKDealerMenu"),
            new ShopMenu("DealerMenu", "SPANK Dealer 5", new List<MenuItem>() {
                    new MenuItem("SPANK", 48, 20) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 5, NumberOfItemsToSellToPlayer = 12 },
                    new MenuItem("Shrewsbury A7-4K",856) { IsIllicilt = true } }, "SPANKDealerMenu"),

            new ShopMenu("DealerMenu", "Meth Dealer 1", new List<MenuItem>() {
                    new MenuItem("Methamphetamine", 65, 40) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 10, NumberOfItemsToSellToPlayer = 12 } }, "MethamphetamineDealerMenu"),
            new ShopMenu("DealerMenu", "Meth Dealer 2", new List<MenuItem>() {
                    new MenuItem("Methamphetamine", 55, 38) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 12, NumberOfItemsToSellToPlayer = 13 } }, "MethamphetamineDealerMenu"),
            new ShopMenu("DealerMenu", "Meth Dealer 3", new List<MenuItem>() {
                    new MenuItem("Methamphetamine", 60, 36) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 15, NumberOfItemsToSellToPlayer = 12 },
                    new MenuItem("Hawk & Little PTF092F",200) { IsIllicilt = true },
                    new MenuItem("Switchblade",300) { IsIllicilt = true },}, "MethamphetamineDealerMenu"),
            new ShopMenu("DealerMenu", "Meth Dealer 4", new List<MenuItem>() {
                    new MenuItem("Methamphetamine", 64, 35) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 5, NumberOfItemsToSellToPlayer = 20 },
                    new MenuItem("Vom Feuer KEK-9",565) { IsIllicilt = true },}, "MethamphetamineDealerMenu"),
            new ShopMenu("DealerMenu", "Meth Dealer 5", new List<MenuItem>() {
                    new MenuItem("Methamphetamine", 62, 36) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 20, NumberOfItemsToSellToPlayer = 13 },
                    new MenuItem("Shrewsbury A7-4K",856) { IsIllicilt = true } }, "MethamphetamineDealerMenu"),

            new ShopMenu("DealerMenu", "Heroin Dealer 1", new List<MenuItem>() {
                    new MenuItem("Heroin", 150, 110) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 6, NumberOfItemsToSellToPlayer = 12 } }, "HeroinDealerMenu"),
            new ShopMenu("DealerMenu", "Heroin Dealer 2", new List<MenuItem>() {
                    new MenuItem("Heroin", 156, 108) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 7, NumberOfItemsToSellToPlayer = 8 } }, "HeroinDealerMenu"),
            new ShopMenu("DealerMenu", "Heroin Dealer 3", new List<MenuItem>() {
                    new MenuItem("Heroin", 160, 101) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 9, NumberOfItemsToSellToPlayer = 14 },
                    new MenuItem("Combat Knife",120) { IsIllicilt = true },
                    new MenuItem("Switchblade",300) { IsIllicilt = true },}, "HeroinDealerMenu"),
            new ShopMenu("DealerMenu", "Heroin Dealer 4", new List<MenuItem>() {
                    new MenuItem("Heroin", 158, 99) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 11, NumberOfItemsToSellToPlayer = 16 },
                    new MenuItem("Vom Feuer KEK-9",565) { IsIllicilt = true },}, "HeroinDealerMenu"),
            new ShopMenu("DealerMenu", "Heroin Dealer 5", new List<MenuItem>() {
                    new MenuItem("Heroin", 155, 105) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 7, NumberOfItemsToSellToPlayer = 7 }, }, "HeroinDealerMenu"),

            new ShopMenu("DealerMenu", "Crack Dealer 1", new List<MenuItem>() {
                    new MenuItem("Crack", 58, 40) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 9, NumberOfItemsToSellToPlayer = 15 } }, "CrackDealerMenu"),
            new ShopMenu("DealerMenu", "Crack Dealer 2", new List<MenuItem>() {
                    new MenuItem("Crack", 48, 38) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 7, NumberOfItemsToSellToPlayer = 20 } }, "CrackDealerMenu"),
            new ShopMenu("DealerMenu", "Crack Dealer 3", new List<MenuItem>() {
                    new MenuItem("Crack", 52, 36) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 12, NumberOfItemsToSellToPlayer = 11 },
                    new MenuItem("Toto 12 Guage Sawed-Off",430) { IsIllicilt = true },
                    new MenuItem("Combat Knife",120) { IsIllicilt = true },
                    new MenuItem("Hawk & Little Desert Slug",950) { IsIllicilt = true },}, "CrackDealerMenu"),
            new ShopMenu("DealerMenu", "SPANK Dealer 4", new List<MenuItem>() {
                    new MenuItem("Crack", 53, 38) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 10, NumberOfItemsToSellToPlayer = 15 },
                    new MenuItem("Vom Feuer KEK-9",565) { IsIllicilt = true },}, "CrackDealerMenu"),
            new ShopMenu("DealerMenu", "SPANK Dealer 5", new List<MenuItem>() {
                    new MenuItem("Crack", 50, 32) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 6, NumberOfItemsToSellToPlayer = 12 },
                    new MenuItem("Shrewsbury A7-4K",856) { IsIllicilt = true } }, "CrackDealerMenu"),

            new ShopMenu("DealerMenu", "Coke Dealer 1", new List<MenuItem>() {
                    new MenuItem("Cocaine", 180, 130) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 6, NumberOfItemsToSellToPlayer = 12 } }, "CokeDealerMenu"),
            new ShopMenu("DealerMenu", "Coke Dealer 2", new List<MenuItem>() {
                    new MenuItem("Cocaine", 175, 126) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 8, NumberOfItemsToSellToPlayer = 15 } }, "CokeDealerMenu"),
            new ShopMenu("DealerMenu", "Coke Dealer 3", new List<MenuItem>() {
                    new MenuItem("Cocaine", 170, 125) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 9, NumberOfItemsToSellToPlayer = 16 },
                    new MenuItem("Toto 12 Guage Sawed-Off",430) { IsIllicilt = true },
                    new MenuItem("Combat Knife",120) { IsIllicilt = true },
                    new MenuItem("Switchblade",300) { IsIllicilt = true },}, "CokeDealerMenu"),
            new ShopMenu("DealerMenu", "Coke Dealer 4", new List<MenuItem>() {
                    new MenuItem("Cocaine", 160, 120) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 12, NumberOfItemsToSellToPlayer = 8 },
                    new MenuItem("Vom Feuer KEK-9",565) { IsIllicilt = true },}, "CokeDealerMenu"),
            new ShopMenu("DealerMenu", "Coke Dealer 5", new List<MenuItem>() {
                    new MenuItem("Cocaine", 172, 128) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 10, NumberOfItemsToSellToPlayer = 10 },
                    new MenuItem("Hawk & Little PTF092F",250) { IsIllicilt = true } }, "CokeDealerMenu"),


            new ShopMenu("DrugCustomerMenu", "Marijuana Customer 1", new List<MenuItem>() {
                    new MenuItem("Marijuana",0, 35) { IsIllicilt = true,NumberOfItemsToPurchaseFromPlayer = 3 }}),
            new ShopMenu("DrugCustomerMenu", "Marijuana Customer 2", new List<MenuItem>() {
                    new MenuItem("Marijuana",0, 32) { IsIllicilt = true, NumberOfItemsToPurchaseFromPlayer = 4 }}),
            new ShopMenu("DrugCustomerMenu", "Marijuana Customer 3", new List<MenuItem>() {
                    new MenuItem("Marijuana",0, 30) { IsIllicilt = true, NumberOfItemsToPurchaseFromPlayer = 4 }}),
            new ShopMenu("DrugCustomerMenu", "Marijuana Customer 4", new List<MenuItem>() {
                    new MenuItem("Marijuana",0, 34) { IsIllicilt = true, NumberOfItemsToPurchaseFromPlayer = 2 }}),
            new ShopMenu("DrugCustomerMenu", "Marijuana Customer 5", new List<MenuItem>() {
                    new MenuItem("Marijuana",0, 33) { IsIllicilt = true, NumberOfItemsToPurchaseFromPlayer = 3 },}),
            new ShopMenu("DrugCustomerMenu", "Marijuana Customer 6", new List<MenuItem>() {
                    new MenuItem("Marijuana",0, 31) { IsIllicilt = true, NumberOfItemsToPurchaseFromPlayer = 5 },}),

            new ShopMenu("DrugCustomerMenu", "Toilet Customer 1", new List<MenuItem>() {
                    new MenuItem("Toilet Cleaner",0, 45) { IsIllicilt = true, NumberOfItemsToPurchaseFromPlayer = 3 } }),
            new ShopMenu("DrugCustomerMenu", "Toilet Customer 2", new List<MenuItem>() {
                    new MenuItem("Toilet Cleaner",0, 42) { IsIllicilt = true, NumberOfItemsToPurchaseFromPlayer = 2 } }),
            new ShopMenu("DrugCustomerMenu", "Toilet Customer 3", new List<MenuItem>() {
                    new MenuItem("Toilet Cleaner",0, 38) { IsIllicilt = true, NumberOfItemsToPurchaseFromPlayer = 1 } }),
            new ShopMenu("DrugCustomerMenu", "Toilet Customer 4", new List<MenuItem>() {
                    new MenuItem("Toilet Cleaner",0, 39) { IsIllicilt = true, NumberOfItemsToPurchaseFromPlayer = 3 } }),

            new ShopMenu("DrugCustomerMenu", "SPANK Customer 1", new List<MenuItem>() {
                    new MenuItem("SPANK", 0, 62) { IsIllicilt = true, NumberOfItemsToPurchaseFromPlayer = 4 } }),
            new ShopMenu("DrugCustomerMenu", "SPANK Customer 2", new List<MenuItem>() {
                    new MenuItem("SPANK", 0, 67) { IsIllicilt = true, NumberOfItemsToPurchaseFromPlayer = 3 } }),
            new ShopMenu("DrugCustomerMenu", "SPANK Customer 3", new List<MenuItem>() {
                    new MenuItem("SPANK", 0, 65) { IsIllicilt = true, NumberOfItemsToPurchaseFromPlayer = 2 } }),
            new ShopMenu("DrugCustomerMenu", "SPANK Customer 4", new List<MenuItem>() {
                    new MenuItem("SPANK", 0, 70) { IsIllicilt = true, NumberOfItemsToPurchaseFromPlayer = 1 } }),

            new ShopMenu("DrugCustomerMenu", "Meth Customer 1", new List<MenuItem>() {
                    new MenuItem("Methamphetamine", 0, 85) { IsIllicilt = true, NumberOfItemsToPurchaseFromPlayer = 3 } }),
            new ShopMenu("DrugCustomerMenu", "Meth Customer 2", new List<MenuItem>() {
                    new MenuItem("Methamphetamine", 0, 80) { IsIllicilt = true, NumberOfItemsToPurchaseFromPlayer = 2 } }),
            new ShopMenu("DrugCustomerMenu", "Meth Customer 3", new List<MenuItem>() {
                    new MenuItem("Methamphetamine", 0, 75) { IsIllicilt = true, NumberOfItemsToPurchaseFromPlayer = 3 } }),
            new ShopMenu("DrugCustomerMenu", "Meth Customer 4", new List<MenuItem>() {
                    new MenuItem("Methamphetamine", 0, 77) { IsIllicilt = true, NumberOfItemsToPurchaseFromPlayer = 5} }),

            new ShopMenu("DrugCustomerMenu", "Crack Customer 1", new List<MenuItem>() {
                    new MenuItem("Crack", 0, 70) { IsIllicilt = true, NumberOfItemsToPurchaseFromPlayer = 4 } }),
            new ShopMenu("DrugCustomerMenu", "Crack Customer 2", new List<MenuItem>() {
                    new MenuItem("Crack", 0, 68) { IsIllicilt = true, NumberOfItemsToPurchaseFromPlayer = 1 } }),
            new ShopMenu("DrugCustomerMenu", "Crack Customer 3", new List<MenuItem>() {
                    new MenuItem("Crack", 0, 65) { IsIllicilt = true, NumberOfItemsToPurchaseFromPlayer = 3 } }),
            new ShopMenu("DrugCustomerMenu", "Crack Customer 4", new List<MenuItem>() {
                    new MenuItem("Crack", 0, 66) { IsIllicilt = true, NumberOfItemsToPurchaseFromPlayer = 5 } }),

            new ShopMenu("DrugCustomerMenu", "Coke Customer 1", new List<MenuItem>() {
                    new MenuItem("Cocaine", 0, 210) { IsIllicilt = true, NumberOfItemsToPurchaseFromPlayer = 3 } }),
            new ShopMenu("DrugCustomerMenu", "Coke Customer 2", new List<MenuItem>() {
                    new MenuItem("Cocaine", 0, 202) { IsIllicilt = true, NumberOfItemsToPurchaseFromPlayer = 2 } }),
            new ShopMenu("DrugCustomerMenu", "Coke Customer 3", new List<MenuItem>() {
                    new MenuItem("Cocaine", 0, 199) { IsIllicilt = true, NumberOfItemsToPurchaseFromPlayer = 3 } }),
            new ShopMenu("DrugCustomerMenu", "Coke Customer 4", new List<MenuItem>() {
                    new MenuItem("Cocaine", 0, 208) { IsIllicilt = true, NumberOfItemsToPurchaseFromPlayer = 4 } }),

            new ShopMenu("DrugCustomerMenu", "Heroin Customer 1", new List<MenuItem>() {
                    new MenuItem("Heroin", 0, 180) { IsIllicilt = true, NumberOfItemsToPurchaseFromPlayer = 3 } }),
            new ShopMenu("DrugCustomerMenu", "Heroin Customer 2", new List<MenuItem>() {
                    new MenuItem("Heroin", 0, 178) { IsIllicilt = true, NumberOfItemsToPurchaseFromPlayer = 1 } }),
            new ShopMenu("DrugCustomerMenu", "Heroin Customer 3", new List<MenuItem>() {
                    new MenuItem("Heroin", 0, 175) { IsIllicilt = true, NumberOfItemsToPurchaseFromPlayer = 3 } }),
            new ShopMenu("DrugCustomerMenu", "Heroin Customer 4", new List<MenuItem>() {
                    new MenuItem("Heroin", 0, 168) { IsIllicilt = true, NumberOfItemsToPurchaseFromPlayer = 2 } }),

            new ShopMenu("GunVendorMenu", "Gun Vendor", new List<MenuItem>() {
                new MenuItem("Shrewsbury 420 Sawed-Off",340) { IsIllicilt = true },
                new MenuItem("Hawk & Little PTF092F",680) { IsIllicilt = true },
                new MenuItem("Shrewsbury Defender",1200) { IsIllicilt = true },
                new MenuItem("Toto 12 Guage Sawed-Off",430) { IsIllicilt = true },
                new MenuItem("M61 Grenade",1000) { IsIllicilt = true },
                new MenuItem("Baseball Bat",70) { IsIllicilt = true },
                new MenuItem("Crowbar",35) { IsIllicilt = true },
                new MenuItem("Golf Club",150) { IsIllicilt = true },
                new MenuItem("Hammer 2",25) { IsIllicilt = true },
                new MenuItem("Hatchet",80) { IsIllicilt = true },
                new MenuItem("Brass Knuckles",200) { IsIllicilt = true },
                new MenuItem("Combat Knife",120) { IsIllicilt = true },
                new MenuItem("Machete",29) { IsIllicilt = true },
                new MenuItem("Switchblade",300) { IsIllicilt = true },
                new MenuItem("Shrewsbury Luzi",956) { IsIllicilt = true },
                new MenuItem("Vom Feuer KEK-9",565) { IsIllicilt = true },
                new MenuItem("Shrewsbury A7-4K",856) { IsIllicilt = true }
            }),
            });
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
        ShopMenuList.AddRange(new List<ShopMenu> { 

                new ShopMenu("GenericGangDenMenu","GenericGangDenMenu",new List<MenuItem>() {
                    new MenuItem("Marijuana",16,12),
                    new MenuItem("Toilet Cleaner",22, 18) ,
                    new MenuItem("SPANK", 45, 20),
                    new MenuItem("Brass Knuckles",175),
                    new MenuItem("Combat Knife",150),
                    new MenuItem("Machete",45),
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
        ShopMenuList.AddRange(new List<ShopMenu> {
               new ShopMenu("LostDenMenu","LostDenMenu",new List<MenuItem>() {


                   new MenuItem("Methamphetamine",50, 40),


                    new MenuItem("Baseball Bat",45),
                    new MenuItem("Crowbar",30),
                    new MenuItem("Golf Club",100),
                    new MenuItem("Hammer",20),
                    new MenuItem("Hatchet",75),
                    new MenuItem("Brass Knuckles",100),
                    new MenuItem("Combat Knife",100),
                    new MenuItem("Machete",20),
                    new MenuItem("Switchblade",45),
                    new MenuItem("Nightstick",45),
                    new MenuItem("Wrench",20),
                    new MenuItem("Pool Cue",30),

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
               // new MenuItem("Tear Gas Grenade",125) { AmmoAmount = 1,AmmoPrice = 125 },
                    }),
        });

    }
    private void FamiliesDenMenu()
    {
        ShopMenuList.AddRange(new List<ShopMenu> {
               new ShopMenu("FamiliesDenMenu","FamiliesDenMenu",new List<MenuItem>() {
                    new MenuItem("Marijuana", 15, 12),
                    new MenuItem("Toilet Cleaner",20, 16) ,
                    new MenuItem("SPANK", 40, 25),
                    new MenuItem("Hatchet",80),
                    new MenuItem("Brass Knuckles",150),
                    new MenuItem("Combat Knife",120),
                    new MenuItem("Machete",29),
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
    private void VagosDenMenu()
    {
        ShopMenuList.AddRange(new List<ShopMenu> {
               new ShopMenu("VagosDenMenu","VagosDenMenu",new List<MenuItem>() {
                    new MenuItem("Marijuana",15, 10),
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
        ShopMenuList.AddRange(new List<ShopMenu> {
               new ShopMenu("BallasDenMenu","BallasDenMenu",new List<MenuItem>() {
                    new MenuItem("Crack",46, 40),
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
        ShopMenuList.AddRange(new List<ShopMenu> {
               new ShopMenu("VarriosDenMenu","VarriosDenMenu",new List<MenuItem>() {
                    new MenuItem("Crack",45, 41),
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
        ShopMenuList.AddRange(new List<ShopMenu> {
                new ShopMenu("MarabunteDenMenu", "MarabunteDenMenu", new List<MenuItem>() {
                    new MenuItem("Marijuana",14, 11),
                    new MenuItem("Brass Knuckles",175),
                    new MenuItem("Combat Knife",150),
                    new MenuItem("Machete",45),
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
    private void TriadsDenMenu()
    {
        ShopMenuList.AddRange(new List<ShopMenu> {
                new ShopMenu("TriadsDenMenu", "TriadsDenMenu", new List<MenuItem>() {
                    new MenuItem("Heroin",130, 100),
                    new MenuItem("Brass Knuckles",175),
                    new MenuItem("Combat Knife",150),
                    new MenuItem("Machete",45),
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
    private void KkangpaeDenMenu()
    {
        ShopMenuList.AddRange(new List<ShopMenu> {
                new ShopMenu("KkangpaeDenMenu", "KkangpaeDenMenu", new List<MenuItem>() {
                    new MenuItem("Heroin",125, 110),
                    new MenuItem("Brass Knuckles",175),
                    new MenuItem("Combat Knife",150),
                    new MenuItem("Machete",45),
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
    private void DiablosDenMenu()
    {
        ShopMenuList.AddRange(new List<ShopMenu> {
               new ShopMenu("DiablosDenMenu","DiablosDenMenu",new List<MenuItem>() {
                    new MenuItem("SPANK", 45, 20),
                    new MenuItem("Hatchet",80),
                    new MenuItem("Brass Knuckles",150),
                    new MenuItem("Combat Knife",120),
                    new MenuItem("Machete",29),
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
    private void YardiesDenMenu()
    {
        ShopMenuList.AddRange(new List<ShopMenu> {
               new ShopMenu("YardiesDenMenu","YardiesDenMenu",new List<MenuItem>() {
                    new MenuItem("Marijuana", 13, 10),
                    new MenuItem("Hatchet",80),
                    new MenuItem("Brass Knuckles",150),
                    new MenuItem("Combat Knife",120),
                    new MenuItem("Machete",29),
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
    private void ArmenianDenMenu()
    {
        ShopMenuList.AddRange(new List<ShopMenu> {
               new ShopMenu("ArmenianDenMenu","ArmenianDenMenu",new List<MenuItem>() {
                    new MenuItem("Heroin",135, 90),
                    new MenuItem("Hatchet",80),
                    new MenuItem("Brass Knuckles",150),
                    new MenuItem("Combat Knife",120),
                    new MenuItem("Machete",29),
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
    private void MadrazoDenMenu()
    {
        ShopMenuList.AddRange(new List<ShopMenu> {
               new ShopMenu("MadrazoDenMenu","MadrazoDenMenu",new List<MenuItem>() {
                    new MenuItem("Methamphetamine",45, 40),
                    new MenuItem("Hatchet",80),
                    new MenuItem("Brass Knuckles",150),
                    new MenuItem("Combat Knife",120),
                    new MenuItem("Machete",29),
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
        ShopMenuList.AddRange(new List<ShopMenu> {
                new ShopMenu("GambettiDenMenu", "GambettiDenMenu", new List<MenuItem>() {
                    new MenuItem("Cocaine",150, 130),
                    new MenuItem("Brass Knuckles",175),
                    new MenuItem("Combat Knife",150),
                    new MenuItem("Machete",45),
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
    private void PavanoDenMenu()
    {
        ShopMenuList.AddRange(new List<ShopMenu> {
                new ShopMenu("PavanoDenMenu", "PavanoDenMenu", new List<MenuItem>() {
                    new MenuItem("Cocaine",150, 130),
                    new MenuItem("Brass Knuckles",175),
                    new MenuItem("Combat Knife",150),
                    new MenuItem("Machete",45),
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
    private void LupisellaDenMenu()
    {
        ShopMenuList.AddRange(new List<ShopMenu> {
                new ShopMenu("LupisellaDenMenu", "LupisellaDenMenu", new List<MenuItem>() {
                    new MenuItem("Cocaine",150, 130),
                    new MenuItem("Brass Knuckles",175),
                    new MenuItem("Combat Knife",150),
                    new MenuItem("Machete",45),
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
    private void MessinaDenMenu()
    {
        ShopMenuList.AddRange(new List<ShopMenu> {
                new ShopMenu("MessinaDenMenu", "MessinaDenMenu", new List<MenuItem>() {
                    new MenuItem("Cocaine",150, 130),
                    new MenuItem("Brass Knuckles",175),
                    new MenuItem("Combat Knife",150),
                    new MenuItem("Machete",45),
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
    private void AncelottiDenMenu()
    {
        ShopMenuList.AddRange(new List<ShopMenu> {
                new ShopMenu("AncelottiDenMenu", "AncelottiDenMenu", new List<MenuItem>() {
                    new MenuItem("Cocaine",150, 130),
                    new MenuItem("Brass Knuckles",175),
                    new MenuItem("Combat Knife",150),
                    new MenuItem("Machete",45),
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
        ShopMenuList.AddRange(new List<ShopMenu>
        {
                new ShopMenu("GunShop1","GunShop1",new List<MenuItem>() {
                    new MenuItem("Baseball Bat",70),
                    new MenuItem("Crowbar",35),
                    new MenuItem("Golf Club",150),
                    new MenuItem("Hammer",25),

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
                    }),
        });
    }
    private void GunShop2()//heavy?
    {
        ShopMenuList.AddRange(new List<ShopMenu>
        {
                new ShopMenu("GunShop2","GunShop2",new List<MenuItem>() {
                    
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
               // new MenuItem("Tear Gas Grenade",125) { AmmoAmount = 1,AmmoPrice = 125 },
                    }),
        });
    }
    private void GunShop3()//SMG
    {
        ShopMenuList.AddRange(new List<ShopMenu>
        {
                new ShopMenu("GunShop3","GunShop3",new List<MenuItem>() {
                    new MenuItem("Combat Knife",120),
                    new MenuItem("Machete",29),
                    new MenuItem("Switchblade",78),
                    new MenuItem("Nightstick",57),
                    new MenuItem("Wrench",24),
                    new MenuItem("Pool Cue",45),

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
               // new MenuItem("Tear Gas Grenade",125) { AmmoAmount = 1,AmmoPrice = 125 },
                    }),
        });
    }
    private void GunShop4()//AR
    {
        ShopMenuList.AddRange(new List<ShopMenu>
        {
                new ShopMenu("GunShop4","GunShop4",new List<MenuItem>() {


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
               // new MenuItem("Tear Gas Grenade",125) { AmmoAmount = 1,AmmoPrice = 125 },
                    }),
        });
    }
    private void GunShop5()//Pistol
    {
        ShopMenuList.AddRange(new List<ShopMenu>
        {
                new ShopMenu("GunShop5","GunShop5",new List<MenuItem>() {
                    
                    new MenuItem("Hammer",25),
                    new MenuItem("Hatchet",80),
                    new MenuItem("Machete",29),
                    new MenuItem("Switchblade",78),
                    new MenuItem("Nightstick",57),


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
               // new MenuItem("Tear Gas Grenade",125) { AmmoAmount = 1,AmmoPrice = 125 },
                    }),
        });
    }
    private void AllWeaponsMenu()
    {
        ShopMenuList.AddRange(new List<ShopMenu>
        {
        new ShopMenu("WeaponsMenu", "WeaponsMenu", new List<MenuItem>() {
                    new MenuItem("Baseball Bat",70),
                    new MenuItem("Crowbar",35),
                    new MenuItem("Golf Club",150),
                    new MenuItem("Hammer",25),
                    new MenuItem("Hatchet",80),
                    new MenuItem("Brass Knuckles",150),
                    new MenuItem("Combat Knife",120),
                    new MenuItem("Machete",29),
                    new MenuItem("Switchblade",78),
                    new MenuItem("Nightstick",57),
                    new MenuItem("Wrench",24),
                    new MenuItem("Pool Cue",45),

                //Pistola
                new MenuItem("Hawk & Little PTF092F",550) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip",0),
                    new MenuItemExtra("Extended Clip", 75),
                    new MenuItemExtra("Flashlight", 70),
                    new MenuItemExtra("Suppressor", 850) },  },
                new MenuItem("Hawk & Little Thunder",650) { Extras = new List<MenuItemExtra>() {
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
                new MenuItem("Hawk & Little Combat Pistol",950) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 85),
                    new MenuItemExtra("Flashlight", 80),
                    new MenuItemExtra("Suppressor", 890), } },
                new MenuItem("Hawk & Little Desert Slug",1500) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120),
                    new MenuItemExtra("Flashlight", 95),
                    new MenuItemExtra("Suppressor", 1100), } },
                new MenuItem("Hawk & Little 1919",1200) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 50),
                    new MenuItemExtra("Flashlight", 75),
                    new MenuItemExtra("Suppressor", 950), } },
                new MenuItem("Hawk & Little Raging Mare",1700),
                new MenuItem("Hawk & Little Raging Mare Dx",1950) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Rounds", 0),
                    new MenuItemExtra("Tracer Rounds", 500),
                    new MenuItemExtra("Incendiary Rounds", 500),
                    new MenuItemExtra("Hollow Point Rounds", 500),
                    new MenuItemExtra("Full Metal Jacket Rounds", 500),
                    new MenuItemExtra("Holographic Sight", 890),
                    new MenuItemExtra("Small Scope", 570),
                    new MenuItemExtra("Flashlight", 70), } },
                new MenuItem("Vom Feuer P69",790) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 85),
                    new MenuItemExtra("Suppressor", 890), } },
                new MenuItem("Vom Feuer SCRAMP",990) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 85),
                    new MenuItemExtra("Flashlight", 80),
                    new MenuItemExtra("Suppressor", 890), } },
                new MenuItem("Shrewsbury S7",1100) { Extras = new List<MenuItemExtra>() {
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
                new MenuItem("Coil Tesla",550),
                new MenuItem("BS M1922",995) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 85),
                    new MenuItemExtra("Suppressor", 890), } },


                //Shotgun
                new MenuItem("Shrewsbury 420 Sawed-Off",250),
                new MenuItem("Shrewsbury 420",400) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Flashlight", 95),
                    new MenuItemExtra("Suppressor", 1100), } },
                new MenuItem("Vom Feuer 569",500) { Extras = new List<MenuItemExtra>() {
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
                new MenuItem("Toto 12 Guage Sawed-Off",350),
                new MenuItem("Shrewsbury Defender",990),
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
                new MenuItem("Hawk & Little MP6",1500) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120),
                    new MenuItemExtra("Drum Magazine", 550),
                    new MenuItemExtra("Flashlight", 80),
                    new MenuItemExtra("Suppressor", 1945) } },
                new MenuItem("Hawk & Little XPM",1600) { Extras = new List<MenuItemExtra>() {
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
                new MenuItem("Vom Feuer Fisher",900) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 120),
                    new MenuItemExtra("Scope", 550),
                    new MenuItemExtra("Flashlight", 80),
                    new MenuItemExtra("Suppressor", 1945) } },
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
                new MenuItem("Shrewsbury A7-4K",650) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 125),
                    new MenuItemExtra("Drum Magazine", 225),
                    new MenuItemExtra("Flashlight", 90),
                    new MenuItemExtra("Scope", 567),
                    new MenuItemExtra("Suppressor", 800),
                    new MenuItemExtra("Grip", 200), } },
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
                new MenuItem("Vom Feuer A5-1R",700) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 125),
                    new MenuItemExtra("Box Magazine", 225),
                    new MenuItemExtra("Flashlight", 90),
                    new MenuItemExtra("Scope", 567),
                    new MenuItemExtra("Suppressor", 800),
                    new MenuItemExtra("Grip", 200), } },
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
                new MenuItem("Vom Feuer SL6",1900) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 125),
                    new MenuItemExtra("Drum Magazine", 225),
                    new MenuItemExtra("Flashlight", 90),
                    new MenuItemExtra("Scope", 567),
                    new MenuItemExtra("Suppressor", 800),
                    new MenuItemExtra("Grip", 200), } },
                new MenuItem("Vom Feuer SL6 MK2",2200) { Extras = new List<MenuItemExtra>() {
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
                new MenuItem("Hawk & Little ZBZ-23",1200) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 125),
                    new MenuItemExtra("Drum Magazine", 225),
                    new MenuItemExtra("Flashlight", 90),
                    new MenuItemExtra("Scope", 567),
                    new MenuItemExtra("Suppressor", 800),
                    new MenuItemExtra("Grip", 200), } },
                new MenuItem("Hawk & Little ZBZ-25X",1300) { Extras = new List<MenuItemExtra>() {
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
                new MenuItem("Shrewsbury PDA",4500) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 125),
                    new MenuItemExtra("Scope", 567),} },
                new MenuItem("Vom Feuer BAT",4340) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 125),
                    new MenuItemExtra("Grip", 90),
                    new MenuItemExtra("Scope", 567),} },
                new MenuItem("Vom Feuer M70E1",5000) { Extras = new List<MenuItemExtra>() {
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
                new MenuItem("Shrewsbury PWN",2500) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Scope", 890),
                    new MenuItemExtra("Advanced Scope", 1400),
                    new MenuItemExtra("Suppressor", 1900),} },
                new MenuItem("Bartlett M92",5790) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Scope", 890),
                    new MenuItemExtra("Advanced Scope", 1400),} },
                new MenuItem("Bartlett M92 Mk2",6780) { Extras = new List<MenuItemExtra>() {
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
                new MenuItem("Vom Feuer M23 DBS",1500) { Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Extended Clip", 125),
                    new MenuItemExtra("Flashlight", 90),
                    new MenuItemExtra("Scope", 567),
                    new MenuItemExtra("Suppressor", 800),
                    new MenuItemExtra("Grip", 200), } },
                new MenuItem("Vom Feuer M23 DBS Scout",1600) { Extras = new List<MenuItemExtra>() {
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
                new MenuItem("RPG-7",12550){ SubAmount = 1,SubPrice = 500 },
                new MenuItem("Hawk & Little MGL",18889){ Extras = new List<MenuItemExtra>() {
                    new MenuItemExtra("Default Clip", 0),
                    new MenuItemExtra("Flashlight", 90),
                    new MenuItemExtra("Scope", 567),
                    new MenuItemExtra("Grip", 200), },SubAmount = 6,SubPrice = 500 },
                new MenuItem("M61 Grenade",800) { SubAmount = 1,SubPrice = 800 },
                new MenuItem("Improvised Incendiary",150) { SubAmount = 1,SubPrice = 150 },
                new MenuItem("BZ Gas Grenade",250) { SubAmount = 1,SubPrice = 150 },
               // new MenuItem("Tear Gas Grenade",125) { AmmoAmount = 1,AmmoPrice = 125 },
                    }),
                });
    }
}

