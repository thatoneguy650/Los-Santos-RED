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


public class CustomizeDemographicsMenu
{
    private IPedSwap PedSwap;
    private MenuPool MenuPool;
    private INameProvideable Names;
    private IPedSwappable Player;
    private IEntityProvideable World;
    private ISettingsProvideable Settings;
    private PedCustomizer PedCustomizer;
    private UIMenuItem ChangeName;
    private UIMenuItem RandomizeName;
    private UIMenuItem ChangeMoney;
    private PedCustomizerMenu PedCustomizerMenu;

    public CustomizeDemographicsMenu(MenuPool menuPool, IPedSwap pedSwap, INameProvideable names, IPedSwappable player, IEntityProvideable world, ISettingsProvideable settings, PedCustomizer pedCustomizer, PedCustomizerMenu pedCustomizerMenu)
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
        UIMenu DemographicsSubMenu = MenuPool.AddSubMenu(CustomizeMainMenu, "Demographics");
        CustomizeMainMenu.MenuItems[CustomizeMainMenu.MenuItems.Count() - 1].Description = "Change demographics for the current ped";
        CustomizeMainMenu.MenuItems[CustomizeMainMenu.MenuItems.Count() - 1].RightBadge = UIMenuItem.BadgeStyle.Lock;
        DemographicsSubMenu.SetBannerType(EntryPoint.LSRedColor);
        DemographicsSubMenu.InstructionalButtonsEnabled = false;

        DemographicsSubMenu.OnMenuOpen += (sender) =>
        {
            PedCustomizer.CameraCycler.SetDefault();
        };
        DemographicsSubMenu.OnMenuClose += (sender) =>
        {
            PedCustomizer.CameraCycler.SetDefault();
        };


        ChangeName = new UIMenuItem("Change Name", "Current: " + PedCustomizer.WorkingName);
        ChangeName.Activated += (sender, selectedItem) =>
        {
            ChangeWorkingName();
        };
        DemographicsSubMenu.AddItem(ChangeName);
        RandomizeName = new UIMenuItem("Randomize Name", "Current: " + PedCustomizer.WorkingName);
        RandomizeName.Activated += (sender, selectedItem) =>
        {
            RandomizeWorkingName();
        };
        DemographicsSubMenu.AddItem(RandomizeName);
        ChangeMoney = new UIMenuItem("Set Money", "Amount: " + PedCustomizer.WorkingMoney.ToString("C0"));
        ChangeMoney.Activated += (sender, selectedItem) =>
        {
            ChangeWorkingMoney();
        };
        DemographicsSubMenu.AddItem(ChangeMoney);








    }
    private void ChangeWorkingMoney()
    {
        if (int.TryParse(NativeHelper.GetKeyboardInput(PedCustomizer.WorkingMoney.ToString()), out int BribeAmount))
        {
            PedCustomizer.WorkingMoney = BribeAmount;
            OnMoneyChanged();
        }
    }
    private void ChangeWorkingName()
    {
        PedCustomizer.WorkingName = NativeHelper.GetKeyboardInput(PedCustomizer.WorkingName);
        OnWorkingNameChanged();
    }
    private void RandomizeWorkingName()
    {
        string Name = "John Doe";
        if (PedCustomizer.ModelPed.Exists())
        {
            Name = Names.GetRandomName(PedCustomizer.ModelPed.IsMale);
        }
        else
        {
            Name = Names.GetRandomName(false);
        }
        PedCustomizer.WorkingName = Name;
        OnWorkingNameChanged();
    }
    private void OnWorkingNameChanged()
    {
        ChangeName.Description = "Current: " + PedCustomizer.WorkingName;
        RandomizeName.Description = "Current: " + PedCustomizer.WorkingName;
    }
    private void OnMoneyChanged()
    {
        ChangeMoney.Description = "Current: " + PedCustomizer.WorkingMoney.ToString("C0");
    }

    public void OnModelChanged()
    {
        OnWorkingNameChanged();
        OnMoneyChanged();
        EntryPoint.WriteToConsole("CustomizeDemographicsMenu.OnModelChanged Executed");
    }
}

