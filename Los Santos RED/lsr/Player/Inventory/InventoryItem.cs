using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

[Serializable()]
public class InventoryItem
{
    private ISettingsProvideable Settings;
    public InventoryItem(ModItem modItem, ISettingsProvideable settings)
    {
        ModItem = modItem;
        Settings = settings;
        //ItemName = modItem?.Name;
    }
    public InventoryItem()
    {

    }
    public string Description => ModItem.FullDescription(Settings) + $"~n~Amount: ~b~{Amount}~s~" + (ModItem.PercentLostOnUse > 0.0f ? $" (~b~{Math.Round(100f * RemainingPercent, 0)}%~s~)" : "");
    public string RightLabel => $"{Amount}~s~" + (ModItem.MeasurementName != "Item" ? " " + ModItem.MeasurementName + "(s)" : "");
   // public string ItemName { get; set; }

    [XmlIgnore]
    //[XML]
    public ModItem ModItem { get; set; }


    public int Amount => (int)Math.Ceiling(RemainingPercent);
    public float RemainingPercent { get; set; }
    public void AddAmount(int toadd)
    {
        RemainingPercent += toadd;
    }
    public bool RemovePercent(float percentToRemove)
    {
        if(RemainingPercent >= percentToRemove)
        {
            RemainingPercent -= percentToRemove;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AddToInteractionMenu(IInteractionable player, UIMenu useItemsMenu, List<UIMenu> uIMenus)
    {
        if(ModItem == null)
        { 
            return;
        }
        UIMenuItem uiMenuItem = new UIMenuItem(ModItem.Name, Description) { RightLabel = RightLabel };
        uiMenuItem.Activated += (sender, e) =>
        {
            player.ActivityManager.UseInventoryItem(ModItem, false);
            uiMenuItem.RightLabel = RightLabel;
            //if(!player.Inventory.ItemsList.Contains(this))
            //{
            //    uiMenuItem.Enabled = false;
            //    uiMenuItem.RightLabel = "0";
            //}
            if (!player.Inventory.ItemsList.Where(x => x.ModItem.Name == ModItem.Name).Any())
            {
                uiMenuItem.Enabled = false;
                uiMenuItem.RightLabel = "0";
            }

            uiMenuItem.Enabled = Amount > 0;
            uiMenuItem.Description = Description;
            Game.DisplaySubtitle($"Used Item {ModItem.Name}");
        };
        uiMenuItem.Enabled = Amount > 0;

        UIMenu toStoreMenu = useItemsMenu;
        if (uIMenus != null && uIMenus.Any())
        {
            UIMenu categoryMenu = uIMenus.FirstOrDefault(x => x.SubtitleText == ModItem.MenuCategory || x.TitleText == ModItem.MenuCategory);
            if(categoryMenu != null)
            {
                toStoreMenu = categoryMenu;
            }
        }

        toStoreMenu.AddItem(uiMenuItem);

    }
}

