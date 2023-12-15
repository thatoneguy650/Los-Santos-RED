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


public class SaveGameTab
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
    private IContacts Contacts;
    private IPedSwap PedSwap;
    private IAgencies Agencies;
    private IEntityProvideable World;
    private IInventoryable Inventoryable;
    private ISaveable Saveable;
    private IInteractionable Interactionable;
    private TabView TabView;
    private List<TabItem> items;
    private bool addedItems;
    private TabItem saveCharacter;
    private TabSubmenuItem myTab;

    public SaveGameTab(ISaveable player, IPlacesOfInterest placesOfInterest, IShopMenus shopMenus, IModItems modItems, IWeapons weapons, IGangTerritories gangTerritories, IZones zones, TabView tabView, ITimeControllable time, ISettingsProvideable settings, IGameSaves gameSaves,
        IGangs gangs, IPedSwap pedSwap, IInventoryable inventoryable, IEntityProvideable world, ISaveable saveable, IAgencies agencies, IContacts contacts, IInteractionable interactionable)
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
        Agencies = agencies;
        Contacts = contacts;
        Interactionable = interactionable;
    }
    public void AddSaveItems()
    {
        List<UIMenuItem> saveListItems = new List<UIMenuItem>();
        UIMenuItem createNew = new UIMenuItem($"Create New Save {GameSaves.GameSaveList.Count()}/99", "");
        createNew.Activated += (s, e) =>
        {
            SimpleWarning popUpWarning = new SimpleWarning("Save", "Are you sure you want to create a new save", "", Player.ButtonPrompts, Settings);
            popUpWarning.Show();
            if (popUpWarning.IsAccepted)
            {
                TabView.Visible = false;
                Game.IsPaused = false;
                GameSaves.Save(Saveable, Weapons, Time, PlacesOfInterest, ModItems, GameSaves.NextSaveGameNumber);
            }
        };
        saveListItems.Add(createNew);
        if (GameSaves.GameSaveList != null && GameSaves.GameSaveList.Any())
        {
            int maxNumber = GameSaves.GameSaveList.Max(x => x.SaveNumber);
            for (int i = 1; i <= maxNumber; i++)
            {
                UIMenuItem saveItem;
                GameSave gs = GameSaves.GameSaveList.FirstOrDefault(x => x.SaveNumber == i);
                if (gs != null)
                {
                    saveItem = new UIMenuItem(gs.Title, "") { RightLabel = gs.RightLabel };
                    if (GameSaves.IsPlaying(gs)) { saveItem.Text += " - Active"; saveItem.BackColor = EntryPoint.LSRedColor; }
                    saveItem.Activated += (s, e) =>
                    {
                        SimpleWarning popUpWarning = new SimpleWarning("Save", "Are you sure you want to overwrite this save", "", Player.ButtonPrompts, Settings);
                        popUpWarning.Show();
                        if (popUpWarning.IsAccepted)
                        {
                            TabView.Visible = false;
                            Game.IsPaused = false;
                            int saveNumber = gs.SaveNumber;
                            GameSaves.DeleteSave(gs);
                            GameSaves.Save(Saveable, Weapons, Time, PlacesOfInterest, ModItems, saveNumber);
                        }
                    };
                }
                else
                {
                    saveItem = new UIMenuItem($"{i.ToString("D2")} - New Save Game", "");
                    saveItem.Activated += (s, e) =>
                    {
                        int index = saveItem.Text.IndexOf(" -");
                        string intAsString = (index > 0 ? saveItem.Text.Substring(0, index) : "");//why is this needed, is the i used as ref or something?
                        int result = 0;
                        int number = i;
                        if (int.TryParse(intAsString, out result))
                        {
                            number = result;
                        }
                        SimpleWarning popUpWarning = new SimpleWarning("Save", "Are you sure you want to create a new save", "", Player.ButtonPrompts, Settings);
                        popUpWarning.Show();
                        if (popUpWarning.IsAccepted)
                        {
                            TabView.Visible = false;
                            Game.IsPaused = false;
                            GameSaves.Save(Saveable, Weapons, Time, PlacesOfInterest, ModItems, result);
                        }
                    };
                }
                saveListItems.Add(saveItem);
            }
        }
        TabInteractiveListItem interactiveListItem2 = new TabInteractiveListItem("SAVE", saveListItems);
        TabView.AddTab(interactiveListItem2);
    }
    public void AddLoadItems()
    {
        List<UIMenuItem> saveListItems = new List<UIMenuItem>();
        UIMenuItem saveCount = new UIMenuItem($"Number of Save Games: {GameSaves.GameSaveList.Count()}", "") { Enabled = false};
        saveListItems.Add(saveCount);
        if (GameSaves.GameSaveList != null && GameSaves.GameSaveList.Any())
        {
            int maxNumber = GameSaves.GameSaveList.Max(x => x.SaveNumber);
            for (int i = 1; i <= maxNumber; i++)
            {
                UIMenuItem loadItem;
                GameSave gs = GameSaves.GameSaveList.FirstOrDefault(x => x.SaveNumber == i);
                if (gs != null)
                {
                    loadItem = new UIMenuItem(gs.Title, "") { RightLabel = gs.RightLabel };
                    if (GameSaves.IsPlaying(gs)) { loadItem.Text += " - Active"; loadItem.BackColor = EntryPoint.LSRedColor; }
                    loadItem.Activated += (s, e) =>
                    {
                        SimpleWarning popUpWarning = new SimpleWarning("Load", "Are you sure you want to load this save", "", Player.ButtonPrompts, Settings);
                        popUpWarning.Show();
                        if (popUpWarning.IsAccepted)
                        {
                            TabView.Visible = false;
                            Game.IsPaused = false;
                            GameFiber.Sleep(500);
                            GameSaves.Load(gs, Weapons, PedSwap, Inventoryable, Settings, World, Gangs, Time, PlacesOfInterest, ModItems, Agencies, Contacts, Interactionable);
                        }
                    };
                }
                else
                {
                    loadItem = new UIMenuItem($"{i.ToString("D2")} - Empty Save", "") { Enabled = false };
                }
                saveListItems.Add(loadItem);
            }
        }
        TabInteractiveListItem interactiveListItem2 = new TabInteractiveListItem("LOAD", saveListItems);
        TabView.AddTab(interactiveListItem2);
    }

    internal void AddDeleteItems()
    {
        List<UIMenuItem> saveListItems = new List<UIMenuItem>();
        UIMenuItem createNew = new UIMenuItem($"Number of Save Games: {GameSaves.GameSaveList.Count()}", "") { Enabled = false };
        saveListItems.Add(createNew);
        if (GameSaves.GameSaveList != null && GameSaves.GameSaveList.Any())
        {
            int maxNumber = GameSaves.GameSaveList.Max(x => x.SaveNumber);
            for (int i = 1; i <= maxNumber; i++)
            {
                UIMenuItem deleteItem;
                GameSave gs = GameSaves.GameSaveList.FirstOrDefault(x => x.SaveNumber == i);
                if (gs != null)
                {
                    deleteItem = new UIMenuItem(gs.Title, "") { RightLabel = gs.RightLabel };
                    if (GameSaves.IsPlaying(gs)) { deleteItem.Text += " - Active"; deleteItem.BackColor = EntryPoint.LSRedColor; }
                    deleteItem.Activated += (s, e) =>
                    {
                        SimpleWarning popUpWarning = new SimpleWarning("Delete", "Are you sure you want to delete this save", "", Player.ButtonPrompts, Settings);
                        popUpWarning.Show();
                        if (popUpWarning.IsAccepted)
                        {
                            TabView.Visible = false;
                            Game.IsPaused = false;
                            GameSaves.DeleteSave(gs);
                        }
                    };
                }
                else
                {
                    deleteItem = new UIMenuItem($"{i.ToString("D2")} - Empty Save", "") { Enabled = false };
                }
                saveListItems.Add(deleteItem);
            }
        }
        TabInteractiveListItem interactiveListItem2 = new TabInteractiveListItem("DELETE", saveListItems);
        TabView.AddTab(interactiveListItem2);
    }
}