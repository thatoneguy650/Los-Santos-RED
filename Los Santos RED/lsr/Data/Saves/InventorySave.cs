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
    public InventorySave(string modItemName, float remainingPercent)
    {
        ModItemName = modItemName;
        RemainingPercent = remainingPercent;
    }

    public string ModItemName { get; set; }
    public float RemainingPercent { get; set; } = 0.0f;
}
