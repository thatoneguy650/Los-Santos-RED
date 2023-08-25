using Rage.Native;
using Rage;
using RAGENativeUI.Elements;
using RAGENativeUI;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using LosSantosRED.lsr.Helper;
using System;
using System.Windows.Media.Media3D;

public class FashionProp
{
    private UIMenuListScrollerItem<PedFashionAlias> DrawableMenuScroller;
    private UIMenuListScrollerItem<PedFashionAlias> TextureMenuScroller;
    private UIMenuItem ClearProps;
    private List<PedFashionAlias> PossibleDrawables;
    private List<PedFashionAlias> PossibleTextures;
    private Ped Ped;
    private PedCustomizer PedCustomizer;
    private string filterString = "";
    private UIMenuItem filterItems;
    private UIMenuItem ResetMenu;

    public FashionProp()
    {
    }
    public FashionProp(int propID, string propName)
    {
        PropID = propID;
        PropName = propName;
    }

    public int PropID { get; set; }
    public string PropName { get; set; }

    public void CombineCustomizeMenu(MenuPool MenuPool, UIMenu topMenu, Ped ped, PedCustomizer pedCustomizer)
    {
        Ped = ped;
        PedCustomizer = pedCustomizer;
        if (Ped.Exists())
        {
            AddMenuItems(topMenu);
        }
    }
    public void AddCustomizeMenu(MenuPool MenuPool, UIMenu topMenu, Ped ped, PedCustomizer pedCustomizer)
    {
        Ped = ped;
        PedCustomizer = pedCustomizer;
        UIMenu componentMenu = MenuPool.AddSubMenu(topMenu, PropName);
        topMenu.MenuItems[topMenu.MenuItems.Count() - 1].Description = $"Customize the {PropName}";
        topMenu.MenuItems[topMenu.MenuItems.Count() - 1].RightLabel = "";
        componentMenu.SetBannerType(EntryPoint.LSRedColor);
        if (Ped.Exists())
        {
            AddMenuItems(componentMenu);
        }
    }
    private void AddMenuItems(UIMenu componentMenu)
    {
        filterString = "";
        AddResetMenuItem(componentMenu);
        AddClearMenuItem(componentMenu);
        AddDrawableItem(componentMenu);
        AddTextureItem(componentMenu);
        AddGoToMenuItem(componentMenu);
        AddSearchMenuItem(componentMenu);
    }
    private void AddResetMenuItem(UIMenu componentMenu)
    {
        ResetMenu = new UIMenuItem("Reset", "Reset the item back to the initial value");
        PedPropComponent initialComponentStart = PedCustomizer.InitialVariation.Props.FirstOrDefault(x => x.PropID == PropID);
        if (initialComponentStart != null)
        {
            ResetMenu.Description = $"Reset the item back to the initial value ~n~({initialComponentStart.DrawableID},{initialComponentStart.TextureID})";
        }
        else
        {
            ResetMenu.Enabled = false;
        }
        ResetMenu.Activated += (sender, e) =>
        {
            SetToInitialValue();
        };
        componentMenu.AddItem(ResetMenu);
    }
    private void AddClearMenuItem(UIMenu componentMenu)
    {
        UIMenuItem RemoveMenu = new UIMenuItem("Remove Item", "Remove the prop from the ped");
        RemoveMenu.Activated += (sender, e) =>
        {
            SetPropRemoved();
        };
        componentMenu.AddItem(RemoveMenu);
    }
    private void AddDrawableItem(UIMenu componentMenu)
    {
        GetPossibleDrawables();
        DrawableMenuScroller = new UIMenuListScrollerItem<PedFashionAlias>("Item", "Select item", PossibleDrawables);
        SetDrawableValue(false);
        DrawableMenuScroller.IndexChanged += (Sender, oldIndex, newIndex) =>
        {
            OnComponentChanged(DrawableMenuScroller.SelectedItem.ID);
        };
        componentMenu.AddItem(DrawableMenuScroller);
    }
    private void AddTextureItem(UIMenu componentMenu)
    {
        PedPropComponent pedComponent = PedCustomizer.WorkingVariation.Props.FirstOrDefault(x => x.PropID == PropID);
        if (pedComponent != null)
        {
            GetPossibleTextures(pedComponent.DrawableID);
        }
        else
        {
            GetPossibleTextures(0);
        }
        TextureMenuScroller = new UIMenuListScrollerItem<PedFashionAlias>("Variation", "Select variation", PossibleTextures);
        SetTextureValue();
        TextureMenuScroller.IndexChanged += (Sender, oldIndex, newIndex) =>
        {
            OnTextureChanged();
        };
        componentMenu.AddItem(TextureMenuScroller);
    }
    private void AddGoToMenuItem(UIMenu componentMenu)
    {
        UIMenuItem goToDrawable = new UIMenuItem("Go To Item", "Enter a specific item number to auto select");
        goToDrawable.Activated += (sender, e) =>
        {
            SetToEnteredDrawableID();
        };
        componentMenu.AddItem(goToDrawable);
    }
    private void AddSearchMenuItem(UIMenu componentMenu)
    {
        filterItems = new UIMenuItem("Search", "Search for specific items matching the search terms");
        filterItems.Activated += (sender, e) =>
        {
            filterString = NativeHelper.GetKeyboardInput("");
            SetFiltering();
        };
        componentMenu.AddItem(filterItems);
    }


    private void GetPossibleDrawables()
    {
        int NumberOfDrawables = NativeFunction.Natives.GET_NUMBER_OF_PED_PROP_DRAWABLE_VARIATIONS<int>(Ped, PropID);
        PossibleDrawables = new List<PedFashionAlias>();
        bool isValid = true;
        for (int DrawableNumber = 0; DrawableNumber < NumberOfDrawables; DrawableNumber++)
        {
            string drawableName = $"({DrawableNumber})";
            string fullName = "";
            isValid = true;
            if (PedCustomizer.PedModelIsFreeMode)
            {
                isValid = false;
                HashSet<FashionItemLookup> possibleDrawables = PedCustomizer.ClothesNames.GetItemsFast(true, PropID, DrawableNumber, PedCustomizer.PedModelGender);
                if (string.IsNullOrEmpty(filterString) || filterString == "")
                {
                    isValid = true;
                }
                else if (possibleDrawables != null)
                {
                    isValid = possibleDrawables?.Any(x =>
                    (x.DrawableID.ToString().Contains(filterString.ToLower())) ||
                    (x.DrawableName != null && x.DrawableName.ToLower().Contains(filterString.ToLower())) ||
                    (x.Name != null && x.Name.ToLower().Contains(filterString.ToLower()))
                    ) == true;
                }
                FashionItemLookup fil = possibleDrawables?.OrderBy(x => x.TextureID).FirstOrDefault();
                if (fil != null)
                {
                    drawableName = fil.GetDrawableString();
                    fullName = fil.Name;
                }         
            }
            if (isValid)
            {
                PossibleDrawables.Add(new PedFashionAlias(DrawableNumber, drawableName));
            }
        }
    }
    private void SetDrawableValue(bool canGo)
    {
        if (DrawableMenuScroller.IsEmpty)
        {
            DrawableMenuScroller.Index = UIMenuScrollerItem.EmptyIndex;
        }
        else
        {
            DrawableMenuScroller.Index = 0;
        }
        PedPropComponent cpc = PedCustomizer.WorkingVariation.Props.Where(x => x.PropID == PropID).FirstOrDefault();
        if (cpc != null)
        {
            PedFashionAlias pfa = PossibleDrawables.Where(x => x.ID == cpc.DrawableID).FirstOrDefault();
            if (pfa != null)
            {
                DrawableMenuScroller.SelectedItem = pfa;
            }
        }
        else if (canGo)
        {
            PedFashionAlias pfa = PossibleDrawables.OrderBy(x => x.ID).FirstOrDefault();
            if (pfa != null)
            {
                DrawableMenuScroller.SelectedItem = pfa;
                //EntryPoint.WriteToConsoleTestLong($"SetDrawableValue SET ANY VARIATION {PropID} {pfa.ID} {pfa.Name} canGo {canGo}");
            }
        }
    }
    private void OnComponentChanged(int newDrawableID)
    {
        if (PedCustomizer.PedCustomizerMenu.IsProgramicallySettingFieldValues)
        {
            return;
        }
        //EntryPoint.WriteToConsoleTestLong("FP OnComponentChanged");
        GetPossibleTextures(newDrawableID);
        TextureMenuScroller.Items = PossibleTextures;
        SetTextureValue();
        int TextureID = !TextureMenuScroller.IsEmpty && TextureMenuScroller.SelectedItem != null ? TextureMenuScroller.SelectedItem.ID : 0;
        PedPropComponent pedComponent = PedCustomizer.WorkingVariation.Props.FirstOrDefault(x => x.PropID == PropID);
        if (pedComponent == null)
        {
            pedComponent = new PedPropComponent(PropID, DrawableMenuScroller.SelectedItem.ID, TextureID);
            PedCustomizer.WorkingVariation.Props.Add(pedComponent);
        }
        else
        {
            pedComponent.DrawableID = DrawableMenuScroller.SelectedItem.ID;
            pedComponent.TextureID = TextureID;
        }
        PedCustomizer.OnVariationChanged();
    }

    private void GetPossibleTextures(int drawableID)
    {
        int NumberOfTextureVariations = NativeFunction.Natives.GET_NUMBER_OF_PED_PROP_TEXTURE_VARIATIONS<int>(Ped, PropID, drawableID);
        PossibleTextures = new List<PedFashionAlias>();
        for (int TextureNumber = 0; TextureNumber < NumberOfTextureVariations; TextureNumber++)
        {
            string textureName = $"({TextureNumber})";
            string fullName = "";
            if (PedCustomizer.PedModelIsFreeMode)
            {
                FashionItemLookup fil = PedCustomizer.ClothesNames.GetItemFast(true, PropID, drawableID, TextureNumber, PedCustomizer.PedModelGender);
                if (fil != null)
                {
                    textureName = fil.GetTextureString();
                    fullName = fil.Name;
                }
            }
            PossibleTextures.Add(new PedFashionAlias(TextureNumber, textureName));
        }

    }
    private void SetTextureValue()
    {
        if (TextureMenuScroller.IsEmpty)
        {
            TextureMenuScroller.Index = UIMenuScrollerItem.EmptyIndex;
        }
        else
        {
            TextureMenuScroller.Index = 0;
        }
        PedPropComponent cpc = PedCustomizer.WorkingVariation.Props.Where(x => x.PropID == PropID).FirstOrDefault();
        if (cpc != null)
        {
            PedFashionAlias pfa = PossibleTextures.Where(x => x.ID == cpc.TextureID).FirstOrDefault();
            if (pfa != null)
            {
                TextureMenuScroller.SelectedItem = pfa;
            }
        }
    }
    private void OnTextureChanged()
    {
        if (PedCustomizer.PedCustomizerMenu.IsProgramicallySettingFieldValues)
        {
            return;
        }
        //EntryPoint.WriteToConsoleTestLong("FP OnTextureChanged");
        int TextureID = 0;
        if (TextureMenuScroller.SelectedItem != null)
        {
            TextureID = TextureMenuScroller.SelectedItem.ID;
        }
        PedPropComponent pedComponent = PedCustomizer.WorkingVariation.Props.FirstOrDefault(x => x.PropID == PropID);
        if (pedComponent == null)
        {
            pedComponent = new PedPropComponent(PropID, DrawableMenuScroller.SelectedItem.ID, TextureID);
            PedCustomizer.WorkingVariation.Props.Add(pedComponent);
        }
        else
        {
            pedComponent.DrawableID = DrawableMenuScroller.SelectedItem.ID;
            pedComponent.TextureID = TextureID;
        }
        PedCustomizer.OnVariationChanged();
    }
    private void SetToInitialValue()
    {
        PedPropComponent initialComponent = PedCustomizer.InitialVariation.Props.FirstOrDefault(x => x.PropID == PropID);
        PedPropComponent pedComponent = PedCustomizer.WorkingVariation.Props.FirstOrDefault(x => x.PropID == PropID);
        ResetFiltering();
        if (initialComponent != null)
        {
            if (pedComponent == null)
            {
                pedComponent = new PedPropComponent(PropID, initialComponent.DrawableID, initialComponent.TextureID);
                PedCustomizer.WorkingVariation.Props.Add(pedComponent);
            }
            else
            {
                pedComponent.DrawableID = initialComponent.DrawableID;
                pedComponent.TextureID = initialComponent.TextureID;
            }
            filterString = "";
            SetFiltering();
            SetCurrent(pedComponent.DrawableID, pedComponent.TextureID);
            ResetMenu.Description = "Reset the drawable back to the initial value" + $"~n~ItemID: {initialComponent.DrawableID} VariationID: {initialComponent.TextureID}";
            PedCustomizer.OnVariationChanged();
        }
        else
        {
            filterString = "";
            SetFiltering();
            SetPropRemoved();
        }
    }
    private void ResetFiltering()
    {
        //EntryPoint.WriteToConsoleTestLong($"ResetFiltering Start {PropID}");
        filterString = "";
        SetFiltering();
        //EntryPoint.WriteToConsoleTestLong($"ResetFiltering End {PropID}");
    }
    private void SetToEnteredDrawableID()
    {
        if (int.TryParse(NativeHelper.GetKeyboardInput(""), out int drawableToSet))
        {
            ResetFiltering();
            PedFashionAlias pfa = DrawableMenuScroller.Items.Where(x => x.ID == drawableToSet).FirstOrDefault();
            if (pfa != null)
            {
                DrawableMenuScroller.SelectedItem = pfa;
            }
        }
    }
    private void SetFiltering()
    {
        filterItems.RightLabel = filterString;
        GetPossibleDrawables();
        PedCustomizer.PedCustomizerMenu.IsProgramicallySettingFieldValues = true;
        DrawableMenuScroller.Items = PossibleDrawables;
        PedCustomizer.PedCustomizerMenu.IsProgramicallySettingFieldValues = false;
        if (!DrawableMenuScroller.Items.Any())
        {
            //EntryPoint.WriteToConsoleTestLong($"SetFiltering NO DRAWABLES {PropID}");
            TextureMenuScroller.Items.Clear();
        }
        else
        {
            SetDrawableValue(true);
            OnComponentChanged(DrawableMenuScroller.SelectedItem.ID);
        }
    }
    private void SetPropRemoved()
    {
        if (Ped.Exists())
        {
            NativeFunction.Natives.CLEAR_PED_PROP(Ped, PropID);
        }
        PedCustomizer.WorkingVariation.Props.RemoveAll(x => x.PropID == PropID);
        PedFashionAlias test = PossibleDrawables.FirstOrDefault(x => x.ID == 0);
        if (test != null)
        {
            DrawableMenuScroller.SelectedItem = test;
        }
        PedCustomizer.PedCustomizerMenu.IsProgramicallySettingFieldValues = true;
        GetPossibleTextures(0);
        TextureMenuScroller.Items = PossibleTextures;
        SetTextureValue();
        if (Ped.Exists())
        {
            NativeFunction.Natives.CLEAR_PED_PROP(Ped, PropID);
        }
        PedCustomizer.WorkingVariation.Props.RemoveAll(x => x.PropID == PropID);
        PedCustomizer.PedCustomizerMenu.IsProgramicallySettingFieldValues = false;
        PedCustomizer.OnVariationChanged();
    }
    public void SetCurrent(int drawableID, int textureID)
    {
        PedFashionAlias pfa = DrawableMenuScroller.Items.Where(x => x.ID == drawableID).FirstOrDefault();
        if (pfa != null)
        {
            DrawableMenuScroller.SelectedItem = pfa;
        }

        PedFashionAlias tfa = TextureMenuScroller.Items.Where(x => x.ID == textureID).FirstOrDefault();
        if (tfa != null)
        {
            TextureMenuScroller.SelectedItem = tfa;
        }

    }
}