using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class CraftableItems : ICraftableItems
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\CraftableItems.xml";
    private List<CraftableItem> CraftableList;
    public List<CraftableItem> Items => CraftableList;
    public CraftableItem Get(string name)
    {
        return CraftableList.FirstOrDefault(x => x.Name == name);
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
    }

    private void DefaultConfig()
    {
        CraftableList = new List<CraftableItem>()
        {
            new CraftableItem("Methamphetamine", new List<Ingredient>() { 
                new Ingredient() { IngredientName =  "Meth Supplies", Quantity = 2 } 
            }, true),
        };
        Serialization.SerializeParams(CraftableList, ConfigFileName);
    }
    public void SerializeAllSettings()
    {
        Serialization.SerializeParams(CraftableList == null ? new List<CraftableItem>() : CraftableList, ConfigFileName);
    }
}
