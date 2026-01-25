using LosSantosRED.lsr.Interface;
using Mod;
using Rage;
using RAGENativeUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ClothingPurchaseMenu
    {
    private ClothingShop ClothingShop;
    private OrbitCamera OrbitCamera;
    private PlayerPoser PlayerPoser;
    private ILocationInteractable Player;
    private ISettingsProvideable Settings;
    private TryOnInteract TryOnInteract;
    private MenuPool MenuPool;
    private UIMenu InteractionMenu;
    //private PedVariation WorkingVariation;
    private List<UIMenu> CategoryList;
    private List<UIMenu> SubCategoryList;

    private List<CategoryLookup> CategoryLookups;



    public PedVariation WorkingVariation { get; private set; }

    public ClothingPurchaseMenu(ILocationInteractable player, ClothingShop clothingShop, TryOnInteract tryOnInteract, ISettingsProvideable settings)
    {
        Player = player;
        ClothingShop = clothingShop;
        Settings = settings;
        TryOnInteract = tryOnInteract;
    }
    public void Start(MenuPool menuPool, UIMenu interactionMenu, OrbitCamera orbitCamera, List<PedClothingShopMenuItem> itemsToCreate, bool IsPurchase, bool MakeVisible)
    {
        MenuPool = menuPool;
        InteractionMenu = interactionMenu;
        OrbitCamera = orbitCamera;
        PlayerPoser = new PlayerPoser(Player, Settings);
        PlayerPoser.Setup();
        EntryPoint.WriteToConsole("ClothingPurchaseMenu START RAN");
        InteractionMenu.OnMenuOpen += (sender) =>
        {
            Player.IsTransacting = true;
        };
        InteractionMenu.OnMenuClose += (sender) =>
        {
            Player.IsTransacting = false;
        };
        CategoryList = new List<UIMenu>();
        SubCategoryList = new List<UIMenu>();
        CategoryLookups = new List<CategoryLookup>();

        WorkingVariation = Player.CurrentModelVariation.Copy();

        if (MakeVisible)
        {
            Player.PedSwap.ResetOffsetForCurrentModel();
        }


        foreach (PedClothingShopMenuItem pedClothingShopMenuItem in itemsToCreate)//ClothingShop.PedClothingShopMenu.PedClothingShopMenuItems.Where(x=> x.ModelNames.Contains(Player.ModelName.ToLower())))
        {
            pedClothingShopMenuItem.AddToMenu(Player, MenuPool, CreateSubMenu(pedClothingShopMenuItem.Category, pedClothingShopMenuItem.SubCategory), ClothingShop, OrbitCamera, IsPurchase, PlayerPoser, this);
        }

        if (MakeVisible)
        {
            PlayerPoser.Start();
            InteractionMenu.Visible = true;
            EntryPoint.WriteToConsole("ClothingPurchaseMenu START MAKE VISIBLE");
        }
    }

    private UIMenu CreateSubMenu(string categoryName, string subCategoryName)
    {
        UIMenu createdCategory = null;
        CategoryLookup categoryLooup = CategoryLookups.Where(x => x.CategoryName == categoryName && x.SubCategoryName == subCategoryName).FirstOrDefault();

        if(categoryLooup == null)
        {
            CategoryLookup newcategoryLooup = CategoryLookups.Where(x => x.CategoryName == categoryName).FirstOrDefault();
            if(newcategoryLooup != null)
            {
                createdCategory = newcategoryLooup.CategoryMenu;
            }
        }



        if (categoryLooup == null)
        {
            if (createdCategory == null)
            {
                createdCategory = MenuPool.AddSubMenu(InteractionMenu, categoryName);
            }
            UIMenu createdSubCategory = MenuPool.AddSubMenu(createdCategory, subCategoryName);
            categoryLooup = new CategoryLookup(categoryName, subCategoryName, createdCategory, createdSubCategory);
            CategoryLookups.Add(categoryLooup);
        }
        if (categoryLooup.SubCategoryMenu == null)
        {
            return categoryLooup.CategoryMenu;
        }
        return categoryLooup.SubCategoryMenu;
    }


    private UIMenu CreateSubMenu_Old(string categoryName,string subCategoryName)
    {
        if (string.IsNullOrEmpty(categoryName))
        {
            return InteractionMenu;
        }
        UIMenu createdCategory = CategoryList.Where(x => x.SubtitleText.ToLower() == categoryName.ToLower()).FirstOrDefault();
        if(createdCategory == null)
        {
            createdCategory = MenuPool.AddSubMenu(InteractionMenu, categoryName);
            CategoryList.Add(createdCategory);
        }
        if(string.IsNullOrEmpty(subCategoryName))
        {
            return createdCategory;
        }
        UIMenu createdSubCategory = SubCategoryList.Where(x => x.SubtitleText.ToLower() == subCategoryName.ToLower()).FirstOrDefault();
        if (createdSubCategory == null)
        {
            createdSubCategory = MenuPool.AddSubMenu(createdCategory, subCategoryName);
            SubCategoryList.Add(createdSubCategory);
        }     
        return createdSubCategory;
    }
    public void Dispose()
    {
        Game.RawFrameRender -= (s, e) => MenuPool.DrawBanners(e.Graphics);
        PlayerPoser.Dispose();
        if (Settings.SettingsManager.PedSwapSettings.AliasPedAsMainCharacter && !Player.CharacterModelIsPrimaryCharacter)
        {
            Player.PedSwap.AddOffset();
        }
    }

    public void CopyCurrentModelVariation()
    {
        WorkingVariation = Player.CurrentModelVariation.Copy();
    }

    private class CategoryLookup
    {
        public CategoryLookup(string categoryName, string subCategoryName, UIMenu categoryMenu, UIMenu subCategoryMenu)
        {
            CategoryName = categoryName;
            SubCategoryName = subCategoryName;
            CategoryMenu = categoryMenu;
            SubCategoryMenu = subCategoryMenu;
        }

        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
        public UIMenu CategoryMenu { get; set; }
        public UIMenu SubCategoryMenu { get; set; }
    }

}

