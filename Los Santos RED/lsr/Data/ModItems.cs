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


public class ModItems : IModItems
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\ModItems.xml";
    private List<ModItem> ModItemsList;
    public List<ModItem> Items => ModItemsList;
    public void ReadConfig()
    {
        if (File.Exists(ConfigFileName))
        {
            ModItemsList = Serialization.DeserializeParams<ModItem>(ConfigFileName);
        }
        else
        {
            DefaultConfig();
            Serialization.SerializeParams(ModItemsList, ConfigFileName);
        }
    }
    private void DefaultConfig()
    {
        ModItemsList = new List<ModItem>
        {

            /*	<PropModel modelName="v_res_tre_pineapple" modelHash="0xce921430" />
	<PropModel modelName="prop_pineapple" modelHash="0xcda1c62b" />
	<PropModel modelName="ng_proc_food_bag01a" modelHash="0xf448d6de" />
	<PropModel modelName="ng_proc_food_bag02a" modelHash="0xa0aba1c9" />
	<PropModel modelName="ng_proc_food_burg01a" modelHash="0x170638da" />
	<PropModel modelName="ng_proc_food_burg02a" modelHash="0x21bdd9d2" />
	<PropModel modelName="ng_proc_food_burg02c" modelHash="0x754d00ef" />
	<PropModel modelName="ng_proc_food_chips01a" modelHash="0x2f311fd2" />
	<PropModel modelName="ng_proc_food_chips01b" modelHash="0x3c67ba3f" />
	<PropModel modelName="ng_proc_food_chips01c" modelHash="0xbd113b84" />
	<PropModel modelName="ng_proc_food_ornge1a" modelHash="0x5757bc3c" />
	<PropModel modelName="prop_food_bag2" modelHash="0x57358f6b" />
	<PropModel modelName="prop_food_bag1" modelHash="0x37d1509b" />
	<PropModel modelName="prop_food_bs_bag_01" modelHash="0x8d6a84aa" />
	<PropModel modelName="prop_food_bs_bag_02" modelHash="0x9b412057" />
	<PropModel modelName="prop_food_bs_bag_03" modelHash="0x837270ba" />
	<PropModel modelName="prop_food_bs_bag_04" modelHash="0x11f88dcc" />
	<PropModel modelName="prop_food_bs_burg1" modelHash="0x7d682c79" />
	<PropModel modelName="prop_food_bs_burg3" modelHash="0x2d488c3f" />
	<PropModel modelName="prop_food_bs_burger2" modelHash="0x3ad97d39" />
	<PropModel modelName="prop_food_bs_chips" modelHash="0x56072f5c" />
	<PropModel modelName="prop_food_bs_coffee" modelHash="0x07a7232a" />
	<PropModel modelName="prop_food_bs_juice01" modelHash="0x7ecb50cc" />
	<PropModel modelName="prop_food_bs_juice02" modelHash="0x1a29871e" />
	<PropModel modelName="prop_food_bs_juice03" modelHash="0x2bdbaa82" />
	<PropModel modelName="prop_food_bs_tray_02" modelHash="0x8662b9bf" />
	<PropModel modelName="prop_food_bs_tray_03" modelHash="0x78151d24" />
	<PropModel modelName="prop_food_burg1" modelHash="0x3482b62e" />
	<PropModel modelName="prop_food_burg2" modelHash="0xdacb82c1" />
	<PropModel modelName="prop_food_burg3" modelHash="0x190bff41" />
	<PropModel modelName="prop_food_cb_bag_01" modelHash="0x0b86b5bb" />
	<PropModel modelName="prop_food_cb_bag_02" modelHash="0x56424b3d" />
	<PropModel modelName="prop_food_cb_burg02" modelHash="0x125e5a30" />
	<PropModel modelName="prop_food_cb_chips" modelHash="0x78f070c0" />
	<PropModel modelName="prop_food_cb_coffee" modelHash="0xdc9894e1" />
	<PropModel modelName="prop_food_cb_donuts" modelHash="0x8dcb8036" />
	<PropModel modelName="prop_food_cb_juice01" modelHash="0xd8e622c5" />
	<PropModel modelName="prop_food_cb_juice02" modelHash="0xf63a5d6d" />
	<PropModel modelName="prop_food_cb_nugets" modelHash="0x83475c8d" />
	<PropModel modelName="prop_food_cb_tray_02" modelHash="0x52c64b49" />
	<PropModel modelName="prop_food_cb_tray_03" modelHash="0x2cf47fa6" />
	<PropModel modelName="prop_food_chips" modelHash="0xe0ec50c9" />
	<PropModel modelName="prop_food_coffee" modelHash="0xb220a1eb" />
	<PropModel modelName="prop_food_juice01" modelHash="0x53df6ed4" />
	<PropModel modelName="prop_food_juice02" modelHash="0xe1950a41" />
	<PropModel modelName="prop_food_tray_02" modelHash="0xa9435803" />
	<PropModel modelName="prop_food_tray_03" modelHash="0xafe36543" />
            */

            //Generic Services
            new ModItem("Room: Single Twin",eConsumableType.Service),
            new ModItem("Room: Single Queen",eConsumableType.Service),
            new ModItem("Room: Double Queen",eConsumableType.Service),
            new ModItem("Room: Single King",eConsumableType.Service),



            //new ModItem("Full",eConsumableType.Service),
            //new ModItem("Half And Half",eConsumableType.Service),
            //new ModItem("Head",eConsumableType.Service),
            //new ModItem("Handy",eConsumableType.Service),

            //Generic Tools
            new ModItem("Screwdriver","prop_tool_screwdvr01",57005,new Vector3(0.1170f, 0.0610f, 0.0150f), new Rotator(-47.199f, 166.62f, -19.9f)),
            new ModItem("Hammer","prop_tool_hammer",57005,new Vector3(0.1170f, 0.0610f, 0.0150f), new Rotator(-47.199f, 166.62f, -19.9f)),
            new ModItem("Drill","prop_tool_drill",57005,new Vector3(0.1170f, 0.0610f, 0.0150f), new Rotator(-47.199f, 166.62f, -19.9f)),
            new ModItem("Pliers","prop_tool_pliers",false,57005,new Vector3(0.1170f, 0.0610f, 0.0150f), new Rotator(-47.199f, 166.62f, -19.9f)),
            new ModItem("Shovel","prop_tool_shovel",true,57005,new Vector3(0.1170f, 0.0610f, 0.0150f), new Rotator(-47.199f, 166.62f, -19.9f)),
            new ModItem("Wrench","prop_tool_wrench",true,57005,new Vector3(0.1170f, 0.0610f, 0.0150f), new Rotator(-47.199f, 166.62f, -19.9f)),
            new ModItem("Lighter","ng_proc_ciglight01a",57005,new Vector3(0.1170f, 0.0610f, 0.0150f), new Rotator(-47.199f, 166.62f, -19.9f)),

            //Generic Food/Drink/SMoke
            new ModItem("Hot Dog",eConsumableType.Eat,"prop_cs_hotdog_01",57005,new Vector3(0.14f, -0.02f, -0.04f),new Rotator(178.0f, 28.0f, 0.0f)) ,
            new ModItem("Hot Sausage",eConsumableType.Eat,"prop_cs_hotdog_01",57005,new Vector3(0.14f, -0.02f, -0.04f),new Rotator(178.0f, 28.0f, 0.0f)),
            new ModItem("Hot Pretzel",eConsumableType.Eat,"",57005,new Vector3(0.14f, -0.02f, -0.04f),new Rotator(178.0f, 28.0f, 0.0f)),
            new ModItem("3 Mini Pretzels",eConsumableType.Eat,"",57005,new Vector3(0.14f, -0.02f, -0.04f),new Rotator(178.0f, 28.0f, 0.0f)),
            new ModItem("Nuts",eConsumableType.Eat,"",57005,new Vector3(0.14f, -0.02f, -0.04f),new Rotator(178.0f, 28.0f, 0.0f)),
            new ModItem("Bottle of Raine Water",eConsumableType.Drink,"ba_prop_club_water_bottle",57005,new Vector3(0.12f, -0.1f, -0.07f),new Rotator(-70.0f, 0.0f, 0.0f)),
            new ModItem("Burger",eConsumableType.Eat,"prop_cs_burger_01",57005,new Vector3(0.16f, 0.01f, -0.04f),new Rotator(0.0f, 28.0f, 0.0f)),     
            new ModItem("Donut",eConsumableType.Eat,"prop_donut_01",57005,new Vector3(0.15f, 0.01f, -0.03f),new Rotator(-15.0f, 17.0f, 0.0f)) ,
            new ModItem("Slice of Pizza",eConsumableType.Eat,"v_res_tt_pizzaplate",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)) ,
            new ModItem("Burger",eConsumableType.Eat,"prop_cs_burger_01",57005,new Vector3(0.16f, 0.01f, -0.04f),new Rotator(0.0f, 28.0f, 0.0f)),
            new ModItem("40 oz",eConsumableType.Drink,"prop_cs_beer_bot_40oz",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)) { IsIntoxicating = true },
            new ModItem("Can of eCola",eConsumableType.Drink,"ng_proc_sodacan_01a",57005,new Vector3(0.13f, -0.06f, -0.06f),new Rotator(-73.0f, 0.0f, 0.0f)),
            new ModItem("Can of Sprunk",eConsumableType.Drink,"ng_proc_sodacan_01b",57005,new Vector3(0.13f, -0.06f, -0.06f),new Rotator(-73.0f, 0.0f, 0.0f)),
            new ModItem("Cup of eCola",eConsumableType.Drink,"prop_food_juice01",57005,new Vector3(0.07f, -0.1f, -0.1f),new Rotator(-74.0f, 28.0f, 3.0f)),
            new ModItem("Cup of Sprunk",eConsumableType.Drink,"ng_proc_sodacup_01b",57005,new Vector3(0.07f, -0.1f, -0.1f),new Rotator(-74.0f, 28.0f, 3.0f)),
            new ModItem("Cup of Coffee",eConsumableType.Drink,"prop_food_coffee",57005,new Vector3(0.12f, -0.06f, -0.08f),new Rotator(-78.0f, 0.0f, 0.0f)),
            new ModItem("French Fries",eConsumableType.Eat,"prop_food_chips",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)),
            new ModItem("Banana",eConsumableType.Eat,"ng_proc_food_nana1a",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)),
            new ModItem("Orange",eConsumableType.Eat,"ng_proc_food_ornge1a",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)),
            new ModItem("Apple",eConsumableType.Eat,"ng_proc_food_aple1a",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)),
            new ModItem("Bottle of A.M.",eConsumableType.Drink,"prop_beer_am",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)) { IsIntoxicating = true },
            new ModItem("Bottle of PiBwasser",eConsumableType.Drink,"prop_amb_beer_bottle",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)) { IsIntoxicating = true },
            new ModItem("Bottle of Barracho",eConsumableType.Drink,"prop_beer_bar",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)) { IsIntoxicating = true},
            new ModItem("Bottle of Blarneys",eConsumableType.Drink,"prop_beer_blr",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)) { IsIntoxicating = true },
            new ModItem("Bottle of Jakeys",eConsumableType.Drink,"prop_beer_jakey",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)) { IsIntoxicating = true },
            new ModItem("Bottle of Logger",eConsumableType.Drink,"prop_beer_logger",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)) { IsIntoxicating = true },
            new ModItem("Bottle of Patriot",eConsumableType.Drink,"prop_beer_patriot",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)) { IsIntoxicating = true },
            new ModItem("Bottle of Pride",eConsumableType.Drink,"prop_beer_pride",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)) { IsIntoxicating = true },
            new ModItem("Bottle of Stronz",eConsumableType.Drink,"prop_beer_stz",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)) { IsIntoxicating = true},
            new ModItem("Bottle of Dusche",eConsumableType.Drink,"prop_beerdusche",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)) { IsIntoxicating = true },
            new ModItem("Phat Chips",eConsumableType.Eat,"ng_proc_food_chips01a",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)),
            new ModItem("Taco",eConsumableType.Eat,"",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)),
            new ModItem("Redwood Regular",eConsumableType.Smoke,"ng_proc_cigarette01a","v_ret_ml_cigs",57005, new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)) {  AmountPerPackage = 20 },
            new ModItem("Redwood Mild",eConsumableType.Smoke,"ng_proc_cigarette01a","v_ret_ml_cigs2",57005, new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)) {  AmountPerPackage = 20 },
            new ModItem("Debonaire",eConsumableType.Smoke,"ng_proc_cigarette01a","v_ret_ml_cigs3",57005, new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)) {  AmountPerPackage = 20 },
            new ModItem("Debonaire Menthol",eConsumableType.Smoke,"ng_proc_cigarette01a","v_ret_ml_cigs4",57005, new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)) {  AmountPerPackage = 20 },
            new ModItem("Caradique",eConsumableType.Smoke,"ng_proc_cigarette01a","v_ret_ml_cigs5",57005, new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)) {  AmountPerPackage = 20 },
            new ModItem("69 Brand",eConsumableType.Smoke,"ng_proc_cigarette01a","v_ret_ml_cigs6",57005, new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)) {  AmountPerPackage = 20 },
            new ModItem("Joint",eConsumableType.Smoke,"p_amb_joint_01",57005, new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)) { AmountPerPackage = 5, IsIntoxicating = true },
            new ModItem("Estancia Cigar",eConsumableType.Smoke,"prop_cigar_02","p_cigar_pack_02_s",57005, new Vector3(0.17f, 0.02f, 0.0f),new Rotator(0.0f, -78f, 0f)) { AmountPerPackage = 20 },
            new ModItem("Ham and Cheese Sandwich",eConsumableType.Eat,"prop_sandwich_01",57005,new Vector3(0.14f, -0.02f, -0.04f),new Rotator(178.0f, 28.0f, 0.0f)) ,
            new ModItem("Turkey Sandwich",eConsumableType.Eat,"prop_sandwich_01",57005,new Vector3(0.14f, -0.02f, -0.04f),new Rotator(178.0f, 28.0f, 0.0f)) ,
            new ModItem("Tuna Sandwich",eConsumableType.Eat,"prop_sandwich_01",57005,new Vector3(0.14f, -0.02f, -0.04f),new Rotator(178.0f, 28.0f, 0.0f)) ,

            //UPNATOM
            new ModItem("Triple Burger",eConsumableType.Eat,"prop_cs_burger_01",57005,new Vector3(0.16f, 0.01f, -0.04f),new Rotator(0.0f, 28.0f, 0.0f)),
            new ModItem("Bacon Triple Cheese Melt",eConsumableType.Eat,"prop_cs_burger_01",57005,new Vector3(0.16f, 0.01f, -0.04f),new Rotator(0.0f, 28.0f, 0.0f)),
            new ModItem("Jumbo Shake",eConsumableType.Drink,"prop_food_juice01",57005,new Vector3(0.16f, 0.01f, -0.04f),new Rotator(0.0f, 28.0f, 0.0f)),


            //BurgerShot
            new ModItem("Money Shot Meal",eConsumableType.Eat,"prop_food_bs_burg1","prop_food_bs_tray_02",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)) ,
            new ModItem("The Bleeder Meal",eConsumableType.Eat,"prop_food_bs_burg1","prop_food_bs_tray_02",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)) ,
            new ModItem("Torpedo Meal",eConsumableType.Eat,"prop_food_bs_burger2","prop_food_bs_tray_03",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)) ,
            new ModItem("Meat Free Meal",eConsumableType.Eat,"prop_food_bs_burger2","prop_food_bs_tray_01",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)) ,
            new ModItem("Freedom Fries",eConsumableType.Eat,"prop_food_bs_chips",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)) ,
            new ModItem("Double Shot Coffee",eConsumableType.Drink,"prop_food_bs_coffee",57005,new Vector3(0.07f, -0.1f, -0.1f),new Rotator(-74.0f, 28.0f, 3.0f)),
            new ModItem("Liter of eCola",eConsumableType.Drink,"prop_food_bs_juice01",57005,new Vector3(0.07f, -0.1f, -0.1f),new Rotator(-74.0f, 28.0f, 3.0f)),
            new ModItem("Liter of Sprunk",eConsumableType.Drink,"prop_food_bs_juice01",57005,new Vector3(0.07f, -0.1f, -0.1f),new Rotator(-74.0f, 28.0f, 3.0f)),

            //Bite
            new ModItem("Gut Buster Sandwich",eConsumableType.Eat,"prop_food_burg2",57005,new Vector3(0.14f, 0.01f, -0.06f),new Rotator(0f, 0f, 0f)) ,
            new ModItem("Ham and Tuna Sandwich",eConsumableType.Eat,"prop_food_burg2",57005,new Vector3(0.14f, 0.01f, -0.06f),new Rotator(0f, 0f, 0f)) ,
            new ModItem("Chef's Salad",eConsumableType.Eat,"","prop_food_bag1",57005,new Vector3(0.14f, -0.02f, -0.04f),new Rotator(178.0f, 28.0f, 0.0f)) ,

            //BeefyBills
            new ModItem("Megacheese Burger",eConsumableType.Eat,"prop_cs_burger_01",57005,new Vector3(0.16f, 0.01f, -0.04f),new Rotator(0.0f, 28.0f, 0.0f)),
            new ModItem("Double Burger",eConsumableType.Eat,"prop_cs_burger_01",57005,new Vector3(0.16f, 0.01f, -0.04f),new Rotator(0.0f, 28.0f, 0.0f)) ,
            new ModItem("Kingsize Burger",eConsumableType.Eat,"prop_cs_burger_01",57005,new Vector3(0.16f, 0.01f, -0.04f),new Rotator(0.0f, 28.0f, 0.0f)) ,
            new ModItem("Bacon Burger",eConsumableType.Eat,"prop_cs_burger_01",57005,new Vector3(0.16f, 0.01f, -0.04f),new Rotator(0.0f, 28.0f, 0.0f)) ,


            //Taco Bomb
            new ModItem("Breakfast Burrito",eConsumableType.Eat,"prop_food_bs_burger2","prop_food_bag1",57005,new Vector3(0.14f, -0.02f, -0.04f),new Rotator(178.0f, 28.0f, 0.0f)),
            new ModItem("Deep Fried Salad",eConsumableType.Eat,"","prop_food_bag1",57005,new Vector3(0.14f, -0.02f, -0.04f),new Rotator(178.0f, 28.0f, 0.0f)),
            new ModItem("Beef Bazooka",eConsumableType.Eat,"prop_food_bs_burger2","prop_food_bag1",57005,new Vector3(0.14f, -0.02f, -0.04f),new Rotator(178.0f, 28.0f, 0.0f)),
            new ModItem("Chimichingado Chiquito",eConsumableType.Eat,"","prop_food_bag1",57005,new Vector3(0.14f, -0.02f, -0.04f),new Rotator(178.0f, 28.0f, 0.0f)),
            new ModItem("Cheesy Meat Flappers",eConsumableType.Eat,"","prop_food_bag1",57005,new Vector3(0.14f, -0.02f, -0.04f),new Rotator(178.0f, 28.0f, 0.0f)),
            new ModItem("Volcano Mudsplatter Nachos",eConsumableType.Eat,"","prop_food_bag1",57005,new Vector3(0.14f, -0.02f, -0.04f),new Rotator(178.0f, 28.0f, 0.0f)),

            //WIgwam Menu
            new ModItem("Wigwam Burger",eConsumableType.Eat,"prop_cs_burger_01",57005,new Vector3(0.16f, 0.01f, -0.04f),new Rotator(0.0f, 28.0f, 0.0f)),
            new ModItem("Wigwam Cheeseburger",eConsumableType.Eat,"prop_cs_burger_01",57005,new Vector3(0.16f, 0.01f, -0.04f),new Rotator(0.0f, 28.0f, 0.0f)),
            new ModItem("Big Wig Burger",eConsumableType.Eat,"prop_cs_burger_01",57005,new Vector3(0.16f, 0.01f, -0.04f),new Rotator(0.0f, 28.0f, 0.0f)),

            //CB

            //Viceroy
            new ModItem("City View King",eConsumableType.Service),
            new ModItem("City View Deluxe King",eConsumableType.Service),
            new ModItem("Partial Ocean View King",eConsumableType.Service),
            new ModItem("Ocean View King",eConsumableType.Service),
            new ModItem("City View Two Bedded Room",eConsumableType.Service),
            new ModItem("Grande King",eConsumableType.Service),
            new ModItem("Grande Ocean View King",eConsumableType.Service),
            new ModItem("Empire Suite",eConsumableType.Service),
            new ModItem("Monarch Suite",eConsumableType.Service),

            //Generic


            //new ModItem("Placeholder",eConsumableType.Eat,"prop_food_burg3",57005,new Vector3(0.16f, -0.02f, -0.11f),new Rotator(0.0f, 38.0f, 0.0f)),
            




            //wrapped burger prop_food_bs_burg1
            //wrapped sandwich prop_food_bs_burger2
            //empty BS tray prop_food_bs_tray_01
            //buirger fries BS Tray prop_food_bs_tray_02
            //sandwich lettuce BS tray prop_food_bs_tray_03


            //empty CB tray prop_food_cb_tray_01
            //burger tray CB prop_food_cb_tray_02
            //niugget tray CB prop_food_cb_tray_03



            //generic bag 1 prop_food_bag1
            //generic bag 2 prop_food_bag2
            //generic wrapped burger prop_food_burg1
            //generic wrapped sandwich prop_food_burg2
            //generic burger box prop_food_burg3
            //generic fries prop_food_chips
            //generic coffee prop_food_coffee
            //generic cup ecola1 prop_food_juice01
            //generic cup ecola2 prop_food_juice02

            //generic empty tray prop_food_tray_01
            //geneir cburger tray prop_food_tray_02
            //generic sandwich tray prop_food_tray_03


            //generic red wine prop_wine_red
            //generic wine bottle prop_wine_bot_01
            //geneirc wine bottle prop_wine_bot_02
            //generic wine rose prop_wine_rose
            //generic wine white prop_wine_white
            //generic wine red prop_wine_red

            //packed cigarettes redwood prop_ld_fags_01
            //packed cigarettes Debonaire prop_ld_fags_02


            //packed redwood 1 v_ret_ml_cigs
            //packed redwood 2 v_ret_ml_cigs2
            //packed debonaire 1 v_ret_ml_cigs3
            //packed debonaire 2 v_ret_ml_cigs4

            //packed Caradique? v_ret_ml_cigs5
            //packed 69 Brand v_ret_ml_cigs6


            
            //Sandwiches


        };
    }
    public ModItem Get(string name)
    {
        return ModItemsList.FirstOrDefault(x => x.Name == name);
    }
}

