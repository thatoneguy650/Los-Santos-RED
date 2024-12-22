using LosSantosRED.lsr.Helper;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Media.Media3D;

public class FashionComponent
{
    private UIMenuListScrollerItem<PedFashionAlias> DrawableMenuScroller;
    private UIMenuListScrollerItem<PedFashionAlias> TextureMenuScroller;
    private List<PedFashionAlias> PossibleDrawables;
    private List<PedFashionAlias> PossibleTextures;
    private Ped Ped;
    private PedCustomizer PedCustomizer;
    private string filterString = "";
    private UIMenuItem filterItems;
    private UIMenuItem goToDrawable;
    private UIMenuItem ResetMenu;

    public FashionComponent()
    {
    }
    public FashionComponent(int componentID, string componentName)
    {
        ComponentID = componentID;
        ComponentName = componentName;
    }
    public int ComponentID { get; set; }
    public string ComponentName { get; set; }
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
        UIMenu componentMenu = MenuPool.AddSubMenu(topMenu, ComponentName);
        topMenu.MenuItems[topMenu.MenuItems.Count() - 1].Description = $"Customize the {ComponentName} - {ComponentID}";
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
        AddDrawableItem(componentMenu);
        AddTextureItem(componentMenu);
        AddGoToMenuItem(componentMenu);
        AddSearchMenuItem(componentMenu);
    }
    private void AddResetMenuItem(UIMenu componentMenu)
    {
        ResetMenu = new UIMenuItem("Reset", "Reset the item back to the initial value");
        PedComponent initialComponentStart = PedCustomizer.InitialVariation.Components.FirstOrDefault(x => x.ComponentID == ComponentID);
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
        PedComponent pedComponent = PedCustomizer.WorkingVariation.Components.FirstOrDefault(x => x.ComponentID == ComponentID);
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
        goToDrawable = new UIMenuItem("Go To Drawable", "Enter a specific item number to auto select");
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
        int NumberOfDrawables = NativeFunction.Natives.GET_NUMBER_OF_PED_DRAWABLE_VARIATIONS<int>(Ped, ComponentID);
        EntryPoint.WriteToConsole($"ComponentID: {ComponentID}   NumberOfDrawables: {NumberOfDrawables}");
        PossibleDrawables = new List<PedFashionAlias>();
        bool isValid = true;
        for (int DrawableNumber = 0; DrawableNumber < NumberOfDrawables; DrawableNumber++)
        {
            string drawableName = $"({DrawableNumber})";
            string fullName = "";
            if (PedCustomizer.PedModelIsFreeMode)
            {
                isValid = false;
                HashSet<FashionItemLookup> possibleDrawables = PedCustomizer.ClothesNames.GetItemsFast(false, ComponentID, DrawableNumber, PedCustomizer.PedModelGender);
                if (string.IsNullOrEmpty(filterString) || filterString == "")
                {
                    isValid = true;
                }
                else
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
        //EntryPoint.WriteToConsoleTestLong($"SetDrawableValue Start {ComponentID} canGo {canGo}");
        if (DrawableMenuScroller.IsEmpty)
        {
            DrawableMenuScroller.Index = UIMenuScrollerItem.EmptyIndex;
        }
        else
        {
            DrawableMenuScroller.Index = 0;
        }
        PedComponent cpc = PedCustomizer.WorkingVariation.Components.Where(x => x.ComponentID == ComponentID).FirstOrDefault();
        if (cpc != null)
        {
            PedFashionAlias pfa = PossibleDrawables.Where(x => x.ID == cpc.DrawableID).FirstOrDefault();
            if (pfa != null)
            {
                DrawableMenuScroller.SelectedItem = pfa;
                //EntryPoint.WriteToConsoleTestLong($"SetDrawableValue SET WORKING VARIATION {ComponentID} {pfa.ID} {pfa.Name} canGo {canGo}");
            }
        }
        else if (canGo)
        {
            PedFashionAlias pfa = PossibleDrawables.OrderBy(x => x.ID).FirstOrDefault();
            if (pfa != null)
            {
                DrawableMenuScroller.SelectedItem = pfa;
                //EntryPoint.WriteToConsoleTestLong($"SetDrawableValue SET ANY VARIATION {ComponentID} {pfa.ID} {pfa.Name} canGo {canGo}");
            }
        }
        //EntryPoint.WriteToConsoleTestLong($"SetDrawableValue End {ComponentID} canGo {canGo}");
    }
    private void OnComponentChanged(int newDrawableID)
    {
        if (PedCustomizer.PedCustomizerMenu.IsProgramicallySettingFieldValues)
        {
            //EntryPoint.WriteToConsoleTestLong($"OnComponentChanged RETURN IsProgramicallySettingFieldValues {ComponentID}");
            return;
        }


        if(PedCustomizer.IsDrawableBlacklisted(ComponentID,newDrawableID,PedCustomizer.PedModelIsFreeMode && !PedCustomizer.PedModelIsFreeModeFemale))
        {
            Game.DisplaySubtitle($"{ComponentID}-{newDrawableID} cannot be set");
            return;
        }


        //EntryPoint.WriteToConsoleTestLong($"OnComponentChanged Start {ComponentID} newDrawableID {newDrawableID}");
        GetPossibleTextures(newDrawableID);
        TextureMenuScroller.Items = PossibleTextures;
        SetTextureValue();       
        int TextureID = !TextureMenuScroller.IsEmpty && TextureMenuScroller.SelectedItem != null ? TextureMenuScroller.SelectedItem.ID : 0;
        PedComponent pedComponent = PedCustomizer.WorkingVariation.Components.FirstOrDefault(x => x.ComponentID == ComponentID);
        if (pedComponent == null)
        {
            pedComponent = new PedComponent(ComponentID, DrawableMenuScroller.SelectedItem.ID, TextureID, 0);
            PedCustomizer.WorkingVariation.Components.Add(pedComponent);
        }
        else
        {
            pedComponent.DrawableID = DrawableMenuScroller.SelectedItem.ID;
            pedComponent.TextureID = TextureID;
        }
        PedCustomizer.OnVariationChanged();
        //EntryPoint.WriteToConsoleTestLong($"OnComponentChanged End {ComponentID} newDrawableID {newDrawableID}");
        //EntryPoint.WriteToConsoleTestLong($"OnComponentChanged End {ComponentID}");
    }
    private void GetPossibleTextures(int drawableID)
    {
        //EntryPoint.WriteToConsoleTestLong($"GetPossibleTextures Start {ComponentID}");
        int NumberOfTextureVariations = NativeFunction.Natives.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS<int>(Ped, ComponentID, drawableID);
        PossibleTextures = new List<PedFashionAlias>();
        for (int TextureNumber = 0; TextureNumber < NumberOfTextureVariations; TextureNumber++)
        {
            string textureName = $"({TextureNumber})";
            string fullName = "";
            if (PedCustomizer.PedModelIsFreeMode)
            {
                FashionItemLookup fil = PedCustomizer.ClothesNames.GetItemFast(false, ComponentID, drawableID, TextureNumber, PedCustomizer.PedModelGender);
                if (fil != null)
                {
                    textureName = fil.GetTextureString();
                    fullName = fil.Name;
                }
            }
            PossibleTextures.Add(new PedFashionAlias(TextureNumber, textureName));
            //EntryPoint.WriteToConsoleTestLong($"GetPossibleTextures ADDED:       {TextureNumber} {textureName}");
        }
        //EntryPoint.WriteToConsoleTestLong($"GetPossibleTextures End {ComponentID}");
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
        PedComponent cpc = PedCustomizer.WorkingVariation.Components.Where(x => x.ComponentID == ComponentID).FirstOrDefault();
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
            //EntryPoint.WriteToConsoleTestLong($"OnTextureChanged RETURN IsProgramicallySettingFieldValues {ComponentID}");
            return;
        }
        //EntryPoint.WriteToConsoleTestLong($"OnTextureChanged Start {ComponentID}");

        int TextureID = 0;
        if (TextureMenuScroller.SelectedItem != null)
        {
            TextureID = TextureMenuScroller.SelectedItem.ID;
        }
        PedComponent pedComponent = PedCustomizer.WorkingVariation.Components.FirstOrDefault(x => x.ComponentID == ComponentID);
        if (pedComponent == null)
        {
            pedComponent = new PedComponent(ComponentID, DrawableMenuScroller.SelectedItem.ID, TextureID, 0);
            PedCustomizer.WorkingVariation.Components.Add(pedComponent);
        }
        else
        {
            pedComponent.DrawableID = DrawableMenuScroller.SelectedItem.ID;
            pedComponent.TextureID = TextureID;
        }
        PedCustomizer.OnVariationChanged();
        //EntryPoint.WriteToConsoleTestLong($"OnTextureChanged End {ComponentID}");
    }
    private void SetToInitialValue()
    {
        //EntryPoint.WriteToConsoleTestLong($"SetToInitialValue Start {ComponentID}");
        PedComponent initialComponent = PedCustomizer.InitialVariation.Components.FirstOrDefault(x => x.ComponentID == ComponentID);
        PedComponent pedComponent = PedCustomizer.WorkingVariation.Components.FirstOrDefault(x => x.ComponentID == ComponentID);
        ResetFiltering();
        if (initialComponent != null)
        {
            if (pedComponent == null)
            {
                pedComponent = new PedComponent(ComponentID, initialComponent.DrawableID, initialComponent.TextureID, 0);
                PedCustomizer.WorkingVariation.Components.Add(pedComponent);
            }
            else
            {
                pedComponent.DrawableID = initialComponent.DrawableID;
                pedComponent.TextureID = initialComponent.TextureID;
            }
            SetCurrent(pedComponent.DrawableID, pedComponent.TextureID);
            ResetMenu.Description = $"Reset the drawable back to the initial value~n~({initialComponent.DrawableID},{initialComponent.TextureID})";
            PedCustomizer.OnVariationChanged();
        }
        //EntryPoint.WriteToConsoleTestLong($"SetToInitialValue End {ComponentID}");
    }
    private void ResetFiltering()
    {
        //EntryPoint.WriteToConsoleTestLong($"ResetFiltering Start {ComponentID}");
        filterString = "";
        SetFiltering();
        //EntryPoint.WriteToConsoleTestLong($"ResetFiltering End {ComponentID}");
    }
    private void SetToEnteredDrawableID()
    {
        //EntryPoint.WriteToConsoleTestLong($"SetToEnteredDrawableID Start {ComponentID}");
        if (int.TryParse(NativeHelper.GetKeyboardInput(""), out int drawableToSet))
        {
            ResetFiltering();
            PedFashionAlias pfa = DrawableMenuScroller.Items.Where(x => x.ID == drawableToSet).FirstOrDefault();
            if (pfa != null)
            {
                DrawableMenuScroller.SelectedItem = pfa;
            }
        }
        //EntryPoint.WriteToConsoleTestLong($"SetToEnteredDrawableID End {ComponentID}");
    }
    private void SetFiltering()
    {
        //EntryPoint.WriteToConsoleTestLong($"SetFiltering Start {ComponentID}");
        filterItems.RightLabel = filterString;
        GetPossibleDrawables();
        PedCustomizer.PedCustomizerMenu.IsProgramicallySettingFieldValues = true;
        DrawableMenuScroller.Items = PossibleDrawables;
        PedCustomizer.PedCustomizerMenu.IsProgramicallySettingFieldValues = false;
        if (!DrawableMenuScroller.Items.Any())
        {
            //EntryPoint.WriteToConsoleTestLong($"SetFiltering NO DRAWABLES {ComponentID}");
            TextureMenuScroller.Items.Clear();
        }
        else
        {
            SetDrawableValue(true);
            OnComponentChanged(DrawableMenuScroller.SelectedItem.ID);
        }
        //EntryPoint.WriteToConsoleTestLong($"SetFiltering End {ComponentID}");
    }
    public void SetCurrent(int drawableID, int textureID)
    {
        //EntryPoint.WriteToConsoleTestLong($"SetCurrent Start {ComponentID}");

        if(DrawableMenuScroller == null || DrawableMenuScroller.Items == null)
        {
            return;
        }



        PedFashionAlias pfa = DrawableMenuScroller.Items.Where(x => x.ID == drawableID).FirstOrDefault();
        if (pfa != null)
        {
            //EntryPoint.WriteToConsoleTestLong($"SetCurrent Drawable Found {pfa.ID} {pfa.Name}");
            DrawableMenuScroller.SelectedItem = pfa;
        }
        if (TextureMenuScroller == null || TextureMenuScroller.Items == null)
        {
            return;
        }
        PedFashionAlias tfa = TextureMenuScroller.Items.Where(x => x.ID == textureID).FirstOrDefault();
        if (tfa != null)
        {
            //EntryPoint.WriteToConsoleTestLong($"SetCurrent Texture Found {tfa.ID} {tfa.Name}");
            TextureMenuScroller.SelectedItem = tfa;
        }
        //EntryPoint.WriteToConsoleTestLong($"SetCurrent End {ComponentID}");
    }

}