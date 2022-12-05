using Rage.Native;
using Rage;
using RAGENativeUI.Elements;
using RAGENativeUI;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using LosSantosRED.lsr.Helper;

public class FashionProp
{
    private UIMenuListScrollerItem<PedFashionAlias> DrawableMenuScroller;
    private UIMenuListScrollerItem<PedFashionAlias> TextureMenuScroller;
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
        if (ped.Exists())
        {
            AddMenuItems(topMenu, ped, pedCustomizer);
        }
    }
    public void AddCustomizeMenu(MenuPool MenuPool, UIMenu topMenu, Ped ped, PedCustomizer pedCustomizer)
    {
        UIMenu componentMenu = MenuPool.AddSubMenu(topMenu, PropName);
        topMenu.MenuItems[topMenu.MenuItems.Count() - 1].Description = $"Customize the {PropName}";
        topMenu.MenuItems[topMenu.MenuItems.Count() - 1].RightLabel = "";
        componentMenu.SetBannerType(EntryPoint.LSRedColor);
        if (ped.Exists())
        {
            AddMenuItems(componentMenu, ped, pedCustomizer);
        }
    }
    private void AddMenuItems(UIMenu propMenu, Ped ped, PedCustomizer pedCustomizer)
    {
        AddResetMenuItem(propMenu, ped, pedCustomizer);
        AddRemoveMenuItem(propMenu, ped, pedCustomizer);
        AddDrawableItem(propMenu, ped, pedCustomizer);
        AddTextureItem(propMenu, ped, pedCustomizer);
        AddGoToMenuItem(propMenu, ped, pedCustomizer);
    }
    private void AddResetMenuItem(UIMenu componentMenu, Ped ped, PedCustomizer pedCustomizer)
    {
        UIMenuItem ResetMenu = new UIMenuItem("Reset", "Reset the drawable back to the initial value");
        PedPropComponent initialComponentStart = pedCustomizer.InitialVariation.Props.FirstOrDefault(x => x.PropID == PropID);
        if (initialComponentStart != null)
        {
            ResetMenu.Description += $"~n~DrawableID: {initialComponentStart.DrawableID} TextureID: {initialComponentStart.TextureID}";
        }
        else
        {
            ResetMenu.Enabled = false;
        }
        ResetMenu.Activated += (sender, e) =>
        {
            PedPropComponent initialComponent = pedCustomizer.InitialVariation.Props.FirstOrDefault(x => x.PropID == PropID);
            PedPropComponent pedComponent = pedCustomizer.WorkingVariation.Props.FirstOrDefault(x => x.PropID == PropID);
            if (initialComponent != null)
            {
                if (pedComponent == null)
                {
                    pedComponent = new PedPropComponent(PropID, initialComponent.DrawableID, initialComponent.TextureID);
                    pedCustomizer.WorkingVariation.Props.Add(pedComponent);
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
    private void AddRemoveMenuItem(UIMenu componentMenu, Ped ped, PedCustomizer pedCustomizer)
    {
        UIMenuItem removeProprMenu = new UIMenuItem("Remove Prop", "Select to remove the current prop");
        removeProprMenu.Activated += (sender, e) =>
        {
            pedCustomizer.WorkingVariation.Props.RemoveAll(x => x.PropID == PropID);
            pedCustomizer.OnVariationChanged();
        };
        componentMenu.AddItem(removeProprMenu);
    }
    private void AddDrawableItem(UIMenu componentMenu, Ped ped, PedCustomizer pedCustomizer)
    {
        int NumberOfDrawables = NativeFunction.Natives.GET_NUMBER_OF_PED_PROP_DRAWABLE_VARIATIONS<int>(ped, PropID);
        List<PedFashionAlias> pedDrawables = new List<PedFashionAlias>();
        for (int DrawableNumber = 0; DrawableNumber < NumberOfDrawables; DrawableNumber++)
        {
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
            OnComponentChanged(pedCustomizer, ped);
        };
        DrawableMenuScroller.Activated += (sender,e) =>
        {
            OnComponentChanged(pedCustomizer, ped);
        };
        componentMenu.AddItem(DrawableMenuScroller);
    }
    private void OnComponentChanged(PedCustomizer pedCustomizer, Ped ped)
    {
        int DrawableID = 0;
        if (DrawableMenuScroller.SelectedItem != null)
        {
            DrawableID = DrawableMenuScroller.SelectedItem.ID;
        }
        int NewNumberOfTextureVariations = NativeFunction.Natives.GET_NUMBER_OF_PED_PROP_TEXTURE_VARIATIONS<int>(ped, PropID, DrawableID) - 1;
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
        PedPropComponent pedComponent = pedCustomizer.WorkingVariation.Props.FirstOrDefault(x => x.PropID == PropID);
        int TextureID = 0;
        if (!TextureMenuScroller.IsEmpty && TextureMenuScroller.SelectedItem != null)
        {
            TextureID = TextureMenuScroller.SelectedItem.ID;
        }
        if (pedComponent == null)
        {
            pedComponent = new PedPropComponent(PropID, DrawableMenuScroller.SelectedItem.ID, TextureID);
            pedCustomizer.WorkingVariation.Props.Add(pedComponent);
        }
        else
        {
            pedComponent.DrawableID = DrawableMenuScroller.SelectedItem.ID;
            pedComponent.TextureID = TextureID;
        }
        pedCustomizer.OnVariationChanged();
    }
    private void AddTextureItem(UIMenu componentMenu, Ped ped, PedCustomizer pedCustomizer)
    {
        int NumberOfTextureVariations = NativeFunction.Natives.GET_NUMBER_OF_PED_PROP_TEXTURE_VARIATIONS<int>(ped, PropID, 0) - 1;
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
            OnTextureChanged(pedCustomizer);
        };
        TextureMenuScroller.Activated += (sender, e) =>
        {
            OnTextureChanged(pedCustomizer);
        };

        componentMenu.AddItem(TextureMenuScroller);
    }

    private void OnTextureChanged(PedCustomizer pedCustomizer)
    {
        int TextureID = 0;
        if (TextureMenuScroller.SelectedItem != null)
        {
            TextureID = TextureMenuScroller.SelectedItem.ID;
        }
        PedPropComponent pedComponent = pedCustomizer.WorkingVariation.Props.FirstOrDefault(x => x.PropID == PropID);
        if (pedComponent == null)
        {
            pedComponent = new PedPropComponent(PropID, DrawableMenuScroller.SelectedItem.ID, TextureID);
            pedCustomizer.WorkingVariation.Props.Add(pedComponent);
        }
        else
        {
            pedComponent.DrawableID = DrawableMenuScroller.SelectedItem.ID;
            pedComponent.TextureID = TextureID;
        }
        pedCustomizer.OnVariationChanged();
    }

    public override string ToString()
    {
        return PropName;
    }
}