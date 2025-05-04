using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Helper.Crafting;
using LosSantosRED.lsr.Interface;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class CraftableItems : ICraftableItems
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\CraftableItems.xml";
    private List<CraftableItem> CraftableList;
    public List<CraftableItem> Items => CraftableList;
    private IModItems ModItems;

    public Dictionary<string, CraftableItemLookupModel> CraftablesLookup { get; set; }
    public CraftableItem Get(string name)
    {
        return CraftableList.FirstOrDefault(x => x.Name == name);
    }
    public CraftableItems(IModItems modItems)
    {
        ModItems = modItems;
    }
    public void ReadConfig(string configName)
    {
        string fileName = string.IsNullOrEmpty(configName) ? "CraftableItems_*.xml" : $"CraftableItems_{configName}.xml";

        DirectoryInfo LSRDirectory = new DirectoryInfo("Plugins\\LosSantosRED");
        FileInfo ConfigFile = LSRDirectory.GetFiles(fileName).OrderByDescending(x => x.Name).FirstOrDefault();
        if (ConfigFile != null && !configName.Equals("Default"))
        {
            EntryPoint.WriteToConsole($"Loaded Craftable Items config: {ConfigFile.FullName}", 0);
            CraftableList = Serialization.DeserializeParams<CraftableItem>(ConfigFile.FullName);
        }
        else if (File.Exists(ConfigFileName))
        {
            EntryPoint.WriteToConsole($"Loaded Craftable Items config  {ConfigFileName}", 0);
            CraftableList = Serialization.DeserializeParams<CraftableItem>(ConfigFileName);
        }
        else
        {
            EntryPoint.WriteToConsole($"No Craftable Items config found, creating default", 0);
            DefaultConfig();
        }
        //Load Additive
        foreach (FileInfo fileInfo in LSRDirectory.GetFiles("CraftableItems+_*.xml").OrderByDescending(x => x.Name))
        {
            EntryPoint.WriteToConsole($"Loaded ADDITIVE Craftable Items config  {fileInfo.FullName}", 0);
            List<CraftableItem> additivePossibleItems = Serialization.DeserializeParams<CraftableItem>(fileInfo.FullName);
            CraftableList.RemoveAll(x => additivePossibleItems.Any(y => y.Name.ToLower() == x.Name.ToLower()));
            CraftableList.AddRange(additivePossibleItems);
        }



        CraftableList.RemoveAll(x => ModItems.Get(x.Resultant) == null || (x.SingleUnit == false && x.ResultantAmount < 1) || x.Ingredients.Any(y => y.Quantity < 1));






    }

    private void DefaultConfig()
    {
        CraftableList = new List<CraftableItem>()
        {
            new CraftableItem("Methamphetamine", "Methamphetamine", new List<Ingredient>()
            {
                new Ingredient() { IngredientName =  "Chesty", Quantity = 5 },
                new Ingredient() { IngredientName =  "pH strip", Quantity = 3 },
                new Ingredient() { IngredientName =  "Chemicals", Quantity = 1 },
                new Ingredient() { IngredientName =  "BlazeBox Ignite", Quantity = 5 }
            })
            {
                CrimeId = StaticStrings.DealingDrugsCrimeID,
                ResultantAmount = 7,
                Cooldown = 8000,
                CraftingFlags = new HashSet<string>{ "DrugLab","JOURNEY" },
                Category = "Narcotics",
                AnimationDictionary = "anim@amb@business@meth@meth_monitoring_cooking@cooking@",
                AnimationName = "chemical_pour_short_cooker"
            },

            new CraftableItem("Cook Crack", "Crack",
            new List<Ingredient>()
            {
                new Ingredient() { IngredientName = "Cocaine", Quantity = 1},
                new Ingredient() { IngredientName = "Bottle of Raine Water", Quantity = 2},
                new Ingredient() { IngredientName = "Baking Soda", Quantity = 2}
            })
            { CrimeId = StaticStrings.DealingDrugsCrimeID,
                ResultantAmount = 6,
                Cooldown = 8000,
                CraftingFlags = new HashSet<string>{ "DrugLab", "Stove" },
                Category = "Narcotics",
                AnimationDictionary = "anim@amb@business@meth@meth_smash_weight_check@",
                AnimationName = "break_weight_char02"
            },

            new CraftableItem("Molotov Cocktail", "Improvised Incendiary", new List<Ingredient>()
            {
                new Ingredient() { IngredientName =  "NOGO Vodka", Quantity = 1 },
                new Ingredient() { IngredientName =  "DIC Lighter", Quantity = 1 , IsConsumed = false}
            })
            { CrimeId = StaticStrings.DealingGunsCrimeID,
                ResultantAmount = 2,
                Cooldown = 5000,
                SingleUnit = true,
                AnimationDictionary= "anim@scripted@player@freemode@ig4_chem_pour@male@",
                AnimationName = "action"
            },

            new CraftableItem("Lean","Lean", new List<Ingredient>()
            {
                new Ingredient() { IngredientName = "Cup of Sprunk", Quantity = 1},
                new Ingredient() { IngredientName = "P's & Q's", Quantity = 1},
                new Ingredient() { IngredientName = "Chesty", Quantity = 1}
            })
            { ResultantAmount = 1 ,
                Cooldown = 4000,
                AnimationDictionary= "anim@amb@nightclub@mini@drinking@drinking_shots@ped_d@normal",
                AnimationName = "pour_one"
            },
            new CraftableItem("DIY pipe bomb","Pipe Bomb", new List<Ingredient>()
            {
                new Ingredient() { IngredientName = "Metal Tube", Quantity = 1},
                new Ingredient() { IngredientName = "Flint Duct Tape", Quantity = 1},
                new Ingredient() { IngredientName = "Fuse", Quantity = 1},
                new Ingredient() { IngredientName = "Scrap Metal", Quantity = 3}
            })
            {
                CrimeId = StaticStrings.TerroristActivityCrimeID,
                Cooldown = 5000,
                ResultantAmount = 1,
                SingleUnit= true,
            }
        };
        Serialization.SerializeParams(CraftableList, ConfigFileName);
    }
    public void SerializeAllSettings()
    {
        Serialization.SerializeParams(CraftableList == null ? new List<CraftableItem>() : CraftableList, ConfigFileName);
    }
}
