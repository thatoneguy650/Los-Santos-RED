using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class TheftInteractItem
{
    private ModItem modItem;
    public ModItem ModItem => modItem;
    public string ModItemName { get; set; }
    public int MinItems { get; set; } = 1;
    public int MaxItems { get; set; } = 1;
    public int Percentage { get; set; }
    public TheftInteractItem()
    {
    }

    public TheftInteractItem(string modItemName, int minItems, int maxItems, int percentage)
    {
        ModItemName = modItemName;
        MinItems = minItems;
        MaxItems = maxItems;
        Percentage = percentage;
    }

    public void Setup(IModItems modItems)
    {
        if(string.IsNullOrEmpty(ModItemName))
        {
            return;
        }
        modItem = modItems.Get(ModItemName);
    }
}

