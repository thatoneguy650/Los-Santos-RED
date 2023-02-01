using LosSantosRED.lsr.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


public class PedKnowledge
{
    private PedExt PedExt;
    private ISettingsProvideable Settings;
    private List<ModItem> KnownItemLocations = new List<ModItem>();
    private List<Gang> KnownGangLocations = new List<Gang>();
    private int ItemKnowPercentage = 45;
    private int GangKnowPercent = 45;
    public PedKnowledge(PedExt pedExt, ISettingsProvideable settings)
    {
        PedExt = pedExt;
        Settings = settings;
    }
    public void Setup(IGangs gangs, IModItems modItems)
    {
        AddGangKnowledge(gangs);
        AddItemKnowledge(modItems);
    }
    public bool KnownsAboutItem(ModItem modItem)
    {
        return KnownItemLocations.Contains(modItem);
    }
    public bool KnownsAboutGang(Gang gang)
    {
        return KnownGangLocations.Contains(gang);
    }




    private void AddGangKnowledge(IGangs gangs)
    {
        AddGeneralGangKnowledge(gangs);
    }
    private void AddGeneralGangKnowledge(IGangs gangs)
    {
        foreach (Gang gang in gangs.AllGangs)
        {
            //if (RandomItems.RandomPercent(PedExt.KnowsAboutRandomGangPercentage))
            //{
            //    if (!KnownGangLocations.Contains(gang))
            //    {
            //        KnownGangLocations.Add(gang);
            //    }
            //}
        }
    }
    private void AddItemKnowledge(IModItems modItems)
    {
        AddOwnedItemKnowledge();
        AddGeneralItemKnowledge(modItems);
    }
    private void AddGeneralItemKnowledge(IModItems modItems)
    {
        foreach (ModItem modItem in modItems.AllItems().Where(x => x.ItemType == ItemType.Drugs && x.ItemSubType == ItemSubType.Narcotic).ToList())
        {
            //if(RandomItems.RandomPercent(PedExt.KnowsAboutRandomDrugsPercentage))
            //{
            //    if (!KnownItemLocations.Contains(modItem))
            //    {
            //        KnownItemLocations.Add(modItem);
            //    }
            //}
        }
    }
    private void AddOwnedItemKnowledge()
    {
        if(PedExt.ShopMenu == null || PedExt.ShopMenu.Items == null)
        {
            return;
        }
        foreach (MenuItem menuItem in PedExt.ShopMenu.Items)
        {
            if (!KnownItemLocations.Contains(menuItem.ModItem))
            {
                KnownItemLocations.Add(menuItem.ModItem);
            }
        }
    }
}

