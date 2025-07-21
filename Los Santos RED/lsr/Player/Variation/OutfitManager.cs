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
using static System.Collections.Specialized.BitVector32;


public class OutfitManager
{
    private IOutfitManageable Player;
    private ISavedOutfits SavedOutfits;
    private List<int> AllPropIDs = new List<int>() { 0,1,6,7 };
    private List<int> AllComponentIDs = new List<int>() { 1,5,7,9 };
    private int DefaulShoeComponentID => Player.ModelName.ToLower() == "mp_f_freemode_01" ? 35 : Player.ModelName.ToLower() == "mp_m_freemode_01" ? 34 : 0;

    private int DefaultTorsoComponentID => Player.ModelName.ToLower() == "mp_f_freemode_01" ? 15 : Player.ModelName.ToLower() == "mp_m_freemode_01" ? 15 : 15;
    private int DefaultUndershirtComponentID => Player.ModelName.ToLower() == "mp_f_freemode_01" ? 15 : Player.ModelName.ToLower() == "mp_m_freemode_01" ? 15 : 15;
    private int DefaulTopComponentID => Player.ModelName.ToLower() == "mp_f_freemode_01" ? 15 : Player.ModelName.ToLower() == "mp_m_freemode_01" ? 15 : 15;
    private int DefaultLowerComponentID => Player.ModelName.ToLower() == "mp_f_freemode_01" ? 15 : Player.ModelName.ToLower() == "mp_m_freemode_01" ? 29 : 15;
    public OutfitManager(IOutfitManageable player, ISavedOutfits savedOutfits)
    {
        Player = player;
        SavedOutfits = savedOutfits;
    }
    public List<SavedOutfit> CurrentCharactersOutfits => SavedOutfits.SavedOutfitList.Where(x => x.ModelName.ToLower() == Player.ModelName.ToLower() && !string.IsNullOrEmpty(x.CharacterName) && x.CharacterName.ToLower() == Player.PlayerName.ToLower()).ToList();
    public List<SavedOutfit> CurrentModelOutfits => SavedOutfits.SavedOutfitList.Where(x=> x.ModelName.ToLower() == Player.ModelName.ToLower()).ToList();

    public bool HasMaskOn { get; private set; }

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
    public void SetAllOff()
    {
        foreach(int propId in AllPropIDs)
        {
            SetProp(propId, false);
        }
        foreach (int componentID in AllComponentIDs)
        {
            SetComponent(componentID, false, true,"","");
        }
    }
    public void SetAllOn()
    {
        foreach (int propId in AllPropIDs)
        {
            SetProp(propId, true);
        }
        foreach (int componentID in AllComponentIDs)
        {
            SetComponent(componentID, true, true,"","");
        }
    }
    public void ToggleHat()
    {
        ToggleProp(0);
    }
    public void ToggleGlasses()
    {
        ToggleProp(1);
    }
    public void ToggleEarrings()
    {
        ToggleProp(2);
    }
    public void ToggleWatches()
    {
        ToggleProp(6);
    }
    public void ToggleBracelet()
    {
        ToggleProp(7);
    }
    public void ToggleMask()
    {
        ToggleComponent(1);
    }
    public void ToggleBag()
    {
        ToggleComponent(5,0,true, "mp_safehouseseated@female@generic@idle_b", "idle_d");
    }
    public void ToggleShoes()
    {
        ToggleComponent(6, DefaulShoeComponentID, true, "pickup_object", "pickup_low");
        EntryPoint.WriteToConsole($"TOGGLE SHOES RAN {Player.ModelName.ToLower()}");
    }
    public void ToggleAccessories()
    {
        ToggleComponent(7,0,true, "mp_safehouseseated@female@generic@idle_b", "idle_d");
    }
    public void ToggleArmor()
    {
        ToggleComponent(9,0,true, "mp_safehouseseated@female@generic@idle_b", "idle_d");
    }
    public void ToggleShirt()
    {

        //mp_safehouseseated@female@generic@idle_b idle_d
        ToggleComponent(3, DefaultTorsoComponentID,false,"","");
        ToggleComponent(8, DefaultUndershirtComponentID,false, "", "");
        ToggleComponent(11, DefaulTopComponentID,false, "", "");
        PlayAnimation("mp_safehouseseated@female@generic@idle_b", "idle_d");
    }
    public void TogglePants()
    {
        ToggleComponent(4, DefaultLowerComponentID, true, "pickup_object", "pickup_low");
    }
    private void UpdateMaskCheck()
    {
        HasMaskOn = NativeFunction.Natives.GET_PED_DRAWABLE_VARIATION<int>(Player.Character, 1) > 0;
        EntryPoint.WriteToConsole($"UpdateMaskCheck HasMaskOn {HasMaskOn}");
    }
    public void ToggleComponent(int componenetID)
    {
        ToggleComponent(componenetID, 0, true, "", "");
    }
    public void ToggleComponent(int componenetID, int DefaultID, bool playAnimation, string animDict, string animName)
    {

        bool IsOn = false;
        int currentVariation = NativeFunction.Natives.GET_PED_DRAWABLE_VARIATION<int>(Player.Character, componenetID);
        //if (currentVariation > 0 || (DefaultID != 0 && currentVariation != DefaultID))
        //{
        //    IsOn = true;
        //}


        if (DefaultID != 0)
        {
            IsOn = !(currentVariation == DefaultID);
        }
        else if (currentVariation > 0)// || (DefaultID != 0 && currentVariation != DefaultID))
        {
            IsOn = true;
        }



        EntryPoint.WriteToConsole($"ToggleComponent1 ComponentID:{componenetID} DefaultID:{DefaultID} IsOn:{IsOn} currentVariation:{currentVariation}");

        if (playAnimation)
        {
            PlayAnimation(animDict, animName);
            GameFiber.Sleep(500);
        }
        if (IsOn)
        {
            NativeFunction.Natives.SET_PED_COMPONENT_VARIATION(Player.Character, componenetID, DefaultID, 0, 0);
        }
        else
        {
            PedComponent componenet = Player.CurrentModelVariation.Components.Where(x => x.ComponentID == componenetID).FirstOrDefault();
            if (componenet == null)
            {
                EntryPoint.WriteToConsole($"ToggleComponent2 {componenetID} IsOn{IsOn} componenet IS NULL");
                return;
            }
            NativeFunction.Natives.SET_PED_COMPONENT_VARIATION(Player.Character, componenetID, componenet.DrawableID, componenet.TextureID, componenet.PaletteID);
            EntryPoint.WriteToConsole($"ToggleComponent3 {componenetID} IsOn{IsOn} SET VALUE {componenet.DrawableID} {componenet.TextureID}");
        }
        UpdateMaskCheck();
    }
    public void SetComponent(int componenetID, bool SetOn, bool playAnimation, string animDict, string animName) => SetComponent(componenetID,0, SetOn, playAnimation, animDict, animName);
    public void SetComponent(int componenetID, int DefaultID, bool SetOn, bool playAnimation, string animDict, string animName)
    {
        bool IsOn = false;
        int currentVariation = NativeFunction.Natives.GET_PED_DRAWABLE_VARIATION<int>(Player.Character, componenetID);

        if(DefaultID != 0)
        {
            IsOn = !(currentVariation == DefaultID);
        }
        else if (currentVariation > 0)// || (DefaultID != 0 && currentVariation != DefaultID))
        {
            IsOn = true;
        }




        if (SetOn && IsOn)
        {
            return;
        }
        if (!SetOn && !IsOn)
        {
            return;
        }
        ToggleComponent(componenetID, DefaultID, playAnimation, animDict, animName);
    }
    public void SetProp(int selectedPropType, bool SetOn)
    {
        bool isPropOn = false;
        int PropIndex = NativeFunction.Natives.GET_PED_PROP_INDEX<int>(Player.Character, selectedPropType);
        if (PropIndex != -1)
        {
            isPropOn = true;
        }
        if(SetOn && isPropOn)
        {
            return;
        }
        if(!SetOn && !isPropOn)
        {
            return;
        }
        ToggleProp(selectedPropType);
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


        PlayAnimation("","");
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
    private void PlayAnimation(string animDict, string animName)
    {
        //anim@mp_helmets@on_foot goggles_down
        string dictionary = "anim@mp_helmets@on_foot";
        string anim = "goggles_down";


        if(!string.IsNullOrEmpty(animDict))
        {
            dictionary = animDict;
            anim = animName;
        }
        if (!AnimationDictionary.RequestAnimationDictionayResult(dictionary))
        {
            return;
        }
        NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, dictionary, anim, 8.0f, -8.0f, -1, (int)(eAnimationFlags.AF_UPPERBODY | eAnimationFlags.AF_SECONDARY), 0, false, false, false);
    }
    private bool HasProp(int PropID)
    {
        PedPropComponent ppc = Player.CurrentModelVariation.Props.Where(x => x.PropID == PropID).FirstOrDefault();
        if(ppc == null)
        {
            return false;
        }
        return true;
    }
    private bool HasComponent(int componentID)
    {
        PedComponent ppc = Player.CurrentModelVariation.Components.Where(x => x.ComponentID == componentID).FirstOrDefault();
        if (ppc == null)
        {
            return false;
        }
        if(ppc.DrawableID == 0)
        {
            return false;
        }
        return true;
    }
    public void CreateAccessoryMenu()
    {
        MenuPool menuPool = new MenuPool();
        UIMenu AccessoryUIMenu = new UIMenu("Accessories", "Toggle outfit accessories and clothing");

        menuPool.Add(AccessoryUIMenu);
        AccessoryUIMenu.Visible = true;


        UIMenuItem TurnAllOffMenuItem = new UIMenuItem("Take All Accessories Off", "Take all accessories and props off");
        TurnAllOffMenuItem.Activated += (sender, selectedItem) =>
        {
            SetAllOff();
        };
        AccessoryUIMenu.AddItem(TurnAllOffMenuItem);



        UIMenuItem TurnAllOnMenuItem = new UIMenuItem("Put All Accessories On", "Put all accessories and props on");
        TurnAllOnMenuItem.Activated += (sender, selectedItem) =>
        {
            SetAllOn();
        };
        AccessoryUIMenu.AddItem(TurnAllOnMenuItem);


        UIMenu HeadUIMenu = menuPool.AddSubMenu(AccessoryUIMenu, "Head");


        UIMenuItem GlassesMenuItem = new UIMenuItem("Toggle Glasses", "Take glasses on or off");
        GlassesMenuItem.Activated += (sender, selectedItem) =>
        {
            ToggleGlasses();
        };
        if (HasProp(1))
        {
            HeadUIMenu.AddItem(GlassesMenuItem);
        }
        UIMenuItem HatMenuItem = new UIMenuItem("Toggle Hat", "Take hat on or off");
        HatMenuItem.Activated += (sender, selectedItem) =>
        {
            ToggleHat();
        };
        if (HasProp(0))
        {
            HeadUIMenu.AddItem(HatMenuItem);
        }
        UIMenuItem MaskMenuItem = new UIMenuItem("Toggle Mask", "Take mask on or off");
        MaskMenuItem.Activated += (sender, selectedItem) =>
        {
            ToggleMask();
        };
        if (HasComponent(1))
        {
            HeadUIMenu.AddItem(MaskMenuItem);
        }
        UIMenuItem EarringMenuItem = new UIMenuItem("Toggle Earrings", "Take earrings on or off");
        EarringMenuItem.Activated += (sender, selectedItem) =>
        {
            ToggleEarrings();
        };
        if (HasProp(2))
        {
            HeadUIMenu.AddItem(EarringMenuItem);
        }



        UIMenu BodyUIMenu = menuPool.AddSubMenu(AccessoryUIMenu, "Body");

        UIMenuItem ArmorMenuItem = new UIMenuItem("Remove Armor", "Remove armor. Added with inventory items");
        ArmorMenuItem.Activated += (sender, selectedItem) =>
        {
            Player.ArmorManager.RemoveArmor();
        };
        BodyUIMenu.AddItem(ArmorMenuItem);
        UIMenuItem BagMenuItem = new UIMenuItem("Toggle Bag", "Take bag on or off");
        BagMenuItem.Activated += (sender, selectedItem) =>
        {
            ToggleBag();
        };
        if (HasComponent(5))
        {
            BodyUIMenu.AddItem(BagMenuItem);
        }
        UIMenuItem WatchesMenuItem = new UIMenuItem("Toggle Watches", "Take watches on or off");
        WatchesMenuItem.Activated += (sender, selectedItem) =>
        {
            ToggleWatches();
        };
        if (HasProp(6))
        {
            BodyUIMenu.AddItem(WatchesMenuItem);
        }
        UIMenuItem BraceletMenuItem = new UIMenuItem("Toggle Bracelets", "Take bracelets on or off");
        BraceletMenuItem.Activated += (sender, selectedItem) =>
        {
            ToggleBracelet();
        };
        if (HasProp(7))
        {
            BodyUIMenu.AddItem(BraceletMenuItem);
        }
        UIMenuItem AccessoriesMenuItem = new UIMenuItem("Toggle Accessories", "Take accessories on or off");
        AccessoriesMenuItem.Activated += (sender, selectedItem) =>
        {
            ToggleAccessories();
        };
        if (HasComponent(7))
        {
            BodyUIMenu.AddItem(AccessoriesMenuItem);
        }
        UIMenuItem ShirtMenuItem = new UIMenuItem("Toggle Shirt", "Take shirt on or off");
        ShirtMenuItem.Activated += (sender, selectedItem) =>
        {
            ToggleShirt();
        };
        BodyUIMenu.AddItem(ShirtMenuItem);


        UIMenu LegsUIMenu = menuPool.AddSubMenu(AccessoryUIMenu, "Legs");
        UIMenuItem ShoesMenuItem = new UIMenuItem("Toggle Shoes", "Take shoes on or off");
        ShoesMenuItem.Activated += (sender, selectedItem) =>
        {
            ToggleShoes();
        };
        LegsUIMenu.AddItem(ShoesMenuItem);
        UIMenuItem PantsMenuItem = new UIMenuItem("Toggle Pants", "Take pants on or off");
        PantsMenuItem.Activated += (sender, selectedItem) =>
        {
            TogglePants();
        };
        LegsUIMenu.AddItem(PantsMenuItem);

        /*            new PopUpBox(0,"Take All Accessories Off",new Action(() => Player.OutfitManager.SetAllOff()),"Take all accessories and props off"),
            new PopUpBox(1,"Put All Accessories On",new Action(() => Player.OutfitManager.SetAllOn()),"Put all accessories and props on"),


            new PopUpBox(2,"Toggle Glasses",new Action(() => Player.OutfitManager.ToggleGlasses()),"Take glasses on or off"),
            new PopUpBox(3,"Toggle Hat",new Action(() => Player.OutfitManager.ToggleHat()),"Take hat on or off"),
            new PopUpBox(4,"Toggle Mask",new Action(() => Player.OutfitManager.ToggleMask()),"Take mask on or off"),
            new PopUpBox(5,"Toggle Bag",new Action(() => Player.OutfitManager.ToggleBag()),"Take bag on or off"),

            new PopUpBox(6,"Toggle Accessories",new Action(() => Player.OutfitManager.ToggleAccessories()),"Take accessories on or off"),
           // new PopUpBox(7,"Toggle Armor",new Action(() => Player.OutfitManager.ToggleArmor()),"Take armor on or off"),
            new PopUpBox(7,"Toggle Shoes",new Action(() => Player.OutfitManager.ToggleShoes()),"Take shoes on or off"),

            new PopUpBox(8,"Toggle Earrings",new Action(() => Player.OutfitManager.ToggleEarrings()),"Take earrings on or off"),
            new PopUpBox(9,"Toggle Watches",new Action(() => Player.OutfitManager.ToggleWatches()),"Take watches on or off"),
            new PopUpBox(10,"Toggle Bracelets",new Action(() => Player.OutfitManager.ToggleBracelet()),"Take bracelets on or off"),

            new PopUpBox(11,"Toggle Shirt",new Action(() => Player.OutfitManager.ToggleShirt()),"Take shirt on or off"),
            new PopUpBox(12,"Toggle Pants",new Action(() => Player.OutfitManager.TogglePants()),"Take pants on or off"),
            new PopUpBox(13,"Remove Armor",new Action(() => Player.ArmorManager.RemoveArmor()),"Remove armor. Added with inventory items"),*/

        GameFiber.StartNew(delegate
        {
            while (menuPool.IsAnyMenuOpen())// && CarryingWeapon && IsShootingCheckerActive && ObservedWantedLevel < 3)
            {
                menuPool.ProcessMenus();
                GameFiber.Yield();
            }
        }, "AccessoryMenu");


    }


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
    public void TakeOffArmorVisually()
    {
        SetComponent(9, false, true, "mp_safehouseseated@female@generic@idle_b", "idle_d");
    }
    public void PutOnArmorVisually()
    {
        SetComponent(9,true, true, "mp_safehouseseated@female@generic@idle_b", "idle_d");
    }
}
