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

    public ModItem Get(string name)
    {
        return ModItemsList.FirstOrDefault(x => x.Name == name);
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
            new ModItem("Room: Single Twin","Cheapest room for the most discerning client",eConsumableType.Service),
            new ModItem("Room: Single Queen","Clean sheets on request",eConsumableType.Service),
            new ModItem("Room: Double Queen","Have a little company, but don't want to get too close?",eConsumableType.Service),
            new ModItem("Room: Single King","Please clean off all mirrors after use",eConsumableType.Service),



            //new ModItem("Full",eConsumableType.Service),
            //new ModItem("Half And Half",eConsumableType.Service),
            //new ModItem("Head",eConsumableType.Service),
            //new ModItem("Handy",eConsumableType.Service),

            //Generic Tools
            new ModItem("Screwdriver","Might get you into some locked things") {
                ModelItem = new PhysicalItem("prop_tool_screwdvr01",57005,new Vector3(0.1170f, 0.0610f, 0.0150f), new Rotator(-47.199f, 166.62f, -19.9f)) },
            new ModItem("Hammer","It's hammer time") {
                ModelItem = new PhysicalItem("prop_tool_hammer",57005,new Vector3(0.1170f, 0.0610f, 0.0150f), new Rotator(-47.199f, 166.62f, -19.9f)) },
            new ModItem("Drill","Not your dentists drill") {
                ModelItem = new PhysicalItem("prop_tool_drill",57005,new Vector3(0.1170f, 0.0610f, 0.0150f), new Rotator(-47.199f, 166.62f, -19.9f)) },
            new ModItem("Pliers","When you really need to grab something") {
                ModelItem = new PhysicalItem("prop_tool_pliers",57005,new Vector3(0.1170f, 0.0610f, 0.0150f), new Rotator(-47.199f, 166.62f, -19.9f)) },
            new ModItem("Shovel","Gets rid of lots of problems") {
                ModelItem = new PhysicalItem("prop_tool_shovel",57005,new Vector3(0.1170f, 0.0610f, 0.0150f), new Rotator(-47.199f, 166.62f, -19.9f)) { IsLarge = true } },
            new ModItem("Wrench","What harm could a loose bolt have?") {
                ModelItem = new PhysicalItem("prop_tool_wrench",57005,new Vector3(0.1170f, 0.0610f, 0.0150f), new Rotator(-47.199f, 166.62f, -19.9f)) { IsLarge = true }  },
            new ModItem("Lighter","Arson strongly discouraged") {
                ModelItem = new PhysicalItem("ng_proc_ciglight01a",57005,new Vector3(0.1170f, 0.0610f, 0.0150f), new Rotator(-47.199f, 166.62f, -19.9f)) },

            //Generic Food/Drink/SMoke
            new ModItem("Hot Dog","Niko would be proud",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("prop_cs_hotdog_01",57005,new Vector3(0.14f, -0.02f, -0.04f),new Rotator(178.0f, 28.0f, 0.0f)), HealthGained = 10 },
            new ModItem("Hot Sausage","Get all your jokes out",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("prop_cs_hotdog_01",57005,new Vector3(0.14f, -0.02f, -0.04f),new Rotator(178.0f, 28.0f, 0.0f)), HealthGained = 10 },
            new ModItem("Hot Pretzel","You tie me up",eConsumableType.Eat) {HealthGained = 10 },
            new ModItem("3 Mini Pretzels","Like a pretzel, but smaller",eConsumableType.Eat) {HealthGained = 10 },
            new ModItem("Nuts","You're gonna love my nuts",eConsumableType.Eat) {HealthGained = 10 },
            new ModItem("Bottle of Raine Water","The water that rich people drink, and the main reason why there are now entire continents of plastic bottles floating in the ocean",eConsumableType.Drink) {
                ModelItem = new PhysicalItem("ba_prop_club_water_bottle",57005,new Vector3(0.12f, -0.1f, -0.07f),new Rotator(-70.0f, 0.0f, 0.0f)), HealthGained = 20 },
            new ModItem("Burger","100% Certified Food",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("prop_cs_burger_01",57005,new Vector3(0.16f, 0.01f, -0.04f),new Rotator(0.0f, 28.0f, 0.0f)), HealthGained = 10 },
            new ModItem("Donut","mmmm Donuts",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("prop_donut_01",57005,new Vector3(0.15f, 0.01f, -0.03f),new Rotator(-15.0f, 17.0f, 0.0f)), HealthGained = 10 } ,
            new ModItem("Slice of Pizza","Caution may be hot",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("v_res_tt_pizzaplate",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)), HealthGained = 10 } ,
            new ModItem("40 oz","Drink like a true thug!",eConsumableType.Drink) {
                ModelItem = new PhysicalItem("prop_cs_beer_bot_40oz",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)), IntoxicantName = "Alcohol" },
            new ModItem("Can of eCola", "Deliciously Infectious!",eConsumableType.Drink) {
                ModelItem = new PhysicalItem("ng_proc_sodacan_01a",57005,new Vector3(0.13f, -0.06f, -0.06f),new Rotator(-73.0f, 0.0f, 0.0f)) , HealthGained = 10},
            new ModItem("Can of Sprunk", "Slurp Sprunk Mmm! Delicious",eConsumableType.Drink) {
                ModelItem = new PhysicalItem("ng_proc_sodacan_01b",57005,new Vector3(0.13f, -0.06f, -0.06f),new Rotator(-73.0f, 0.0f, 0.0f)), HealthGained = 10 },
            new ModItem("Cup of eCola", "Deliciously Infectious!",eConsumableType.Drink) {
                ModelItem = new PhysicalItem("prop_food_juice01",57005,new Vector3(0.07f, -0.1f, -0.1f),new Rotator(-74.0f, 28.0f, 3.0f)), HealthGained = 10 },
            new ModItem("Cup of Sprunk", "Slurp Sprunk Mmm! Delicious",eConsumableType.Drink) {
                ModelItem = new PhysicalItem("ng_proc_sodacup_01b",57005,new Vector3(0.07f, -0.1f, -0.1f),new Rotator(-74.0f, 28.0f, 3.0f)), HealthGained = 10 },
            new ModItem("Cup of Coffee","Finally something without sugar! Sugar on Request",eConsumableType.Drink) {
                ModelItem = new PhysicalItem("prop_food_coffee",57005,new Vector3(0.12f, -0.06f, -0.08f),new Rotator(-78.0f, 0.0f, 0.0f)), HealthGained = 10 },
            new ModItem("French Fries","Freedom fries made from true Cataldo potatoes!",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("prop_food_chips",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)), HealthGained = 5 },
            new ModItem("Fries","Freedom fries made from true Cataldo potatoes!",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("prop_food_chips",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)), HealthGained = 5 },
            new ModItem("Banana","An elongated, edible fruit – botanically a berry[1][2] – produced by several kinds of large herbaceous flowering plants in the genus Musa",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("ng_proc_food_nana1a",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)), HealthGained = 10 },
            new ModItem("Orange","Not just a color",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("ng_proc_food_ornge1a",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)), HealthGained = 10 },
            new ModItem("Apple","Certified sleeping death free",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("ng_proc_food_aple1a",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)), HealthGained = 10 },
            new ModItem("Ham and Cheese Sandwich","Basic and shitty, just like you",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("prop_sandwich_01",57005,new Vector3(0.14f, -0.02f, -0.04f),new Rotator(178.0f, 28.0f, 0.0f)), HealthGained = 10 },
            new ModItem("Turkey Sandwich","The most plain sandwich for the most plain person",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("prop_sandwich_01",57005,new Vector3(0.14f, -0.02f, -0.04f),new Rotator(178.0f, 28.0f, 0.0f)), HealthGained = 10 } ,
            new ModItem("Tuna Sandwich","Haven't got enough heavy metals in you at your job? Try tuna!",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("prop_sandwich_01",57005,new Vector3(0.14f, -0.02f, -0.04f),new Rotator(178.0f, 28.0f, 0.0f)), HealthGained = 10 } ,


            new ModItem("Bottle of A.M.","Mornings Golden Shower",eConsumableType.Drink) {
                ModelItem = new PhysicalItem("prop_beer_am",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)), IntoxicantName = "Alcohol" },
            new ModItem("Bottle of PiBwasser","Cheap 11% ABV fighting lager brewed in Germany for export only from rice, barley, hops and the fresh urine of Bavarian virgins",eConsumableType.Drink) {
                ModelItem = new PhysicalItem("prop_amb_beer_bottle",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)), IntoxicantName = "Alcohol" },
            new ModItem("Bottle of Barracho","Es Playtime!",eConsumableType.Drink) {
                ModelItem = new PhysicalItem("prop_beer_bar",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)), IntoxicantName = "Alcohol"},
            new ModItem("Bottle of Blarneys","Making your mouth feel lucky",eConsumableType.Drink) {
                ModelItem = new PhysicalItem("prop_beer_blr",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)), IntoxicantName = "Alcohol" },
            new ModItem("Bottle of Jakeys","Drink Outdoors With Jakey's",eConsumableType.Drink) {
                ModelItem = new PhysicalItem("prop_beer_jakey",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)), IntoxicantName = "Alcohol"},
            new ModItem("Bottle of Logger","A classic American tasteless, watery beer, made by Rednecks for Rednecks. Now Chinese owned",eConsumableType.Drink) {
                ModelItem = new PhysicalItem("prop_beer_logger",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)), IntoxicantName = "Alcohol"},
            new ModItem("Bottle of Patriot","Never refuse a patriot",eConsumableType.Drink) {
                ModelItem = new PhysicalItem("prop_beer_patriot",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)), IntoxicantName = "Alcohol" },
            new ModItem("Bottle of Pride","Swallow Me",eConsumableType.Drink) {
                ModelItem = new PhysicalItem("prop_beer_pride",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)),IntoxicantName = "Alcohol" },
            new ModItem("Bottle of Stronzo","Birra forte d'Italia",eConsumableType.Drink) {
                ModelItem = new PhysicalItem("prop_beer_stz",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)),IntoxicantName = "Alcohol" },
            new ModItem("Bottle of Dusche","Das Ist Gut Ja!",eConsumableType.Drink) {
                ModelItem = new PhysicalItem("prop_beerdusche",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)), IntoxicantName = "Alcohol"},
            new ModItem("Phat Chips","They are extra phat",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("ng_proc_food_chips01a",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)), HealthGained = 10 },

            new ModItem("Ego Chaser Energy Bar","Contains 20,000 Calories! ~n~'It's all about you'",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("prop_choc_ego",57005,new Vector3(0.13f, 0.05f, -0.02f),new Rotator(25f, -11f, -95f)), HealthGained = 20 },

            new ModItem("King Size P's & Q's","The candy bar that kids and stoners love. EXTRA Large",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("prop_candy_pqs",57005,new Vector3(0.16f, 0.01f, -0.02f),new Rotator(-178f, -169f, 169f)), HealthGained = 15 },
            new ModItem("P's & Q's","The candy bar that kids and stoners love",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("prop_choc_pq",57005,new Vector3(0.12f, 0.02f, -0.02f),new Rotator(-178f, -169f, 79f)), HealthGained = 10 },
            new ModItem("Meteorite Bar","Dark chocolate with a GOOEY core",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("prop_choc_meto",57005,new Vector3(0.12f, 0.03f, -0.02f),new Rotator(169f, 170f, 76f)), HealthGained = 10 },

            

            new ModItem("Taco",eConsumableType.Eat) { HealthGained = 10 },
            new ModItem("Redwood Regular", "Tobacco products for real men who don't go to the doctors or read fear-mongering, left-wing so-called medical propaganda",eConsumableType.Smoke) {
                ModelItem = new PhysicalItem("ng_proc_cigarette01a",57005, new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)),
                PackageItem = new PhysicalItem("v_ret_ml_cigs",57005, new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)),
                AmountPerPackage = 20 },


            new ModItem("Redwood Mild", "Tobacco products for real men who don't go to the doctors or read fear-mongering, left-wing so-called medical propaganda. Milder version",eConsumableType.Smoke) {
                ModelItem = new PhysicalItem("ng_proc_cigarette01a",57005, new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)),
                PackageItem = new PhysicalItem("v_ret_ml_cigs2",57005, new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)),AmountPerPackage = 20 },
            new ModItem("Debonaire", "Tobacco products marketed at the more sophisticated smoker, whoever that is",eConsumableType.Smoke) {
                ModelItem = new PhysicalItem("ng_proc_cigarette01a",57005, new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)),
                PackageItem = new PhysicalItem("v_ret_ml_cigs3",57005, new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)),AmountPerPackage = 20 },
            new ModItem("Debonaire Menthol", "Tobacco products marketed at the more sophisticated smoker, whoever that is. With Menthol!",eConsumableType.Smoke) {
                ModelItem = new PhysicalItem("ng_proc_cigarette01a",57005, new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)),
                PackageItem = new PhysicalItem("v_ret_ml_cigs4",57005, new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)),AmountPerPackage = 20 },
            new ModItem("Caradique", "Fine Napoleon Cigarettes",eConsumableType.Smoke) {
                ModelItem = new PhysicalItem("ng_proc_cigarette01a",57005, new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)),
                PackageItem = new PhysicalItem("v_ret_ml_cigs5",57005, new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)),AmountPerPackage = 20 },
            new ModItem("69 Brand","Don;t let an embargo stop you",eConsumableType.Smoke) {
                ModelItem = new PhysicalItem("ng_proc_cigarette01a",57005, new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)),
                PackageItem = new PhysicalItem("v_ret_ml_cigs6",57005, new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)),AmountPerPackage = 20 },
            new ModItem("Joint","Weed in rolled form",eConsumableType.Smoke) {
                ModelItem = new PhysicalItem("p_amb_joint_01",57005, new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)), IntoxicantName = "Marijuana" },
            new ModItem("Estancia Cigar","Medium Cut. Hand Rolled.",eConsumableType.Smoke) {
                ModelItem = new PhysicalItem("prop_cigar_02",57005, new Vector3(0.17f, 0.02f, 0.0f),new Rotator(0.0f, -78f, 0f)),
                PackageItem = new PhysicalItem("p_cigar_pack_02_s",57005, new Vector3(0.17f, 0.02f, 0.0f),new Rotator(0.0f, -78f, 0f)),AmountPerPackage = 20 },
            

            //UPNATOM
            new ModItem("Triple Burger", "Three times the meat, three times the cholesterol", eConsumableType.Eat) {
                ModelItem = new PhysicalItem("prop_cs_burger_01",57005,new Vector3(0.16f, 0.01f, -0.04f),new Rotator(0.0f, 28.0f, 0.0f)), HealthGained = 10 },
            new ModItem("Bacon Triple Cheese Melt", "More meat AND more bacon", eConsumableType.Eat) {
                ModelItem = new PhysicalItem("prop_cs_burger_01",57005,new Vector3(0.16f, 0.01f, -0.04f),new Rotator(0.0f, 28.0f, 0.0f)), HealthGained = 10 },
            new ModItem("Jumbo Shake", "Almost a whole cow full of milk", eConsumableType.Drink) {
                ModelItem = new PhysicalItem("prop_food_juice01",57005,new Vector3(0.16f, 0.01f, -0.04f),new Rotator(0.0f, 28.0f, 0.0f)), HealthGained = 10 },


            //BurgerShot
            new ModItem("Money Shot Meal",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("prop_food_bs_burg1",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f))
                ,PackageItem = new PhysicalItem("prop_food_bs_tray_02"), HealthGained = 12},
            new ModItem("The Bleeder Meal","",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("prop_food_bs_burg1",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f))
                ,PackageItem = new PhysicalItem("prop_food_bs_tray_02"), HealthGained = 20},
            new ModItem("Torpedo Meal","Torpedo your hunger",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("prop_food_bs_burger2",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f))
                ,PackageItem = new PhysicalItem("prop_food_bs_tray_03"), HealthGained = 15},
            new ModItem("Meat Free Meal","For the bleeding hearts",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("prop_food_bs_burger2",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f))
                ,PackageItem = new PhysicalItem("prop_food_bs_tray_01"), HealthGained = 5},
            new ModItem("Freedom Fries",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("prop_food_bs_chips",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)), HealthGained = 5 },
            new ModItem("Double Shot Coffee",eConsumableType.Drink) {
                ModelItem = new PhysicalItem("prop_food_bs_coffee",57005,new Vector3(0.07f, -0.1f, -0.1f),new Rotator(-74.0f, 28.0f, 3.0f)), HealthGained = 5 },
            new ModItem("Liter of eCola",eConsumableType.Drink) {
                ModelItem = new PhysicalItem("prop_food_bs_juice01",57005,new Vector3(0.07f, -0.1f, -0.1f),new Rotator(-74.0f, 28.0f, 3.0f)), HealthGained = 5 },
            new ModItem("Liter of Sprunk",eConsumableType.Drink) {
                ModelItem = new PhysicalItem("prop_food_bs_juice01",57005,new Vector3(0.07f, -0.1f, -0.1f),new Rotator(-74.0f, 28.0f, 3.0f)), HealthGained = 5 },

            //Bite
            new ModItem("Gut Buster Sandwich",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("prop_food_burg2",57005,new Vector3(0.14f, 0.01f, -0.06f),new Rotator(0f, 0f, 0f)), HealthGained = 10 } ,
            new ModItem("Ham and Tuna Sandwich",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("prop_food_burg2",57005,new Vector3(0.14f, 0.01f, -0.06f),new Rotator(0f, 0f, 0f)), HealthGained = 10 } ,
            new ModItem("Chef's Salad",eConsumableType.Eat) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthGained = 5} ,

            //BeefyBills
            new ModItem("Megacheese Burger",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("prop_cs_burger_01",57005,new Vector3(0.16f, 0.01f, -0.04f),new Rotator(0.0f, 28.0f, 0.0f)), HealthGained = 10 },
            new ModItem("Double Burger",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("prop_cs_burger_01",57005,new Vector3(0.16f, 0.01f, -0.04f),new Rotator(0.0f, 28.0f, 0.0f)), HealthGained = 12 },
            new ModItem("Kingsize Burger",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("prop_cs_burger_01",57005,new Vector3(0.16f, 0.01f, -0.04f),new Rotator(0.0f, 28.0f, 0.0f)), HealthGained = 15 },
            new ModItem("Bacon Burger",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("prop_cs_burger_01",57005,new Vector3(0.16f, 0.01f, -0.04f),new Rotator(0.0f, 28.0f, 0.0f)), HealthGained = 17 },


            //Taco Bomb
            new ModItem("Breakfast Burrito",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("prop_food_bs_burger2",57005,new Vector3(0.14f, -0.02f, -0.04f),new Rotator(178.0f, 28.0f, 0.0f))
                ,PackageItem = new PhysicalItem("prop_food_bag1"), HealthGained = 12},
            new ModItem("Deep Fried Salad",eConsumableType.Eat) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthGained = 10},
            new ModItem("Beef Bazooka",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("prop_food_bs_burger2",57005,new Vector3(0.14f, -0.02f, -0.04f),new Rotator(178.0f, 28.0f, 0.0f))
                ,PackageItem = new PhysicalItem("prop_food_bag1"), HealthGained = 15},
            new ModItem("Chimichingado Chiquito",eConsumableType.Eat) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthGained = 10},
            new ModItem("Cheesy Meat Flappers",eConsumableType.Eat) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthGained = 10},
            new ModItem("Volcano Mudsplatter Nachos",eConsumableType.Eat) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthGained = 10},

            //WIgwam Menu
            new ModItem("Wigwam Burger",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("prop_cs_burger_01",57005,new Vector3(0.16f, 0.01f, -0.04f),new Rotator(0.0f, 28.0f, 0.0f)), HealthGained = 10 },
            new ModItem("Wigwam Cheeseburger",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("prop_cs_burger_01",57005,new Vector3(0.16f, 0.01f, -0.04f),new Rotator(0.0f, 28.0f, 0.0f)), HealthGained = 12 },
            new ModItem("Big Wig Burger",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("prop_cs_burger_01",57005,new Vector3(0.16f, 0.01f, -0.04f),new Rotator(0.0f, 28.0f, 0.0f)), HealthGained = 12 },

            //CB
            new ModItem("Cluckin' Little Meal",eConsumableType.Eat) {
                PackageItem = new PhysicalItem("prop_food_cb_tray_03"), HealthGained = 5 },
            new ModItem("Cluckin' Big Meal",eConsumableType.Eat) {
                PackageItem = new PhysicalItem("prop_food_cb_tray_02"), HealthGained = 10 } ,
            new ModItem("Cluckin' Huge Meal",eConsumableType.Eat) {
                PackageItem = new PhysicalItem("prop_food_cb_tray_02"), HealthGained = 15 } ,
            new ModItem("Wing Piece",eConsumableType.Eat) {
                PackageItem = new PhysicalItem("prop_food_cb_tray_03"), HealthGained = 10 },
            new ModItem("Little Peckers",eConsumableType.Eat) {
                PackageItem = new PhysicalItem("prop_food_cb_tray_03"), HealthGained = 5 },
            new ModItem("Balls & Rings",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("prop_food_burg3",57005,new Vector3(0.14f, -0.02f, -0.04f),new Rotator(178.0f, 28.0f, 0.0f)), HealthGained = 5 },
            new ModItem("Fowlburger",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("prop_food_burg1",57005,new Vector3(0.16f, 0.01f, -0.04f),new Rotator(0.0f, 28.0f, 0.0f)),
                PackageItem = new PhysicalItem("prop_food_burg3"), HealthGained = 10 } ,

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


            //FancyDeli
            new ModItem("Chicken Club Salad",eConsumableType.Eat) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthGained = 20 } ,
            new ModItem("Spicy Seafood Gumbo",eConsumableType.Eat) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthGained = 20 } ,
            new ModItem("Muffaletta",eConsumableType.Eat) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthGained = 20 } ,
            new ModItem("Zucchini Garden Pasta",eConsumableType.Eat) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthGained = 20 } ,
            new ModItem("Pollo Mexicano",eConsumableType.Eat) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthGained = 20 } ,
            new ModItem("Italian Cruz Po'boy",eConsumableType.Eat) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthGained = 20 } ,
            new ModItem("Chipotle Chicken Panini",eConsumableType.Eat) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthGained = 20 } ,


            //FancyFish
            new ModItem("Coconut Crusted Prawns",eConsumableType.Eat) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthGained = 20 } ,
            new ModItem("Crab and Shrimp Louie",eConsumableType.Eat) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthGained = 20 } ,
            new ModItem("Open-Faced Crab Melt",eConsumableType.Eat) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthGained = 20 } ,
            new ModItem("King Salmon",eConsumableType.Eat) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthGained = 20 } ,
            new ModItem("Ahi Tuna",eConsumableType.Eat) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthGained = 20 } ,
            new ModItem("Key Lime Pie",eConsumableType.Eat) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthGained = 20 } ,
            
            //FancyGeneric
            new ModItem("Smokehouse Burger",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("prop_cs_burger_01",57005,new Vector3(0.16f, 0.01f, -0.04f),new Rotator(0.0f, 28.0f, 0.0f)), HealthGained = 20 },
            new ModItem("Chicken Critters Basket",eConsumableType.Eat) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthGained = 20 } ,
            new ModItem("Prime Rib 16 oz",eConsumableType.Eat) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthGained = 20 } ,
            new ModItem("Bone-In Ribeye",eConsumableType.Eat) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthGained = 20 } ,
            new ModItem("Grilled Pork Chops",eConsumableType.Eat) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthGained = 20 } ,
            new ModItem("Grilled Shrimp",eConsumableType.Eat) {
                PackageItem = new PhysicalItem("prop_food_bag1") , HealthGained = 20} ,

            //Noodles
            new ModItem("Juek Suk tong Mandu",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("prop_ff_noodle_01",57005,new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0f, 0f, 0f)), HealthGained = 10 },
            new ModItem("Hayan Jam Pong",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("prop_ff_noodle_02",57005,new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0f, 0f, 0f)), HealthGained = 15 } ,
            new ModItem("Sal Gook Su Jam Pong",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("v_ret_247_noodle1",57005,new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0f, 0f, 0f)), HealthGained = 20 } ,
            new ModItem("Chul Pan Bokkeum Jam Pong",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("v_ret_247_noodle2",57005,new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0f, 0f, 0f)), HealthGained = 20 } ,
            new ModItem("Deul Gae Udon",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("v_ret_247_noodle3",57005,new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0f, 0f, 0f)), HealthGained = 20 } ,
            new ModItem("Dakgogo Bokkeum Bap",eConsumableType.Eat) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthGained = 20 } ,

            //Weed
            new ModItem("White Widow Preroll",eConsumableType.Smoke) {
                ModelItem = new PhysicalItem("p_amb_joint_01",57005, new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)), AmountPerPackage = 5, IntoxicantName = "Marijuana" },
            new ModItem("OG Kush Preroll",eConsumableType.Smoke) {
                ModelItem = new PhysicalItem("p_amb_joint_01",57005, new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)), AmountPerPackage = 5, IntoxicantName = "Marijuana" },
            new ModItem("Northern Lights Preroll",eConsumableType.Smoke) {
                ModelItem = new PhysicalItem("p_amb_joint_01",57005, new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)), AmountPerPackage = 5, IntoxicantName = "Marijuana" },
            new ModItem("White Widow Gram",eConsumableType.None) {
                ModelItem = new PhysicalItem("prop_cs_pills",57005,new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0f, 0f, 0f)) },
            new ModItem("OG Kush Gram",eConsumableType.None) {
                ModelItem = new PhysicalItem("prop_cs_pills",57005,new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0f, 0f, 0f)) },
            new ModItem("Northern Lights Gram",eConsumableType.None) {
                ModelItem = new PhysicalItem("prop_cs_pills",57005,new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0f, 0f, 0f)) },
            new ModItem("Bong",eConsumableType.None) {
                ModelItem = new PhysicalItem("prop_bong_01",57005,new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0f, 0f, 0f)) } ,






            new ModItem("Gram of Schwag",eConsumableType.None) {
                ModelItem = new PhysicalItem("prop_cs_pills",57005,new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0f, 0f, 0f)),IntoxicantName = "Marijuana" },
            new ModItem("Gram of Mids",eConsumableType.None) {
                ModelItem = new PhysicalItem("prop_cs_pills",57005,new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0f, 0f, 0f)),IntoxicantName = "Marijuana" },
            new ModItem("Gram of Dank",eConsumableType.None) {
                ModelItem = new PhysicalItem("prop_cs_pills",57005,new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0f, 0f, 0f)),IntoxicantName = "Marijuana" },

            new ModItem("SPANK",eConsumableType.Ingest) {
                ModelItem = new PhysicalItem("prop_cs_pills",57005,new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0f, 0f, 0f)),IntoxicantName = "SPANK" },
            new ModItem("Toilet Cleaner",eConsumableType.Drink) {
                ModelItem = new PhysicalItem("prop_detergent_01b",57005,new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0f, 0f, 0f)),IntoxicantName = "Toilet Cleaner" },
            new ModItem("Gram of Coke",eConsumableType.None) {
                ModelItem = new PhysicalItem("prop_cs_pills",57005,new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0f, 0f, 0f)),IntoxicantName = "Cocaine" },
            new ModItem("Gram of Meth",eConsumableType.None) {
                ModelItem = new PhysicalItem("prop_cs_pills",57005,new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0f, 0f, 0f)),IntoxicantName = "Meth" },


            new ModItem("Bull Shark Testosterone","More bite than bush elephant testosterone. Become more aggressive, hornier, and irresistible to women! The ultimate man!",eConsumableType.Ingest) {
                ModelItem = new PhysicalItem("prop_cs_pills",57005,new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0f, 0f, 0f)),IntoxicantName = "Bull Shark Testosterone" },
            new ModItem("Alco Patch","The Alco Patch. It's the same refreshing feeling of your favorite drink, but delivered transdermally and discreetly. Pick up the Alco Patch at your local pharmacy.",eConsumableType.Ingest) {
                ModelItem = new PhysicalItem("prop_cs_pills",57005,new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0f, 0f, 0f)),IntoxicantName = "Alco Patch" },
            new ModItem("Mollis","For outstanding erections. Get the performance you've always dreamed of",eConsumableType.Ingest) {
                ModelItem = new PhysicalItem("prop_cs_pills",57005,new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0f, 0f, 0f)),IntoxicantName = "Mollis" },
            new ModItem("Chesty","Cough suppressant manufactured by Good Aids Pharmacy. Gives 24-hour relief and is available in honey flavour.",eConsumableType.Ingest) {
                ModelItem = new PhysicalItem("prop_cs_pills",57005,new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0f, 0f, 0f)),IntoxicantName = "Chesty" },
            new ModItem("Equanox","Combats dissatisfaction, lethargy, depression, melancholy, sexual dysfunction. Equanox may cause nausea, loss of sleep, blurred vision, leakage, kidney problems and breathing irregularities.",eConsumableType.Ingest) {
                ModelItem = new PhysicalItem("prop_cs_pills",57005,new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0f, 0f, 0f)),IntoxicantName = "Equanox" },
            new ModItem("Zombix","Painkiller and antidepressant manufactured by O'Deas Pharmacy. ~n~'Go straight for the head.'",eConsumableType.Ingest) {
                ModelItem = new PhysicalItem("prop_cs_pills",57005,new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0f, 0f, 0f)),IntoxicantName = "Zombix" },
            //        WeedDealerMenu = new List<MenuItem>() {
            //new MenuItem("Brick Gram",6, 1),
            //new MenuItem("Mid Gram",9, 3),
            //new MenuItem("Dank Gram",12, 4),
            //new MenuItem("Joint",3, 1)};


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

        //generic single piull bottle prop_cs_pills

        //Sandwiches




        //PILLS

            //ng_proc_drug01a002


        //Cars & Motorcycles
        new ModItem("Albany Alpha*") { ModelItem = new PhysicalItem("alpha") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Albany Roosevelt*") { ModelItem = new PhysicalItem("btype") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Albany Fränken Stange*") { ModelItem = new PhysicalItem("btype2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Albany Roosevelt Valor*") { ModelItem = new PhysicalItem("btype3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Albany Buccaneer") { ModelItem = new PhysicalItem("buccaneer") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Albany Buccaneer Custom*") { ModelItem = new PhysicalItem("buccaneer2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Albany Cavalcade") { ModelItem = new PhysicalItem("cavalcade") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Albany Cavalcade 2") { ModelItem = new PhysicalItem("cavalcade2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Albany Emperor") { ModelItem = new PhysicalItem("emperor") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Albany Emperor 2") { ModelItem = new PhysicalItem("Emperor2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Albany Emperor 3") { ModelItem = new PhysicalItem("emperor3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Albany Hermes*") { ModelItem = new PhysicalItem("hermes") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Albany Lurcher*") { ModelItem = new PhysicalItem("lurcher") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Albany Manana") { ModelItem = new PhysicalItem("manana") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Albany Manana Custom*") { ModelItem = new PhysicalItem("manana2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Albany Primo") { ModelItem = new PhysicalItem("primo") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Albany Primo Custom*") { ModelItem = new PhysicalItem("primo2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Albany Virgo*") { ModelItem = new PhysicalItem("virgo") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Albany V-STR*") { ModelItem = new PhysicalItem("vstr") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Albany Washington") { ModelItem = new PhysicalItem("washington") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Annis Elegy Retro Custom*") { ModelItem = new PhysicalItem("elegy") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Annis Elegy RH8") { ModelItem = new PhysicalItem("elegy2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Annis Euros*") { ModelItem = new PhysicalItem("Euros") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Annis Hellion*") { ModelItem = new PhysicalItem("hellion") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Annis RE-7B*") { ModelItem = new PhysicalItem("le7b") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Annis Remus*") { ModelItem = new PhysicalItem("remus") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Annis S80RR*") { ModelItem = new PhysicalItem("s80") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Annis Savestra*") { ModelItem = new PhysicalItem("savestra") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Annis ZR350*") { ModelItem = new PhysicalItem("zr350") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Annis Apocalypse ZR380*") { ModelItem = new PhysicalItem("zr380") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Annis Future Shock ZR380*") { ModelItem = new PhysicalItem("zr3802") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Annis Nightmare ZR380*") { ModelItem = new PhysicalItem("zr3803") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Benefactor Apocalypse Bruiser*") { ModelItem = new PhysicalItem("bruiser") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Benefactor Future Shock Bruiser*") { ModelItem = new PhysicalItem("bruiser2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Benefactor Nightmare Bruiser*") { ModelItem = new PhysicalItem("bruiser3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Benefactor Dubsta") { ModelItem = new PhysicalItem("dubsta") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Benefactor Dubsta 2") { ModelItem = new PhysicalItem("dubsta2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Benefactor Dubsta 6x6*") { ModelItem = new PhysicalItem("dubsta3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Benefactor Feltzer") { ModelItem = new PhysicalItem("feltzer2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Benefactor Stirling GT*") { ModelItem = new PhysicalItem("feltzer3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Benefactor Glendale*") { ModelItem = new PhysicalItem("glendale") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Benefactor Glendale Custom*") { ModelItem = new PhysicalItem("glendale2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Benefactor Turreted Limo*") { ModelItem = new PhysicalItem("limo2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Benefactor BR8*") { ModelItem = new PhysicalItem("openwheel1") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Benefactor Panto*") { ModelItem = new PhysicalItem("panto") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Benefactor Schafter") { ModelItem = new PhysicalItem("schafter2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Benefactor Schafter V12*") { ModelItem = new PhysicalItem("schafter3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Benefactor Schafter LWB*") { ModelItem = new PhysicalItem("schafter4") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Benefactor Schafter V12 (Armored)*") { ModelItem = new PhysicalItem("schafter5") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Benefactor Schafter LWB (Armored)*") { ModelItem = new PhysicalItem("schafter6") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Benefactor Schwartzer") { ModelItem = new PhysicalItem("schwarzer") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Benefactor Serrano") { ModelItem = new PhysicalItem("serrano") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Benefactor Surano") { ModelItem = new PhysicalItem("Surano") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Benefactor XLS*") { ModelItem = new PhysicalItem("xls") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Benefactor XLS (Armored)*") { ModelItem = new PhysicalItem("xls2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Benefactor Krieger*") { ModelItem = new PhysicalItem("krieger") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Benefactor Schlagen GT*") { ModelItem = new PhysicalItem("schlagen") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Benefactor Streiter*") { ModelItem = new PhysicalItem("streiter") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Benefactor Terrorbyte*") { ModelItem = new PhysicalItem("terbyte") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("BF Injection") { ModelItem = new PhysicalItem("BfInjection") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("BF Bifta*") { ModelItem = new PhysicalItem("bifta") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("BF Club*") { ModelItem = new PhysicalItem("club") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("BF Dune Buggy") { ModelItem = new PhysicalItem("dune") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("BF Dune FAV*") { ModelItem = new PhysicalItem("dune3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("BF Raptor*") { ModelItem = new PhysicalItem("raptor") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("BF Surfer") { ModelItem = new PhysicalItem("SURFER") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("BF Surfer") { ModelItem = new PhysicalItem("Surfer2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("BF Weevil*") { ModelItem = new PhysicalItem("weevil") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Bollokan Prairie") { ModelItem = new PhysicalItem("prairie") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Bravado Banshee") { ModelItem = new PhysicalItem("banshee") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Bravado Banshee 900R*") { ModelItem = new PhysicalItem("banshee2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Bravado Bison") { ModelItem = new PhysicalItem("bison") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Bravado Bison 2") { ModelItem = new PhysicalItem("Bison2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Bravado Bison 3") { ModelItem = new PhysicalItem("Bison3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Bravado Buffalo") { ModelItem = new PhysicalItem("buffalo") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Bravado Buffalo S") { ModelItem = new PhysicalItem("buffalo2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Bravado Sprunk Buffalo") { ModelItem = new PhysicalItem("buffalo3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Bravado Duneloader") { ModelItem = new PhysicalItem("dloader") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Bravado Gauntlet") { ModelItem = new PhysicalItem("Gauntlet") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Bravado Redwood Gauntlet") { ModelItem = new PhysicalItem("gauntlet2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Bravado Gauntlet Classic*") { ModelItem = new PhysicalItem("gauntlet3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Bravado Gauntlet Hellfire*") { ModelItem = new PhysicalItem("gauntlet4") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Bravado Gauntlet Classic Custom*") { ModelItem = new PhysicalItem("gauntlet5") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Bravado Gresley") { ModelItem = new PhysicalItem("gresley") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Bravado Half-track*") { ModelItem = new PhysicalItem("halftrack") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Bravado Apocalypse Sasquatch*") { ModelItem = new PhysicalItem("monster3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Bravado Future Shock Sasquatch*") { ModelItem = new PhysicalItem("monster4") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Bravado Nightmare Sasquatch*") { ModelItem = new PhysicalItem("monster5") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Bravado Paradise*") { ModelItem = new PhysicalItem("paradise") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Bravado Rat-Truck*") { ModelItem = new PhysicalItem("ratloader2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Bravado Rumpo") { ModelItem = new PhysicalItem("rumpo") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Bravado Rumpo 2") { ModelItem = new PhysicalItem("rumpo2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Bravado Rumpo Custom*") { ModelItem = new PhysicalItem("rumpo3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Bravado Verlierer*") { ModelItem = new PhysicalItem("verlierer2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Bravado Youga") { ModelItem = new PhysicalItem("youga") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Bravado Youga Classic*") { ModelItem = new PhysicalItem("youga2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Bravado Youga Classic 4x4*") { ModelItem = new PhysicalItem("youga3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Brute Boxville") { ModelItem = new PhysicalItem("boxville") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Brute Boxville 3") { ModelItem = new PhysicalItem("boxville3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Brute Boxville 4*") { ModelItem = new PhysicalItem("boxville4") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Brute Camper") { ModelItem = new PhysicalItem("CAMPER") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Brute Pony") { ModelItem = new PhysicalItem("pony") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Brute Pony 2") { ModelItem = new PhysicalItem("pony2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Brute Stockade") { ModelItem = new PhysicalItem("stockade") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Brute Stockade 3") { ModelItem = new PhysicalItem("stockade3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Brute Tipper") { ModelItem = new PhysicalItem("TipTruck") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Canis Bodhi") { ModelItem = new PhysicalItem("Bodhi2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Canis Crusader") { ModelItem = new PhysicalItem("CRUSADER") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Canis Freecrawler*") { ModelItem = new PhysicalItem("freecrawler") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Canis Kalahari*") { ModelItem = new PhysicalItem("kalahari") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Canis Kamacho*") { ModelItem = new PhysicalItem("kamacho") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Canis Mesa") { ModelItem = new PhysicalItem("MESA") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Canis Mesa 2") { ModelItem = new PhysicalItem("mesa2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Canis Mesa 3") { ModelItem = new PhysicalItem("MESA3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Canis Seminole") { ModelItem = new PhysicalItem("Seminole") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Canis Seminole Frontier*") { ModelItem = new PhysicalItem("seminole2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Chariot Romero Hearse") { ModelItem = new PhysicalItem("romero") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Cheval Fugitive") { ModelItem = new PhysicalItem("fugitive") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Cheval Marshall") { ModelItem = new PhysicalItem("marshall") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Cheval Picador") { ModelItem = new PhysicalItem("picador") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Cheval Surge") { ModelItem = new PhysicalItem("surge") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Cheval Taipan*") { ModelItem = new PhysicalItem("taipan") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Coil Brawler*") { ModelItem = new PhysicalItem("brawler") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Coil Cyclone*") { ModelItem = new PhysicalItem("cyclone") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Coil Raiden*") { ModelItem = new PhysicalItem("raiden") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Coil Voltic") { ModelItem = new PhysicalItem("voltic") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Coil Rocket Voltic*") { ModelItem = new PhysicalItem("voltic2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Asea") { ModelItem = new PhysicalItem("asea") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Asea") { ModelItem = new PhysicalItem("asea2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Apocalypse Brutus*") { ModelItem = new PhysicalItem("brutus") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Future Shock Brutus*") { ModelItem = new PhysicalItem("brutus2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Nightmare Brutus*") { ModelItem = new PhysicalItem("brutus3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Burrito") { ModelItem = new PhysicalItem("Burrito") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Bugstars Burrito") { ModelItem = new PhysicalItem("burrito2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Burrito 3") { ModelItem = new PhysicalItem("burrito3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Burrito 4") { ModelItem = new PhysicalItem("Burrito4") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Burrito 5") { ModelItem = new PhysicalItem("burrito5") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Gang Burrito") { ModelItem = new PhysicalItem("gburrito") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Gang Burrito 2*") { ModelItem = new PhysicalItem("gburrito2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Granger") { ModelItem = new PhysicalItem("GRANGER") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Hotring Sabre*") { ModelItem = new PhysicalItem("hotring") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Impaler*") { ModelItem = new PhysicalItem("impaler") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Apocalypse Impaler*") { ModelItem = new PhysicalItem("impaler2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Future Shock Impaler*") { ModelItem = new PhysicalItem("impaler3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Nightmare Impaler*") { ModelItem = new PhysicalItem("impaler4") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Lifeguard") { ModelItem = new PhysicalItem("lguard") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Mamba*") { ModelItem = new PhysicalItem("mamba") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Moonbeam*") { ModelItem = new PhysicalItem("moonbeam") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Moonbeam Custom*") { ModelItem = new PhysicalItem("moonbeam2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse DR1*") { ModelItem = new PhysicalItem("openwheel2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Premier") { ModelItem = new PhysicalItem("premier") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Rancher XL") { ModelItem = new PhysicalItem("RancherXL") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Rancher XL 2") { ModelItem = new PhysicalItem("rancherxl2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Rhapsody*") { ModelItem = new PhysicalItem("rhapsody") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Sabre Turbo") { ModelItem = new PhysicalItem("sabregt") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Sabre Turbo Custom*") { ModelItem = new PhysicalItem("sabregt2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Scramjet*") { ModelItem = new PhysicalItem("scramjet") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Stallion") { ModelItem = new PhysicalItem("stalion") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Burger Shot Stallion") { ModelItem = new PhysicalItem("stalion2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Tampa*") { ModelItem = new PhysicalItem("tampa") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Drift Tampa*") { ModelItem = new PhysicalItem("tampa2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Weaponized Tampa*") { ModelItem = new PhysicalItem("tampa3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Tornado") { ModelItem = new PhysicalItem("tornado") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Tornado 2") { ModelItem = new PhysicalItem("tornado2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Tornado 3") { ModelItem = new PhysicalItem("tornado3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Tornado 4") { ModelItem = new PhysicalItem("tornado4") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Tornado Custom*") { ModelItem = new PhysicalItem("tornado5") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Tornado Rat Rod*") { ModelItem = new PhysicalItem("tornado6") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Tulip*") { ModelItem = new PhysicalItem("tulip") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Vamos*") { ModelItem = new PhysicalItem("vamos") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Vigero") { ModelItem = new PhysicalItem("vigero") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Voodoo Custom*") { ModelItem = new PhysicalItem("voodoo") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Voodoo") { ModelItem = new PhysicalItem("voodoo2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Yosemite*") { ModelItem = new PhysicalItem("yosemite") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Drift Yosemite*") { ModelItem = new PhysicalItem("yosemite2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Yosemite Rancher*") { ModelItem = new PhysicalItem("yosemite3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dewbauchee Exemplar") { ModelItem = new PhysicalItem("exemplar") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dewbauchee JB 700") { ModelItem = new PhysicalItem("jb700") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dewbauchee JB 700W*") { ModelItem = new PhysicalItem("jb7002") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dewbauchee Massacro*") { ModelItem = new PhysicalItem("massacro") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dewbauchee Massacro (Racecar)*") { ModelItem = new PhysicalItem("massacro2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dewbauchee Rapid GT") { ModelItem = new PhysicalItem("RapidGT") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dewbauchee Rapid GT 2") { ModelItem = new PhysicalItem("RapidGT2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dewbauchee Rapid GT Classic*") { ModelItem = new PhysicalItem("rapidgt3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dewbauchee Seven-70*") { ModelItem = new PhysicalItem("SEVEN70") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dewbauchee Specter*") { ModelItem = new PhysicalItem("SPECTER") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dewbauchee Specter Custom*") { ModelItem = new PhysicalItem("SPECTER2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dewbauchee Vagner*") { ModelItem = new PhysicalItem("vagner") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dinka Akuma") { ModelItem = new PhysicalItem("akuma") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dinka Blista") { ModelItem = new PhysicalItem("blista") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dinka Blista Compact") { ModelItem = new PhysicalItem("blista2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dinka Go Go Monkey Blista") { ModelItem = new PhysicalItem("blista3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dinka Double-T") { ModelItem = new PhysicalItem("double") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dinka Enduro*") { ModelItem = new PhysicalItem("enduro") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dinka Jester*") { ModelItem = new PhysicalItem("jester") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dinka Jester (Racecar)*") { ModelItem = new PhysicalItem("jester2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dinka Jester Classic*") { ModelItem = new PhysicalItem("jester3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dinka Jester RR*") { ModelItem = new PhysicalItem("jester4") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dinka Blista Kanjo*") { ModelItem = new PhysicalItem("kanjo") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dinka RT3000*") { ModelItem = new PhysicalItem("rt3000") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dinka Sugoi*") { ModelItem = new PhysicalItem("Sugoi") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dinka Thrust*") { ModelItem = new PhysicalItem("thrust") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dinka Verus*") { ModelItem = new PhysicalItem("verus") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dinka Veto Classic*") { ModelItem = new PhysicalItem("veto") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dinka Veto Modern*") { ModelItem = new PhysicalItem("veto2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dinka Vindicator*") { ModelItem = new PhysicalItem("vindicator") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dundreary Landstalker") { ModelItem = new PhysicalItem("landstalker") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dundreary Landstalker XL*") { ModelItem = new PhysicalItem("landstalker2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dundreary Regina") { ModelItem = new PhysicalItem("regina") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dundreary Stretch") { ModelItem = new PhysicalItem("stretch") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dundreary Virgo Classic Custom*") { ModelItem = new PhysicalItem("virgo2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dundreary Virgo Classic*") { ModelItem = new PhysicalItem("virgo3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Emperor Habanero") { ModelItem = new PhysicalItem("habanero") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Emperor ETR1*") { ModelItem = new PhysicalItem("sheava") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Emperor Vectre*") { ModelItem = new PhysicalItem("vectre") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Enus Cognoscenti 55*") { ModelItem = new PhysicalItem("cog55") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Enus Cognoscenti 55 (Armored)*") { ModelItem = new PhysicalItem("cog552") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Enus Cognoscenti Cabrio") { ModelItem = new PhysicalItem("cogcabrio") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Enus Cognoscenti*") { ModelItem = new PhysicalItem("cognoscenti") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Enus Cognoscenti (Armored)*") { ModelItem = new PhysicalItem("cognoscenti2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Enus Huntley S*") { ModelItem = new PhysicalItem("huntley") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Enus Paragon R*") { ModelItem = new PhysicalItem("paragon") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Enus Paragon R (Armored)*") { ModelItem = new PhysicalItem("paragon2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Enus Stafford*") { ModelItem = new PhysicalItem("stafford") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Enus Super Diamond") { ModelItem = new PhysicalItem("superd") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Enus Windsor*") { ModelItem = new PhysicalItem("windsor") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Enus Windsor Drop*") { ModelItem = new PhysicalItem("windsor2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Fathom FQ 2") { ModelItem = new PhysicalItem("fq2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Gallivanter Baller") { ModelItem = new PhysicalItem("Baller") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Gallivanter Baller 2") { ModelItem = new PhysicalItem("baller2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Gallivanter Baller LE*") { ModelItem = new PhysicalItem("baller3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Gallivanter Baller LE LWB*") { ModelItem = new PhysicalItem("baller4") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Gallivanter Baller LE (Armored)*") { ModelItem = new PhysicalItem("baller5") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Gallivanter Baller LE LWB (Armored)*") { ModelItem = new PhysicalItem("baller6") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Grotti Bestia GTS*") { ModelItem = new PhysicalItem("bestiagts") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Grotti Brioso R/A*") { ModelItem = new PhysicalItem("brioso") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Grotti Brioso 300*") { ModelItem = new PhysicalItem("brioso2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Grotti Carbonizzare") { ModelItem = new PhysicalItem("carbonizzare") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Grotti Cheetah") { ModelItem = new PhysicalItem("cheetah") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Grotti Cheetah Classic*") { ModelItem = new PhysicalItem("cheetah2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Grotti Furia*") { ModelItem = new PhysicalItem("furia") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Grotti GT500*") { ModelItem = new PhysicalItem("gt500") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Grotti Itali GTO*") { ModelItem = new PhysicalItem("italigto") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Grotti Itali RSX*") { ModelItem = new PhysicalItem("italirsx") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Grotti X80 Proto*") { ModelItem = new PhysicalItem("prototipo") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Grotti Stinger") { ModelItem = new PhysicalItem("stinger") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Grotti Stinger GT") { ModelItem = new PhysicalItem("stingergt") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Grotti Turismo Classic*") { ModelItem = new PhysicalItem("turismo2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Grotti Turismo R*") { ModelItem = new PhysicalItem("turismor") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Grotti Visione*") { ModelItem = new PhysicalItem("visione") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Hijak Khamelion") { ModelItem = new PhysicalItem("khamelion") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Hijak Ruston*") { ModelItem = new PhysicalItem("ruston") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("HVY Barracks Semi") { ModelItem = new PhysicalItem("BARRACKS2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("HVY Biff") { ModelItem = new PhysicalItem("Biff") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("HVY Dozer") { ModelItem = new PhysicalItem("bulldozer") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("HVY Cutter") { ModelItem = new PhysicalItem("cutter") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("HVY Dump") { ModelItem = new PhysicalItem("dump") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("HVY Forklift") { ModelItem = new PhysicalItem("FORKLIFT") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("HVY Insurgent Pick-Up*") { ModelItem = new PhysicalItem("insurgent") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("HVY Insurgent*") { ModelItem = new PhysicalItem("insurgent2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("HVY Insurgent Pick-Up Custom*") { ModelItem = new PhysicalItem("insurgent3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("HVY Menacer*") { ModelItem = new PhysicalItem("menacer") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("HVY Mixer") { ModelItem = new PhysicalItem("Mixer") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("HVY Mixer 2") { ModelItem = new PhysicalItem("Mixer2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("HVY Nightshark*") { ModelItem = new PhysicalItem("nightshark") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("HVY Apocalypse Scarab*") { ModelItem = new PhysicalItem("scarab") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("HVY Future Shock Scarab*") { ModelItem = new PhysicalItem("scarab2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("HVY Nightmare Scarab*") { ModelItem = new PhysicalItem("scarab3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Imponte Deluxo*") { ModelItem = new PhysicalItem("deluxo") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Imponte Dukes") { ModelItem = new PhysicalItem("dukes") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Imponte Duke O'Death") { ModelItem = new PhysicalItem("dukes2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Imponte Beater Dukes*") { ModelItem = new PhysicalItem("dukes3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Imponte Nightshade*") { ModelItem = new PhysicalItem("nightshade") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Imponte Phoenix") { ModelItem = new PhysicalItem("Phoenix") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Imponte Ruiner") { ModelItem = new PhysicalItem("ruiner") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Imponte Ruiner 2000*") { ModelItem = new PhysicalItem("ruiner2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Imponte Ruiner*") { ModelItem = new PhysicalItem("ruiner3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Invetero Coquette") { ModelItem = new PhysicalItem("coquette") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Invetero Coquette Classic*") { ModelItem = new PhysicalItem("coquette2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Invetero Coquette BlackFin*") { ModelItem = new PhysicalItem("coquette3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Invetero Coquette D10*") { ModelItem = new PhysicalItem("coquette4") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("JoBuilt Hauler") { ModelItem = new PhysicalItem("Hauler") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("JoBuilt Hauler Custom*") { ModelItem = new PhysicalItem("Hauler2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("JoBuilt Phantom") { ModelItem = new PhysicalItem("Phantom") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("JoBuilt Phantom Wedge*") { ModelItem = new PhysicalItem("phantom2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("JoBuilt Phantom Custom*") { ModelItem = new PhysicalItem("phantom3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("JoBuilt Rubble") { ModelItem = new PhysicalItem("Rubble") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Karin Asterope") { ModelItem = new PhysicalItem("asterope") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Karin BeeJay XL") { ModelItem = new PhysicalItem("BjXL") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Karin Calico GTF*") { ModelItem = new PhysicalItem("calico") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Karin Dilettante") { ModelItem = new PhysicalItem("dilettante") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Karin Dilettante 2") { ModelItem = new PhysicalItem("dilettante2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Karin Everon*") { ModelItem = new PhysicalItem("everon") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Karin Futo") { ModelItem = new PhysicalItem("futo") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Karin Futo GTX*") { ModelItem = new PhysicalItem("futo2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Karin Intruder") { ModelItem = new PhysicalItem("intruder") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Karin Kuruma*") { ModelItem = new PhysicalItem("kuruma") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Karin Kuruma (armored)*") { ModelItem = new PhysicalItem("kuruma2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Karin Previon*") { ModelItem = new PhysicalItem("previon") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Karin Rusty Rebel") { ModelItem = new PhysicalItem("Rebel") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Karin Rebel") { ModelItem = new PhysicalItem("rebel2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Karin Sultan") { ModelItem = new PhysicalItem("sultan") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Karin Sultan Classic*") { ModelItem = new PhysicalItem("sultan2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Karin Sultan RS Classic*") { ModelItem = new PhysicalItem("sultan3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Karin Sultan RS*") { ModelItem = new PhysicalItem("sultanrs") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Karin Technical*") { ModelItem = new PhysicalItem("technical") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Karin Technical Custom*") { ModelItem = new PhysicalItem("technical3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Karin 190z*") { ModelItem = new PhysicalItem("z190") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Lampadati Casco*") { ModelItem = new PhysicalItem("casco") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Lampadati Felon") { ModelItem = new PhysicalItem("felon") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Lampadati Felon GT") { ModelItem = new PhysicalItem("felon2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Lampadati Furore GT*") { ModelItem = new PhysicalItem("furoregt") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Lampadati Michelli GT*") { ModelItem = new PhysicalItem("michelli") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Lampadati Pigalle*") { ModelItem = new PhysicalItem("pigalle") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Lampadati Tropos Rallye*") { ModelItem = new PhysicalItem("tropos") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Lampadati Komoda*") { ModelItem = new PhysicalItem("komoda") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Lampadati Novak*") { ModelItem = new PhysicalItem("Novak") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Lampadati Tigon*") { ModelItem = new PhysicalItem("tigon") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Lampadati Viseris*") { ModelItem = new PhysicalItem("viseris") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("LCC Avarus*") { ModelItem = new PhysicalItem("avarus") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("LCC Hexer") { ModelItem = new PhysicalItem("hexer") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("LCC Innovation*") { ModelItem = new PhysicalItem("innovation") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("LCC Sanctus*") { ModelItem = new PhysicalItem("sanctus") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Maibatsu Manchez*") { ModelItem = new PhysicalItem("manchez") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Maibatsu Manchez Scout*") { ModelItem = new PhysicalItem("manchez2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Maibatsu Mule") { ModelItem = new PhysicalItem("Mule") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Maibatsu Mule") { ModelItem = new PhysicalItem("Mule2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Maibatsu Mule*") { ModelItem = new PhysicalItem("Mule3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Maibatsu Mule Custom*") { ModelItem = new PhysicalItem("mule4") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Maibatsu Penumbra") { ModelItem = new PhysicalItem("penumbra") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Maibatsu Penumbra FF*") { ModelItem = new PhysicalItem("penumbra2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Maibatsu Sanchez (livery)") { ModelItem = new PhysicalItem("Sanchez") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Maibatsu Sanchez") { ModelItem = new PhysicalItem("sanchez2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Mammoth Patriot") { ModelItem = new PhysicalItem("patriot") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Mammoth Patriot Stretch*") { ModelItem = new PhysicalItem("patriot2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Mammoth Squaddie*") { ModelItem = new PhysicalItem("squaddie") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Maxwell Asbo*") { ModelItem = new PhysicalItem("asbo") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Maxwell Vagrant*") { ModelItem = new PhysicalItem("vagrant") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("MTL Brickade*") { ModelItem = new PhysicalItem("brickade") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("MTL Apocalypse Cerberus*") { ModelItem = new PhysicalItem("cerberus") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("MTL Future Shock Cerberus*") { ModelItem = new PhysicalItem("cerberus2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("MTL Nightmare Cerberus*") { ModelItem = new PhysicalItem("cerberus3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("MTL Fire Truck") { ModelItem = new PhysicalItem("firetruk") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("MTL Flatbed") { ModelItem = new PhysicalItem("FLATBED") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("MTL Packer") { ModelItem = new PhysicalItem("Packer") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("MTL Pounder") { ModelItem = new PhysicalItem("Pounder") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("MTL Pounder Custom*") { ModelItem = new PhysicalItem("pounder2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("MTL Dune*") { ModelItem = new PhysicalItem("rallytruck") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("MTL Wastelander*") { ModelItem = new PhysicalItem("wastelander") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Nagasaki BF400*") { ModelItem = new PhysicalItem("bf400") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Nagasaki Blazer") { ModelItem = new PhysicalItem("blazer") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Nagasaki Blazer Lifeguard") { ModelItem = new PhysicalItem("blazer2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Nagasaki Hot Rod Blazer") { ModelItem = new PhysicalItem("blazer3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Nagasaki Street Blazer*") { ModelItem = new PhysicalItem("blazer4") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Nagasaki Carbon RS") { ModelItem = new PhysicalItem("carbonrs") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Nagasaki Chimera*") { ModelItem = new PhysicalItem("chimera") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Nagasaki Outlaw*") { ModelItem = new PhysicalItem("outlaw") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Nagasaki Shotaro*") { ModelItem = new PhysicalItem("shotaro") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Nagasaki Stryder*") { ModelItem = new PhysicalItem("Stryder") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Obey 8F Drafter*") { ModelItem = new PhysicalItem("drafter") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Obey 9F") { ModelItem = new PhysicalItem("ninef") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Obey 9F Cabrio") { ModelItem = new PhysicalItem("ninef2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Obey Omnis*") { ModelItem = new PhysicalItem("omnis") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Obey Rocoto") { ModelItem = new PhysicalItem("rocoto") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Obey Tailgater") { ModelItem = new PhysicalItem("tailgater") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Obey Tailgater S*") { ModelItem = new PhysicalItem("tailgater2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Ocelot Ardent*") { ModelItem = new PhysicalItem("ardent") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Ocelot F620") { ModelItem = new PhysicalItem("f620") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Ocelot R88*") { ModelItem = new PhysicalItem("formula2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Ocelot Jackal") { ModelItem = new PhysicalItem("jackal") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Ocelot Jugular*") { ModelItem = new PhysicalItem("jugular") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Ocelot Locust*") { ModelItem = new PhysicalItem("locust") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Ocelot Lynx*") { ModelItem = new PhysicalItem("lynx") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Ocelot Pariah*") { ModelItem = new PhysicalItem("pariah") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Ocelot Penetrator*") { ModelItem = new PhysicalItem("penetrator") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Ocelot Swinger*") { ModelItem = new PhysicalItem("swinger") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Ocelot XA-21*") { ModelItem = new PhysicalItem("xa21") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Overflod Autarch*") { ModelItem = new PhysicalItem("autarch") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Overflod Entity XXR*") { ModelItem = new PhysicalItem("entity2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Overflod Entity XF") { ModelItem = new PhysicalItem("entityxf") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Overflod Imorgon*") { ModelItem = new PhysicalItem("imorgon") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Overflod Tyrant*") { ModelItem = new PhysicalItem("tyrant") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pegassi Bati 801") { ModelItem = new PhysicalItem("bati") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pegassi Bati 801RR") { ModelItem = new PhysicalItem("bati2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pegassi Esskey*") { ModelItem = new PhysicalItem("esskey") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pegassi Faggio Sport*") { ModelItem = new PhysicalItem("faggio") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pegassi Faggio") { ModelItem = new PhysicalItem("faggio2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pegassi Faggio Mod*") { ModelItem = new PhysicalItem("faggio3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pegassi FCR 1000*") { ModelItem = new PhysicalItem("fcr") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pegassi FCR 1000 Custom*") { ModelItem = new PhysicalItem("fcr2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pegassi Infernus") { ModelItem = new PhysicalItem("infernus") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pegassi Infernus Classic*") { ModelItem = new PhysicalItem("infernus2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pegassi Monroe") { ModelItem = new PhysicalItem("monroe") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pegassi Oppressor*") { ModelItem = new PhysicalItem("oppressor") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pegassi Oppressor Mk II*") { ModelItem = new PhysicalItem("oppressor2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pegassi Osiris*") { ModelItem = new PhysicalItem("osiris") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pegassi Reaper*") { ModelItem = new PhysicalItem("reaper") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pegassi Ruffian") { ModelItem = new PhysicalItem("ruffian") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pegassi Tempesta*") { ModelItem = new PhysicalItem("tempesta") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pegassi Tezeract*") { ModelItem = new PhysicalItem("tezeract") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pegassi Torero*") { ModelItem = new PhysicalItem("torero") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pegassi Toros*") { ModelItem = new PhysicalItem("toros") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pegassi Vacca") { ModelItem = new PhysicalItem("vacca") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pegassi Vortex*") { ModelItem = new PhysicalItem("vortex") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pegassi Zentorno*") { ModelItem = new PhysicalItem("zentorno") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pegassi Zorrusso*") { ModelItem = new PhysicalItem("zorrusso") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pfister Comet") { ModelItem = new PhysicalItem("comet2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pfister Comet Retro Custom*") { ModelItem = new PhysicalItem("comet3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pfister Comet Safari*") { ModelItem = new PhysicalItem("comet4") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pfister Comet SR*") { ModelItem = new PhysicalItem("comet5") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pfister Comet S2*") { ModelItem = new PhysicalItem("comet6") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pfister Growler*") { ModelItem = new PhysicalItem("growler") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pfister Neon*") { ModelItem = new PhysicalItem("neon") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pfister 811*") { ModelItem = new PhysicalItem("pfister811") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Principe Deveste Eight*") { ModelItem = new PhysicalItem("deveste") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Principe Diabolus*") { ModelItem = new PhysicalItem("diablous") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Principe Diabolus Custom*") { ModelItem = new PhysicalItem("diablous2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Principe Lectro*") { ModelItem = new PhysicalItem("lectro") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Principe Nemesis") { ModelItem = new PhysicalItem("nemesis") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Progen Emerus*") { ModelItem = new PhysicalItem("emerus") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Progen PR4*") { ModelItem = new PhysicalItem("formula") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Progen GP1*") { ModelItem = new PhysicalItem("gp1") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Progen Itali GTB*") { ModelItem = new PhysicalItem("italigtb") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Progen Itali GTB Custom*") { ModelItem = new PhysicalItem("italigtb2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Progen T20*") { ModelItem = new PhysicalItem("t20") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Progen Tyrus*") { ModelItem = new PhysicalItem("tyrus") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("RUNE Cheburek*") { ModelItem = new PhysicalItem("cheburek") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Schyster Deviant*") { ModelItem = new PhysicalItem("deviant") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Schyster Fusilade") { ModelItem = new PhysicalItem("fusilade") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Shitzu Defiler*") { ModelItem = new PhysicalItem("defiler") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Shitzu Hakuchou*") { ModelItem = new PhysicalItem("hakuchou") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Shitzu Hakuchou Drag*") { ModelItem = new PhysicalItem("hakuchou2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Shitzu PCJ 600") { ModelItem = new PhysicalItem("pcj") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Shitzu Vader") { ModelItem = new PhysicalItem("Vader") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Stanley Fieldmaster") { ModelItem = new PhysicalItem("tractor2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Stanley Fieldmaster") { ModelItem = new PhysicalItem("tractor3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Truffade Adder") { ModelItem = new PhysicalItem("adder") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Truffade Nero*") { ModelItem = new PhysicalItem("nero") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Truffade Nero Custom*") { ModelItem = new PhysicalItem("nero2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Truffade Thrax*") { ModelItem = new PhysicalItem("thrax") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Truffade Z-Type") { ModelItem = new PhysicalItem("Ztype") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Ubermacht Oracle XS") { ModelItem = new PhysicalItem("oracle") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Ubermacht Oracle") { ModelItem = new PhysicalItem("oracle2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Ubermacht Revolter*") { ModelItem = new PhysicalItem("revolter") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Ubermacht SC1*") { ModelItem = new PhysicalItem("sc1") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Ubermacht Sentinel XS") { ModelItem = new PhysicalItem("sentinel") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Ubermacht Sentinel 2") { ModelItem = new PhysicalItem("sentinel2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Ubermacht Sentinel 3*") { ModelItem = new PhysicalItem("sentinel3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Ubermacht Zion") { ModelItem = new PhysicalItem("zion") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Ubermacht Zion Cabrio") { ModelItem = new PhysicalItem("zion2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Ubermacht Zion Classic*") { ModelItem = new PhysicalItem("zion3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Ubermacht Cypher*") { ModelItem = new PhysicalItem("cypher") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Ubermacht Rebla GTS*") { ModelItem = new PhysicalItem("rebla") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Benson") { ModelItem = new PhysicalItem("Benson") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Blade*") { ModelItem = new PhysicalItem("blade") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Bobcat XL") { ModelItem = new PhysicalItem("bobcatXL") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Bullet") { ModelItem = new PhysicalItem("bullet") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Caracara*") { ModelItem = new PhysicalItem("caracara") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Caracara 4x4*") { ModelItem = new PhysicalItem("caracara2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Chino*") { ModelItem = new PhysicalItem("chino") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Chino Custom*") { ModelItem = new PhysicalItem("chino2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Clique*") { ModelItem = new PhysicalItem("clique") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Contender*") { ModelItem = new PhysicalItem("contender") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Dominator") { ModelItem = new PhysicalItem("Dominator") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Pisswasser Dominator") { ModelItem = new PhysicalItem("dominator2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Dominator GTX*") { ModelItem = new PhysicalItem("dominator3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Apocalypse Dominator*") { ModelItem = new PhysicalItem("dominator4") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Future Shock Dominator*") { ModelItem = new PhysicalItem("dominator5") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Nightmare Dominator*") { ModelItem = new PhysicalItem("dominator6") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Dominator ASP*") { ModelItem = new PhysicalItem("dominator7") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Dominator GTT*") { ModelItem = new PhysicalItem("dominator8") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Ellie*") { ModelItem = new PhysicalItem("ellie") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Flash GT*") { ModelItem = new PhysicalItem("flashgt") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid FMJ*") { ModelItem = new PhysicalItem("fmj") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid GB200*") { ModelItem = new PhysicalItem("gb200") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Guardian*") { ModelItem = new PhysicalItem("guardian") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Hotknife") { ModelItem = new PhysicalItem("hotknife") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Hustler*") { ModelItem = new PhysicalItem("hustler") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Apocalypse Imperator*") { ModelItem = new PhysicalItem("imperator") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Future Shock Imperator*") { ModelItem = new PhysicalItem("imperator2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Nightmare Imperator*") { ModelItem = new PhysicalItem("imperator3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Minivan") { ModelItem = new PhysicalItem("minivan") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Minivan Custom*") { ModelItem = new PhysicalItem("minivan2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Monster*") { ModelItem = new PhysicalItem("monster") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Peyote") { ModelItem = new PhysicalItem("peyote") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Peyote Gasser*") { ModelItem = new PhysicalItem("peyote2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Peyote Custom*") { ModelItem = new PhysicalItem("peyote3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Radius") { ModelItem = new PhysicalItem("radi") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Retinue*") { ModelItem = new PhysicalItem("retinue") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Retinue Mk II*") { ModelItem = new PhysicalItem("retinue2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Riata*") { ModelItem = new PhysicalItem("riata") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Sadler") { ModelItem = new PhysicalItem("Sadler") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Sadler 2") { ModelItem = new PhysicalItem("sadler2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Sandking XL") { ModelItem = new PhysicalItem("sandking") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Sandking SWB") { ModelItem = new PhysicalItem("sandking2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Slamtruck*") { ModelItem = new PhysicalItem("slamtruck") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Slamvan*") { ModelItem = new PhysicalItem("slamvan") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Lost Slamvan*") { ModelItem = new PhysicalItem("slamvan2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Slamvan Custom*") { ModelItem = new PhysicalItem("slamvan3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Apocalypse Slamvan*") { ModelItem = new PhysicalItem("slamvan4") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Future Shock Slamvan*") { ModelItem = new PhysicalItem("slamvan5") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Nightmare Slamvan*") { ModelItem = new PhysicalItem("slamvan6") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Speedo") { ModelItem = new PhysicalItem("speedo") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Clown Van") { ModelItem = new PhysicalItem("speedo2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Speedo Custom*") { ModelItem = new PhysicalItem("speedo4") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Stanier") { ModelItem = new PhysicalItem("stanier") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Trophy Truck*") { ModelItem = new PhysicalItem("trophytruck") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Desert Raid*") { ModelItem = new PhysicalItem("trophytruck2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Winky*") { ModelItem = new PhysicalItem("winky") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vulcar Fagaloa*") { ModelItem = new PhysicalItem("fagaloa") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vulcar Ingot") { ModelItem = new PhysicalItem("ingot") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vulcar Nebula Turbo*") { ModelItem = new PhysicalItem("nebula") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vulcar Warrener*") { ModelItem = new PhysicalItem("warrener") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vulcar Warrener HKR*") { ModelItem = new PhysicalItem("warrener2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vysser Neo*") { ModelItem = new PhysicalItem("neo") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Weeny Dynasty*") { ModelItem = new PhysicalItem("Dynasty") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Weeny Issi") { ModelItem = new PhysicalItem("issi2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Weeny Issi Classic*") { ModelItem = new PhysicalItem("issi3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Weeny Apocalypse Issi*") { ModelItem = new PhysicalItem("issi4") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Weeny Future Shock Issi*") { ModelItem = new PhysicalItem("issi5") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Weeny Nightmare Issi*") { ModelItem = new PhysicalItem("issi6") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Weeny Issi Sport*") { ModelItem = new PhysicalItem("issi7") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Western Bagger") { ModelItem = new PhysicalItem("bagger") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Western Cliffhanger*") { ModelItem = new PhysicalItem("cliffhanger") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Western Daemon") { ModelItem = new PhysicalItem("daemon") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Western Daemon 2*") { ModelItem = new PhysicalItem("daemon2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Western Apocalypse Deathbike*") { ModelItem = new PhysicalItem("deathbike") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Western Future Shock Deathbike*") { ModelItem = new PhysicalItem("deathbike2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Western Nightmare Deathbike*") { ModelItem = new PhysicalItem("deathbike3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Western Gargoyle*") { ModelItem = new PhysicalItem("gargoyle") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Western Nightblade*") { ModelItem = new PhysicalItem("nightblade") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Western Rat Bike*") { ModelItem = new PhysicalItem("ratbike") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Western Rampant Rocket*") { ModelItem = new PhysicalItem("rrocket") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Western Sovereign*") { ModelItem = new PhysicalItem("sovereign") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Western Wolfsbane*") { ModelItem = new PhysicalItem("wolfsbane") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Western Zombie Bobber*") { ModelItem = new PhysicalItem("zombiea") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Western Zombie Chopper*") { ModelItem = new PhysicalItem("zombieb") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Willard Faction*") { ModelItem = new PhysicalItem("faction") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Willard Faction Custom*") { ModelItem = new PhysicalItem("faction2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Willard Faction Custom Donk*") { ModelItem = new PhysicalItem("faction3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Zirconium Journey") { ModelItem = new PhysicalItem("journey") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Zirconium Stratum") { ModelItem = new PhysicalItem("stratum") { Type = ePhysicalItemType.Vehicle }},
        
        //Heli
        new ModItem("Buckingham SuperVolito*") { ModelItem = new PhysicalItem("supervolito") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Buckingham SuperVolito Carbon*") { ModelItem = new PhysicalItem("supervolito2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Buckingham Swift*") { ModelItem = new PhysicalItem("swift") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Buckingham Swift Deluxe*") { ModelItem = new PhysicalItem("swift2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Buckingham Volatus*") { ModelItem = new PhysicalItem("volatus") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Mammoth Thruster*") { ModelItem = new PhysicalItem("thruster") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Nagasaki Havok*") { ModelItem = new PhysicalItem("havok") { Type = ePhysicalItemType.Vehicle }},

        //plane
        new ModItem("Buckingham Alpha-Z1*") { ModelItem = new PhysicalItem("alphaz1") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Buckingham Howard NX-25*") { ModelItem = new PhysicalItem("howard") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Buckingham Luxor") { ModelItem = new PhysicalItem("luxor") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Buckingham Luxor Deluxe*") { ModelItem = new PhysicalItem("luxor2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Buckingham Miljet*") { ModelItem = new PhysicalItem("Miljet") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Buckingham Nimbus*") { ModelItem = new PhysicalItem("nimbus") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Buckingham Pyro*") { ModelItem = new PhysicalItem("pyro") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Buckingham Shamal") { ModelItem = new PhysicalItem("Shamal") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Buckingham Vestra*") { ModelItem = new PhysicalItem("vestra") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Mammoth Avenger*") { ModelItem = new PhysicalItem("avenger") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Mammoth Avenger 2*") { ModelItem = new PhysicalItem("avenger2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Mammoth Dodo") { ModelItem = new PhysicalItem("dodo") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Mammoth Hydra*") { ModelItem = new PhysicalItem("hydra") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Mammoth Mogul*") { ModelItem = new PhysicalItem("mogul") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Mammoth Tula*") { ModelItem = new PhysicalItem("tula") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Nagasaki Ultralight*") { ModelItem = new PhysicalItem("microlight") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Western Besra*") { ModelItem = new PhysicalItem("besra") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Western Rogue*") { ModelItem = new PhysicalItem("rogue") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Western Seabreeze*") { ModelItem = new PhysicalItem("seabreeze") { Type = ePhysicalItemType.Vehicle }},

        //boat
        new ModItem("Dinka Marquis") { ModelItem = new PhysicalItem("marquis") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Lampadati Toro*") { ModelItem = new PhysicalItem("toro") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Lampadati Toro*") { ModelItem = new PhysicalItem("toro2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Nagasaki Dinghy") { ModelItem = new PhysicalItem("Dinghy") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Nagasaki Dinghy 2") { ModelItem = new PhysicalItem("dinghy2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Nagasaki Dinghy 3*") { ModelItem = new PhysicalItem("dinghy3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Nagasaki Dinghy 4*") { ModelItem = new PhysicalItem("dinghy4") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Nagasaki Weaponized Dinghy*") { ModelItem = new PhysicalItem("dinghy5") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pegassi Speeder*") { ModelItem = new PhysicalItem("speeder") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pegassi Speeder*") { ModelItem = new PhysicalItem("speeder2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Shitzu Jetmax") { ModelItem = new PhysicalItem("jetmax") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Shitzu Longfin*") { ModelItem = new PhysicalItem("longfin") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Shitzu Squalo") { ModelItem = new PhysicalItem("squalo") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Shitzu Suntrap") { ModelItem = new PhysicalItem("Suntrap") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Shitzu Tropic") { ModelItem = new PhysicalItem("tropic") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Shitzu Tropic*") { ModelItem = new PhysicalItem("tropic2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Speedophile Seashark") { ModelItem = new PhysicalItem("seashark") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Speedophile Seashark 2") { ModelItem = new PhysicalItem("seashark2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Speedophile Seashark 3*") { ModelItem = new PhysicalItem("seashark3") { Type = ePhysicalItemType.Vehicle }},
    };
    }

}

