using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class AttachedItem
{
    public AttachedItem()
    {
    }

    public AttachedItem(ModItem modItem, Rage.Object spawnedItem)
    {
        ModItem = modItem;
        SpawnedItem = spawnedItem;
    }

    public ModItem ModItem { get; set; }
    public Rage.Object SpawnedItem { get; set; }
}

