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

    public ShopMenuGroup(string iD, string name, string categoryID, List<DispatchableShopMenu> dispatchableShopMenus)
    {
        ID = iD;
        Name = name;
        DispatchableShopMenus = dispatchableShopMenus;
        CategoryID = categoryID;
    }
    public string ID { get; set; }  
    public string Name { get; set; }
    public string CategoryID { get; set; }
    public List<DispatchableShopMenu> DispatchableShopMenus { get; set; }   
    public ShopMenu GetRandomMenu()// List<string> RequiredModels)
    {
        if (DispatchableShopMenus == null || !DispatchableShopMenus.Any())
            return null;

        List<DispatchableShopMenu> ToPickFrom = DispatchableShopMenus.ToList();//.Where(x => wantedLevel >= x.MinWantedLevelSpawn && wantedLevel <= x.MaxWantedLevelSpawn).ToList();
        //if (unUsed != "" && !string.IsNullOrEmpty(unUsed))
        //{
        //    //ToPickFrom = ToPickFrom.Where(x => x.GroupName == RequiredPedGroup).ToList();
        //}
        int Total = ToPickFrom.Sum(x => x.SelectChance);
        int RandomPick = RandomItems.MyRand.Next(0, Total);
        foreach (DispatchableShopMenu dispatchableSHopMenu in ToPickFrom)
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

