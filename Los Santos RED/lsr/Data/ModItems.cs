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
            new ModItem("Hot Dog",eConsumableType.Eat,"prop_cs_hotdog_01",57005,new Vector3(0.14f, -0.02f, -0.04f),new Rotator(178.0f, 28.0f, 0.0f)) ,
            new ModItem("Hot Sausage",eConsumableType.Eat,"prop_cs_hotdog_01",57005,new Vector3(0.14f, -0.02f, -0.04f),new Rotator(178.0f, 28.0f, 0.0f)),
            new ModItem("Hot Pretzel",eConsumableType.Eat,"",57005,new Vector3(0.14f, -0.02f, -0.04f),new Rotator(178.0f, 28.0f, 0.0f)),
            new ModItem("3 Mini Pretzels",eConsumableType.Eat,"",57005,new Vector3(0.14f, -0.02f, -0.04f),new Rotator(178.0f, 28.0f, 0.0f)),
            new ModItem("Nuts",eConsumableType.Eat,"",57005,new Vector3(0.14f, -0.02f, -0.04f),new Rotator(178.0f, 28.0f, 0.0f)),
            new ModItem("Bottle of Water",eConsumableType.Drink,"",57005,new Vector3(0.13f, -0.06f, -0.06f),new Rotator(-73.0f, 0.0f, 0.0f)),
            new ModItem("Burger",eConsumableType.Eat,"prop_cs_burger_01",57005,new Vector3(0.16f, 0.01f, -0.04f),new Rotator(0.0f, 28.0f, 0.0f)),
            new ModItem("Megacheese Burger",eConsumableType.Eat,"prop_cs_burger_01",57005,new Vector3(0.16f, 0.01f, -0.04f),new Rotator(0.0f, 28.0f, 0.0f)),
            new ModItem("Double Burger",eConsumableType.Eat,"prop_cs_burger_01",57005,new Vector3(0.16f, 0.01f, -0.04f),new Rotator(0.0f, 28.0f, 0.0f)) ,
            new ModItem("Kingsize Burger",eConsumableType.Eat,"prop_cs_burger_01",57005,new Vector3(0.16f, 0.01f, -0.04f),new Rotator(0.0f, 28.0f, 0.0f)) ,
            new ModItem("Bacon Burger",eConsumableType.Eat,"prop_cs_burger_01",57005,new Vector3(0.16f, 0.01f, -0.04f),new Rotator(0.0f, 28.0f, 0.0f)) ,
            new ModItem("Redwood Cigarette",eConsumableType.Smoke,"ng_proc_cigarette01a","p_fag_packet_01_s",57005, new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)) {  AmountPerPackage = 20 },
            new ModItem("Joint",eConsumableType.Smoke,"p_amb_joint_01",57005, new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)) { AmountPerPackage = 5, IsIntoxicating = true },
            new ModItem("Estancia Cigar",eConsumableType.Smoke,"prop_cigar_02","p_cigar_pack_02_s",57005, new Vector3(0.17f, 0.02f, 0.0f),new Rotator(0.0f, -78f, 0f)) { AmountPerPackage = 20 },
            new ModItem("Donut",eConsumableType.Eat,"prop_donut_01",57005,new Vector3(0.15f, 0.01f, -0.03f),new Rotator(-15.0f, 17.0f, 0.0f)) ,
            new ModItem("Slice of Pizza",eConsumableType.Eat,"v_res_tt_pizzaplate",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)) ,
            new ModItem("Burger",eConsumableType.Eat,"prop_cs_burger_01",57005,new Vector3(0.16f, 0.01f, -0.04f),new Rotator(0.0f, 28.0f, 0.0f)),
            new ModItem("40 oz",eConsumableType.Drink,"prop_cs_beer_bot_40oz",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)) { IsIntoxicating = true },
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
            new ModItem("Can of eCola",eConsumableType.Drink,"ng_proc_sodacan_01a",57005,new Vector3(0.13f, -0.06f, -0.06f),new Rotator(-73.0f, 0.0f, 0.0f)),
            new ModItem("Can of Sprunk",eConsumableType.Drink,"ng_proc_sodacan_01b",57005,new Vector3(0.13f, -0.06f, -0.06f),new Rotator(-73.0f, 0.0f, 0.0f)),
            new ModItem("Cup of eCola",eConsumableType.Drink,"ng_proc_sodacup_01a",57005,new Vector3(0.07f, -0.1f, -0.1f),new Rotator(-74.0f, 28.0f, 3.0f)),
            new ModItem("Cup of Sprunk",eConsumableType.Drink,"ng_proc_sodacup_01b",57005,new Vector3(0.07f, -0.1f, -0.1f),new Rotator(-74.0f, 28.0f, 3.0f)),
            new ModItem("Cup of Coffee",eConsumableType.Drink,"prop_food_bs_coffee",57005,new Vector3(0.12f, -0.06f, -0.08f),new Rotator(-78.0f, 0.0f, 0.0f)),
            new ModItem("French Fries",eConsumableType.Eat,"prop_food_bs_chips",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)),
            new ModItem("Banana",eConsumableType.Eat,"ng_proc_food_nana1a",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)),
            new ModItem("Orange",eConsumableType.Eat,"ng_proc_food_ornge1a",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)),
            new ModItem("Apple",eConsumableType.Eat,"ng_proc_food_aple1a",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)),
            
            new ModItem("Phat Chips",eConsumableType.Eat,"ng_proc_food_chips01a",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)),
            new ModItem("Triple Burger",eConsumableType.Eat,"prop_cs_burger_01",57005,new Vector3(0.16f, 0.01f, -0.04f),new Rotator(0.0f, 28.0f, 0.0f)),
            new ModItem("Bacon Triple Cheese Melt",eConsumableType.Eat,"prop_cs_burger_01",57005,new Vector3(0.16f, 0.01f, -0.04f),new Rotator(0.0f, 28.0f, 0.0f)),
            new ModItem("Jumbo Shake",eConsumableType.Drink,"",57005,new Vector3(0.16f, 0.01f, -0.04f),new Rotator(0.0f, 28.0f, 0.0f)),
            new ModItem("French Fries",eConsumableType.Eat,"",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)),
            new ModItem("Taco",eConsumableType.Eat,"",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)),

            new ModItem("Screwdriver","prop_tool_screwdvr01",57005,new Vector3(0.1170f, 0.0610f, 0.0150f), new Rotator(-47.199f, 166.62f, -19.9f)),
            new ModItem("Hammer","prop_tool_hammer",57005,new Vector3(0.1170f, 0.0610f, 0.0150f), new Rotator(-47.199f, 166.62f, -19.9f)),
            new ModItem("Drill","prop_tool_drill",57005,new Vector3(0.1170f, 0.0610f, 0.0150f), new Rotator(-47.199f, 166.62f, -19.9f)) ,
            new ModItem("Pliers","prop_tool_pliers",57005,new Vector3(0.1170f, 0.0610f, 0.0150f), new Rotator(-47.199f, 166.62f, -19.9f)),
            new ModItem("Shovel","prop_tool_shovel",57005,new Vector3(0.1170f, 0.0610f, 0.0150f), new Rotator(-47.199f, 166.62f, -19.9f)),
            new ModItem("Wrench","prop_tool_wrench",57005,new Vector3(0.1170f, 0.0610f, 0.0150f), new Rotator(-47.199f, 166.62f, -19.9f)),
            

            new ModItem("Room: Single Twin",eConsumableType.Service),
            new ModItem("Room: Single Queen",eConsumableType.Service),
            new ModItem("Room: Double Queen",eConsumableType.Service),
            new ModItem("Room: Single King",eConsumableType.Service),

            new ModItem("Full",eConsumableType.Service),
            new ModItem("Half And Half",eConsumableType.Service),
            new ModItem("Head",eConsumableType.Service),
            new ModItem("Handy",eConsumableType.Service),
        };
    }
    public ModItem Get(string name)
    {
        return ModItemsList.FirstOrDefault(x => x.Name == name);
    }
}

