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


public class CustomizeHairMenu
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
    private UIMenu HairSubMenu;
    private UIMenu CustomizeMainMenu;


    private List<ColorLookup> ColorList;
    private UIMenuItem RandomizeHair;
    private UIMenuListScrollerItem<ColorLookup> HairPrimaryColorMenu;
    private UIMenuListScrollerItem<ColorLookup> HairSecondaryColorMenu;
    private FashionComponent HairFashionComponenet;
    public CustomizeHairMenu(MenuPool menuPool, IPedSwap pedSwap, INameProvideable names, IPedSwappable player, IEntityProvideable world, ISettingsProvideable settings, PedCustomizer pedCustomizer, PedCustomizerMenu pedCustomizerMenu)
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
        new ColorLookup(61,"Black 4"),
        new ColorLookup(62,"Unk 1"),
        new ColorLookup(63,"Unk 2"),
        };
    }
    public void Create(UIMenu headSubMenu)
    {
        HeadSubMenu = headSubMenu;
        HairSubMenu = MenuPool.AddSubMenu(HeadSubMenu, "Hair");
        HairSubMenu.SubtitleText = "HAIR";
        HeadSubMenu.MenuItems[HeadSubMenu.MenuItems.Count() - 1].Description = "Change the hair of the current ped";
        HeadSubMenu.MenuItems[HeadSubMenu.MenuItems.Count() - 1].RightBadge = UIMenuItem.BadgeStyle.Barber;
        HairSubMenu.SetBannerType(EntryPoint.LSRedColor);
        HairSubMenu.InstructionalButtonsEnabled = false;
        HairSubMenu.OnMenuOpen += (sender) =>
        {

        };
    }
    public void Setup()
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

        OnHairValuesChanged();
    }
    private void SetPrimaryHairColor(int newIndex)
    {
        if (PedCustomizerMenu.IsProgramicallySettingFieldValues)
        {
            return;
        }
        PedCustomizer.WorkingVariation.PrimaryHairColor = newIndex;
        if (PedCustomizer.PedModelIsFreeMode)
        {
            NativeFunction.Natives.x4CFFC65454C93A49(PedCustomizer.ModelPed, PedCustomizer.WorkingVariation.PrimaryHairColor, PedCustomizer.WorkingVariation.SecondaryHairColor);
            //EntryPoint.WriteToConsole($"PedSwapCustomeMenu Hair Color Changed {PedCustomizer.WorkingVariation.PrimaryHairColor} {PedCustomizer.WorkingVariation.SecondaryHairColor}", 5);
        }
    }
    private void SetSecondaryHairColor(int newIndex)
    {
        if (PedCustomizerMenu.IsProgramicallySettingFieldValues)
        {
            return;
        }
        PedCustomizer.WorkingVariation.SecondaryHairColor = newIndex;
        if (PedCustomizer.PedModelIsFreeMode)
        {
            NativeFunction.Natives.x4CFFC65454C93A49(PedCustomizer.ModelPed, PedCustomizer.WorkingVariation.PrimaryHairColor, PedCustomizer.WorkingVariation.SecondaryHairColor);
            //EntryPoint.WriteToConsole($"PedSwapCustomeMenu Hair Color Changed {PedCustomizer.WorkingVariation.PrimaryHairColor} {PedCustomizer.WorkingVariation.SecondaryHairColor}", 5);
        }
    }
    private void RandomizePedHair()
    {
        if (PedCustomizer.ModelPed.Exists() && PedCustomizer.PedModelIsFreeMode)
        {
            RandomizeHairStyle();
            PedCustomizer.OnVariationChanged();
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
        OnHairValuesChanged();
    }
    private void OnHairValuesChanged()
    {
        if (HairPrimaryColorMenu != null && PedCustomizer.WorkingVariation != null)
        {
            HairPrimaryColorMenu.Index = PedCustomizer.WorkingVariation.PrimaryHairColor == -1 ? 0 : PedCustomizer.WorkingVariation.PrimaryHairColor;
        }
        if (HairSecondaryColorMenu != null && PedCustomizer.WorkingVariation != null)
        {
            HairSecondaryColorMenu.Index = PedCustomizer.WorkingVariation.SecondaryHairColor == -1 ? 0 : PedCustomizer.WorkingVariation.SecondaryHairColor;
        }

        if (PedCustomizer.WorkingVariation.Components != null)
        {
            foreach (PedComponent pc in PedCustomizer.WorkingVariation.Components)
            {
                if (pc.ComponentID == 2)
                {
                    HairFashionComponenet.SetCurrent(pc.DrawableID, pc.TextureID);
                }
            }
        }
        //EntryPoint.WriteToConsoleTestLong("OnHairValuesChanged Executed");
    }
}

