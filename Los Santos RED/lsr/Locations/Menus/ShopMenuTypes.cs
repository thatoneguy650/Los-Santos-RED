using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ShopMenuTypes
{
    public ShopMenuTypes()
    {
    }
    public List<ShopMenu> ShopMenuList { get; private set; } = new List<ShopMenu>();
    public List<ShopMenuGroup> ShopMenuGroupList { get; private set; } = new List<ShopMenuGroup>();
    public List<ShopMenuGroupContainer> ShopMenuGroupContainers { get; private set; } = new List<ShopMenuGroupContainer>();
    public List<PropShopMenu> PropShopMenus { get; private set; } = new List<PropShopMenu>();
    public List<TreatmentOptions> TreatmentOptionsList { get; private set; } = new List<TreatmentOptions>();
    public List<PedVariationShopMenu> PedVariationShopMenus { get; private set; } = new List<PedVariationShopMenu>();
}

