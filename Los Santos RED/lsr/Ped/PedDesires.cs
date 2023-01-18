using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PedDesires
{
    private PedExt PedExt;
    private ISettingsProvideable Settings;

    public List<DesiredItem> DesiredItems { get; set; } = new List<DesiredItem>();
    public PedDesires(PedExt pedExt, ISettingsProvideable settings)
    {
        PedExt = pedExt;
        Settings = settings;
    }
    public void AddDesiredItem(ShopMenu shopMenu)
    {
        DesiredItems.Clear();
        foreach (MenuItem menuItem in shopMenu.Items)
        {
            DesiredItems.Add(new DesiredItem(menuItem.ModItem, menuItem.NumberOfItemsToSellToPlayer, menuItem.NumberOfItemsToPurchaseFromPlayer));
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
        di.ItemsBoughtFromPlayer += amount;
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

