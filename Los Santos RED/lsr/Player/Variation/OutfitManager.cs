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
    private ISettingsProvideable Settings;
    private ILocationInteractable LocationInteractable;
    private List<int> AllPropIDs = new List<int>() { 0,1,6,7 };
    private List<int> AllComponentIDs = new List<int>() { 1,5,7,9 };


    private List<TorsoLookup> FreeModeMaleTorsoLookup = new List<TorsoLookup>();
    private List<TorsoLookup> FreeModeFemaleTorsoLookup = new List<TorsoLookup>();
    private ClothingPurchaseMenu clothingPurchaseMenuProcess;

    private int DefaulShoeComponentID => IsFreeModeFemale ? 35 : IsFreeModeMale ? 34 : 0;
    private int DefaultTorsoComponentID => IsFreeModeFemale ? 15 : IsFreeModeMale ? 15 : 15;
    private int DefaultUndershirtComponentID => IsFreeModeFemale ? 15 : IsFreeModeMale ? 15 : 15;
    private int DefaulTopComponentID => IsFreeModeFemale ? 15 : IsFreeModeMale ? 15 : 15;
    private int DefaultLowerComponentID => IsFreeModeFemale ? 15 : IsFreeModeMale ? 29 : 15;
    private bool IsFreeModeMale => Player.ModelName.ToLower() == "mp_m_freemode_01";
    private bool IsFreeModeFemale => Player.ModelName.ToLower() == "mp_f_freemode_01";
    private bool IsPlayerFreeMode => IsFreeModeMale || IsFreeModeFemale;
    private bool IsPlayerMainCharacter => Player.CharacterModelIsPrimaryCharacter;
    private int DefaultHelmetDrawableID => IsFreeModeFemale || IsFreeModeMale ? 18 : Player.ModelName.ToLower() == "player_two" ? 25 : Player.ModelName.ToLower() == "player_one" ? 6 : Player.ModelName.ToLower() == "player_zero" ? 11 : -1;
    private int DefaultMaskDrawableID => IsFreeModeFemale || IsFreeModeMale ? 4 : Player.ModelName.ToLower() == "player_two" ? 2 : Player.ModelName.ToLower() == "player_one" ? 12 : Player.ModelName.ToLower() == "player_zero" ? 2 : 0;


    public OutfitManager(IOutfitManageable player, ISavedOutfits savedOutfits, ISettingsProvideable settings, ILocationInteractable locationInteractable)
    {
        Player = player;
        SavedOutfits = savedOutfits;
        Settings = settings;
        LocationInteractable = locationInteractable;
    }
    public List<SavedOutfit> CurrentCharactersOutfits => SavedOutfits.SavedOutfitList.Where(x => x.ModelName.ToLower() == Player.ModelName.ToLower() && !string.IsNullOrEmpty(x.CharacterName) && x.CharacterName.ToLower() == Player.PlayerName.ToLower()).ToList();
    public List<SavedOutfit> CurrentModelOutfits => SavedOutfits.SavedOutfitList.Where(x=> x.ModelName.ToLower() == Player.ModelName.ToLower()).ToList();
    public List<PedClothingShopMenuItem> PurchasedPedClothingShopMenuItems { get; set; } = new List<PedClothingShopMenuItem>();
    public bool HasMaskOn { get; private set; }

    public void Setup()
    {
        //lookup from https://github.com/andristum/dpclothing/blob/master/Client/Variations.lua
        FreeModeMaleTorsoLookup = new List<TorsoLookup>()
        {
            new TorsoLookup(16,4)
            ,new TorsoLookup(17,4)
            ,new TorsoLookup(18,4)
            ,new TorsoLookup(19,0)
            ,new TorsoLookup(20,1)
            ,new TorsoLookup(21,2)
            ,new TorsoLookup(22,4)
            ,new TorsoLookup(23,5)
            ,new TorsoLookup(24,6)
            ,new TorsoLookup(25,8)
            ,new TorsoLookup(26,11)
            ,new TorsoLookup(27,12)
            ,new TorsoLookup(28,14)
            ,new TorsoLookup(29,15)
            ,new TorsoLookup(30,0)
            ,new TorsoLookup(31,1)
            ,new TorsoLookup(32,2)
            ,new TorsoLookup(33,4)
            ,new TorsoLookup(34,5)
            ,new TorsoLookup(35,6)
            ,new TorsoLookup(36,8)
            ,new TorsoLookup(37,11)
            ,new TorsoLookup(38,12)
            ,new TorsoLookup(39,14)
            ,new TorsoLookup(40,15)
            ,new TorsoLookup(41,0)
            ,new TorsoLookup(42,1)
            ,new TorsoLookup(43,2)
            ,new TorsoLookup(44,4)
            ,new TorsoLookup(45,5)
            ,new TorsoLookup(46,6)
            ,new TorsoLookup(47,8)
            ,new TorsoLookup(48,11)
            ,new TorsoLookup(49,12)
            ,new TorsoLookup(50,14)
            ,new TorsoLookup(51,15)
            ,new TorsoLookup(52,0)
            ,new TorsoLookup(53,1)
            ,new TorsoLookup(54,2)
            ,new TorsoLookup(55,4)
            ,new TorsoLookup(56,5)
            ,new TorsoLookup(57,6)
            ,new TorsoLookup(58,8)
            ,new TorsoLookup(59,11)
            ,new TorsoLookup(60,12)
            ,new TorsoLookup(61,14)
            ,new TorsoLookup(62,15)
            ,new TorsoLookup(63,0)
            ,new TorsoLookup(64,1)
            ,new TorsoLookup(65,2)
            ,new TorsoLookup(66,4)
            ,new TorsoLookup(67,5)
            ,new TorsoLookup(68,6)
            ,new TorsoLookup(69,8)
            ,new TorsoLookup(70,11)
            ,new TorsoLookup(71,12)
            ,new TorsoLookup(72,14)
            ,new TorsoLookup(73,15)
            ,new TorsoLookup(74,0)
            ,new TorsoLookup(75,1)
            ,new TorsoLookup(76,2)
            ,new TorsoLookup(77,4)
            ,new TorsoLookup(78,5)
            ,new TorsoLookup(79,6)
            ,new TorsoLookup(80,8)
            ,new TorsoLookup(81,11)
            ,new TorsoLookup(82,12)
            ,new TorsoLookup(83,14)
            ,new TorsoLookup(84,15)
            ,new TorsoLookup(85,0)
            ,new TorsoLookup(86,1)
            ,new TorsoLookup(87,2)
            ,new TorsoLookup(88,4)
            ,new TorsoLookup(89,5)
            ,new TorsoLookup(90,6)
            ,new TorsoLookup(91,8)
            ,new TorsoLookup(92,11)
            ,new TorsoLookup(93,12)
            ,new TorsoLookup(94,14)
            ,new TorsoLookup(95,15)
            ,new TorsoLookup(96,4)
            ,new TorsoLookup(97,4)
            ,new TorsoLookup(98,4)
            ,new TorsoLookup(99,0)
            ,new TorsoLookup(100,1)
            ,new TorsoLookup(101,2)
            ,new TorsoLookup(102,4)
            ,new TorsoLookup(103,5)
            ,new TorsoLookup(104,6)
            ,new TorsoLookup(105,8)
            ,new TorsoLookup(106,11)
            ,new TorsoLookup(107,12)
            ,new TorsoLookup(108,14)
            ,new TorsoLookup(109,15)
            ,new TorsoLookup(110,4)
            ,new TorsoLookup(111,4)
            ,new TorsoLookup(115,112)
            ,new TorsoLookup(116,112)
            ,new TorsoLookup(117,112)
            ,new TorsoLookup(118,112)
            ,new TorsoLookup(119,112)
            ,new TorsoLookup(120,112)
            ,new TorsoLookup(121,112)
            ,new TorsoLookup(122,113)
            ,new TorsoLookup(123,113)
            ,new TorsoLookup(124,113)
            ,new TorsoLookup(125,113)
            ,new TorsoLookup(126,113)
            ,new TorsoLookup(127,113)
            ,new TorsoLookup(128,113)
            ,new TorsoLookup(129,114)
            ,new TorsoLookup(130,114)
            ,new TorsoLookup(131,114)
            ,new TorsoLookup(132,114)
            ,new TorsoLookup(133,114)
            ,new TorsoLookup(134,114)
            ,new TorsoLookup(135,114)
            ,new TorsoLookup(136,15)
            ,new TorsoLookup(137,15)
            ,new TorsoLookup(138,0)
            ,new TorsoLookup(139,1)
            ,new TorsoLookup(140,2)
            ,new TorsoLookup(141,4)
            ,new TorsoLookup(142,5)
            ,new TorsoLookup(143,6)
            ,new TorsoLookup(144,8)
            ,new TorsoLookup(145,11)
            ,new TorsoLookup(146,12)
            ,new TorsoLookup(147,14)
            ,new TorsoLookup(148,112)
            ,new TorsoLookup(149,113)
            ,new TorsoLookup(150,114)
            ,new TorsoLookup(151,0)
            ,new TorsoLookup(152,1)
            ,new TorsoLookup(153,2)
            ,new TorsoLookup(154,4)
            ,new TorsoLookup(155,5)
            ,new TorsoLookup(156,6)
            ,new TorsoLookup(157,8)
            ,new TorsoLookup(158,11)
            ,new TorsoLookup(159,12)
            ,new TorsoLookup(160,14)
            ,new TorsoLookup(161,112)
            ,new TorsoLookup(162,113)
            ,new TorsoLookup(163,114)
            ,new TorsoLookup(165,4)
            ,new TorsoLookup(166,4)
            ,new TorsoLookup(167,4)

        };

        FreeModeFemaleTorsoLookup = new List<TorsoLookup>()
        {
            new TorsoLookup(16,11)
            ,new TorsoLookup(17,3)
            ,new TorsoLookup(18,3)
            ,new TorsoLookup(19,3)
            ,new TorsoLookup(20,0)
            ,new TorsoLookup(21,1)
            ,new TorsoLookup(22,2)
            ,new TorsoLookup(23,3)
            ,new TorsoLookup(24,4)
            ,new TorsoLookup(25,5)
            ,new TorsoLookup(26,6)
            ,new TorsoLookup(27,7)
            ,new TorsoLookup(28,9)
            ,new TorsoLookup(29,11)
            ,new TorsoLookup(30,12)
            ,new TorsoLookup(31,14)
            ,new TorsoLookup(32,15)
            ,new TorsoLookup(33,0)
            ,new TorsoLookup(34,1)
            ,new TorsoLookup(35,2)
            ,new TorsoLookup(36,3)
            ,new TorsoLookup(37,4)
            ,new TorsoLookup(38,5)
            ,new TorsoLookup(39,6)
            ,new TorsoLookup(40,7)
            ,new TorsoLookup(41,9)
            ,new TorsoLookup(42,11)
            ,new TorsoLookup(43,12)
            ,new TorsoLookup(44,14)
            ,new TorsoLookup(45,15)
            ,new TorsoLookup(46,0)
            ,new TorsoLookup(47,1)
            ,new TorsoLookup(48,2)
            ,new TorsoLookup(49,3)
            ,new TorsoLookup(50,4)
            ,new TorsoLookup(51,5)
            ,new TorsoLookup(52,6)
            ,new TorsoLookup(53,7)
            ,new TorsoLookup(54,9)
            ,new TorsoLookup(55,11)
            ,new TorsoLookup(56,12)
            ,new TorsoLookup(57,14)
            ,new TorsoLookup(58,15)
            ,new TorsoLookup(59,0)
            ,new TorsoLookup(60,1)
            ,new TorsoLookup(61,2)
            ,new TorsoLookup(62,3)
            ,new TorsoLookup(63,4)
            ,new TorsoLookup(64,5)
            ,new TorsoLookup(65,6)
            ,new TorsoLookup(66,7)
            ,new TorsoLookup(67,9)
            ,new TorsoLookup(68,11)
            ,new TorsoLookup(69,12)
            ,new TorsoLookup(70,14)
            ,new TorsoLookup(71,15)
            ,new TorsoLookup(72,0)
            ,new TorsoLookup(73,1)
            ,new TorsoLookup(74,2)
            ,new TorsoLookup(75,3)
            ,new TorsoLookup(76,4)
            ,new TorsoLookup(77,5)
            ,new TorsoLookup(78,6)
            ,new TorsoLookup(79,7)
            ,new TorsoLookup(80,9)
            ,new TorsoLookup(81,11)
            ,new TorsoLookup(82,12)
            ,new TorsoLookup(83,14)
            ,new TorsoLookup(84,15)
            ,new TorsoLookup(85,0)
            ,new TorsoLookup(86,1)
            ,new TorsoLookup(87,2)
            ,new TorsoLookup(88,3)
            ,new TorsoLookup(89,4)
            ,new TorsoLookup(90,5)
            ,new TorsoLookup(91,6)
            ,new TorsoLookup(92,7)
            ,new TorsoLookup(93,9)
            ,new TorsoLookup(94,11)
            ,new TorsoLookup(95,12)
            ,new TorsoLookup(96,14)
            ,new TorsoLookup(97,15)
            ,new TorsoLookup(98,0)
            ,new TorsoLookup(99,1)
            ,new TorsoLookup(100,2)
            ,new TorsoLookup(101,3)
            ,new TorsoLookup(102,4)
            ,new TorsoLookup(103,5)
            ,new TorsoLookup(104,6)
            ,new TorsoLookup(105,7)
            ,new TorsoLookup(106,9)
            ,new TorsoLookup(107,11)
            ,new TorsoLookup(108,12)
            ,new TorsoLookup(109,14)
            ,new TorsoLookup(110,15)
            ,new TorsoLookup(111,3)
            ,new TorsoLookup(112,3)
            ,new TorsoLookup(113,3)
            ,new TorsoLookup(114,0)
            ,new TorsoLookup(115,1)
            ,new TorsoLookup(116,2)
            ,new TorsoLookup(117,3)
            ,new TorsoLookup(118,4)
            ,new TorsoLookup(119,5)
            ,new TorsoLookup(120,6)
            ,new TorsoLookup(121,7)
            ,new TorsoLookup(122,9)
            ,new TorsoLookup(123,11)
            ,new TorsoLookup(124,12)
            ,new TorsoLookup(125,14)
            ,new TorsoLookup(126,15)
            ,new TorsoLookup(127,3)
            ,new TorsoLookup(128,3)
            ,new TorsoLookup(132,129)
            ,new TorsoLookup(133,129)
            ,new TorsoLookup(134,129)
            ,new TorsoLookup(135,129)
            ,new TorsoLookup(136,129)
            ,new TorsoLookup(137,129)
            ,new TorsoLookup(138,129)
            ,new TorsoLookup(139,130)
            ,new TorsoLookup(140,130)
            ,new TorsoLookup(141,130)
            ,new TorsoLookup(142,130)
            ,new TorsoLookup(143,130)
            ,new TorsoLookup(144,130)
            ,new TorsoLookup(145,130)
            ,new TorsoLookup(146,131)
            ,new TorsoLookup(147,131)
            ,new TorsoLookup(148,131)
            ,new TorsoLookup(149,131)
            ,new TorsoLookup(150,131)
            ,new TorsoLookup(151,131)
            ,new TorsoLookup(152,131)
            ,new TorsoLookup(154,153)
            ,new TorsoLookup(155,153)
            ,new TorsoLookup(156,153)
            ,new TorsoLookup(157,153)
            ,new TorsoLookup(158,153)
            ,new TorsoLookup(159,153)
            ,new TorsoLookup(160,153)
            ,new TorsoLookup(162,161)
            ,new TorsoLookup(163,161)
            ,new TorsoLookup(164,161)
            ,new TorsoLookup(165,161)
            ,new TorsoLookup(166,161)
            ,new TorsoLookup(167,161)
            ,new TorsoLookup(168,161)
            ,new TorsoLookup(169,15)
            ,new TorsoLookup(170,15)
            ,new TorsoLookup(171,0)
            ,new TorsoLookup(172,1)
            ,new TorsoLookup(173,2)
            ,new TorsoLookup(174,3)
            ,new TorsoLookup(175,4)
            ,new TorsoLookup(176,5)
            ,new TorsoLookup(177,6)
            ,new TorsoLookup(178,7)
            ,new TorsoLookup(179,9)
            ,new TorsoLookup(180,11)
            ,new TorsoLookup(181,12)
            ,new TorsoLookup(182,14)
            ,new TorsoLookup(183,129)
            ,new TorsoLookup(184,130)
            ,new TorsoLookup(185,131)
            ,new TorsoLookup(186,153)
            ,new TorsoLookup(187,0)
            ,new TorsoLookup(188,1)
            ,new TorsoLookup(189,2)
            ,new TorsoLookup(190,3)
            ,new TorsoLookup(191,4)
            ,new TorsoLookup(192,5)
            ,new TorsoLookup(193,6)
            ,new TorsoLookup(194,7)
            ,new TorsoLookup(195,9)
            ,new TorsoLookup(196,11)
            ,new TorsoLookup(197,12)
            ,new TorsoLookup(198,14)
            ,new TorsoLookup(199,129)
            ,new TorsoLookup(200,130)
            ,new TorsoLookup(201,131)
            ,new TorsoLookup(202,153)
            ,new TorsoLookup(203,161)
            ,new TorsoLookup(204,161)
            ,new TorsoLookup(206,3)
            ,new TorsoLookup(207,3)
            ,new TorsoLookup(208,3)

        };
    }
    public void Dispose()
    {
        clothingPurchaseMenuProcess?.Dispose();
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
        Player.CurrentModelVariation.ApplyToPed(Player.Character, false);
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
        if (IsPlayerFreeMode)
        {
            ToggleComponent(1);
        }
        else if(IsPlayerMainCharacter)
        {
            ToggleComponent(0);
        }
        
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
        ToggleComponent(3, DefaultTorsoComponentID, false,"","");
        ToggleComponent(8, DefaultUndershirtComponentID,false, "", "");
        ToggleComponent(11, DefaulTopComponentID,false, "", "");
        PlayAnimation("mp_safehouseseated@female@generic@idle_b", "idle_d");
    }
    public void TogglePants()
    {
        ToggleComponent(4, DefaultLowerComponentID, true, "pickup_object", "pickup_low");
    }
    public void ToggleGloves()
    {
        //14
        //31,44,57,70
        PedComponent currentTorso = Player.CurrentModelVariation.Components.Where(x => x.ComponentID == 3).FirstOrDefault();
        if(currentTorso == null)
        {
            return;
        }
        if(currentTorso.DrawableID <= 15)
        {
            return;//no gloves on
        }
        int noneGlovedTorsoID = GetTorsoFromGlove(currentTorso);//.DrawableID;






        EntryPoint.WriteToConsole($"ToggleGloves {noneGlovedTorsoID}");

        ToggleComponent(3, noneGlovedTorsoID, true, "mp_safehouseseated@female@generic@idle_b", "idle_d");
    }
    private int GetTorsoFromGlove(PedComponent currentTorso)
    {
        if(currentTorso == null)
        {
            return 0;
        }
        TorsoLookup TorsoLookup = null;
        if (IsFreeModeMale)
        {
            TorsoLookup = FreeModeMaleTorsoLookup.Where(x=> x.AssignedComponenetID == currentTorso.DrawableID).FirstOrDefault();
        }
        else if (IsFreeModeFemale)
        {
            TorsoLookup = FreeModeFemaleTorsoLookup.Where(x => x.AssignedComponenetID == currentTorso.DrawableID).FirstOrDefault();
        }
        if(TorsoLookup == null)
        {
            return 0;
        }
        return TorsoLookup.MatchingTorsoComponenetID;
    }

    private bool GetIsHelmetOn()
    {
        if (Player.CurrentModelVariation.Helmet == null)
        {
            Player.CurrentModelVariation.Helmet = new PedPropComponent(0, DefaultHelmetDrawableID, 0);
        }
        bool isHelmetOn = false;
        bool isPropOn = false;
        int PropIndex = NativeFunction.Natives.GET_PED_PROP_INDEX<int>(Player.Character, 0);
        if (PropIndex != -1)
        {
            isPropOn = true;
        }


        if (isPropOn && PropIndex == Player.CurrentModelVariation.Helmet.DrawableID)
        {
            isHelmetOn = true;
        }
        return isHelmetOn;
    }
    public void ToggleHelmet()
    {
        if(Player.CurrentModelVariation.Helmet == null)
        {
            Player.CurrentModelVariation.Helmet = new PedPropComponent(0, DefaultHelmetDrawableID, 0);
        }


        if(GetIsHelmetOn())
        {
            ResetProps(0, "veh@bike@common@front@base", "take_off_helmet_stand");
        }
        else
        {
            PlayAnimation("veh@bike@sport@front@base", "put_on_helmet");
            GameFiber.Sleep(750);
            NativeFunction.Natives.SET_PED_PROP_INDEX(Player.Character, Player.CurrentModelVariation.Helmet.PropID, Player.CurrentModelVariation.Helmet.DrawableID, Player.CurrentModelVariation.Helmet.TextureID, false);
        }
    }
    private void UpdateMaskCheck()
    {
        if (IsPlayerFreeMode)
        {
            HasMaskOn = NativeFunction.Natives.GET_PED_DRAWABLE_VARIATION<int>(Player.Character, 1) > 0;
        }
        else if (IsPlayerMainCharacter)
        {
            int PropIndex = NativeFunction.Natives.GET_PED_PROP_INDEX<int>(Player.Character, 0);
            HasMaskOn = PropIndex == DefaultMaskDrawableID;
        }
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
    private void ResetProps(int selectedPropType, string dict, string anim)
    {
        bool isPropOn = false;
        bool IsHelmetOn = GetIsHelmetOn();
        List<int> CurrentProps = new List<int>();
        foreach (PedPropComponent Prop in Player.CurrentModelVariation.Props)
        {
            int propDrawable = NativeFunction.Natives.GET_PED_PROP_INDEX<int>(Player.Character, Prop.PropID);
            //bool isLocalOn = propDrawable > -1 && propDrawable == Prop.DrawableID;

            bool isLocalOn = propDrawable > -1 && Prop.PropID != selectedPropType;// propDrawable == prop.DrawableID;

            if (propDrawable > -1 && Prop.PropID == selectedPropType && propDrawable == Prop.DrawableID)
            {
                isLocalOn = true;
            }


            if (isLocalOn)
            {
                CurrentProps.Add(Prop.PropID);
            }
        }
        PlayAnimation(dict, anim);
        GameFiber.Sleep(500);
        NativeFunction.Natives.CLEAR_ALL_PED_PROPS(Player.Character);
        foreach (PedPropComponent Prop in Player.CurrentModelVariation.Props)
        {
            if (Prop.PropID == selectedPropType)
            {

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
    public void ToggleProp(int selectedPropType)
    {
        EntryPoint.WriteToConsole($"TOGGLE PROP RAN selectedPropType {selectedPropType}");
        bool isPropOn = false;
        int PropIndex = NativeFunction.Natives.GET_PED_PROP_INDEX<int>(Player.Character, selectedPropType);
        if(PropIndex != -1)
        {
            isPropOn = true;
        }
        //EntryPoint.WriteToConsole($"isPropOn{isPropOn} PropIndex{PropIndex}");
        PedPropComponent currentVariationProp = Player.CurrentModelVariation.Props.Where(x => x.PropID == selectedPropType).FirstOrDefault();
        if(currentVariationProp == null)
        {
            //EntryPoint.WriteToConsole($"ToggleProp {selectedPropType} isPropOn{isPropOn} componenet IS NULL");
            return;
        }
        List<int> CurrentProps = new List<int>();
        foreach (PedPropComponent Prop in Player.CurrentModelVariation.Props)
        {
            int propDrawable = NativeFunction.Natives.GET_PED_PROP_INDEX<int>(Player.Character, Prop.PropID);
            bool isLocalOn = propDrawable > -1 && Prop.PropID != selectedPropType;// propDrawable == prop.DrawableID;

            if(propDrawable > -1 && Prop.PropID == selectedPropType && propDrawable == PropIndex)
            {
                isLocalOn = true;
            }

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
        UpdateMaskCheck();
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
        return ppc.DrawableID > 0;
    }
    private bool HasGloves()
    {
        PedComponent ppc = Player.CurrentModelVariation.Components.Where(x => x.ComponentID == 3).FirstOrDefault();
        if (ppc == null)
        {
            return false;
        }
        if(ppc.DrawableID > 15)
        {
            return true;
        }
        return false;
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
        SetDefaultMask();


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

        string propHatText = "Toggle Hat";
        string propHatTextDesc = "Take hat on or off";

        if(IsPlayerMainCharacter)
        {
            propHatText = "Toggle Hat/Mask";
            propHatTextDesc = "Take hat/mask on or off";
        }

        UIMenuItem HatMenuItem = new UIMenuItem(propHatText, propHatTextDesc);
        HatMenuItem.Activated += (sender, selectedItem) =>
        {
            ToggleHat();
        };
        if (HasProp(0))
        {
            HeadUIMenu.AddItem(HatMenuItem);
        }



        UIMenuItem HelmetMenuItem = new UIMenuItem("Toggle Helmet", "Take helmet on or off");
        HelmetMenuItem.Activated += (sender, selectedItem) =>
        {
            ToggleHelmet();
        };

        HeadUIMenu.AddItem(HelmetMenuItem);
        


        UIMenuItem MaskMenuItem = new UIMenuItem("Toggle Mask", "Take mask on or off");
        MaskMenuItem.Activated += (sender, selectedItem) =>
        {
            ToggleMask();
        };
        if (IsPlayerFreeMode)// && HasComponent(1))
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



        UIMenuItem GlovesMenuItem = new UIMenuItem("Toggle Gloves", "Take gloves on or off");
        GlovesMenuItem.Activated += (sender, selectedItem) =>
        {
            ToggleGloves();
        };
        if (IsPlayerFreeMode && HasGloves())
        {
            BodyUIMenu.AddItem(GlovesMenuItem);
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
        if (IsPlayerFreeMode && HasComponent(7))
        {
            BodyUIMenu.AddItem(AccessoriesMenuItem);
        }
        UIMenuItem ShirtMenuItem = new UIMenuItem("Toggle Shirt", "Take shirt on or off");
        ShirtMenuItem.Activated += (sender, selectedItem) =>
        {
            ToggleShirt();
        };
        if (IsPlayerFreeMode)
        {
            BodyUIMenu.AddItem(ShirtMenuItem);
        }
        UIMenu LegsUIMenu = menuPool.AddSubMenu(AccessoryUIMenu, "Legs");
        UIMenuItem ShoesMenuItem = new UIMenuItem("Toggle Shoes", "Take shoes on or off");
        ShoesMenuItem.Activated += (sender, selectedItem) =>
        {
            ToggleShoes();
        };

        if (IsPlayerFreeMode)
        {
            LegsUIMenu.AddItem(ShoesMenuItem);
        }
        UIMenuItem PantsMenuItem = new UIMenuItem("Toggle Pants", "Take pants on or off");
        PantsMenuItem.Activated += (sender, selectedItem) =>
        {
            TogglePants();
        };
        if (IsPlayerFreeMode)
        {
            LegsUIMenu.AddItem(PantsMenuItem);
        }

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
    private void SetDefaultMask()
    {
        if(IsPlayerFreeMode)
        {
            if (HasComponent(1))
            {
                return;
            }
            if (Player.CurrentModelVariation == null || Player.CurrentModelVariation.Components == null)
            {
                return;
            }
            Player.CurrentModelVariation.Components.RemoveAll(x => x.ComponentID == 1);
            Player.CurrentModelVariation.Components.Add(new PedComponent(1, DefaultMaskDrawableID, 0, 0) { IsDefaultNotApplied = true });
            EntryPoint.WriteToConsole("ADDED A DEFAULT MASK TO THE FREEMODE PED");
        }
        else if (IsPlayerMainCharacter)
        {
            if(HasProp(0))
            {
                return;
            }
            if (Player.CurrentModelVariation == null || Player.CurrentModelVariation.Props == null)
            {
                return;
            }
            Player.CurrentModelVariation.Props.RemoveAll(x => x.PropID == 0);
            Player.CurrentModelVariation.Props.Add(new PedPropComponent(0, DefaultMaskDrawableID, 0) { IsDefaultNotApplied = true });
            EntryPoint.WriteToConsole("ADDED A DEFAULT MASK TO THE MAIN CHAR PED");
        }


        
    }
    public void CreateOutfitMenu(MenuPool menuPool, UIMenu subMenu, bool doAnimations, bool removeBanner, bool AddPurchased, bool MakeVisible)
    {
        EntryPoint.WriteToConsole("CreateOutfitMenu RAN!");
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

        if (AddPurchased)
        {
            EntryPoint.WriteToConsole($"DEBUG PURCHASED ITEMS RAN! AddPurchased{AddPurchased}");
            UIMenu PurchasedItemsSubMenu = menuPool.AddSubMenu(subMenu, "Purchased Items");
            clothingPurchaseMenuProcess = new ClothingPurchaseMenu(LocationInteractable, null, null, Settings);
            clothingPurchaseMenuProcess.Start(menuPool, PurchasedItemsSubMenu, null, PurchasedPedClothingShopMenuItems, false, MakeVisible);
        }
        //clothingPurchaseMenuProcess.Dispose();
    }
    public void TakeOffArmorVisually()
    {
        SetComponent(9, false, true, "mp_safehouseseated@female@generic@idle_b", "idle_d");
    }
    public void PutOnArmorVisually()
    {
        SetComponent(9,true, true, "mp_safehouseseated@female@generic@idle_b", "idle_d");
    }



    public void PurchasePedClothingItem(PedClothingShopMenuItem purchasedItem)
    {
        if(purchasedItem == null)
        {
            return;
        }
        PurchasedPedClothingShopMenuItems.RemoveAll(x => x.Name == purchasedItem.Name && x.Category == purchasedItem.Category && x.ModelNames.All(purchasedItem.ModelNames.Contains));
        PurchasedPedClothingShopMenuItems.Add(purchasedItem);
        EntryPoint.WriteToConsole($"You purchased {purchasedItem.Name} FOR:{string.Join(",", purchasedItem.ModelNames)} Category:{purchasedItem.Category}");
    }
    private class TorsoLookup
    {
        public TorsoLookup(int assignedComponenetID, int matchningTorsoComponenetID)
        {
            AssignedComponenetID = assignedComponenetID;
            MatchingTorsoComponenetID = matchningTorsoComponenetID;
        }

        public int AssignedComponenetID { get; set; }
        public int MatchingTorsoComponenetID { get; set; }
    }

}
