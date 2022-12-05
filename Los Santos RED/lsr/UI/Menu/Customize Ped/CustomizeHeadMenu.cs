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


public class CustomizeHeadMenu
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
    private List<HeadOverlayData> HeadOverlayLookups;
    private List<ColorLookup> ColorList;
    private UIMenuItem RandomizeHead;
    private UIMenuListScrollerItem<HeadLookup> Parent1IDMenu;
    private UIMenuListScrollerItem<HeadLookup> Parent2IDMenu;
    private UIMenuNumericScrollerItem<float> Parent1MixMenu;
    private UIMenuNumericScrollerItem<float> Parent2MixMenu;
    private UIMenuItem RandomizeHair;
    private UIMenuListScrollerItem<ColorLookup> HairPrimaryColorMenu;
    private UIMenuListScrollerItem<ColorLookup> HairSecondaryColorMenu;
    private UIMenu HeadSubMenu;
    private UIMenuItem HeadSubMenuItem;
    private UIMenu CustomizeMainMenu;
    private UIMenu AncestrySubMenu;
    private UIMenu HairSubMenu;
    private UIMenu FaceSubMenu;
    private List<OverlayMenuGroup> OverlayMenus = new List<OverlayMenuGroup>();
    private FashionComponent HairFashionComponenet;

    public CustomizeHeadMenu(MenuPool menuPool, IPedSwap pedSwap, INameProvideable names, IPedSwappable player, IEntityProvideable world, ISettingsProvideable settings, PedCustomizer pedCustomizer, PedCustomizerMenu pedCustomizerMenu)
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
        HeadOverlayLookups = new List<HeadOverlayData>() {
            new HeadOverlayData(0,"Blemishes"),
            new HeadOverlayData(1, "Facial Hair") { ColorType = 1 },
            new HeadOverlayData(2, "Eyebrows") { ColorType = 1 },
            new HeadOverlayData(3, "Ageing"),
            new HeadOverlayData(4, "Makeup"),
            new HeadOverlayData(5, "Blush") { ColorType = 2 },
            new HeadOverlayData(6, "Complexion"),
            new HeadOverlayData(7, "Sun Damage"),
            new HeadOverlayData(8, "Lipstick") { ColorType = 2 },
            new HeadOverlayData(9, "Moles/Freckles"),
            new HeadOverlayData(10, "Chest Hair") { ColorType = 1 },
            new HeadOverlayData(11, "Body Blemishes"),
            new HeadOverlayData(12, "Add Body Blemishes"),};
        ColorList = new List<ColorLookup>()
        {
        new ColorLookup(0,"Black 1"),
        new ColorLookup(1,"Black 2"),
        new ColorLookup(2,"Black 3"),
        new ColorLookup(3,"Brown 1"),
        new ColorLookup(4,"Brown 2"),
        new ColorLookup(5,"Brown 3"),
        new ColorLookup(6,"Brown 4"),
        new ColorLookup(7,"Brown 5"),
        new ColorLookup(8,"Brown 6"),
        new ColorLookup(9,"Blonde 1"),
        new ColorLookup(10,"Blonde 2"),
        new ColorLookup(11,"Blonde 3"),
        new ColorLookup(12,"Blonde 4"),
        new ColorLookup(13,"Blonde 5"),
        new ColorLookup(14,"Blonde 6"),
        new ColorLookup(15,"Blonde 7"),
        new ColorLookup(16,"Blonde 8"),
        new ColorLookup(17,"Blonde 9"),
        new ColorLookup(18,"Redhead 1"),
        new ColorLookup(19,"Redhead 2"),
        new ColorLookup(20,"Redhead 3"),
        new ColorLookup(21,"Redhead 4"),
        new ColorLookup(22,"Orange 1"),
        new ColorLookup(23,"Orange 2"),
        new ColorLookup(24,"Orange 3"),
        new ColorLookup(25,"Orange 4"),
        new ColorLookup(26,"Grey 1"),
        new ColorLookup(27,"Grey 2"),
        new ColorLookup(28,"White 1"),
        new ColorLookup(29,"White 2"),
        new ColorLookup(30,"Purple 1"),
        new ColorLookup(31,"Purple 2"),
        new ColorLookup(32,"Purple 3"),
        new ColorLookup(33,"Pink 1"),
        new ColorLookup(34,"Pink 2"),
        new ColorLookup(35,"Pink 3"),
        new ColorLookup(36,"Blue 4"),
        new ColorLookup(37,"Blue 5"),
        new ColorLookup(38,"Blue 6"),
        new ColorLookup(39,"Green 1"),
        new ColorLookup(40,"Green 2"),
        new ColorLookup(41,"Green 3"),
        new ColorLookup(42,"Green 4"),
        new ColorLookup(43,"Green 5"),
        new ColorLookup(44,"Green 6"),
        new ColorLookup(45,"Yellow 1"),
        new ColorLookup(46,"Yellow 2"),
        new ColorLookup(47,"Yellow 3"),
        new ColorLookup(48,"Orange 5"),
        new ColorLookup(49,"Orange 6"),
        new ColorLookup(50,"Orange 7"),
        new ColorLookup(51,"Orange 8"),
        new ColorLookup(52,"Red 1"),
        new ColorLookup(53,"Red 1"),
        new ColorLookup(54,"Red 1"),
        new ColorLookup(55,"Brown 7"),
        new ColorLookup(56,"Brown 8"),
        new ColorLookup(57,"Brown 9"),
        new ColorLookup(58,"Brown 10"),
        new ColorLookup(59,"Brown 11"),
        new ColorLookup(60,"Brown 12"),
        new ColorLookup(61,"Black 4"),
        new ColorLookup(62,"Unk 1"),
        new ColorLookup(63,"Unk 2"),
        };
    }
    public void Setup(UIMenu customizeMainMenu)
    {
        CustomizeMainMenu = customizeMainMenu;
        HeadSubMenu = MenuPool.AddSubMenu(CustomizeMainMenu, "Head");
        HeadSubMenuItem = CustomizeMainMenu.MenuItems[CustomizeMainMenu.MenuItems.Count() - 1];


        HeadSubMenuItem.Description = "Change the head features of the current ped";
        HeadSubMenuItem.RightBadge = UIMenuItem.BadgeStyle.Makeup;
        HeadSubMenu.SetBannerType(EntryPoint.LSRedColor);


        SetHeadEnabledStatus();





        AncestrySubMenu = MenuPool.AddSubMenu(HeadSubMenu, "Ancestry");
        HeadSubMenu.MenuItems[HeadSubMenu.MenuItems.Count() - 1].Description = "Change the ancestry of the current ped";
        HeadSubMenu.MenuItems[HeadSubMenu.MenuItems.Count() - 1].RightBadge = UIMenuItem.BadgeStyle.Heart;
        AncestrySubMenu.SetBannerType(EntryPoint.LSRedColor);

        HairSubMenu = MenuPool.AddSubMenu(HeadSubMenu, "Hair");
        HeadSubMenu.MenuItems[HeadSubMenu.MenuItems.Count() - 1].Description = "Change the hair of the current ped";
        HeadSubMenu.MenuItems[HeadSubMenu.MenuItems.Count() - 1].RightBadge = UIMenuItem.BadgeStyle.Barber;
        HairSubMenu.SetBannerType(EntryPoint.LSRedColor);
        HairSubMenu.OnMenuOpen += (sender) =>
        {

        };

        FaceSubMenu = MenuPool.AddSubMenu(HeadSubMenu, "Face");
        HeadSubMenu.MenuItems[HeadSubMenu.MenuItems.Count() - 1].Description = "Change the face of the current ped";
        HeadSubMenu.MenuItems[HeadSubMenu.MenuItems.Count() - 1].RightBadge = UIMenuItem.BadgeStyle.Mask;
        FaceSubMenu.SetBannerType(EntryPoint.LSRedColor);

        SetupAncestryMenu();
        SetupHairMenu();
        SetupFaceMenu();

        
    }
    private void SetHeadEnabledStatus()
    {
        if (PedCustomizer.ModelPed.Exists() && PedCustomizer.PedModelIsFreeMode)
        {
            HeadSubMenuItem.Enabled = true;
        }
        else
        {
            HeadSubMenuItem.Enabled = false;
        }
    }
    private void SetupAncestryMenu()
    {
        AncestrySubMenu.Clear();
        RandomizeHead = new UIMenuItem("Randomize", "Randomize head data");
        RandomizeHead.Activated += (sender, selectedItem) =>
        {
            RandomizePedHead();
        };
        AncestrySubMenu.AddItem(RandomizeHead);

        Parent1IDMenu = new UIMenuListScrollerItem<HeadLookup>("Set Parent 1", "Select parent ID 1", HeadList);
        Parent1IDMenu.Activated += (sender, selectedItem) =>
        {
            Parent1Activated();
        };
        Parent1IDMenu.IndexChanged += (sender, oldIndex, newIndex) =>
        {
            Parent1Activated();
        };
        AncestrySubMenu.AddItem(Parent1IDMenu);

        Parent2IDMenu = new UIMenuListScrollerItem<HeadLookup>("Set Parent 2", "Select parent ID 2", HeadList);
        Parent2IDMenu.Activated += (sender, selectedItem) =>
        {
            Parent2Activated();
        };
        Parent2IDMenu.IndexChanged += (sender, oldIndex, newIndex) =>
        {
            Parent2Activated();
        };
        AncestrySubMenu.AddItem(Parent2IDMenu);

        Parent1MixMenu = new UIMenuNumericScrollerItem<float>("Set Parent 1 Mix", "Select percent of parent ID 1 to use", 0.0f, 1.0f, 0.1f);
        Parent1MixMenu.Activated += (sender, selectedItem) =>
        {
            Parent1MixActivated();
        };
        Parent1MixMenu.IndexChanged += (sender, oldIndex, newIndex) =>
        {
            Parent1MixActivated();
        };
        AncestrySubMenu.AddItem(Parent1MixMenu);

        Parent2MixMenu = new UIMenuNumericScrollerItem<float>("Set Parent 2 Mix", "Select percent of parent ID 2 to use", 0.0f, 1.0f, 0.1f);
        Parent2MixMenu.Activated += (sender, selectedItem) =>
        {
            Parent2MixActivated();
        };
        Parent2MixMenu.IndexChanged += (sender, oldIndex, newIndex) =>
        {
            Parent2MixActivated();
        };
        AncestrySubMenu.AddItem(Parent2MixMenu);
    }
    private void Parent1Activated()
    {
        PedCustomizer.WorkingVariation.HeadBlendData.skinFirst = Parent1IDMenu.SelectedItem.HeadID;
        PedCustomizer.WorkingVariation.HeadBlendData.shapeFirst = Parent1IDMenu.SelectedItem.HeadID;
        PedCustomizer.OnVariationChanged();
        //OnVariationChanged();
    }
    private void Parent2Activated()
    {
        PedCustomizer.WorkingVariation.HeadBlendData.skinSecond = Parent2IDMenu.SelectedItem.HeadID;
        PedCustomizer.WorkingVariation.HeadBlendData.shapeSecond = Parent2IDMenu.SelectedItem.HeadID;
        PedCustomizer.OnVariationChanged();
        //OnVariationChanged();
    }
    private void Parent1MixActivated()
    {
        float newMix = Parent1MixMenu.Value;
        PedCustomizer.WorkingVariation.HeadBlendData.shapeMix = newMix;
        PedCustomizer.WorkingVariation.HeadBlendData.skinMix = 1.0f - newMix;
        Parent2MixMenu.Value = 1.0f - newMix;
        PedCustomizer.OnVariationChanged();
        //OnVariationChanged();
    }
    private void Parent2MixActivated()
    {
        float newMix = Parent2MixMenu.Value;
        PedCustomizer.WorkingVariation.HeadBlendData.skinMix = newMix;
        PedCustomizer.WorkingVariation.HeadBlendData.shapeMix = 1.0f - newMix;
        Parent1MixMenu.Value = 1.0f - newMix;
        PedCustomizer.OnVariationChanged();
        //OnVariationChanged();
    }
    private void SetupHairMenu()
    {
        HairSubMenu.Clear();
        RandomizeHair = new UIMenuItem("Randomize Hair", "Set random hair (use components to select manually)");
        RandomizeHair.Activated += (sender, selectedItem) =>
        {
            RandomizePedHair();
        };
        HairSubMenu.AddItem(RandomizeHair);

        HairPrimaryColorMenu = new UIMenuListScrollerItem<ColorLookup>("Set Primary Hair Color", "Select primary hair color (requires head data)", ColorList);
        HairPrimaryColorMenu.Activated += (sender, selectedItem) =>
        {
            SetPrimaryHairColor(HairPrimaryColorMenu.SelectedItem.ColorID);
        };
        HairPrimaryColorMenu.IndexChanged += (sender, e, selectedItem) =>
        {
            SetPrimaryHairColor(HairPrimaryColorMenu.SelectedItem.ColorID);
        };
        HairSubMenu.AddItem(HairPrimaryColorMenu);
        HairSecondaryColorMenu = new UIMenuListScrollerItem<ColorLookup>("Set Secondary Hair Color", "Select secondary hair color (requires head data)", ColorList);
        HairSecondaryColorMenu.Activated += (sender, selectedItem) =>
        {
            SetSecondaryHairColor(HairSecondaryColorMenu.SelectedItem.ColorID);
        };
        HairSecondaryColorMenu.IndexChanged += (sender, e, selectedItem) =>
        {
            SetSecondaryHairColor(HairSecondaryColorMenu.SelectedItem.ColorID);
        };
        HairSubMenu.AddItem(HairSecondaryColorMenu);

        HairFashionComponenet = new FashionComponent(2, "Hair");
        HairFashionComponenet.CombineCustomizeMenu(MenuPool, HairSubMenu, PedCustomizer.ModelPed, PedCustomizer);


    }
    private void SetupFaceMenu()
    {
        FaceSubMenu.Clear();
        foreach (HeadOverlayData ho in HeadOverlayLookups)
        {
            UIMenu overlayHeaderMenu = MenuPool.AddSubMenu(FaceSubMenu, ho.Part);
            overlayHeaderMenu.SetBannerType(EntryPoint.LSRedColor);
            overlayHeaderMenu.TitleText = $"{ho.Part}";

            int TotalItems = NativeFunction.Natives.xCF1CE768BB43480E<int>(ho.OverlayID);
            UIMenuNumericScrollerItem<int> OverlayIndexMenu = new UIMenuNumericScrollerItem<int>($"Index", $"Modify index", -1, TotalItems - 1, 1);
            OverlayIndexMenu.Formatter = v => v == -1 ? "None" : "Overlay " + v.ToString();
            if (ho.Index != 255)
            {
                OverlayIndexMenu.Value = ho.Index;
            }
            else
            {
                OverlayIndexMenu.Value = -1;
            }
            OverlayIndexMenu.Activated += (sender, selecteditem) =>
            {
                FaceIndexActivated(ho.OverlayID, OverlayIndexMenu);
            };
            OverlayIndexMenu.IndexChanged += (sender, oldIndex, newIndex) =>
            {
                FaceIndexActivated(ho.OverlayID, OverlayIndexMenu);
            };
            overlayHeaderMenu.AddItem(OverlayIndexMenu);

            UIMenuListScrollerItem<ColorLookup> PrimaryColorMenu = new UIMenuListScrollerItem<ColorLookup>("Set Primary Color", "Select primary color", ColorList);
            PrimaryColorMenu.Activated += (sender, selecteditem) =>
            {
                FacePrimaryColorActivated(ho.OverlayID, PrimaryColorMenu);
            };
            PrimaryColorMenu.IndexChanged += (sender, oldIndex, newIndex) =>
            {
                FacePrimaryColorActivated(ho.OverlayID, PrimaryColorMenu);
            };
            overlayHeaderMenu.AddItem(PrimaryColorMenu);

            UIMenuListScrollerItem<ColorLookup> SecondaryColorMenu = new UIMenuListScrollerItem<ColorLookup>("Set Secondary Color", "Select secondary color", ColorList);
            SecondaryColorMenu.Activated += (sender, selecteditem) =>
            {
                FaceSecondaryColorActivated(ho.OverlayID, SecondaryColorMenu);
            };
            SecondaryColorMenu.IndexChanged += (sender, oldIndex, newIndex) =>
            {
                FaceSecondaryColorActivated(ho.OverlayID, SecondaryColorMenu);
            };
            overlayHeaderMenu.AddItem(SecondaryColorMenu);

            UIMenuNumericScrollerItem<float> OpacityMenu = new UIMenuNumericScrollerItem<float>($"Opacity", $"Modify opacity", 0.0f, 1.0f, 0.1f);
            OpacityMenu.Value = 1.0f;
            OpacityMenu.Formatter = v => v.ToString("P0");
            OpacityMenu.Activated += (sender, selecteditem) =>
            {
                FaceOpacityActivated(ho.OverlayID, OpacityMenu);
            };
            OpacityMenu.IndexChanged += (sender, oldIndex, newIndex) =>
            {
                FaceOpacityActivated(ho.OverlayID, OpacityMenu);
            };
            overlayHeaderMenu.AddItem(OpacityMenu);
            OverlayMenus.Add(new OverlayMenuGroup(ho.OverlayID,OverlayIndexMenu,PrimaryColorMenu,SecondaryColorMenu,OpacityMenu));
        }
    }
    private void FaceOpacityActivated(int OverlayID, UIMenuNumericScrollerItem<float> OpacityMenu)
    {
        AddOverlay(OverlayID);
        HeadOverlayData toChange = PedCustomizer.WorkingVariation.HeadOverlays.FirstOrDefault(x => x.OverlayID == OverlayID);
        if (toChange != null)
        {
            EntryPoint.WriteToConsole("OpacityMenu FOUND OVERLAY TO CHANGE");
            toChange.Opacity = OpacityMenu.Value;
            PedCustomizer.WorkingVariation.ApplyToPed(PedCustomizer.ModelPed);
           // OnVariationChanged();
        }
    }
    private void FaceSecondaryColorActivated(int OverlayID, UIMenuListScrollerItem<ColorLookup> SecondaryColorMenu)
    {
        AddOverlay(OverlayID);
        HeadOverlayData toChange = PedCustomizer.WorkingVariation.HeadOverlays.FirstOrDefault(x => x.OverlayID == OverlayID);
        if (toChange != null)
        {
            toChange.SecondaryColor = SecondaryColorMenu.SelectedItem.ColorID;
            PedCustomizer.WorkingVariation.ApplyToPed(PedCustomizer.ModelPed);
            //OnVariationChanged();
        }
    }
    private void FacePrimaryColorActivated(int OverlayID, UIMenuListScrollerItem<ColorLookup> PrimaryColorMenu)
    {
        AddOverlay(OverlayID);
        HeadOverlayData toChange = PedCustomizer.WorkingVariation.HeadOverlays.FirstOrDefault(x => x.OverlayID == OverlayID);
        if (toChange != null)
        {
            toChange.PrimaryColor = PrimaryColorMenu.SelectedItem.ColorID;
            PedCustomizer.WorkingVariation.ApplyToPed(PedCustomizer.ModelPed);
            //OnVariationChanged();
        }
    }
    private void FaceIndexActivated(int OverlayID, UIMenuNumericScrollerItem<int> OverlayIndexMenu)
    {
        AddOverlay(OverlayID);
        HeadOverlayData toChange = PedCustomizer.WorkingVariation.HeadOverlays.FirstOrDefault(x => x.OverlayID == OverlayID);
        if (toChange != null)
        {
            toChange.Index = OverlayIndexMenu.Value;
            PedCustomizer.WorkingVariation.ApplyToPed(PedCustomizer.ModelPed);
            //OnVariationChanged();
        }
    }
    private void AddOverlay(int id)
    {
        EntryPoint.WriteToConsole($"AddOverlay id {id}");
        if (!PedCustomizer.WorkingVariation.HeadOverlays.Any(x=> x.OverlayID == id))
        {
            HeadOverlayData hod = HeadOverlayLookups.FirstOrDefault(x => x.OverlayID == id);
            if(hod != null)
            {
                HeadOverlayData myOverlay = new HeadOverlayData(hod.OverlayID, hod.Part) { ColorType = hod.ColorType };
                PedCustomizer.WorkingVariation.HeadOverlays.Add(myOverlay);
                EntryPoint.WriteToConsole($"AddOverlay ");
            }
        }
    }
    private void SetPrimaryHairColor(int newIndex)
    {
        PedCustomizer.WorkingVariation.PrimaryHairColor = newIndex;
        if (PedCustomizer.PedModelIsFreeMode)
        {
            NativeFunction.Natives.x4CFFC65454C93A49(PedCustomizer.ModelPed, PedCustomizer.WorkingVariation.PrimaryHairColor, PedCustomizer.WorkingVariation.SecondaryHairColor);
            EntryPoint.WriteToConsole($"PedSwapCustomeMenu Hair Color Changed {PedCustomizer.WorkingVariation.PrimaryHairColor} {PedCustomizer.WorkingVariation.SecondaryHairColor}", 5);
        }
        //OnVariationChanged();
    }
    private void SetSecondaryHairColor(int newIndex)
    {
        PedCustomizer.WorkingVariation.SecondaryHairColor = newIndex;
        if (PedCustomizer.PedModelIsFreeMode)
        {
            NativeFunction.Natives.x4CFFC65454C93A49(PedCustomizer.ModelPed, PedCustomizer.WorkingVariation.PrimaryHairColor, PedCustomizer.WorkingVariation.SecondaryHairColor);
            EntryPoint.WriteToConsole($"PedSwapCustomeMenu Hair Color Changed {PedCustomizer.WorkingVariation.PrimaryHairColor} {PedCustomizer.WorkingVariation.SecondaryHairColor}", 5);
        }
        //OnVariationChanged();
    }
    private void RandomizePedHair()
    {
        if (PedCustomizer.ModelPed.Exists() && PedCustomizer.PedModelIsFreeMode)
        {
            //PedCustomizer.WorkingVariation.ApplyToPed(PedCustomizer.ModelPed);
            RandomizeHairStyle();
            //PedCustomizer.WorkingVariation.ApplyToPed(PedCustomizer.ModelPed);
            PedCustomizer.OnVariationChanged();
           // GameFiber.Yield();
            //OnVariationChanged();
        }    
    }
    private void RandomizePedHead()
    {
        if (PedCustomizer.ModelPed.Exists() && PedCustomizer.PedModelIsFreeMode)
        {
            RandomizeOverlay();
            RandomizeHeadblend();
            //PedCustomizer.WorkingVariation.ApplyToPed(PedCustomizer.ModelPed);
            PedCustomizer.OnVariationChanged();
            //GameFiber.Yield();
            //OnVariationChanged();
        }
    }
    private void RandomizeHairStyle()
    {
        int DrawableID = RandomItems.GetRandomNumberInt(0, NativeFunction.Natives.GET_NUMBER_OF_PED_DRAWABLE_VARIATIONS<int>(PedCustomizer.ModelPed, 2));
        int TextureID = RandomItems.GetRandomNumberInt(0, NativeFunction.Natives.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS<int>(PedCustomizer.ModelPed, 2, DrawableID) - 1);
        PedCustomizer.WorkingVariation.PrimaryHairColor = RandomItems.GetRandomNumberInt(0, ColorList.Count());
        PedCustomizer.WorkingVariation.SecondaryHairColor = RandomItems.GetRandomNumberInt(0, ColorList.Count());
        PedComponent hairComponent = PedCustomizer.WorkingVariation.Components.FirstOrDefault(x => x.ComponentID == 2);
        if (hairComponent == null)
        {
            PedCustomizer.WorkingVariation.Components.Add(new PedComponent(2, DrawableID, TextureID, 0));
        }
        else
        {
            hairComponent.DrawableID = DrawableID;
            hairComponent.TextureID = TextureID;
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
        //OnVariationChanged();
    }
    private void RandomizeOverlay()
    {
        PedCustomizer.WorkingVariation.HeadOverlays.Clear();
        foreach (HeadOverlayData ho in HeadOverlayLookups)// PedCustomizer.WorkingVariation.HeadOverlays)
        {
            int TotalItems = NativeFunction.Natives.xCF1CE768BB43480E<int>(ho.OverlayID);
            int Index = RandomItems.GetRandomNumberInt(-1, TotalItems - 1);
            float Opacity = RandomItems.GetRandomNumber(0.0f, 1.0f);
            int PrimaryColor = RandomItems.GetRandomNumberInt(0, ColorList.Count());
            int SecondaryColor = RandomItems.GetRandomNumberInt(0, ColorList.Count());
            PedCustomizer.WorkingVariation.HeadOverlays.Add(new HeadOverlayData(ho.OverlayID, ho.Part) { Index = Index, Opacity = Opacity, PrimaryColor = PrimaryColor, SecondaryColor = SecondaryColor });
        }
        //OnVariationChanged();
    }
    public void OnVariationChanged()
    {
        if (PedCustomizer.WorkingVariation != null)
        {
            if(PedCustomizer.WorkingVariation.Components != null)
            {
                foreach(PedComponent pc in PedCustomizer.WorkingVariation.Components)
                {
                    if(pc.ComponentID == 2)
                    {
                        HairFashionComponenet.SetCurrent(pc.DrawableID, pc.TextureID);
                    }
                }
            }
            if (HeadList != null && PedCustomizer.WorkingVariation.HeadBlendData != null)
            {
                HeadLookup Parent1IDMenuHead = HeadList.FirstOrDefault(x => x.HeadID == PedCustomizer.WorkingVariation.HeadBlendData.skinFirst);
                if (Parent1IDMenuHead != null)
                {
                    Parent1IDMenu.SelectedItem = Parent1IDMenuHead;
                }
                HeadLookup Parent2IDMenuHead = HeadList.FirstOrDefault(x => x.HeadID == PedCustomizer.WorkingVariation.HeadBlendData.skinSecond);
                if (Parent2IDMenuHead != null)
                {
                    Parent2IDMenu.SelectedItem = Parent2IDMenuHead;
                }
            }
            if(PedCustomizer.WorkingVariation.HeadOverlays != null)
            {
                foreach(HeadOverlayData hod in PedCustomizer.WorkingVariation.HeadOverlays)
                {
                    OverlayMenuGroup overlayStuff = OverlayMenus.FirstOrDefault(x => x.OverlayID == hod.OverlayID);
                    if(overlayStuff != null)
                    {
                        EntryPoint.WriteToConsole($"OnVariationChanged {hod.OverlayID} Updating Boxes");

                        if (hod.Index != 255)
                        {
                            overlayStuff.OverlayIndexMenu.Value = hod.Index;
                        }
                        else
                        {
                            overlayStuff.OverlayIndexMenu.Value = -1;
                        }
                        overlayStuff.PrimaryColorMenu.SelectedItem = ColorList.FirstOrDefault(x=> x.ColorID ==  hod.PrimaryColor);
                        overlayStuff.SecondaryColorMenu.SelectedItem = ColorList.FirstOrDefault(x => x.ColorID == hod.SecondaryColor);
                        overlayStuff.OpacityMenu.Value = hod.Opacity;
                    }
                }
            }
            if (PedCustomizer.WorkingVariation.HeadBlendData != null)
            {
                Parent1IDMenu.Index = PedCustomizer.WorkingVariation.HeadBlendData.shapeFirst == -1 ? 0 : PedCustomizer.WorkingVariation.HeadBlendData.shapeFirst;
                Parent2IDMenu.Index = PedCustomizer.WorkingVariation.HeadBlendData.shapeSecond == -1 ? 0 : PedCustomizer.WorkingVariation.HeadBlendData.shapeSecond;
                Parent1MixMenu.Value = PedCustomizer.WorkingVariation.HeadBlendData.shapeMix == -1 ? 0 : PedCustomizer.WorkingVariation.HeadBlendData.shapeMix;
                Parent2MixMenu.Value = PedCustomizer.WorkingVariation.HeadBlendData.skinMix == -1 ? 0 : PedCustomizer.WorkingVariation.HeadBlendData.skinMix;
            }
            HairPrimaryColorMenu.Index = PedCustomizer.WorkingVariation.PrimaryHairColor == -1 ? 0 : PedCustomizer.WorkingVariation.PrimaryHairColor;
            HairSecondaryColorMenu.Index = PedCustomizer.WorkingVariation.SecondaryHairColor == -1 ? 0 : PedCustomizer.WorkingVariation.SecondaryHairColor;     
        }
    }
    public void OnModelChanged()
    {
        SetHeadEnabledStatus();
        SetupHairMenu();
        SetupAncestryMenu();
        SetupFaceMenu();
        OnVariationChanged();
    }
}

