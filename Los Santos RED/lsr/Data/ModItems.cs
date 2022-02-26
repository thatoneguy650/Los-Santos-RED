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
            new ModItem("Room: Single Twin","Cheapest room for the most discerning client",eConsumableType.Service) {ConsumeOnPurchase = true, MeasurementName = "Night" },
            new ModItem("Room: Single Queen","Clean sheets on request",eConsumableType.Service) {ConsumeOnPurchase = true, MeasurementName = "Night" },
            new ModItem("Room: Double Queen","Have a little company, but don't want to get too close?",eConsumableType.Service) {ConsumeOnPurchase = true, MeasurementName = "Night" },
            new ModItem("Room: Single King","Please clean off all mirrors after use",eConsumableType.Service) {ConsumeOnPurchase = true, MeasurementName = "Night" },



            new ModItem("Scrap Vehicle","Sell the vehicle for scrap",eConsumableType.Service) { ConsumeOnPurchase = true, MeasurementName = "Vehicle" },




            //new ModItem("Full",eConsumableType.Service),
            //new ModItem("Half And Half",eConsumableType.Service),
            //new ModItem("Head",eConsumableType.Service),
            //new ModItem("Handy",eConsumableType.Service),

            //Generic Tools
            new ModItem("Screwdriver","Might get you into some locked things") {
                ModelItem = new PhysicalItem("prop_tool_screwdvr01",57005,new Vector3(0.1170f, 0.0610f, 0.0150f), new Rotator(-47.199f, 166.62f, -19.9f)), ToolType = ToolTypes.Screwdriver },
            //new ModItem("Hammer","3 Year guarantee as standard. EASI-GRIP handle. It's hammer time!") {
            //    ModelItem = new PhysicalItem("prop_tool_hammer",57005,new Vector3(0.1170f, 0.0610f, 0.0150f), new Rotator(-47.199f, 166.62f, -19.9f)), ToolType = ToolTypes.Hammer  },
            new ModItem("Drill","2-Speed Battery Drill. Impact-resistant casing. Light, compact and easy to use.") {
                ModelItem = new PhysicalItem("prop_tool_drill",57005,new Vector3(0.1170f, 0.0610f, 0.0150f), new Rotator(-47.199f, 166.62f, -19.9f)), ToolType = ToolTypes.Drill  },
            new ModItem("Pliers","For mechanics, pipe bomb makers, and amateur dentists alike. When you really need to grab something.") {
                ModelItem = new PhysicalItem("prop_tool_pliers",57005,new Vector3(0.1170f, 0.0610f, 0.0150f), new Rotator(-47.199f, 166.62f, -19.9f)), ToolType = ToolTypes.Pliers  },
            new ModItem("Shovel","A lot of holes in the desert, and a lot of problems are buried in those holes. But you gotta do it right. I mean, you gotta have the hole already dug before you show up with a package in the trunk.") {
                ModelItem = new PhysicalItem("prop_tool_shovel",57005,new Vector3(0.1170f, 0.0610f, 0.0150f), new Rotator(-47.199f, 166.62f, -19.9f)) { IsLarge = true }, ToolType = ToolTypes.Shovel  },
            //new ModItem("Wrench","What harm could a loose bolt have?") {
            //    ModelItem = new PhysicalItem("prop_tool_wrench",57005,new Vector3(0.1170f, 0.0610f, 0.0150f), new Rotator(-47.199f, 166.62f, -19.9f)) { IsLarge = true }, ToolType = ToolTypes.Wrench   },
            new ModItem("DIC Lighter","A disposable lighter in production by Société Dic since 1973. Arson strongly discouraged") {
                ModelItem = new PhysicalItem("p_cs_lighter_01",57005,new Vector3(0.1170f, 0.0610f, 0.0150f), new Rotator(-47.199f, 166.62f, -19.9f)), ToolType = ToolTypes.Lighter , PercentLostOnUse = 0.01f },
            //new ModItem("Crowbar","Heavy-duty crowbar forged from high quality, tempered steel for that extra leverage you need to get the job done.") {
            //    ModelItem = new PhysicalItem("prop_ing_crowbar",57005,new Vector3(0.1170f, 0.0610f, 0.0150f), new Rotator(-47.199f, 166.62f, -19.9f)) { IsLarge = true }, ToolType = ToolTypes.Crowbar  },

            new ModItem("Bong","Also known as a water pipe") {
                ModelItem = new PhysicalItem("prop_bong_01",57005,new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0f, 0f, 0f)), ToolType = ToolTypes.Bong } ,




            new ModItem("DIC Lighter Ultra","A disposable lighter in production by Société Dic since 1973. Arson strongly discouraged. Long burn version.") {
                ModelItem = new PhysicalItem("p_cs_lighter_01",57005,new Vector3(0.1170f, 0.0610f, 0.0150f), new Rotator(-47.199f, 166.62f, -19.9f)), ToolType = ToolTypes.Lighter , PercentLostOnUse = 0.005f },
            new ModItem("Dippo Lighter","Want to have all the hassle of carrying a lighter only for it to be out of fluid when you need it? Dippo is for you!") {
                ModelItem = new PhysicalItem("v_res_tt_lighter",57005,new Vector3(0.1170f, 0.0610f, 0.0150f), new Rotator(-47.199f, 166.62f, -19.9f)), ToolType = ToolTypes.Lighter , PercentLostOnUse = 0.05f },
            new ModItem("DIC Lighter Silver","A disposable lighter in production by Société Dic since 1973. Arson strongly discouraged. Too poor for gold?") {
                ModelItem = new PhysicalItem("ex_prop_exec_lighter_01",57005,new Vector3(0.1170f, 0.0610f, 0.0150f), new Rotator(-47.199f, 166.62f, -19.9f)), ToolType = ToolTypes.Lighter , PercentLostOnUse = 0.02f },
            new ModItem("DIC Lighter Gold","A disposable lighter in production by Société Dic since 1973. Arson strongly discouraged. Golden so it must be good!") {
                ModelItem = new PhysicalItem("lux_prop_lighter_luxe",57005,new Vector3(0.1170f, 0.0610f, 0.0150f), new Rotator(-47.199f, 166.62f, -19.9f)), ToolType = ToolTypes.Lighter , PercentLostOnUse = 0.02f },
            ///p_cs_lighter_01.ydr, same as above
            /////v_res_tt_lighter.ydr zippo
            /////lux_prop_lighter_luxe.ydr kinda disposable shaped but gold
            //ex_prop_exec_lighter_01.ydr same as above but silver
            //Generic Food/Drink/SMoke
            new ModItem("Hot Dog","Your favorite mystery meat sold on street corners everywhere. Niko would be proud",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("prop_cs_hotdog_01",57005,new Vector3(0.14f, -0.02f, -0.04f),new Rotator(178.0f, 28.0f, 0.0f)), HealthGained = 10 },
            new ModItem("Hot Sausage","Get all your jokes out",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("prop_cs_hotdog_01",57005,new Vector3(0.14f, -0.02f, -0.04f),new Rotator(178.0f, 28.0f, 0.0f)), HealthGained = 10 },
            new ModItem("Hot Pretzel","You tie me up",eConsumableType.Eat) {HealthGained = 10 },
            new ModItem("3 Mini Pretzels","Like a pretzel, but smaller",eConsumableType.Eat) {HealthGained = 10 },
            new ModItem("Nuts","You're gonna love my nuts",eConsumableType.Eat) {HealthGained = 10 },
            new ModItem("Bottle of Raine Water","The water that rich people drink, and the main reason why there are now entire continents of plastic bottles floating in the ocean",eConsumableType.Drink) {
                ModelItem = new PhysicalItem("ba_prop_club_water_bottle",57005,new Vector3(0.12f, -0.1f, -0.07f),new Rotator(-70.0f, 0.0f, 0.0f)), HealthGained = 20 },


            new ModItem("Bottle of GREY Water","Expensive water that tastes worse than tap!",eConsumableType.Drink) {
                ModelItem = new PhysicalItem("h4_prop_battle_waterbottle_01a",57005,new Vector3(0.12f, -0.1f, -0.07f),new Rotator(-70.0f, 0.0f, 0.0f)), HealthGained = 20 },

            new ModItem("Burger","100% Certified Food",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("prop_cs_burger_01",57005,new Vector3(0.16f, 0.01f, -0.04f),new Rotator(0.0f, 28.0f, 0.0f)), HealthGained = 10 },
            new ModItem("Donut","MMMMMMM Donuts",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("prop_donut_01",57005,new Vector3(0.15f, 0.01f, -0.03f),new Rotator(-15.0f, 17.0f, 0.0f)), HealthGained = 10 } ,
            new ModItem("Slice of Pizza","Caution may be hot",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("v_res_tt_pizzaplate",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)), HealthGained = 10 } ,
            new ModItem("40 oz","Drink like a true thug!",eConsumableType.Drink) {
                ModelItem = new PhysicalItem("prop_cs_beer_bot_40oz",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)), IntoxicantName = "Alcohol" },
            new ModItem("Bagel Sandwich","Bagel with extras, what more do you need?",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("p_amb_bagel_01",57005,new Vector3(0.15f, 0.01f, -0.03f),new Rotator(-15.0f, 17.0f, 0.0f)), HealthGained = 12 } ,

            new ModItem("Bottle of JUNK Energy", "The Quick Fix!",eConsumableType.Drink) {
                ModelItem = new PhysicalItem("prop_energy_drink",57005,new Vector3(0.13f, -0.06f, -0.06f),new Rotator(-73.0f, 0.0f, 0.0f)) , HealthGained = 30},

            new ModItem("Can of eCola", "Deliciously Infectious!",eConsumableType.Drink) {
                ModelItem = new PhysicalItem("ng_proc_sodacan_01a",57005,new Vector3(0.13f, -0.06f, -0.06f),new Rotator(-73.0f, 0.0f, 0.0f)) , HealthGained = 10},
            new ModItem("Can of Sprunk", "Slurp Sprunk Mmm! Delicious",eConsumableType.Drink) {
                ModelItem = new PhysicalItem("ng_proc_sodacan_01b",57005,new Vector3(0.13f, -0.06f, -0.06f),new Rotator(-73.0f, 0.0f, 0.0f)), HealthGained = 10 },
            new ModItem("Can of Orang-O-Tang", "Orange AND Tang! Orang-O-Tang!",eConsumableType.Drink) {
                ModelItem = new PhysicalItem("prop_orang_can_01",57005,new Vector3(0.13f, -0.06f, -0.06f),new Rotator(-73.0f, 0.0f, 0.0f)), HealthGained = 10 },

            new ModItem("Carton of Milk", "Full Fat. Farmed and produced in U.S.A.",eConsumableType.Drink) { HealthGained = 10,
               //ModelItem = new PhysicalItem("v_res_tt_milk",57005,new Vector3(0.13f, -0.06f, -0.06f),new Rotator(-73.0f, 0.0f, 0.0f)),  
            },

            new ModItem("Cup of eCola", "Deliciously Infectious!",eConsumableType.Drink) {
                ModelItem = new PhysicalItem("prop_food_juice01",57005,new Vector3(0.07f, -0.1f, -0.1f),new Rotator(-74.0f, 28.0f, 3.0f)), HealthGained = 10 },
            new ModItem("Cup of Sprunk", "Slurp Sprunk Mmm! Delicious",eConsumableType.Drink) {
                ModelItem = new PhysicalItem("ng_proc_sodacup_01b",57005,new Vector3(0.07f, -0.1f, -0.1f),new Rotator(-74.0f, 28.0f, 3.0f)), HealthGained = 10 },
            new ModItem("Cup of Coffee","Finally something without sugar! Sugar on Request",eConsumableType.Drink) {
               ModelItem = new PhysicalItem("p_ing_coffeecup_02",57005,new Vector3(0.12f, -0.06f, -0.08f),new Rotator(-78.0f, 0.0f, 0.0f)), HealthGained = 10 }, //ModelItem = new PhysicalItem("prop_food_coffee",57005,new Vector3(0.12f, -0.06f, -0.08f),new Rotator(-78.0f, 0.0f, 0.0f)), HealthGained = 10 },
            
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
            new ModItem("Taco",eConsumableType.Eat) { HealthGained = 10 },
            new ModItem("Strawberry Rails Cereal","The breakfast food you snort!",eConsumableType.Eat) { HealthGained = 50,
                //ModelItem = new PhysicalItem("v_res_tt_cereal01",57005,new Vector3(0.14f, -0.02f, -0.04f),new Rotator(178.0f, 28.0f, 0.0f)),  
            } ,
            new ModItem("Crackles O' Dawn Cereal","Smile at the crack!",eConsumableType.Eat) { HealthGained = 60,
                //ModelItem = new PhysicalItem("v_res_tt_cereal02",57005,new Vector3(0.14f, -0.02f, -0.04f),new Rotator(178.0f, 28.0f, 0.0f)),  
            } ,
            new ModItem("White Bread","Extra white, with minimal taste.",eConsumableType.Eat) { HealthGained = 10, AmountPerPackage = 25,
                //ModelItem = new PhysicalItem("v_res_fa_bread03",57005,new Vector3(0.14f, -0.02f, -0.04f),new Rotator(178.0f, 28.0f, 0.0f)), ,
                //PackageItem = new PhysicalItem("v_res_fa_bread01",57005,new Vector3(0.14f, -0.02f, -0.04f),new Rotator(178.0f, 28.0f, 0.0f))
            } ,




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


            new ModItem("Can of Hoplivion Double IPA","So many hops it should be illegal.",eConsumableType.Drink) {
                ModelItem = new PhysicalItem("h4_prop_h4_can_beer_01a",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)), IntoxicantName = "Alcohol"},
            new ModItem("Can of Blarneys","Making your mouth feel lucky",eConsumableType.Drink) {
                //ModelItem = new PhysicalItem("v_res_tt_can01",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)),
                IntoxicantName = "Alcohol" },
            new ModItem("Can of Logger","A classic American tasteless, watery beer, made by Rednecks for Rednecks. Now Chinese owned",eConsumableType.Drink) {
                //ModelItem = new PhysicalItem("v_res_tt_can02",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)), 
                IntoxicantName = "Alcohol"},

            new ModItem("Sinsimito Tequila","Extra Anejo 100% De Agave. 42% Alcohol by volume",eConsumableType.Drink) {
                ModelItem = new PhysicalItem("h4_prop_h4_t_bottle_02a",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)), IntoxicantName = "Alcohol"},
            new ModItem("Cazafortuna Tequila","Tequila Anejo. 100% Blue Agave 40% Alcohol by volume",eConsumableType.Drink) {
                ModelItem = new PhysicalItem("h4_prop_h4_t_bottle_01a",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)), IntoxicantName = "Alcohol"},


            new ModItem("Sticky Rib Phat Chips","They are extra phat. Sticky Rib Flavor.",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("v_ret_ml_chips1",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)), HealthGained = 10 },
            new ModItem("Habanero Phat Chips","They are extra phat. Habanero flavor",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("v_ret_ml_chips2",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)), HealthGained = 10 },
            new ModItem("Supersalt Phat Chips","They are extra phat. Supersalt flavor.",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("v_ret_ml_chips3",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)), HealthGained = 10 },
            new ModItem("Big Cheese Phat Chips","They are extra phat. Big Cheese flavor.",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("v_ret_ml_chips4",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)), HealthGained = 10 },

            new ModItem("Ego Chaser Energy Bar","Contains 20,000 Calories! ~n~'It's all about you'",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("prop_choc_ego",57005,new Vector3(0.13f, 0.05f, -0.02f),new Rotator(25f, -11f, -95f)), HealthGained = 20 },
            new ModItem("King Size P's & Q's","The candy bar that kids and stoners love. EXTRA Large",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("prop_candy_pqs",57005,new Vector3(0.16f, 0.01f, -0.02f),new Rotator(-178f, -169f, 169f)), HealthGained = 15 },
            new ModItem("P's & Q's","The candy bar that kids and stoners love",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("prop_choc_pq",57005,new Vector3(0.12f, 0.02f, -0.02f),new Rotator(-178f, -169f, 79f)), HealthGained = 10 },
            new ModItem("Meteorite Bar","Dark chocolate with a GOOEY core",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("prop_choc_meto",57005,new Vector3(0.12f, 0.03f, -0.02f),new Rotator(169f, 170f, 76f)), HealthGained = 10 },

            new ModItem("Redwood Regular", "Tobacco products for real men who don't go to the doctors or read fear-mongering, left-wing so-called medical propaganda",eConsumableType.Smoke) {
                ModelItem = new PhysicalItem("ng_proc_cigarette01a",57005, new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)),
                PackageItem = new PhysicalItem("v_ret_ml_cigs",57005, new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)),
                AmountPerPackage = 20, RequiredToolType = ToolTypes.Lighter },
            new ModItem("Redwood Mild", "Tobacco products for real men who don't go to the doctors or read fear-mongering, left-wing so-called medical propaganda. Milder version",eConsumableType.Smoke) {
                ModelItem = new PhysicalItem("ng_proc_cigarette01a",57005, new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)),
                PackageItem = new PhysicalItem("v_ret_ml_cigs2",57005, new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)),AmountPerPackage = 20, RequiredToolType = ToolTypes.Lighter },
            new ModItem("Debonaire", "Tobacco products marketed at the more sophisticated smoker, whoever that is",eConsumableType.Smoke) {
                ModelItem = new PhysicalItem("ng_proc_cigarette01a",57005, new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)),
                PackageItem = new PhysicalItem("v_ret_ml_cigs3",57005, new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)),AmountPerPackage = 20, RequiredToolType = ToolTypes.Lighter },
            new ModItem("Debonaire Menthol", "Tobacco products marketed at the more sophisticated smoker, whoever that is. With Menthol!",eConsumableType.Smoke) {
                ModelItem = new PhysicalItem("ng_proc_cigarette01a",57005, new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)),
                PackageItem = new PhysicalItem("v_ret_ml_cigs4",57005, new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)),AmountPerPackage = 20, RequiredToolType = ToolTypes.Lighter },
            new ModItem("Caradique", "Fine Napoleon Cigarettes",eConsumableType.Smoke) {
                ModelItem = new PhysicalItem("ng_proc_cigarette01a",57005, new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)),
                PackageItem = new PhysicalItem("v_ret_ml_cigs5",57005, new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)),AmountPerPackage = 20, RequiredToolType = ToolTypes.Lighter },
            new ModItem("69 Brand","Don't let an embargo stop you",eConsumableType.Smoke) {
                ModelItem = new PhysicalItem("ng_proc_cigarette01a",57005, new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)),
                PackageItem = new PhysicalItem("v_ret_ml_cigs6",57005, new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)),AmountPerPackage = 20, RequiredToolType = ToolTypes.Lighter },
            new ModItem("Joint","Weed in rolled form",eConsumableType.Smoke) {
                ModelItem = new PhysicalItem("p_amb_joint_01",57005, new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)), IntoxicantName = "Marijuana", RequiredToolType = ToolTypes.Lighter },
            new ModItem("Estancia Cigar","Medium Cut. Hand Rolled.",eConsumableType.Smoke) {
                ModelItem = new PhysicalItem("prop_cigar_02",57005, new Vector3(0.17f, 0.02f, 0.0f),new Rotator(0.0f, -78f, 0f)),
                PackageItem = new PhysicalItem("p_cigar_pack_02_s",57005, new Vector3(0.17f, 0.02f, 0.0f),new Rotator(0.0f, -78f, 0f)),AmountPerPackage = 20, RequiredToolType = ToolTypes.Lighter },
            new ModItem("ElectroToke Vape","The Electrotoke uses highly sophisticated micro-molecule atomization technology to make the ingestion of hard drugs healthy, dscreet, pleasurable and, best of all, completely safe.",eConsumableType.Smoke) {
                ModelItem = new PhysicalItem("h4_prop_battle_vape_01",57005, new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)), IntoxicantName = "Marijuana", PercentLostOnUse = 0.05f },        


            //h4_prop_battle_vape_01.ydr

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
                ,PackageItem = new PhysicalItem("prop_food_bs_tray_02"), HealthGained = 12, ConsumeOnPurchase = true},
            new ModItem("The Bleeder Meal","",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("prop_food_bs_burg1",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f))
                ,PackageItem = new PhysicalItem("prop_food_bs_tray_02"), HealthGained = 20, ConsumeOnPurchase = true},
            new ModItem("Torpedo Meal","Torpedo your hunger",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("prop_food_bs_burger2",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f))
                ,PackageItem = new PhysicalItem("prop_food_bs_tray_03"), HealthGained = 15, ConsumeOnPurchase = true},
            new ModItem("Meat Free Meal","For the bleeding hearts",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("prop_food_bs_burger2",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f))
                ,PackageItem = new PhysicalItem("prop_food_bs_tray_01"), HealthGained = 5, ConsumeOnPurchase = true},
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
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthGained = 5, ConsumeOnPurchase = true} ,

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
                ,PackageItem = new PhysicalItem("prop_food_bag1"), HealthGained = 12, ConsumeOnPurchase = true},
            new ModItem("Deep Fried Salad",eConsumableType.Eat) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthGained = 10, ConsumeOnPurchase = true},
            new ModItem("Beef Bazooka",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("prop_food_bs_burger2",57005,new Vector3(0.14f, -0.02f, -0.04f),new Rotator(178.0f, 28.0f, 0.0f))
                ,PackageItem = new PhysicalItem("prop_food_bag1"), HealthGained = 15, ConsumeOnPurchase = true},
            new ModItem("Chimichingado Chiquito",eConsumableType.Eat) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthGained = 10, ConsumeOnPurchase = true},
            new ModItem("Cheesy Meat Flappers",eConsumableType.Eat) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthGained = 10, ConsumeOnPurchase = true},
            new ModItem("Volcano Mudsplatter Nachos",eConsumableType.Eat) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthGained = 10, ConsumeOnPurchase = true},

            //WIgwam Menu
            new ModItem("Wigwam Burger",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("prop_cs_burger_01",57005,new Vector3(0.16f, 0.01f, -0.04f),new Rotator(0.0f, 28.0f, 0.0f)), HealthGained = 10 },
            new ModItem("Wigwam Cheeseburger",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("prop_cs_burger_01",57005,new Vector3(0.16f, 0.01f, -0.04f),new Rotator(0.0f, 28.0f, 0.0f)), HealthGained = 12 },
            new ModItem("Big Wig Burger",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("prop_cs_burger_01",57005,new Vector3(0.16f, 0.01f, -0.04f),new Rotator(0.0f, 28.0f, 0.0f)), HealthGained = 12 },

            //CB
            new ModItem("Cluckin' Little Meal","May contain meat",eConsumableType.Eat) {
                PackageItem = new PhysicalItem("prop_food_cb_tray_03"), HealthGained = 5, ConsumeOnPurchase = true },
            new ModItem("Cluckin' Big Meal","200% bigger breasts",eConsumableType.Eat) {
                PackageItem = new PhysicalItem("prop_food_cb_tray_02"), HealthGained = 10, ConsumeOnPurchase = true } ,
            new ModItem("Cluckin' Huge Meal",eConsumableType.Eat) {
                PackageItem = new PhysicalItem("prop_food_cb_tray_02"), HealthGained = 15, ConsumeOnPurchase = true } ,
            new ModItem("Wing Piece",eConsumableType.Eat) {
                PackageItem = new PhysicalItem("prop_food_cb_tray_03"), HealthGained = 10, ConsumeOnPurchase = true },
            new ModItem("Little Peckers",eConsumableType.Eat) {
                PackageItem = new PhysicalItem("prop_food_cb_tray_03"), HealthGained = 5 , ConsumeOnPurchase = true},
            new ModItem("Balls & Rings",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("prop_food_burg3",57005,new Vector3(0.14f, -0.02f, -0.04f),new Rotator(178.0f, 28.0f, 0.0f)), HealthGained = 5, ConsumeOnPurchase = true },
            new ModItem("Fowlburger",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("prop_food_burg1",57005,new Vector3(0.16f, 0.01f, -0.04f),new Rotator(0.0f, 28.0f, 0.0f)),
                PackageItem = new PhysicalItem("prop_food_burg3"), HealthGained = 10 } ,


            //Bean Machine
            new ModItem("High Noon Coffee","Drip coffee, carbonated water, fruit syrup and taurine.",eConsumableType.Drink) {
               ModelItem = new PhysicalItem("p_ing_coffeecup_01",57005,new Vector3(0.12f, -0.06f, -0.08f),new Rotator(-78.0f, 0.0f, 0.0f)), HealthGained = 10 },
            new ModItem("The Eco-ffee","Decaf light, rain forest rain, saved whale milk, chemically reclaimed freerange organic tofu, and recycled brown sugar",eConsumableType.Drink) {
               ModelItem = new PhysicalItem("p_ing_coffeecup_01",57005,new Vector3(0.12f, -0.06f, -0.08f),new Rotator(-78.0f, 0.0f, 0.0f)), HealthGained = 12 },
            new ModItem("Speedball Coffee","Caffeine tripe-shot, guarana, bat guano, and mate.",eConsumableType.Drink) {
               ModelItem = new PhysicalItem("p_ing_coffeecup_01",57005,new Vector3(0.12f, -0.06f, -0.08f),new Rotator(-78.0f, 0.0f, 0.0f)), HealthGained = 15 },
            new ModItem("Gunkacchino Coffee","Caffeine, refined sugar, trans fat, high-fructose corn syrup, and cheesecake base.",eConsumableType.Drink) {
               ModelItem = new PhysicalItem("p_ing_coffeecup_01",57005,new Vector3(0.12f, -0.06f, -0.08f),new Rotator(-78.0f, 0.0f, 0.0f)), HealthGained = 17 },
            new ModItem("Bratte Coffee","Double shot latte, and 100 pumps of caramel.",eConsumableType.Drink) {
               ModelItem = new PhysicalItem("p_ing_coffeecup_01",57005,new Vector3(0.12f, -0.06f, -0.08f),new Rotator(-78.0f, 0.0f, 0.0f)), HealthGained = 5 },
            new ModItem("Flusher Coffee","Caffeine, organic castor oil, concanetrated OJ, chicken vindaldo, and senna pods.",eConsumableType.Drink) {
               ModelItem = new PhysicalItem("p_ing_coffeecup_01",57005,new Vector3(0.12f, -0.06f, -0.08f),new Rotator(-78.0f, 0.0f, 0.0f)), HealthGained = 10 },
            new ModItem("Caffeagra Coffee","Caffeine (Straight up), rhino horn, oyster shell, and sildenafil citrate.",eConsumableType.Drink) {
               ModelItem = new PhysicalItem("p_ing_coffeecup_01",57005,new Vector3(0.12f, -0.06f, -0.08f),new Rotator(-78.0f, 0.0f, 0.0f)), HealthGained = 17 },
            new ModItem("Big Fruit Smoothie","Frothalot, watermel, carbonated water, taurine, and fruit syrup.",eConsumableType.Drink) {
               ModelItem = new PhysicalItem("p_ing_coffeecup_01",57005,new Vector3(0.12f, -0.06f, -0.08f),new Rotator(-78.0f, 0.0f, 0.0f)), HealthGained = 15 },


            //Viceroy
            new ModItem("City View King","Standard room with a view of the city",eConsumableType.Service) {ConsumeOnPurchase = true, MeasurementName = "Night" },
            new ModItem("City View Deluxe King","Deluxe room with view of the city.",eConsumableType.Service) {ConsumeOnPurchase = true, MeasurementName = "Night" },
            //new ModItem("Partial Ocean View King","Standard room with a partial view of the ocean.",eConsumableType.Service) {ConsumeOnPurchase = true, MeasurementName = "Night" },
            new ModItem("Ocean View King","Standard room a full view of the ocean. ",eConsumableType.Service) {ConsumeOnPurchase = true, MeasurementName = "Night" },
           // new ModItem("City View Two Bedded Room","Standard room with a view of the city",eConsumableType.Service) {ConsumeOnPurchase = true, MeasurementName = "Night" },
            new ModItem("Grande King","XL Deluxe room with plenty of space and amenities.",eConsumableType.Service) {ConsumeOnPurchase = true, MeasurementName = "Night" },
            new ModItem("Grande Ocean View King","XL Deluxe room with with plenty of space and amenities and a view of the ocean",eConsumableType.Service) {ConsumeOnPurchase = true, MeasurementName = "Night" },
           // new ModItem("Empire Suite","",eConsumableType.Service) {ConsumeOnPurchase = true, MeasurementName = "Night" },
            new ModItem("Monarch Suite","Penthouse suite, reserved for the most discerning tastes",eConsumableType.Service) {ConsumeOnPurchase = true, MeasurementName = "Night" },

            //Generic
            //FancyDeli
            new ModItem("Chicken Club Salad",eConsumableType.Eat) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthGained = 20, ConsumeOnPurchase = true } ,
            new ModItem("Spicy Seafood Gumbo",eConsumableType.Eat) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthGained = 20, ConsumeOnPurchase = true } ,
            new ModItem("Muffaletta",eConsumableType.Eat) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthGained = 20, ConsumeOnPurchase = true } ,
            new ModItem("Zucchini Garden Pasta",eConsumableType.Eat) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthGained = 20, ConsumeOnPurchase = true } ,
            new ModItem("Pollo Mexicano",eConsumableType.Eat) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthGained = 20, ConsumeOnPurchase = true } ,
            new ModItem("Italian Cruz Po'boy",eConsumableType.Eat) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthGained = 20, ConsumeOnPurchase = true } ,
            new ModItem("Chipotle Chicken Panini",eConsumableType.Eat) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthGained = 20, ConsumeOnPurchase = true } ,


            //FancyFish
            new ModItem("Coconut Crusted Prawns",eConsumableType.Eat) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthGained = 20, ConsumeOnPurchase = true } ,
            new ModItem("Crab and Shrimp Louie",eConsumableType.Eat) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthGained = 20, ConsumeOnPurchase = true } ,
            new ModItem("Open-Faced Crab Melt",eConsumableType.Eat) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthGained = 20, ConsumeOnPurchase = true } ,
            new ModItem("King Salmon",eConsumableType.Eat) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthGained = 20, ConsumeOnPurchase = true } ,
            new ModItem("Ahi Tuna",eConsumableType.Eat) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthGained = 20, ConsumeOnPurchase = true } ,
            new ModItem("Key Lime Pie",eConsumableType.Eat) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthGained = 20, ConsumeOnPurchase = true } ,
            
            //FancyGeneric
            new ModItem("Smokehouse Burger",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("prop_cs_burger_01",57005,new Vector3(0.16f, 0.01f, -0.04f),new Rotator(0.0f, 28.0f, 0.0f)), HealthGained = 20, ConsumeOnPurchase = true },
            new ModItem("Chicken Critters Basket",eConsumableType.Eat) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthGained = 20, ConsumeOnPurchase = true } ,
            new ModItem("Prime Rib 16 oz",eConsumableType.Eat) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthGained = 20, ConsumeOnPurchase = true } ,
            new ModItem("Bone-In Ribeye",eConsumableType.Eat) {
                PackageItem = new PhysicalItem("prop_cs_steak"), HealthGained = 20, ConsumeOnPurchase = true } ,
            new ModItem("Grilled Pork Chops",eConsumableType.Eat) {
                PackageItem = new PhysicalItem("prop_food_bag1"), HealthGained = 20, ConsumeOnPurchase = true } ,
            new ModItem("Grilled Shrimp",eConsumableType.Eat) {
                PackageItem = new PhysicalItem("prop_food_bag1") , HealthGained = 20, ConsumeOnPurchase = true } ,

            //Noodles
            new ModItem("Juek Suk tong Mandu",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("prop_ff_noodle_01",57005,new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0f, 0f, 0f)), HealthGained = 10 },
            new ModItem("Hayan Jam Pong",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("prop_ff_noodle_02",57005,new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0f, 0f, 0f)), HealthGained = 15 } ,
            new ModItem("Sal Gook Su Jam Pong",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("prop_ff_noodle_01",57005,new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0f, 0f, 0f)), HealthGained = 20 } ,
            new ModItem("Chul Pan Bokkeum Jam Pong",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("prop_ff_noodle_02",57005,new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0f, 0f, 0f)), HealthGained = 20 } ,
            new ModItem("Deul Gae Udon",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("prop_ff_noodle_02",57005,new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0f, 0f, 0f)), HealthGained = 20 } ,
            new ModItem("Dakgogo Bokkeum Bap",eConsumableType.Eat) {
                ModelItem = new PhysicalItem("prop_ff_noodle_01",57005,new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0f, 0f, 0f)), HealthGained = 20 } ,

            //Weed
            new ModItem("White Widow","Among the most famous strains worldwide is White Widow, a balanced hybrid first bred in the Netherlands by Green House Seeds.",eConsumableType.Smoke) {
                ModelItem = new PhysicalItem("p_amb_joint_01",57005, new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)),PackageItem = new PhysicalItem("prop_weed_bottle",57005,new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0f, 0f, 0f)), PercentLostOnUse = 0.25f, MeasurementName = "Gram", IntoxicantName = "Marijuana", RequiredToolType = ToolTypes.Lighter },
            new ModItem("OG Kush","OG Kush, also known as 'Premium OG Kush', was first cultivated in Florida in the early '90s when a marijuana strain from Northern California was supposedly crossed with Chemdawg, Lemon Thai and a Hindu Kush plant from Amsterdam.",eConsumableType.Smoke) {
                ModelItem = new PhysicalItem("p_amb_joint_01",57005, new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)),PackageItem = new PhysicalItem("prop_weed_bottle",57005,new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0f, 0f, 0f)), PercentLostOnUse = 0.25f, MeasurementName = "Gram", IntoxicantName = "Marijuana", RequiredToolType = ToolTypes.Lighter },
            new ModItem("Northern Lights","Northern Lights, also known as 'NL', is an indica marijuana strain made by crossing Afghani with Thai.",eConsumableType.Smoke) {
                ModelItem = new PhysicalItem("p_amb_joint_01",57005, new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)),PackageItem = new PhysicalItem("prop_weed_bottle",57005,new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0f, 0f, 0f)), PercentLostOnUse = 0.25f, MeasurementName = "Gram", IntoxicantName = "Marijuana", RequiredToolType = ToolTypes.Lighter },

            //new ModItem("Schwag Weed","Was considered good in the 1970s at least",eConsumableType.Smoke) {
            //    ModelItem = new PhysicalItem("p_amb_joint_01",57005, new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)),PackageItem = new PhysicalItem("sf_prop_sf_bag_weed_01a",57005,new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0f, 0f, 0f)), PercentLostOnUse = 0.25f, MeasurementName = "Gram", IntoxicantName = "Marijuana", RequiredToolType = ToolTypes.Lighter },
            //new ModItem("Mid Weed","One step above brick pack",eConsumableType.Smoke) {
            //    ModelItem = new PhysicalItem("p_amb_joint_01",57005, new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)),PackageItem = new PhysicalItem("sf_prop_sf_bag_weed_01a",57005,new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0f, 0f, 0f)), PercentLostOnUse = 0.25f, MeasurementName = "Gram", IntoxicantName = "Marijuana", RequiredToolType = ToolTypes.Lighter },
            new ModItem("Marijuana","Little Jacob Tested, Truth Approved",eConsumableType.Smoke) {
                ModelItem = new PhysicalItem("p_amb_joint_01",57005, new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f))
                ,PackageItem = new PhysicalItem("sf_prop_sf_bag_weed_01a",57005,new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0f, 0f, 0f)), PercentLostOnUse = 0.25f, MeasurementName = "Gram", IntoxicantName = "Marijuana", RequiredToolType = ToolTypes.Lighter },

            new ModItem("SPANK","You looking for some fun? a little.. hmmm? Some SPANK?",eConsumableType.Ingest) {
                ModelItem = new PhysicalItem("prop_cs_pills",57005,new Vector3(0.12f, 0.03f, 0.0f),new Rotator(-76f, 0f, 0f)),IntoxicantName = "SPANK", PercentLostOnUse = 0.5f, MeasurementName = "Gram" },
            new ModItem("Toilet Cleaner","Meth brought you forbidden fruits of incest. Bath salts brought you the taboo joys of cannibalism. It's time to step things up a level. The hot new legal high that takes you to places you never imagined and leaves you forever changed - Toilet Cleaner.",eConsumableType.Ingest) {
                ModelItem = new PhysicalItem("prop_cs_pills",57005,new Vector3(0.12f, 0.03f, 0.0f),new Rotator(-76f, 0f, 0f)),IntoxicantName = "Toilet Cleaner" },
            new ModItem("Cocaine","Also known as coke, crack, girl, lady, charlie, caine, tepung, and snow",eConsumableType.Snort) {
                ModelItem = new PhysicalItem("ba_prop_battle_sniffing_pipe",57005,new Vector3(0.11f, 0.0f, -0.02f),new Rotator(-179f, 72f, -28f))
                ,PackageItem = new PhysicalItem("prop_meth_bag_01")
                ,IntoxicantName = "Cocaine", PercentLostOnUse = 0.5f, MeasurementName = "Gram" },

            new ModItem("Crack","",eConsumableType.AltSmoke) {
                ModelItem = new PhysicalItem("prop_cs_crackpipe",57005,new Vector3(0.14f, 0.07f, 0.02f),new Rotator(-119f, 47f, 0f))
                ,PackageItem = new PhysicalItem("prop_meth_bag_01")
                ,IntoxicantName = "Crack", PercentLostOnUse = 0.5f, MeasurementName = "Gram", RequiredToolType = ToolTypes.Lighter },


            new ModItem("Heroin","Heroin was first made by C. R. Alder Wright in 1874 from morphine, a natural product of the opium poppy",eConsumableType.Inject) {
                ModelItem = new PhysicalItem("prop_syringe_01",57005,new Vector3(0.16f, 0.02f, -0.07f),new Rotator(-170f, -148f, -36f))
                ,PackageItem = new PhysicalItem("prop_meth_bag_01")
                ,IntoxicantName = "Heroin", PercentLostOnUse = 0.5f, MeasurementName = "Gram" },


            new ModItem("Methamphetamine","Also referred to as Speed, Sabu, Crystal and Meth",eConsumableType.AltSmoke) {
                ModelItem = new PhysicalItem("prop_cs_meth_pipe",57005,new Vector3(0.14f, 0.05f, -0.01f),new Rotator(-119f, 0f, 0f))
                ,PackageItem = new PhysicalItem("prop_meth_bag_01")
                ,IntoxicantName = "Methamphetamine", PercentLostOnUse = 0.25f, MeasurementName = "Gram", RequiredToolType = ToolTypes.Lighter },



            new ModItem("Bull Shark Testosterone","More bite than bush elephant testosterone. Become more aggressive, hornier, and irresistible to women! The ultimate man!",eConsumableType.Ingest) {
                ModelItem = new PhysicalItem("prop_cs_pills",57005,new Vector3(0.12f, 0.03f, 0.0f),new Rotator(-76f, 0f, 0f)),IntoxicantName = "Bull Shark Testosterone" , AmountPerPackage = 10},  
            new ModItem("Alco Patch","The Alco Patch. It's the same refreshing feeling of your favorite drink, but delivered transdermally and discreetly. Pick up the Alco Patch at your local pharmacy.",eConsumableType.Ingest) {
                ModelItem = new PhysicalItem("prop_cs_pills",57005,new Vector3(0.12f, 0.03f, 0.0f),new Rotator(-76f, 0f, 0f)),IntoxicantName = "Alco Patch",AmountPerPackage = 10,
                //PackageItem = new PhysicalItem("v_res_tt_pharm2",57005,new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0f, 0f, 0f)), 
            },
            new ModItem("Lax to the Max","Lubricated suppositories. Get flowing again!",eConsumableType.Ingest) {
                ModelItem = new PhysicalItem("prop_cs_pills",57005,new Vector3(0.12f, 0.03f, 0.0f),new Rotator(-76f, 0f, 0f)),IntoxicantName = "Alco Patch",AmountPerPackage = 10,
                //PackageItem = new PhysicalItem("v_res_tt_pharm3",57005,new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0f, 0f, 0f)), 
            },
            new ModItem("Mollis","For outstanding erections. Get the performance you've always dreamed of",eConsumableType.Ingest) {
                ModelItem = new PhysicalItem("prop_cs_pills",57005,new Vector3(0.12f, 0.03f, 0.0f),new Rotator(-76f, 0f, 0f)),IntoxicantName = "Mollis",AmountPerPackage = 10,
                //PackageItem = new PhysicalItem("v_res_tt_pharm1",57005,new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0f, 0f, 0f)),  
            },
            new ModItem("Chesty","Cough suppressant manufactured by Good Aids Pharmacy. Gives 24-hour relief and is available in honey flavour.",eConsumableType.Ingest) {
                ModelItem = new PhysicalItem("prop_cs_pills",57005,new Vector3(0.12f, 0.03f, 0.0f),new Rotator(-76f, 0f, 0f)),IntoxicantName = "Chesty", AmountPerPackage = 10 },
            new ModItem("Equanox","Combats dissatisfaction, lethargy, depression, melancholy, sexual dysfunction. Equanox may cause nausea, loss of sleep, blurred vision, leakage, kidney problems and breathing irregularities.",eConsumableType.Ingest) {
                ModelItem = new PhysicalItem("prop_cs_pills",57005,new Vector3(0.12f, 0.03f, 0.0f),new Rotator(-76f, 0f, 0f)),IntoxicantName = "Equanox", AmountPerPackage = 10 },
            new ModItem("Zombix","Painkiller and antidepressant manufactured by O'Deas Pharmacy. ~n~'Go straight for the head.'",eConsumableType.Ingest) {
                ModelItem = new PhysicalItem("prop_cs_pills",57005,new Vector3(0.12f, 0.03f, 0.0f),new Rotator(-76f, 0f, 0f)),IntoxicantName = "Zombix", AmountPerPackage = 10 },

        //WEAPONS
        //Melee
        new ModItem("Baseball Bat","Aluminum baseball bat with leather grip. Lightweight yet powerful for all you big hitters out there.", false) { ModelItem = new PhysicalItem("weapon_bat",0x958A4A8F) { Type = ePhysicalItemType.Weapon }},
        new ModItem("Crowbar","Heavy-duty crowbar forged from high quality, tempered steel for that extra leverage you need to get the job done.", false) { ModelItem = new PhysicalItem("weapon_crowbar",0x84BD7BFD) { Type = ePhysicalItemType.Weapon }},
        new ModItem("Golf Club","Standard length, mid iron golf club with rubber grip for a lethal short game.", false) { ModelItem = new PhysicalItem("weapon_golfclub",0x440E4788) { Type = ePhysicalItemType.Weapon }},
        new ModItem("Hammer","A robust, multi-purpose hammer with wooden handle and curved claw, this old classic still nails the competition.", false) { ModelItem = new PhysicalItem("weapon_hammer",0x4E875F73) { Type = ePhysicalItemType.Weapon }},
        new ModItem("Hatchet","Add a good old-fashioned hatchet to your armory, and always have a back up for when ammo is hard to come by.", false) { ModelItem = new PhysicalItem("weapon_hatchet",0xF9DCBF2D) { Type = ePhysicalItemType.Weapon }},
        new ModItem("Brass Knuckles","Perfect for knocking out gold teeth, or as a gift to the trophy partner who has everything.", false) { ModelItem = new PhysicalItem("weapon_knuckle",0xD8DF3C3C) { Type = ePhysicalItemType.Weapon }},
        new ModItem("Combat Knife","This carbon steel 7 inch bladed knife is dual edged with a serrated spine to provide improved stabbing and thrusting capabilities.", false) { ModelItem = new PhysicalItem("weapon_knife",0x99B507EA) { Type = ePhysicalItemType.Weapon }},
        new ModItem("Machete","America's West African arms trade isn't just about giving. Rediscover the simple life with this rusty cleaver.", false) { ModelItem = new PhysicalItem("weapon_machete",0xDD5DF8D9) { Type = ePhysicalItemType.Weapon }},
        new ModItem("Switchblade","From your pocket to hilt-deep in the other guy's ribs in under a second: folding knives will never go out of style.", false) { ModelItem = new PhysicalItem("weapon_switchblade",0xDFE37640) { Type = ePhysicalItemType.Weapon }},
        new ModItem("Nightstick","24 inch polycarbonate side-handled nightstick.", false) { ModelItem = new PhysicalItem("weapon_nightstick",0x678B81B1) { Type = ePhysicalItemType.Weapon }},
        new ModItem("Wrench","Perennial favourite of apocalyptic survivalists and violent fathers the world over, apparently it also doubles as some kind of tool.", false) { ModelItem = new PhysicalItem("weapon_wrench",0x19044EE0) { Type = ePhysicalItemType.Weapon }},
        new ModItem("Pool Cue","Ah, there's no sound as satisfying as the crack of a perfect break, especially when it's the other guy's spine.", false) { ModelItem = new PhysicalItem("weapon_poolcue",0x94117305) { Type = ePhysicalItemType.Weapon }},

        //Pistola
        new ModItem("Hawk & Little PTF092F","Standard handgun. A 9mm combat pistol with a magazine capacity of 12 rounds that can be extended to 16.", false) {  ModelItem = new PhysicalItem("weapon_pistol",0x1B06D571) { Type = ePhysicalItemType.Weapon }},
        new ModItem("Hawk & Little Thunder","Balance, simplicity, precision: nothing keeps the peace like an extended barrel in the other guy's mouth.", true) { ModelItem = new PhysicalItem("weapon_pistol_mk2",0xBFE256D4) { Type = ePhysicalItemType.Weapon }},
        new ModItem("Hawk & Little Combat Pistol","A compact, lightweight semi-automatic pistol designed for law enforcement and personal defense use. 12-round magazine with option to extend to 16 rounds.", false) { ModelItem = new PhysicalItem("weapon_combatpistol",0x5EF9FEC4) { Type = ePhysicalItemType.Weapon }},
        new ModItem("Hawk & Little Desert Slug","High-impact pistol that delivers immense power but with extremely strong recoil. Holds 9 rounds in magazine.", false) { ModelItem = new PhysicalItem("weapon_pistol50",0x99AEEB3B) { Type = ePhysicalItemType.Weapon }},
        new ModItem("Vom Feuer P69","Not your grandma's ceramics. Although this pint-sized pistol is small enough to fit into her purse and won't set off a metal detector.", true) { ModelItem = new PhysicalItem("weapon_ceramicpistol",0x2B5EF5EC) { Type = ePhysicalItemType.Weapon }},
        new ModItem("Vom Feuer SCRAMP","High-penetration, fully-automatic pistol. Holds 18 rounds in magazine with option to extend to 36 rounds.", false) { ModelItem = new PhysicalItem("weapon_appistol",0x22D8FE39) { Type = ePhysicalItemType.Weapon }},
        new ModItem("Hawk & Little 1919","The heavyweight champion of the magazine fed, semi-automatic handgun world. Delivers accuracy and a serious forearm workout every time.", false) { ModelItem = new PhysicalItem("weapon_heavypistol",0xD205520E) { Type = ePhysicalItemType.Weapon }},
        new ModItem("Hawk & Little Raging Mare","A handgun with enough stopping power to drop a crazed rhino, and heavy enough to beat it to death if you're out of ammo.", true) { ModelItem = new PhysicalItem("weapon_revolver",0xC1B3C3D1) { Type = ePhysicalItemType.Weapon }},
        new ModItem("Hawk & Little Raging Mare Dx","If you can lift it, this is the closest you'll get to shooting someone with a freight train.", true) { ModelItem = new PhysicalItem("weapon_revolver_mk2",0xCB96392F) { Type = ePhysicalItemType.Weapon }},
        new ModItem("Shrewsbury S7","Like condoms or hairspray, this fits in your pocket for a night on the town. The price of a bottle at a club, it's half as accurate as a champagne cork, and twice as deadly.", false) { ModelItem = new PhysicalItem("weapon_snspistol",0xBFD21232) { Type = ePhysicalItemType.Weapon }},
        new ModItem("Shrewsbury S7A","The ultimate purse-filler: if you want to make Saturday Night really special, this is your ticket.", true) { ModelItem = new PhysicalItem("weapon_snspistol_mk2",0x88374054) { Type = ePhysicalItemType.Weapon }},
        new ModItem("Coil Tesla","Fires a projectile that administers a voltage capable of temporarily stunning an assailant. It's like, literally stunning.", false) { ModelItem = new PhysicalItem("weapon_stungun",0x45CD9CF3) { Type = ePhysicalItemType.Weapon }},
        new ModItem("BS M1922","What you really need is a more recognisable gun. Stand out from the crowd at an armed robbery with this engraved pistol.", true) { ModelItem = new PhysicalItem("weapon_vintagepistol",0x83839C4) { Type = ePhysicalItemType.Weapon }},

        //Shotgun
        new ModItem("Shrewsbury 420 Sawed-Off","This single-barrel, sawed-off shotgun compensates for its low range and ammo capacity with devastating efficiency in close combat.", false) { ModelItem = new PhysicalItem("weapon_sawnoffshotgun",0x7846A318) { Type = ePhysicalItemType.Weapon }},
        new ModItem("Shrewsbury 420","Standard shotgun ideal for short-range combat. A high-projectile spread makes up for its lower accuracy at long range.", false) { ModelItem = new PhysicalItem("weapon_pumpshotgun",0x1D073A89) { Type = ePhysicalItemType.Weapon }},
        new ModItem("Vom Feuer 569","Only one thing pumps more action than a pump action: watch out, the recoil is almost as deadly as the shot.", true) { ModelItem = new PhysicalItem("weapon_pumpshotgun_mk2",0x555AF99A) { Type = ePhysicalItemType.Weapon }},
        new ModItem("Vom Feuer IBS-12","Fully automatic shotgun with 8 round magazine and high rate of fire.", false) { ModelItem = new PhysicalItem("weapon_assaultshotgun",0xE284C527) { Type = ePhysicalItemType.Weapon }},
        new ModItem("Hawk & Little HLSG","More than makes up for its slow, pump-action rate of fire with its range and spread. Decimates anything in its projectile path.", false) { ModelItem = new PhysicalItem("weapon_bullpupshotgun",0x9D61E50F) { Type = ePhysicalItemType.Weapon }},
        new ModItem("Shrewsbury Taiga-12","The weapon to reach for when you absolutely need to make a horrible mess of the room. Best used near easy-wipe surfaces only.", true) { ModelItem = new PhysicalItem("weapon_heavyshotgun",0x3AABBBAA) { Type = ePhysicalItemType.Weapon }},
        new ModItem("Toto 12 Guage Sawed-Off","Do one thing, do it well. Who needs a high rate of fire when your first shot turns the other guy into a fine mist?.", true) { ModelItem = new PhysicalItem("weapon_dbshotgun",0xEF951FBB) { Type = ePhysicalItemType.Weapon }},
        new ModItem("Shrewsbury Defender","How many effective tools for riot control can you tuck into your pants? Ok, two. But this is the other one.", true) { ModelItem = new PhysicalItem("weapon_autoshotgun",0x12E82D3D) { Type = ePhysicalItemType.Weapon }},
        new ModItem("Leotardo SPAZ-11","There's only one semi-automatic shotgun with a fire rate that sets the LSFD alarm bells ringing, and you're looking at it.", true) { ModelItem = new PhysicalItem("weapon_combatshotgun",0x5A96BA4) { Type = ePhysicalItemType.Weapon }},
    
        //SMG
        new ModItem("Shrewsbury Luzi","Combines compact design with a high rate of fire at approximately 700-900 rounds per minute.", false) { ModelItem = new PhysicalItem("weapon_microsmg",0x13532244) { Type = ePhysicalItemType.Weapon }},
        new ModItem("Hawk & Little MP6","This is known as a good all-around submachine gun. Lightweight with an accurate sight and 30-round magazine capacity.", false) { ModelItem = new PhysicalItem("weapon_smg",0x2BE6766B) { Type = ePhysicalItemType.Weapon }},
        new ModItem("Hawk & Little XPM","Lightweight, compact, with a rate of fire to die very messily for: turn any confined space into a kill box at the click of a well-oiled trigger.", true) { ModelItem = new PhysicalItem("weapon_smg_mk2",0x78A97CD0) { Type = ePhysicalItemType.Weapon }},
        new ModItem("Vom Feuer Fisher","A high-capacity submachine gun that is both compact and lightweight. Holds up to 30 bullets in one magazine.", false) { ModelItem = new PhysicalItem("weapon_assaultsmg",0xEFE7E2DF) { Type = ePhysicalItemType.Weapon }},
        new ModItem("Coil PXM","Who said personal weaponry couldn't be worthy of military personnel? Thanks to our lobbyists, not Congress. Integral suppressor.", false) { ModelItem = new PhysicalItem("weapon_combatpdw",0x0A3D4D34) { Type = ePhysicalItemType.Weapon }},
        new ModItem("Vom Feuer KEK-9","This fully automatic is the snare drum to your twin-engine V8 bass: no drive-by sounds quite right without it.", false) { ModelItem = new PhysicalItem("weapon_machinepistol",0xDB1AA450) { Type = ePhysicalItemType.Weapon }},
        new ModItem("Hawk & Little Millipede","Increasingly popular since the marketing team looked beyond spec ops units and started caring about the little guys in low income areas.", false) { ModelItem = new PhysicalItem("weapon_minismg",0xBD248B55) { Type = ePhysicalItemType.Weapon }},

        //AR
        new ModItem("Shrewsbury A7-4K","This standard assault rifle boasts a large capacity magazine and long distance accuracy.", false) { ModelItem = new PhysicalItem("weapon_assaultrifle",0xBFEFFF6D) { Type = ePhysicalItemType.Weapon }},
        new ModItem("Shrewsbury A2-1K","The definitive revision of an all-time classic: all it takes is a little work, and looks can kill after all.", true) { ModelItem = new PhysicalItem("weapon_assaultrifle_mk2",0x394F415C) { Type = ePhysicalItemType.Weapon }},
        new ModItem("Vom Feuer A5-1R","Combining long distance accuracy with a high capacity magazine, the Carbine Rifle can be relied on to make the hit.", false) { ModelItem = new PhysicalItem("weapon_carbinerifle",0x83BF0278) { Type = ePhysicalItemType.Weapon }},
        new ModItem("Vom Feuer A5-1R MK2","This is bespoke, artisan firepower: you couldn't deliver a hail of bullets with more love and care if you inserted them by hand.", true) { ModelItem = new PhysicalItem("weapon_carbinerifle_mk2",0xFAD1F1C9) { Type = ePhysicalItemType.Weapon }},
        new ModItem("Vom Feuer BFR","The most lightweight and compact of all assault rifles, without compromising accuracy and rate of fire.", false) { ModelItem = new PhysicalItem("weapon_advancedrifle",0xAF113F99) { Type = ePhysicalItemType.Weapon }},
        new ModItem("Vom Feuer SL6","Combining accuracy, maneuverability, firepower and low recoil, this is an extremely versatile assault rifle for any combat situation.", false) { ModelItem = new PhysicalItem("weapon_specialcarbine",0xC0A3098D) { Type = ePhysicalItemType.Weapon }},
        new ModItem("Vom Feuer SL6 MK2","The jack of all trades just got a serious upgrade: bow to the master.", true) { ModelItem = new PhysicalItem("weapon_specialcarbine_mk2",0x969C3D67) { Type = ePhysicalItemType.Weapon }},
        new ModItem("Hawk & Little ZBZ-23","The latest Chinese import taking America by storm, this rifle is known for its balanced handling. Lightweight and very controllable in automatic fire.", false) { ModelItem = new PhysicalItem("weapon_bullpuprifle",0x7F229F94) { Type = ePhysicalItemType.Weapon }},
        new ModItem("Hawk & Little ZBZ-25X","So precise, so exquisite, it's not so much a hail of bullets as a symphony.", true) { ModelItem = new PhysicalItem("weapon_bullpuprifle_mk2",0x84D6FAFD) { Type = ePhysicalItemType.Weapon }},
        new ModItem("Shrewsbury Stinkov","Half the size, all the power, double the recoil: there's no riskier way to say 'I'm compensating for something'.", false) { ModelItem = new PhysicalItem("weapon_compactrifle",0x624FE830) { Type = ePhysicalItemType.Weapon }},
        new ModItem("Vom Feuer GUH-B4","This immensely powerful assault rifle was designed for highly qualified, exceptionally skilled soldiers. Yes, you can buy it.", false) { ModelItem = new PhysicalItem("weapon_militaryrifle",0x9D1F17E6) { Type = ePhysicalItemType.Weapon }},
        new ModItem("Vom Feuer POCK","The no-holds barred 30-round answer to that eternal question: how do I get this guy off my back?", true) { ModelItem = new PhysicalItem("weapon_heavyrifle",0xC78D71B4) { Type = ePhysicalItemType.Weapon }},

        //LMG
        new ModItem("Shrewsbury PDA","General purpose machine gun that combines rugged design with dependable performance. Long range penetrative power. Very effective against large groups.", false) { ModelItem = new PhysicalItem("weapon_mg",0x9D07F764) { Type = ePhysicalItemType.Weapon }},
        new ModItem("Vom Feuer BAT","Lightweight, compact machine gun that combines excellent maneuverability with a high rate of fire to devastating effect.", false) { ModelItem = new PhysicalItem("weapon_combatmg",0x7FD62962) { Type = ePhysicalItemType.Weapon }},
        new ModItem("Vom Feuer M70E1","You can never have too much of a good thing: after all, if the first shot counts, then the next hundred or so must count for double.", true) { ModelItem = new PhysicalItem("weapon_combatmg_mk2",0xDBBD7280) { Type = ePhysicalItemType.Weapon }},
        new ModItem("Hawk & Little Kenan","Complete your look with a Prohibition gun. Looks great being fired from an Albany Roosevelt or paired with a pinstripe suit.", false) { ModelItem = new PhysicalItem("weapon_gusenberg",0x61012683) { Type = ePhysicalItemType.Weapon }},

        //SNIPER
        new ModItem("Shrewsbury PWN","Standard sniper rifle. Ideal for situations that require accuracy at long range. Limitations include slow reload speed and very low rate of fire.", false) { ModelItem = new PhysicalItem("weapon_sniperrifle",0x05FC3C11) { Type = ePhysicalItemType.Weapon }},
        new ModItem("Bartlett M92","Features armor-piercing rounds for heavy damage. Comes with laser scope as standard.", false) { ModelItem = new PhysicalItem("weapon_heavysniper",0x0C472FE2) { Type = ePhysicalItemType.Weapon }},
        new ModItem("Bartlett M92 Mk2","Far away, yet always intimate: if you're looking for a secure foundation for that long-distance relationship, this is it.", true) { ModelItem = new PhysicalItem("weapon_heavysniper_mk2",0xA914799) { Type = ePhysicalItemType.Weapon }},
        new ModItem("Vom Feuer M23 DBS","Whether you're up close or a disconcertingly long way away, this weapon will get the job done. A multi-range tool for tools.", false) { ModelItem = new PhysicalItem("weapon_marksmanrifle",0xC734385A) { Type = ePhysicalItemType.Weapon }},
        new ModItem("Vom Feuer M23 DBS Scout","Known in military circles as The Dislocator, this mod set will destroy both the target and your shoulder, in that order.", true) { ModelItem = new PhysicalItem("weapon_marksmanrifle_mk2",0x6A6C02E0) { Type = ePhysicalItemType.Weapon }},

        //OTHER
        new ModItem("RPG-7","A portable, shoulder-launched, anti-tank weapon that fires explosive warheads. Very effective for taking down vehicles or large groups of assailants.", false) { ModelItem = new PhysicalItem("weapon_rpg",0xB1CA77B1) { Type = ePhysicalItemType.Weapon }},
        new ModItem("Hawk & Little MGL","A compact, lightweight grenade launcher with semi-automatic functionality. Holds up to 10 rounds.", false) { ModelItem = new PhysicalItem("weapon_grenadelauncher",0xA284510B) { Type = ePhysicalItemType.Weapon }},
        new ModItem("M61 Grenade","Standard fragmentation grenade. Pull pin, throw, then find cover. Ideal for eliminating clustered assailants.", false) { ModelItem = new PhysicalItem("weapon_grenade",0x93E220BD) { Type = ePhysicalItemType.Weapon }},
        new ModItem("Improvised Incendiary","Crude yet highly effective incendiary weapon. No happy hour with this cocktail.", false) { ModelItem = new PhysicalItem("weapon_molotov",0x24B17070) { Type = ePhysicalItemType.Weapon }},
        new ModItem("BZ Gas Grenade","BZ gas grenade, particularly effective at incapacitating multiple assailants.", false) { ModelItem = new PhysicalItem("weapon_bzgas",0xA0973D5E) { Type = ePhysicalItemType.Weapon }},
        new ModItem("Tear Gas Grenade","Tear gas grenade, particularly effective at incapacitating multiple assailants. Sustained exposure can be lethal.", false) { ModelItem = new PhysicalItem("weapon_smokegrenade",0xBFE256D4) { Type = ePhysicalItemType.Weapon }},

        //Cars & Motorcycles
        new ModItem("Albany Alpha", true) { ModelItem = new PhysicalItem("alpha") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Albany Roosevelt", true) { ModelItem = new PhysicalItem("btype") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Albany Fränken Stange", true) { ModelItem = new PhysicalItem("btype2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Albany Roosevelt Valor", true) { ModelItem = new PhysicalItem("btype3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Albany Buccaneer") { ModelItem = new PhysicalItem("buccaneer") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Albany Buccaneer Custom", true) { ModelItem = new PhysicalItem("buccaneer2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Albany Cavalcade") { ModelItem = new PhysicalItem("cavalcade") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Albany Cavalcade 2") { ModelItem = new PhysicalItem("cavalcade2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Albany Emperor") { ModelItem = new PhysicalItem("emperor") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Albany Emperor 2") { ModelItem = new PhysicalItem("Emperor2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Albany Emperor 3") { ModelItem = new PhysicalItem("emperor3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Albany Hermes", true) { ModelItem = new PhysicalItem("hermes") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Albany Lurcher", true) { ModelItem = new PhysicalItem("lurcher") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Albany Manana") { ModelItem = new PhysicalItem("manana") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Albany Manana Custom", true) { ModelItem = new PhysicalItem("manana2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Albany Primo") { ModelItem = new PhysicalItem("primo") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Albany Primo Custom", true) { ModelItem = new PhysicalItem("primo2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Albany Virgo", true) { ModelItem = new PhysicalItem("virgo") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Albany V-STR", true) { ModelItem = new PhysicalItem("vstr") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Albany Washington") { ModelItem = new PhysicalItem("washington") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Annis Elegy Retro Custom", true) { ModelItem = new PhysicalItem("elegy") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Annis Elegy RH8") { ModelItem = new PhysicalItem("elegy2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Annis Euros", true) { ModelItem = new PhysicalItem("Euros") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Annis Hellion", true) { ModelItem = new PhysicalItem("hellion") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Annis RE-7B", true) { ModelItem = new PhysicalItem("le7b") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Annis Remus", true) { ModelItem = new PhysicalItem("remus") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Annis S80RR", true) { ModelItem = new PhysicalItem("s80") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Annis Savestra", true) { ModelItem = new PhysicalItem("savestra") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Annis ZR350", true) { ModelItem = new PhysicalItem("zr350") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Annis Apocalypse ZR380", true) { ModelItem = new PhysicalItem("zr380") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Annis Future Shock ZR380", true) { ModelItem = new PhysicalItem("zr3802") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Annis Nightmare ZR380", true) { ModelItem = new PhysicalItem("zr3803") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Benefactor Apocalypse Bruiser", true) { ModelItem = new PhysicalItem("bruiser") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Benefactor Future Shock Bruiser", true) { ModelItem = new PhysicalItem("bruiser2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Benefactor Nightmare Bruiser", true) { ModelItem = new PhysicalItem("bruiser3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Benefactor Dubsta") { ModelItem = new PhysicalItem("dubsta") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Benefactor Dubsta 2") { ModelItem = new PhysicalItem("dubsta2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Benefactor Dubsta 6x6", true) { ModelItem = new PhysicalItem("dubsta3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Benefactor Feltzer") { ModelItem = new PhysicalItem("feltzer2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Benefactor Stirling GT", true) { ModelItem = new PhysicalItem("feltzer3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Benefactor Glendale", true) { ModelItem = new PhysicalItem("glendale") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Benefactor Glendale Custom", true) { ModelItem = new PhysicalItem("glendale2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Benefactor Turreted Limo", true) { ModelItem = new PhysicalItem("limo2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Benefactor BR8", true) { ModelItem = new PhysicalItem("openwheel1") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Benefactor Panto", true) { ModelItem = new PhysicalItem("panto") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Benefactor Schafter","Good-looking yet utilitarian, sexy yet asexual, slender yet terrifyingly powerful, the Schafter is German engineering at its very finest.") { ModelItem = new PhysicalItem("schafter2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Benefactor Schafter V12","Powerful, understated, reliable. You have absolutely none of these qualities, so it's important to you that your car does. The new Benefactor Schafter screams corporate anonymity just as much as its predecessor, so to justify the massive price hike we've thrown in a few flourishes and a plush interior where you can screw your secretary's secretary in comfort. This model has a V12 engine.", true) { ModelItem = new PhysicalItem("schafter3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Benefactor Schafter LWB","Powerful, understated, reliable. You have absolutely none of these qualities, so it's important to you that your car does. The new Benefactor Schafter screams corporate anonymity just as much as its predecessor, so to justify the massive price hike we've thrown in a few flourishes and a plush interior where you can screw your secretary's secretary in comfort. This model has an extended wheelbase.",true) { ModelItem = new PhysicalItem("schafter4") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Benefactor Schafter V12 (Armored)", true) { ModelItem = new PhysicalItem("schafter5") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Benefactor Schafter LWB (Armored)", true) { ModelItem = new PhysicalItem("schafter6") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Benefactor Schwartzer","Say what you will about the Germans - they know luxury. And their economy is the only one worth a crap in Europe. This model has all kinds of extras - too many to list for legal reasons.") { ModelItem = new PhysicalItem("schwarzer") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Benefactor Serrano","Fun fact: what's the fastest growing market in the American auto industry? That's right! Compact SUVs! And do you know why? That's right! Neither do we! And is that a good enough reason to buy one? That's right! It had better be!") { ModelItem = new PhysicalItem("serrano") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Benefactor Surano","This is luxury reasserted. Right in your neighbour's face. Boom. You like that. That's right, you are better than him, and you could have his wife if you wanted. Try it on with her as soon as she sees this ride. You'll be a double benefactor.") { ModelItem = new PhysicalItem("Surano") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Benefactor XLS", true) { ModelItem = new PhysicalItem("xls") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Benefactor XLS (Armored)", true) { ModelItem = new PhysicalItem("xls2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Benefactor Krieger", true) { ModelItem = new PhysicalItem("krieger") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Benefactor Schlagen GT", true) { ModelItem = new PhysicalItem("schlagen") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Benefactor Streiter", true) { ModelItem = new PhysicalItem("streiter") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Benefactor Terrorbyte", true) { ModelItem = new PhysicalItem("terbyte") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("BF Injection") { ModelItem = new PhysicalItem("BfInjection") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("BF Bifta", true) { ModelItem = new PhysicalItem("bifta") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("BF Club", true) { ModelItem = new PhysicalItem("club") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("BF Dune Buggy") { ModelItem = new PhysicalItem("dune") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("BF Dune FAV", true) { ModelItem = new PhysicalItem("dune3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("BF Raptor", true) { ModelItem = new PhysicalItem("raptor") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("BF Surfer") { ModelItem = new PhysicalItem("SURFER") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("BF Surfer") { ModelItem = new PhysicalItem("Surfer2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("BF Weevil", true) { ModelItem = new PhysicalItem("weevil") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Bollokan Prairie") { ModelItem = new PhysicalItem("prairie") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Bravado Banshee") { ModelItem = new PhysicalItem("banshee") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Bravado Banshee 900R", true) { ModelItem = new PhysicalItem("banshee2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Bravado Bison") { ModelItem = new PhysicalItem("bison") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Bravado Bison 2") { ModelItem = new PhysicalItem("Bison2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Bravado Bison 3") { ModelItem = new PhysicalItem("Bison3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Bravado Buffalo") { ModelItem = new PhysicalItem("buffalo") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Bravado Buffalo S") { ModelItem = new PhysicalItem("buffalo2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Bravado Sprunk Buffalo") { ModelItem = new PhysicalItem("buffalo3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Bravado Duneloader") { ModelItem = new PhysicalItem("dloader") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Bravado Gauntlet") { ModelItem = new PhysicalItem("Gauntlet") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Bravado Redwood Gauntlet") { ModelItem = new PhysicalItem("gauntlet2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Bravado Gauntlet Classic", true) { ModelItem = new PhysicalItem("gauntlet3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Bravado Gauntlet Hellfire", true) { ModelItem = new PhysicalItem("gauntlet4") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Bravado Gauntlet Classic Custom", true) { ModelItem = new PhysicalItem("gauntlet5") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Bravado Gresley") { ModelItem = new PhysicalItem("gresley") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Bravado Half-track", true) { ModelItem = new PhysicalItem("halftrack") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Bravado Apocalypse Sasquatch", true) { ModelItem = new PhysicalItem("monster3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Bravado Future Shock Sasquatch", true) { ModelItem = new PhysicalItem("monster4") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Bravado Nightmare Sasquatch", true) { ModelItem = new PhysicalItem("monster5") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Bravado Paradise", true) { ModelItem = new PhysicalItem("paradise") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Bravado Rat-Truck", true) { ModelItem = new PhysicalItem("ratloader2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Bravado Rumpo") { ModelItem = new PhysicalItem("rumpo") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Bravado Rumpo 2") { ModelItem = new PhysicalItem("rumpo2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Bravado Rumpo Custom", true) { ModelItem = new PhysicalItem("rumpo3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Bravado Verlierer", true) { ModelItem = new PhysicalItem("verlierer2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Bravado Youga") { ModelItem = new PhysicalItem("youga") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Bravado Youga Classic", true) { ModelItem = new PhysicalItem("youga2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Bravado Youga Classic 4x4", true) { ModelItem = new PhysicalItem("youga3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Brute Boxville") { ModelItem = new PhysicalItem("boxville") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Brute Boxville 3") { ModelItem = new PhysicalItem("boxville3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Brute Boxville 4", true) { ModelItem = new PhysicalItem("boxville4") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Brute Camper") { ModelItem = new PhysicalItem("CAMPER") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Brute Pony") { ModelItem = new PhysicalItem("pony") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Brute Pony 2") { ModelItem = new PhysicalItem("pony2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Brute Stockade") { ModelItem = new PhysicalItem("stockade") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Brute Stockade 3") { ModelItem = new PhysicalItem("stockade3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Brute Tipper") { ModelItem = new PhysicalItem("TipTruck") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Canis Bodhi") { ModelItem = new PhysicalItem("Bodhi2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Canis Crusader") { ModelItem = new PhysicalItem("CRUSADER") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Canis Freecrawler", true) { ModelItem = new PhysicalItem("freecrawler") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Canis Kalahari", true) { ModelItem = new PhysicalItem("kalahari") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Canis Kamacho", true) { ModelItem = new PhysicalItem("kamacho") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Canis Mesa") { ModelItem = new PhysicalItem("MESA") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Canis Mesa 2") { ModelItem = new PhysicalItem("mesa2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Canis Mesa 3") { ModelItem = new PhysicalItem("MESA3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Canis Seminole") { ModelItem = new PhysicalItem("Seminole") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Canis Seminole Frontier", true) { ModelItem = new PhysicalItem("seminole2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Chariot Romero Hearse") { ModelItem = new PhysicalItem("romero") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Cheval Fugitive") { ModelItem = new PhysicalItem("fugitive") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Cheval Marshall") { ModelItem = new PhysicalItem("marshall") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Cheval Picador") { ModelItem = new PhysicalItem("picador") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Cheval Surge") { ModelItem = new PhysicalItem("surge") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Cheval Taipan", true) { ModelItem = new PhysicalItem("taipan") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Coil Brawler", true) { ModelItem = new PhysicalItem("brawler") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Coil Cyclone", true) { ModelItem = new PhysicalItem("cyclone") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Coil Raiden", true) { ModelItem = new PhysicalItem("raiden") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Coil Voltic") { ModelItem = new PhysicalItem("voltic") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Coil Rocket Voltic", true) { ModelItem = new PhysicalItem("voltic2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Asea") { ModelItem = new PhysicalItem("asea") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Asea") { ModelItem = new PhysicalItem("asea2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Apocalypse Brutus", true) { ModelItem = new PhysicalItem("brutus") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Future Shock Brutus", true) { ModelItem = new PhysicalItem("brutus2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Nightmare Brutus", true) { ModelItem = new PhysicalItem("brutus3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Burrito") { ModelItem = new PhysicalItem("Burrito") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Bugstars Burrito") { ModelItem = new PhysicalItem("burrito2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Burrito 3") { ModelItem = new PhysicalItem("burrito3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Burrito 4") { ModelItem = new PhysicalItem("Burrito4") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Burrito 5") { ModelItem = new PhysicalItem("burrito5") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Gang Burrito") { ModelItem = new PhysicalItem("gburrito") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Gang Burrito 2", true) { ModelItem = new PhysicalItem("gburrito2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Granger") { ModelItem = new PhysicalItem("GRANGER") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Hotring Sabre", true) { ModelItem = new PhysicalItem("hotring") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Impaler", true) { ModelItem = new PhysicalItem("impaler") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Apocalypse Impaler", true) { ModelItem = new PhysicalItem("impaler2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Future Shock Impaler", true) { ModelItem = new PhysicalItem("impaler3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Nightmare Impaler", true) { ModelItem = new PhysicalItem("impaler4") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Lifeguard") { ModelItem = new PhysicalItem("lguard") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Mamba", true) { ModelItem = new PhysicalItem("mamba") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Moonbeam", true) { ModelItem = new PhysicalItem("moonbeam") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Moonbeam Custom", true) { ModelItem = new PhysicalItem("moonbeam2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse DR1", true) { ModelItem = new PhysicalItem("openwheel2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Premier") { ModelItem = new PhysicalItem("premier") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Rancher XL") { ModelItem = new PhysicalItem("RancherXL") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Rancher XL 2") { ModelItem = new PhysicalItem("rancherxl2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Rhapsody", true) { ModelItem = new PhysicalItem("rhapsody") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Sabre Turbo") { ModelItem = new PhysicalItem("sabregt") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Sabre Turbo Custom", true) { ModelItem = new PhysicalItem("sabregt2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Scramjet", true) { ModelItem = new PhysicalItem("scramjet") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Stallion") { ModelItem = new PhysicalItem("stalion") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Burger Shot Stallion") { ModelItem = new PhysicalItem("stalion2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Tampa", true) { ModelItem = new PhysicalItem("tampa") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Drift Tampa", true) { ModelItem = new PhysicalItem("tampa2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Weaponized Tampa", true) { ModelItem = new PhysicalItem("tampa3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Tornado") { ModelItem = new PhysicalItem("tornado") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Tornado 2") { ModelItem = new PhysicalItem("tornado2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Tornado 3") { ModelItem = new PhysicalItem("tornado3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Tornado 4") { ModelItem = new PhysicalItem("tornado4") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Tornado Custom", true) { ModelItem = new PhysicalItem("tornado5") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Tornado Rat Rod", true) { ModelItem = new PhysicalItem("tornado6") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Tulip", true) { ModelItem = new PhysicalItem("tulip") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Vamos", true) { ModelItem = new PhysicalItem("vamos") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Vigero") { ModelItem = new PhysicalItem("vigero") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Voodoo Custom", true) { ModelItem = new PhysicalItem("voodoo") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Voodoo") { ModelItem = new PhysicalItem("voodoo2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Yosemite", true) { ModelItem = new PhysicalItem("yosemite") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Drift Yosemite", true) { ModelItem = new PhysicalItem("yosemite2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Declasse Yosemite Rancher", true) { ModelItem = new PhysicalItem("yosemite3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dewbauchee Exemplar") { ModelItem = new PhysicalItem("exemplar") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dewbauchee JB 700") { ModelItem = new PhysicalItem("jb700") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dewbauchee JB 700W", true) { ModelItem = new PhysicalItem("jb7002") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dewbauchee Massacro", true) { ModelItem = new PhysicalItem("massacro") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dewbauchee Massacro (Racecar)", true) { ModelItem = new PhysicalItem("massacro2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dewbauchee Rapid GT") { ModelItem = new PhysicalItem("RapidGT") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dewbauchee Rapid GT 2") { ModelItem = new PhysicalItem("RapidGT2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dewbauchee Rapid GT Classic", true) { ModelItem = new PhysicalItem("rapidgt3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dewbauchee Seven-70", true) { ModelItem = new PhysicalItem("SEVEN70") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dewbauchee Specter", true) { ModelItem = new PhysicalItem("SPECTER") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dewbauchee Specter Custom", true) { ModelItem = new PhysicalItem("SPECTER2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dewbauchee Vagner", true) { ModelItem = new PhysicalItem("vagner") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dinka Akuma") { ModelItem = new PhysicalItem("akuma") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dinka Blista") { ModelItem = new PhysicalItem("blista") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dinka Blista Compact") { ModelItem = new PhysicalItem("blista2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dinka Go Go Monkey Blista") { ModelItem = new PhysicalItem("blista3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dinka Double-T") { ModelItem = new PhysicalItem("double") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dinka Enduro", true) { ModelItem = new PhysicalItem("enduro") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dinka Jester", true) { ModelItem = new PhysicalItem("jester") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dinka Jester (Racecar)", true) { ModelItem = new PhysicalItem("jester2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dinka Jester Classic", true) { ModelItem = new PhysicalItem("jester3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dinka Jester RR", true) { ModelItem = new PhysicalItem("jester4") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dinka Blista Kanjo", true) { ModelItem = new PhysicalItem("kanjo") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dinka RT3000", true) { ModelItem = new PhysicalItem("rt3000") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dinka Sugoi", true) { ModelItem = new PhysicalItem("Sugoi") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dinka Thrust", true) { ModelItem = new PhysicalItem("thrust") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dinka Verus", true) { ModelItem = new PhysicalItem("verus") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dinka Veto Classic", true) { ModelItem = new PhysicalItem("veto") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dinka Veto Modern", true) { ModelItem = new PhysicalItem("veto2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dinka Vindicator", true) { ModelItem = new PhysicalItem("vindicator") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dundreary Landstalker") { ModelItem = new PhysicalItem("landstalker") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dundreary Landstalker XL", true) { ModelItem = new PhysicalItem("landstalker2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dundreary Regina") { ModelItem = new PhysicalItem("regina") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dundreary Stretch") { ModelItem = new PhysicalItem("stretch") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dundreary Virgo Classic Custom", true) { ModelItem = new PhysicalItem("virgo2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Dundreary Virgo Classic", true) { ModelItem = new PhysicalItem("virgo3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Emperor Habanero") { ModelItem = new PhysicalItem("habanero") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Emperor ETR1", true) { ModelItem = new PhysicalItem("sheava") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Emperor Vectre", true) { ModelItem = new PhysicalItem("vectre") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Enus Cognoscenti 55", true) { ModelItem = new PhysicalItem("cog55") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Enus Cognoscenti 55 (Armored)", true) { ModelItem = new PhysicalItem("cog552") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Enus Cognoscenti Cabrio") { ModelItem = new PhysicalItem("cogcabrio") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Enus Cognoscenti", true) { ModelItem = new PhysicalItem("cognoscenti") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Enus Cognoscenti (Armored)", true) { ModelItem = new PhysicalItem("cognoscenti2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Enus Huntley S", true) { ModelItem = new PhysicalItem("huntley") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Enus Paragon R", true) { ModelItem = new PhysicalItem("paragon") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Enus Paragon R (Armored)", true) { ModelItem = new PhysicalItem("paragon2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Enus Stafford", true) { ModelItem = new PhysicalItem("stafford") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Enus Super Diamond") { ModelItem = new PhysicalItem("superd") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Enus Windsor", true) { ModelItem = new PhysicalItem("windsor") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Enus Windsor Drop", true) { ModelItem = new PhysicalItem("windsor2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Fathom FQ 2") { ModelItem = new PhysicalItem("fq2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Gallivanter Baller") { ModelItem = new PhysicalItem("Baller") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Gallivanter Baller 2") { ModelItem = new PhysicalItem("baller2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Gallivanter Baller LE", true) { ModelItem = new PhysicalItem("baller3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Gallivanter Baller LE LWB", true) { ModelItem = new PhysicalItem("baller4") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Gallivanter Baller LE (Armored)", true) { ModelItem = new PhysicalItem("baller5") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Gallivanter Baller LE LWB (Armored)", true) { ModelItem = new PhysicalItem("baller6") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Grotti Bestia GTS", true) { ModelItem = new PhysicalItem("bestiagts") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Grotti Brioso R/A", true) { ModelItem = new PhysicalItem("brioso") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Grotti Brioso 300", true) { ModelItem = new PhysicalItem("brioso2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Grotti Carbonizzare") { ModelItem = new PhysicalItem("carbonizzare") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Grotti Cheetah") { ModelItem = new PhysicalItem("cheetah") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Grotti Cheetah Classic", true) { ModelItem = new PhysicalItem("cheetah2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Grotti Furia", true) { ModelItem = new PhysicalItem("furia") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Grotti GT500", true) { ModelItem = new PhysicalItem("gt500") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Grotti Itali GTO", true) { ModelItem = new PhysicalItem("italigto") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Grotti Itali RSX", true) { ModelItem = new PhysicalItem("italirsx") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Grotti X80 Proto", true) { ModelItem = new PhysicalItem("prototipo") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Grotti Stinger") { ModelItem = new PhysicalItem("stinger") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Grotti Stinger GT") { ModelItem = new PhysicalItem("stingergt") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Grotti Turismo Classic", true) { ModelItem = new PhysicalItem("turismo2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Grotti Turismo R", true) { ModelItem = new PhysicalItem("turismor") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Grotti Visione", true) { ModelItem = new PhysicalItem("visione") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Hijak Khamelion") { ModelItem = new PhysicalItem("khamelion") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Hijak Ruston", true) { ModelItem = new PhysicalItem("ruston") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("HVY Barracks Semi") { ModelItem = new PhysicalItem("BARRACKS2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("HVY Biff") { ModelItem = new PhysicalItem("Biff") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("HVY Dozer") { ModelItem = new PhysicalItem("bulldozer") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("HVY Cutter") { ModelItem = new PhysicalItem("cutter") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("HVY Dump") { ModelItem = new PhysicalItem("dump") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("HVY Forklift") { ModelItem = new PhysicalItem("FORKLIFT") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("HVY Insurgent Pick-Up", true) { ModelItem = new PhysicalItem("insurgent") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("HVY Insurgent", true) { ModelItem = new PhysicalItem("insurgent2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("HVY Insurgent Pick-Up Custom", true) { ModelItem = new PhysicalItem("insurgent3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("HVY Menacer", true) { ModelItem = new PhysicalItem("menacer") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("HVY Mixer") { ModelItem = new PhysicalItem("Mixer") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("HVY Mixer 2") { ModelItem = new PhysicalItem("Mixer2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("HVY Nightshark", true) { ModelItem = new PhysicalItem("nightshark") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("HVY Apocalypse Scarab", true) { ModelItem = new PhysicalItem("scarab") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("HVY Future Shock Scarab", true) { ModelItem = new PhysicalItem("scarab2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("HVY Nightmare Scarab", true) { ModelItem = new PhysicalItem("scarab3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Imponte Deluxo", true) { ModelItem = new PhysicalItem("deluxo") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Imponte Dukes") { ModelItem = new PhysicalItem("dukes") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Imponte Duke O'Death") { ModelItem = new PhysicalItem("dukes2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Imponte Beater Dukes", true) { ModelItem = new PhysicalItem("dukes3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Imponte Nightshade", true) { ModelItem = new PhysicalItem("nightshade") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Imponte Phoenix") { ModelItem = new PhysicalItem("Phoenix") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Imponte Ruiner") { ModelItem = new PhysicalItem("ruiner") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Imponte Ruiner 2000", true) { ModelItem = new PhysicalItem("ruiner2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Imponte Ruiner", true) { ModelItem = new PhysicalItem("ruiner3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Invetero Coquette") { ModelItem = new PhysicalItem("coquette") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Invetero Coquette Classic", true) { ModelItem = new PhysicalItem("coquette2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Invetero Coquette BlackFin", true) { ModelItem = new PhysicalItem("coquette3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Invetero Coquette D10", true) { ModelItem = new PhysicalItem("coquette4") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("JoBuilt Hauler") { ModelItem = new PhysicalItem("Hauler") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("JoBuilt Hauler Custom", true) { ModelItem = new PhysicalItem("Hauler2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("JoBuilt Phantom") { ModelItem = new PhysicalItem("Phantom") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("JoBuilt Phantom Wedge", true) { ModelItem = new PhysicalItem("phantom2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("JoBuilt Phantom Custom", true) { ModelItem = new PhysicalItem("phantom3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("JoBuilt Rubble") { ModelItem = new PhysicalItem("Rubble") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Karin Asterope") { ModelItem = new PhysicalItem("asterope") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Karin BeeJay XL") { ModelItem = new PhysicalItem("BjXL") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Karin Calico GTF", true) { ModelItem = new PhysicalItem("calico") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Karin Dilettante") { ModelItem = new PhysicalItem("dilettante") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Karin Dilettante 2") { ModelItem = new PhysicalItem("dilettante2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Karin Everon", true) { ModelItem = new PhysicalItem("everon") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Karin Futo") { ModelItem = new PhysicalItem("futo") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Karin Futo GTX", true) { ModelItem = new PhysicalItem("futo2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Karin Intruder") { ModelItem = new PhysicalItem("intruder") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Karin Kuruma", true) { ModelItem = new PhysicalItem("kuruma") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Karin Kuruma (armored)", true) { ModelItem = new PhysicalItem("kuruma2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Karin Previon", true) { ModelItem = new PhysicalItem("previon") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Karin Rusty Rebel") { ModelItem = new PhysicalItem("Rebel") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Karin Rebel") { ModelItem = new PhysicalItem("rebel2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Karin Sultan") { ModelItem = new PhysicalItem("sultan") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Karin Sultan Classic", true) { ModelItem = new PhysicalItem("sultan2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Karin Sultan RS Classic", true) { ModelItem = new PhysicalItem("sultan3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Karin Sultan RS", true) { ModelItem = new PhysicalItem("sultanrs") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Karin Technical", true) { ModelItem = new PhysicalItem("technical") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Karin Technical Custom", true) { ModelItem = new PhysicalItem("technical3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Karin 190z", true) { ModelItem = new PhysicalItem("z190") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Lampadati Casco", true) { ModelItem = new PhysicalItem("casco") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Lampadati Felon") { ModelItem = new PhysicalItem("felon") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Lampadati Felon GT") { ModelItem = new PhysicalItem("felon2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Lampadati Furore GT", true) { ModelItem = new PhysicalItem("furoregt") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Lampadati Michelli GT", true) { ModelItem = new PhysicalItem("michelli") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Lampadati Pigalle", true) { ModelItem = new PhysicalItem("pigalle") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Lampadati Tropos Rallye", true) { ModelItem = new PhysicalItem("tropos") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Lampadati Komoda", true) { ModelItem = new PhysicalItem("komoda") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Lampadati Novak", true) { ModelItem = new PhysicalItem("Novak") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Lampadati Tigon", true) { ModelItem = new PhysicalItem("tigon") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Lampadati Viseris", true) { ModelItem = new PhysicalItem("viseris") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("LCC Avarus", true) { ModelItem = new PhysicalItem("avarus") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("LCC Hexer") { ModelItem = new PhysicalItem("hexer") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("LCC Innovation", true) { ModelItem = new PhysicalItem("innovation") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("LCC Sanctus", true) { ModelItem = new PhysicalItem("sanctus") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Maibatsu Manchez", true) { ModelItem = new PhysicalItem("manchez") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Maibatsu Manchez Scout", true) { ModelItem = new PhysicalItem("manchez2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Maibatsu Mule") { ModelItem = new PhysicalItem("Mule") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Maibatsu Mule") { ModelItem = new PhysicalItem("Mule2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Maibatsu Mule", true) { ModelItem = new PhysicalItem("Mule3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Maibatsu Mule Custom", true) { ModelItem = new PhysicalItem("mule4") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Maibatsu Penumbra") { ModelItem = new PhysicalItem("penumbra") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Maibatsu Penumbra FF", true) { ModelItem = new PhysicalItem("penumbra2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Maibatsu Sanchez (livery)") { ModelItem = new PhysicalItem("Sanchez") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Maibatsu Sanchez") { ModelItem = new PhysicalItem("sanchez2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Mammoth Patriot") { ModelItem = new PhysicalItem("patriot") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Mammoth Patriot Stretch", true) { ModelItem = new PhysicalItem("patriot2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Mammoth Squaddie", true) { ModelItem = new PhysicalItem("squaddie") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Maxwell Asbo", true) { ModelItem = new PhysicalItem("asbo") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Maxwell Vagrant", true) { ModelItem = new PhysicalItem("vagrant") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("MTL Brickade", true) { ModelItem = new PhysicalItem("brickade") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("MTL Apocalypse Cerberus", true) { ModelItem = new PhysicalItem("cerberus") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("MTL Future Shock Cerberus", true) { ModelItem = new PhysicalItem("cerberus2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("MTL Nightmare Cerberus", true) { ModelItem = new PhysicalItem("cerberus3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("MTL Fire Truck") { ModelItem = new PhysicalItem("firetruk") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("MTL Flatbed") { ModelItem = new PhysicalItem("FLATBED") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("MTL Packer") { ModelItem = new PhysicalItem("Packer") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("MTL Pounder") { ModelItem = new PhysicalItem("Pounder") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("MTL Pounder Custom", true) { ModelItem = new PhysicalItem("pounder2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("MTL Dune", true) { ModelItem = new PhysicalItem("rallytruck") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("MTL Wastelander", true) { ModelItem = new PhysicalItem("wastelander") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Nagasaki BF400", true) { ModelItem = new PhysicalItem("bf400") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Nagasaki Blazer") { ModelItem = new PhysicalItem("blazer") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Nagasaki Blazer Lifeguard") { ModelItem = new PhysicalItem("blazer2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Nagasaki Hot Rod Blazer") { ModelItem = new PhysicalItem("blazer3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Nagasaki Street Blazer", true) { ModelItem = new PhysicalItem("blazer4") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Nagasaki Carbon RS") { ModelItem = new PhysicalItem("carbonrs") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Nagasaki Chimera", true) { ModelItem = new PhysicalItem("chimera") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Nagasaki Outlaw", true) { ModelItem = new PhysicalItem("outlaw") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Nagasaki Shotaro", true) { ModelItem = new PhysicalItem("shotaro") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Nagasaki Stryder", true) { ModelItem = new PhysicalItem("Stryder") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Obey 8F Drafter", true) { ModelItem = new PhysicalItem("drafter") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Obey 9F") { ModelItem = new PhysicalItem("ninef") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Obey 9F Cabrio") { ModelItem = new PhysicalItem("ninef2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Obey Omnis", true) { ModelItem = new PhysicalItem("omnis") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Obey Rocoto") { ModelItem = new PhysicalItem("rocoto") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Obey Tailgater") { ModelItem = new PhysicalItem("tailgater") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Obey Tailgater S", true) { ModelItem = new PhysicalItem("tailgater2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Ocelot Ardent", true) { ModelItem = new PhysicalItem("ardent") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Ocelot F620") { ModelItem = new PhysicalItem("f620") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Ocelot R88", true) { ModelItem = new PhysicalItem("formula2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Ocelot Jackal") { ModelItem = new PhysicalItem("jackal") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Ocelot Jugular", true) { ModelItem = new PhysicalItem("jugular") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Ocelot Locust", true) { ModelItem = new PhysicalItem("locust") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Ocelot Lynx", true) { ModelItem = new PhysicalItem("lynx") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Ocelot Pariah", true) { ModelItem = new PhysicalItem("pariah") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Ocelot Penetrator", true) { ModelItem = new PhysicalItem("penetrator") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Ocelot Swinger", true) { ModelItem = new PhysicalItem("swinger") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Ocelot XA-21", true) { ModelItem = new PhysicalItem("xa21") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Overflod Autarch", true) { ModelItem = new PhysicalItem("autarch") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Overflod Entity XXR", true) { ModelItem = new PhysicalItem("entity2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Overflod Entity XF") { ModelItem = new PhysicalItem("entityxf") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Overflod Imorgon", true) { ModelItem = new PhysicalItem("imorgon") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Overflod Tyrant", true) { ModelItem = new PhysicalItem("tyrant") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pegassi Bati 801") { ModelItem = new PhysicalItem("bati") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pegassi Bati 801RR") { ModelItem = new PhysicalItem("bati2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pegassi Esskey", true) { ModelItem = new PhysicalItem("esskey") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pegassi Faggio Sport", true) { ModelItem = new PhysicalItem("faggio") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pegassi Faggio") { ModelItem = new PhysicalItem("faggio2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pegassi Faggio Mod", true) { ModelItem = new PhysicalItem("faggio3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pegassi FCR 1000", true) { ModelItem = new PhysicalItem("fcr") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pegassi FCR 1000 Custom", true) { ModelItem = new PhysicalItem("fcr2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pegassi Infernus") { ModelItem = new PhysicalItem("infernus") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pegassi Infernus Classic", true) { ModelItem = new PhysicalItem("infernus2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pegassi Monroe") { ModelItem = new PhysicalItem("monroe") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pegassi Oppressor", true) { ModelItem = new PhysicalItem("oppressor") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pegassi Oppressor Mk II", true) { ModelItem = new PhysicalItem("oppressor2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pegassi Osiris", true) { ModelItem = new PhysicalItem("osiris") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pegassi Reaper", true) { ModelItem = new PhysicalItem("reaper") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pegassi Ruffian") { ModelItem = new PhysicalItem("ruffian") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pegassi Tempesta", true) { ModelItem = new PhysicalItem("tempesta") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pegassi Tezeract", true) { ModelItem = new PhysicalItem("tezeract") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pegassi Torero", true) { ModelItem = new PhysicalItem("torero") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pegassi Toros", true) { ModelItem = new PhysicalItem("toros") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pegassi Vacca") { ModelItem = new PhysicalItem("vacca") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pegassi Vortex", true) { ModelItem = new PhysicalItem("vortex") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pegassi Zentorno", true) { ModelItem = new PhysicalItem("zentorno") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pegassi Zorrusso", true) { ModelItem = new PhysicalItem("zorrusso") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pfister Comet") { ModelItem = new PhysicalItem("comet2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pfister Comet Retro Custom", true) { ModelItem = new PhysicalItem("comet3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pfister Comet Safari", true) { ModelItem = new PhysicalItem("comet4") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pfister Comet SR", true) { ModelItem = new PhysicalItem("comet5") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pfister Comet S2", true) { ModelItem = new PhysicalItem("comet6") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pfister Growler", true) { ModelItem = new PhysicalItem("growler") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pfister Neon", true) { ModelItem = new PhysicalItem("neon") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pfister 811", true) { ModelItem = new PhysicalItem("pfister811") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Principe Deveste Eight", true) { ModelItem = new PhysicalItem("deveste") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Principe Diabolus", true) { ModelItem = new PhysicalItem("diablous") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Principe Diabolus Custom", true) { ModelItem = new PhysicalItem("diablous2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Principe Lectro", true) { ModelItem = new PhysicalItem("lectro") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Principe Nemesis") { ModelItem = new PhysicalItem("nemesis") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Progen Emerus", true) { ModelItem = new PhysicalItem("emerus") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Progen PR4", true) { ModelItem = new PhysicalItem("formula") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Progen GP1", true) { ModelItem = new PhysicalItem("gp1") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Progen Itali GTB", true) { ModelItem = new PhysicalItem("italigtb") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Progen Itali GTB Custom", true) { ModelItem = new PhysicalItem("italigtb2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Progen T20", true) { ModelItem = new PhysicalItem("t20") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Progen Tyrus", true) { ModelItem = new PhysicalItem("tyrus") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("RUNE Cheburek", true) { ModelItem = new PhysicalItem("cheburek") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Schyster Deviant", true) { ModelItem = new PhysicalItem("deviant") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Schyster Fusilade") { ModelItem = new PhysicalItem("fusilade") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Shitzu Defiler", true) { ModelItem = new PhysicalItem("defiler") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Shitzu Hakuchou", true) { ModelItem = new PhysicalItem("hakuchou") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Shitzu Hakuchou Drag", true) { ModelItem = new PhysicalItem("hakuchou2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Shitzu PCJ 600") { ModelItem = new PhysicalItem("pcj") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Shitzu Vader") { ModelItem = new PhysicalItem("Vader") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Stanley Fieldmaster") { ModelItem = new PhysicalItem("tractor2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Stanley Fieldmaster") { ModelItem = new PhysicalItem("tractor3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Truffade Adder") { ModelItem = new PhysicalItem("adder") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Truffade Nero", true) { ModelItem = new PhysicalItem("nero") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Truffade Nero Custom", true) { ModelItem = new PhysicalItem("nero2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Truffade Thrax", true) { ModelItem = new PhysicalItem("thrax") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Truffade Z-Type") { ModelItem = new PhysicalItem("Ztype") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Ubermacht Oracle XS") { ModelItem = new PhysicalItem("oracle") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Ubermacht Oracle") { ModelItem = new PhysicalItem("oracle2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Ubermacht Revolter", true) { ModelItem = new PhysicalItem("revolter") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Ubermacht SC1", true) { ModelItem = new PhysicalItem("sc1") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Ubermacht Sentinel XS") { ModelItem = new PhysicalItem("sentinel") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Ubermacht Sentinel 2") { ModelItem = new PhysicalItem("sentinel2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Ubermacht Sentinel 3", true) { ModelItem = new PhysicalItem("sentinel3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Ubermacht Zion") { ModelItem = new PhysicalItem("zion") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Ubermacht Zion Cabrio") { ModelItem = new PhysicalItem("zion2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Ubermacht Zion Classic", true) { ModelItem = new PhysicalItem("zion3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Ubermacht Cypher", true) { ModelItem = new PhysicalItem("cypher") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Ubermacht Rebla GTS", true) { ModelItem = new PhysicalItem("rebla") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Benson") { ModelItem = new PhysicalItem("Benson") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Blade", true) { ModelItem = new PhysicalItem("blade") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Bobcat XL") { ModelItem = new PhysicalItem("bobcatXL") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Bullet") { ModelItem = new PhysicalItem("bullet") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Caracara", true) { ModelItem = new PhysicalItem("caracara") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Caracara 4x4", true) { ModelItem = new PhysicalItem("caracara2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Chino", true) { ModelItem = new PhysicalItem("chino") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Chino Custom", true) { ModelItem = new PhysicalItem("chino2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Clique", true) { ModelItem = new PhysicalItem("clique") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Contender", true) { ModelItem = new PhysicalItem("contender") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Dominator","Baby boomer teen dream repackaged for the mass market generation. A muscle car without the muscle that's the rental vehicle of choice for tourists looking for a slice of 'real' Americana.") { ModelItem = new PhysicalItem("Dominator") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Pisswasser Dominator") { ModelItem = new PhysicalItem("dominator2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Dominator GTX*","Step one: take the best-looking muscle car the 60's ever saw, and introduce it to the greatest American supercar of the modern era. When your pedigree is this damn good, there's nothing wrong with keeping it in the family.") { ModelItem = new PhysicalItem("dominator3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Apocalypse Dominator", true) { ModelItem = new PhysicalItem("dominator4") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Future Shock Dominator", true) { ModelItem = new PhysicalItem("dominator5") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Nightmare Dominator", true) { ModelItem = new PhysicalItem("dominator6") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Dominator ASP", true) { ModelItem = new PhysicalItem("dominator7") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Dominator GTT", true) { ModelItem = new PhysicalItem("dominator8") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Ellie", true) { ModelItem = new PhysicalItem("ellie") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Flash GT", true) { ModelItem = new PhysicalItem("flashgt") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid FMJ", true) { ModelItem = new PhysicalItem("fmj") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid GB200", true) { ModelItem = new PhysicalItem("gb200") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Guardian", true) { ModelItem = new PhysicalItem("guardian") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Hotknife") { ModelItem = new PhysicalItem("hotknife") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Hustler", true) { ModelItem = new PhysicalItem("hustler") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Apocalypse Imperator", true) { ModelItem = new PhysicalItem("imperator") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Future Shock Imperator", true) { ModelItem = new PhysicalItem("imperator2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Nightmare Imperator", true) { ModelItem = new PhysicalItem("imperator3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Minivan") { ModelItem = new PhysicalItem("minivan") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Minivan Custom", true) { ModelItem = new PhysicalItem("minivan2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Monster", true) { ModelItem = new PhysicalItem("monster") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Peyote") { ModelItem = new PhysicalItem("peyote") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Peyote Gasser", true) { ModelItem = new PhysicalItem("peyote2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Peyote Custom", true) { ModelItem = new PhysicalItem("peyote3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Radius") { ModelItem = new PhysicalItem("radi") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Retinue", true) { ModelItem = new PhysicalItem("retinue") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Retinue Mk II", true) { ModelItem = new PhysicalItem("retinue2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Riata", true) { ModelItem = new PhysicalItem("riata") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Sadler") { ModelItem = new PhysicalItem("Sadler") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Sadler 2") { ModelItem = new PhysicalItem("sadler2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Sandking XL") { ModelItem = new PhysicalItem("sandking") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Sandking SWB") { ModelItem = new PhysicalItem("sandking2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Slamtruck", true) { ModelItem = new PhysicalItem("slamtruck") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Slamvan", true) { ModelItem = new PhysicalItem("slamvan") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Lost Slamvan", true) { ModelItem = new PhysicalItem("slamvan2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Slamvan Custom", true) { ModelItem = new PhysicalItem("slamvan3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Apocalypse Slamvan", true) { ModelItem = new PhysicalItem("slamvan4") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Future Shock Slamvan", true) { ModelItem = new PhysicalItem("slamvan5") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Nightmare Slamvan", true) { ModelItem = new PhysicalItem("slamvan6") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Speedo") { ModelItem = new PhysicalItem("speedo") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Clown Van") { ModelItem = new PhysicalItem("speedo2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Speedo Custom", true) { ModelItem = new PhysicalItem("speedo4") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Stanier","If you took a cab or got arrested in the 1990s, there's a high chance you ended up in the back of a Vapid Stanier. Discontinued following widespread reports of fuel tanks exploding on impact in rear-end collisions. So try to avoid that.") { ModelItem = new PhysicalItem("stanier") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Trophy Truck", true) { ModelItem = new PhysicalItem("trophytruck") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Desert Raid", true) { ModelItem = new PhysicalItem("trophytruck2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vapid Winky", true) { ModelItem = new PhysicalItem("winky") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vulcar Fagaloa", true) { ModelItem = new PhysicalItem("fagaloa") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vulcar Ingot") { ModelItem = new PhysicalItem("ingot") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vulcar Nebula Turbo", true) { ModelItem = new PhysicalItem("nebula") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vulcar Warrener", true) { ModelItem = new PhysicalItem("warrener") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vulcar Warrener HKR", true) { ModelItem = new PhysicalItem("warrener2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Vysser Neo", true) { ModelItem = new PhysicalItem("neo") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Weeny Dynasty", true) { ModelItem = new PhysicalItem("Dynasty") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Weeny Issi") { ModelItem = new PhysicalItem("issi2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Weeny Issi Classic", true) { ModelItem = new PhysicalItem("issi3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Weeny Apocalypse Issi", true) { ModelItem = new PhysicalItem("issi4") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Weeny Future Shock Issi", true) { ModelItem = new PhysicalItem("issi5") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Weeny Nightmare Issi", true) { ModelItem = new PhysicalItem("issi6") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Weeny Issi Sport", true) { ModelItem = new PhysicalItem("issi7") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Western Bagger") { ModelItem = new PhysicalItem("bagger") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Western Cliffhanger", true) { ModelItem = new PhysicalItem("cliffhanger") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Western Daemon") { ModelItem = new PhysicalItem("daemon") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Western Daemon 2", true) { ModelItem = new PhysicalItem("daemon2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Western Apocalypse Deathbike", true) { ModelItem = new PhysicalItem("deathbike") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Western Future Shock Deathbike", true) { ModelItem = new PhysicalItem("deathbike2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Western Nightmare Deathbike", true) { ModelItem = new PhysicalItem("deathbike3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Western Gargoyle", true) { ModelItem = new PhysicalItem("gargoyle") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Western Nightblade", true) { ModelItem = new PhysicalItem("nightblade") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Western Rat Bike", true) { ModelItem = new PhysicalItem("ratbike") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Western Rampant Rocket", true) { ModelItem = new PhysicalItem("rrocket") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Western Sovereign", true) { ModelItem = new PhysicalItem("sovereign") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Western Wolfsbane", true) { ModelItem = new PhysicalItem("wolfsbane") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Western Zombie Bobber", true) { ModelItem = new PhysicalItem("zombiea") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Western Zombie Chopper", true) { ModelItem = new PhysicalItem("zombieb") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Willard Faction", true) { ModelItem = new PhysicalItem("faction") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Willard Faction Custom", true) { ModelItem = new PhysicalItem("faction2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Willard Faction Custom Donk", true) { ModelItem = new PhysicalItem("faction3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Zirconium Journey") { ModelItem = new PhysicalItem("journey") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Zirconium Stratum") { ModelItem = new PhysicalItem("stratum") { Type = ePhysicalItemType.Vehicle }},
        
        //Heli
        new ModItem("Buckingham SuperVolito", true) { ModelItem = new PhysicalItem("supervolito") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Buckingham SuperVolito Carbon", true) { ModelItem = new PhysicalItem("supervolito2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Buckingham Swift", true) { ModelItem = new PhysicalItem("swift") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Buckingham Swift Deluxe", true) { ModelItem = new PhysicalItem("swift2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Buckingham Volatus", true) { ModelItem = new PhysicalItem("volatus") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Mammoth Thruster", true) { ModelItem = new PhysicalItem("thruster") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Nagasaki Havok", true) { ModelItem = new PhysicalItem("havok") { Type = ePhysicalItemType.Vehicle }},

        //plane
        new ModItem("Buckingham Alpha-Z1", true) { ModelItem = new PhysicalItem("alphaz1") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Buckingham Howard NX-25", true) { ModelItem = new PhysicalItem("howard") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Buckingham Luxor") { ModelItem = new PhysicalItem("luxor") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Buckingham Luxor Deluxe", true) { ModelItem = new PhysicalItem("luxor2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Buckingham Miljet", true) { ModelItem = new PhysicalItem("Miljet") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Buckingham Nimbus", true) { ModelItem = new PhysicalItem("nimbus") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Buckingham Pyro", true) { ModelItem = new PhysicalItem("pyro") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Buckingham Shamal") { ModelItem = new PhysicalItem("Shamal") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Buckingham Vestra", true) { ModelItem = new PhysicalItem("vestra") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Mammoth Avenger", true) { ModelItem = new PhysicalItem("avenger") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Mammoth Avenger 2", true) { ModelItem = new PhysicalItem("avenger2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Mammoth Dodo") { ModelItem = new PhysicalItem("dodo") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Mammoth Hydra", true) { ModelItem = new PhysicalItem("hydra") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Mammoth Mogul", true) { ModelItem = new PhysicalItem("mogul") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Mammoth Tula", true) { ModelItem = new PhysicalItem("tula") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Nagasaki Ultralight", true) { ModelItem = new PhysicalItem("microlight") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Western Besra", true) { ModelItem = new PhysicalItem("besra") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Western Rogue", true) { ModelItem = new PhysicalItem("rogue") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Western Seabreeze", true) { ModelItem = new PhysicalItem("seabreeze") { Type = ePhysicalItemType.Vehicle }},

        //boat
        new ModItem("Dinka Marquis") { ModelItem = new PhysicalItem("marquis") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Lampadati Toro", true) { ModelItem = new PhysicalItem("toro") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Lampadati Toro", true) { ModelItem = new PhysicalItem("toro2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Nagasaki Dinghy") { ModelItem = new PhysicalItem("Dinghy") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Nagasaki Dinghy 2") { ModelItem = new PhysicalItem("dinghy2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Nagasaki Dinghy 3", true) { ModelItem = new PhysicalItem("dinghy3") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Nagasaki Dinghy 4", true) { ModelItem = new PhysicalItem("dinghy4") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Nagasaki Weaponized Dinghy", true) { ModelItem = new PhysicalItem("dinghy5") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pegassi Speeder", true) { ModelItem = new PhysicalItem("speeder") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Pegassi Speeder", true) { ModelItem = new PhysicalItem("speeder2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Shitzu Jetmax") { ModelItem = new PhysicalItem("jetmax") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Shitzu Longfin", true) { ModelItem = new PhysicalItem("longfin") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Shitzu Squalo") { ModelItem = new PhysicalItem("squalo") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Shitzu Suntrap") { ModelItem = new PhysicalItem("Suntrap") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Shitzu Tropic") { ModelItem = new PhysicalItem("tropic") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Shitzu Tropic", true) { ModelItem = new PhysicalItem("tropic2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Speedophile Seashark") { ModelItem = new PhysicalItem("seashark") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Speedophile Seashark 2") { ModelItem = new PhysicalItem("seashark2") { Type = ePhysicalItemType.Vehicle }},
        new ModItem("Speedophile Seashark 3", true) { ModelItem = new PhysicalItem("seashark3") { Type = ePhysicalItemType.Vehicle }},
    };
    }

}




//prop_cs_crackpipe.ydr
//prop_cs_coke_line.ydr
//prop_armour_pickup.ydr body armor


//prop_cs_steak.ydr steak!
//prop_turkey_leg_01.ydr TURKEY LEG!


//prop_weed_bottle.ydr

//p_cs_papers_01.ydr rolling papers closed
//p_cs_papers_02.ydr rolling papers open 
//p_cs_papers_03.ydr rolling papaers completemly closed

//p_ing_coffeecup_01.yft bean machine coffee
//p_ing_coffeecup_02.yft blank normal coffee cup

//ba_meth_smallbag.ydr
//bkr_prop_meth_openbag_01a.ydr same as above but open
//bkr_prop_weed_bag_01a.ydr might not have textures? probably does
//bkr_prop_weed_smallbag_01a.ydr might not have texutres?

//sf_prop_sf_bag_weed_01a.ydr regular weed closed with texures
//sf_prop_sf_bag_weed_open_01a.ydr ld organics bag, open

//p_meth_bag_01_s.ydr looks like small amount of white poweder in a platics bag, closed top
//prop_inhaler_01.ydr USE INHALER FOR SOME OF THESE
//new ModItem("White Widow Preroll",eConsumableType.Smoke) {
//    ModelItem = new PhysicalItem("p_amb_joint_01",57005, new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)), AmountPerPackage = 5, IntoxicantName = "Marijuana", RequiredToolType = ToolTypes.Lighter },
//new ModItem("OG Kush Preroll",eConsumableType.Smoke) {
//    ModelItem = new PhysicalItem("p_amb_joint_01",57005, new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)), AmountPerPackage = 5, IntoxicantName = "Marijuana", RequiredToolType = ToolTypes.Lighter },
//new ModItem("Northern Lights Preroll",eConsumableType.Smoke) {
//    ModelItem = new PhysicalItem("p_amb_joint_01",57005, new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)), AmountPerPackage = 5, IntoxicantName = "Marijuana", RequiredToolType = ToolTypes.Lighter },
//new ModItem("White Widow Gram",eConsumableType.None) {
//    ModelItem = new PhysicalItem("prop_weed_bottle",57005,new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0f, 0f, 0f)) },
//new ModItem("OG Kush Gram",eConsumableType.None) {
//    ModelItem = new PhysicalItem("prop_weed_bottle",57005,new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0f, 0f, 0f)) },
//new ModItem("Northern Lights Gram",eConsumableType.None) {
//    ModelItem = new PhysicalItem("prop_weed_bottle",57005,new Vector3(0.0f, 0.0f, 0.0f),new Rotator(0f, 0f, 0f)) },


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