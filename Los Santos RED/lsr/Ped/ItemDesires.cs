using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ItemDesires
{
    public List<DesiredItem> DesiredItems { get; set; } = new List<DesiredItem>();
    public ItemDesires()
    {

    }
    public void AddDesiredItem(ShopMenu shopMenu, bool matchWithMenu)
    {
        DesiredItems.Clear();
        foreach (MenuItem menuItem in shopMenu.Items)
        {
            if (matchWithMenu)
            {
                DesiredItems.Add(new DesiredItem(menuItem.ModItem, menuItem.NumberOfItemsToSellToPlayer, menuItem.NumberOfItemsToPurchaseFromPlayer)
                {
                    ItemsPurchasedFromPlayer = menuItem.NumberOfItemsPurchasedByPlayer,
                    ItemsSoldToPlayer = menuItem.NumberOfItemsSoldToPlayer
                }
                );
            }
            else
            {
                DesiredItems.Add(new DesiredItem(menuItem.ModItem, menuItem.NumberOfItemsToSellToPlayer, menuItem.NumberOfItemsToPurchaseFromPlayer));
            }
            //DesiredItems.Add(new DesiredItem(menuItem.ModItem, menuItem.NumberOfItemsToSellToPlayer, menuItem.NumberOfItemsToPurchaseFromPlayer));
        }
    }

    public void OnItemsSoldToPlayer(ModItem modItem, int amount)
    {
        DesiredItem di = Get(modItem);
        if (di == null)
        {
            return;
        }
        di.ItemsSoldToPlayer += amount;
    }
    public void OnItemsBoughtFromPlayer(ModItem modItem, int amount)
    {
        DesiredItem di = Get(modItem);
        if (di == null)
        {
            return;
        }
        di.ItemsPurchasedFromPlayer += amount;
    }
    public DesiredItem Get(ModItem modItem)
    {
        if (modItem == null)
        {
            return null;
        }
        DesiredItem di = DesiredItems.Where(x => x.ModItem.Name == modItem.Name).FirstOrDefault();
        return di;
    }

}

