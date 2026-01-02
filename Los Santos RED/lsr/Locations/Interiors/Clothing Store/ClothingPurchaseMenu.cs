using LosSantosRED.lsr.Interface;
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
    private ILocationInteractable Player;
    private ISettingsProvideable Settings;
    private TryOnInteract TryOnInteract;
    private MenuPool MenuPool;
    private UIMenu InteractionMenu;
    private PedVariation WorkingVariation;
    private List<UIMenu> CategoryList;
    public ClothingPurchaseMenu(ILocationInteractable player, ClothingShop clothingShop, TryOnInteract tryOnInteract, ISettingsProvideable settings)
    {
        Player = player;
        ClothingShop = clothingShop;
        Settings = settings;
        TryOnInteract = tryOnInteract;
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
        };
        CategoryList = new List<UIMenu>();
        foreach (PedClothingShopMenuItem pedClothingShopMenuItem in ClothingShop.PedClothingShopMenu.PedClothingShopMenuItems.Where(x=> x.ModelNames.Contains(Player.ModelName.ToLower())))
        {
            pedClothingShopMenuItem.AddToMenu(Player, MenuPool, CreateSubMenu(pedClothingShopMenuItem.Category), ClothingShop);
        }

        InteractionMenu.Visible = true;

    }
    private UIMenu CreateSubMenu(string menuName)
    {
        if (string.IsNullOrEmpty(menuName))
        {
            return InteractionMenu;
        }
        UIMenu createdCategory = CategoryList.Where(x => x.SubtitleText.ToLower() == menuName.ToLower()).FirstOrDefault();
        if(createdCategory != null)
        {
            //EntryPoint.WriteToConsole($"menuName {menuName} FOUND {    string.Join(",", CategoryList.Select(x=>x.SubtitleText)       ) }");
            return createdCategory;

        }
        //else
        //{
        //    EntryPoint.WriteToConsole($"menuName {menuName} NOT FOUND {string.Join(",", CategoryList.Select(x => x.SubtitleText))}");
        //}
        createdCategory = MenuPool.AddSubMenu(InteractionMenu, menuName);
        CategoryList.Add(createdCategory);

        return createdCategory;
    }
    public void Dispose()
    {
        Game.RawFrameRender -= (s, e) => MenuPool.DrawBanners(e.Graphics);
    }
}

