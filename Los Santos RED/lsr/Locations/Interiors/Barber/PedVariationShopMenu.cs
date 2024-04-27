using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class PedVariationShopMenu
{
    //Components
    //Overlays
    //Coloring
    //Face Morph

    public string ID { get; set; }

    public int StandardMaleHaircutPrice { get; set; } = 25;
    public int StandardFemaleHaircutPrice { get; set; } = 35;
    public int StandardBeardTrimPrice { get; set; } = 30;
    public int StandardHairColoringPrice { get; set; } = 45;
    public int StandardMakeupPrice { get; set; } = 55;
    public int PremiumHaircutExtra { get; set; } = 15;
    public int PremiumBearTrimExtra { get; set; } = 10;
    public int PremiumColoringExtra { get; set; } = 25;
    public int PremiumMakeupExtra { get; set; } = 25;
    public PedVariationShopMenu()
    {

    }
    public List<PedComponentShopMenu> PedVariationComponentMenu { get; set; }
    public List<PedOverlayShopMenu> PedOverlayShopMenu { get; set; }
    public PedComponentShopMenu GetDrawableCost(ILocationInteractable player, int componentID, int drawableID, int textureID)
    {
        if(PedVariationComponentMenu == null || !PedVariationComponentMenu.Any())
        {
            return null;
        }
        return PedVariationComponentMenu.Where(x => x.ModelName.ToLower() == player.ModelName.ToLower() && x.ComponentID == componentID && x.DrawableID == drawableID && (x.TextureID == -1 || x.TextureID == textureID)).FirstOrDefault();
    }
    public int GetOverlayCost(ILocationInteractable player, int overlayID)
    {
        if(PedOverlayShopMenu == null || !PedOverlayShopMenu.Any())
        {
            return -1;
        }
        PedOverlayShopMenu selected = PedOverlayShopMenu.Where(x => x.Male == player.IsMale && x.OverlayID == overlayID).FirstOrDefault();
        if (selected != null)
        {
            return selected.Price;
        }
        return -1;
    }
}

