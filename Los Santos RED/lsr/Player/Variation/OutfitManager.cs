using LosSantosRED.lsr.Interface;
using Microsoft.VisualBasic.Devices;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
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
    public List<SavedOutfit> CurrentCharactersOutfits => SavedOutfits.SavedOutfitList.Where(x => x.ModelName.ToLower() == Player.ModelName.ToLower() && !string.IsNullOrEmpty(x.CharacterName) && x.CharacterName.ToLower() == Player.PlayerName.ToLower()).ToList();
    public List<SavedOutfit> CurrentModelOutfits => SavedOutfits.SavedOutfitList.Where(x=> x.ModelName.ToLower() == Player.ModelName.ToLower()).ToList();
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
            GameFiber.Sleep(500);
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
        NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, dictionary, anim, 2.0f, -2.0f, -1, 0, 0, false, false, false);
    }

    public void ToggleGlasses()
    {
        ToggleProp(1);
    }
    public void ToggleHat()
    {
        ToggleProp(0);
    }
    public void ToggleMask()
    {
        ToggleComponent(1);
    }
    public void ToggleComponent(int componenetID)
    {
        bool IsOn = false;
        int currentVariation = NativeFunction.Natives.GET_PED_DRAWABLE_VARIATION<int>(Player.Character, componenetID);
        if(currentVariation > 0)
        {
            IsOn = true;
        }

        PlayAnimation();
        GameFiber.Sleep(500);

        if (IsOn)
        {
            NativeFunction.Natives.SET_PED_COMPONENT_VARIATION(Player.Character, componenetID, 0, 0, 0);
        }
        else
        {
            PedComponent componenet = Player.CurrentModelVariation.Components.Where(x => x.ComponentID == componenetID).FirstOrDefault();
            if (componenet == null)
            {
                //EntryPoint.WriteToConsole($"ToggleComponent {componenetID} IsOn{IsOn} componenet IS NULL");
                return;
            }
            NativeFunction.Natives.SET_PED_COMPONENT_VARIATION(Player.Character, componenetID, componenet.DrawableID, componenet.TextureID, componenet.PaletteID);
            //EntryPoint.WriteToConsole($"ToggleComponent {componenetID} IsOn{IsOn} SET VALUE {componenet.DrawableID} {componenet.TextureID}");
        }
    }
    public void ToggleProp(int selectedPropType)
    {
        bool isPropOn = false;
        int PropIndex = NativeFunction.Natives.GET_PED_PROP_INDEX<int>(Player.Character, selectedPropType);
        if(PropIndex != -1)
        {
            isPropOn = true;
        }
        //EntryPoint.WriteToConsole($"isPropOn{isPropOn} PropIndex{PropIndex}");
        PedPropComponent prop = Player.CurrentModelVariation.Props.Where(x => x.PropID == selectedPropType).FirstOrDefault();
        if(prop == null)
        {
            //EntryPoint.WriteToConsole($"ToggleProp {selectedPropType} isPropOn{isPropOn} componenet IS NULL");
            return;
        }
        List<int> CurrentProps = new List<int>();
        foreach (PedPropComponent Prop in Player.CurrentModelVariation.Props)
        {
            bool isLocalOn = NativeFunction.Natives.GET_PED_PROP_INDEX<int>(Player.Character, Prop.PropID) > -1;
            if(isLocalOn)
            {
                CurrentProps.Add(Prop.PropID);
            }
        }
        //EntryPoint.WriteToConsole($"CURRENT ON PROPS {string.Join(",", CurrentProps)}");
        //EntryPoint.WriteToConsole($"I WANT TO TOGGLE {selectedPropType} IT IS CURRENTLY ON {isPropOn}");


        PlayAnimation();
        GameFiber.Sleep(500);

        NativeFunction.Natives.CLEAR_ALL_PED_PROPS(Player.Character);
        foreach (PedPropComponent Prop in Player.CurrentModelVariation.Props)
        {
            //bool isLocalOn = NativeFunction.Natives.GET_PED_PROP_INDEX<int>(Player.Character, Prop.PropID) > -1;
            if (Prop.PropID == selectedPropType)
            {
                if (isPropOn)
                {

                }
                else
                {
                    NativeFunction.Natives.SET_PED_PROP_INDEX(Player.Character, Prop.PropID, Prop.DrawableID, Prop.TextureID, false);
                    //EntryPoint.WriteToConsole($"SET_PED_PROP_INDEX {Prop.PropID} isPropOn{isPropOn} SET VALUE {Prop.DrawableID} {Prop.TextureID}");
                }
            }
            else
            {
                if (CurrentProps.Contains(Prop.PropID))
                {
                    NativeFunction.Natives.SET_PED_PROP_INDEX(Player.Character, Prop.PropID, Prop.DrawableID, Prop.TextureID, false);
                    //EntryPoint.WriteToConsole($"SET_PED_PROP_INDEX {Prop.PropID} isPropOn{isPropOn} SET VALUE {Prop.DrawableID} {Prop.TextureID}");
                }
            }     
        }
    }

    private void PlayAnimation()
    {
        //anim@mp_helmets@on_foot goggles_down
        string dictionary = "anim@mp_helmets@on_foot";
        string anim = "goggles_down";
        if (!AnimationDictionary.RequestAnimationDictionayResult(dictionary))
        {
            return;
        }
        NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, dictionary, anim, 8.0f, -8.0f, -1, (int)(eAnimationFlags.AF_UPPERBODY | eAnimationFlags.AF_SECONDARY), 0, false, false, false);
    }

    //public void PlayIdleAnimation()
    //{
    //    string dictionary = "anim@amb@business@cfid@cfid_photograph@";
    //    string anim = "base_model";
    //    if (!AnimationDictionary.RequestAnimationDictionayResult(dictionary))
    //    {
    //        return;
    //    }
    //    NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, dictionary, anim, 4.0f, -4.0f, -1, 1, 0, false, false, false);
    //}

    public void CreateOutfitMenu(MenuPool menuPool, UIMenu subMenu, bool doAnimations, bool removeBanner)
    {
        subMenu.Clear();
        UIMenu ModelSubMenu = menuPool.AddSubMenu(subMenu, "By Model");
        UIMenu CharacterSubMenu = menuPool.AddSubMenu(subMenu, "By Character");

        if (removeBanner)
        {
            ModelSubMenu.RemoveBanner();
            CharacterSubMenu.RemoveBanner();
        }
        else
        {
            ModelSubMenu.SetBannerType(EntryPoint.LSRedColor);
            CharacterSubMenu.SetBannerType(EntryPoint.LSRedColor);
        }

        foreach (SavedOutfit so in CurrentModelOutfits.OrderBy(x=> x.CharacterName).ThenBy(x=> x.Name))
        {
            UIMenuItem uIMenuItem = new UIMenuItem(so.Name,$"Character: {so.CharacterName}");
            uIMenuItem.Activated += (sender, e) =>
            {
                SetOutfit(so, doAnimations);
            };
            ModelSubMenu.AddItem(uIMenuItem);
        }
        foreach (SavedOutfit so in CurrentCharactersOutfits.OrderBy(x => x.Name))
        {
            UIMenuItem uIMenuItem = new UIMenuItem(so.Name);
            uIMenuItem.Activated += (sender, e) =>
            {
                SetOutfit(so, doAnimations);
            };
            CharacterSubMenu.AddItem(uIMenuItem);
        }
    }
}
