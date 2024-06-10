using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.AxHost;

public class DebugInventorySubMenu : DebugSubMenu
{
    private ISettingsProvideable Settings;
    private ICrimes Crimes;
    private ITaskerable Tasker;
    private IEntityProvideable World;
    private IWeapons Weapons;
    private IModItems ModItems;
    private ITimeControllable Time;
    private IRadioStations RadioStations;
    private INameProvideable Names;
    public DebugInventorySubMenu(UIMenu debug, MenuPool menuPool, IActionable player, ISettingsProvideable settings, ICrimes crimes, ITaskerable tasker, IEntityProvideable world, IWeapons weapons, IModItems modItems, ITimeControllable time, IRadioStations radioStations, INameProvideable names) : base(debug, menuPool, player)
    {
        Settings = settings;
        Crimes = crimes;
        Tasker = tasker;
        World = world;
        Weapons = weapons;
        ModItems = modItems;
        Time = time;
        RadioStations = radioStations;
        Names = names;
    }
    public override void AddItems()
    {
        UIMenu MainInventoryItemsMenu = MenuPool.AddSubMenu(Debug, "Inventory Menu");
        MainInventoryItemsMenu.SetBannerType(EntryPoint.LSRedColor);
        Debug.MenuItems[Debug.MenuItems.Count() - 1].Description = "Change player inventory.";

        UIMenuItem removeAll = new UIMenuItem("Remove All", "Remove all items from inventory");
        removeAll.Activated += (menu, item) =>
        {
            Player.Inventory.Reset();
            menu.Visible = false;
        };
        MainInventoryItemsMenu.AddItem(removeAll);

        UIMenuItem GetAllItems = new UIMenuItem("Get All Items", "Gets 10 of every item");
        GetAllItems.Activated += (menu, item) =>
        {
            foreach (ModItem modItem in ModItems.InventoryItems())
            {
                if (!modItem.ConsumeOnPurchase)
                {
                    Player.Inventory.Add(modItem, 10);
                }
            }
            menu.Visible = false;
        };
        MainInventoryItemsMenu.AddItem(GetAllItems);

        UIMenuItem GetSomeItems = new UIMenuItem("Get Some Items", "Gets 10 of 30 random items");
        GetSomeItems.Activated += (menu, item) =>
        {
            foreach (ModItem modItem in ModItems.InventoryItems().OrderBy(x => RandomItems.MyRand.Next()).Take(30))
            {
                if (!modItem.ConsumeOnPurchase)
                {
                    Player.Inventory.Add(modItem, 10);
                }
            }
            menu.Visible = false;
        };
        MainInventoryItemsMenu.AddItem(GetSomeItems);




        UIMenu GetSpecificItemMenu = MenuPool.AddSubMenu(MainInventoryItemsMenu, "Specific Items");
        GetSpecificItemMenu.SetBannerType(EntryPoint.LSRedColor);
        MainInventoryItemsMenu.MenuItems[Debug.MenuItems.Count() - 1].Description = "Get specific items.";
        //List<string> itemTypes = Player.Inventory.ItemsList.Select(x=> x.ModItem != null && x.ModItem.ItemType)


        List<ItemType> ItemTypes = ModItems.InventoryItems().ToList().Select(x=> x.ItemType).Distinct().OrderBy(x=> x).ToList();
        List<UIMenu> itemTypeMenus = new List<UIMenu>();
        List<UIMenu> itemSubTypeMenus = new List<UIMenu>();
        foreach (ItemType itemtype in ItemTypes)
        {
            UIMenu itemTypeMenu = MenuPool.AddSubMenu(GetSpecificItemMenu, itemtype.ToString());
            itemTypeMenu.SetBannerType(EntryPoint.LSRedColor);
            itemTypeMenus.Add(itemTypeMenu);


            List<ItemSubType> ItemSubTypes = ModItems.InventoryItems().ToList().Where(x=> x.ItemType == itemtype).Select(x => x.ItemSubType).Distinct().OrderBy(x => x).ToList();
            foreach(ItemSubType itemSubType in ItemSubTypes)
            {
                UIMenu itemSubTypeMenu = MenuPool.AddSubMenu(itemTypeMenu, itemSubType.ToString());
                itemSubTypeMenu.SetBannerType(EntryPoint.LSRedColor);
                itemSubTypeMenus.Add(itemSubTypeMenu);

                List<ModItem> modItems = ModItems.InventoryItems().ToList().Where(x => x.ItemType == itemtype && x.ItemSubType == itemSubType).Distinct().ToList();
                foreach (ModItem modItem in modItems)
                {
                    UIMenuNumericScrollerItem<int> giveItem = new UIMenuNumericScrollerItem<int>(modItem.Name, modItem.FullDescription(Settings), 1, 100, 1) { Formatter = v => v.ToString() + " " + modItem.MeasurementName + (v > 1 ? "(s)" : "") };
                    giveItem.Value = 1;
                    giveItem.Activated += (menu, item) =>
                    {
                        if (modItem.ConsumeOnPurchase)
                        {
                            Player.Inventory.Use(modItem);
                            Game.DisplaySubtitle($"Used {modItem.Name}");
                        }
                        else
                        {
                            Player.Inventory.Add(modItem, giveItem.Value.Clamp(1, 100));
                            Game.DisplaySubtitle($"Given ({giveItem.Value.Clamp(1, 100)}) - {modItem.Name} ");
                        }
                        //menu.Visible = false;
                    };
                    itemSubTypeMenu.AddItem(giveItem);
                }


            }


        }




    }
}

