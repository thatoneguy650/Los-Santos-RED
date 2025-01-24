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
    public void ReadConfig()
    {
        DirectoryInfo LSRDirectory = new DirectoryInfo("Plugins\\LosSantosRED");
        FileInfo ConfigFile = LSRDirectory.GetFiles("Craftable*.xml").OrderByDescending(x => x.Name).FirstOrDefault();
        if (ConfigFile != null)
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
        CraftableList.RemoveAll(x => ModItems.Get(x.Resultant) == null || (x.SingleUnit == false && x.ResultantAmount <1) || x.Ingredients.Any(y=>y.Quantity < 1) );
    }

    private void DefaultConfig()
    {
        CraftableList = new List<CraftableItem>()
        {
            new CraftableItem("Methamphetamine", "Methamphetamine", new List<Ingredient>() {
                new Ingredient() { IngredientName =  "Chesty", Quantity = 2 }
            }) { CrimeId = StaticStrings.DealingDrugsCrimeID, ResultantAmount = 1, Cooldown = 2000},
            new CraftableItem("Cut Cocaine", "Crack", new List<Ingredient>() {
                new Ingredient() { IngredientName =  "Cocaine", Quantity = 1}
            }) { CrimeId = StaticStrings.DealingDrugsCrimeID, ResultantAmount = 2, Cooldown = 2000},
            new CraftableItem("Molotov Cocktail", "Improvised Incendiary", new List<Ingredient>() {
                new Ingredient() { IngredientName =  "NOGO Vodka", Quantity = 1 },
                new Ingredient() { IngredientName =  "DIC Lighter", Quantity = 1 }
            }) { CrimeId = StaticStrings.DealingGunsCrimeID, ResultantAmount = 2, Cooldown = 2000, SingleUnit = true},
        };
        Serialization.SerializeParams(CraftableList, ConfigFileName);
    }
    public void SerializeAllSettings()
    {
        Serialization.SerializeParams(CraftableList == null ? new List<CraftableItem>() : CraftableList, ConfigFileName);
    }
}
//
