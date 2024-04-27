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
using Microsoft.Win32;

public class SalonPurchaseMenu
{
    private BarberShop BarberShop;
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
    private IClothesNames ClothesNames;
    private SalonInteract HaircutInteract;
    private UIMenuItem setHairColoringMenu;


    public SalonPurchaseMenu(ILocationInteractable player, BarberShop barberShop, SalonInteract haircutInteract, PedExt hairstylist,Rage.Object scissors, ISettingsProvideable settings, Vector3 animEnterPosition, Vector3 animEnterRotation, IClothesNames clothesNames)
    {
        Player = player;
        BarberShop = barberShop;
        Settings = settings;
        AnimEnterPosition = animEnterPosition;
        AnimEnterRotation = animEnterRotation;
        ClothesNames = clothesNames;
        HaircutInteract = haircutInteract;
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
        Setup();
        //Reset Offset for current model in pedswap
        if(Player == null)
        {
            EntryPoint.WriteToConsole("PLAYER IS NULL");
        }
        if (Player.PedSwap == null)
        {
            EntryPoint.WriteToConsole("PLAYER PEDSWAP IS NULL");
        }
        Player.PedSwap.ResetOffsetForCurrentModel();
        if (BarberShop != null && BarberShop.AllowsHaircuts)
        {
            AddHairMenu();

            if (Player.CharacterModelIsFreeMode)
            {
                AddHairColorMenu();
            }
        }
        if (BarberShop != null && BarberShop.AllowsBeards)
        {
            AddBeardMenu();
        }
        if (BarberShop != null && BarberShop.AllowsMakeup)
        {
            AddOverlayMenu(2, "Eyebrows", 1, BarberShop.PedVariationShopMenu.StandardMakeupPrice, BarberShop.PedVariationShopMenu.PremiumMakeupExtra, "None", "Strength", false);
            AddOverlayMenu(4, "Makeup", 1, BarberShop.PedVariationShopMenu.StandardMakeupPrice, BarberShop.PedVariationShopMenu.PremiumMakeupExtra, "None", "Strength", false);
            AddOverlayMenu(5, "Blush", 2, BarberShop.PedVariationShopMenu.StandardMakeupPrice, BarberShop.PedVariationShopMenu.PremiumMakeupExtra, "None", "Strength", false);
            AddOverlayMenu(8, "Lipstick", 2, BarberShop.PedVariationShopMenu.StandardMakeupPrice, BarberShop.PedVariationShopMenu.PremiumMakeupExtra, "None", "Strength", false);
        }
        InteractionMenu.Visible = true;
        EntryPoint.WriteToConsole("HAIRCUT INTERCATION SHOWING MENU");
    }
    public void Dispose()
    {
        Game.RawFrameRender -= (s, e) => MenuPool.DrawBanners(e.Graphics);
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
    private void SetFaceOverlay(int OverlayID, int index, float opactiy, int colorType, int primaryColor, int secondaryColor)
    {
        NativeFunction.Natives.SET_PED_HEAD_OVERLAY(Player.Character, OverlayID, index, opactiy);
        NativeFunction.Natives.x497BF74A7B9CB952(Player.Character, OverlayID, colorType, primaryColor, secondaryColor);
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
        int baseHaircutCost = 45;
        int premiumHaircutCostExtra = 15;
        if (BarberShop != null)
        {
            baseHaircutCost = Player.IsMale ? BarberShop.PedVariationShopMenu.StandardMaleHaircutPrice : BarberShop.PedVariationShopMenu.StandardFemaleHaircutPrice;
            premiumHaircutCostExtra = BarberShop.PedVariationShopMenu.PremiumHaircutExtra;
        }
        AddSelectionItems(2, baseHaircutCost, premiumHaircutCostExtra, HaircutsSubMenu, ref HairLookup);
    }
    private void AddHairColorMenu()
    {
        UIMenuCheckboxItem addHighlight = new UIMenuCheckboxItem("Add Highlight", false, "Select to add highlights");
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
        addHighlight.Checked = true;
        addHighlight.CheckboxEvent += (sender, Checked) =>
        {
            HairSecondaryColorMenu.Enabled = !HairSecondaryColorMenu.Enabled;
            if(!Checked)
            {
                currentSecondaryColor = currentPrimaryColor;
            }
            SetSecondaryHairColor(currentSecondaryColor);
            setHairColoringMenu.RightLabel = $"${GetHairColorPrice(Checked)}";
        };

        HairColorSubMenu.AddItem(addHighlight);

        HairPrimaryColorMenu = new UIMenuListScrollerItem<ColorLookup>("Color", "Select hair color", ColorList) { SelectedItem = ColorList.FirstOrDefault(x => x.ColorID == currentPrimaryColor) };
        //HairPrimaryColorMenu.Activated += (sender, selectedItem) =>
        //{
        //    currentPrimaryColor = HairPrimaryColorMenu.SelectedItem.ColorID;
        //    SetPrimaryHairColor(HairPrimaryColorMenu.SelectedItem.ColorID);
        //};
        HairPrimaryColorMenu.IndexChanged += (sender, e, selectedItem) =>
        {
            currentPrimaryColor = HairPrimaryColorMenu.SelectedItem.ColorID;

            if (!addHighlight.Enabled)
            {
                currentSecondaryColor = HairPrimaryColorMenu.SelectedItem.ColorID;
            }
            else
            {
                currentSecondaryColor = HairSecondaryColorMenu.SelectedItem.ColorID;
            }

            SetPrimaryHairColor(HairPrimaryColorMenu.SelectedItem.ColorID);
        };
        HairColorSubMenu.AddItem(HairPrimaryColorMenu);
        HairSecondaryColorMenu = new UIMenuListScrollerItem<ColorLookup>("Highlight Color", "Select highlight hair color", ColorList) { SelectedItem = ColorList.FirstOrDefault(x => x.ColorID == currentSecondaryColor) };
        //HairSecondaryColorMenu.Activated += (sender, selectedItem) =>
        //{
        //    currentSecondaryColor = HairSecondaryColorMenu.SelectedItem.ColorID;

        //    if (!addHighlight.Enabled)
        //    {
        //        currentSecondaryColor = HairPrimaryColorMenu.SelectedItem.ColorID;
        //    }

        //    SetSecondaryHairColor(currentSecondaryColor);
        //};
        HairSecondaryColorMenu.IndexChanged += (sender, e, selectedItem) =>
        {
            if (!addHighlight.Enabled)
            {
                currentSecondaryColor = HairPrimaryColorMenu.SelectedItem.ColorID;
            }
            else
            {
                currentSecondaryColor = HairSecondaryColorMenu.SelectedItem.ColorID;
            }
            SetSecondaryHairColor(currentSecondaryColor);
        };
        HairColorSubMenu.AddItem(HairSecondaryColorMenu);

        int TotalCost = GetHairColorPrice(addHighlight.Checked);

        setHairColoringMenu = new UIMenuItem("Purchase", "Select to purchase this hair coloring") { RightLabel = $"${TotalCost}" };
        setHairColoringMenu.Activated += (sender, e) =>
        {
            int currentPrice = GetHairColorPrice(addHighlight.Checked);
            //Charge them and set it as your character variation
            if (Player.BankAccounts.GetMoney(true) < currentPrice)
            {
                BarberShop.PlayErrorSound();
                BarberShop.DisplayMessage("~r~Insufficient Funds", "We are sorry, we are unable to complete this transation, as you do not have the required funds");
            }
            else
            {
                Player.CurrentModelVariation.PrimaryHairColor = currentPrimaryColor;
                Player.CurrentModelVariation.SecondaryHairColor = currentSecondaryColor;
                HaircutInteract?.PlayHaircutAnimation(currentPrice);
                EntryPoint.WriteToConsole("HAIRCUT ANIM DONE");
            }
        };
        HairColorSubMenu.AddItem(setHairColoringMenu);
    }
    private int GetHairColorPrice(bool addHighlights)
    {
        int TotalCost = 45;
        if (BarberShop != null)
        {
            TotalCost = BarberShop.PedVariationShopMenu.StandardHairColoringPrice;
            if (addHighlights)
            {
                TotalCost += BarberShop.PedVariationShopMenu.PremiumColoringExtra;
            }
        }
        return TotalCost;
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
            AddBeardOverlayItems(new HeadOverlayData(1, "Facial Hair") { ColorType = 1 }, BarberShop.PedVariationShopMenu.StandardBeardTrimPrice,BarberShop.PedVariationShopMenu.PremiumBearTrimExtra, BeardsSubMenu, "Clean Shaven","Thickness");
        }
        else
        {
            AddSelectionItems(1, BarberShop.PedVariationShopMenu.StandardBeardTrimPrice,BarberShop.PedVariationShopMenu.PremiumBearTrimExtra, BeardsSubMenu, ref BeardLookup);
        }
    }
    private void AddSelectionItems(int componentID, int baseCost,int extraCost, UIMenu toAdd, ref List<MenuLookup> lookupList)
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






                if (Player.CharacterModelIsFreeMode)
                {
                    string newName = ClothesNames.GetName(false, componentID, drawableID, TextureNumber, Player.Gender);
                    if(!string.IsNullOrEmpty(newName))
                    {
                        drawableName = newName;
                    }
                }






                int textureID = TextureNumber;

                int TotalCost = baseCost;
                if (drawableID >= 15)
                {
                    TotalCost += extraCost;
                }
                if (BarberShop != null && BarberShop.PedVariationShopMenu != null)
                {
                    PedComponentShopMenu pcsm = BarberShop.PedVariationShopMenu.GetDrawableCost(Player, componentID, drawableID, textureID);
                    if (pcsm != null)
                    {
                        TotalCost = pcsm.Price;
                        EntryPoint.WriteToConsole($"SPECIAL COMPONENT COST ITEM ADDED {componentID} {drawableID} {textureID} ${pcsm.Price}");
                        if (!string.IsNullOrEmpty(pcsm.DebugName))
                        {
                            drawableName = pcsm.DebugName;
                        }
                    }
                }






                EntryPoint.WriteToConsole($"NAME: {drawableName} componentID:{componentID} drawableID:{drawableID} TextureNumber:{textureID}");
                lookupList.Add(new MenuLookup(toAdd.MenuItems.Count(), drawableID, textureID));



                UIMenuItem SetStyle1 = new UIMenuItem(drawableName, "Select to purchase this item") { RightLabel = $"${TotalCost}" };
                SetStyle1.Activated += (sender, e) =>
                {
                    if (Player.BankAccounts.GetMoney(true) < TotalCost)
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
                        HaircutInteract?.PlayHaircutAnimation(TotalCost);
                        EntryPoint.WriteToConsole("ANIM DONE");
                    }
                };
                toAdd.AddItem(SetStyle1);
                styleNumber++;
            }
        }
    }
    private void AddBeardOverlayItems(HeadOverlayData ho, int baseCost,int extraCost, UIMenu toAdd, string noneName, string opacityName)
    {
        int TotalItems = NativeFunction.Natives.xCF1CE768BB43480E<int>(ho.OverlayID);
        OverlayIndexMenu = new UIMenuNumericScrollerItem<int>($"Style", $"Modify style", -1, TotalItems - 1, 1);
        HashSet<FashionItemLookup> overlayName =  ClothesNames.PedOverlays.Where(x => x.ItemID == ho.OverlayID).ToHashSet();
        OverlayIndexMenu.Formatter = v => v == -1 ? noneName : overlayName.Where(x => x.DrawableID == v).FirstOrDefault() != null ? overlayName.Where(x=> x.DrawableID == v).FirstOrDefault()?.Name : "#" + v.ToString();  
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
        };
        OverlayIndexMenu.IndexChanged += (sender, oldIndex, newIndex) =>
        {
            SetFaceOverlay(ho.OverlayID, OverlayIndexMenu.Value, OpacityMenu.Value, ho.ColorType, PrimaryColorMenu.SelectedItem.ColorID, SecondaryColorMenu.SelectedItem.ColorID);
        };
        toAdd.AddItem(OverlayIndexMenu);

        bool useHighlights = true;


        UIMenuCheckboxItem addHighlight = new UIMenuCheckboxItem("Add Highlight", true);
        addHighlight.CheckboxEvent += (sender, Checked) =>
        {
            SecondaryColorMenu.Enabled = Checked;
            useHighlights = Checked;
        };
        //toAdd.AddItem(addHighlight);
        useHighlights = false;

        PrimaryColorMenu = new UIMenuListScrollerItem<ColorLookup>("Color", "Select color", ColorList);
        PrimaryColorMenu.IndexChanged += (sender, oldIndex, newIndex) =>
        {
            SetFaceOverlay(ho.OverlayID, OverlayIndexMenu.Value, OpacityMenu.Value, ho.ColorType, PrimaryColorMenu.SelectedItem.ColorID, useHighlights ? SecondaryColorMenu.SelectedItem.ColorID : PrimaryColorMenu.SelectedItem.ColorID);
        };
        toAdd.AddItem(PrimaryColorMenu);

        SecondaryColorMenu = new UIMenuListScrollerItem<ColorLookup>("Highlight Color", "Select highlight color", ColorList);
        SecondaryColorMenu.IndexChanged += (sender, oldIndex, newIndex) =>
        {
            SetFaceOverlay(ho.OverlayID, OverlayIndexMenu.Value, OpacityMenu.Value, ho.ColorType, PrimaryColorMenu.SelectedItem.ColorID, useHighlights ? SecondaryColorMenu.SelectedItem.ColorID : PrimaryColorMenu.SelectedItem.ColorID);
        };
        //toAdd.AddItem(SecondaryColorMenu);



        OpacityMenu = new UIMenuNumericScrollerItem<float>(opacityName, $"Set {opacityName.ToLower()}", 0.0f, 1.0f, 0.1f);
        OpacityMenu.Value = 1.0f;
        OpacityMenu.Formatter = v => v.ToString("P0");
        OpacityMenu.Activated += (sender, selecteditem) =>
        {
            SetFaceOverlay(ho.OverlayID, OverlayIndexMenu.Value, OpacityMenu.Value, ho.ColorType, PrimaryColorMenu.SelectedItem.ColorID, useHighlights ? SecondaryColorMenu.SelectedItem.ColorID : PrimaryColorMenu.SelectedItem.ColorID);
        };
        OpacityMenu.IndexChanged += (sender, oldIndex, newIndex) =>
        {
            SetFaceOverlay(ho.OverlayID, OverlayIndexMenu.Value, OpacityMenu.Value, ho.ColorType, PrimaryColorMenu.SelectedItem.ColorID, useHighlights ? SecondaryColorMenu.SelectedItem.ColorID : PrimaryColorMenu.SelectedItem.ColorID);
        };
        toAdd.AddItem(OpacityMenu);




        int TotalCost = baseCost;

        if (BarberShop != null && BarberShop.PedVariationShopMenu != null)
        {
            int customizedCost = BarberShop.PedVariationShopMenu.GetOverlayCost(Player, ho.OverlayID);
            if (customizedCost != -1)
            {
                TotalCost = customizedCost;
                EntryPoint.WriteToConsole($"SPECIAL OVERLAY COST ITEM ADDED {ho.OverlayID} ${customizedCost}");
            }
        }




        UIMenuItem purchaseStyle = new UIMenuItem("Purchase", "Select to purchase this item") { RightLabel = $"${TotalCost}" };
        purchaseStyle.Activated += (sender, e) =>
        {
            if (Player.BankAccounts.GetMoney(true) < TotalCost)
            {
                BarberShop.PlayErrorSound();
                BarberShop.DisplayMessage("~r~Insufficient Funds", "We are sorry, we are unable to complete this transation, as you do not have the required funds");
            }
            else
            {
                HeadOverlayData existingComponenet = Player.CurrentModelVariation.HeadOverlays.FirstOrDefault(x => x.OverlayID == ho.OverlayID);
                if (existingComponenet == null)
                {
                    existingComponenet = new HeadOverlayData() 
                    {
                        OverlayID = ho.OverlayID,
                        Index = OverlayIndexMenu.Value,
                        Opacity = OpacityMenu.Value,
                        ColorType = ho.ColorType,
                        PrimaryColor = PrimaryColorMenu.SelectedItem.ColorID,
                        SecondaryColor = useHighlights ? SecondaryColorMenu.SelectedItem.ColorID : PrimaryColorMenu.SelectedItem.ColorID,
                    };
                    Player.CurrentModelVariation.HeadOverlays.Add(existingComponenet);
                    //EntryPoint.WriteToConsole($"PURCHASE STYLE ADDED NEW OVERLAY OverlayID:{ho.OverlayID} Index:{OverlayIndexMenu.Value} Opacity:{OpacityMenu.Value} PriColor:{PrimaryColorMenu.SelectedItem.ColorID} SecColor:{SecondaryColorMenu.SelectedItem.ColorID}");
                }
                else
                {
                    existingComponenet.OverlayID = ho.OverlayID;
                    existingComponenet.Index = OverlayIndexMenu.Value; 
                    existingComponenet.Opacity = OpacityMenu.Value;
                    existingComponenet.ColorType = ho.ColorType;
                    existingComponenet.PrimaryColor = PrimaryColorMenu.SelectedItem.ColorID;
                    existingComponenet.SecondaryColor = useHighlights ? SecondaryColorMenu.SelectedItem.ColorID : PrimaryColorMenu.SelectedItem.ColorID;
                    //EntryPoint.WriteToConsole($"PURCHASE STYLE UPDATED EXISTING OVERLAY OverlayID:{ho.OverlayID} Index:{OverlayIndexMenu.Value} Opacity:{OpacityMenu.Value} PriColor:{PrimaryColorMenu.SelectedItem.ColorID} SecColor:{SecondaryColorMenu.SelectedItem.ColorID}");
                }
                HaircutInteract?.PlayHaircutAnimation(TotalCost);
                //EntryPoint.WriteToConsole("ANIM DONE");
            }
        };
        toAdd.AddItem(purchaseStyle);
    }
    private void AddOverlayMenu(int overlayID, string menuName, int colorType, int standardCost, int extraCost, string noneName, string opacityName, bool hasSecondaryColor)
    {
        if (!Player.CharacterModelIsFreeMode)
        {
            return;
        }

        HeadOverlayData ho = new HeadOverlayData(overlayID, menuName) { ColorType = colorType };


        int TotalItems = NativeFunction.Natives.xCF1CE768BB43480E<int>(ho.OverlayID);
        List<MenuLookup> overlayLookup = new List<MenuLookup>();
        UIMenu overLaySubMenu = MenuPool.AddSubMenu(InteractionMenu, menuName);
        UIMenuItem MakeupSubMenuItem = InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1];


        UIMenuNumericScrollerItem<int> OverlayIndexMenuNEW = new UIMenuNumericScrollerItem<int>($"Style", $"Modify style", -1, TotalItems - 1, 1);
        UIMenuListScrollerItem<ColorLookup> PrimaryColorMenuNEW = new UIMenuListScrollerItem<ColorLookup>("Color", "Select color", ColorList);
        UIMenuListScrollerItem<ColorLookup> SecondaryColorMenuNEW = new UIMenuListScrollerItem<ColorLookup>("Highlight Color", "Select highlight color", ColorList);
        UIMenuNumericScrollerItem<float> OpacityMenuNEW = new UIMenuNumericScrollerItem<float>($"{opacityName}", $"Set {opacityName.ToLower()}", 0.0f, 1.0f, 0.1f);


        overLaySubMenu.OnMenuOpen += (sender) =>
        {
            if (Player.CharacterModelIsFreeMode)
            {
                SetFaceOverlay(overlayID, OverlayIndexMenuNEW.Value, OpacityMenuNEW.Value, 1, PrimaryColorMenuNEW.SelectedItem.ColorID, SecondaryColorMenuNEW.SelectedItem.ColorID);
            }
        };
        overLaySubMenu.OnMenuClose += (sender) =>
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
            overLaySubMenu.SetBannerType(BannerImage);
            Game.RawFrameRender += (s, e) => MenuPool.DrawBanners(e.Graphics);
            EntryPoint.WriteToConsole("BARBER INTERACTION AccountsSubMenu.SetBannerType(BannerImage) RAN");
        }

        HashSet<FashionItemLookup> overlayName = ClothesNames.PedOverlays.Where(x => x.ItemID == ho.OverlayID).ToHashSet();
        OverlayIndexMenuNEW.Formatter = v => v == -1 ? noneName : overlayName.Where(x => x.DrawableID == v).FirstOrDefault() != null ? overlayName.Where(x => x.DrawableID == v).FirstOrDefault()?.Name : "#" + v.ToString();
        if (ho.Index != 255)
        {
            OverlayIndexMenuNEW.Value = ho.Index;
        }
        else
        {
            OverlayIndexMenuNEW.Value = -1;
        }
        OverlayIndexMenuNEW.Activated += (sender, selecteditem) =>
        {
            SetFaceOverlay(ho.OverlayID, OverlayIndexMenuNEW.Value, OpacityMenuNEW.Value, ho.ColorType, PrimaryColorMenuNEW.SelectedItem.ColorID, SecondaryColorMenuNEW.SelectedItem.ColorID);
        };
        OverlayIndexMenuNEW.IndexChanged += (sender, oldIndex, newIndex) =>
        {
            SetFaceOverlay(ho.OverlayID, OverlayIndexMenuNEW.Value, OpacityMenuNEW.Value, ho.ColorType, PrimaryColorMenuNEW.SelectedItem.ColorID, SecondaryColorMenuNEW.SelectedItem.ColorID);
        };
        overLaySubMenu.AddItem(OverlayIndexMenuNEW);

        bool useHighlights = true;


        UIMenuCheckboxItem addHighlight = new UIMenuCheckboxItem("Add Highlight", true);
        addHighlight.CheckboxEvent += (sender, Checked) =>
        {
            SecondaryColorMenuNEW.Enabled = Checked;
            useHighlights = Checked;
        };
        if (hasSecondaryColor)
        {
            overLaySubMenu.AddItem(addHighlight);
        }
        else
        {
            useHighlights = false;
        }
  
        PrimaryColorMenuNEW.IndexChanged += (sender, oldIndex, newIndex) =>
        {
            SetFaceOverlay(ho.OverlayID, OverlayIndexMenuNEW.Value, OpacityMenuNEW.Value, ho.ColorType, PrimaryColorMenuNEW.SelectedItem.ColorID, useHighlights ? SecondaryColorMenuNEW.SelectedItem.ColorID : PrimaryColorMenuNEW.SelectedItem.ColorID);
        };
        overLaySubMenu.AddItem(PrimaryColorMenuNEW);

        
        SecondaryColorMenuNEW.IndexChanged += (sender, oldIndex, newIndex) =>
        {
            SetFaceOverlay(ho.OverlayID, OverlayIndexMenuNEW.Value, OpacityMenuNEW.Value, ho.ColorType, PrimaryColorMenuNEW.SelectedItem.ColorID, useHighlights ? SecondaryColorMenuNEW.SelectedItem.ColorID : PrimaryColorMenuNEW.SelectedItem.ColorID);
        };
        if (hasSecondaryColor)
        {
            overLaySubMenu.AddItem(SecondaryColorMenuNEW);
        }
    
        OpacityMenuNEW.Value = 1.0f;
        OpacityMenuNEW.Formatter = v => v.ToString("P0");
        OpacityMenuNEW.Activated += (sender, selecteditem) =>
        {
            SetFaceOverlay(ho.OverlayID, OverlayIndexMenuNEW.Value, OpacityMenuNEW.Value, ho.ColorType, PrimaryColorMenuNEW.SelectedItem.ColorID, useHighlights ? SecondaryColorMenuNEW.SelectedItem.ColorID : PrimaryColorMenuNEW.SelectedItem.ColorID);
        };
        OpacityMenuNEW.IndexChanged += (sender, oldIndex, newIndex) =>
        {
            SetFaceOverlay(ho.OverlayID, OverlayIndexMenuNEW.Value, OpacityMenuNEW.Value, ho.ColorType, PrimaryColorMenuNEW.SelectedItem.ColorID, useHighlights ? SecondaryColorMenuNEW.SelectedItem.ColorID : PrimaryColorMenuNEW.SelectedItem.ColorID);
        };
        overLaySubMenu.AddItem(OpacityMenuNEW);

        int TotalCost = standardCost;

        if (BarberShop != null && BarberShop.PedVariationShopMenu != null)
        {
            int customizedCost = BarberShop.PedVariationShopMenu.GetOverlayCost(Player, ho.OverlayID);
            if (customizedCost != -1)
            {
                TotalCost = customizedCost;
                EntryPoint.WriteToConsole($"SPECIAL OVERLAY COST ITEM ADDED {ho.OverlayID} ${customizedCost}");
            }
        }


        UIMenuItem purchaseStyle = new UIMenuItem("Purchase", "Select to purchase this item") { RightLabel = $"${TotalCost}" };
        purchaseStyle.Activated += (sender, e) =>
        {
            if (Player.BankAccounts.GetMoney(true) < TotalCost)
            {
                BarberShop.PlayErrorSound();
                BarberShop.DisplayMessage("~r~Insufficient Funds", "We are sorry, we are unable to complete this transation, as you do not have the required funds");
            }
            else
            {
                HeadOverlayData existingComponenet = Player.CurrentModelVariation.HeadOverlays.FirstOrDefault(x => x.OverlayID == ho.OverlayID);
                if (existingComponenet == null)
                {
                    existingComponenet = new HeadOverlayData()
                    {
                        OverlayID = ho.OverlayID,
                        Index = OverlayIndexMenuNEW.Value,
                        Opacity = OpacityMenuNEW.Value,
                        ColorType = ho.ColorType,
                        PrimaryColor = PrimaryColorMenuNEW.SelectedItem.ColorID,
                        SecondaryColor = useHighlights ? SecondaryColorMenuNEW.SelectedItem.ColorID : PrimaryColorMenuNEW.SelectedItem.ColorID,
                    };
                    Player.CurrentModelVariation.HeadOverlays.Add(existingComponenet);
                    //EntryPoint.WriteToConsole($"PURCHASE STYLE ADDED NEW OVERLAY OverlayID:{ho.OverlayID} Index:{OverlayIndexMenu.Value} Opacity:{OpacityMenu.Value} PriColor:{PrimaryColorMenu.SelectedItem.ColorID} SecColor:{SecondaryColorMenu.SelectedItem.ColorID}");
                }
                else
                {
                    existingComponenet.OverlayID = ho.OverlayID;
                    existingComponenet.Index = OverlayIndexMenuNEW.Value;
                    existingComponenet.Opacity = OpacityMenuNEW.Value;
                    existingComponenet.ColorType = ho.ColorType;
                    existingComponenet.PrimaryColor = PrimaryColorMenuNEW.SelectedItem.ColorID;
                    existingComponenet.SecondaryColor = useHighlights ? SecondaryColorMenuNEW.SelectedItem.ColorID : PrimaryColorMenuNEW.SelectedItem.ColorID;
                    //EntryPoint.WriteToConsole($"PURCHASE STYLE UPDATED EXISTING OVERLAY OverlayID:{ho.OverlayID} Index:{OverlayIndexMenu.Value} Opacity:{OpacityMenu.Value} PriColor:{PrimaryColorMenu.SelectedItem.ColorID} SecColor:{SecondaryColorMenu.SelectedItem.ColorID}");
                }
                HaircutInteract?.PlayHaircutAnimation(TotalCost);
                //EntryPoint.WriteToConsole("ANIM DONE");
            }
        };
        overLaySubMenu.AddItem(purchaseStyle);


    }
  
    private void Setup()
    {
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

