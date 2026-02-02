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
    private PedClothingShopMenuItem BaseballHatRegular1_MPF;
    private PedClothingShopMenuItem BaseballHatRegular1Reverse_MPF;
    private PedClothingShopMenuItem StrawHat1_MPF;
    private PedClothingShopMenuItem Necklace1_MPF;
    private PedClothingShopMenuItem BusinessOutfit1_MPF;
    private PedClothingShopMenuItem BusinessOutfit2_MPF;
    private PedClothingShopMenuItem Necklace1_MPM;
    private PedClothingShopMenuItem DangleEarring1_MPM;
    private PedClothingShopMenuItem UnbuttonedCasual1_MPM;
    private PedClothingShopMenuItem LostSupported_MPM;
    private PedClothingShopMenuItem BaseballHatProLaps1_MPM;
    private PedClothingShopMenuItem BaseballHatRegular1_MPM;
    private PedClothingShopMenuItem BaseballHatRegular1Reverse_MPM;
    private PedClothingShopMenuItem StrawHat1_MPM;
    private PedClothingShopMenuItem Aviators1_MPM;
    private PedClothingShopMenuItem Chucks1_MPM;
    private PedClothingShopMenuItem FlipFlops1_MPM;
    private PedClothingShopMenuItem Watch1_MPM;
    private PedClothingShopMenuItem Watch2_MPM;
    private PedClothingShopMenuItem Watch3_MPM;
    private PedClothingShopMenuItem BusinessOutfit1_MPM;
    private PedClothingShopMenuItem DirtHelmet1_MPF;
    private PedClothingShopMenuItem DirtHelmet2_MPF;
    private PedClothingShopMenuItem HalfHelmet1_MPF;
    private PedClothingShopMenuItem FullHelmet1_MPF;
    private PedClothingShopMenuItem FullHelmet2_MPF;
    private PedClothingShopMenuItem FullHelmet3_MPF;
    private PedClothingShopMenuItem FullHelmet4_MPF;
    private PedClothingShopMenuItem FullHelmet5_MPF;
    private PedClothingShopMenuItem OpenFullHelmet1_MPF;
    private PedClothingShopMenuItem PilotHelmet1_MPF;
    private PedClothingShopMenuItem DirtHelmet1_MPM;
    private PedClothingShopMenuItem DirtHelmet2_MPM;
    private PedClothingShopMenuItem HalfHelmet1_MPM;
    private PedClothingShopMenuItem FullHelmet1_MPM;
    private PedClothingShopMenuItem FullHelmet2_MPM;
    private PedClothingShopMenuItem FullHelmet3_MPM;
    private PedClothingShopMenuItem FullHelmet4_MPM;
    private PedClothingShopMenuItem FullHelmet5_MPM;
    private PedClothingShopMenuItem OpenFullHelmet1_MPM;
    private PedClothingShopMenuItem PilotHelmet1_MPM;

    public PedClothingShopMenu HelmetShopMenu { get; private set; }
    public PedClothingShopMenu GenericClothesShopMenu { get; private set; }
    public PedClothingShopMenu MaskShopMenu { get; private set; }

    public void Setup()
    {
        MPFemale();
        MPMale();
        MPCombined();
        
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
            Polo1_MPF,UnbuttonedCasual1_MPM,
            FittedT_MPF,
            LooseButton_MPF,
            LostSupported_MPF,LostSupported_MPM,
            Aviators1_MPF,Aviators1_MPM,
            Heels1_MPF,
            Chucks1_MPF,Chucks1_MPM,
            FlipFlops1_MPF,FlipFlops1_MPM,
            GoldWatch1_MPF,Watch1_MPM,
            GoldWatch2_MPF,Watch2_MPM,
            Watch3_MPM,
            DangleEarring1_MPF,DangleEarring1_MPM,
            BaseballHatProLaps1_MPF,BaseballHatProLaps1_MPM,
            BaseballHatRegular1Reverse_MPF,BaseballHatRegular1Reverse_MPM,
            BaseballHatRegular1_MPF,BaseballHatRegular1_MPM,
            StrawHat1_MPF,StrawHat1_MPM,
            Necklace1_MPF,Necklace1_MPM,
            BusinessOutfit1_MPF,BusinessOutfit1_MPM,
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

        HelmetShopMenu = new PedClothingShopMenu();
        HelmetShopMenu.ID = "HelmetShopMenu";
        HelmetShopMenu.PedClothingShopMenuItems = new List<PedClothingShopMenuItem>()
        {
            DirtHelmet1_MPF,
            DirtHelmet2_MPF,
            HalfHelmet1_MPF,
            FullHelmet1_MPF,
            FullHelmet2_MPF,
            FullHelmet3_MPF,
            FullHelmet4_MPF,
            FullHelmet5_MPF,
            OpenFullHelmet1_MPF,
            PilotHelmet1_MPF,

            DirtHelmet1_MPM,
            DirtHelmet2_MPM,
            HalfHelmet1_MPM,
            FullHelmet1_MPM,
            FullHelmet2_MPM,
            FullHelmet3_MPM,
            FullHelmet4_MPM,
            FullHelmet5_MPM,
            OpenFullHelmet1_MPM,
            PilotHelmet1_MPM,
        };

    }
    private void MPCombined()
    {
        MPCombined_Masks();
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
        MPFemale_Helmets();

        
    }
    private void MPMale()
    {
        MPMale_Accessories();
        MPMale_Hats();
        MPMale_Glasses();
        MPMale_Shoes();
        MPMale_Tops();
        MPMale_Outfits();
        MPMale_Watches();
        MPMale_Bracelets();
        MPMale_Ears();
        MPMale_Helmets();
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
    private void MPMale_Accessories()
    {
        Necklace1_MPM = new PedClothingShopMenuItem("Chain 1", "", 100, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(7, 17, new List<int>() { 0 }), })
        {
            Category = "Chains",
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
    private void MPMale_Outfits()
    {
        BusinessOutfit1_MPM = new PedClothingShopMenuItem("Business Outfit 1", "", 1200, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(3, 4, new List<int>() { 0 }),
        new PedClothingComponent(4, 10, new List<int>() { 0 }){ AllowAllTextureVariations = true },
        new PedClothingComponent(6, 10, new List<int>() { 0 }){ AllowAllTextureVariations = true },
        new PedClothingComponent(7, 21, new List<int>() { 0 }) { AllowAllTextureVariations = true },
        new PedClothingComponent(8, 31, new List<int>() { 0 }){ AllowAllTextureVariations = true },
        new PedClothingComponent(11, 4, new List<int>() { 0 }){ AllowAllTextureVariations = true },
        })
        {
            Category = "Outfits",
            SubCategory = "Business",
            PedFocusZone = ePedFocusZone.Body,
        };

        /*new PedComponent(3, 4, 0, 0),
                new PedComponent(4, 10, 0, 0),
                new PedComponent(6, 10, 0, 0),
                new PedComponent(7, 21, 2, 0),
                new PedComponent(8, 10, 0, 0),
                new PedComponent(11, 4, 0, 0)*/
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
    private void MPMale_Ears()
    {
        DangleEarring1_MPM = new PedClothingShopMenuItem("Simple Earring", "", 67, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(2, 4, new List<int>() { 0 }) { IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Earrings",
            SubCategory = "Casual",
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
    private void MPMale_Tops()
    {
        UnbuttonedCasual1_MPM = new PedClothingShopMenuItem("Unbuttoned Casual Shirt", "Show you are worth it.", 50, new List<string>() { "mp_m_freemode_01" },
            new List<PedClothingComponent>() {
                new PedClothingComponent(11, 346, new List<int>() { 0 }) { AllowAllTextureVariations = true, },
                new PedClothingComponent(8, 0, new List<int>() { 0 }),
                new PedClothingComponent(3, 11, new List<int>() { 0 }), })
        {
            Category = "Shirts",
            SubCategory = "Button Up",
            PedFocusZone = ePedFocusZone.Chest,
            RemoveTorsoDecals = true,
        };


        

        LostSupported_MPM = new PedClothingShopMenuItem("LOST Supporter", "Test Description 3", 60, new List<string>() { "mp_m_freemode_01" },
        new List<PedClothingComponent>() {
                new PedClothingComponent(11, 366, new List<int>() { 0 }) { AllowAllTextureVariations = true, },
                new PedClothingComponent(8, 81, new List<int>() { 22,23,24 }) { AllowAllTextureVariations = true, },
                new PedClothingComponent(3, 0, new List<int>() { 0 }) , })
        {
            Category = "Shirts",
            SubCategory = "Cuts",
            PedFocusZone = ePedFocusZone.Chest,
            ForceSetOverlays = new List<AppliedOverlay>() {
                    new AppliedOverlay("mpBiker_overlays","MP_Biker_Tee_028_M","ZONE_TORSO"),
                    new AppliedOverlay("mpBiker_overlays","MP_Biker_Tee_029_M","ZONE_TORSO"),
                    new AppliedOverlay("mpBiker_overlays","MP_Biker_Tee_030_M","ZONE_TORSO"),
                    new AppliedOverlay("mpBiker_overlays","MP_Biker_Tee_031_M","ZONE_TORSO"),
                    new AppliedOverlay("mpBiker_overlays","MP_Biker_Tee_034_M","ZONE_TORSO"),
                    new AppliedOverlay("mpBiker_overlays","MP_Biker_Tee_035_M","ZONE_TORSO"),
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
        BaseballHatRegular1_MPF = new PedClothingShopMenuItem("Baseball Cap 1", "", 78, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 153, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Hats",
            SubCategory = "Baseball Caps",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        BaseballHatRegular1Reverse_MPF = new PedClothingShopMenuItem("Backwards Flat Cap", "", 78, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
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
    private void MPMale_Hats()
    {
        BaseballHatProLaps1_MPM = new PedClothingShopMenuItem("Prolaps Golf", "", 78, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 159, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Hats",
            SubCategory = "Baseball Caps",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        BaseballHatRegular1_MPM = new PedClothingShopMenuItem("Baseball Cap 1", "", 78, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 154, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Hats",
            SubCategory = "Baseball Caps",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        BaseballHatRegular1Reverse_MPM = new PedClothingShopMenuItem("Backwards Baseball Cap 1", "", 78, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
        new PedClothingComponent(0, 155, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Hats",
            SubCategory = "Baseball Caps",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
        };
        StrawHat1_MPM = new PedClothingShopMenuItem("Straw Hat", "", 25, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 21, new List<int>() { 0 }){ IsProp = true, AllowAllTextureVariations = true, }, })
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
    private void MPMale_Glasses()
    {
        Aviators1_MPM = new PedClothingShopMenuItem("Aviators 1", "", 150, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(1, 12, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
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
    private void MPMale_Shoes()
    {
        Chucks1_MPM = new PedClothingShopMenuItem("Blaines", "", 75, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(6, 4, new List<int>() { 0 }){ AllowAllTextureVariations = true }, })
        {
            Category = "Shoes",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.Feet
        };
        FlipFlops1_MPM = new PedClothingShopMenuItem("Flip Flops", "", 75, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
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
    private void MPMale_Watches()
    {
        Watch1_MPM = new PedClothingShopMenuItem("Watch 1", "", 85, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(6, 1, new List<int>() { 0 }) { IsProp = true, AllowAllTextureVariations = true, }, })
        {
            Category = "Watches",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.LeftWrist,
            IsAccessory = true,
        };
        Watch2_MPM = new PedClothingShopMenuItem("Watch 2", "", 65, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(6, 3, new List<int>() { 0 }) { IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Watches",
            SubCategory = "Casual",
            PedFocusZone = ePedFocusZone.LeftWrist,
            IsAccessory = true,
        };
        Watch3_MPM = new PedClothingShopMenuItem("Watch 3", "", 65, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
            new PedClothingComponent(6, 4, new List<int>() { 0 }) { IsProp = true,AllowAllTextureVariations = true, }, })
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
    private void MPMale_Bracelets()
    {

    }

    private void MPFemale_Helmets()
    {
        DirtHelmet1_MPF = new PedClothingShopMenuItem("Dirt Helmet 1", "", 78, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 16, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Helmets",
            SubCategory = "Dirt",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
            IsHelmet = true,
        };
        DirtHelmet2_MPF = new PedClothingShopMenuItem("Dirt Helmet 2", "", 78, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 48, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Helmets",
            SubCategory = "Dirt",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
            IsHelmet = true,
        };
        HalfHelmet1_MPF = new PedClothingShopMenuItem("Half Helmet 1", "", 78, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 17, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Helmets",
            SubCategory = "Half",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
            IsHelmet = true,
        };
        FullHelmet1_MPF = new PedClothingShopMenuItem("Full Helmet 1", "", 78, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 18, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Helmets",
            SubCategory = "Regular",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
            IsHelmet = true,
        };
        FullHelmet2_MPF = new PedClothingShopMenuItem("Full Helmet 2", "", 78, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 49, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Helmets",
            SubCategory = "Regular",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
            IsHelmet = true,
        };
        FullHelmet3_MPF = new PedClothingShopMenuItem("Full Helmet 3", "", 78, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 50, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Helmets",
            SubCategory = "Regular",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
            IsHelmet = true,
        };
        FullHelmet4_MPF = new PedClothingShopMenuItem("Full Helmet 4", "", 78, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 51, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Helmets",
            SubCategory = "Regular",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
            IsHelmet = true,
        };
        FullHelmet5_MPF = new PedClothingShopMenuItem("Full Helmet 5", "", 78, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 52, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Helmets",
            SubCategory = "Regular",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
            IsHelmet = true,
        };
        OpenFullHelmet1_MPF = new PedClothingShopMenuItem("Open Full Helmet 1", "", 78, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 66, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Helmets",
            SubCategory = "Regular",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
            IsHelmet = true,
        };

        PilotHelmet1_MPF = new PedClothingShopMenuItem("Pilot Helmet 1", "", 78, new List<string>() { "mp_f_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 37, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Helmets",
            SubCategory = "Military",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
            IsHelmet = true,
        };
    }
    private void MPMale_Helmets()
    {
        DirtHelmet1_MPM = new PedClothingShopMenuItem("Dirt Helmet 1", "", 78, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 16, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Helmets",
            SubCategory = "Dirt",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
            IsHelmet = true,
        };
        DirtHelmet2_MPM = new PedClothingShopMenuItem("Dirt Helmet 2", "", 78, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 48, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Helmets",
            SubCategory = "Dirt",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
            IsHelmet = true,
        };
        HalfHelmet1_MPM = new PedClothingShopMenuItem("Half Helmet 1", "", 78, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 17, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Helmets",
            SubCategory = "Half",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
            IsHelmet = true,
        };
        FullHelmet1_MPM = new PedClothingShopMenuItem("Full Helmet 1", "", 78, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 18, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Helmets",
            SubCategory = "Regular",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
            IsHelmet = true,
        };
        FullHelmet2_MPM = new PedClothingShopMenuItem("Full Helmet 2", "", 78, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 50, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Helmets",
            SubCategory = "Regular",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
            IsHelmet = true,
        };
        FullHelmet3_MPM = new PedClothingShopMenuItem("Full Helmet 3", "", 78, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 51, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Helmets",
            SubCategory = "Regular",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
            IsHelmet = true,
        };
        FullHelmet4_MPM = new PedClothingShopMenuItem("Full Helmet 4", "", 78, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 52, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Helmets",
            SubCategory = "Regular",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
            IsHelmet = true,
        };
        FullHelmet5_MPM = new PedClothingShopMenuItem("Full Helmet 5", "", 78, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 53, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Helmets",
            SubCategory = "Regular",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
            IsHelmet = true,
        };
        OpenFullHelmet1_MPM = new PedClothingShopMenuItem("Open Full Helmet 1", "", 78, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 67, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Helmets",
            SubCategory = "Regular",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
            IsHelmet = true,
        };

        PilotHelmet1_MPM = new PedClothingShopMenuItem("Pilot Helmet 1", "", 78, new List<string>() { "mp_m_freemode_01" }, new List<PedClothingComponent>() {
                new PedClothingComponent(0, 38, new List<int>() { 0 }){ IsProp = true,AllowAllTextureVariations = true, }, })
        {
            Category = "Helmets",
            SubCategory = "Military",
            PedFocusZone = ePedFocusZone.Head,
            IsAccessory = true,
            IsHelmet = true,
        };
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

}