using ExtensionsMethods;
using System.Collections.Generic;
using System.Linq;


public class ShopMenuGroupContainer
{
    public ShopMenuGroupContainer()
    {

    }

    public ShopMenuGroupContainer(string iD, string name, List<PercentageSelectGroupMenuContainer> possibleShopMenus)
    {
        ID = iD;
        Name = name;
        PercentageSelectGropMenuGroups = possibleShopMenus;
    }
    public string ID { get; set; }
    public string Name { get; set; }
    public List<PercentageSelectGroupMenuContainer> PercentageSelectGropMenuGroups { get; set; }
    public string GetRandomWeightedShopMenuGroupID()
    {
        if (PercentageSelectGropMenuGroups == null || !PercentageSelectGropMenuGroups.Any())
        {
            return null;
        }
        List<PercentageSelectGroupMenuContainer> ToPickFrom = PercentageSelectGropMenuGroups.ToList();
        int Total = ToPickFrom.Sum(x => x.SelectChance);
        int RandomPick = RandomItems.MyRand.Next(0, Total);
        foreach (PercentageSelectGroupMenuContainer dispatchableSHopMenu in ToPickFrom)
        {
            int SpawnChance = dispatchableSHopMenu.SelectChance;
            if (RandomPick < SpawnChance)
            {
                return dispatchableSHopMenu.ShopMenuGroupID;
            }
            RandomPick -= SpawnChance;
        }
        if (ToPickFrom.Any())
        {
            return ToPickFrom.PickRandom()?.ShopMenuGroupID;
        }
        return null;
    }

}

