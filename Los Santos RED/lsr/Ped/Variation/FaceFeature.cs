using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class FaceFeature
{
    private Ped Ped;
    private PedCustomizer PedCustomizer;

    public int Index { get; set; }
    public string Name { get; set; }    
    public float Scale { get; set; }
    public float RangeLow { get; set; } = -1.0f;
    public float RangeHigh { get; set; } = 1.0f;
    public FaceFeature()
    {

    }
    public FaceFeature(int index, string name)
    {
        Index = index;
        Name = name;
    }


    public void AddCustomizeMenu(MenuPool menuPool, UIMenu faceSubMenu, Ped modelPed, PedCustomizer pedCustomizer)
    {
        Ped = modelPed;
        PedCustomizer = pedCustomizer;
        UIMenu componentMenu = menuPool.AddSubMenu(faceSubMenu, Name);
        faceSubMenu.MenuItems[faceSubMenu.MenuItems.Count() - 1].Description = $"Customize the {Name}";
        faceSubMenu.MenuItems[faceSubMenu.MenuItems.Count() - 1].RightLabel = "";
        componentMenu.SetBannerType(EntryPoint.LSRedColor);
        if (Ped.Exists())
        {
            AddMenuItems(componentMenu);
        }
    }

    private void AddMenuItems(UIMenu componentMenu)
    {
        AddScaleItem(componentMenu);
    }
    private void AddScaleItem(UIMenu componentMenu)
    {
        UIMenuNumericScrollerItem<float> FeatureMenu = new UIMenuNumericScrollerItem<float>(Name, "Set the face feature",RangeLow, RangeHigh, 0.1f);
        FeatureMenu.Activated += (sender, e) =>
        {
            OnScaleItemChanged(FeatureMenu.Value);
        };
        FeatureMenu.IndexChanged += (sender, oldIndex, newIndex) =>
        {
            OnScaleItemChanged(FeatureMenu.Value);
        };
        componentMenu.AddItem(FeatureMenu);
    }

    private void OnScaleItemChanged(float selectedscale)
    {
        if (PedCustomizer.PedCustomizerMenu.IsProgramicallySettingFieldValues)
        {
            return;
        }
        Scale = selectedscale;
        FaceFeature ff = PedCustomizer.WorkingVariation.FaceFeatures.FirstOrDefault(x => x.Name == Name);
        if (ff == null)
        {
            PedCustomizer.WorkingVariation.FaceFeatures.Add(new FaceFeature(Index, Name) { Scale = Scale, RangeLow = RangeLow, RangeHigh = RangeHigh });
        }
        else
        {
            ff.Scale = Scale;
        }
        PedCustomizer.OnVariationChanged();
    }
}

