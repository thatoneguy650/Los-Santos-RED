using LosSantosRED.lsr;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;


public class DebugAnimationSubMenu : DebugSubMenu
{
    private string FilterString;
    private UIMenu ModelSearchResultSubMenu;
    private ModDataFileManager ModDataFileManager;
    private TestAnimation SelectedAnimation;

    private float BlendIn = 8.0f;
    private float BlendOut = -8.0f;
    private int Time = -1;
    private int Flags = 0;
    private UIMenuItem playAnimationMenu;
    private UIMenuCheckboxItem IsFacialMenu;

    public DebugAnimationSubMenu(UIMenu debug, MenuPool menuPool, IActionable player, ModDataFileManager modDataFileManager) : base(debug, menuPool, player)
    {
        ModDataFileManager = modDataFileManager;
    }
    public override void AddItems()
    {
        SubMenu = MenuPool.AddSubMenu(Debug, "Animation Menu");
        SubMenu.SetBannerType(EntryPoint.LSRedColor);
        Debug.MenuItems[Debug.MenuItems.Count() - 1].Description = "Find and test various animations.";
        SubMenu.Clear();
        CreateAnimationSearchMenuItems();
        CreateAnimationPlayMenuItems();

    }

    private void CreateAnimationPlayMenuItems()
    {
        UIMenu PlayAnimationSubMenu = MenuPool.AddSubMenu(SubMenu, "Play Animation");
        SubMenu.MenuItems[SubMenu.MenuItems.Count() - 1].Description = "Play the selected animation.";
        SubMenu.MenuItems[SubMenu.MenuItems.Count() - 1].RightLabel = "";
        PlayAnimationSubMenu.SetBannerType(EntryPoint.LSRedColor);
        PlayAnimationSubMenu.InstructionalButtonsEnabled = false;
        PlayAnimationSubMenu.Width = 0.35f;





        playAnimationMenu = new UIMenuItem("Play Animation", "Play the selected animation");
        playAnimationMenu.Activated += (sender, selectedItem) =>
        {
            if(SelectedAnimation == null)
            {
                Game.DisplaySubtitle("Select an animation first");
                return;
            }
            PlaySelectedAnimation();
        };
        PlayAnimationSubMenu.AddItem(playAnimationMenu);


        UIMenuItem stopAnimationMenu = new UIMenuItem("Stop Animation", "Stop the current animation");
        stopAnimationMenu.Activated += (sender, selectedItem) =>
        {
            NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
        };
        PlayAnimationSubMenu.AddItem(stopAnimationMenu);


        UIMenuItem SetBlendInMenu = new UIMenuItem("Blend In", "Set Blend In Parameter") { RightLabel = $"{BlendIn}" };
        SetBlendInMenu.Activated += (sender, selectedItem) =>
        {
            if (float.TryParse(NativeHelper.GetKeyboardInput($"{BlendIn}"), out float NewBlendIn))
            {
                BlendIn = NewBlendIn;
                SetBlendInMenu.RightLabel = $"{BlendIn}";
            }
        };
        PlayAnimationSubMenu.AddItem(SetBlendInMenu);

        UIMenuItem SetBlendOutMenu = new UIMenuItem("Blend Out", "Set Blend Out Parameter") { RightLabel = $"{BlendOut}" };
        SetBlendOutMenu.Activated += (sender, selectedItem) =>
        {
            if (float.TryParse(NativeHelper.GetKeyboardInput($"{BlendOut}"), out float NewBlendOut))
            {
                BlendOut = NewBlendOut;
                SetBlendOutMenu.RightLabel = $"{BlendOut}";
            }
        };
        PlayAnimationSubMenu.AddItem(SetBlendOutMenu);

        UIMenuItem SetTimeMenu = new UIMenuItem("Time", "Set Time Parameter") { RightLabel = $"{Time}" };
        SetTimeMenu.Activated += (sender, selectedItem) =>
        {
            if (int.TryParse(NativeHelper.GetKeyboardInput($"{Time}"), out int newTime))
            {
                Time = newTime;
                SetTimeMenu.RightLabel = $"{Time}";
            }
        };
        PlayAnimationSubMenu.AddItem(SetTimeMenu);


        UIMenuItem SetFlagsMenu = new UIMenuItem("Flags", "Set Flags Parameter") { RightLabel = $"{Flags}" };
        SetFlagsMenu.Activated += (sender, selectedItem) =>
        {
            if (int.TryParse(NativeHelper.GetKeyboardInput($"{Flags}"), out int newFlags))
            {
                Flags = newFlags;
                SetFlagsMenu.RightLabel = $"{Flags}";
            }
        };
        PlayAnimationSubMenu.AddItem(SetFlagsMenu);



        IsFacialMenu = new UIMenuCheckboxItem("Facial", false, "If checked the animation will play on the face");
        PlayAnimationSubMenu.AddItem(IsFacialMenu);


    }

    private void CreateAnimationSearchMenuItems()
    {
        UIMenu ModelSearchSubMenu = MenuPool.AddSubMenu(SubMenu, "Search For Animation");
        SubMenu.MenuItems[SubMenu.MenuItems.Count() - 1].Description = "Search for the animation by partial or full name.";
        SubMenu.MenuItems[SubMenu.MenuItems.Count() - 1].RightLabel = "";
        ModelSearchSubMenu.SetBannerType(EntryPoint.LSRedColor);
        ModelSearchSubMenu.InstructionalButtonsEnabled = false;
        ModelSearchSubMenu.Width = 0.35f;

        UIMenuItem SearchModel = new UIMenuItem("Search Value", "Input the search string for the animation dictionary or name");
        SearchModel.Activated += (sender, selectedItem) =>
        {
            FilterString = NativeHelper.GetKeyboardInput("");
            if (string.IsNullOrEmpty(FilterString))
            {
                FilterString = "";
            }

            CreateAnimationMenu();

            SearchModel.RightLabel = FilterString;
        };
        ModelSearchSubMenu.AddItem(SearchModel);

        ModelSearchResultSubMenu = MenuPool.AddSubMenu(ModelSearchSubMenu, "Filtered Animations");
        SubMenu.MenuItems[SubMenu.MenuItems.Count() - 1].Description = "Browse filtered animations by dictionary.";
        SubMenu.MenuItems[SubMenu.MenuItems.Count() - 1].RightLabel = "";
        ModelSearchResultSubMenu.SetBannerType(EntryPoint.LSRedColor);
        ModelSearchResultSubMenu.InstructionalButtonsEnabled = false;
        ModelSearchResultSubMenu.Width = 0.35f;
        CreateAnimationMenu();
    }

    private void CreateAnimationMenu()
    {
        ModelSearchResultSubMenu.Clear();
        List<UIMenu> DictionarySubMenus = new List<UIMenu>();

        foreach (TestAnimation testAnimation in ModDataFileManager.TestAnimations.Animations.Where(x => string.IsNullOrEmpty(FilterString) || x.Dictionary.Contains(FilterString) || x.Name.Contains(FilterString)).Take(5000))
        {
            UIMenu submenuToAdd = DictionarySubMenus.FirstOrDefault(x => x.SubtitleText == testAnimation.Dictionary);
            if (submenuToAdd == null)
            {
                submenuToAdd = MenuPool.AddSubMenu(ModelSearchResultSubMenu, testAnimation.Dictionary);
                submenuToAdd.SetBannerType(EntryPoint.LSRedColor);
                submenuToAdd.Width = 0.35f;
                DictionarySubMenus.Add(submenuToAdd);
            }
            UIMenuItem animationMenuItem = new UIMenuItem(testAnimation.Name);
            animationMenuItem.Activated += (sender, e) =>
            {
                SelectedAnimation = testAnimation;
                if(playAnimationMenu != null)
                {
                    playAnimationMenu.Description = $"Selected: {testAnimation.Dictionary} {testAnimation.Name}";
                }
                PlaySelectedAnimation();

            };
            submenuToAdd.AddItem(animationMenuItem);
        }
    }
    private void PlaySelectedAnimation()
    {
        Game.DisplaySubtitle($"ANIM SELECTED DICT: {SelectedAnimation.Dictionary} NAME: {SelectedAnimation.Name}");
        if (!AnimationDictionary.RequestAnimationDictionayResult(SelectedAnimation.Dictionary))
        {
            Game.DisplaySubtitle("Could not load animation dictionary");
            return;
        }

        if (IsFacialMenu.Checked)
        {
            NativeFunction.Natives.PLAY_FACIAL_ANIM(Player.Character, SelectedAnimation.Name, SelectedAnimation.Dictionary);
        }
        else
        {
            NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, SelectedAnimation.Dictionary, SelectedAnimation.Name, BlendIn, BlendOut, Time, Flags, 0, false, false, false);
        }
    }

}

