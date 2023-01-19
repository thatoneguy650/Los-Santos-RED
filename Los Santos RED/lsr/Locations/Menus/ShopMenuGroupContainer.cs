using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
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

    public bool HasItem(ModItem modItem, IShopMenus shopMenus)
    {
        if(PercentageSelectGropMenuGroups == null || modItem == null)
        {
            return false;
        }
        foreach(PercentageSelectGroupMenuContainer percentageSelectGroupMenuContainer in PercentageSelectGropMenuGroups.Where(x=> x.SelectChance >= 80f))
        {
            //EntryPoint.WriteToConsole($"    HasItem {percentageSelectGroupMenuContainer.ShopMenuGroupID}");
            ShopMenuGroup smg = shopMenus.GetSpecificMenuGroup(percentageSelectGroupMenuContainer.ShopMenuGroupID);
            if(smg == null)
            {
                continue;
            }
            //EntryPoint.WriteToConsole($"        ShopMenuGroup smg.ID {smg.ID}");
            foreach (PercentageSelectShopMenu percentageSelectShopMenu in smg.PossibleShopMenus)
            {
                //EntryPoint.WriteToConsole($"            HasItem3 {percentageSelectShopMenu.ShopMenu?.Name}");
                if (percentageSelectShopMenu.ShopMenu?.Items?.Any(x => x.ModItem?.Name == modItem.Name) == true)
                {
                    return true;
                }
            }
        }
        return false;
    }


}

