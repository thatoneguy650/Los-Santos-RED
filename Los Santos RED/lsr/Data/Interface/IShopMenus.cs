using Rage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosSantosRED.lsr.Interface
{
    public interface IShopMenus
    {
        ShopMenuGroupContainer GetSpecificGroupContainer(string containerID);
        ShopMenu GetSpecificMenu(string menuID);
        ShopMenu GetVendingMenu(string modelName);
        ShopMenu GetWeightedRandomMenuFromGroup(string groupID);
        Tuple<int, int> GetPrices(string name);
       // ShopMenu GetWeightedRandomMenuFromGroupByCategory(string drugDealerMenuID);
        ShopMenu GetRandomDrugCustomerMenu();
        ShopMenu GetWeightedRandomMenuFromContainer(string lookingForID);
        ShopMenuGroup GetSpecificMenuGroup(string shopMenuGroupID);
        List<MedicalTreatment> GetMedicalTreatments(string treatmentOptionsID);
        PedVariationShopMenu GetPedVariationMenu(string pedVariationShopMenuID);
    }
}
