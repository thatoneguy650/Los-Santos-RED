//using LosSantosRED.lsr.Data;
//using LosSantosRED.lsr.Helper;
//using LosSantosRED.lsr.Interface;
//using Rage;
//using RAGENativeUI.Elements;
//using RAGENativeUI.PauseMenu;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;


//public class SaveGameTabOLD
//{
//    private ISaveable Player;
//    private IPlacesOfInterest PlacesOfInterest;
//    private IShopMenus ShopMenus;
//    private IModItems ModItems;
//    private IWeapons Weapons;
//    private IGangTerritories GangTerritories;
//    private IZones Zones;
//    private ITimeControllable Time;
//    private ISettingsProvideable Settings;
//    private IGameSaves GameSaves;
//    private IGangs Gangs;
//    private IPedSwap PedSwap;
//    private IEntityProvideable World;
//    private IInventoryable Inventoryable;
//    private ISaveable Saveable;
//    private BigMessageThread BigMessage;
//    private TabView TabView;
//    private List<TabItem> items;
//    private bool addedItems;
//    private TabItem saveCharacter;
//    private TabSubmenuItem myTab;

//    public SaveGameTabOLD(ISaveable player, IPlacesOfInterest placesOfInterest, IShopMenus shopMenus, IModItems modItems, IWeapons weapons, IGangTerritories gangTerritories, IZones zones, TabView tabView, ITimeControllable time, ISettingsProvideable settings, IGameSaves gameSaves, IGangs gangs, IPedSwap pedSwap,IInventoryable inventoryable, IEntityProvideable world,ISaveable saveable)
//    {
//        Player = player;
//        PlacesOfInterest = placesOfInterest;
//        ShopMenus = shopMenus;
//        ModItems = modItems;
//        Weapons = weapons;
//        GangTerritories = gangTerritories;
//        Zones = zones;
//        TabView = tabView;
//        Time = time;
//        Settings = settings;
//        GameSaves = gameSaves;
//        Gangs = gangs;
//        PedSwap = pedSwap;
//        World = world;
//        Inventoryable = inventoryable;
//        Saveable = saveable;

//        BigMessage = new BigMessageThread(true);

//    }
//    public void AddItems()
//    {
//        items = new List<TabItem>();
//        addedItems = true;

//        saveCharacter = new TabTextItem("Save Game", "Save Game", "Saves the Current Character");
//        saveCharacter.Activated += (s, e) =>
//        {
//            if (Settings.SettingsManager.UIGeneralSettings.ShowFullscreenWarnings && 1 == 0)
//            {
//                TabView.Visible = false;
//                PopUpWarning popUpWarning = new PopUpWarning("Save", "Are you sure you want to save", "", Player.ButtonPrompts, Settings);
//                popUpWarning.Setup();
//                popUpWarning.ShowAndWait();
//                if (popUpWarning.IsAccepted)
//                {
//                    Game.IsPaused = false;
//                    TabView.Visible = false;
//                    GameFiber.Sleep(500);
//                    GameSaves.SaveSamePlayer_Obsolete(Saveable, Weapons, Time, PlacesOfInterest, ModItems);
//                }
//                else
//                {
//                    TabView.Visible = true;
//                }
//            }
//            else
//            {
//                TabView.Visible = false;
//                Game.IsPaused = false;
//                GameSaves.SaveSamePlayer_Obsolete(Saveable, Weapons, Time, PlacesOfInterest, ModItems);
//            }
//        };
//        items.Add(saveCharacter);
//        foreach (GameSave gs in GameSaves.GameSaveList)
//        {
//            AddItem(gs);
//        }
//        if (addedItems)
//        {
//            myTab = new TabSubmenuItem("Save", items);
//            TabView.AddTab(myTab);
//        }
//    }
//    private void AddItem(GameSave gs)
//    {
//        TabMissionSelectItem MajorTabItem = gs.SaveTabInfo(Time, Gangs, Weapons, ModItems);
//        MajorTabItem.OnItemSelect += (selectedItem) =>
//        {
//            if (selectedItem != null && selectedItem.Name == "Load")
//            {
//                if (Settings.SettingsManager.UIGeneralSettings.ShowFullscreenWarnings && 1==0)
//                {
//                    TabView.Visible = false;
//                    PopUpWarning popUpWarning = new PopUpWarning("Load", "Are you sure you want to load this save", "", Player.ButtonPrompts, Settings);
//                    popUpWarning.Setup();
//                    popUpWarning.ShowAndWait();
//                    if (popUpWarning.IsAccepted)
//                    {
//                        Game.IsPaused = false;
//                        TabView.Visible = false;
//                        GameFiber.Sleep(500);
//                        GameSaves.Load(gs, Weapons, PedSwap, Inventoryable, Settings, World, Gangs, Time, PlacesOfInterest, ModItems);
//                    }
//                    else
//                    {
//                        TabView.Visible = true;
//                    }
//                }
//                else
//                {
//                    TabView.Visible = false;
//                    Game.IsPaused = false;
//                    GameSaves.Load(gs, Weapons, PedSwap, Inventoryable, Settings, World, Gangs, Time, PlacesOfInterest, ModItems);
//                }
//            }
//            if (selectedItem != null && selectedItem.Name == "Delete")
//            {
//                if (Settings.SettingsManager.UIGeneralSettings.ShowFullscreenWarnings && 1 == 0)
//                {
//                    TabView.Visible = false;
//                    PopUpWarning popUpWarning = new PopUpWarning("Delete", "Are you sure you want to delete this save", "", Player.ButtonPrompts, Settings);
//                    popUpWarning.Setup();
//                    popUpWarning.ShowAndWait();
//                    if (popUpWarning.IsAccepted)
//                    {
//                        Game.IsPaused = false;
//                        TabView.Visible = false;
//                        GameFiber.Sleep(500);
//                        GameSaves.DeleteSave(gs);
//                    }
//                    else
//                    {
//                        TabView.Visible = true;
//                    }
//                }
//                else
//                {
//                    TabView.Visible = false;
//                    Game.IsPaused = false;
//                    GameSaves.DeleteSave(gs);
//                }
//            }
//        };
//        items.Add(MajorTabItem);
//        addedItems = true;
//    }


//}