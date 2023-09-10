using LosSantosRED.lsr.Interface;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DebugTeleportSubMenu : DebugSubMenu
{
    private IPlacesOfInterest PlacesOfInterest;
    public DebugTeleportSubMenu(UIMenu debug, MenuPool menuPool, IActionable player, IPlacesOfInterest placesOfInterest) : base(debug, menuPool, player)
    {
        PlacesOfInterest = placesOfInterest;
    }
    public override void AddItems()
    {
        UIMenu LocationItemsMenu = MenuPool.AddSubMenu(Debug, "Teleport Menu");
        LocationItemsMenu.SetBannerType(EntryPoint.LSRedColor);
        Debug.MenuItems[Debug.MenuItems.Count() - 1].Description = "Teleport to various locations";
        LocationItemsMenu.Width = 0.6f;
        List<GameLocation> DirectoryLocations = PlacesOfInterest.AllLocations().ToList();
        foreach (string typeName in DirectoryLocations.OrderBy(x => x.TypeName).Select(x => x.TypeName).Distinct())
        {
            UIMenuListScrollerItem<GameLocation> myLocationType = new UIMenuListScrollerItem<GameLocation>($"{typeName}", "Teleports to A POI on the Map", DirectoryLocations.Where(x => x.TypeName == typeName));
            myLocationType.Activated += (menu, item) =>
            {
                GameLocation toTele = myLocationType.SelectedItem;
                if (toTele != null)
                {
                    Game.LocalPlayer.Character.Position = toTele.EntrancePosition;
                    Game.LocalPlayer.Character.Heading = toTele.EntranceHeading;
                }
                //menu.Visible = false;
            };
            LocationItemsMenu.AddItem(myLocationType);
        }
    }
}

