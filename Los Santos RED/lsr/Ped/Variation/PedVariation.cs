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


    public PedVariation(List<PedPropComponent> _MyPedProps,List<PedComponent> _MyPedComponents)
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
    public List<FaceFeature> FaceFeatures { get; set; } = new List<FaceFeature>();
    public int EyeColor { get; set; } = -1;
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
                EntryPoint.WriteToConsole("FREEMODE APPLY");
                NativeFunction.Natives.SET_PED_HEAD_BLEND_DATA(ped, HeadBlendData.shapeFirst, HeadBlendData.shapeSecond, HeadBlendData.shapeThird, HeadBlendData.skinFirst, HeadBlendData.skinSecond, HeadBlendData.skinThird, HeadBlendData.shapeMix, HeadBlendData.skinMix, HeadBlendData.thirdMix, false);
                if (PrimaryHairColor != -1 && SecondaryHairColor != -1)
                {
                    NativeFunction.Natives.x4CFFC65454C93A49(ped, PrimaryHairColor, SecondaryHairColor);
                }
                foreach (HeadOverlayData headOverlayData in HeadOverlays)
                {
                    EntryPoint.WriteToConsole($"FREEMODE APPLY OVERLAYS {headOverlayData.OverlayID} {headOverlayData.Index} {headOverlayData.PrimaryColor} {headOverlayData.SecondaryColor} {headOverlayData.Opacity}");
                    NativeFunction.Natives.SET_PED_HEAD_OVERLAY(ped, headOverlayData.OverlayID, headOverlayData.Index, headOverlayData.Opacity);
                    NativeFunction.Natives.x497BF74A7B9CB952(ped, headOverlayData.OverlayID, headOverlayData.ColorType, headOverlayData.PrimaryColor, headOverlayData.SecondaryColor);//colors?
                }
                foreach(FaceFeature faceFeature in FaceFeatures)
                {
                    NativeFunction.Natives.x71A5C1DBA060049E(ped, faceFeature.Index, faceFeature.Scale);
                }
            }
        }
        catch (Exception ex)
        {
            EntryPoint.WriteToConsole($"ReplacePedComponentVariation Error {ex.Message} {ex.StackTrace}", 0);
        }
    }
    public void ApplyToPedSlow(Ped ped, bool setDefaultFirst)
    {
        try
        {
            if (ped.Exists())
            {
                if (setDefaultFirst)
                {
                    NativeFunction.Natives.SET_PED_DEFAULT_COMPONENT_VARIATION(ped);
                    GameFiber.Yield();
                }
                if (ped.Exists())
                {
                    foreach (PedComponent Component in Components)
                    {
                        GameFiber.Yield();
                        if (ped.Exists())
                        {
                            NativeFunction.Natives.SET_PED_COMPONENT_VARIATION(ped, Component.ComponentID, Component.DrawableID, Component.TextureID, Component.PaletteID);
                            GameFiber.Yield();
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (setDefaultFirst)
                    {
                        NativeFunction.Natives.CLEAR_ALL_PED_PROPS(ped);
                    }
                    foreach (PedPropComponent Prop in Props)
                    {
                        GameFiber.Yield();
                        if (ped.Exists())
                        {
                            NativeFunction.Natives.SET_PED_PROP_INDEX(ped, Prop.PropID, Prop.DrawableID, Prop.TextureID, false);
                            GameFiber.Yield();
                        }
                        else
                        {
                            break;
                        }
                    }
                    //Freemode only below
                    if (ped.Exists() && HeadBlendData != null && (HeadBlendData.shapeFirst != -1 || HeadBlendData.shapeSecond != -1 || HeadBlendData.shapeThird != -1))
                    {
                        NativeFunction.Natives.SET_PED_HEAD_BLEND_DATA(ped, HeadBlendData.shapeFirst, HeadBlendData.shapeSecond, HeadBlendData.shapeThird, HeadBlendData.skinFirst, HeadBlendData.skinSecond, HeadBlendData.skinThird, HeadBlendData.shapeMix, HeadBlendData.skinMix, HeadBlendData.thirdMix, false);
                        GameFiber.Yield();
                        if (ped.Exists())
                        {
                            if (PrimaryHairColor != -1 && SecondaryHairColor != -1)
                            {
                                NativeFunction.Natives.x4CFFC65454C93A49(ped, PrimaryHairColor, SecondaryHairColor);
                                GameFiber.Yield();
                            }
                            if (ped.Exists())
                            {
                                GameFiber.Yield();
                                foreach (HeadOverlayData headOverlayData in HeadOverlays)
                                {
                                    if (ped.Exists())
                                    {
                                        NativeFunction.Natives.SET_PED_HEAD_OVERLAY(ped, headOverlayData.OverlayID, headOverlayData.Index, headOverlayData.Opacity);
                                        NativeFunction.Natives.x497BF74A7B9CB952(ped, headOverlayData.OverlayID, headOverlayData.ColorType, headOverlayData.PrimaryColor, headOverlayData.SecondaryColor);//colors?



                                        foreach (FaceFeature faceFeature in FaceFeatures)
                                        {
                                            NativeFunction.Natives.x71A5C1DBA060049E(ped, faceFeature.Index, faceFeature.Scale);
                                        }



                                    }
                                    else
                                    {
                                        break;
                                    }
                                    GameFiber.Yield();
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            EntryPoint.WriteToConsole($"ReplacePedComponentVariation Error {ex.Message} {ex.StackTrace}", 0);
        }
    }
    public override string ToString()
    {
        string Output = "new PedVariation(new List<PedComponent>() { ";
        foreach (PedComponent Component in Components)
        {
            Output += $"new PedComponent({Component.ComponentID}, {Component.DrawableID}, {Component.TextureID}, {Component.PaletteID}),";
        }
        Output += " },new List<PedPropComponent>() { ";
        foreach (PedPropComponent Prop in Props)
        {
            Output += $"new PedPropComponent({Prop.PropID}, {Prop.DrawableID}, {Prop.TextureID}),";
        }
        Output += " }) },";
        return Output.ToString();
    }
}

