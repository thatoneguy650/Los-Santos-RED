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
    private UIMenu componentMenu;
    private UIMenuItem ResetMenu;
    private List<TattooOverlay> PossibleTattoos;
    private UIMenuListScrollerItem<TattooOverlay> TattooOverlayMenuList;
    private MenuPool MenuPool;
    private UIMenuItem ResetZoneMenu;

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
    public void AddCustomizeMenu(MenuPool menuPool, UIMenu topMenu, Ped ped, PedCustomizer pedCustomizer)
    {
        MenuPool = menuPool;
        Ped = ped;
        PedCustomizer = pedCustomizer;
        componentMenu = MenuPool.AddSubMenu(topMenu, ZoneDisplay);
        topMenu.MenuItems[topMenu.MenuItems.Count() - 1].Description = $"Customize the {ZoneDisplay}";
        topMenu.MenuItems[topMenu.MenuItems.Count() - 1].RightLabel = "";
        componentMenu.SetBannerType(EntryPoint.LSRedColor);
        componentMenu.Width = 0.35f;
        if (Ped.Exists())
        {
            AddMenuItems(componentMenu);
        }
    }
    private void AddMenuItems(UIMenu componentMenu)
    {
        AddResetMenuItem(componentMenu);
        PossibleTattoos = PedCustomizer.TattooNames.GetOverlaysByZone(ZoneName).ToList();
        foreach (var speechGroup in PossibleTattoos.GroupBy(x => x.CollectionName).Select(x => x))
        {
            UIMenu GroupMenu = MenuPool.AddSubMenu(componentMenu, speechGroup.Key);
            GroupMenu.SetBannerType(EntryPoint.LSRedColor);
            GroupMenu.Width = 0.35f;
            UIMenuListScrollerItem<TattooOverlay> TattooOverlayMenuList = new UIMenuListScrollerItem<TattooOverlay>("Item", "Select item", PossibleTattoos.Where(x => x.CollectionName == speechGroup.Key));
            TattooOverlayMenuList.Activated += (sender, selectedItem) =>
            {
                OnOverlayChanged(TattooOverlayMenuList.SelectedItem);
            };
            GroupMenu.AddItem(TattooOverlayMenuList);
        } 
    }
    private void AddResetMenuItem(UIMenu componentMenu)
    {
        ResetMenu = new UIMenuItem("Reset All", "Reset all the overlays applied");
        ResetMenu.RightBadge = UIMenuItem.BadgeStyle.Alert;
        ResetMenu.Activated += (sender, e) =>
        {
            PossibleTattoos.ForEach(x => x.IsApplied = false);
            PedCustomizer.WorkingVariation.AppliedOverlays?.Clear();
            PedCustomizer.OnVariationChanged();
            Game.DisplaySubtitle($"Removed All");
        };
        componentMenu.AddItem(ResetMenu);

        ResetZoneMenu = new UIMenuItem("Reset Zone", "Reset the overlays for the given zone");
        ResetZoneMenu.RightBadge = UIMenuItem.BadgeStyle.Tatoo;
        ResetZoneMenu.Activated += (sender, e) =>
        {
            PossibleTattoos.Where(x=> x.ZoneName == ZoneName).ToList().ForEach(x => x.IsApplied = false);
            PedCustomizer.WorkingVariation.AppliedOverlays?.RemoveAll(x => x.ZoneName == ZoneName);
            PedCustomizer.OnVariationChanged();
            Game.DisplaySubtitle($"Removed All from {ZoneDisplay}");
        };
        componentMenu.AddItem(ResetZoneMenu);
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
            selectedItem.IsApplied = false;
            PedCustomizer.WorkingVariation.AppliedOverlays?.RemoveAll(x => x.CollectionName == selectedItem.CollectionName && x.OverlayName == selectedItem.OverlayName && x.ZoneName == selectedItem.ZoneName);
            Game.DisplaySubtitle($"Removed {selectedItem.OverlayName} from {ZoneDisplay}");
        }
        else
        {
            selectedItem.IsApplied = true;
            PedCustomizer.WorkingVariation.AppliedOverlays.Add(new AppliedOverlay(selectedItem.CollectionName, selectedItem.OverlayName, ZoneName));
            Game.DisplaySubtitle($"Added {selectedItem.OverlayName} to {ZoneDisplay}");
        }     
        PedCustomizer.OnVariationChanged();
    }

}

