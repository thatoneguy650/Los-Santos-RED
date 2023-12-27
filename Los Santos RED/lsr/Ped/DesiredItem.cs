using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DesiredItem
{
    public DesiredItem(ModItem modItem, int numberOfItemsToSellToPlayer, int numberOfItemsToPurchaseFromPlayer)
    {
        ModItem = modItem;
        NumberOfItemsToSellToPlayer = numberOfItemsToSellToPlayer;
        NumberOfItemsToPurchaseFromPlayer = numberOfItemsToPurchaseFromPlayer;
    }

    public ModItem ModItem { get; set; }
    public int NumberOfItemsToSellToPlayer { get; set; } = -1;
    public int NumberOfItemsToPurchaseFromPlayer { get; set; } = -1;
    public int ItemsSoldToPlayer { get; set; } = 0;
    public int ItemsPurchasedFromPlayer { get; set; } = 0;
}

