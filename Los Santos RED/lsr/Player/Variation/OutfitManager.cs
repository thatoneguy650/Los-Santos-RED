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
    public void SetOutfit(SavedOutfit savedOutfit, bool doAnimation)
    {
        if (savedOutfit.PedVariation == null)
        {
            Game.DisplaySubtitle("No Variation to Set");
            return;
        }

        if (doAnimation)
        {
            Game.FadeScreenOut(500, true);
        }
        Player.Character.ResetVariation();
        NativeFunction.Natives.CLEAR_ALL_PED_PROPS(Player.Character);
        PedVariation newVariation = savedOutfit.PedVariation.Copy();
        Player.CurrentModelVariation = newVariation;
        Player.CurrentModelVariation.ApplyToPed(Player.Character);
        if (doAnimation)
        {
            PlayDisplayItem();
            Game.FadeScreenIn(500, true);
        }
        Game.DisplaySubtitle($"Applied Outfit {savedOutfit.Name}");
    }
    private void PlayDisplayItem()
    {
        string dictionary = "move_clown@p_m_one_idles@";
        string anim = "fidget_look_at_outfit_01";
        if (!AnimationDictionary.RequestAnimationDictionayResult(dictionary))
        {
            return;
        }
        NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, dictionary, anim, 4.0f, -4.0f, -1, 0, 0, false, false, false);
    }
    public void PlayIdleAnimation()
    {
        string dictionary = "anim@amb@business@cfid@cfid_photograph@";
        string anim = "base_model";
        if (!AnimationDictionary.RequestAnimationDictionayResult(dictionary))
        {
            return;
        }
        NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, dictionary, anim, 4.0f, -4.0f, -1, 1, 0, false, false, false);
    }
}
