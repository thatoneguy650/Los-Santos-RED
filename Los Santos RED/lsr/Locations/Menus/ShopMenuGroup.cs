using ExtensionsMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ShopMenuGroup
{
    public ShopMenuGroup()
    {

    }

    public ShopMenuGroup(string iD, string name, string categoryID, List<PercentageSelectShopMenu> possibleShopMenus)
    {
        ID = iD;
        Name = name;
        PossibleShopMenus = possibleShopMenus;
        CategoryID = categoryID;
    }
    public string ID { get; set; }  
    public string Name { get; set; }
    public string CategoryID { get; set; }
    public List<PercentageSelectShopMenu> PossibleShopMenus { get; set; }   
    public ShopMenu GetRandomMenu()
    {
        if (PossibleShopMenus == null || !PossibleShopMenus.Any())
        {
            return null;
        }

        List<PercentageSelectShopMenu> ToPickFrom = PossibleShopMenus.ToList();
        int Total = ToPickFrom.Sum(x => x.SelectChance);
        int RandomPick = RandomItems.MyRand.Next(0, Total);
        foreach (PercentageSelectShopMenu dispatchableSHopMenu in ToPickFrom)
        {
            int SpawnChance = dispatchableSHopMenu.SelectChance;
            if (RandomPick < SpawnChance)
            {
                return dispatchableSHopMenu.ShopMenu;
            }
            RandomPick -= SpawnChance;
        }
        if (ToPickFrom.Any())
        {
            return ToPickFrom.PickRandom()?.ShopMenu;
        }
        return null;
    }

}

