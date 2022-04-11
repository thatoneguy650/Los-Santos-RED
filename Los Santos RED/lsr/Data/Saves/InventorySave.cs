using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class InventorySave
{
    public InventorySave()
    {

    }
    public InventorySave(string modItemName, List<float> remainingPercent)
    {
        ModItemName = modItemName;
        RemainingPercent = remainingPercent;
    }

    public string ModItemName { get; set; }
    public List<float> RemainingPercent { get; set; } = new List<float>() { 0.0f };
}
