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
    private List<ColorLookup> ColorList;
    private int currentPrimaryColor;
    private int currentSecondaryColor;
    private ISettingsProvideable Settings;

    public ChangeHaircutProcess(ILocationInteractable player, BarberShop barberShop, PedExt hairstylist, ISettingsProvideable settings)
    {
        Player = player;
        BarberShop = barberShop;
        Hairstylist = hairstylist;
        Settings= settings;
    }
    public void SetAnimPositions(Vector3 animEnterPosition,Vector3 animEnterRotation)
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

        //Reset Offset for current model in pedswap
        Player.PedSwap.ResetOffsetForCurrentModel();

        AddSubMenus();
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
    private void AddSubMenus()
    {
        HaircutsSubMenu = MenuPool.AddSubMenu(InteractionMenu, $"Hairstyles");
        HaircutsSubMenuItem = InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1];
        HaircutsSubMenu.OnMenuOpen += (sender) =>
        {

        };
        HaircutsSubMenu.OnIndexChange += (sender, newIndex) =>
        {
            SetPreview(newIndex,2);
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
        AddHairMenu();
        if(Player.CharacterModelIsFreeMode)
        {
            AddHairColorMenu();
        }

        BeardsSubMenu = MenuPool.AddSubMenu(InteractionMenu, $"Beards");
        BeardsSubMenuItem = InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1];
        BeardsSubMenu.OnMenuOpen += (sender) =>
        {

        };
        BeardsSubMenu.OnIndexChange += (sender, newIndex) =>
        {
            SetPreview(newIndex, 1);
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
        AddBeardMenu();

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

        HairPrimaryColorMenu = new UIMenuListScrollerItem<ColorLookup>("Set Primary Hair Color", "Select primary hair color (requires head data)", ColorList) { SelectedItem = ColorList.FirstOrDefault(x=> x.ColorID == currentPrimaryColor) };
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
        HairSecondaryColorMenu = new UIMenuListScrollerItem<ColorLookup>("Set Secondary Hair Color", "Select secondary hair color (requires head data)",ColorList) { SelectedItem = ColorList.FirstOrDefault(x => x.ColorID == currentSecondaryColor) };
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
                DoHaircutAnimation();
                EntryPoint.WriteToConsole("HAIRCUT ANIM DONE");
                Player.BankAccounts.GiveMoney(-1 * cost, true);
                BarberShop.PlaySuccessSound();
                BarberShop.DisplayMessage("~g~Purchased", $"Thank you for purchase");

            }
        };
        HairColorSubMenu.AddItem(SetStyle1);





    }
    private void SetSecondaryHairColor(int colorID)
    {
        NativeFunction.Natives.x4CFFC65454C93A49(Player.Character, currentPrimaryColor, currentSecondaryColor);
    }
    private void SetPrimaryHairColor(int colorID)
    {
        NativeFunction.Natives.x4CFFC65454C93A49(Player.Character, currentPrimaryColor, currentSecondaryColor);
    }
    private void SetPreview(int newIndex,int ComponentID)
    {
        //PedVariation prewviewvariation = Extensions.DeepCopy(Player.CurrentModelVariation);

        //PedComponent hairComponenet = prewviewvariation.Components.FirstOrDefault(x => x.ComponentID == ComponentID);
        //if (hairComponenet == null)
        //{
        //    hairComponenet = new PedComponent(ComponentID, newIndex, 0);
        //    prewviewvariation.Components.Add(hairComponenet);

        //}
        //else
        //{
        //    hairComponenet.DrawableID = newIndex;
        //}
        //prewviewvariation.ApplyToPed(Player.Character);

        NativeFunction.Natives.SET_PED_COMPONENT_VARIATION(Player.Character, ComponentID, newIndex, 0, 0);


    }
    private void AddHairMenu()
    {

            AddRegularHair();


    }
    private void AddBeardMenu()
    {
        if (Player.CharacterModelIsFreeMode)
        {
            AddFreeemodeBeard();
        }
        else
        {
            AddRegularBeard();
        }
    }
    private void AddRegularHair()
    {
        int NumberOfDrawables = NativeFunction.Natives.GET_NUMBER_OF_PED_DRAWABLE_VARIATIONS<int>(Player.Character, 2);
        for (int DrawableNumber = 0; DrawableNumber < NumberOfDrawables; DrawableNumber++)
        {
            int cost = 45;
            string drawableName = $"Style #{DrawableNumber+1}";
            int drawableID = DrawableNumber;
            UIMenuItem SetStyle1 = new UIMenuItem(drawableName, "Select to purchase this haircut") { RightLabel = $"${cost}" };


            SetStyle1.Activated += (sender, e) =>
            {
                //Charge them and set it as your character variation
                if(Player.BankAccounts.GetMoney(true) < cost)
                {
                    BarberShop.PlayErrorSound();
                    BarberShop.DisplayMessage("~r~Insufficient Funds", "We are sorry, we are unable to complete this transation, as you do not have the required funds");
                }
                else
                {
                    PedComponent hairComponenet = Player.CurrentModelVariation.Components.FirstOrDefault(x => x.ComponentID == 2);
                    if(hairComponenet == null)
                    {
                        hairComponenet = new PedComponent(2, drawableID, 0);
                        Player.CurrentModelVariation.Components.Add(hairComponenet);

                    }
                    else
                    {
                        hairComponenet.DrawableID = drawableID;
                    }
                    DoHaircutAnimation();
                    EntryPoint.WriteToConsole("HAIRCUT ANIM DONE");
                    Player.BankAccounts.GiveMoney(-1 * cost, true);
                    BarberShop.PlaySuccessSound();
                    BarberShop.DisplayMessage("~g~Purchased", $"Thank you for purchase");

                }
            };
            HaircutsSubMenu.AddItem(SetStyle1);
        }
    }
    private void AddRegularBeard()
    {
        int NumberOfDrawables = NativeFunction.Natives.GET_NUMBER_OF_PED_DRAWABLE_VARIATIONS<int>(Player.Character, 1);
        for (int DrawableNumber = 0; DrawableNumber < NumberOfDrawables; DrawableNumber++)
        {
            int cost = 35;
            string drawableName = $"Style #{DrawableNumber + 1}";
            int drawableID = DrawableNumber;
            UIMenuItem SetStyle1 = new UIMenuItem(drawableName, "Select to purchase this beard style") { RightLabel = $"${cost}" };
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
                    PedComponent hairComponenet = Player.CurrentModelVariation.Components.FirstOrDefault(x => x.ComponentID == 1);
                    if (hairComponenet == null)
                    {
                        hairComponenet = new PedComponent(1, drawableID, 0);
                        Player.CurrentModelVariation.Components.Add(hairComponenet);

                    }
                    else
                    {
                        hairComponenet.DrawableID = drawableID;
                    }
                    DoHaircutAnimation();
                    EntryPoint.WriteToConsole("BEARD ANIM DONE");
                    Player.BankAccounts.GiveMoney(-1 * cost, true);
                    BarberShop.PlaySuccessSound();
                    BarberShop.DisplayMessage("~g~Purchased", $"Thank you for purchase");
                }
            };
            BeardsSubMenu.AddItem(SetStyle1);
        }
    }
    private void DoHaircutAnimation()
    {
        bool hasApplied = false;
        if (Hairstylist != null && Hairstylist.Pedestrian.Exists())
        {
            EntryPoint.WriteToConsole("HAIRCUT ANIM START STYLIST");
            NativeFunction.CallByName<bool>("TASK_PLAY_ANIM_ADVANCED", Hairstylist.Pedestrian, "misshair_shop@hair_dressers", "keeper_hair_cut_a", AnimEnterPosition.X, AnimEnterPosition.Y, AnimEnterPosition.Z, AnimEnterRotation.X, AnimEnterRotation.Y, AnimEnterRotation.Z, 1000f, -1000f, -1, 5642, 0.0f, 2, 1);
            uint GameTimeStarted = Game.GameTime;
            GameFiber.Sleep(1000);
            while(Game.GameTime - GameTimeStarted <= 10000)
            {
                if(!Hairstylist.Pedestrian.Exists())
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
                GameFiber.Yield();
            }
            if (Hairstylist != null && Hairstylist.Pedestrian.Exists())
            {
                GameFiber.Sleep(2000);
                NativeFunction.CallByName<bool>("TASK_PLAY_ANIM_ADVANCED", Hairstylist.Pedestrian, "misshair_shop@hair_dressers", "keeper_base", AnimEnterPosition.X, AnimEnterPosition.Y, AnimEnterPosition.Z, AnimEnterRotation.X, AnimEnterRotation.Y, AnimEnterRotation.Z, 8.0f, -8.0f, -1, 5641, 0.0f, 2, 1);
            }
        }
        else
        {
            Player.CurrentModelVariation.ApplyToPed(Player.Character);
        }
    }
    private void AddFreeemodeHair()
    {

    }
    private void AddFreeemodeBeard()
    {

    }
}

