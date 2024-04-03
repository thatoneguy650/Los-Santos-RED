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


public class CustomizeFaceMenu
{
    private IPedSwap PedSwap;
    private MenuPool MenuPool;
    private INameProvideable Names;
    private IPedSwappable Player;
    private IEntityProvideable World;
    private ISettingsProvideable Settings;
    private PedCustomizer PedCustomizer;
    private PedCustomizerMenu PedCustomizerMenu;
    private UIMenu HeadSubMenu;

    private UIMenu CustomizeMainMenu;
    private List<ColorLookup> ColorList;
    private UIMenu FaceSubMenu;
    private List<OverlayMenuGroup> OverlayMenus = new List<OverlayMenuGroup>();
    private List<HeadOverlayData> HeadOverlayLookups;
    private List<ColorLookup> EyeColorList;

    public CustomizeFaceMenu(MenuPool menuPool, IPedSwap pedSwap, INameProvideable names, IPedSwappable player, IEntityProvideable world, ISettingsProvideable settings, PedCustomizer pedCustomizer, PedCustomizerMenu pedCustomizerMenu)
    {
        PedSwap = pedSwap;
        MenuPool = menuPool;
        Names = names;
        Player = player;
        World = world;
        Settings = settings;
        PedCustomizer = pedCustomizer;
        PedCustomizerMenu = pedCustomizerMenu;
        ColorList = new List<ColorLookup>()
        {
        //new ColorLookup(0,"Black 1"),
        //new ColorLookup(1,"Black 2"),
        //new ColorLookup(2,"Black 3"),
        //new ColorLookup(3,"Brown 1"),
        //new ColorLookup(4,"Brown 2"),
        //new ColorLookup(5,"Brown 3"),
        //new ColorLookup(6,"Brown 4"),
        //new ColorLookup(7,"Brown 5"),
        //new ColorLookup(8,"Brown 6"),
        //new ColorLookup(9,"Blonde 1"),
        //new ColorLookup(10,"Blonde 2"),
        //new ColorLookup(11,"Blonde 3"),
        //new ColorLookup(12,"Blonde 4"),
        //new ColorLookup(13,"Blonde 5"),
        //new ColorLookup(14,"Blonde 6"),
        //new ColorLookup(15,"Blonde 7"),
        //new ColorLookup(16,"Blonde 8"),
        //new ColorLookup(17,"Blonde 9"),
        //new ColorLookup(18,"Redhead 1"),
        //new ColorLookup(19,"Redhead 2"),
        //new ColorLookup(20,"Redhead 3"),
        //new ColorLookup(21,"Redhead 4"),
        //new ColorLookup(22,"Orange 1"),
        //new ColorLookup(23,"Orange 2"),
        //new ColorLookup(24,"Orange 3"),
        //new ColorLookup(25,"Orange 4"),
        //new ColorLookup(26,"Grey 1"),
        //new ColorLookup(27,"Grey 2"),
        //new ColorLookup(28,"White 1"),
        //new ColorLookup(29,"White 2"),
        //new ColorLookup(30,"Purple 1"),
        //new ColorLookup(31,"Purple 2"),
        //new ColorLookup(32,"Purple 3"),
        //new ColorLookup(33,"Pink 1"),
        //new ColorLookup(34,"Pink 2"),
        //new ColorLookup(35,"Pink 3"),
        //new ColorLookup(36,"Blue 4"),
        //new ColorLookup(37,"Blue 5"),
        //new ColorLookup(38,"Blue 6"),
        //new ColorLookup(39,"Green 1"),
        //new ColorLookup(40,"Green 2"),
        //new ColorLookup(41,"Green 3"),
        //new ColorLookup(42,"Green 4"),
        //new ColorLookup(43,"Green 5"),
        //new ColorLookup(44,"Green 6"),
        //new ColorLookup(45,"Yellow 1"),
        //new ColorLookup(46,"Yellow 2"),
        //new ColorLookup(47,"Yellow 3"),
        //new ColorLookup(48,"Orange 5"),
        //new ColorLookup(49,"Orange 6"),
        //new ColorLookup(50,"Orange 7"),
        //new ColorLookup(51,"Orange 8"),
        //new ColorLookup(52,"Red 1"),
        //new ColorLookup(53,"Red 1"),
        //new ColorLookup(54,"Red 1"),
        //new ColorLookup(55,"Brown 7"),
        //new ColorLookup(56,"Brown 8"),
        //new ColorLookup(57,"Brown 9"),
        //new ColorLookup(58,"Brown 10"),
        //new ColorLookup(59,"Brown 11"),
        //new ColorLookup(60,"Brown 12"),
            new ColorLookup(1,"Black"),
            new ColorLookup(2,"Dark Gray"),
            new ColorLookup(3,"Medium Gray"),
            new ColorLookup(4,"Darkest Brown"),
            new ColorLookup(5,"Dark Brown"),
            new ColorLookup(6,"Brown"),
            new ColorLookup(7,"Light Brown"),
            new ColorLookup(8,"Lighter Brown"),
            new ColorLookup(9,"Lightest Brown"),
            new ColorLookup(10,"Faded Brown"),
            new ColorLookup(11,"Faded Blonde"),
            new ColorLookup(12,"Lightest Blonde"),
            new ColorLookup(13,"Lighter Blonde"),
            new ColorLookup(14,"Light Blonde"),
            new ColorLookup(15,"White Blonde"),
            new ColorLookup(16,"Grayish Brown"),
            new ColorLookup(17,"Redish Brown"),
            new ColorLookup(18,"Red Brown"),
            new ColorLookup(19,"Dark Red"),
            new ColorLookup(20,"Red"),
            new ColorLookup(21,"Very Red"),
            new ColorLookup(22,"Vibrant Red"),
            new ColorLookup(23,"Orangeish Red"),
            new ColorLookup(24,"Faded Red"),
            new ColorLookup(25,"Faded Orange"),
            new ColorLookup(26,"Gray"),
            new ColorLookup(27,"Light Gray"),
            new ColorLookup(28,"Lighter Gray"),
            new ColorLookup(29,"Lightest Gray"),
            new ColorLookup(30,"Dark Purple"),
            new ColorLookup(31,"Purple"),
            new ColorLookup(32,"Light Purple"),
            new ColorLookup(33,"Violet"),
            new ColorLookup(34,"Vibrant Violet"),
            new ColorLookup(35,"Candy Pink"),
            new ColorLookup(36,"Light Pink"),
            new ColorLookup(37,"Cyan"),
            new ColorLookup(38,"Blue"),
            new ColorLookup(39,"Dark Blue"),
            new ColorLookup(40,"Green"),
            new ColorLookup(41,"Emerald"),
            new ColorLookup(42,"Oil Slick"),
            new ColorLookup(43,"Shiney Green"),
            new ColorLookup(44,"Vibrant Green"),
            new ColorLookup(45,"Green"),
            new ColorLookup(46,"Bleach Blonde"),
            new ColorLookup(47,"Golden Blonde"),
            new ColorLookup(48,"Orange Blonde"),
            new ColorLookup(49,"Orange"),
            new ColorLookup(50,"Vibrant Orange"),
            new ColorLookup(51,"Shiny Orange"),
            new ColorLookup(52,"Dark Orange"),
            new ColorLookup(53,"Red"),
            new ColorLookup(54,"Dark Red"),
            new ColorLookup(55,"Very Dark Red"),
            new ColorLookup(56,"Black"),
            new ColorLookup(57,"Black"),
            new ColorLookup(58,"Black"),
            new ColorLookup(59,"Black"),
            new ColorLookup(60,"Black"),
        new ColorLookup(61,"Black 4"),
        new ColorLookup(62,"Unk 1"),
        new ColorLookup(63,"Unk 2"),
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

        EyeColorList = new List<ColorLookup>()
        {
        new ColorLookup(0,"Black"),
        new ColorLookup(1,"Light Blue/Green"),
        new ColorLookup(2,"Dark Blue"),
        new ColorLookup(3,"Brown"),
        new ColorLookup(4,"Darker Brown"),
        new ColorLookup(5,"Light Brown"),
        new ColorLookup(6,"Blue"),
        new ColorLookup(7,"Light Blue"),
        new ColorLookup(8,"Pink"),
        new ColorLookup(9,"Yellow"),
        new ColorLookup(10,"Purple"),
        new ColorLookup(11,"Black"),
        new ColorLookup(12,"Dark Green"),
        new ColorLookup(13,"Light Brown"),
        new ColorLookup(14,"Yellow & Black"),
        new ColorLookup(15,"Light Spiral"),
        new ColorLookup(16,"Shiny Red"),
        new ColorLookup(17,"Shiny Red Blue"),
        new ColorLookup(18,"Half Black Light Blue"),
        new ColorLookup(19,"White W/ Red Outline"),
        new ColorLookup(20,"Green Snake"),
        new ColorLookup(21,"Red Snake"),
        new ColorLookup(22,"Dark Blue Snake"),
        new ColorLookup(23,"Dark Yellow"),
        new ColorLookup(24,"Bright Yellow"),
        new ColorLookup(25,"All Black"),
        new ColorLookup(26,"Red Small Pupil"),
        new ColorLookup(27,"Devil Blue"),
        new ColorLookup(28,"White Small Pupil"),
        new ColorLookup(29,"Glossed over"),
        };

    }
    public void Create(UIMenu headSubMenu)
    {
        HeadSubMenu = headSubMenu;
        FaceSubMenu = MenuPool.AddSubMenu(HeadSubMenu, "Overlays");
        FaceSubMenu.SubtitleText = "OVERLAY";
        HeadSubMenu.MenuItems[HeadSubMenu.MenuItems.Count() - 1].Description = "Change the face of the current ped";
        HeadSubMenu.MenuItems[HeadSubMenu.MenuItems.Count() - 1].RightBadge = UIMenuItem.BadgeStyle.Mask;
        FaceSubMenu.SetBannerType(EntryPoint.LSRedColor);
        FaceSubMenu.InstructionalButtonsEnabled = false;
    }
    public void Setup()
    {
        FaceSubMenu.Clear();

        UIMenuListScrollerItem<ColorLookup> EyeColorMenu = new UIMenuListScrollerItem<ColorLookup>("Set Eye Color", "Select eye color", EyeColorList);
        EyeColorMenu.Activated += (sender, selecteditem) =>
        {
            SetEyeColor(EyeColorMenu);
        };
        EyeColorMenu.IndexChanged += (sender, oldIndex, newIndex) =>
        {
            SetEyeColor(EyeColorMenu);
        };
        FaceSubMenu.AddItem(EyeColorMenu);



        OverlayMenus.Clear();
        foreach (HeadOverlayData ho in HeadOverlayLookups)
        {
            UIMenu overlayHeaderMenu = MenuPool.AddSubMenu(FaceSubMenu, ho.Part);
            overlayHeaderMenu.SetBannerType(EntryPoint.LSRedColor);
            overlayHeaderMenu.InstructionalButtonsEnabled = false;
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
                SetIndex(ho.OverlayID, OverlayIndexMenu);
            };
            OverlayIndexMenu.IndexChanged += (sender, oldIndex, newIndex) =>
            {
                SetIndex(ho.OverlayID, OverlayIndexMenu);
            };
            overlayHeaderMenu.AddItem(OverlayIndexMenu);

            UIMenuListScrollerItem<ColorLookup> PrimaryColorMenu = new UIMenuListScrollerItem<ColorLookup>("Set Primary Color", "Select primary color", ColorList);
            PrimaryColorMenu.Activated += (sender, selecteditem) =>
            {
                SetPrimaryColor(ho.OverlayID, PrimaryColorMenu);
            };
            PrimaryColorMenu.IndexChanged += (sender, oldIndex, newIndex) =>
            {
                SetPrimaryColor(ho.OverlayID, PrimaryColorMenu);
            };
            overlayHeaderMenu.AddItem(PrimaryColorMenu);

            UIMenuListScrollerItem<ColorLookup> SecondaryColorMenu = new UIMenuListScrollerItem<ColorLookup>("Set Secondary Color", "Select secondary color", ColorList);
            SecondaryColorMenu.Activated += (sender, selecteditem) =>
            {
                SetSecondaryColor(ho.OverlayID, SecondaryColorMenu);
            };
            SecondaryColorMenu.IndexChanged += (sender, oldIndex, newIndex) =>
            {
                SetSecondaryColor(ho.OverlayID, SecondaryColorMenu);
            };
            overlayHeaderMenu.AddItem(SecondaryColorMenu);

            UIMenuNumericScrollerItem<float> OpacityMenu = new UIMenuNumericScrollerItem<float>($"Opacity", $"Modify opacity", 0.0f, 1.0f, 0.1f);
            OpacityMenu.Value = 1.0f;
            OpacityMenu.Formatter = v => v.ToString("P0");
            OpacityMenu.Activated += (sender, selecteditem) =>
            {
                SetOpacity(ho.OverlayID, OpacityMenu);
            };
            OpacityMenu.IndexChanged += (sender, oldIndex, newIndex) =>
            {
                SetOpacity(ho.OverlayID, OpacityMenu);
            };
            overlayHeaderMenu.AddItem(OpacityMenu);
            OverlayMenus.Add(new OverlayMenuGroup(ho.OverlayID, OverlayIndexMenu, PrimaryColorMenu, SecondaryColorMenu, OpacityMenu));
        }

        OnOverlayValuesChanged();
    }

    private void SetEyeColor(UIMenuListScrollerItem<ColorLookup> eyeColorMenu)
    {
        if (PedCustomizerMenu.IsProgramicallySettingFieldValues)
        {
            return;
        }
        PedCustomizer.WorkingVariation.EyeColor = eyeColorMenu.SelectedItem.ColorID;
        PedCustomizer.WorkingVariation.ApplyToPed(PedCustomizer.ModelPed);
    }

    private void SetIndex(int OverlayID, UIMenuNumericScrollerItem<int> OverlayIndexMenu)
    {
        if (PedCustomizerMenu.IsProgramicallySettingFieldValues)
        {
            return;
        }
        AddOverlay(OverlayID);
        HeadOverlayData toChange = PedCustomizer.WorkingVariation.HeadOverlays.FirstOrDefault(x => x.OverlayID == OverlayID);
        if (toChange != null)
        {
            toChange.Index = OverlayIndexMenu.Value;
            PedCustomizer.WorkingVariation.ApplyToPed(PedCustomizer.ModelPed);
        }
    }
    private void SetPrimaryColor(int OverlayID, UIMenuListScrollerItem<ColorLookup> PrimaryColorMenu)
    {
        if (PedCustomizerMenu.IsProgramicallySettingFieldValues)
        {
            return;
        }
        AddOverlay(OverlayID);
        HeadOverlayData toChange = PedCustomizer.WorkingVariation.HeadOverlays.FirstOrDefault(x => x.OverlayID == OverlayID);
        if (toChange != null)
        {
            toChange.PrimaryColor = PrimaryColorMenu.SelectedItem.ColorID;
            PedCustomizer.WorkingVariation.ApplyToPed(PedCustomizer.ModelPed);
        }
    }
    private void SetSecondaryColor(int OverlayID, UIMenuListScrollerItem<ColorLookup> SecondaryColorMenu)
    {
        if (PedCustomizerMenu.IsProgramicallySettingFieldValues)
        {
            return;
        }
        AddOverlay(OverlayID);
        HeadOverlayData toChange = PedCustomizer.WorkingVariation.HeadOverlays.FirstOrDefault(x => x.OverlayID == OverlayID);
        if (toChange != null)
        {
            toChange.SecondaryColor = SecondaryColorMenu.SelectedItem.ColorID;
            PedCustomizer.WorkingVariation.ApplyToPed(PedCustomizer.ModelPed);
        }
    }
    private void SetOpacity(int OverlayID, UIMenuNumericScrollerItem<float> OpacityMenu)
    {
        if (PedCustomizerMenu.IsProgramicallySettingFieldValues)
        {
            return;
        }
        AddOverlay(OverlayID);
        HeadOverlayData toChange = PedCustomizer.WorkingVariation.HeadOverlays.FirstOrDefault(x => x.OverlayID == OverlayID);
        if (toChange != null)
        {
            //EntryPoint.WriteToConsoleTestLong("OpacityMenu FOUND OVERLAY TO CHANGE");
            toChange.Opacity = OpacityMenu.Value;
            PedCustomizer.WorkingVariation.ApplyToPed(PedCustomizer.ModelPed);
        }
    }
    private void AddOverlay(int id)
    {
        //EntryPoint.WriteToConsoleTestLong($"AddOverlay id {id}");
        if (!PedCustomizer.WorkingVariation.HeadOverlays.Any(x => x.OverlayID == id))
        {
            HeadOverlayData hod = HeadOverlayLookups.FirstOrDefault(x => x.OverlayID == id);
            if (hod != null)
            {
                HeadOverlayData myOverlay = new HeadOverlayData(hod.OverlayID, hod.Part) { ColorType = hod.ColorType };
                PedCustomizer.WorkingVariation.HeadOverlays.Add(myOverlay);
                //EntryPoint.WriteToConsoleTestLong($"AddOverlay ");
            }
        }
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
        OnOverlayValuesChanged();
    }
    private void OnOverlayValuesChanged()
    {
        if (PedCustomizerMenu.IsProgramicallySettingFieldValues)
        {
            return;
        }
        foreach (HeadOverlayData hod in PedCustomizer.WorkingVariation.HeadOverlays)
        {
            OverlayMenuGroup overlayStuff = OverlayMenus.FirstOrDefault(x => x.OverlayID == hod.OverlayID);
            if (overlayStuff != null)
            {
                //EntryPoint.WriteToConsoleTestLong($"OnOverlayValuesChanged ID {hod.OverlayID} Part {hod.Part} Index {hod.Index} Opacity {hod.Opacity}");
                if (hod.Index != 255)
                {
                    overlayStuff.OverlayIndexMenu.Value = hod.Index;
                }
                else
                {
                    overlayStuff.OverlayIndexMenu.Value = -1;
                }
                overlayStuff.PrimaryColorMenu.SelectedItem = ColorList.FirstOrDefault(x => x.ColorID == hod.PrimaryColor);
                overlayStuff.SecondaryColorMenu.SelectedItem = ColorList.FirstOrDefault(x => x.ColorID == hod.SecondaryColor);
                overlayStuff.OpacityMenu.Value = hod.Opacity;
            }
        }
        //EntryPoint.WriteToConsoleTestLong("OnOverlayValuesChanged Executed");
    }
}

