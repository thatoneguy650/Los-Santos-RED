using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class FaceFeature
{
    private Ped Ped;
    private PedCustomizer PedCustomizer;
    private UIMenuNumericScrollerItem<float> ChangeAmount;
    private float faceFeatureScale = 0.1f;
    private UIMenuNumericScrollerItem<float> FeatureMenu;

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

        ChangeAmount = new UIMenuNumericScrollerItem<float>("Scale Amount", "How much each increment will increase or decrease the amount", 0.01f, 0.5f, 0.01f);
        ChangeAmount.Value = 0.1f;
        ChangeAmount.Formatter = v => v.ToString("P0");
        ChangeAmount.IndexChanged += (sender, oldIndex, newIndex) =>
        {
            faceFeatureScale = ChangeAmount.Value;
            FeatureMenu.Step = ChangeAmount.Value;
        };
        componentMenu.AddItem(ChangeAmount);

        FeatureMenu = new UIMenuNumericScrollerItem<float>(Name, "Set the face feature",RangeLow, RangeHigh, faceFeatureScale);
        FeatureMenu.Value = 0.0f;
        FeatureMenu.Formatter = v => v.ToString("P0");
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

