using ExtensionsMethods;
using LosSantosRED.lsr;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

    public List<AppliedOverlay> AppliedOverlays { get; set; } = new List<AppliedOverlay>();

    public PedVariation ApplyToPed(Ped ped)
    {
        return ApplyToPed(ped, false, false);
    }
    //public PedVariation ApplyToPed(Ped ped, bool setDefaultFirst)
    //{
    //    if (setDefaultFirst)
    //    {
    //        NativeFunction.Natives.SET_PED_DEFAULT_COMPONENT_VARIATION(ped);
    //    }
    //    return ApplyToPed(ped);
    //}
    public PedVariation ApplyToPed(Ped ped, bool setDefaultFirst, bool checkComponentValid)
    {
        try
        {
            if (setDefaultFirst)
            {
                NativeFunction.Natives.SET_PED_DEFAULT_COMPONENT_VARIATION(ped);
            }


            PedVariation setVariation = new PedVariation();
            foreach (PedComponent Component in Components)
            {
                if (!checkComponentValid || NativeFunction.Natives.IS_PED_COMPONENT_VARIATION_VALID<bool>(ped, Component.ComponentID, Component.DrawableID, Component.TextureID))
                {
                    NativeFunction.Natives.SET_PED_COMPONENT_VARIATION(ped, Component.ComponentID, Component.DrawableID, Component.TextureID, Component.PaletteID);
                    setVariation.Components.Add(new PedComponent(Component.ComponentID, Component.DrawableID, Component.TextureID, Component.PaletteID));
                }
            }
            NativeFunction.Natives.CLEAR_ALL_PED_PROPS(ped);
            foreach (PedPropComponent Prop in Props)
            {
                NativeFunction.Natives.SET_PED_PROP_INDEX(ped, Prop.PropID, Prop.DrawableID, Prop.TextureID, false);
                setVariation.Props.Add(new PedPropComponent(Prop.PropID, Prop.DrawableID, Prop.TextureID));
            }
            NativeFunction.Natives.CLEAR_PED_DECORATIONS(ped);
            if (AppliedOverlays != null && AppliedOverlays.Any())
            {
                //NativeFunction.Natives.CLEAR_PED_DECORATIONS(ped);
                foreach(AppliedOverlay ao in AppliedOverlays)
                {
                    uint collectionHash = Game.GetHashKey(ao.CollectionName);
                    uint overlayHash = Game.GetHashKey(ao.OverlayName);
                    NativeFunction.Natives.ADD_PED_DECORATION_FROM_HASHES(ped, collectionHash, overlayHash);
                    setVariation.AppliedOverlays.Add(new AppliedOverlay(ao.CollectionName, ao.OverlayName, ao.ZoneName, collectionHash, overlayHash));
                }
            }
            //Freemode only below
            if (HeadBlendData != null && (HeadBlendData.shapeFirst != -1 || HeadBlendData.shapeSecond != -1 || HeadBlendData.shapeThird != -1))
            {
                //EntryPoint.WriteToConsoleTestLong("FREEMODE APPLY");
                NativeFunction.Natives.SET_PED_HEAD_BLEND_DATA(ped, HeadBlendData.shapeFirst, HeadBlendData.shapeSecond, HeadBlendData.shapeThird, HeadBlendData.skinFirst, HeadBlendData.skinSecond, HeadBlendData.skinThird, HeadBlendData.shapeMix, HeadBlendData.skinMix, HeadBlendData.thirdMix, false);
                setVariation.HeadBlendData = new HeadBlendData(HeadBlendData.shapeFirst, HeadBlendData.shapeSecond, HeadBlendData.shapeThird, HeadBlendData.skinFirst, HeadBlendData.skinSecond, HeadBlendData.skinThird, HeadBlendData.shapeMix, HeadBlendData.skinMix, HeadBlendData.thirdMix);
                if (PrimaryHairColor != -1 && SecondaryHairColor != -1)
                {
                    NativeFunction.Natives.x4CFFC65454C93A49(ped, PrimaryHairColor, SecondaryHairColor);
                    setVariation.PrimaryHairColor = PrimaryHairColor;
                    setVariation.SecondaryHairColor = SecondaryHairColor;
                }
                foreach (HeadOverlayData headOverlayData in HeadOverlays)
                {
                    //EntryPoint.WriteToConsoleTestLong($"FREEMODE APPLY OVERLAYS {headOverlayData.OverlayID} {headOverlayData.Index} {headOverlayData.PrimaryColor} {headOverlayData.SecondaryColor} {headOverlayData.Opacity}");
                    NativeFunction.Natives.SET_PED_HEAD_OVERLAY(ped, headOverlayData.OverlayID, headOverlayData.Index, headOverlayData.Opacity);
                    NativeFunction.Natives.x497BF74A7B9CB952(ped, headOverlayData.OverlayID, headOverlayData.ColorType, headOverlayData.PrimaryColor, headOverlayData.SecondaryColor);//colors?
                    setVariation.HeadOverlays.Add(new HeadOverlayData(headOverlayData.OverlayID, headOverlayData.Part) { ColorType = headOverlayData.ColorType, Index = headOverlayData.Index, Opacity = headOverlayData.Opacity, OverlayID = headOverlayData.OverlayID, PrimaryColor = headOverlayData.PrimaryColor, SecondaryColor = headOverlayData.SecondaryColor });
                }
                foreach(FaceFeature faceFeature in FaceFeatures)
                {
                    //EntryPoint.WriteToConsoleTestLong($"APPLYING FACE FEATURE {faceFeature.Name} {faceFeature.Index} {faceFeature.Scale}");
                    NativeFunction.Natives.SET_PED_MICRO_MORPH(ped, faceFeature.Index, faceFeature.Scale);
                    setVariation.FaceFeatures.Add(new FaceFeature(faceFeature.Index, faceFeature.Name) { Index = faceFeature.Index, Scale = faceFeature.Scale, RangeLow = faceFeature.RangeLow, RangeHigh = faceFeature.RangeHigh });
                }
                if (EyeColor != -1)
                {
                    NativeFunction.Natives.x50B56988B170AFDF(ped, EyeColor);
                    setVariation.EyeColor = EyeColor;
                }
            }
            return setVariation;
        }
        catch (Exception ex)
        {
            EntryPoint.WriteToConsole($"ReplacePedComponentVariation Error {ex.Message} {ex.StackTrace}", 0);
        }
        return null;
    }
    public PedVariation ApplyToPedSlow(Ped ped, bool setDefaultFirst)
    {
        try
        {
            if (!ped.Exists())
            {
                return null;
            }
            PedVariation setVariation = new PedVariation();
            if (setDefaultFirst)
            {
                NativeFunction.Natives.SET_PED_DEFAULT_COMPONENT_VARIATION(ped);
                GameFiber.Yield();
            }
            if (!ped.Exists())
            {
                return setVariation;
            }
            foreach (PedComponent Component in Components)
            {
                GameFiber.Yield();
                if (!ped.Exists())
                {
                    return setVariation;
                }
                NativeFunction.Natives.SET_PED_COMPONENT_VARIATION(ped, Component.ComponentID, Component.DrawableID, Component.TextureID, Component.PaletteID);
                setVariation.Components.Add(new PedComponent(Component.ComponentID, Component.DrawableID, Component.TextureID, Component.PaletteID));
            }
            if (!ped.Exists())
            {
                return setVariation;
            }
            if (setDefaultFirst)
            {
                NativeFunction.Natives.CLEAR_ALL_PED_PROPS(ped);
            }
            foreach (PedPropComponent Prop in Props)
            {
                GameFiber.Yield();
                if (!ped.Exists())
                {
                    return setVariation;
                }
                NativeFunction.Natives.SET_PED_PROP_INDEX(ped, Prop.PropID, Prop.DrawableID, Prop.TextureID, false);
                setVariation.Props.Add(new PedPropComponent(Prop.PropID, Prop.DrawableID, Prop.TextureID));
            }
            if (!ped.Exists())
            {
                return setVariation;
            }
            if (AppliedOverlays != null && AppliedOverlays.Any())
            {
                NativeFunction.Natives.CLEAR_PED_DECORATIONS(ped);
                foreach (AppliedOverlay ao in AppliedOverlays)
                {
                    uint collectionHash = Game.GetHashKey(ao.CollectionName);
                    uint overlayHash = Game.GetHashKey(ao.OverlayName);
                    NativeFunction.Natives.ADD_PED_DECORATION_FROM_HASHES(ped, collectionHash, overlayHash);
                    setVariation.AppliedOverlays.Add(new AppliedOverlay(ao.CollectionName, ao.OverlayName, ao.ZoneName, collectionHash, overlayHash));
                }
            }
            //Freemode only below
            if (HeadBlendData != null && (HeadBlendData.shapeFirst != -1 || HeadBlendData.shapeSecond != -1 || HeadBlendData.shapeThird != -1))
            {
                NativeFunction.Natives.SET_PED_HEAD_BLEND_DATA(ped, HeadBlendData.shapeFirst, HeadBlendData.shapeSecond, HeadBlendData.shapeThird, HeadBlendData.skinFirst, HeadBlendData.skinSecond, HeadBlendData.skinThird, HeadBlendData.shapeMix, HeadBlendData.skinMix, HeadBlendData.thirdMix, false);
                setVariation.HeadBlendData = new HeadBlendData(HeadBlendData.shapeFirst, HeadBlendData.shapeSecond, HeadBlendData.shapeThird, HeadBlendData.skinFirst, HeadBlendData.skinSecond, HeadBlendData.skinThird, HeadBlendData.shapeMix, HeadBlendData.skinMix, HeadBlendData.thirdMix);
                GameFiber.Yield();
                if (!ped.Exists())
                {
                    return setVariation;
                }
                if (PrimaryHairColor != -1 && SecondaryHairColor != -1)
                {
                    NativeFunction.Natives.x4CFFC65454C93A49(ped, PrimaryHairColor, SecondaryHairColor);
                    setVariation.PrimaryHairColor = PrimaryHairColor;
                    setVariation.SecondaryHairColor = SecondaryHairColor;
                    GameFiber.Yield();
                }
                if (!ped.Exists())
                {
                    return setVariation;
                }
                foreach (HeadOverlayData headOverlayData in HeadOverlays)
                {
                    if (!ped.Exists())
                    {
                        return setVariation;
                    }
                    NativeFunction.Natives.SET_PED_HEAD_OVERLAY(ped, headOverlayData.OverlayID, headOverlayData.Index, headOverlayData.Opacity);
                    NativeFunction.Natives.x497BF74A7B9CB952(ped, headOverlayData.OverlayID, headOverlayData.ColorType, headOverlayData.PrimaryColor, headOverlayData.SecondaryColor);//colors?
                    setVariation.HeadOverlays.Add(new HeadOverlayData(headOverlayData.OverlayID, headOverlayData.Part) { ColorType = headOverlayData.ColorType, Index = headOverlayData.Index, Opacity = headOverlayData.Opacity, OverlayID = headOverlayData.OverlayID, PrimaryColor = headOverlayData.PrimaryColor, SecondaryColor = headOverlayData.SecondaryColor });
                    GameFiber.Yield();
                }
                if (!ped.Exists())
                {
                    return setVariation;
                }
                foreach (FaceFeature faceFeature in FaceFeatures)
                {
                    if (!ped.Exists())
                    {
                        return setVariation;
                    }
                    NativeFunction.Natives.x71A5C1DBA060049E(ped, faceFeature.Index, faceFeature.Scale);
                    setVariation.FaceFeatures.Add(new FaceFeature(faceFeature.Index, faceFeature.Name) { Index = faceFeature.Index, Scale = faceFeature.Scale, RangeLow = faceFeature.RangeLow, RangeHigh = faceFeature.RangeHigh });
                    GameFiber.Yield();
                }
                if (!ped.Exists())
                {
                    return setVariation;
                }
                if (EyeColor != -1)
                {
                    NativeFunction.Natives.x50B56988B170AFDF(ped, EyeColor);
                    setVariation.EyeColor = EyeColor;
                }
            }
            return setVariation;
        }
        catch (Exception ex)
        {
            EntryPoint.WriteToConsole($"ReplacePedComponentVariation Error {ex.Message} {ex.StackTrace}", 0);
        }
        return null;
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

