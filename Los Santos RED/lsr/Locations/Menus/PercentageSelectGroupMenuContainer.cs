using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;


public class PercentageSelectGroupMenuContainer
{
    public PercentageSelectGroupMenuContainer()
    {
    }

    public PercentageSelectGroupMenuContainer(string shopMenuGroupID, int selectChance)
    {
        ShopMenuGroupID = shopMenuGroupID;
        SelectChance = selectChance;
    }
    public string ShopMenuGroupID { get; set; }
    public int SelectChance { get; set; } = 0;
}

