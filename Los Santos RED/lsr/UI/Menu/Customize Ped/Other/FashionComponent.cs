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
        if (ped.Exists())
        {
            AddMenuItems(topMenu, ped, pedCustomizer);
        }
    }
    public void AddCustomizeMenu(MenuPool MenuPool, UIMenu topMenu, Ped ped, PedCustomizer pedCustomizer)
    {
        UIMenu componentMenu = MenuPool.AddSubMenu(topMenu, ComponentName);
        topMenu.MenuItems[topMenu.MenuItems.Count() - 1].Description = $"Customize the {ComponentName}";
        topMenu.MenuItems[topMenu.MenuItems.Count() - 1].RightLabel = "";
        componentMenu.SetBannerType(EntryPoint.LSRedColor);
        if (ped.Exists())
        {
            AddMenuItems(componentMenu, ped, pedCustomizer);
        }
    }
    private void AddMenuItems(UIMenu componentMenu, Ped ped, PedCustomizer pedCustomizer)
    {
        AddResetMenuItem(componentMenu, ped, pedCustomizer);
        AddDrawableItem(componentMenu, ped, pedCustomizer);
        AddTextureItem(componentMenu, ped, pedCustomizer);
        AddGoToMenuItem(componentMenu, ped, pedCustomizer);
    }
    private void AddResetMenuItem(UIMenu componentMenu, Ped ped, PedCustomizer pedCustomizer)
    {
        UIMenuItem ResetMenu = new UIMenuItem("Reset", "Reset the drawable back to the initial value");
        PedComponent initialComponentStart = pedCustomizer.InitialVariation.Components.FirstOrDefault(x => x.ComponentID == ComponentID);
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
            PedComponent initialComponent = pedCustomizer.InitialVariation.Components.FirstOrDefault(x => x.ComponentID == ComponentID);
            PedComponent pedComponent = pedCustomizer.WorkingVariation.Components.FirstOrDefault(x => x.ComponentID == ComponentID);
            if (initialComponent != null)
            {
                if (pedComponent == null)
                {
                    pedComponent = new PedComponent(ComponentID, initialComponent.DrawableID, initialComponent.TextureID, 0);
                    pedCustomizer.WorkingVariation.Components.Add(pedComponent);
                }
                else
                {
                    pedComponent.DrawableID = initialComponent.DrawableID;
                    pedComponent.TextureID = initialComponent.TextureID;
                }

                ResetMenu.Description += $"~n~DrawableID: {initialComponent.DrawableID} TextureID: {initialComponent.TextureID}";
                pedCustomizer.OnVariationChanged();
            }
        };
        componentMenu.AddItem(ResetMenu);
    }
    private void AddDrawableItem(UIMenu componentMenu, Ped ped, PedCustomizer pedCustomizer)
    {
        int NumberOfDrawables = NativeFunction.Natives.GET_NUMBER_OF_PED_DRAWABLE_VARIATIONS<int>(ped, ComponentID);
        List<PedFashionAlias> pedDrawables = new List<PedFashionAlias>();
        for (int DrawableNumber = 0; DrawableNumber < NumberOfDrawables; DrawableNumber++)
        {
            //uint hashName = NativeFunction.Natives.GET_HASH_NAME_FOR_COMPONENT<uint>(ped, ComponentID, DrawableNumber, 0);
            pedDrawables.Add(new PedFashionAlias(DrawableNumber, DrawableNumber.ToString()));
        }
        DrawableMenuScroller = new UIMenuListScrollerItem<PedFashionAlias>("Drawables", "Select drawable", pedDrawables);
        if (DrawableMenuScroller.IsEmpty)
        {
            DrawableMenuScroller.Index = UIMenuScrollerItem.EmptyIndex;
        }
        else
        {
            DrawableMenuScroller.Index = 0;
        }
        DrawableMenuScroller.IndexChanged += (Sender, oldIndex, newIndex) =>
        {
            int NewNumberOfTextureVariations = NativeFunction.Natives.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS<int>(ped, ComponentID, newIndex) - 1;
            List<PedFashionAlias> NewpedTextures = new List<PedFashionAlias>();
            for (int TextureNumber = 0; TextureNumber < NewNumberOfTextureVariations; TextureNumber++)
            {
                NewpedTextures.Add(new PedFashionAlias(TextureNumber, TextureNumber.ToString()));
            }
            TextureMenuScroller.Items = NewpedTextures;
            if (TextureMenuScroller.IsEmpty)
            {
                TextureMenuScroller.Index = UIMenuScrollerItem.EmptyIndex;
            }
            else
            {
                TextureMenuScroller.Index = 0;
            }
            PedComponent pedComponent = pedCustomizer.WorkingVariation.Components.FirstOrDefault(x => x.ComponentID == ComponentID);
            int TextureID = 0;
            if (!TextureMenuScroller.IsEmpty && TextureMenuScroller.SelectedItem != null)
            {
                TextureID = TextureMenuScroller.SelectedItem.ID;
            }
            if (pedComponent == null)
            {
                pedComponent = new PedComponent(ComponentID, DrawableMenuScroller.SelectedItem.ID, TextureID, 0);
                pedCustomizer.WorkingVariation.Components.Add(pedComponent);
            }
            else
            {
                pedComponent.DrawableID = DrawableMenuScroller.SelectedItem.ID;
                pedComponent.TextureID = TextureID;
            }
            pedCustomizer.OnVariationChanged();
        };
        componentMenu.AddItem(DrawableMenuScroller);
    }
    private void AddTextureItem(UIMenu componentMenu, Ped ped, PedCustomizer pedCustomizer)
    {
        int NumberOfTextureVariations = NativeFunction.Natives.GET_NUMBER_OF_PED_TEXTURE_VARIATIONS<int>(ped, ComponentID, 0) - 1;
        List<PedFashionAlias> pedTextures = new List<PedFashionAlias>();
        for (int TextureNumber = 0; TextureNumber < NumberOfTextureVariations; TextureNumber++)
        {
            pedTextures.Add(new PedFashionAlias(TextureNumber, TextureNumber.ToString()));
        }
        TextureMenuScroller = new UIMenuListScrollerItem<PedFashionAlias>("Textures", "Select Texture", pedTextures);
        if (TextureMenuScroller.IsEmpty)
        {
            TextureMenuScroller.Index = UIMenuScrollerItem.EmptyIndex;
        }
        else
        {
            TextureMenuScroller.Index = 0;
        }        
        TextureMenuScroller.IndexChanged += (Sender, oldIndex, newIndex) =>
        {
            int TextureID = 0;
            if (TextureMenuScroller.SelectedItem != null)
            {
                TextureID = TextureMenuScroller.SelectedItem.ID;
            }
            PedComponent pedComponent = pedCustomizer.WorkingVariation.Components.FirstOrDefault(x => x.ComponentID == ComponentID);
            if (pedComponent == null)
            {
                pedComponent = new PedComponent(ComponentID, DrawableMenuScroller.SelectedItem.ID, TextureID, 0);
                pedCustomizer.WorkingVariation.Components.Add(pedComponent);
            }
            else
            {
                pedComponent.DrawableID = DrawableMenuScroller.SelectedItem.ID;
                pedComponent.TextureID = TextureID;
            }
            pedCustomizer.OnVariationChanged();
        };
        componentMenu.AddItem(TextureMenuScroller);
    }
    private void AddGoToMenuItem(UIMenu componentMenu, Ped ped, PedCustomizer pedCustomizer)
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