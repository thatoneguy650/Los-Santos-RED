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


public class ConsumableSubstances : IConsumableSubstances
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\ConsumableSubstances.xml";
    private List<ConsumableSubstance> ConsumableSubstancesList;
    public List<ConsumableSubstance> Consumables => ConsumableSubstancesList;
    public void ReadConfig()
    {
        if (File.Exists(ConfigFileName))
        {
            ConsumableSubstancesList = Serialization.DeserializeParams<ConsumableSubstance>(ConfigFileName);
        }
        else
        {
            DefaultConfig();
            Serialization.SerializeParams(ConsumableSubstancesList, ConfigFileName);
        }
    }
    private void DefaultConfig()
    {
        ConsumableSubstancesList = new List<ConsumableSubstance>
        {

            new ConsumableSubstance("Hot Dog",eConsumableType.Eat,eConsumableCategory.Food,"prop_cs_hotdog_01",57005,new Vector3(0.14f, -0.02f, -0.04f),new Rotator(178.0f, 28.0f, 0.0f)) { Price = 5 },
            new ConsumableSubstance("Hot Sausage",eConsumableType.Eat,eConsumableCategory.Food,"prop_cs_hotdog_01",57005,new Vector3(0.14f, -0.02f, -0.04f),new Rotator(178.0f, 28.0f, 0.0f)) { Price = 5 },
            new ConsumableSubstance("Hot Pretzel",eConsumableType.Eat,eConsumableCategory.Food,"",57005,new Vector3(0.14f, -0.02f, -0.04f),new Rotator(178.0f, 28.0f, 0.0f)) { Price = 2 },
            new ConsumableSubstance("3 Mini Pretzels",eConsumableType.Eat,eConsumableCategory.Food,"",57005,new Vector3(0.14f, -0.02f, -0.04f),new Rotator(178.0f, 28.0f, 0.0f)) { Price = 3 },
            new ConsumableSubstance("Nuts",eConsumableType.Eat,eConsumableCategory.Food,"",57005,new Vector3(0.14f, -0.02f, -0.04f),new Rotator(178.0f, 28.0f, 0.0f)) { Price = 2 },
            new ConsumableSubstance("Bottle of Water",eConsumableType.Drink,eConsumableCategory.Soda,"",57005,new Vector3(0.13f, -0.06f, -0.06f),new Rotator(-73.0f, 0.0f, 0.0f)) { Price = 2 },

            new ConsumableSubstance("Burger",eConsumableType.Eat,eConsumableCategory.Food,"prop_cs_burger_01",57005,new Vector3(0.16f, 0.01f, -0.04f),new Rotator(0.0f, 28.0f, 0.0f)) { Price = 1 },
            new ConsumableSubstance("Megacheese Burger",eConsumableType.Eat,eConsumableCategory.Food,"prop_cs_burger_01",57005,new Vector3(0.16f, 0.01f, -0.04f),new Rotator(0.0f, 28.0f, 0.0f)) { Price = 2 },
            new ConsumableSubstance("Double Burger",eConsumableType.Eat,eConsumableCategory.Food,"prop_cs_burger_01",57005,new Vector3(0.16f, 0.01f, -0.04f),new Rotator(0.0f, 28.0f, 0.0f)) { Price = 2 },
            new ConsumableSubstance("Kingsize Burger",eConsumableType.Eat,eConsumableCategory.Food,"prop_cs_burger_01",57005,new Vector3(0.16f, 0.01f, -0.04f),new Rotator(0.0f, 28.0f, 0.0f)) { Price = 2 },
            new ConsumableSubstance("Bacon Burger",eConsumableType.Eat,eConsumableCategory.Food,"prop_cs_burger_01",57005,new Vector3(0.16f, 0.01f, -0.04f),new Rotator(0.0f, 28.0f, 0.0f)) { Price = 2 },



            new ConsumableSubstance("Redwood Cigarette",eConsumableType.Smoke,eConsumableCategory.Cigarette,"ng_proc_cigarette01a",57005, new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)) { Price = 30, AmountPerPackage = 20, PackageModel = "p_fag_packet_01_s" },
            new ConsumableSubstance("Joint",eConsumableType.Smoke,eConsumableCategory.Marijuana,"p_amb_joint_01",57005, new Vector3(0.14f, 0.03f, 0.0f),new Rotator(0.49f, 79f, 79f)) { Price = 25, AmountPerPackage = 5, IsIntoxicating = true },
            new ConsumableSubstance("Estancia Cigar",eConsumableType.Smoke,eConsumableCategory.Cigarette,"prop_cigar_02",57005, new Vector3(0.17f, 0.02f, 0.0f),new Rotator(0.0f, -78f, 0f)) { Price = 50, AmountPerPackage = 20, PackageModel ="p_cigar_pack_02_s" },

            new ConsumableSubstance("Donut",eConsumableType.Eat,eConsumableCategory.Food,"prop_donut_01",57005,new Vector3(0.15f, 0.01f, -0.03f),new Rotator(-15.0f, 17.0f, 0.0f)) { Price = 1 },
            new ConsumableSubstance("Slice of Pizza",eConsumableType.Eat,eConsumableCategory.Food,"v_res_tt_pizzaplate",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)) { Price = 3 },
            
            new ConsumableSubstance("Burger",eConsumableType.Eat,eConsumableCategory.Food,"prop_cs_burger_01",57005,new Vector3(0.16f, 0.01f, -0.04f),new Rotator(0.0f, 28.0f, 0.0f)) { Price = 3 },

            new ConsumableSubstance("40 oz",eConsumableType.Drink,eConsumableCategory.Beer,"prop_cs_beer_bot_40oz",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)) { IsIntoxicating = true, Price = 5 },
            new ConsumableSubstance("Bottle of A.M.",eConsumableType.Drink,eConsumableCategory.Beer,"prop_beer_am",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)) { IsIntoxicating = true, Price = 4 },
            new ConsumableSubstance("Bottle of PiBwasser",eConsumableType.Drink,eConsumableCategory.Beer,"prop_amb_beer_bottle",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)) { IsIntoxicating = true, Price = 3 },
            new ConsumableSubstance("Bottle of Barracho",eConsumableType.Drink,eConsumableCategory.Beer,"prop_beer_bar",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)) { IsIntoxicating = true, Price = 4 },
            new ConsumableSubstance("Bottle of Blarneys",eConsumableType.Drink,eConsumableCategory.Beer,"prop_beer_blr",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)) { IsIntoxicating = true, Price = 4 },
            new ConsumableSubstance("Bottle of Jakeys",eConsumableType.Drink,eConsumableCategory.Beer,"prop_beer_jakey",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)) { IsIntoxicating = true, Price = 3 },
            new ConsumableSubstance("Bottle of Logger",eConsumableType.Drink,eConsumableCategory.Beer,"prop_beer_logger",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)) { IsIntoxicating = true, Price = 4 },
            new ConsumableSubstance("Bottle of Patriot",eConsumableType.Drink,eConsumableCategory.Beer,"prop_beer_patriot",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)) { IsIntoxicating = true, Price = 3 },
            new ConsumableSubstance("Bottle of Pride",eConsumableType.Drink,eConsumableCategory.Beer,"prop_beer_pride",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)) { IsIntoxicating = true, Price = 3 },
            new ConsumableSubstance("Bottle of Stronz",eConsumableType.Drink,eConsumableCategory.Beer,"prop_beer_stz",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)) { IsIntoxicating = true , Price = 4},
            new ConsumableSubstance("Bottle of Dusche",eConsumableType.Drink,eConsumableCategory.Beer,"prop_beerdusche",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)) { IsIntoxicating = true, Price = 4 },


            

            new ConsumableSubstance("Can of eCola",eConsumableType.Drink,eConsumableCategory.Soda,"ng_proc_sodacan_01a",57005,new Vector3(0.13f, -0.06f, -0.06f),new Rotator(-73.0f, 0.0f, 0.0f)) { Price = 1 },
            new ConsumableSubstance("Can of Sprunk",eConsumableType.Drink,eConsumableCategory.Soda,"ng_proc_sodacan_01b",57005,new Vector3(0.13f, -0.06f, -0.06f),new Rotator(-73.0f, 0.0f, 0.0f)) { Price = 1 },
            new ConsumableSubstance("Cup of eCola",eConsumableType.Drink,eConsumableCategory.Soda,"ng_proc_sodacup_01a",57005,new Vector3(0.07f, -0.1f, -0.1f),new Rotator(-74.0f, 28.0f, 3.0f)) { Price = 2 },
            new ConsumableSubstance("Cup of Sprunk",eConsumableType.Drink,eConsumableCategory.Soda,"ng_proc_sodacup_01b",57005,new Vector3(0.07f, -0.1f, -0.1f),new Rotator(-74.0f, 28.0f, 3.0f)) { Price = 2 },

            new ConsumableSubstance("Cup of Coffee",eConsumableType.Drink,eConsumableCategory.Coffee,"prop_food_bs_coffee",57005,new Vector3(0.12f, -0.06f, -0.08f),new Rotator(-78.0f, 0.0f, 0.0f)) { Price = 2 },

            new ConsumableSubstance("French Fries",eConsumableType.Eat,eConsumableCategory.Snack,"prop_food_bs_chips",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)) { Price = 2 },
            new ConsumableSubstance("Banana",eConsumableType.Eat,eConsumableCategory.Snack,"ng_proc_food_nana1a",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)) { Price = 3 },
            new ConsumableSubstance("Phat Chips",eConsumableType.Eat,eConsumableCategory.Snack,"ng_proc_food_chips01a",57005,new Vector3(0.12f, 0.0f, -0.06f),new Rotator(-77.0f, 23.0f, 0.0f)) { Price = 2 },

        //            ConsumableSubstance HotDog = ConsumableSubstances.Get("Hot Dog");
        //ConsumableSubstance HotSausage = ConsumableSubstances.Get("Hot Sausage");
        //ConsumableSubstance HotPretzel = ConsumableSubstances.Get("Hot Pretzel");
        //ConsumableSubstance ThreeMiniPretzel = ConsumableSubstances.Get("3 Mini Pretzel");
        //ConsumableSubstance nuts = ConsumableSubstances.Get("Nuts");
        //ConsumableSubstance cansprunk = ConsumableSubstances.Get("Can of Sprunk");
        //ConsumableSubstance bottleofwater = ConsumableSubstances.Get("Bottle of Water");
    };
    }
    public ConsumableSubstance Get(string name)
    {
        return ConsumableSubstancesList.FirstOrDefault(x=> x.Name == name);
    }
}

