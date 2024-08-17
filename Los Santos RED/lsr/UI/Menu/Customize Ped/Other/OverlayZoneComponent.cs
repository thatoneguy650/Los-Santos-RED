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
    private List<UIMenuListScrollerItem<TattooOverlay>> MenusToUpdate = new List<UIMenuListScrollerItem<TattooOverlay>>();
   
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
        MenusToUpdate.Clear();
        AddResetMenuItem(componentMenu);
        PossibleTattoos = PedCustomizer.TattooNames.GetOverlaysByZone(ZoneName).ToList();
        ResetApplied();
        foreach (var newTypeGroup in PossibleTattoos.GroupBy(x => x.Type).Select(x => x))
        {
            string formattedTypeGroup = newTypeGroup.Key == "TYPE_BADGE" ? "Badge" : newTypeGroup.Key == "TYPE_TATTOO" ? "Tattoo" : newTypeGroup.Key;
            UIMenu GroupMenu = MenuPool.AddSubMenu(componentMenu, formattedTypeGroup);
            GroupMenu.SetBannerType(EntryPoint.LSRedColor);
            GroupMenu.Width = 0.35f;
            foreach (var collectionGroup in PossibleTattoos.Where(x => x.Type == newTypeGroup.Key).GroupBy(x => x.CollectionName).Select(x => x))
            {
                UIMenu SubGroupMenu = MenuPool.AddSubMenu(GroupMenu, collectionGroup.Key);
                SubGroupMenu.SetBannerType(EntryPoint.LSRedColor);
                SubGroupMenu.Width = 0.35f;
                string GenderLimit = "";




                UIMenuListScrollerItem<TattooOverlay> TattooOverlayMenuList = new UIMenuListScrollerItem<TattooOverlay>("Item", "Select item",
                    PossibleTattoos.Where(x => x.CollectionName == collectionGroup.Key && x.Type == newTypeGroup.Key && (GenderLimit == "" || x.Gender == GenderLimit)));
                TattooOverlayMenuList.IndexChanged += (sender,oldIndex,newIndex) =>
                {
                    OnOverlayPreviewChanged(TattooOverlayMenuList.SelectedItem);
                };
                UIMenuItem ApplyOverlayMenuItem = new UIMenuItem("Apply", "Apply the select overlay.");
                ApplyOverlayMenuItem.Activated += (sender, selectedItem) =>
                {
                    OnOverlayChanged(TattooOverlayMenuList.SelectedItem);
                    TattooOverlayMenuList.Reformat();
                };


                UIMenuListScrollerItem<string> LimitGender = new UIMenuListScrollerItem<string>("Gender", "Select to limit the search results gender.", new List<string>() { "M", "F", "U", "All" });
                LimitGender.Activated += (sender, selectedItem) =>
                {
                    GenderLimit = LimitGender.SelectedItem == "All" ? "" : LimitGender.SelectedItem;
                    TattooOverlayMenuList.Items = PossibleTattoos.Where(x => x.CollectionName == collectionGroup.Key && x.Type == newTypeGroup.Key && (GenderLimit == "" || x.Gender == GenderLimit)).ToList();// VoicePreviewList.Where(x => FilterString == "" || x.VoiceName.ToLower().Contains(FilterString.ToLower())).OrderBy(x => x.TypeName).ThenByDescending(x => x.Gender).ThenBy(x => x.VoiceName).ToList();
                };
                SubGroupMenu.AddItem(LimitGender);


                SubGroupMenu.AddItem(TattooOverlayMenuList);
                SubGroupMenu.AddItem(ApplyOverlayMenuItem);
                MenusToUpdate.Add(TattooOverlayMenuList);
                SubGroupMenu.OnMenuOpen += (sender) =>
                {
                    OnOverlayPreviewChanged(TattooOverlayMenuList.SelectedItem);
                };
                SubGroupMenu.OnMenuClose += (sender) =>
                {
                    PedCustomizer.OnVariationChanged();
                };

            }
        }
    }
    private void OnOverlayPreviewChanged(TattooOverlay selectedItem)
    {
        //apply the preview
        if(selectedItem == null)
        {
            return;
        }
        if(!PedCustomizer.ModelPed.Exists())
        {
            return;
        }
        PedCustomizer.OnVariationChanged();
        uint collectionHash = Game.GetHashKey(selectedItem.CollectionName);
        uint overlayHash = Game.GetHashKey(selectedItem.OverlayName);
        NativeFunction.Natives.ADD_PED_DECORATION_FROM_HASHES(PedCustomizer.ModelPed, collectionHash, overlayHash);

    }





    //private void AddMenuItems(UIMenu componentMenu)
    //{
    //    MenusToUpdate.Clear();
    //    AddResetMenuItem(componentMenu);
    //    PossibleTattoos = PedCustomizer.TattooNames.GetOverlaysByZone(ZoneName).ToList();
    //    ResetApplied();
    //    foreach (var speechGroup in PossibleTattoos.GroupBy(x => x.CollectionName).Select(x => x))
    //    {
    //        UIMenu GroupMenu = MenuPool.AddSubMenu(componentMenu, speechGroup.Key);
    //        GroupMenu.SetBannerType(EntryPoint.LSRedColor);
    //        GroupMenu.Width = 0.35f;
    //        foreach(var typeGroup in PossibleTattoos.Where(x => x.CollectionName == speechGroup.Key).GroupBy(x=> x.Type).Select(x=> x))
    //        {
    //            string formattedTypeGroup = typeGroup.Key == "TYPE_BADGE" ? "Badge" : typeGroup.Key == "TYPE_TATTOO" ? "Tattoo" : typeGroup.Key;
    //            UIMenu SubGroupMenu = MenuPool.AddSubMenu(GroupMenu, formattedTypeGroup);
    //            SubGroupMenu.SetBannerType(EntryPoint.LSRedColor);
    //            SubGroupMenu.Width = 0.35f;

    //            UIMenuListScrollerItem<TattooOverlay> TattooOverlayMenuList = new UIMenuListScrollerItem<TattooOverlay>("Item", "Select item", 
    //                PossibleTattoos.Where(x => x.CollectionName == speechGroup.Key && x.Type == typeGroup.Key));
    //            //TattooOverlayMenuList.Activated += (sender, selectedItem) =>
    //            //{
    //            //    OnOverlayChanged(TattooOverlayMenuList.SelectedItem);
    //            //    TattooOverlayMenuList.Reformat();
    //            //};

    //            UIMenuItem ApplyOverlayMenuItem = new UIMenuItem("Apply","Apply the select overlay.");
    //            ApplyOverlayMenuItem.Activated += (sender, selectedItem) =>
    //             {
    //                 OnOverlayChanged(TattooOverlayMenuList.SelectedItem);
    //                 TattooOverlayMenuList.Reformat();
    //             };
    //            SubGroupMenu.AddItem(TattooOverlayMenuList);


    //            SubGroupMenu.AddItem(ApplyOverlayMenuItem);

    //            MenusToUpdate.Add(TattooOverlayMenuList);
    //        }
    //    } 
    //}
    private void AddResetMenuItem(UIMenu componentMenu)
    {
        ResetMenu = new UIMenuItem("Reset All", "Reset all the overlays applied");
        ResetMenu.RightBadge = UIMenuItem.BadgeStyle.Alert;
        ResetMenu.Activated += (sender, e) =>
        {
            PossibleTattoos.ForEach(x => x.IsApplied = false);
            PedCustomizer.WorkingVariation.AppliedOverlays?.Clear();
            PedCustomizer.OnVariationChanged();
            foreach(UIMenuListScrollerItem<TattooOverlay> ui in MenusToUpdate)
            {
                ui.Reformat();
            }
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
            foreach (UIMenuListScrollerItem<TattooOverlay> ui in MenusToUpdate)
            {
                ui.Reformat();
            }
            Game.DisplaySubtitle($"Removed All from {ZoneDisplay}");
        };
        componentMenu.AddItem(ResetZoneMenu);
    }

    //private void OnOverlayPreviewed(TattooOverlay selectedItem)
    //{
    //    if (selectedItem == null)
    //    {
    //        return;
    //    }
    //    if (PedCustomizer.WorkingVariation.AppliedOverlays == null)
    //    {
    //        PedCustomizer.WorkingVariation.AppliedOverlays = new List<AppliedOverlay>();
    //    }
    //    if (PedCustomizer.WorkingVariation.AppliedOverlays.Any(x => x.CollectionName == selectedItem.CollectionName && x.OverlayName == selectedItem.OverlayName && x.ZoneName == selectedItem.ZoneName))
    //    {
    //        //selectedItem.IsApplied = false;
    //       // PedCustomizer.WorkingVariation.AppliedOverlays?.RemoveAll(x => x.CollectionName == selectedItem.CollectionName && x.OverlayName == selectedItem.OverlayName && x.ZoneName == selectedItem.ZoneName);
    //        //Game.DisplaySubtitle($"Removed {selectedItem.OverlayName} from {ZoneDisplay}");
    //    }
    //    else
    //    {
    //        //selectedItem.IsApplied = true;
    //        //PedCustomizer.WorkingVariation.AppliedOverlays.Add(new AppliedOverlay(selectedItem.CollectionName, selectedItem.OverlayName, ZoneName));
    //        //Game.DisplaySubtitle($"Added {selectedItem.OverlayName} to {ZoneDisplay}");
    //    }
    //    PedCustomizer.OnVariationChanged();
    //}


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
    private void ResetApplied()
    {
        if(PossibleTattoos == null)
        {
            return;
        }
        foreach (TattooOverlay to in PossibleTattoos)
        {
            if (PedCustomizer.WorkingVariation.AppliedOverlays.Any(x => x.CollectionName == to.CollectionName && x.OverlayName == to.OverlayName && x.ZoneName == to.ZoneName))
            {
                to.IsApplied = true;
            }
            else
            {
                to.IsApplied = false;
            }
        }
    }
}

