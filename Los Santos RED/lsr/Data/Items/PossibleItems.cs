using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PossibleItems
{
    public PossibleItems()
    {

    }
    public List<FlashlightItem> FlashlightItems { get; private set; } = new List<FlashlightItem>();
    public List<ModItem> ModItems { get; private set; } = new List<ModItem>();
}

