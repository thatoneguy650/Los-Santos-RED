using System;
using System.Collections.Generic;


public class ShopMenus_Clothing
{
    private PedClothingShopMenuItem Polo1_MPF;
    private PedClothingShopMenuItem FittedT_MPF;
    private PedClothingShopMenuItem LooseButton_MPF;
    private PedClothingShopMenuItem LostSupported_MPF;
    private PedClothingShopMenuItem Aviators1_MPF;
    private PedClothingShopMenuItem MaskHockey_MPC;
    private PedClothingShopMenuItem MaskMonkey_MPC;
    private PedClothingShopMenuItem MaskLuchador_MPC;
    private PedClothingShopMenuItem MaskGargoyle1_MPC;
    private PedClothingShopMenuItem Heels1_MPF;
    private PedClothingShopMenuItem Chucks1_MPF;
    private PedClothingShopMenuItem FlipFlops1_MPF;
    private PedClothingShopMenuItem GoldWatch1_MPF;
    private PedClothingShopMenuItem GoldWatch2_MPF;
    private PedClothingShopMenuItem DangleEarring1_MPF;
    private PedClothingShopMenuItem BaseballHatProLaps1_MPF;
    private PedClothingShopMenuItem BaseballHatSprunkReverse_MPF;
    private PedClothingShopMenuItem StrawHat1_MPF;
    private PedClothingShopMenuItem Necklace1_MPF;
    private PedClothingShopMenuItem BusinessOutfit1_MPF;
    private PedClothingShopMenuItem BusinessOutfit2_MPF;

    public PedClothingShopMenu GenericClothesShopMenu { get; private set; }
    public PedClothingShopMenu MaskShopMenu { get; private set; }

    public void Setup()
    {
        MPFemale();
        MPMale();
        SetMenus();
    }
    public void SetMenus()
    {
        //Suburban
        //Binco
        //Discount Store
        //Poisonbys
        //Didier Sachs
        GenericClothesShopMenu = new PedClothingShopMenu();
        GenericClothesShopMenu.ID = "GenericClothesShop";
        GenericClothesShopMenu.PedClothingShopMenuItems = new List<PedClothingShopMenuItem>()
        {
            Polo1_MPF,
            FittedT_MPF,
            LooseButton_MPF,
            LostSupported_MPF,
            Aviators1_MPF,
            MaskHockey_MPC,
            MaskMonkey_MPC,
            MaskLuchador_MPC,
            MaskGargoyle1_MPC,
            Heels1_MPF,
            Chucks1_MPF,
            FlipFlops1_MPF,
            GoldWatch1_MPF,
            GoldWatch2_MPF,
            DangleEarring1_MPF,
            BaseballHatProLaps1_MPF,
            BaseballHatSprunkReverse_MPF,
            StrawHat1_MPF,
            Necklace1_MPF,
            BusinessOutfit1_MPF,
            BusinessOutfit2_MPF,
        };

        MaskShopMenu = new PedClothingShopMenu();
        MaskShopMenu.ID = "MaskShopMenu";
        MaskShopMenu.PedClothingShopMenuItems = new List<PedClothingShopMenuItem>()
        {

            MaskHockey_MPC,
            MaskMonkey_MPC,
            MaskLuchador_MPC,
            MaskGargoyle1_MPC,
        };
    }
    private void MPFemale()
    {
        //Components Mostly
        MPFemale_Tops();
        MPFemale_Shoes();   
        
        MPFemale_Accessories();

        //Props?
        MPFemale_Hats();
        MPFemale_Glasses();
        MPFemale_Ears();
        MPFemale_Watches();
        MPFemale_Bracelets();

        MPFemale_Outfits();


        MPCombined_Masks();
    }

    private void MPFemale_Accessories()
    {
        Necklace1_MPF = new PedClothingShopMenuItem("Necklace 1", "", 100, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(7, 7, new List<int>() { 0 }), })
        {
            Category = "Necklaces",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Neck,
            IsAccessory = true,
        };
    }

    private void MPFemale_Outfits()
    {
        BusinessOutfit1_MPF = new PedClothingShopMenuItem("Business Outfit 1", "", 1200, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(3, 0, new List<int>() { 0 }),
            new PedClothingComponent(4, 24, new List<int>() { 0,1,2,3,4,5 }){ AllowAllTextureVariations = true },
        new PedClothingComponent(6, 19, new List<int>() { 0 }){ AllowAllTextureVariations = true },
        //new PedClothingComponent(7, 13, new List<int>() { 0 }),
        new PedClothingComponent(8, 15, new List<int>() { 0 }){  },
        new PedClothingComponent(11, 27, new List<int>() { 0 }){ AllowAllTextureVariations = true },
        })
        {
            Category = "Outfits",
            SubCategory = "Business",
            PedFocusZone = ePedFocusZone.Body,
        };
        BusinessOutfit2_MPF = new PedClothingShopMenuItem("Business Outfit 2", "", 1400, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(3, 5, new List<int>() { 0 }),
        new PedClothingComponent(4, 6, new List<int>() { 2 }){ AllowAllTextureVariations = true },
        new PedClothingComponent(6, 20, new List<int>() { 7 }){ AllowAllTextureVariations = true },
        new PedClothingComponent(8, 40, new List<int>() { 6 }) { AllowAllTextureVariations = true },//new PedClothingComponent(8, 23, new List<int>() { 6 }),
        new PedClothingComponent(11, 6, new List<int>() { 2 }){ AllowAllTextureVariations = true },
        })
        {
            Category = "Outfits",
            SubCategory = "Business",
            PedFocusZone = ePedFocusZone.Body,
        };
    }

    private void MPFemale_Ears()
    {
        DangleEarring1_MPF = new PedClothingShopMenuItem("Dangle Earring", "", 67, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() { 
            new PedClothingComponent(2, 4, new List<int>() { 0 }) { IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Earrings",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.LeftEar
        };
    }
    private void MPFemale_Tops()
    {
        Polo1_MPF = new PedClothingShopMenuItem("Branded Polo Shirt", "Show you are worth it.", 50, new List<string>() { "mp_f_freemode_01" }, 
            new List<PedClothingComponent>() { 
                new PedClothingComponent(11, 14, new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 }) { AllowAllTextureVariations = true, }, 
                new PedClothingComponent(8, 15, new List<int>() { 0 }), 
                new PedClothingComponent(3, 14, new List<int>() { 0 }), })
        {
            Category = "Shirts",
            SubCategory = "Polo",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };

        FittedT_MPF = new PedClothingShopMenuItem("Fitted T Shirt", "Test Description", 50, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(11, 49, new List<int>() { 1, 0 }) { AllowAllTextureVariations = true, }, 
            new PedClothingComponent(8, 15, new List<int>() { 0 }), 
            new PedClothingComponent(3, 14, new List<int>() { 0 }), })
        {
            Category = "Shirts",
            SubCategory = "T-Shirts",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };
        LooseButton_MPF = new PedClothingShopMenuItem("Loose Button-Up", "Test Description 2", 60, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() { 
            new PedClothingComponent(11, 9, new List<int>() { 13 }) {AllowAllTextureVariations = true, }, 
            new PedClothingComponent(8, 15, new List<int>() { 0 }), 
            new PedClothingComponent(3, 9, new List<int>() { 0 }), })
        {
            Category = "Shirts",
            SubCategory = "Button Up",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };

        LostSupported_MPF = new PedClothingShopMenuItem("LOST Supporter", "Test Description 3", 60, new List<string>() { "mp_f_freemode_01" },
        new List<PedClothingComponent>() {
                new PedClothingComponent(11, 385, new List<int>() { 0 }),
                new PedClothingComponent(8, 86, new List<int>() { 22,23,24 }),
                new PedClothingComponent(3, 0, new List<int>() { 0 }) , })
        {
            Category = "Shirts",
            SubCategory = "Cuts",
            PedFocusZone = ePedFocusZone.Chest,
            ForceSetOverlays = new List<AppliedOverlay>() {
                    new AppliedOverlay("mpBiker_overlays","MP_Biker_Tee_028_F","ZONE_TORSO"),
                    new AppliedOverlay("mpBiker_overlays","MP_Biker_Tee_029_F","ZONE_TORSO"),
                    new AppliedOverlay("mpBiker_overlays","MP_Biker_Tee_030_F","ZONE_TORSO"),
                    new AppliedOverlay("mpBiker_overlays","MP_Biker_Tee_031_F","ZONE_TORSO"),
                    new AppliedOverlay("mpBiker_overlays","MP_Biker_Tee_034_F","ZONE_TORSO"),
                    new AppliedOverlay("mpBiker_overlays","MP_Biker_Tee_035_F","ZONE_TORSO"),
                },
            RemoveTorsoDecals = true,
        };
    }
    private void MPFemale_Hats()
    {
        BaseballHatProLaps1_MPF = new PedClothingShopMenuItem("Prolaps Golf", "", 78, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 158, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Hats",
            SubCategory = "Baseball Caps",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        BaseballHatSprunkReverse_MPF = new PedClothingShopMenuItem("Backwards Flat Cap", "", 78, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 154, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Hats",
            SubCategory = "Baseball Caps",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        StrawHat1_MPF = new PedClothingShopMenuItem("Straw Hat", "", 25, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 20, new List<int>() { 0 }){ IsProp = true, AllowAllTextureVariations = true, }, })
        {
            Category = "Hats",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
    }
    private void MPFemale_Glasses()
    {
        Aviators1_MPF = new PedClothingShopMenuItem("Aviators 1", "", 150, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(1, 11, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Glasses",
            SubCategory = "Sunglasses",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
    }
    private void MPFemale_Shoes()
    {
        Heels1_MPF = new PedClothingShopMenuItem("Heels", "", 75, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(6, 0, new List<int>() { 0 }) { AllowAllTextureVariations = true }, })
        {
            Category = "Shoes",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.Feet
        };
        Chucks1_MPF = new PedClothingShopMenuItem("Blaines", "", 75, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() { 
            new PedClothingComponent(6, 3, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Shoes",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Feet
        };
        FlipFlops1_MPF = new PedClothingShopMenuItem("Flip Flops", "", 75, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() { 
            new PedClothingComponent(6, 5, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Shoes",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Feet
        };
    }
    private void MPFemale_Watches()
    {
        GoldWatch1_MPF = new PedClothingShopMenuItem("Small Watch", "", 660, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() { 
            new PedClothingComponent(6, 2, new List<int>() { 0 }) { IsProp = true, AllowAllTextureVariations = true, }, })
        {
            Category = "Watches",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.LeftWrist,
            IsAccessory = true,
        };
        GoldWatch2_MPF = new PedClothingShopMenuItem("Large Watch", "", 860, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(6, 5, new List<int>() { 0 }) { IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Watches",
            SubCategory = "Formal",
            PedFocusZone = ePedFocusZone.LeftWrist,
            IsAccessory = true,
        };
    }
    private void MPFemale_Bracelets()
    {

    }
    private void MPCombined_Masks()
    {
        MaskHockey_MPC = new PedClothingShopMenuItem("Hockey Mask", "", 150, new List<string>() { "mp_f_freemode_01", "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(1, 4, new List<int>() { 0 }){  AllowAllTextureVariations = true, }, })
        {
            Category = "Masks",
            SubCategory = "Horror",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        MaskMonkey_MPC = new PedClothingShopMenuItem("Monkey Mask", "No monkeying around!", 150, new List<string>() { "mp_f_freemode_01", "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(1, 5, new List<int>() { 0 }){ AllowAllTextureVariations = true, }, })
        {
            Category = "Masks",
            SubCategory = "Other",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        MaskLuchador_MPC = new PedClothingShopMenuItem("Luchador Mask", "", 150, new List<string>() { "mp_f_freemode_01", "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(1, 6, new List<int>() { 0 }){ AllowAllTextureVariations = true, }, })
        {
            Category = "Masks",
            SubCategory = "Other",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        MaskGargoyle1_MPC = new PedClothingShopMenuItem("Gargoyle Mask", "", 150, new List<string>() { "mp_f_freemode_01", "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(1, 7, new List<int>() { 0 }){AllowAllTextureVariations = true, }, })
        {
            Category = "Masks",
            SubCategory = "Horror",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
    }
    private void MPMale()
    {

    }
}