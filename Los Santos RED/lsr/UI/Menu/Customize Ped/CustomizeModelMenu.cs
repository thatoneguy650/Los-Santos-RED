using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Mod;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class CustomizeModelMenu
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

    public CustomizeModelMenu(MenuPool menuPool, IPedSwap pedSwap, INameProvideable names, IPedSwappable player, IEntityProvideable world, ISettingsProvideable settings, PedCustomizer pedCustomizer, PedCustomizerMenu pedCustomizerMenu)
    {
        PedSwap = pedSwap;
        MenuPool = menuPool;
        Names = names;
        Player = player;
        World = world;
        Settings = settings;
        PedCustomizer = pedCustomizer;
        PedCustomizerMenu = pedCustomizerMenu;
    }
    public void Setup(UIMenu CustomizeMainMenu)
    {
        UIMenu ModelSubMenu = MenuPool.AddSubMenu(CustomizeMainMenu, "Model");
        CustomizeMainMenu.MenuItems[CustomizeMainMenu.MenuItems.Count() - 1].Description = "Change the model of the current ped";
        CustomizeMainMenu.MenuItems[CustomizeMainMenu.MenuItems.Count() - 1].RightBadge = UIMenuItem.BadgeStyle.Clothes;
        ModelSubMenu.SetBannerType(EntryPoint.LSRedColor);
        InputModel = new UIMenuItem("Input Model", "Input the model name");
        InputModel.Activated += (sender, selectedItem) =>
        {
            SetModelFromInput();
        };
        ModelSubMenu.AddItem(InputModel);

        SelectModel = new UIMenuListScrollerItem<string>("Select Model", "Select the model from a list", Rage.Model.PedModels.Select(x => x.Name));
        SelectModel.Activated += (sender, selectedItem) =>
        {
            SetModelFromString(SelectModel.SelectedItem);
        };
        ModelSubMenu.AddItem(SelectModel);


        ModelSearchSubMenu = MenuPool.AddSubMenu(ModelSubMenu, "Search Model");
        ModelSubMenu.MenuItems[ModelSubMenu.MenuItems.Count() - 1].Description = "Search for the model by name";
        ModelSubMenu.MenuItems[ModelSubMenu.MenuItems.Count() - 1].RightLabel = "";
        ModelSearchSubMenu.SetBannerType(EntryPoint.LSRedColor);

        SearchModel = new UIMenuItem("Input Name", "Input the model name");
        SearchModel.Activated += (sender, selectedItem) =>
        {
            int itemsAdded = 0;
            FilterString = NativeHelper.GetKeyboardInput("");
            for (int i = ModelSearchSubMenu.MenuItems.Count; i-- > 0;)
            {
                if (ModelSearchSubMenu.MenuItems[i].Text != "Input Name")// && ModelSubMenu.MenuItems[i].Text != removeGPSTTI.Title)
                {
                    ModelSearchSubMenu.MenuItems.RemoveAt(i);
                }
            }
            ModelSearchSubMenu.RefreshIndex();
            foreach (string modelName in Rage.Model.PedModels.Select(x => x.Name))
            {
                if(FilterString == "" || modelName.ToLower().Contains(FilterString.ToLower()))
                {
                    UIMenuItem test = new UIMenuItem(modelName, "");
                    test.Activated += (sender2,selecteditem2) =>
                    {
                        SetModelFromString(test.Text);
                    };
                    ModelSearchSubMenu.AddItem(test);
                    itemsAdded++;
                }
                if(itemsAdded >= 50)
                {
                    break;
                }
            }
            
        };
        ModelSearchSubMenu.AddItem(SearchModel);
    }
    private void SetModelFromInput()
    {
        string modelName = NativeHelper.GetKeyboardInput("player_zero");
        if (new Rage.Model(modelName).IsValid)
        {
            PedCustomizer.WorkingModelName = modelName;
            PedCustomizer.OnModelChanged(true);
        }
    }
    private void SetModelFromString(string modelName)
    {    
        if (new Rage.Model(modelName).IsValid)
        {
            PedCustomizer.WorkingModelName = modelName;
            PedCustomizer.OnModelChanged(true);
        }
    }
}

