using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ExtensionsMethods;
using Extensions = ExtensionsMethods.Extensions;

public class ChangeHaircutProcess
{
    private BarberShop BarberShop;
    private PedExt Hairstylist;
    private ILocationInteractable Player;
    private MenuPool MenuPool;
    private UIMenu InteractionMenu;
    private UIMenu HaircutsSubMenu;
    private UIMenuItem HaircutsSubMenuItem;
    private Texture BannerImage;
    private UIMenu BeardsSubMenu;
    private UIMenuItem BeardsSubMenuItem;
    private Vector3 AnimEnterPosition;
    private Vector3 AnimEnterRotation;
    private UIMenu HairColorSubMenu;
    private UIMenuItem HairColorSubMenuItem;
    private UIMenuListScrollerItem<ColorLookup> HairPrimaryColorMenu;
    private UIMenuListScrollerItem<ColorLookup> HairSecondaryColorMenu;
    private List<HeadOverlayData> HeadOverlayLookups;
    private List<ColorLookup> ColorList;
    private int currentPrimaryColor;
    private int currentSecondaryColor;
    private ISettingsProvideable Settings;
    private List<MenuLookup> HairLookup = new List<MenuLookup>();
    private List<MenuLookup> BeardLookup = new List<MenuLookup>();
    private UIMenuNumericScrollerItem<float> OpacityMenu;
    private UIMenuListScrollerItem<ColorLookup> SecondaryColorMenu;
    private UIMenuListScrollerItem<ColorLookup> PrimaryColorMenu;
    private UIMenuNumericScrollerItem<int> OverlayIndexMenu;

    public ChangeHaircutProcess(ILocationInteractable player, BarberShop barberShop, PedExt hairstylist, ISettingsProvideable settings)
    {
        Player = player;
        BarberShop = barberShop;
        Hairstylist = hairstylist;
        Settings = settings;
    }
    public void SetAnimPositions(Vector3 animEnterPosition, Vector3 animEnterRotation)
    {
        AnimEnterPosition = animEnterPosition;
        AnimEnterRotation = animEnterRotation;
    }
    public void Start(MenuPool menuPool, UIMenu interactionMenu)
    {
        MenuPool = menuPool;
        InteractionMenu = interactionMenu;

        InteractionMenu.OnMenuOpen += (sender) =>
        {
            Player.IsTransacting = true;
        };
        InteractionMenu.OnMenuClose += (sender) =>
        {
            Player.IsTransacting = false;
            //Add
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
        //Reset Offset for current model in pedswap
        Player.PedSwap.ResetOffsetForCurrentModel();
        AddHairMenu();
        if (Player.CharacterModelIsFreeMode)
        {
            AddHairColorMenu();
        }
        AddBeardMenu();
        InteractionMenu.Visible = true;
        EntryPoint.WriteToConsole("HAIRCUT INTERCATION SHOWING MENU");
    }
    public void Dispose()
    {
        Game.RawFrameRender -= (s, e) => MenuPool.DrawBanners(e.Graphics);
        //Add offset if required
        if (Settings.SettingsManager.PedSwapSettings.AliasPedAsMainCharacter && !Player.CharacterModelIsPrimaryCharacter)
        {
            Player.PedSwap.AddOffset();
        }
    }

    private void SetSecondaryHairColor(int colorID)
    {
        NativeFunction.Natives.x4CFFC65454C93A49(Player.Character, currentPrimaryColor, currentSecondaryColor);
    }
    private void SetPrimaryHairColor(int colorID)
    {
        NativeFunction.Natives.x4CFFC65454C93A49(Player.Character, currentPrimaryColor, currentSecondaryColor);
    }
    private void SetComponenetPreview(int newIndex, int ComponentID, int TextureID)
    {
        if (newIndex < 0 || ComponentID < 0 || TextureID < 0)
        {
            return;
        }
        EntryPoint.WriteToConsole($"SET PREVIEW comp:{ComponentID} draw:{newIndex} text:{TextureID}");
        NativeFunction.Natives.SET_PED_COMPONENT_VARIATION(Player.Character, ComponentID, newIndex, TextureID, 0);
    }
    private void AddHairMenu()
    {
        HaircutsSubMenu = MenuPool.AddSubMenu(InteractionMenu, $"Hairstyles");
        HaircutsSubMenuItem = InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1];
        HaircutsSubMenu.OnMenuOpen += (sender) =>
        {
            MenuLookup ml = HairLookup.FirstOrDefault(x => x.MenuID == sender.CurrentSelection);
            if (ml != null)
            {
                SetComponenetPreview(ml.DrawableID, 2, ml.TextureID);
            }
        };
        HaircutsSubMenu.OnIndexChange += (sender, newIndex) =>
        {
            MenuLookup ml = HairLookup.FirstOrDefault(x => x.MenuID == sender.CurrentSelection);
            if (ml != null)
            {
                SetComponenetPreview(ml.DrawableID, 2, ml.TextureID);
            }
        };
        HaircutsSubMenu.OnMenuClose += (sender) =>
        {
            Player.CurrentModelVariation.ApplyToPed(Player.Character);
        };
        InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1].Description = $"Choose style";
        if (BarberShop == null)
        {
            return;
        }
        if (BarberShop.HasBannerImage)
        {
            BannerImage = Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\{BarberShop.BannerImagePath}");
            HaircutsSubMenu.SetBannerType(BannerImage);
            Game.RawFrameRender += (s, e) => MenuPool.DrawBanners(e.Graphics);
            EntryPoint.WriteToConsole("BARBER INTERACTION AccountsSubMenu.SetBannerType(BannerImage) RAN");
        }
        AddSelectionItems(2, 45, HaircutsSubMenu, ref HairLookup);
    }
    private void AddHairColorMenu()
    {
        HairColorSubMenu = MenuPool.AddSubMenu(InteractionMenu, $"Hair Color");
        HairColorSubMenuItem = InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1];
        HairColorSubMenu.OnMenuOpen += (sender) =>
        {

        };
        HairColorSubMenu.OnIndexChange += (sender, newIndex) =>
        {
            //SetPreview(newIndex, 2);
        };
        HairColorSubMenu.OnMenuClose += (sender) =>
        {
            Player.CurrentModelVariation.ApplyToPed(Player.Character);
        };
        InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1].Description = $"Choose style";
        if (BarberShop == null)
        {
            return;
        }
        if (BarberShop.HasBannerImage)
        {
            BannerImage = Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\{BarberShop.BannerImagePath}");
            HairColorSubMenu.SetBannerType(BannerImage);
            Game.RawFrameRender += (s, e) => MenuPool.DrawBanners(e.Graphics);
            EntryPoint.WriteToConsole("BARBER INTERACTION AccountsSubMenu.SetBannerType(BannerImage) RAN");
        }
        currentPrimaryColor = Player.CurrentModelVariation.PrimaryHairColor;
        currentSecondaryColor = Player.CurrentModelVariation.SecondaryHairColor;

        HairPrimaryColorMenu = new UIMenuListScrollerItem<ColorLookup>("Set Primary Hair Color", "Select primary hair color (requires head data)", ColorList) { SelectedItem = ColorList.FirstOrDefault(x => x.ColorID == currentPrimaryColor) };
        HairPrimaryColorMenu.Activated += (sender, selectedItem) =>
        {
            currentPrimaryColor = HairPrimaryColorMenu.SelectedItem.ColorID;
            SetPrimaryHairColor(HairPrimaryColorMenu.SelectedItem.ColorID);
        };
        HairPrimaryColorMenu.IndexChanged += (sender, e, selectedItem) =>
        {
            currentPrimaryColor = HairPrimaryColorMenu.SelectedItem.ColorID;
            SetPrimaryHairColor(HairPrimaryColorMenu.SelectedItem.ColorID);
        };
        HairColorSubMenu.AddItem(HairPrimaryColorMenu);
        HairSecondaryColorMenu = new UIMenuListScrollerItem<ColorLookup>("Set Secondary Hair Color", "Select secondary hair color (requires head data)", ColorList) { SelectedItem = ColorList.FirstOrDefault(x => x.ColorID == currentSecondaryColor) };
        HairSecondaryColorMenu.Activated += (sender, selectedItem) =>
        {
            currentSecondaryColor = HairSecondaryColorMenu.SelectedItem.ColorID;
            SetSecondaryHairColor(HairSecondaryColorMenu.SelectedItem.ColorID);
        };
        HairSecondaryColorMenu.IndexChanged += (sender, e, selectedItem) =>
        {
            currentSecondaryColor = HairSecondaryColorMenu.SelectedItem.ColorID;
            SetSecondaryHairColor(HairSecondaryColorMenu.SelectedItem.ColorID);
        };
        HairColorSubMenu.AddItem(HairSecondaryColorMenu);

        int cost = 25;
        UIMenuItem SetStyle1 = new UIMenuItem("Purchase", "Select to purchase this hair coloring") { RightLabel = $"${cost}" };
        SetStyle1.Activated += (sender, e) =>
        {
            //Charge them and set it as your character variation
            if (Player.BankAccounts.GetMoney(true) < cost)
            {
                BarberShop.PlayErrorSound();
                BarberShop.DisplayMessage("~r~Insufficient Funds", "We are sorry, we are unable to complete this transation, as you do not have the required funds");
            }
            else
            {
                Player.CurrentModelVariation.PrimaryHairColor = currentPrimaryColor;
                Player.CurrentModelVariation.SecondaryHairColor = currentSecondaryColor;
                DoAnimation(cost);
                EntryPoint.WriteToConsole("HAIRCUT ANIM DONE");


            }
        };
        HairColorSubMenu.AddItem(SetStyle1);
    }
    private void AddBeardMenu()
    {
        BeardsSubMenu = MenuPool.AddSubMenu(InteractionMenu, $"Beards");
        BeardsSubMenuItem = InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1];
        BeardsSubMenu.OnMenuOpen += (sender) =>
        {
            if (Player.CharacterModelIsFreeMode)
            {
                SetFaceOverlay(1, OverlayIndexMenu.Value, OpacityMenu.Value, 1, PrimaryColorMenu.SelectedItem.ColorID, SecondaryColorMenu.SelectedItem.ColorID);
            }
            else
            {
                MenuLookup ml = BeardLookup.FirstOrDefault(x => x.MenuID == sender.CurrentSelection);
                if (ml != null)
                {
                    SetComponenetPreview(ml.DrawableID, 1, ml.TextureID);
                }
            }
        };
        BeardsSubMenu.OnIndexChange += (sender, newIndex) =>
        {
            if (!Player.CharacterModelIsFreeMode)
            {
                MenuLookup ml = BeardLookup.FirstOrDefault(x => x.MenuID == sender.CurrentSelection);
                if (ml != null)
                {
                    SetComponenetPreview(ml.DrawableID, 1, ml.TextureID);
                }
            }
        };
        BeardsSubMenu.OnMenuClose += (sender) =>
        {
            Player.CurrentModelVariation.ApplyToPed(Player.Character);
        };
        InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1].Description = $"Choose style";
        if (BarberShop == null)
        {
            return;
        }
        if (BarberShop.HasBannerImage)
        {
            BannerImage = Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\{BarberShop.BannerImagePath}");
            BeardsSubMenu.SetBannerType(BannerImage);
            Game.RawFrameRender += (s, e) => MenuPool.DrawBanners(e.Graphics);
            EntryPoint.WriteToConsole("BARBER INTERACTION AccountsSubMenu.SetBannerType(BannerImage) RAN");
        }
        if (Player.CharacterModelIsFreeMode)
        {
            AddOverlayItems(new HeadOverlayData(1, "Facial Hair") { ColorType = 1 }, 35,BeardsSubMenu);
        }
        else
        {
            AddSelectionItems(1, 35, BeardsSubMenu, ref BeardLookup);
        }
    }
    private void AddSelectionItems(int componentID, int cost, UIMenu toAdd, ref List<MenuLookup> lookupList)
    {
        int styleNumber = 1;
        lookupList = new List<MenuLookup>();
        int NumberOfDrawables = NativeFunction.Natives.GET_NUMBER_OF_PED_DRAWABLE_VARIATIONS<int>(Player.Character, componentID);
        for (int DrawableNumber = 0; DrawableNumber < NumberOfDrawables; DrawableNumber++)
        {
            int drawableID = DrawableNumber;
            int NumberOfTextureVariations = NativeFunction.Natives.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS<int>(Player.Character, componentID, drawableID);
            for (int TextureNumber = 0; TextureNumber < NumberOfTextureVariations; TextureNumber++)
            {

                string drawableName = $"Style #{styleNumber}";
                int textureID = TextureNumber;
                EntryPoint.WriteToConsole($"NAME: {drawableName} componentID:{componentID} drawableID:{drawableID} TextureNumber:{textureID}");

                lookupList.Add(new MenuLookup(toAdd.MenuItems.Count(), drawableID, textureID));


                UIMenuItem SetStyle1 = new UIMenuItem(drawableName, "Select to purchase this item") { RightLabel = $"${cost}" };

                SetStyle1.Activated += (sender, e) =>
                {
                    if (Player.BankAccounts.GetMoney(true) < cost)
                    {
                        BarberShop.PlayErrorSound();
                        BarberShop.DisplayMessage("~r~Insufficient Funds", "We are sorry, we are unable to complete this transation, as you do not have the required funds");
                    }
                    else
                    {
                        PedComponent existingComponenet = Player.CurrentModelVariation.Components.FirstOrDefault(x => x.ComponentID == componentID);
                        if (existingComponenet == null)
                        {
                            existingComponenet = new PedComponent(componentID, drawableID, textureID);
                            Player.CurrentModelVariation.Components.Add(existingComponenet);
                        }
                        else
                        {
                            existingComponenet.DrawableID = drawableID;
                            existingComponenet.TextureID = textureID;
                        }
                        DoAnimation(cost);
                        EntryPoint.WriteToConsole("ANIM DONE");
                    }
                };
                toAdd.AddItem(SetStyle1);
                styleNumber++;
            }
        }
    }
    private void AddOverlayItems(HeadOverlayData ho, int cost, UIMenu toAdd)
    {
        int TotalItems = NativeFunction.Natives.xCF1CE768BB43480E<int>(ho.OverlayID);
        OverlayIndexMenu = new UIMenuNumericScrollerItem<int>($"Style", $"Modify style", -1, TotalItems - 1, 1);
        OverlayIndexMenu.Formatter = v => v == -1 ? "Clean Shaven" : "#" + v.ToString();
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
            SetFaceOverlay(ho.OverlayID, OverlayIndexMenu.Value,OpacityMenu.Value,ho.ColorType,PrimaryColorMenu.SelectedItem.ColorID,SecondaryColorMenu.SelectedItem.ColorID);
            //SetIndex(ho.OverlayID, OverlayIndexMenu);
        };
        OverlayIndexMenu.IndexChanged += (sender, oldIndex, newIndex) =>
        {
            SetFaceOverlay(ho.OverlayID, OverlayIndexMenu.Value, OpacityMenu.Value, ho.ColorType, PrimaryColorMenu.SelectedItem.ColorID, SecondaryColorMenu.SelectedItem.ColorID);
        };
        toAdd.AddItem(OverlayIndexMenu);

        PrimaryColorMenu = new UIMenuListScrollerItem<ColorLookup>("Color", "Select color", ColorList);
        PrimaryColorMenu.Activated += (sender, selecteditem) =>
        {
            SetFaceOverlay(ho.OverlayID, OverlayIndexMenu.Value, OpacityMenu.Value, ho.ColorType, PrimaryColorMenu.SelectedItem.ColorID, SecondaryColorMenu.SelectedItem.ColorID);
        };
        PrimaryColorMenu.IndexChanged += (sender, oldIndex, newIndex) =>
        {
            SetFaceOverlay(ho.OverlayID, OverlayIndexMenu.Value, OpacityMenu.Value, ho.ColorType, PrimaryColorMenu.SelectedItem.ColorID, SecondaryColorMenu.SelectedItem.ColorID);
        };
        toAdd.AddItem(PrimaryColorMenu);

        SecondaryColorMenu = new UIMenuListScrollerItem<ColorLookup>("Highlight Color", "Select highlight color", ColorList);
        SecondaryColorMenu.Activated += (sender, selecteditem) =>
        {
            SetFaceOverlay(ho.OverlayID, OverlayIndexMenu.Value, OpacityMenu.Value, ho.ColorType, PrimaryColorMenu.SelectedItem.ColorID, SecondaryColorMenu.SelectedItem.ColorID);
        };
        SecondaryColorMenu.IndexChanged += (sender, oldIndex, newIndex) =>
        {
            SetFaceOverlay(ho.OverlayID, OverlayIndexMenu.Value, OpacityMenu.Value, ho.ColorType, PrimaryColorMenu.SelectedItem.ColorID, SecondaryColorMenu.SelectedItem.ColorID);
        };
        toAdd.AddItem(SecondaryColorMenu);

        OpacityMenu = new UIMenuNumericScrollerItem<float>($"Thickness", $"Set thickness", 0.0f, 1.0f, 0.1f);
        OpacityMenu.Value = 1.0f;
        OpacityMenu.Formatter = v => v.ToString("P0");
        OpacityMenu.Activated += (sender, selecteditem) =>
        {
            SetFaceOverlay(ho.OverlayID, OverlayIndexMenu.Value, OpacityMenu.Value, ho.ColorType, PrimaryColorMenu.SelectedItem.ColorID, SecondaryColorMenu.SelectedItem.ColorID);
        };
        OpacityMenu.IndexChanged += (sender, oldIndex, newIndex) =>
        {
            SetFaceOverlay(ho.OverlayID, OverlayIndexMenu.Value, OpacityMenu.Value, ho.ColorType, PrimaryColorMenu.SelectedItem.ColorID, SecondaryColorMenu.SelectedItem.ColorID);
        };
        toAdd.AddItem(OpacityMenu);



        UIMenuItem purchaseStyle = new UIMenuItem("Purchase", "Select to purchase this item") { RightLabel = $"${cost}" };
        purchaseStyle.Activated += (sender, e) =>
        {
            if (Player.BankAccounts.GetMoney(true) < cost)
            {
                BarberShop.PlayErrorSound();
                BarberShop.DisplayMessage("~r~Insufficient Funds", "We are sorry, we are unable to complete this transation, as you do not have the required funds");
            }
            else
            {
                HeadOverlayData existingComponenet = Player.CurrentModelVariation.HeadOverlays.FirstOrDefault(x => x.OverlayID == ho.OverlayID);
                if (existingComponenet == null)
                {
                    existingComponenet = new HeadOverlayData() {
                        OverlayID = ho.OverlayID,
                        Index = OverlayIndexMenu.Value,
                        Opacity = OpacityMenu.Value,
                        ColorType = ho.ColorType,
                        PrimaryColor = PrimaryColorMenu.SelectedItem.ColorID,
                        SecondaryColor = SecondaryColorMenu.SelectedItem.ColorID };
                    Player.CurrentModelVariation.HeadOverlays.Add(existingComponenet);
                    EntryPoint.WriteToConsole($"PURCHASE STYLE ADDED NEW OVERLAY OverlayID:{ho.OverlayID} Index:{OverlayIndexMenu.Value} Opacity:{OpacityMenu.Value} PriColor:{PrimaryColorMenu.SelectedItem.ColorID} SecColor:{SecondaryColorMenu.SelectedItem.ColorID}");
                }
                else
                {
                    existingComponenet.OverlayID = ho.OverlayID;
                    existingComponenet.Index = OverlayIndexMenu.Value; 
                    existingComponenet.Opacity = OpacityMenu.Value;
                    existingComponenet.ColorType = ho.ColorType;
                    existingComponenet.PrimaryColor = PrimaryColorMenu.SelectedItem.ColorID;
                    existingComponenet.SecondaryColor = SecondaryColorMenu.SelectedItem.ColorID;
                    EntryPoint.WriteToConsole($"PURCHASE STYLE UPDATED EXISTING OVERLAY OverlayID:{ho.OverlayID} Index:{OverlayIndexMenu.Value} Opacity:{OpacityMenu.Value} PriColor:{PrimaryColorMenu.SelectedItem.ColorID} SecColor:{SecondaryColorMenu.SelectedItem.ColorID}");
                }
                DoAnimation(cost);
                EntryPoint.WriteToConsole("ANIM DONE");
            }
        };
        toAdd.AddItem(purchaseStyle);
    }
    private void SetFaceOverlay(int OverlayID, int index, float opactiy, int colorType, int primaryColor, int secondaryColor)
    {
        NativeFunction.Natives.SET_PED_HEAD_OVERLAY(Player.Character, OverlayID, index, opactiy);
        NativeFunction.Natives.x497BF74A7B9CB952(Player.Character, OverlayID, colorType, primaryColor, secondaryColor);
    }
    private void DoAnimation(int cost)
    {
        bool hasApplied = false;
        bool hasShownStuff = false;
        Player.BankAccounts.GiveMoney(-1 * cost, true);
        if (Hairstylist != null && Hairstylist.Pedestrian.Exists())
        {
            EntryPoint.WriteToConsole("HAIRCUT ANIM START STYLIST");
            NativeFunction.CallByName<bool>("TASK_PLAY_ANIM_ADVANCED", Hairstylist.Pedestrian, "misshair_shop@hair_dressers", "keeper_hair_cut_a", AnimEnterPosition.X, AnimEnterPosition.Y, AnimEnterPosition.Z, AnimEnterRotation.X, AnimEnterRotation.Y, AnimEnterRotation.Z, 1000f, -1000f, -1, 5642, 0.0f, 2, 1);
            uint GameTimeStarted = Game.GameTime;
            GameFiber.Sleep(1000);
            while (Game.GameTime - GameTimeStarted <= 10000)
            {
                if (!Hairstylist.Pedestrian.Exists())
                {
                    EntryPoint.WriteToConsole("HAIRCUT ANIM NO STYLIST");
                    break;
                }
                float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Hairstylist.Pedestrian, "misshair_shop@hair_dressers", "keeper_hair_cut_a");
                if (AnimationTime >= 1.0f)
                {
                    EntryPoint.WriteToConsole("HAIRCUT ANIM OVER TIME");
                    break;
                }
                if (AnimationTime >= 0.5f && !hasShownStuff)
                {

                    BarberShop.PlaySuccessSound();
                    hasShownStuff = true;
                }
                GameFiber.Yield();
            }
            if (Hairstylist != null && Hairstylist.Pedestrian.Exists())
            {
                GameFiber.Sleep(2000);
                NativeFunction.CallByName<bool>("TASK_PLAY_ANIM_ADVANCED", Hairstylist.Pedestrian, "misshair_shop@hair_dressers", "keeper_base", AnimEnterPosition.X, AnimEnterPosition.Y, AnimEnterPosition.Z, AnimEnterRotation.X, AnimEnterRotation.Y, AnimEnterRotation.Z, 8.0f, -8.0f, -1, 5641, 0.0f, 2, 1);
            }
        }
        //else
        //{
        //    Player.CurrentModelVariation.ApplyToPed(Player.Character);
        //}
    }
    private class MenuLookup
    {
        public MenuLookup(int menuID, int drawableID, int textureID)
        {
            MenuID = menuID;
            DrawableID = drawableID;
            TextureID = textureID;
        }

        public int MenuID { get; set; }
        public int DrawableID { get; set; }
        public int TextureID { get; set; }
    }
}

