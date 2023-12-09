using LosSantosRED.lsr.Data;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Mod;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class CustomizeExistingVariationsMenu
{
    private IPedSwap PedSwap;
    private MenuPool MenuPool;
    private INameProvideable Names;
    private IPedSwappable Player;
    private IEntityProvideable World;
    private ISettingsProvideable Settings;
    private PedCustomizer PedCustomizer;
    private UIMenuItem InputModel;
    private UIMenuListScrollerItem<string> SelectModel;
    private UIMenuItem SearchModel;
    private PedCustomizerMenu PedCustomizerMenu;
    private UIMenu ModelSearchSubMenu;
    private string FilterString;
    private IDispatchablePeople DispatchablePeople;
    private IHeads Heads;
    private UIMenu savedVariationsMenu;
    private IGameSaves GameSaves;
    private ISavedOutfits SavedOutfits;
    private UIMenu outfitsSubMenu;
    //private UIMenu modeloutfitsSubMenu;
    private bool IncludeAllOutfits = false;

    public CustomizeExistingVariationsMenu(MenuPool menuPool, IPedSwap pedSwap, INameProvideable names, IPedSwappable player, IEntityProvideable world, ISettingsProvideable settings, PedCustomizer pedCustomizer, PedCustomizerMenu pedCustomizerMenu, 
        IDispatchablePeople dispatchablePeople, IHeads heads, IGameSaves gameSaves, ISavedOutfits savedOutfits)
    {
        PedSwap = pedSwap;
        MenuPool = menuPool;
        Names = names;
        Player = player;
        World = world;
        Settings = settings;
        PedCustomizer = pedCustomizer;
        PedCustomizerMenu = pedCustomizerMenu;
        DispatchablePeople = dispatchablePeople;
        Heads = heads;
        GameSaves = gameSaves;
        SavedOutfits = savedOutfits;
    }
    public void Setup(UIMenu CustomizeMainMenu)
    {
        savedVariationsMenu = MenuPool.AddSubMenu(CustomizeMainMenu, "Saved Variations");
        CustomizeMainMenu.MenuItems[CustomizeMainMenu.MenuItems.Count() - 1].Description = "Choose from a list of saved variations";
        //CustomizeMainMenu.MenuItems[CustomizeMainMenu.MenuItems.Count() - 1].RightBadge = UIMenuItem.BadgeStyle.Clothes;
        savedVariationsMenu.SetBannerType(EntryPoint.LSRedColor);
        savedVariationsMenu.InstructionalButtonsEnabled = false;
        savedVariationsMenu.OnMenuOpen += (sender) =>
        {
            PedCustomizer.CameraCycler.SetDefault();
        };
        savedVariationsMenu.OnMenuClose += (sender) =>
        {
            PedCustomizer.CameraCycler.SetDefault();
        };  
        AddOutfits();
        AddSaveGames();
        AddDispatchablePeople();
    }
    public void OnModelChanged()
    {
        SetOutfits();
    }
    private void AddOutfits()
    {
        outfitsSubMenu = MenuPool.AddSubMenu(savedVariationsMenu, "Outfits");
        savedVariationsMenu.MenuItems[savedVariationsMenu.MenuItems.Count() - 1].Description = "Choose a new variation for the current model from a saved outfit. Will keep the current character.";
        outfitsSubMenu.SetBannerType(EntryPoint.LSRedColor);
        outfitsSubMenu.InstructionalButtonsEnabled = false;
        SetOutfits();
    }
    private void SetOutfits()
    {
        outfitsSubMenu.Clear();
        UIMenuCheckboxItem includeAllOutfits = new UIMenuCheckboxItem("Include All", IncludeAllOutfits, "If enabled, you will see all outfits for the given model, if disabled you will only see outfits for the current character.");
        includeAllOutfits.Activated += (sender, e) =>
        {
            IncludeAllOutfits = !IncludeAllOutfits;
            SetOutfits();
            outfitsSubMenu.RefreshIndex();
        };
        outfitsSubMenu.AddItem(includeAllOutfits);
        SetByModel();
    }
    private void SetByModel()
    {

        UIMenuItem saveOutfitMenuItem = new UIMenuItem("Save Outfit");
        saveOutfitMenuItem.Activated += (sender, e) =>
        {
            string outfitName = NativeHelper.GetKeyboardInput("");
            if (string.IsNullOrEmpty(outfitName) || outfitName == "")
            {
                Game.DisplaySubtitle("No Outfit Name Set");
                return;
            }
            SavedOutfits.AddOutfit(new SavedOutfit(outfitName, PedCustomizer.WorkingModelName, PedCustomizer.WorkingName, PedCustomizer.WorkingVariation.Copy()));
            SetOutfits();
            outfitsSubMenu.RefreshIndex();
        };
        outfitsSubMenu.AddItem(saveOutfitMenuItem);
        foreach (SavedOutfit so in SavedOutfits.SavedOutfitList.Where(x => x.ModelName.ToLower() == PedCustomizer.WorkingModelName.ToLower() && (string.IsNullOrEmpty(x.CharacterName) || IncludeAllOutfits || x.CharacterName.ToLower() == PedCustomizer.WorkingName.ToLower())))
        {
            //EntryPoint.WriteToConsoleTestLong($"OUTFIT MANAGER:     ADDING OUTFIT {so.Name}");
            UIMenuListScrollerItem<string> uIMenuItem = new UIMenuListScrollerItem<string>(so.Name, so.CharacterName, new List<string>() { "Set", "Delete" });
            uIMenuItem.Activated += (sender, e) =>
            {
                if (uIMenuItem.SelectedItem == "Set")
                {
                    if (so.PedVariation == null)
                    {
                        Game.DisplaySubtitle("No Variation to Set");
                        return;
                    }
                    PedVariation newVariation = so.PedVariation.Copy();
                    PedCustomizer.WorkingVariation = newVariation;
                    PedCustomizer.InitialVariation = newVariation.Copy();
                    PedCustomizer.OnVariationChanged();
                    Game.DisplaySubtitle($"Applied Outfit {so.Name}");
                }
                else if (uIMenuItem.SelectedItem == "Delete")
                {
                    SavedOutfits.RemoveOutfit(so);
                    SetOutfits();
                    outfitsSubMenu.RefreshIndex();
                }
            };
            outfitsSubMenu.AddItem(uIMenuItem);
        }
    }
   
    private void AddSaveGames()
    {
        UIMenu dispatchablePeopleSubMenu = MenuPool.AddSubMenu(savedVariationsMenu, "Save Games");
        savedVariationsMenu.MenuItems[savedVariationsMenu.MenuItems.Count() - 1].Description = "Choose a new model and variation from one of the save games. Will reset the current character.";
        dispatchablePeopleSubMenu.SetBannerType(EntryPoint.LSRedColor);
        dispatchablePeopleSubMenu.InstructionalButtonsEnabled = false;
        foreach (GameSave gs in GameSaves.GameSaveList)
        {
            UIMenuItem uIMenuItem = new UIMenuItem(gs.Title);
            uIMenuItem.Activated += (sender, e) =>
            {
                PedCustomizer.WorkingModelName = gs.ModelName;
                if (gs.CurrentModelVariation == null)
                {
                    PedCustomizer.WorkingVariation = new PedVariation();
                    PedCustomizer.InitialVariation = new PedVariation();
                }
                else
                {
                    PedVariation newVariation = gs.CurrentModelVariation.Copy();
                    PedCustomizer.WorkingVariation = newVariation;
                    PedCustomizer.InitialVariation = newVariation.Copy();
                }
                PedCustomizer.OnModelChanged(false);
            };
            dispatchablePeopleSubMenu.AddItem(uIMenuItem);
        }
    }
    private void AddDispatchablePeople()
    {
        UIMenu dispatchablePeopleSubMenu = MenuPool.AddSubMenu(savedVariationsMenu, "Dispatchable People");
        savedVariationsMenu.MenuItems[savedVariationsMenu.MenuItems.Count() - 1].Description = "Choose a new model and variation from one of the dispatched people.Will reset the current character.";
        dispatchablePeopleSubMenu.SetBannerType(EntryPoint.LSRedColor);
        dispatchablePeopleSubMenu.InstructionalButtonsEnabled = false;
        foreach (DispatchablePersonGroup dpg in DispatchablePeople.AllPeople)
        {
            UIMenu dpgSubMenu = MenuPool.AddSubMenu(dispatchablePeopleSubMenu, dpg.DispatchablePersonGroupID);
            dpgSubMenu.SetBannerType(EntryPoint.LSRedColor);
            dpgSubMenu.InstructionalButtonsEnabled = false;
            foreach (DispatchablePerson dp in dpg.DispatchablePeople)
            {
                UIMenuItem uIMenuItem = new UIMenuItem(dp.DebugName + " - " + dp.ModelName, dp.DebugName);
                uIMenuItem.Activated += (sender, e) =>
                {
                    PedCustomizer.WorkingModelName = dp.ModelName;
                    if (dp.RequiredVariation == null)
                    {
                        PedCustomizer.WorkingVariation = new PedVariation();
                        PedCustomizer.InitialVariation = new PedVariation();
                    }
                    else
                    {
                        PedVariation newVariation = dp.RequiredVariation.Copy();
                        PedCustomizer.WorkingVariation = newVariation;
                        PedCustomizer.InitialVariation = newVariation.Copy();
                    }
                    PedCustomizer.OnModelChanged(false);
                };
                dpgSubMenu.AddItem(uIMenuItem);
            }
        }
    }
}

