using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Mod;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class CustomizeFaceMorphMenu
{
    private IPedSwap PedSwap;
    private MenuPool MenuPool;
    private INameProvideable Names;
    private IPedSwappable Player;
    private IEntityProvideable World;
    private ISettingsProvideable Settings;
    private PedCustomizer PedCustomizer;
    private PedCustomizerMenu PedCustomizerMenu;
    private List<FaceFeature> FaceFeatures;
    private UIMenu HeadSubMenu;

    private UIMenu CustomizeMainMenu;
    private UIMenu FaceMorphSubMenu;

    public CustomizeFaceMorphMenu(MenuPool menuPool, IPedSwap pedSwap, INameProvideable names, IPedSwappable player, IEntityProvideable world, ISettingsProvideable settings, PedCustomizer pedCustomizer, PedCustomizerMenu pedCustomizerMenu)
    {
        PedSwap = pedSwap;
        MenuPool = menuPool;
        Names = names;
        Player = player;
        World = world;
        Settings = settings;
        PedCustomizer = pedCustomizer;
        PedCustomizerMenu = pedCustomizerMenu;

        FaceFeatures = new List<FaceFeature>() {
            new FaceFeature(0,"Nose Width"),
            new FaceFeature(1, "Nose Peak"),
            new FaceFeature(2, "Nose Length"),
            new FaceFeature(3, "Nose Bone Curveness"),
            new FaceFeature(4, "Nose Tip"),
            new FaceFeature(5, "Nose Bone Twist"),
            new FaceFeature(6, "Eyebrow Up/Down"),
            new FaceFeature(7, "Eyebrow In/Out"),
            new FaceFeature(8, "Cheek Bones Up/Down"),
            new FaceFeature(9, "Cheek Sideways Bone Size"),
            new FaceFeature(10, "Cheek Bones Width"),
            new FaceFeature(11, "Eye Opening"),
            new FaceFeature(12, "Lip Thickness"),
            new FaceFeature(13, "Jaw Bone Width"),
            new FaceFeature(14, "Jaw Bone Shape"),
            new FaceFeature(15, "Chin Bone"),
            new FaceFeature(16, "Chin Bone Length"),
            new FaceFeature(17, "Chin Bone Shape"),
            new FaceFeature(18, "Chin Hole") { RangeLow = 0.0f },
            new FaceFeature(19, "Neck Thickness") { RangeLow = 0.0f },


        };
    }
    public void Create(UIMenu headSubMenu)
    {
        HeadSubMenu = headSubMenu;
        FaceMorphSubMenu = MenuPool.AddSubMenu(HeadSubMenu, "Features");
        FaceMorphSubMenu.SubtitleText = "FEATURES";
        HeadSubMenu.MenuItems[HeadSubMenu.MenuItems.Count() - 1].Description = "Change the features of the current ped";
        HeadSubMenu.MenuItems[HeadSubMenu.MenuItems.Count() - 1].RightBadge = UIMenuItem.BadgeStyle.Mask;
        FaceMorphSubMenu.SetBannerType(EntryPoint.LSRedColor);
        FaceMorphSubMenu.InstructionalButtonsEnabled = false;
    }
    public void Setup()
    {
        FaceMorphSubMenu.Clear();
        foreach(FaceFeature ff in FaceFeatures)
        {
            ff.AddCustomizeMenu(MenuPool, FaceMorphSubMenu, PedCustomizer.ModelPed, PedCustomizer);
        }
    }
}

