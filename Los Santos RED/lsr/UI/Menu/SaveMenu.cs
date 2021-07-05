using LosSantosRED.lsr.Data;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System.Collections.Generic;
using System.Drawing;

public class SaveMenu : Menu
{
    private UIMenu Saves;
    private UIMenuItem SaveGameItem;
    private UIMenuListScrollerItem<GameSave> GameSaveMenuList;
    private ISaveable Player;
    private IWeapons Weapons;
    private IGameSaves GameSaves;
    private IPedSwap PedSwap;
    public SaveMenu(MenuPool menuPool, UIMenu parentMenu, ISaveable player, IGameSaves gameSaves, IWeapons weapons, IPedSwap pedSwap)
    {
        Player = player;
        GameSaves = gameSaves;
        Weapons = weapons;
        PedSwap = pedSwap;
        Saves = menuPool.AddSubMenu(parentMenu, "Save/Load Player");
        CreateSavesMenu();
    }
    public override void Hide()
    {
        Saves.Visible = false;
    }
    public override void Show()
    {
        Update();
        Saves.Visible = true;
    }
    public override void Toggle()
    {
        if (!Saves.Visible)
        {
            Saves.Visible = true;
        }
        else
        {
            Saves.Visible = false;
        }
    }
    public void Update()
    {
        CreateSavesMenu();
    }
    private void CreateSavesMenu()
    {
        Saves.Clear();
        GameSaveMenuList = new UIMenuListScrollerItem<GameSave>("Load Player", "Load the selected player", GameSaves.GameSaveList);
        SaveGameItem = new UIMenuItem("Save Player", "Save current player");
        Saves.AddItem(GameSaveMenuList);
        Saves.AddItem(SaveGameItem);
        Saves.OnItemSelect += OnActionItemSelect;
    }
    private void OnActionItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (selectedItem == GameSaveMenuList)
        {
            GameSaves.Load(GameSaveMenuList.SelectedItem,Weapons,PedSwap);
        }
        else if (selectedItem == SaveGameItem)
        {
            GameSaves.Save(Player, Weapons);
        }
        Saves.Visible = false;
    }
}