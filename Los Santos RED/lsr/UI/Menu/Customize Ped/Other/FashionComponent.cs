using LosSantosRED.lsr.Helper;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
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
        topMenu.MenuItems[topMenu.MenuItems.Count() - 1].Description = $"Customize the {ComponentName}";
        topMenu.MenuItems[topMenu.MenuItems.Count() - 1].RightLabel = "";
        componentMenu.SetBannerType(EntryPoint.LSRedColor);
        if (Ped.Exists())
        {
            AddMenuItems(componentMenu);
        }
    }
    private void AddMenuItems(UIMenu componentMenu)
    {
        AddResetMenuItem(componentMenu);
        AddDrawableItem(componentMenu);
        AddTextureItem(componentMenu);
        AddGoToMenuItem(componentMenu);
        AddSearchMenuItem(componentMenu);
    }
    private void AddResetMenuItem(UIMenu componentMenu)
    {
        UIMenuItem ResetMenu = new UIMenuItem("Reset", "Reset the drawable back to the initial value");
        PedComponent initialComponentStart = PedCustomizer.InitialVariation.Components.FirstOrDefault(x => x.ComponentID == ComponentID);
        if (initialComponentStart != null)
        {
            ResetMenu.Description = $"Reset the drawable back to the initial value ~n~DrawableID: {initialComponentStart.DrawableID} TextureID: {initialComponentStart.TextureID}";
        }
        else
        {
            ResetMenu.Enabled = false;
        }
        ResetMenu.Activated += (sender, e) =>
        {
            PedComponent initialComponent = PedCustomizer.InitialVariation.Components.FirstOrDefault(x => x.ComponentID == ComponentID);
            PedComponent pedComponent = PedCustomizer.WorkingVariation.Components.FirstOrDefault(x => x.ComponentID == ComponentID);
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
                ResetMenu.Description = "Reset the drawable back to the initial value" + $"~n~ItemID: {initialComponent.DrawableID} VariationID: {initialComponent.TextureID}";
                PedCustomizer.OnVariationChanged();
            }
        };
        componentMenu.AddItem(ResetMenu);
    }



    private void AddDrawableItem(UIMenu componentMenu)
    {
        GetPossibleDrawables();
        DrawableMenuScroller = new UIMenuListScrollerItem<PedFashionAlias>("Item", "Select item", PossibleDrawables);
        SetDrawableValue();
        DrawableMenuScroller.IndexChanged += (Sender, oldIndex, newIndex) =>
        {
            OnComponentChanged(newIndex);
        };
        componentMenu.AddItem(DrawableMenuScroller);
    }
    private void GetPossibleDrawables()
    {
        int NumberOfDrawables = NativeFunction.Natives.GET_NUMBER_OF_PED_DRAWABLE_VARIATIONS<int>(Ped, ComponentID);
        PossibleDrawables = new List<PedFashionAlias>();
        for (int DrawableNumber = 0; DrawableNumber < NumberOfDrawables; DrawableNumber++)
        {
            string drawableName = $"({DrawableNumber})";
            string fullName = "";
            if (PedCustomizer.PedModelIsFreeMode)
            {
                FashionItemLookup fil = PedCustomizer.ClothesNames.GetItemFast(false, ComponentID, DrawableNumber, 0, PedCustomizer.PedModelGender);
                if(fil != null)
                {
                    drawableName = fil.GetDrawableString();
                    fullName = fil.Name;
                }
            }
            if (filterString == "" || drawableName.ToLower().Contains(filterString.ToLower()) || fullName.ToLower().Contains(filterString.ToLower()))
            {
                PossibleDrawables.Add(new PedFashionAlias(DrawableNumber, drawableName));
            }
        }
    }
    private void SetDrawableValue()
    {
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
            }
        }
    }
    private void OnComponentChanged(int newDrawableID)
    {
        if (PedCustomizer.PedCustomizerMenu.IsProgramicallySettingFieldValues)
        {
            return;
        }
        EntryPoint.WriteToConsole("FC OnComponentChanged");
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
    private void GetPossibleTextures(int drawableID)
    {
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

            if (filterString == "" || textureName.ToLower().Contains(filterString.ToLower()) || fullName.ToLower().Contains(filterString.ToLower()))
            {
                PossibleTextures.Add(new PedFashionAlias(TextureNumber, textureName));
            }



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
            return;
        }
        EntryPoint.WriteToConsole("FC OnTextureChanged");

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
    }


    private void AddGoToMenuItem(UIMenu componentMenu)
    {
        UIMenuItem goToDrawable = new UIMenuItem("Go To Drawable", "Enter a specific drawable number to auto select");
        goToDrawable.Activated += (sender, e) =>
        {
            if (int.TryParse(NativeHelper.GetKeyboardInput(""), out int moneyToSet))
            {
                PedFashionAlias pfa = DrawableMenuScroller.Items.Where(x => x.ID == moneyToSet).FirstOrDefault();
                if (pfa != null)
                {
                    DrawableMenuScroller.SelectedItem = pfa;
                }
            }
        };
        componentMenu.AddItem(goToDrawable);
    }



    private void AddSearchMenuItem(UIMenu componentMenu)
    {
        UIMenuItem filterItems = new UIMenuItem("Search", "Search for specific items matching the search terms");
        filterItems.Activated += (sender, e) =>
        {
            

            filterString = NativeHelper.GetKeyboardInput("");
            filterItems.RightLabel = filterString;



            GetPossibleDrawables();
            DrawableMenuScroller.Items = PossibleDrawables;
            SetDrawableValue();


            if (PossibleDrawables.Any())
            {
                //int drawableid = PossibleDrawables.Min(x => x.ID);
                //EntryPoint.WriteToConsole($"Search MENU OnComponentChanged({drawableid})");
                //GetPossibleTextures(drawableid);
                //TextureMenuScroller.Items = PossibleTextures;
                //SetTextureValue();
            }

        };
        componentMenu.AddItem(filterItems);
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
    public override string ToString()
    {
        return ComponentName;
    }
    //    for (int DrawableNumber = 0; DrawableNumber < NumberOfDrawables; DrawableNumber++)
    //    {
    //        int NumberOfTextureVariations = NativeFunction.Natives.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS<int>(ModelPed, ComponentNumber, DrawableNumber) - 1;
    //        UIMenuNumericScrollerItem<int> Test = new UIMenuNumericScrollerItem<int>($"Drawable: {DrawableNumber}", "Arrow to change texture, select to reset texture", 0, NumberOfTextureVariations, 1);
    //        Test.Formatter = v => v == 0 ? "Default" : "Texture ID " + v.ToString() + $" of {NumberOfTextureVariations}";
    //        ComponentSubMenu.AddItem(Test);
    //    }


    //componentMenu

    //each sub menu

    //Reset - goes back to whatever the componenet was before any changes here
    //drawableID with scroller , shoudl this be name if I have it? where would price be displayed?
    //a textureID with scroller //on scroll change 

    //goto specific drawable
    //Save as Favorite (maybe)


    //accept drawable, (or just not reset?

}