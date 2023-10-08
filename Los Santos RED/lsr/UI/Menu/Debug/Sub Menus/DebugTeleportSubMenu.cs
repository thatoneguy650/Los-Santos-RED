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



        UIMenuItem teleportToMarker = new UIMenuItem("Teleport To Marker", "Teleport to the current marker.");
        teleportToMarker.Activated += (sender, selectedItem) =>
        {
            Player.GPSManager.TeleportToMarker();
            sender.Visible = false;
        };
        LocationItemsMenu.AddItem(teleportToMarker);



        List<GameLocation> DirectoryLocations = PlacesOfInterest.AllLocations().ToList();
        foreach (string typeName in DirectoryLocations.OrderBy(x => x.TypeName).Select(x => x.TypeName).Distinct())
        {
            UIMenuListScrollerItem<GameLocation> myLocationType = new UIMenuListScrollerItem<GameLocation>($"{typeName}", "Teleports to A POI on the Map", DirectoryLocations.Where(x => x.TypeName == typeName));
            myLocationType.Activated += (menu, item) =>
            {
                GameLocation toTele = myLocationType.SelectedItem;
                if (toTele != null)
                {

                    //SpawnLocation spawnLocation = new SpawnLocation(toTele.EntrancePosition);
                    //spawnLocation.GetClosestStreet(false);
                    //spawnLocation.GetClosestSideOfRoad();
                    //if(spawnLocation.HasSideOfRoadPosition)
                    //{
                    //    spawnLocation.StreetPosition = Player.GPSManager.ForceGroundZ(spawnLocation.StreetPosition);
                    //    EntryPoint.WriteToConsole($"TELE HAS SIDE OF ROAD POS");
                    //    Game.LocalPlayer.Character.Position = spawnLocation.StreetPosition;
                    //    Game.LocalPlayer.Character.Heading = spawnLocation.Heading;
                    //    return;
                    //}
                    //if (spawnLocation.HasStreetPosition)
                    //{
                    //    spawnLocation.StreetPosition = Player.GPSManager.ForceGroundZ(spawnLocation.StreetPosition);
                    //    EntryPoint.WriteToConsole($"TELE HAS STREET POS");
                    //    Game.LocalPlayer.Character.Position = spawnLocation.StreetPosition;
                    //    Game.LocalPlayer.Character.Heading = spawnLocation.Heading;
                    //    return;
                    //}
                    EntryPoint.WriteToConsole($"TELE HAS ONLY ENTRANCE POS");
                    Game.LocalPlayer.Character.Position = toTele.EntrancePosition;
                    Game.LocalPlayer.Character.Heading = toTele.EntranceHeading;
                }
                //menu.Visible = false;
            };
            LocationItemsMenu.AddItem(myLocationType);
        }
    }
}

