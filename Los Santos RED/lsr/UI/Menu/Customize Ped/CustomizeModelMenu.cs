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
    private string FilterString = "";
    private UIMenuListScrollerItem<string> FilteredModels;
    private UIMenuListScrollerItem<string> SelectSimpleModel;

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
        //CustomizeMainMenu.MenuItems[CustomizeMainMenu.MenuItems.Count() - 1].RightBadge = UIMenuItem.BadgeStyle.Clothes;
        ModelSubMenu.SetBannerType(EntryPoint.LSRedColor);
        ModelSubMenu.InstructionalButtonsEnabled = false;
        ModelSubMenu.Width = 0.35f;

        ModelSubMenu.OnMenuOpen += (sender) =>
        {
            PedCustomizer.CameraCycler.SetDefault();
        };
        ModelSubMenu.OnMenuClose += (sender) =>
        {
            PedCustomizer.CameraCycler.SetDefault();
        };


        InputModel = new UIMenuItem("Input Model Name", "Input the full model name exactly as expected. If unsure, use the search.");
        InputModel.Activated += (sender, selectedItem) =>
        {
            SetModelFromInput();
        };
        ModelSubMenu.AddItem(InputModel);

        List<string> pedModelList = new List<string>
        {
            "mp_m_freemode_01",
            "mp_f_freemode_01",
            


            "player_zero",
            "player_one",
            "player_two",
            "ig_tonyprince",
            "ig_tracydisanto",
            "ig_amandatownley",
            "ig_andreas",
            "ig_ashley",
            "ig_barry",



            "ig_brad",
            "ig_davenorton",
            "ig_devin",
            "ig_denise",
            "ig_clay",
            "ig_drfriedlander",
            "ig_fabien",
            "ig_floyd",

            "ig_jimmydisanto",
            "ig_karen_daniels",
            "ig_lamardavis",
            "ig_lazlow",
            "ig_lestercrest",
            "ig_michelle",
            "ig_nervousron",
            "ig_taocheng",
            "ig_brucie2",
        };

        SelectSimpleModel = new UIMenuListScrollerItem<string>("Simple Model List", "Select the model name from a list of most used models. Only default models are included. Add on models must have the full model name entered in 'Input Model Name'.",
            pedModelList);
        SelectSimpleModel.Activated += (sender, selectedItem) =>
        {
            SetModelFromString(SelectSimpleModel.SelectedItem);
        };
        ModelSubMenu.AddItem(SelectSimpleModel);






        SelectModel = new UIMenuListScrollerItem<string>("Full Model List", "Select the model name from a list. Only default models are included. Add on models must have the full model name entered in 'Input Model Name'.", 
            Rage.Model.PedModels.Select(x => x.Name));
        SelectModel.Activated += (sender, selectedItem) =>
        {
            SetModelFromString(SelectModel.SelectedItem);
        };
        ModelSubMenu.AddItem(SelectModel);





        ModelSearchSubMenu = MenuPool.AddSubMenu(ModelSubMenu, "Search For Model");
        ModelSubMenu.MenuItems[ModelSubMenu.MenuItems.Count() - 1].Description = "Search for the model by partial or full name. Only default models are included.";
        ModelSubMenu.MenuItems[ModelSubMenu.MenuItems.Count() - 1].RightLabel = "";
        ModelSearchSubMenu.SetBannerType(EntryPoint.LSRedColor);
        ModelSearchSubMenu.InstructionalButtonsEnabled = false;
        ModelSearchSubMenu.Width = 0.35f;
        SearchModel = new UIMenuItem("Search Value", "Input the search string for the model name");
        SearchModel.Activated += (sender, selectedItem) =>
        {
            FilterString = NativeHelper.GetKeyboardInput("");
            if (string.IsNullOrEmpty(FilterString)) 
            {
                FilterString = "";
            }
            FilteredModels.Items = Rage.Model.PedModels.Select(x => x.Name).Where(x => FilterString == "" || x.ToLower().Contains(FilterString.ToLower())).ToList();
            SearchModel.RightLabel = FilterString;
        };
        ModelSearchSubMenu.AddItem(SearchModel);


        FilteredModels = new UIMenuListScrollerItem<string>("Filtered Model List", "Select the model name from a filtered list. Only default models are included.", Rage.Model.PedModels.Select(x => x.Name).Where(x=> FilterString == "" || x.ToLower().Contains(FilterString.ToLower())));
        FilteredModels.Activated += (sender, selectedItem) =>
        {
            SetModelFromString(FilteredModels.SelectedItem);
        };
        ModelSearchSubMenu.AddItem(FilteredModels);


    }
    private void SetModelFromInput()
    {
        try
        {
            string modelName = NativeHelper.GetKeyboardInput("player_zero");
            if (string.IsNullOrEmpty(modelName) || modelName == "")
            {
                return;
            }
            if (new Rage.Model(modelName).IsValid)
            {
                PedCustomizer.WorkingModelName = modelName;
                PedCustomizer.OnModelChanged(true);
            }
        }
        catch(Exception ex)
        {
            EntryPoint.WriteToConsole($"SetModelFromInput ERROR {ex.Message} {ex.StackTrace}", 0);
            Rage.Game.DisplayNotification("Error Setting Model");
        }
    }
    private void SetModelFromString(string modelName)
    {
        try
        {
            if(string.IsNullOrEmpty(modelName) || modelName == "")
            {
                return;
            }
            if (new Rage.Model(modelName).IsValid)
            {
                PedCustomizer.WorkingModelName = modelName;
                PedCustomizer.OnModelChanged(true);
            }
        }
        catch(Exception ex)
        {
            EntryPoint.WriteToConsole($"SetModelFromString ERROR {ex.Message} {ex.StackTrace}",0);
            Rage.Game.DisplayNotification("Error Setting Model");
        }
    }
}

