using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Mod;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class CustomizeAncestryMenu
{
    private IPedSwap PedSwap;
    private MenuPool MenuPool;
    private INameProvideable Names;
    private IPedSwappable Player;
    private IEntityProvideable World;
    private ISettingsProvideable Settings;
    private PedCustomizer PedCustomizer;
    private PedCustomizerMenu PedCustomizerMenu;
    private List<HeadLookup> HeadList;
    private UIMenuItem RandomizeHead;
    private UIMenuListScrollerItem<HeadLookup> Parent1IDMenu;
    private UIMenuListScrollerItem<HeadLookup> Parent2IDMenu;
    private UIMenuNumericScrollerItem<float> Parent1MixMenu;
    private UIMenuNumericScrollerItem<float> Parent2MixMenu;
    private UIMenu HeadSubMenu;
    private UIMenu CustomizeMainMenu;
    private UIMenu AncestrySubMenu;
    public CustomizeAncestryMenu(MenuPool menuPool, IPedSwap pedSwap, INameProvideable names, IPedSwappable player, IEntityProvideable world, ISettingsProvideable settings, PedCustomizer pedCustomizer, PedCustomizerMenu pedCustomizerMenu)
    {
        PedSwap = pedSwap;
        MenuPool = menuPool;
        Names = names;
        Player = player;
        World = world;
        Settings = settings;
        PedCustomizer = pedCustomizer;
        PedCustomizerMenu = pedCustomizerMenu;
        HeadList = new List<HeadLookup>()
        {
            new HeadLookup(0,"Benjamin"),
            new HeadLookup(1,"Daniel"),
            new HeadLookup(2,"Joshua"),
            new HeadLookup(3,"Noah"),
            new HeadLookup(4,"Andrew"),
            new HeadLookup(5,"Juan"),
            new HeadLookup(6,"Alex"),
            new HeadLookup(7,"Isaac"),
            new HeadLookup(8,"Evan"),
            new HeadLookup(9,"Ethan"),
            new HeadLookup(10,"Vincent"),
            new HeadLookup(11,"Angel"),
            new HeadLookup(12,"Diego"),
            new HeadLookup(13,"Adrian"),
            new HeadLookup(14,"Gabriel"),
            new HeadLookup(15,"Michael"),
            new HeadLookup(16,"Santiago"),
            new HeadLookup(17,"Kevin"),
            new HeadLookup(18,"Louis"),
            new HeadLookup(19,"Samuel"),
            new HeadLookup(20,"Anthony"),
            new HeadLookup(21,"Hannah"),
            new HeadLookup(22,"Audrey"),
            new HeadLookup(23,"Jasmine"),
            new HeadLookup(24,"Giselle"),
            new HeadLookup(25,"Amelia"),
            new HeadLookup(26,"Isabella"),
            new HeadLookup(27,"Zoe"),
            new HeadLookup(28,"Ava"),
            new HeadLookup(29,"Camila"),
            new HeadLookup(30,"Violet"),
            new HeadLookup(31,"Sophia"),
            new HeadLookup(32,"Evelyn"),
            new HeadLookup(33,"Nicole"),
            new HeadLookup(34,"Ashley"),
            new HeadLookup(35,"Grace"),
            new HeadLookup(36,"Brianna"),
            new HeadLookup(37,"Natalie"),
            new HeadLookup(38,"Olivia"),
            new HeadLookup(39,"Elizabeth"),
            new HeadLookup(40,"Charlotte"),
            new HeadLookup(41,"Emma"),
            new HeadLookup(42,"Claude"),
            new HeadLookup(43,"Niko"),
            new HeadLookup(44,"John"),
            new HeadLookup(45,"Misty"),
        };
    }
    public void Create(UIMenu headSubMenu)
    {
        HeadSubMenu = headSubMenu;
        AncestrySubMenu = MenuPool.AddSubMenu(HeadSubMenu, "Heritage");
        AncestrySubMenu.SubtitleText = "HERITAGE";
        HeadSubMenu.MenuItems[HeadSubMenu.MenuItems.Count() - 1].Description = "Change the heritage of the current ped";
        HeadSubMenu.MenuItems[HeadSubMenu.MenuItems.Count() - 1].RightBadge = UIMenuItem.BadgeStyle.Heart;
        AncestrySubMenu.SetBannerType(EntryPoint.LSRedColor);
        AncestrySubMenu.InstructionalButtonsEnabled = false;
    }
    public void Setup()
    {
        AncestrySubMenu.Clear();
        if (PedCustomizer.PedModelIsFreeMode)
        {
            if (PedCustomizer.WorkingVariation.HeadBlendData == null)
            {
                PedCustomizer.WorkingVariation.HeadBlendData = new HeadBlendData(0, 0, 0, 0, 0, 0, 1.0f, 1.0f, 0.0f);
            }
        }
        RandomizeHead = new UIMenuItem("Randomize", "Randomize head data");
        RandomizeHead.Activated += (sender, selectedItem) =>
        {
            RandomizePedHead();
        };
        AncestrySubMenu.AddItem(RandomizeHead);

        Parent1IDMenu = new UIMenuListScrollerItem<HeadLookup>("First Parent", "Select first parent", HeadList);
        Parent1IDMenu.SelectedItem = HeadList.FirstOrDefault(x => x.HeadID == 0);
        Parent1IDMenu.Activated += (sender, selectedItem) =>
        {
            Parent1Activated();
        };
        Parent1IDMenu.IndexChanged += (sender, oldIndex, newIndex) =>
        {
            Parent1Activated();
        };
        AncestrySubMenu.AddItem(Parent1IDMenu);

        Parent2IDMenu = new UIMenuListScrollerItem<HeadLookup>("Second Parent", "Select second parent", HeadList);
        Parent2IDMenu.SelectedItem = HeadList.FirstOrDefault(x => x.HeadID == 0);
        Parent2IDMenu.Activated += (sender, selectedItem) =>
        {
            Parent2Activated();
        };
        Parent2IDMenu.IndexChanged += (sender, oldIndex, newIndex) =>
        {
            Parent2Activated();
        };
        AncestrySubMenu.AddItem(Parent2IDMenu);

        Parent1MixMenu = new UIMenuNumericScrollerItem<float>("Resemblance", "Select if your resemblance is influenced more by the first parent or second parent.", 0.0f, 1.0f, 0.1f);
        Parent1MixMenu.Formatter = v => "First - " + (1.0f - v).ToString("P0") + " Second - " + v.ToString("P0");
        Parent1MixMenu.Value = 1.0f;
        Parent1MixMenu.Activated += (sender, selectedItem) =>
        {
            Parent1MixActivated();
        };
        Parent1MixMenu.IndexChanged += (sender, oldIndex, newIndex) =>
        {
            Parent1MixActivated();
        };
        AncestrySubMenu.AddItem(Parent1MixMenu);

        Parent2MixMenu = new UIMenuNumericScrollerItem<float>("Skin Tone", "Select if your skin tone is influenced more by the first parent or second parent.", 0.0f, 1.0f, 0.1f);
        Parent2MixMenu.Formatter = v => "First - " + (1.0f - v).ToString("P0") + " Second - " + v.ToString("P0");
        Parent2MixMenu.Value = 1.0f;
        Parent2MixMenu.Activated += (sender, selectedItem) =>
        {
            Parent2MixActivated();
        };
        Parent2MixMenu.IndexChanged += (sender, oldIndex, newIndex) =>
        {
            Parent2MixActivated();
        };
        AncestrySubMenu.AddItem(Parent2MixMenu);

        OnHeadblendValuesChanged();
    }
    private void Parent1Activated()
    {
        if (PedCustomizerMenu.IsProgramicallySettingFieldValues)
        {
            return;
        }
        //EntryPoint.WriteToConsoleTestLong("Parent1Activated");
        PedCustomizer.WorkingVariation.HeadBlendData.skinFirst = Parent1IDMenu.SelectedItem.HeadID;
        PedCustomizer.WorkingVariation.HeadBlendData.shapeFirst = Parent1IDMenu.SelectedItem.HeadID;
        PedCustomizer.OnVariationChanged();
    }
    private void Parent2Activated()
    {
        if (PedCustomizerMenu.IsProgramicallySettingFieldValues)
        {
            return;
        }
        //EntryPoint.WriteToConsoleTestLong("Parent2Activated");
        PedCustomizer.WorkingVariation.HeadBlendData.skinSecond = Parent2IDMenu.SelectedItem.HeadID;
        PedCustomizer.WorkingVariation.HeadBlendData.shapeSecond = Parent2IDMenu.SelectedItem.HeadID;
        PedCustomizer.OnVariationChanged();
    }
    private void Parent1MixActivated()
    {
        if (PedCustomizerMenu.IsProgramicallySettingFieldValues)
        {
            return;
        }
        float newMix = Parent1MixMenu.Value;
        PedCustomizer.WorkingVariation.HeadBlendData.shapeMix = newMix;
        PedCustomizer.OnVariationChanged();
    }
    private void Parent2MixActivated()
    {
        if (PedCustomizerMenu.IsProgramicallySettingFieldValues)
        {
            return;
        }
        float newMix = Parent2MixMenu.Value;
        PedCustomizer.WorkingVariation.HeadBlendData.skinMix = newMix;
        PedCustomizer.OnVariationChanged();
    }
    private void RandomizePedHead()
    {
        if (PedCustomizer.ModelPed.Exists() && PedCustomizer.PedModelIsFreeMode)
        {
            //RandomizeOverlay();
            RandomizeHeadblend();
            //RandomizeHairStyle();
            PedCustomizer.OnVariationChanged();
        }
    }
    private void RandomizeHeadblend()
    {
        int MotherID = 0;
        int FatherID = 0;
        float FatherSide = 0f;
        float MotherSide = 0f;
        MotherID = RandomItems.GetRandomNumberInt(0, 45);
        FatherID = RandomItems.GetRandomNumberInt(0, 45);
        if (PedCustomizer.ModelPed.IsMale)
        {
            FatherSide = RandomItems.GetRandomNumber(0.75f, 1.0f);
            MotherSide = 1.0f - FatherSide;
        }
        else
        {
            MotherSide = RandomItems.GetRandomNumber(0.75f, 1.0f);
            FatherSide = 1.0f - MotherSide;
        }
        PedCustomizer.WorkingVariation.HeadBlendData = new HeadBlendData(MotherID, FatherID, 0, MotherID, FatherID, 0, MotherSide, FatherSide, 0.0f);
        OnHeadblendValuesChanged();
    }
    private void OnHeadblendValuesChanged()
    {
        if (PedCustomizerMenu.IsProgramicallySettingFieldValues)
        {
            return;
        }
        if (HeadList != null && PedCustomizer.WorkingVariation.HeadBlendData != null)
        {
            HeadLookup Parent1IDMenuHead = HeadList.FirstOrDefault(x => x.HeadID == PedCustomizer.WorkingVariation.HeadBlendData.skinFirst);
            if (Parent1IDMenuHead != null)
            {
                Parent1IDMenu.SelectedItem = Parent1IDMenuHead;
                //EntryPoint.WriteToConsoleTestLong($"OnHeadblendValuesChanged Parent1IDMenuHead {Parent1IDMenuHead.HeadName}");
            }
            HeadLookup Parent2IDMenuHead = HeadList.FirstOrDefault(x => x.HeadID == PedCustomizer.WorkingVariation.HeadBlendData.skinSecond);
            if (Parent2IDMenuHead != null)
            {
                Parent2IDMenu.SelectedItem = Parent2IDMenuHead;
                //EntryPoint.WriteToConsoleTestLong($"OnHeadblendValuesChanged Parent2IDMenuHead {Parent2IDMenuHead.HeadName}");
            }
            Parent1MixMenu.Value = PedCustomizer.WorkingVariation.HeadBlendData.shapeMix == -1 ? 0 : PedCustomizer.WorkingVariation.HeadBlendData.shapeMix;
            Parent2MixMenu.Value = PedCustomizer.WorkingVariation.HeadBlendData.skinMix == -1 ? 0 : PedCustomizer.WorkingVariation.HeadBlendData.skinMix;
            //EntryPoint.WriteToConsoleTestLong($"OnHeadblendValuesChanged MIX P1{Parent1MixMenu.Value} P2{Parent2MixMenu.Value}");
        }
        //EntryPoint.WriteToConsoleTestLong("OnHeadblendValuesChanged Executed");
    }
    private void MatchHeadblendToMenuValues()
    {
        PedCustomizer.WorkingVariation.HeadBlendData.skinFirst = Parent1IDMenu.SelectedItem.HeadID;
        PedCustomizer.WorkingVariation.HeadBlendData.shapeFirst = Parent1IDMenu.SelectedItem.HeadID;
        PedCustomizer.WorkingVariation.HeadBlendData.skinSecond = Parent2IDMenu.SelectedItem.HeadID;
        PedCustomizer.WorkingVariation.HeadBlendData.shapeSecond = Parent2IDMenu.SelectedItem.HeadID;
        PedCustomizer.WorkingVariation.HeadBlendData.shapeMix = Parent1MixMenu.Value;
        PedCustomizer.WorkingVariation.HeadBlendData.skinMix = Parent2MixMenu.Value;
    }
}

