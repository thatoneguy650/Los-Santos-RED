using LosSantosRED.lsr.Data;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class SavedOutfits : ISavedOutfits
{
    private readonly string ConfigFileName = "Plugins\\LosSantosRED\\SavedOutfits.xml";
    public SavedOutfits()
    {
    }
    public List<SavedOutfit> SavedOutfitList { get; private set; } = new List<SavedOutfit>();
    public void ReadConfig()
    {
        DirectoryInfo LSRDirectory = new DirectoryInfo("Plugins\\LosSantosRED");
        FileInfo ConfigFile = LSRDirectory.GetFiles("SavedOutfits*.xml").OrderByDescending(x => x.Name).FirstOrDefault();
        if (ConfigFile != null)
        {
            EntryPoint.WriteToConsole($"Loaded Saved Outfits config: {ConfigFile.FullName}", 0);
            SavedOutfitList = Serialization.DeserializeParams<SavedOutfit>(ConfigFile.FullName);
        }
        else if (File.Exists(ConfigFileName))
        {
            EntryPoint.WriteToConsole($"Loaded Saved Outfits config  {ConfigFileName}", 0);
            SavedOutfitList = Serialization.DeserializeParams<SavedOutfit>(ConfigFileName);
        }
        else
        {
            EntryPoint.WriteToConsole($"No Saved Outfits config found, creating default", 0);
            DefaultConfig();
        }
    }
    public void AddOutfit(SavedOutfit so)
    {
        SavedOutfitList.Add(so);
        Serialization.SerializeParams(SavedOutfitList, ConfigFileName);
    }
    public void RemoveOutfit(SavedOutfit so)
    {
        SavedOutfitList.Remove(so);
        Serialization.SerializeParams(SavedOutfitList, ConfigFileName);
    }
    private void DefaultConfig()
    {
        SavedOutfitList = new List<SavedOutfit>();
        AddAlexis();
        AddClaude();
        AddLamar();
        AddNicholasClark();
        AddFranklin();
        Serialization.SerializeParams(SavedOutfitList, ConfigFileName);
    }
    private void AddAlexis()
    {
        PedVariation AlexisVariation = new PedVariation(new List<PedComponent>()
        {
            new PedComponent(0, 0, 0, 0),
            new PedComponent(1, 0, 0, 0),
            new PedComponent(2, 42, 0, 0) ,
            new PedComponent(3, 14, 0, 0) ,
            new PedComponent(4, 11, 8, 0) ,
            new PedComponent(5, 0, 0, 0) ,
            new PedComponent(6, 11, 2, 0) ,
            new PedComponent(7, 0, 0, 0) ,
            new PedComponent(8, 8, 0, 0) ,
            new PedComponent(9, 0, 0, 0) ,
            new PedComponent(10, 0, 0, 0) ,
            new PedComponent(11, 49, 1, 0)
        },
        new List<PedPropComponent>()
        {

        },
        new List<HeadOverlayData>()
        {
            new HeadOverlayData(0,"Blemishes") { Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0},
            new HeadOverlayData(1, "Facial Hair") { ColorType = 1,Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(2, "Eyebrows") { ColorType = 1,Index = 3,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(3, "Ageing") {Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(4, "Makeup") { Index = 12,Opacity = 0.5f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(5, "Blush") { ColorType = 2, Index = 3,Opacity = 0.4f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(6, "Complexion"){ Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(7, "Sun Damage"){ Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(8, "Lipstick") { ColorType = 2, Index = 2,Opacity = 0.6f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(9, "Moles/Freckles"){ Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(10, "Chest Hair") { ColorType = 1, Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(11, "Body Blemishes") { Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(12, "Add Body Blemishes"){Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
        },
        new HeadBlendData(12, 31, 0, 12, 31, 0, 0.8f, 0.2f, 0.0f),
        11,
        12);
        SavedOutfit AlexisDefaultOutfit = new SavedOutfit("Default", "MP_F_FREEMODE_01","Alexis Davis",AlexisVariation);
        SavedOutfitList.Add(AlexisDefaultOutfit);

        PedVariation AlexisVariation2 = new PedVariation(new List<PedComponent>()
        {
            new PedComponent(0, 0, 0, 0),
            new PedComponent(1, 0, 0, 0),
            new PedComponent(2, 42, 0, 0) ,
            new PedComponent(3, 9, 0, 0) ,
            new PedComponent(4, 9, 11, 0) ,
            new PedComponent(5, 0, 0, 0) ,
            new PedComponent(6, 3, 12, 0) ,
            new PedComponent(7, 0, 0, 0) ,
            new PedComponent(8, 0, 0, 0) ,
            new PedComponent(9, 0, 0, 0) ,
            new PedComponent(10, 0, 0, 0) ,
            new PedComponent(11, 9, 13, 0)
        },
        new List<PedPropComponent>()
        {

        },
        new List<HeadOverlayData>() {
            new HeadOverlayData(0,"Blemishes") { Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0},
            new HeadOverlayData(1, "Facial Hair") { ColorType = 1,Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(2, "Eyebrows") { ColorType = 1,Index = 3,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(3, "Ageing") {Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(4, "Makeup") { Index = 12,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(5, "Blush") { ColorType = 2, Index = 3,Opacity = 0.4f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(6, "Complexion"){ Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(7, "Sun Damage"){ Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(8, "Lipstick") { ColorType = 2, Index = 2,Opacity = 0.6f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(9, "Moles/Freckles"){ Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(10, "Chest Hair") { ColorType = 1, Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(11, "Body Blemishes") { Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(12, "Add Body Blemishes"){Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 }, 
        },
        new HeadBlendData(12, 31, 0, 12, 31, 0, 0.8f, 0.2f, 0.0f),
        11,
        12);
        SavedOutfit AlexisDefaultOutfit2 = new SavedOutfit("Classic", "MP_F_FREEMODE_01", "Alexis Davis", AlexisVariation2);
        SavedOutfitList.Add(AlexisDefaultOutfit2);

        PedVariation AlexisVariation3 = new PedVariation(new List<PedComponent>()
        {
            new PedComponent(0, 0, 0, 0),
            new PedComponent(1, 0, 0, 0),
            new PedComponent(2, 42, 0, 0) ,
            new PedComponent(3, 7, 0, 0) ,
            new PedComponent(4, 0, 1, 0) ,
            new PedComponent(5, 0, 0, 0) ,
            new PedComponent(6, 20, 2, 0) ,
            new PedComponent(7, 0, 0, 0) ,
            new PedComponent(8, 64, 0, 0) ,
            new PedComponent(9, 0, 0, 0) ,
            new PedComponent(10, 0, 0, 0) ,
            new PedComponent(11, 64, 0, 0)
        },
        new List<PedPropComponent>()
        {

        },
        new List<HeadOverlayData>()
        {
            new HeadOverlayData(0,"Blemishes") { Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0},
            new HeadOverlayData(1, "Facial Hair") { ColorType = 1,Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(2, "Eyebrows") { ColorType = 1,Index = 3,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(3, "Ageing") {Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(4, "Makeup") { Index = 4,Opacity = 0.1f, PrimaryColor = 40,SecondaryColor = 0 },
            new HeadOverlayData(5, "Blush") { ColorType = 2, Index = 3,Opacity = 0.4f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(6, "Complexion"){ Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(7, "Sun Damage"){ Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(8, "Lipstick") { ColorType = 2, Index = 2,Opacity = 0.6f, PrimaryColor = 2,SecondaryColor = 0 },
            new HeadOverlayData(9, "Moles/Freckles"){ Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(10, "Chest Hair") { ColorType = 1, Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(11, "Body Blemishes") { Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(12, "Add Body Blemishes"){Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
        },
        new HeadBlendData(12, 31, 0, 12, 31, 0, 0.8f, 0.2f, 0.0f),
        11,
        12);
        SavedOutfit AlexisDefaultOutfit3 = new SavedOutfit("Coat", "MP_F_FREEMODE_01", "Alexis Davis", AlexisVariation3);
        SavedOutfitList.Add(AlexisDefaultOutfit3);

        PedVariation AlexisVariation4 = new PedVariation(new List<PedComponent>()
        {
            new PedComponent(0, 0, 0, 0),
            new PedComponent(1, 0, 0, 0),
            new PedComponent(2, 49, 0, 0) ,
            new PedComponent(3, 15, 0, 0) ,
            new PedComponent(4, 14, 1, 0) ,
            new PedComponent(5, 0, 0, 0) ,
            new PedComponent(6, 5, 1, 0) ,
            new PedComponent(7, 0, 0, 0) ,
            new PedComponent(8, 8, 0, 0) ,
            new PedComponent(9, 0, 0, 0) ,
            new PedComponent(10, 0, 0, 0) ,
            new PedComponent(11, 4, 10, 0)
        },
        new List<PedPropComponent>()
        {

        },
        new List<HeadOverlayData>()
        {
            new HeadOverlayData(0,"Blemishes") { Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0},
            new HeadOverlayData(1, "Facial Hair") { ColorType = 1,Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(2, "Eyebrows") { ColorType = 1,Index = 2,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(3, "Ageing") {Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(4, "Makeup") { Index = 1,Opacity = 0.6f, PrimaryColor = 40,SecondaryColor = 0 },
            new HeadOverlayData(5, "Blush") { ColorType = 2, Index = -1,Opacity = 0.6f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(6, "Complexion"){ Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(7, "Sun Damage"){ Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(8, "Lipstick") { ColorType = 2, Index = 2,Opacity = 0.6f, PrimaryColor = 2,SecondaryColor = 0 },
            new HeadOverlayData(9, "Moles/Freckles"){ Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(10, "Chest Hair") { ColorType = 1, Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(11, "Body Blemishes") { Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(12, "Add Body Blemishes"){Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
        },
        new HeadBlendData(12, 31, 0, 12, 31, 0, 0.8f, 0.2f, 0.0f),
        4,
        3);
        SavedOutfit AlexisDefaultOutfit4 = new SavedOutfit("Summer", "MP_F_FREEMODE_01", "Alexis Davis", AlexisVariation4);
        SavedOutfitList.Add(AlexisDefaultOutfit4);
    }
    private void AddClaude()
    {
        PedVariation pedVariation = new PedVariation(
        new List<PedComponent>()
        {
            new PedComponent(0, 0, 0, 0),
            new PedComponent(1, 0, 0, 0),
            new PedComponent(2, 38, 0, 0) ,
            new PedComponent(3, 0, 0, 0) ,
            new PedComponent(4, 129, 2, 0) ,
            new PedComponent(5, 0, 0, 0) ,
            new PedComponent(6, 31, 2, 0) ,
            new PedComponent(7, 0, 0, 0) ,
            new PedComponent(8, 15, 0, 0) ,
            new PedComponent(9, 0, 0, 0) ,
            new PedComponent(10, 0, 0, 0) ,
            new PedComponent(11, 0, 1, 0)
        },
        new List<PedPropComponent>()
        {

        },
        new List<HeadOverlayData>() {
            new HeadOverlayData(0,"Blemishes") { Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0},
            new HeadOverlayData(1, "Facial Hair") { ColorType = 1,Index = 0,Opacity = 0.6f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(2, "Eyebrows") { ColorType = 1,Index = 3,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(3, "Ageing") {Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(4, "Makeup") { Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(5, "Blush") { ColorType = 2, Index = 255,Opacity = 0.4f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(6, "Complexion"){ Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(7, "Sun Damage"){ Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(8, "Lipstick") { ColorType = 2, Index = 255,Opacity = 0.6f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(9, "Moles/Freckles"){ Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(10, "Chest Hair") { ColorType = 1, Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(11, "Body Blemishes") { Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(12, "Add Body Blemishes"){Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },},
        new HeadBlendData(42, 42, 0, 42, 42, 0, 1.0f, 0.0f, 0.0f),
        3,
        3);
        SavedOutfit savedOutfit = new SavedOutfit("Default", "MP_M_FREEMODE_01", "Claude Speed", pedVariation);
        SavedOutfitList.Add(savedOutfit);

        PedVariation pedVariation2 = new PedVariation(
        new List<PedComponent>()
        {
            new PedComponent(0, 0, 0, 0),
            new PedComponent(1, 0, 0, 0),
            new PedComponent(2, 38, 0, 0) ,
            new PedComponent(3, 1, 0, 0) ,
            new PedComponent(4, 129, 2, 0) ,
            new PedComponent(5, 0, 0, 0) ,
            new PedComponent(6, 31, 2, 0) ,
            new PedComponent(7, 0, 0, 0) ,
            new PedComponent(8, 0, 2, 0) ,
            new PedComponent(9, 0, 0, 0) ,
            new PedComponent(10, 0, 0, 0) ,
            new PedComponent(11, 163, 0, 0)
        },
        new List<PedPropComponent>()
        {

        },
        new List<HeadOverlayData>() {
            new HeadOverlayData(0,"Blemishes") { Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0},
            new HeadOverlayData(1, "Facial Hair") { ColorType = 1,Index = 0,Opacity = 0.6f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(2, "Eyebrows") { ColorType = 1,Index = 3,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(3, "Ageing") {Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(4, "Makeup") { Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(5, "Blush") { ColorType = 2, Index = 255,Opacity = 0.4f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(6, "Complexion"){ Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(7, "Sun Damage"){ Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(8, "Lipstick") { ColorType = 2, Index = 255,Opacity = 0.6f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(9, "Moles/Freckles"){ Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(10, "Chest Hair") { ColorType = 1, Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(11, "Body Blemishes") { Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },
            new HeadOverlayData(12, "Add Body Blemishes"){Index = 255,Opacity = 1.0f, PrimaryColor = 0,SecondaryColor = 0 },},
        new HeadBlendData(42, 42, 0, 42, 42, 0, 1.0f, 0.0f, 0.0f),
        3,
        3);
        SavedOutfit savedOutfit2 = new SavedOutfit("Classic", "MP_M_FREEMODE_01", "Claude Speed", pedVariation2);
        SavedOutfitList.Add(savedOutfit2);

    }
    private void AddLamar()
    {
        PedVariation pedVariation = new PedVariation(
            new List<PedComponent>() {
                new PedComponent(1, 2, 0, 0),
                new PedComponent(2, 2, 0, 0),
                new PedComponent(3, 2, 2, 0),
                new PedComponent(4, 5, 0, 0),
                new PedComponent(5, 0, 0, 0),
                new PedComponent(6, 1, 0, 0),
                new PedComponent(7, 0, 0, 0),
                new PedComponent(8, 0, 0, 0),
                new PedComponent(9, 0, 0, 0),
                new PedComponent(10, 1, 0, 0),
                new PedComponent(11, 0, 0, 0),
            },
            new List<PedPropComponent>() { });

        SavedOutfit savedOutfit = new SavedOutfit("Default", "ig_lamardavis", "Lamar Davis", pedVariation);
        SavedOutfitList.Add(savedOutfit);
    }
    private void AddNicholasClark()
    {
        PedVariation pedVariation = new PedVariation(new List<PedComponent>() {
            new PedComponent(3,1,0,0),
            new PedComponent(4, 35, 0, 0),
            new PedComponent(6, 10, 0, 0),
            new PedComponent(7, 125, 0, 0),
            new PedComponent(8,130,0,0),
            new PedComponent(10,0,0,0),
            new PedComponent(11,348,0,0),
            new PedComponent(2,56,0,0),
        }, new List<PedPropComponent>() { })
        {
            HeadOverlays = new List<HeadOverlayData>() { new HeadOverlayData(2, "Eyebrows") { Opacity = 1.0f, Index = 0, ColorType = 1, PrimaryColor = 0, SecondaryColor = 0 } },
            HeadBlendData = new HeadBlendData(9, 20, 0, 9, 20, 0, 1, 0, 0),
            PrimaryHairColor = 27,
            SecondaryHairColor = 7,
            EyeColor = 4,
            FaceFeatures = new List<FaceFeature>() {
                new FaceFeature(13, "Jaw Bone Width") { Scale = 0.0f,RangeLow = -1.0f, RangeHigh = 1.0f },
                new FaceFeature(12, "Lip Thickness") { Scale = 0.9f,RangeLow = -1.0f, RangeHigh = 1.0f },
                new FaceFeature(10, "Cheek Bones Width") { Scale = -0.7f,RangeLow = -1.0f, RangeHigh = 1.0f },
                new FaceFeature(7, "Eyebrow In/Out") { Scale = 0.3f,RangeLow = -1.0f, RangeHigh = 1.0f },
                new FaceFeature(4, "Nose Tip") { Scale = 1.0f,RangeLow = -1.0f, RangeHigh = 1.0f },
                new FaceFeature(0, "Nose Width") { Scale = 1.0f,RangeLow = -1.0f, RangeHigh = 1.0f },
            },
        };
        SavedOutfit savedOutfit = new SavedOutfit("Default","mp_m_freemode_01", "Nicholas Clark", pedVariation);
        SavedOutfitList.Add(savedOutfit);
    }
    private void AddFranklin()
    {
        PedVariation pedVariation1 = new PedVariation(
            new List<PedComponent>() {
                new PedComponent(1, 0, 0, 0),
                new PedComponent(2, 0, 0, 0),
                new PedComponent(3, 8, 0, 0),
                new PedComponent(4, 8, 0, 0),
                new PedComponent(5, 0, 0, 0),
                new PedComponent(6, 6, 0, 0),
                new PedComponent(7, 0, 0, 0),
                new PedComponent(8, 14, 0, 0),
                new PedComponent(9, 0, 0, 0),
                new PedComponent(10, 0, 0, 0),
                new PedComponent(11, 0, 0, 0),
            },
            new List<PedPropComponent>() {
               
            });
        SavedOutfit savedOutfit1 = new SavedOutfit("Default", "player_one", "Franklin Clinton", pedVariation1);
        SavedOutfitList.Add(savedOutfit1);

        PedVariation pedVariation2 = new PedVariation(
            new List<PedComponent>() {
                new PedComponent(0, 0, 0, 0),
                new PedComponent(1, 0, 0, 0),
                new PedComponent(2, 0, 0, 0),
                new PedComponent(3, 12, 0, 0),
                new PedComponent(4, 8, 5, 0),
                new PedComponent(5, 0, 0, 0),
                new PedComponent(6, 4, 0, 0),
                new PedComponent(7, 0, 0, 0),
                new PedComponent(8, 14, 0, 0),
                new PedComponent(9, 0, 0, 0),
                new PedComponent(10, 5, 6, 0),
                new PedComponent(11, 0, 0, 0),
            },
            new List<PedPropComponent>()
            {
                new PedPropComponent(0,16,1),

            });
        SavedOutfit savedOutfit2 = new SavedOutfit("Letterman", "player_one", "Franklin Clinton", pedVariation2);
        SavedOutfitList.Add(savedOutfit2);

        PedVariation pedVariation3 = new PedVariation(
            new List<PedComponent>() {
                new PedComponent(0, 0, 0, 0),
                new PedComponent(1, 0, 0, 0),
                new PedComponent(2, 0, 0, 0),
                new PedComponent(3, 7, 3, 0),
                new PedComponent(4, 2, 0, 0),
                new PedComponent(5, 0, 0, 0),
                new PedComponent(6, 14, 1, 0),
                new PedComponent(7, 0, 0, 0),
                new PedComponent(8, 14, 0, 0),
                new PedComponent(9, 0, 0, 0),
                new PedComponent(10, 0, 0, 0),
                new PedComponent(11, 0, 0, 0),
            },
            new List<PedPropComponent>()
            {
                new PedPropComponent(0,16,1),

            });
        SavedOutfit savedOutfit3 = new SavedOutfit("Casual", "player_one", "Franklin Clinton", pedVariation3);
        SavedOutfitList.Add(savedOutfit3);
    }
}

