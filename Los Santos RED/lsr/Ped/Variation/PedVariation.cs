using ExtensionsMethods;
using LosSantosRED.lsr;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
[Serializable]
public class PedVariation
{
    public PedVariation()
    {

    }
    public PedVariation(List<PedPropComponent> _MyPedProps)
    {
        Props = _MyPedProps;
    }
    public PedVariation(List<PedComponent> _MyPedComponents)
    {
        Components = _MyPedComponents;
    }
    public PedVariation(List<PedComponent> _MyPedComponents, List<PedPropComponent> _MyPedProps)
    {
        Components = _MyPedComponents;
        Props = _MyPedProps;
    }
    public PedVariation(List<PedComponent> _MyPedComponents, List<PedPropComponent> _MyPedProps, List<HeadOverlayData> headOverlays, HeadBlendData headBlendData, int primaryHairColor, int secondaryHairColor)
    {
        Components = _MyPedComponents;
        Props = _MyPedProps;
        HeadOverlays = headOverlays;
        HeadBlendData = headBlendData;
        PrimaryHairColor = primaryHairColor;
        SecondaryHairColor = secondaryHairColor;
    }
    public List<PedComponent> Components { get; set; } = new List<PedComponent>();
    public List<PedPropComponent> Props { get; set; } = new List<PedPropComponent>();
    public List<HeadOverlayData> HeadOverlays { get; set; } = new List<HeadOverlayData>();
    public HeadBlendData HeadBlendData { get; set; } = new HeadBlendData();
    public int PrimaryHairColor { get; set; } = -1;
    public int SecondaryHairColor { get; set; } = -1;


    public bool SetRandomRegularHeadVariation { get; set; } = false;
    public bool IsMaleRandomRegularHeadVariation { get; set; } = true;

    public void ApplyToPed(Ped ped)
    {
        try
        {
            NativeFunction.Natives.SET_PED_DEFAULT_COMPONENT_VARIATION(ped);
            foreach (PedComponent Component in Components)
            {
                NativeFunction.Natives.SET_PED_COMPONENT_VARIATION(ped, Component.ComponentID, Component.DrawableID, Component.TextureID, Component.PaletteID);
            }
            NativeFunction.Natives.CLEAR_ALL_PED_PROPS(ped);
            foreach (PedPropComponent Prop in Props)
            {
                NativeFunction.Natives.SET_PED_PROP_INDEX(ped, Prop.PropID, Prop.DrawableID, Prop.TextureID, false);
            }
            //Freemode only below
            if (HeadBlendData != null && (HeadBlendData.shapeFirst != -1 || HeadBlendData.shapeSecond != -1 || HeadBlendData.shapeThird != -1))
            {
                NativeFunction.Natives.SET_PED_HEAD_BLEND_DATA(ped, HeadBlendData.shapeFirst, HeadBlendData.shapeSecond, HeadBlendData.shapeThird, HeadBlendData.skinFirst, HeadBlendData.skinSecond, HeadBlendData.skinThird, HeadBlendData.shapeMix, HeadBlendData.skinMix, HeadBlendData.thirdMix, false);
                if (PrimaryHairColor != -1 && SecondaryHairColor != -1)
                {
                    NativeFunction.Natives.x4CFFC65454C93A49(ped, PrimaryHairColor, SecondaryHairColor);
                }
                foreach (HeadOverlayData headOverlayData in HeadOverlays)
                {
                    NativeFunction.Natives.SET_PED_HEAD_OVERLAY(ped, headOverlayData.OverlayID, headOverlayData.Index, headOverlayData.Opacity);
                    NativeFunction.Natives.x497BF74A7B9CB952(ped, headOverlayData.OverlayID, headOverlayData.ColorType, headOverlayData.PrimaryColor, headOverlayData.SecondaryColor);//colors?
                }
            }
        }
        catch (Exception ex)
        {
            EntryPoint.WriteToConsole($"ReplacePedComponentVariation Error {ex.Message} {ex.StackTrace}", 0);
        }
    }
    public void RandomizeHead(Ped ped, RandomHeadData myHead)
    {
        GameFiber.Yield();
        if (ped.Exists())
        {
            int HairColor = myHead.HairColors.PickRandom();
            int HairID = myHead.HairComponents.PickRandom();
            NativeFunction.Natives.SET_PED_COMPONENT_VARIATION(ped, 2, HairID, 0, 0);
            GameFiber.Yield();
            NativeFunction.Natives.SET_PED_HEAD_BLEND_DATA(ped, myHead.HeadID, myHead.HeadID, 0, myHead.HeadID, myHead.HeadID, 0, 1.0f, 0, 0, false);
            NativeFunction.Natives.x4CFFC65454C93A49(ped, HairColor, HairColor);
            NativeFunction.Natives.SET_PED_HEAD_OVERLAY(ped, 2, RandomItems.GetRandomNumberInt(0, 5), 1.0f);
            NativeFunction.Natives.x497BF74A7B9CB952(ped, 2, 1, HairColor, HairColor);//colors?
            GameFiber.Yield();
            //EntryPoint.WriteToConsole($"myHead {myHead.HeadID} {myHead.Name} HairID {HairID}");
            
        }
    }
    private void GetRegularRandomHead(Ped ped)
    {
        
        
    }
}

