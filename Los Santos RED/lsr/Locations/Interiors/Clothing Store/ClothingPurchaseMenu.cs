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


        foreach(PedClothingShopMenuItem pedClothingShopMenuItem in ClothingShop.PedClothingShopMenu.PedClothingShopMenuItems.Where(x=> x.ModelName.ToLower() == Player.ModelName.ToLower()))
        {
            pedClothingShopMenuItem.AddToMenu(Player, MenuPool, InteractionMenu, ClothingShop);
        }

        InteractionMenu.Visible = true;

    }
    public void Dispose()
    {
        Game.RawFrameRender -= (s, e) => MenuPool.DrawBanners(e.Graphics);
    }
}

