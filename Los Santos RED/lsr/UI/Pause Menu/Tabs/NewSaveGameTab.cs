using LosSantosRED.lsr.Data;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI.Elements;
using RAGENativeUI.PauseMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class NewSaveGameTab
{
    private ISaveable Player;
    private IPlacesOfInterest PlacesOfInterest;
    private IShopMenus ShopMenus;
    private IModItems ModItems;
    private IWeapons Weapons;
    private IGangTerritories GangTerritories;
    private IZones Zones;
    private ITimeControllable Time;
    private ISettingsProvideable Settings;
    private IGameSaves GameSaves;
    private IGangs Gangs;
    private IPedSwap PedSwap;
    private IEntityProvideable World;
    private IInventoryable Inventoryable;
    private ISaveable Saveable;
    private BigMessageThread BigMessage;
    private TabView TabView;
    private List<TabItem> items;
    private bool addedItems;
    private TabItem saveCharacter;
    private TabSubmenuItem myTab;

    public NewSaveGameTab(ISaveable player, IPlacesOfInterest placesOfInterest, IShopMenus shopMenus, IModItems modItems, IWeapons weapons, IGangTerritories gangTerritories, IZones zones, TabView tabView, ITimeControllable time, ISettingsProvideable settings, IGameSaves gameSaves, IGangs gangs, IPedSwap pedSwap, IInventoryable inventoryable, IEntityProvideable world, ISaveable saveable)
    {
        Player = player;
        PlacesOfInterest = placesOfInterest;
        ShopMenus = shopMenus;
        ModItems = modItems;
        Weapons = weapons;
        GangTerritories = gangTerritories;
        Zones = zones;
        TabView = tabView;
        Time = time;
        Settings = settings;
        GameSaves = gameSaves;
        Gangs = gangs;
        PedSwap = pedSwap;
        World = world;
        Inventoryable = inventoryable;
        Saveable = saveable;
        BigMessage = new BigMessageThread(true);
    }
    public void AddSaveItems()
    {
        List<UIMenuItem> saveListItems = new List<UIMenuItem>();
        UIMenuItem createNew = new UIMenuItem($"Create New Save {GameSaves.GameSaveList.Count()}/15", "");
        createNew.Activated += (s, e) =>
        {
            TabView.Visible = false;
            Game.IsPaused = false;
            GameSaves.Save(Saveable, Weapons, Time, PlacesOfInterest, ModItems);
        };
        saveListItems.Add(createNew);

        int SaveNumber = 1;
        foreach (GameSave gs in GameSaves.GameSaveList.OrderBy(x => x.SaveDateTime))
        {
            UIMenuItem saveItem = new UIMenuItem($"{SaveNumber.ToString("D2")} - {gs.Title}", "") { RightLabel = gs.SaveDateTime.ToString("MM/dd/yyyy HH:mm") };
            saveItem.Activated += (s, e) =>
            {
                TabView.Visible = false;
                Game.IsPaused = false;
                GameSaves.DeleteSave(gs);
                GameSaves.Save(Saveable, Weapons, Time, PlacesOfInterest, ModItems);
            };
            saveListItems.Add(saveItem);
            SaveNumber++;
        }

        TabInteractiveListItem interactiveListItem2 = new TabInteractiveListItem("SAVE", saveListItems);
        TabView.AddTab(interactiveListItem2);
    }
    public void AddLoadItems()
    {
        List<UIMenuItem> saveListItems = new List<UIMenuItem>();
        UIMenuItem createNew = new UIMenuItem($"Number of Save Games: {GameSaves.GameSaveList.Count()}", "") { Enabled = false};
        saveListItems.Add(createNew);
        int SaveNumber = 1;
        foreach (GameSave gs in GameSaves.GameSaveList.OrderBy(x=> x.SaveDateTime))
        {
            UIMenuItem saveItem = new UIMenuItem($"{SaveNumber.ToString("D2")} - {gs.Title}", "") { RightLabel = gs.SaveDateTime.ToString("MM/dd/yyyy HH:mm") };
            saveItem.Activated += (s, e) =>
            {
                TabView.Visible = false;
                Game.IsPaused = false;
                GameSaves.Load(gs, Weapons, PedSwap, Inventoryable, Settings, World, Gangs, Time, PlacesOfInterest, ModItems);
            };
            saveListItems.Add(saveItem);
            SaveNumber++;
        }
        TabInteractiveListItem interactiveListItem2 = new TabInteractiveListItem("LOAD", saveListItems);
        TabView.AddTab(interactiveListItem2);
    }
}