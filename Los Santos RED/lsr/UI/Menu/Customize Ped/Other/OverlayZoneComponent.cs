using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class OverlayZoneComponent
{
    private Ped Ped;
    private PedCustomizer PedCustomizer;
    private UIMenuItem ResetMenu;
    private List<TattooOverlay> PossibleTattoos;
    private UIMenuListScrollerItem<TattooOverlay> TattooOverlayMenuList;

    public int ID { get; set; }
    public string ZoneName { get; set; }
    public string ZoneDisplay { get; set; }
    public OverlayZoneComponent()
    {
    }
    public OverlayZoneComponent(int id, string zoneName, string zoneDisplay)
    {
        ID = id;
        ZoneName = zoneName;
        ZoneDisplay = zoneDisplay;
    }
    public void AddCustomizeMenu(MenuPool MenuPool, UIMenu topMenu, Ped ped, PedCustomizer pedCustomizer)
    {
        Ped = ped;
        PedCustomizer = pedCustomizer;
        UIMenu componentMenu = MenuPool.AddSubMenu(topMenu, ZoneDisplay);
        topMenu.MenuItems[topMenu.MenuItems.Count() - 1].Description = $"Customize the {ZoneDisplay}";
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
    }
    private void AddResetMenuItem(UIMenu componentMenu)
    {
        ResetMenu = new UIMenuItem("Reset", "Reset the overlays for the given zone");
        ResetMenu.Activated += (sender, e) =>
        {
            PedCustomizer.WorkingVariation.AppliedOverlays?.RemoveAll(x => x.ZoneName == ZoneName);
            PedCustomizer.OnVariationChanged();
        };
        componentMenu.AddItem(ResetMenu);
    }
    private void AddDrawableItem(UIMenu componentMenu)
    {
        PossibleTattoos = PedCustomizer.TattooNames.GetOverlaysByZone(ZoneName);
        TattooOverlayMenuList = new UIMenuListScrollerItem<TattooOverlay>("Item", "Select item", PossibleTattoos);
        TattooOverlayMenuList.IndexChanged += (Sender, oldIndex, newIndex) =>
        {
           
        };
        TattooOverlayMenuList.Activated += (sender, selectedItem) =>
        {
            OnOverlayChanged(TattooOverlayMenuList.SelectedItem);
        };
        componentMenu.AddItem(TattooOverlayMenuList);
    }

    private void OnOverlayChanged(TattooOverlay selectedItem)
    {
        if(selectedItem == null)
        {
            return;
        }
        if(PedCustomizer.WorkingVariation.AppliedOverlays == null)
        {
            PedCustomizer.WorkingVariation.AppliedOverlays = new List<AppliedOverlay>();
        }
        if(PedCustomizer.WorkingVariation.AppliedOverlays.Any(x=> x.CollectionName == selectedItem.CollectionName && x.OverlayName == selectedItem.OverlayName && x.ZoneName == selectedItem.ZoneName))
        {
            PedCustomizer.WorkingVariation.AppliedOverlays?.RemoveAll(x => x.CollectionName == selectedItem.CollectionName && x.OverlayName == selectedItem.OverlayName && x.ZoneName == selectedItem.ZoneName);
        }
        else
        {
            PedCustomizer.WorkingVariation.AppliedOverlays.Add(new AppliedOverlay(selectedItem.CollectionName, selectedItem.OverlayName, ZoneName));
        }     
        PedCustomizer.OnVariationChanged();
    }

}

