using Rage.Native;
using Rage;
using RAGENativeUI.Elements;
using RAGENativeUI;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using LosSantosRED.lsr.Helper;
using System;

public class FashionProp
{
    private UIMenuListScrollerItem<PedFashionAlias> DrawableMenuScroller;
    private UIMenuListScrollerItem<PedFashionAlias> TextureMenuScroller;
    private UIMenuItem ClearProps;
    private List<PedFashionAlias> PossibleDrawables;
    private List<PedFashionAlias> PossibleTextures;
    private Ped Ped;
    private PedCustomizer PedCustomizer;

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
        AddResetMenuItem(componentMenu);
        AddDrawableItem(componentMenu);
        AddTextureItem(componentMenu);
        AddGoToMenuItem(componentMenu);
    }
    private void AddResetMenuItem(UIMenu componentMenu)
    {
        UIMenuItem ResetMenu = new UIMenuItem("Reset", "Reset the drawable back to the initial value");
        PedPropComponent initialComponentStart = PedCustomizer.InitialVariation.Props.FirstOrDefault(x => x.PropID == PropID);
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
            PedPropComponent initialComponent = PedCustomizer.InitialVariation.Props.FirstOrDefault(x => x.PropID == PropID);
            PedPropComponent pedComponent = PedCustomizer.WorkingVariation.Props.FirstOrDefault(x => x.PropID == PropID);
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

                ResetMenu.Description = "Reset the drawable back to the initial value" + $"~n~DrawableID: {initialComponent.DrawableID} TextureID: {initialComponent.TextureID}";
                PedCustomizer.OnVariationChanged();
            }
        };
        componentMenu.AddItem(ResetMenu);
    }



    private void AddDrawableItem(UIMenu componentMenu)
    {
        GetPossibleDrawables();
        DrawableMenuScroller = new UIMenuListScrollerItem<PedFashionAlias>("Drawables", "Select drawable", PossibleDrawables);
        SetDrawableValue();
        DrawableMenuScroller.IndexChanged += (Sender, oldIndex, newIndex) =>
        {
            OnComponentChanged(newIndex);
        };
        componentMenu.AddItem(DrawableMenuScroller);
    }
    private void GetPossibleDrawables()
    {
        int NumberOfDrawables = NativeFunction.Natives.GET_NUMBER_OF_PED_PROP_DRAWABLE_VARIATIONS<int>(Ped, PropID);
        PossibleDrawables = new List<PedFashionAlias>();
        for (int DrawableNumber = 0; DrawableNumber < NumberOfDrawables; DrawableNumber++)
        {
            PossibleDrawables.Add(new PedFashionAlias(DrawableNumber, DrawableNumber.ToString()));
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
        PedPropComponent cpc = PedCustomizer.WorkingVariation.Props.Where(x => x.PropID == PropID).FirstOrDefault();
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
        TextureMenuScroller = new UIMenuListScrollerItem<PedFashionAlias>("Textures", "Select Texture", PossibleTextures);
        SetTextureValue();
        TextureMenuScroller.IndexChanged += (Sender, oldIndex, newIndex) =>
        {
            OnTextureChanged();
        };
        componentMenu.AddItem(TextureMenuScroller);
    }
    private void GetPossibleTextures(int drawableID)
    {
        int NumberOfTextureVariations = NativeFunction.Natives.GET_NUMBER_OF_PED_PROP_TEXTURE_VARIATIONS<int>(Ped, PropID, drawableID) - 1;
        PossibleTextures = new List<PedFashionAlias>();
        for (int TextureNumber = 0; TextureNumber < NumberOfTextureVariations; TextureNumber++)
        {
            PossibleTextures.Add(new PedFashionAlias(TextureNumber, TextureNumber.ToString()));
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
        return PropName;
    }






    //public void CombineCustomizeMenu(MenuPool MenuPool, UIMenu topMenu, Ped ped, PedCustomizer pedCustomizer)
    //{
    //    if (ped.Exists())
    //    {
    //        AddMenuItems(topMenu, ped, pedCustomizer);
    //    }
    //}
    //public void AddCustomizeMenu(MenuPool MenuPool, UIMenu topMenu, Ped ped, PedCustomizer pedCustomizer)
    //{
    //    UIMenu componentMenu = MenuPool.AddSubMenu(topMenu, PropName);
    //    topMenu.MenuItems[topMenu.MenuItems.Count() - 1].Description = $"Customize the {PropName}";
    //    topMenu.MenuItems[topMenu.MenuItems.Count() - 1].RightLabel = "";
    //    componentMenu.SetBannerType(EntryPoint.LSRedColor);
    //    if (ped.Exists())
    //    {
    //        AddMenuItems(componentMenu, ped, pedCustomizer);
    //    }
    //}
    //private void AddMenuItems(UIMenu propMenu, Ped ped, PedCustomizer pedCustomizer)
    //{
    //    AddResetMenuItem(propMenu, ped, pedCustomizer);
    //    AddRemoveMenuItem(propMenu, ped, pedCustomizer);
    //    AddDrawableItem(propMenu, ped, pedCustomizer);
    //    AddTextureItem(propMenu, ped, pedCustomizer);
    //    AddGoToMenuItem(propMenu, ped, pedCustomizer);
    //}
    //private void AddResetMenuItem(UIMenu componentMenu, Ped ped, PedCustomizer pedCustomizer)
    //{
    //    UIMenuItem ResetMenu = new UIMenuItem("Reset", "Reset the drawable back to the initial value");
    //    PedPropComponent initialComponentStart = pedCustomizer.InitialVariation.Props.FirstOrDefault(x => x.PropID == PropID);
    //    if (initialComponentStart != null)
    //    {
    //        ResetMenu.Description += $"~n~DrawableID: {initialComponentStart.DrawableID} TextureID: {initialComponentStart.TextureID}";
    //    }
    //    else
    //    {
    //        ResetMenu.Enabled = false;
    //    }
    //    ResetMenu.Activated += (sender, e) =>
    //    {
    //        PedPropComponent initialComponent = pedCustomizer.InitialVariation.Props.FirstOrDefault(x => x.PropID == PropID);
    //        PedPropComponent pedComponent = pedCustomizer.WorkingVariation.Props.FirstOrDefault(x => x.PropID == PropID);
    //        if (initialComponent != null)
    //        {
    //            if (pedComponent == null)
    //            {
    //                pedComponent = new PedPropComponent(PropID, initialComponent.DrawableID, initialComponent.TextureID);
    //                pedCustomizer.WorkingVariation.Props.Add(pedComponent);
    //            }
    //            else
    //            {
    //                pedComponent.DrawableID = initialComponent.DrawableID;
    //                pedComponent.TextureID = initialComponent.TextureID;
    //            }

    //            ResetMenu.Description = "Reset the drawable back to the initial value" + $"~n~DrawableID: {initialComponent.DrawableID} TextureID: {initialComponent.TextureID}";
    //            pedCustomizer.OnVariationChanged();
    //        }
    //    };
    //    componentMenu.AddItem(ResetMenu);
    //}
    //private void AddGoToMenuItem(UIMenu componentMenu, Ped ped, PedCustomizer pedCustomizer)
    //{
    //    UIMenuItem goToDrawable = new UIMenuItem("Go To Drawable", "Enter a specific drawable number to auto select");
    //    goToDrawable.Activated += (sender, e) =>
    //    {
    //        if (int.TryParse(NativeHelper.GetKeyboardInput(""), out int moneyToSet))
    //        {
    //            PedFashionAlias pfa = DrawableMenuScroller.Items.Where(x => x.ID == moneyToSet).FirstOrDefault();
    //            if (pfa != null)
    //            {
    //                DrawableMenuScroller.SelectedItem = pfa;
    //            }
    //        }
    //    };
    //    componentMenu.AddItem(goToDrawable);
    //}
    //private void AddRemoveMenuItem(UIMenu componentMenu, Ped ped, PedCustomizer pedCustomizer)
    //{
    //    UIMenuItem removeProprMenu = new UIMenuItem("Remove Prop", "Select to remove the current prop");
    //    removeProprMenu.Activated += (sender, e) =>
    //    {
    //        if(!DrawableMenuScroller.IsEmpty)
    //        {
    //            DrawableMenuScroller.Index = 0;
    //        }
    //        else
    //        {
    //            DrawableMenuScroller.Index = UIMenuScrollerItem.EmptyIndex;
    //        }
    //        if (!TextureMenuScroller.IsEmpty)
    //        {
    //            TextureMenuScroller.Index = 0;
    //        }
    //        else
    //        {
    //            TextureMenuScroller.Index = UIMenuScrollerItem.EmptyIndex;
    //        }
    //        pedCustomizer.WorkingVariation.Props.RemoveAll(x => x.PropID == PropID);
    //        pedCustomizer.OnVariationChanged();
    //    };
    //    componentMenu.AddItem(removeProprMenu);
    //}
    //private void AddDrawableItem(UIMenu componentMenu, Ped ped, PedCustomizer pedCustomizer)
    //{
    //    int NumberOfDrawables = NativeFunction.Natives.GET_NUMBER_OF_PED_PROP_DRAWABLE_VARIATIONS<int>(ped, PropID);
    //    List<PedFashionAlias> pedDrawables = new List<PedFashionAlias>();
    //    for (int DrawableNumber = 0; DrawableNumber < NumberOfDrawables; DrawableNumber++)
    //    {
    //        pedDrawables.Add(new PedFashionAlias(DrawableNumber, DrawableNumber.ToString()));
    //    }
    //    DrawableMenuScroller = new UIMenuListScrollerItem<PedFashionAlias>("Drawables", "Select drawable", pedDrawables);
    //    if (DrawableMenuScroller.IsEmpty)
    //    {
    //        DrawableMenuScroller.Index = UIMenuScrollerItem.EmptyIndex;
    //    }
    //    else
    //    {
    //        DrawableMenuScroller.Index = 0;
    //    }
    //    PedPropComponent cpc = pedCustomizer.WorkingVariation.Props.Where(x => x.PropID == PropID).FirstOrDefault();
    //    if (cpc != null)
    //    {
    //        PedFashionAlias pfa = pedDrawables.Where(x => x.ID == cpc.DrawableID).FirstOrDefault();
    //        if (pfa != null)
    //        {
    //            DrawableMenuScroller.SelectedItem = pfa;
    //        }
    //    }



    //    DrawableMenuScroller.IndexChanged += (Sender, oldIndex, newIndex) =>
    //    {
    //        OnComponentChanged(pedCustomizer, ped);
    //    };
    //    DrawableMenuScroller.Activated += (sender,e) =>
    //    {
    //        OnComponentChanged(pedCustomizer, ped);
    //    };
    //    componentMenu.AddItem(DrawableMenuScroller);
    //}
    //private void OnComponentChanged(PedCustomizer pedCustomizer, Ped ped)
    //{
    //    int DrawableID = 0;
    //    if (DrawableMenuScroller.SelectedItem != null)
    //    {
    //        DrawableID = DrawableMenuScroller.SelectedItem.ID;
    //    }
    //    int NewNumberOfTextureVariations = NativeFunction.Natives.GET_NUMBER_OF_PED_PROP_TEXTURE_VARIATIONS<int>(ped, PropID, DrawableID) - 1;
    //    List<PedFashionAlias> NewpedTextures = new List<PedFashionAlias>();
    //    for (int TextureNumber = 0; TextureNumber < NewNumberOfTextureVariations; TextureNumber++)
    //    {
    //        NewpedTextures.Add(new PedFashionAlias(TextureNumber, TextureNumber.ToString()));
    //    }
    //    TextureMenuScroller.Items = NewpedTextures;
    //    if (TextureMenuScroller.IsEmpty)
    //    {
    //        TextureMenuScroller.Index = UIMenuScrollerItem.EmptyIndex;
    //    }
    //    else
    //    {
    //        TextureMenuScroller.Index = 0;
    //    }
    //    PedPropComponent pedComponent = pedCustomizer.WorkingVariation.Props.FirstOrDefault(x => x.PropID == PropID);
    //    int TextureID = 0;
    //    if (!TextureMenuScroller.IsEmpty && TextureMenuScroller.SelectedItem != null)
    //    {
    //        TextureID = TextureMenuScroller.SelectedItem.ID;
    //    }
    //    if (pedComponent == null)
    //    {
    //        pedComponent = new PedPropComponent(PropID, DrawableMenuScroller.SelectedItem.ID, TextureID);
    //        pedCustomizer.WorkingVariation.Props.Add(pedComponent);
    //    }
    //    else
    //    {
    //        pedComponent.DrawableID = DrawableMenuScroller.SelectedItem.ID;
    //        pedComponent.TextureID = TextureID;
    //    }
    //    pedCustomizer.OnVariationChanged();
    //}
    //private void AddTextureItem(UIMenu componentMenu, Ped ped, PedCustomizer pedCustomizer)
    //{
    //    int NumberOfTextureVariations = NativeFunction.Natives.GET_NUMBER_OF_PED_PROP_TEXTURE_VARIATIONS<int>(ped, PropID, 0) - 1;
    //    List<PedFashionAlias> pedTextures = new List<PedFashionAlias>();
    //    for (int TextureNumber = 0; TextureNumber < NumberOfTextureVariations; TextureNumber++)
    //    {
    //        pedTextures.Add(new PedFashionAlias(TextureNumber, TextureNumber.ToString()));
    //    }
    //    TextureMenuScroller = new UIMenuListScrollerItem<PedFashionAlias>("Textures", "Select Texture", pedTextures);
    //    if (TextureMenuScroller.IsEmpty)
    //    {
    //        TextureMenuScroller.Index = UIMenuScrollerItem.EmptyIndex;
    //    }
    //    else
    //    {
    //        TextureMenuScroller.Index = 0;
    //    }


    //    PedPropComponent cpc = pedCustomizer.WorkingVariation.Props.Where(x => x.PropID == PropID).FirstOrDefault();
    //    if (cpc != null)
    //    {
    //        PedFashionAlias pfa = pedTextures.Where(x => x.ID == cpc.DrawableID).FirstOrDefault();
    //        if (pfa != null)
    //        {
    //            TextureMenuScroller.SelectedItem = pfa;
    //        }
    //    }


    //    TextureMenuScroller.IndexChanged += (Sender, oldIndex, newIndex) =>
    //    {
    //        OnTextureChanged(pedCustomizer);
    //    };
    //    TextureMenuScroller.Activated += (sender, e) =>
    //    {
    //        OnTextureChanged(pedCustomizer);
    //    };

    //    componentMenu.AddItem(TextureMenuScroller);
    //}
    //private void OnTextureChanged(PedCustomizer pedCustomizer)
    //{
    //    int TextureID = 0;
    //    if (TextureMenuScroller.SelectedItem != null)
    //    {
    //        TextureID = TextureMenuScroller.SelectedItem.ID;
    //    }
    //    PedPropComponent pedComponent = pedCustomizer.WorkingVariation.Props.FirstOrDefault(x => x.PropID == PropID);
    //    if (pedComponent == null)
    //    {
    //        pedComponent = new PedPropComponent(PropID, DrawableMenuScroller.SelectedItem.ID, TextureID);
    //        pedCustomizer.WorkingVariation.Props.Add(pedComponent);
    //    }
    //    else
    //    {
    //        pedComponent.DrawableID = DrawableMenuScroller.SelectedItem.ID;
    //        pedComponent.TextureID = TextureID;
    //    }
    //    pedCustomizer.OnVariationChanged();
    //}
    //public override string ToString()
    //{
    //    return PropName;
    //}
}