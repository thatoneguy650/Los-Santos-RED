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
        if (File.Exists(ConfigFileName))
        {
            ShopMenuList = Serialization.DeserializeParams<ShopMenu>(ConfigFileName);
        }
        else
        {
            DefaultConfig();
            Serialization.SerializeParams(ShopMenuList, ConfigFileName);
        }
    }
    public ShopMenu GetMenu(string menuID)
    {
        return ShopMenuList.Where(x => x.ID == menuID).FirstOrDefault();
    }
    public ShopMenu GetRanomdDrugMenu()
    {
        ShopMenu Possible = ShopMenuList.Where(x => x.ID == "WeedDealerMenu").FirstOrDefault();
        if(Possible != null)
        {
            Possible.Randomize();
        }
        return Possible;
    }
    private void DefaultConfig()
    {
        ShopMenuList = new List<ShopMenu>()
        {
            //Generic
            new ShopMenu("ToolMenu","Tools",new List<MenuItem>() { new MenuItem("Screwdriver",19),new MenuItem("Hammer", 15),new MenuItem("Drill", 50),new MenuItem("Pliers", 20),new MenuItem("Shovel", 60),new MenuItem("Wrench", 24),}),
            new ShopMenu("CheapHotelMenu","Cheap Hotel",new List<MenuItem>() { new MenuItem("Room: Single Twin",99),new MenuItem("Room: Single Queen", 130),new MenuItem("Room: Double Queen", 150),new MenuItem("Room: Single King", 160), }),
            new ShopMenu("ExpensiveHotelMenu","Expensive Hotel",new List<MenuItem>() { new MenuItem("Room: Single Queen", 189),new MenuItem("Room: Double Queen", 220),new MenuItem("Room: Single King", 250),new MenuItem("Room: Delux", 280), }),
            new ShopMenu("HookerMenu","Hooker",new List<MenuItem>() { new MenuItem("Handy", 50),new MenuItem("Head", 75),new MenuItem("Half And Half", 150),new MenuItem("Full",200),}),
            new ShopMenu("ConvenienceStoreMenu","Convenience Store",new List<MenuItem>() { new MenuItem("Hot Dog", 5),new MenuItem("Burger",3),new MenuItem("Phat Chips", 2),new MenuItem("Donut", 1),new MenuItem("Redwood Regular", 30),new MenuItem("Redwood Mild", 32),new MenuItem("Debonaire", 35)
                ,new MenuItem("Debonaire Menthol", 38),new MenuItem("Caradique", 35),new MenuItem("69 Brand", 40),new MenuItem("Estancia Cigar", 50),new MenuItem("Lighter", 5),new MenuItem("Can of eCola", 1),new MenuItem("Can of Sprunk", 1),new MenuItem("Bottle of PiBwasser", 3),new MenuItem("Bottle of Jakeys", 3)
                ,new MenuItem("Cup of Coffee", 2),new MenuItem("Bottle of Raine Water", 2) }),
            new ShopMenu("SandwichMenu","Sanwiches",new List<MenuItem>() { new MenuItem("Ham and Cheese Sandwich", 2),new MenuItem("Turkey Sandwich", 2),new MenuItem("Tuna Sandwich", 2),new MenuItem("Phat Chips", 2),new MenuItem("Bottle of Raine Water", 2),new MenuItem("Can of eCola", 1),new MenuItem("Can of Sprunk", 1) }),
            new ShopMenu("HeadShopMenu","Head Shop",new List<MenuItem>() { new MenuItem("Joint", 25),new MenuItem("Redwood Regular", 30),new MenuItem("Redwood Mild", 32),new MenuItem("Debonaire", 35),new MenuItem("Debonaire Menthol", 38),new MenuItem("Caradique", 35),new MenuItem("69 Brand", 40),new MenuItem("Estancia Cigar", 50)
                ,new MenuItem("Lighter", 5),new MenuItem("Bottle of Raine Water", 2) }),
            new ShopMenu("LiquorStoreMenu","Liquor Store",new List<MenuItem>() { new MenuItem("40 oz", 5),new MenuItem("Bottle of Barracho", 3),new MenuItem("Bottle of PiBwasser", 3),new MenuItem("Bottle of Blarneys", 3),new MenuItem("Bottle of Logger", 3,1),new MenuItem("Bottle of Patriot", 3,1),new MenuItem("Bottle of Pride", 3)
                ,new MenuItem("Bottle of Stronz", 4),new MenuItem("Bottle of A.M.", 4),new MenuItem("Bottle of Jakeys", 4),new MenuItem("Bottle of Dusche", 4) }),
            new ShopMenu("BarMenu","Bar",new List<MenuItem>() { new MenuItem("Burger", 5),new MenuItem("Hot Dog", 5),new MenuItem("Bottle of Raine Water", 2),new MenuItem("Cup of eCola", 2),new MenuItem("40 oz", 5),new MenuItem("Bottle of Barracho", 4),new MenuItem("Bottle of PiBwasser", 4),new MenuItem("Bottle of Blarneys", 5)
                ,new MenuItem("Bottle of Logger", 5),new MenuItem("Bottle of Patriot", 5),new MenuItem("Bottle of Pride", 4),new MenuItem("Bottle of Stronz", 5),new MenuItem("Bottle of A.M.", 4),new MenuItem("Bottle of Jakeys", 5),new MenuItem("Bottle of Dusche", 5) }),
            new ShopMenu("CoffeeMenu","Coffee",new List<MenuItem>() {new MenuItem("Cup of Coffee", 2),new MenuItem("Donut", 5),new MenuItem("Bottle of Raine Water", 2) }),
            new ShopMenu("GenericMenu","Generic",new List<MenuItem>() {new MenuItem("Burger",3),new MenuItem("Phat Chips", 2),new MenuItem("Can of eCola", 1),new MenuItem("Can of Sprunk", 1),new MenuItem("Bottle of Raine Water", 2) }),
            new ShopMenu("PizzaMenu","Pizza",new List<MenuItem>() { new MenuItem("Slice of Pizza", 3),new MenuItem("Cup of Sprunk", 2),new MenuItem("Bottle of A.M.", 3),new MenuItem("Bottle of PiBwasser", 3),new MenuItem("Bottle of Barracho", 4),new MenuItem("Bottle of Blarneys", 4),new MenuItem("Bottle of Jakeys", 3)
                ,new MenuItem("Bottle of Stronz", 4),new MenuItem("Bottle of Dusche", 3) }),
            new ShopMenu("DonutMenu","Donut",new List<MenuItem>() {new MenuItem("Hot Dog", 5),new MenuItem("Phat Chips", 2),new MenuItem("Donut", 1),new MenuItem("Can of eCola", 1),new MenuItem("Cup of eCola", 2),new MenuItem("Cup of Coffee", 3) }),
            new ShopMenu("StoreMenu","Store",new List<MenuItem>() { new MenuItem("Joint", 25),new MenuItem("Redwood Regular", 30),new MenuItem("Redwood Mild", 32),new MenuItem("Debonaire", 35),new MenuItem("Estancia Cigar", 50),new MenuItem("Cup of Sprunk", 2),new MenuItem("Banana", 3),new MenuItem("Donut", 1),new MenuItem("Hot Pretzel", 2),new MenuItem("40 oz", 5),new MenuItem("Bottle of Barracho",3),new MenuItem("Bottle of PiBwasser", 3),new MenuItem("Bottle of Blarneys",3),new MenuItem("Bottle of Logger", 4),new MenuItem("Bottle of Patriot",4),new MenuItem("Bottle of Pride", 3),new MenuItem("Bottle of Stronz", 4) }),
            new ShopMenu("FruitMenu","Fruit",new List<MenuItem>() { new MenuItem("Banana", 2),new MenuItem("Orange", 2),new MenuItem("Apple", 2),new MenuItem("Nuts", 2),new MenuItem("Bottle of Raine Water", 2),new MenuItem("Can of eCola", 1),new MenuItem("Can of Sprunk", 1) }),
            new ShopMenu("GasStationMenu","Gas Station",new List<MenuItem>() { new MenuItem("Hot Dog", 5),new MenuItem("Burger",3),new MenuItem("Phat Chips", 2),new MenuItem("Donut", 1),new MenuItem("Redwood Regular", 30),new MenuItem("Redwood Mild", 32),new MenuItem("Debonaire", 35),new MenuItem("Estancia Cigar", 50),new MenuItem("Lighter", 5),new MenuItem("Can of eCola", 1),new MenuItem("Can of Sprunk", 1),new MenuItem("Bottle of PiBwasser", 3),new MenuItem("Bottle of Jakeys", 3),new MenuItem("Cup of Coffee", 2),new MenuItem("Bottle of Raine Water", 2) }),
            new ShopMenu("ConvenienceAndLiquorStoreMenu","Gas & Convenience",new List<MenuItem>() { new MenuItem("Phat Chips", 2),new MenuItem("Donut", 1),new MenuItem("Redwood Regular", 30),new MenuItem("Redwood Mild", 32),new MenuItem("Debonaire", 35),new MenuItem("Debonaire Menthol", 38),new MenuItem("Caradique", 35),new MenuItem("69 Brand", 40),new MenuItem("Estancia Cigar", 50),new MenuItem("Lighter", 5),new MenuItem("Can of eCola", 1),new MenuItem("Can of Sprunk", 1),new MenuItem("Bottle of Barracho", 3),new MenuItem("Bottle of PiBwasser", 3),new MenuItem("Bottle of Blarneys", 3),new MenuItem("Bottle of Logger", 3),new MenuItem("Bottle of Patriot", 3),new MenuItem("Bottle of Pride", 3),new MenuItem("Bottle of Stronz", 4),new MenuItem("Bottle of A.M.", 4),new MenuItem("Bottle of Jakeys", 4),new MenuItem("Bottle of Dusche", 4),new MenuItem("Cup of Coffee", 2),new MenuItem("Bottle of Raine Water", 2) }),
            new ShopMenu("FancyDeliMenu","Deli",new List<MenuItem>() { new MenuItem("Chicken Club Salad",10),new MenuItem("Spicy Seafood Gumbo",14),new MenuItem("Muffaletta",8),new MenuItem("Zucchini Garden Pasta",9),new MenuItem("Pollo Mexicano",12),new MenuItem("Italian Cruz Po'boy",19),new MenuItem("Chipotle Chicken Panini",10),new MenuItem("Bottle of Raine Water",2),new MenuItem("Cup of eCola",2),new MenuItem("Cup of Sprunk",2),}),
            new ShopMenu("FancyFishMenu","Fish",new List<MenuItem>() { new MenuItem("Coconut Crusted Prawns",12),new MenuItem("Crab and Shrimp Louie",10),new MenuItem("Open-Faced Crab Melt",28),new MenuItem("King Salmon",48),new MenuItem("Ahi Tuna",44),new MenuItem("Key Lime Pie",13),new MenuItem("Bottle of Raine Water",2), }),
            new ShopMenu("FancyGenericMenu","Restaurant",new List<MenuItem>() { new MenuItem("Smokehouse Burger",10),new MenuItem("Chicken Critters Basket",7),new MenuItem("Prime Rib 16 oz",22),new MenuItem("Bone-In Ribeye",25),new MenuItem("Grilled Pork Chops",14),new MenuItem("Grilled Shrimp",15),new MenuItem("Bottle of Raine Water",2),new MenuItem("Cup of eCola",2),new MenuItem("Cup of Sprunk",2),}),
            new ShopMenu("NoodleMenu","Noodles",new List<MenuItem>() { new MenuItem("Juek Suk tong Mandu",8),new MenuItem("Hayan Jam Pong",9),new MenuItem("Sal Gook Su Jam Pong",12),new MenuItem("Chul Pan Bokkeum Jam Pong",20),new MenuItem("Deul Gae Udon",12),new MenuItem("Dakgogo Bokkeum Bap",9),new MenuItem("Bottle of Raine Water",2),new MenuItem("Cup of eCola",2),new MenuItem("Cup of Sprunk",2),}),
            new ShopMenu("WeedMenu","Marijuana",new List<MenuItem>() { new MenuItem("White Widow Preroll",2),new MenuItem("OG Kush Preroll",3),new MenuItem("Northern Lights Preroll",3),new MenuItem("White Widow Gram",7),new MenuItem("OG Kush Gram",8),new MenuItem("Northern Lights Gram",9),new MenuItem("Bong",25),new MenuItem("Lighter",5),}),
            new ShopMenu("WeedAndCigMenu","Marijuana/Cigarette",new List<MenuItem>() { new MenuItem("White Widow Preroll",2),new MenuItem("OG Kush Preroll",3),new MenuItem("Northern Lights Preroll",3),new MenuItem("White Widow Gram",7),new MenuItem("OG Kush Gram",8),new MenuItem("Northern Lights Gram",9),new MenuItem("Bong",25),new MenuItem("Redwood Regular", 30),new MenuItem("Redwood Mild", 32),new MenuItem("Debonaire", 35),new MenuItem("Debonaire Menthol", 38),new MenuItem("Caradique", 35),new MenuItem("69 Brand", 40),new MenuItem("Estancia Cigar", 50),new MenuItem("Lighter",5),}),
            new ShopMenu("WeedDealerMenu","Marijuana Dealer",new List<MenuItem>() { new MenuItem("Gram of Schwag",6, 1),new MenuItem("Gram of Mids",9, 3),new MenuItem("Gram of Dank",12, 4),new MenuItem("Joint",3, 1)}),
            //Specific
            new ShopMenu("BurgerShotMenu","Burger Shot",new List<MenuItem> { new MenuItem("Money Shot Meal", 7),new MenuItem("The Bleeder Meal", 4),new MenuItem("Torpedo Meal", 6),new MenuItem("Meat Free Meal", 5),new MenuItem("Freedom Fries", 2),new MenuItem("Liter of eCola", 2),new MenuItem("Liter of Sprunk", 2),new MenuItem("Bottle of Raine Water", 2),new MenuItem("Double Shot Coffee", 2) }),
            new ShopMenu("UpNAtomMenu","Up-N-Atom",new List<MenuItem>() { new MenuItem("Triple Burger", 4),new MenuItem("Bacon Triple Cheese Melt", 3),new MenuItem("Jumbo Shake", 5),new MenuItem("Bacon Burger", 2),new MenuItem("French Fries", 2),new MenuItem("Cup of eCola", 2),new MenuItem("Cup of Sprunk", 2),new MenuItem("Cup of Coffee", 3),new MenuItem("Bottle of Raine Water", 5) }),
            new ShopMenu("BeefyBillsMenu","Beefy Bills",new List<MenuItem>() { new MenuItem("Burger", 3),new MenuItem("Megacheese Burger", 2),new MenuItem("Double Burger", 2),new MenuItem("Kingsize Burger", 2),new MenuItem("Bacon Burger", 2),new MenuItem("French Fries", 2),new MenuItem("Can of eCola", 1),new MenuItem("Can of Sprunk", 1),new MenuItem("Bottle of Raine Water", 2) }),
            new ShopMenu("ChihuahuaHotDogMenu","Chihuahua HotDogs",new List<MenuItem>() { new MenuItem("Hot Dog", 5, 2),new MenuItem("Hot Sausage", 5),new MenuItem("Hot Pretzel", 2),new MenuItem("3 Mini Pretzels", 3),new MenuItem("Nuts", 2),new MenuItem("Can of Sprunk", 1, 1),new MenuItem("Bottle of Raine Water", 2, 1) }),
            new ShopMenu("TacoFarmerMenu","Taco Farmer",new List<MenuItem>() { new MenuItem("Taco", 2),new MenuItem("Can of eCola", 1),new MenuItem("Can of Sprunk", 1),new MenuItem("Cup of Coffee", 3),new MenuItem("Bottle of Raine Water", 2) }),
            new ShopMenu("BiteMenu","Bite!",new List<MenuItem>() { new MenuItem("Gut Buster Sandwich", 9),new MenuItem("Ham and Tuna Sandwich", 7),new MenuItem("Chef's Salad", 4),new MenuItem("Cup of eCola", 1),new MenuItem("Cup of Sprunk", 1),new MenuItem("Bottle of Raine Water", 2) }),
            new ShopMenu("TacoBombMenu","Taco Bomb",new List<MenuItem> { new MenuItem("Breakfast Burrito",4),new MenuItem("Deep Fried Salad",7),new MenuItem("Beef Bazooka",8),new MenuItem("Chimichingado Chiquito",5),new MenuItem("Cheesy Meat Flappers",6),new MenuItem("Volcano Mudsplatter Nachos",7),new MenuItem("Can of eCola", 1),new MenuItem("Can of Sprunk", 1),new MenuItem("Bottle of Raine Water", 2) }),
            new ShopMenu("WigwamMenu","Wigwam",new List<MenuItem>() { new MenuItem("Wigwam Burger", 3),new MenuItem("Wigwam Cheeseburger", 2),new MenuItem("Big Wig Burger", 5),new MenuItem("French Fries", 2),new MenuItem("Cup of eCola", 1),new MenuItem("Cup of Sprunk", 1),new MenuItem("Bottle of Raine Water", 2) }),
            new ShopMenu("ViceroyMenu","Viceroy",new List<MenuItem>() { new MenuItem("City View King",354),new MenuItem("City View Deluxe King", 378),new MenuItem("Partial Ocean View King", 392),new MenuItem("Ocean View King", 423),new MenuItem("City View Two Bedded Room", 456),new MenuItem("Grande King", 534),new MenuItem("Grande Ocean View King", 647),new MenuItem("Empire Suite", 994),new MenuItem("Monarch Suite", 1327), }),
            new ShopMenu("CluckinBellMenu","Cluckin' Bell",new List<MenuItem>() { new MenuItem("Cluckin' Little Meal",2),new MenuItem("Cluckin' Big Meal",6),new MenuItem("Cluckin' Huge Meal",12),new MenuItem("Wing Piece",7),new MenuItem("Little Peckers",8),new MenuItem("Balls & Rings",4),new MenuItem("Fries",2),new MenuItem("Fowlburger",5),new MenuItem("Cup Of Coffee",3),new MenuItem("Cup of eCola",2),new MenuItem("Cup of Sprunk",2), }),
            new ShopMenu("AlDentesMenu","Al Dentes",new List<MenuItem>() { new MenuItem("Slice of Pizza", 3, 2),new MenuItem("Cup of Sprunk", 2, 1),new MenuItem("Bottle of A.M.", 3, 1),new MenuItem("Bottle of PiBwasser", 3),new MenuItem("Bottle of PiBwasser", -1,2),new MenuItem("Bottle of Barracho", 4),new MenuItem("Bottle of Blarneys", 4),new MenuItem("Bottle of Jakeys", 3),new MenuItem("Bottle of Stronz", 4),new MenuItem("Bottle of Dusche", 3) }),
            new ShopMenu("BenefactorGallavanterMenu","Benefactor/Gallavanter",new List<MenuItem>() { new MenuItem("Gallivanter Baller",67000),new MenuItem("Gallivanter Baller II",90000),new MenuItem("Gallivanter Baller LE",149000),new MenuItem("Gallivanter Baller LE LWB",247000),new MenuItem("Benefactor Schafter",65000),new MenuItem("Benefactor Schafter V12",112000),new MenuItem("Benefactor Feltzer",145000),new MenuItem("Benefactor Schwartzer",48000),new MenuItem("Benefactor Surano",110000),new MenuItem("Benefactor Serrano",60000),new MenuItem("Benefactor Dubsta",110000),new MenuItem("Benefactor Dubsta 2",120000),new MenuItem("Benefactor XLS",151000),new MenuItem("Benefactor Streiter",156000),new MenuItem("Benefactor Schlagen GT",500000),new MenuItem("Benefactor Krieger",750000),}),
            new ShopMenu("TwentyFourSevenMenu","24/7",new List<MenuItem>() { new MenuItem("Hot Dog", 5),new MenuItem("Burger",3),new MenuItem("Phat Chips", 2),new MenuItem("Donut", 1),new MenuItem("Redwood Regular", 30),new MenuItem("Redwood Mild", 32),new MenuItem("Debonaire", 35)
                ,new MenuItem("Debonaire Menthol", 38),new MenuItem("Caradique", 35),new MenuItem("69 Brand", 40),new MenuItem("Estancia Cigar", 50),new MenuItem("Lighter", 5),new MenuItem("Can of eCola", 1),new MenuItem("Can of Sprunk", 1),new MenuItem("Bottle of PiBwasser", 3),new MenuItem("Bottle of Jakeys", 3)
                ,new MenuItem("Cup of Coffee", 2),new MenuItem("Bottle of Raine Water", 2) }),
            new ShopMenu("GrainOfTruthMenu","Grain Of Truth",new List<MenuItem>() { new MenuItem("Phat Chips", 2),new MenuItem("Donut", 1),new MenuItem("Redwood Regular", 30),new MenuItem("Redwood Mild", 32),new MenuItem("Debonaire", 35),new MenuItem("Debonaire Menthol", 38),new MenuItem("Caradique", 35),new MenuItem("69 Brand", 40),new MenuItem("Estancia Cigar", 50),new MenuItem("Lighter", 5),new MenuItem("Can of eCola", 1),new MenuItem("Can of Sprunk", 1),new MenuItem("Bottle of Barracho", 3),new MenuItem("Bottle of PiBwasser", 3),new MenuItem("Bottle of Blarneys", 3),new MenuItem("Bottle of Logger", 3),new MenuItem("Bottle of Patriot", 3),new MenuItem("Bottle of Pride", 3),new MenuItem("Bottle of Stronz", 4),new MenuItem("Bottle of A.M.", 4),new MenuItem("Bottle of Jakeys", 4),new MenuItem("Bottle of Dusche", 4),new MenuItem("Cup of Coffee", 2),new MenuItem("Bottle of Raine Water", 2) }),
            new ShopMenu("FruitVineMenu","Fruit Of The Vine",new List<MenuItem>() { new MenuItem("Phat Chips", 2),new MenuItem("Donut", 1),new MenuItem("Redwood Regular", 30),new MenuItem("Redwood Mild", 32),new MenuItem("Debonaire", 35),new MenuItem("Debonaire Menthol", 38),new MenuItem("Caradique", 35),new MenuItem("69 Brand", 40),new MenuItem("Estancia Cigar", 50),new MenuItem("Lighter", 5),new MenuItem("Can of eCola", 1),new MenuItem("Can of Sprunk", 1),new MenuItem("Bottle of Barracho", 3),new MenuItem("Bottle of PiBwasser", 3),new MenuItem("Bottle of Blarneys", 3),new MenuItem("Bottle of Logger", 3),new MenuItem("Bottle of Patriot", 3),new MenuItem("Bottle of Pride", 3),new MenuItem("Bottle of Stronz", 4),new MenuItem("Bottle of A.M.", 4),new MenuItem("Bottle of Jakeys", 4),new MenuItem("Bottle of Dusche", 4),new MenuItem("Cup of Coffee", 2),new MenuItem("Bottle of Raine Water", 2) }),
            new ShopMenu("RonMenu","Ron",new List<MenuItem>() { new MenuItem("Hot Dog", 5),new MenuItem("Burger",3),new MenuItem("Phat Chips", 2),new MenuItem("Donut", 1),new MenuItem("Redwood Regular", 30),new MenuItem("Redwood Mild", 32),new MenuItem("Debonaire", 35),new MenuItem("Estancia Cigar", 50),new MenuItem("Lighter", 5),new MenuItem("Can of eCola", 1),new MenuItem("Can of Sprunk", 1),new MenuItem("Bottle of PiBwasser", 3),new MenuItem("Bottle of Jakeys", 3),new MenuItem("Cup of Coffee", 2),new MenuItem("Bottle of Raine Water", 2) }),
            new ShopMenu("XeroMenu","Xero",new List<MenuItem>() { new MenuItem("Hot Dog", 5),new MenuItem("Burger",3),new MenuItem("Phat Chips", 2),new MenuItem("Donut", 1),new MenuItem("Redwood Regular", 30),new MenuItem("Redwood Mild", 32),new MenuItem("Debonaire", 35),new MenuItem("Estancia Cigar", 50),new MenuItem("Lighter", 5),new MenuItem("Can of eCola", 1),new MenuItem("Can of Sprunk", 1),new MenuItem("Bottle of PiBwasser", 3),new MenuItem("Bottle of Jakeys", 3),new MenuItem("Cup of Coffee", 2),new MenuItem("Bottle of Raine Water", 2) }),
            new ShopMenu("LTDMenu","LTD",new List<MenuItem>() { new MenuItem("Hot Dog", 5),new MenuItem("Burger",3),new MenuItem("Phat Chips", 2),new MenuItem("Donut", 1),new MenuItem("Redwood Regular", 30),new MenuItem("Redwood Mild", 32),new MenuItem("Debonaire", 35),new MenuItem("Estancia Cigar", 50),new MenuItem("Lighter", 5),new MenuItem("Can of eCola", 1),new MenuItem("Can of Sprunk", 1),new MenuItem("Bottle of PiBwasser", 3),new MenuItem("Bottle of Jakeys", 3),new MenuItem("Cup of Coffee", 2),new MenuItem("Bottle of Raine Water", 2) }),
    };
    }
}

