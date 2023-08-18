using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class OutfitManager
{
    private IOutfitManageable Player;
    private ISavedOutfits SavedOutfits;

    public OutfitManager(IOutfitManageable player, ISavedOutfits savedOutfits)
    {
        Player = player;
        SavedOutfits = savedOutfits;
    }
    public List<SavedOutfit> CurrentPlayerOutfits => SavedOutfits.SavedOutfitList.Where(x=> x.ModelName.ToLower() == Player.ModelName.ToLower()).ToList();
    public void Dispose()
    {

    }
    public void SetOutfit(SavedOutfit savedOutfit)
    {
        if (savedOutfit.PedVariation == null)
        {
            Game.DisplaySubtitle("No Variation to Set");
            return;
        }
        Player.Character.ResetVariation();
        NativeFunction.Natives.CLEAR_ALL_PED_PROPS(Player.Character);
        PedVariation newVariation = savedOutfit.PedVariation.Copy();
        Player.CurrentModelVariation = newVariation;
        Player.CurrentModelVariation.ApplyToPed(Player.Character);
        Game.DisplaySubtitle($"Applied Outfit {savedOutfit.Name}");
    }
}
