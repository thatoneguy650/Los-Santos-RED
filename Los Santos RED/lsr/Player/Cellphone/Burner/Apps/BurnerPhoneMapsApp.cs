using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Mod;
using NAudio.Wave.Compression;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using RAGENativeUI.PauseMenu;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.TextFormatting;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

public class BurnerPhoneMapsApp : BurnerPhoneApp
{
    private MenuPool MenuPool;
    private UIMenu MapsMenu;
    private IPlacesOfInterest PlacesOfInterest;
    private IEntityProvideable World;

    private Texture MapsMenuBanner;
    // Filter Strings
    private string AddressFilterString = "";
    private string NameFilterString = "";
    private string TypeFilterString = "";
    private string ZoneFilterString = "";
    private string DistanceFilterString = "";
    private string InteriorFilter = "Any";
    private bool OpenFilter = true;

    private bool _destinationsBuilt = false;
    public BurnerPhoneMapsApp(BurnerPhone burnerPhone, ICellPhoneable player, ITimeReportable time, ISettingsProvideable settings, int index, IPlacesOfInterest placesOfInterest, IEntityProvideable world)
        : base(burnerPhone, player, time, settings, index, "Maps", 6)
    {
        if (!string.IsNullOrEmpty(settings.SettingsManager.PlayerOtherSettings.BurnerPhoneMapsAppName))
            Name = settings.SettingsManager.PlayerOtherSettings.BurnerPhoneMapsAppName;

        PlacesOfInterest = placesOfInterest;
        World = world;
        EntryPoint.WriteToConsole("BurnerPhoneMapsApp BULLSHIT RAN");

        MenuPool = new MenuPool();
        MapsMenuBanner = Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\{Settings.SettingsManager.PlayerOtherSettings.BurnerPhoneMapsAppBannerLocation}");
        SetupInteraction();
    }
    public override void Open(bool Reset)
    {
        StartLoop();
    }
    private void SetupInteraction()
    {
        MapsMenu = new UIMenu("", "Select an Option");
        SetMenuBanner(MapsMenu);
        MenuPool.Add(MapsMenu);

        // Destinations
        AddDestinationMenu();
        // Quick GPS
        // Remove GPS
        AddRemoveGPS(MapsMenu);
    }
    private void StartLoop()
    {
        MapsMenu.Visible = true;

        GameFiber.StartNew(delegate
        {
            try
            {
                while (MenuPool.IsAnyMenuOpen())
                {
                    GameFiber.Yield();
                }
                Game.RawFrameRender -= (s, e) => MenuPool.DrawBanners(e.Graphics);
                Player.CellPhone.Close(250);
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole(ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "CellPhone");
    }
    public override void Update()
    {
        if (MenuPool != null)
        {
            MenuPool.ProcessMenus();
        }
    }
    public void OnLeftMaps()
    {

    }
    private void SetMenuBanner(UIMenu menu)
    {
        if (MapsMenuBanner != null)
        {
            menu.SetBannerType(MapsMenuBanner);
            Game.RawFrameRender += (s, e) => MenuPool.DrawBanners(e.Graphics);
        }
        else
        {
            menu.RemoveBanner();
        }
    }
    private void AddDestinationMenu()
    {
        UIMenu DestinationSubMenu = MenuPool.AddSubMenu(MapsMenu, "Locations");
        MapsMenu.MenuItems[MapsMenu.MenuItems.Count() - 1].Description = "Find your way around with Nudle Maps, the smartest way to navigate the city without getting lost.";
        SetMenuBanner(DestinationSubMenu);

        DestinationSubMenu.OnMenuOpen += (sender1) =>
        {
            if (_destinationsBuilt)
                return;

            DestinationSubMenu.Clear();
            UIMenu FilterMenu = MenuPool.AddSubMenu(DestinationSubMenu, "Location Filter");
            SetMenuBanner(FilterMenu);
            AdjustFilters(FilterMenu);

            List<UIMenu> SubMenus = new List<UIMenu>();
            List<GameLocation> PossibleLocations = PlacesOfInterest.PossibleLocations.InteractableLocations().Where(x => LocationFitsFilter(x)).ToList();
            List<GameLocation> DistanceSorted = PossibleLocations.OrderBy(x => Player.Character.Position.DistanceTo2D(x.EntrancePosition)).ToList();
            foreach (GameLocation gameLocation in PossibleLocations.OrderBy(x => x.TypeName))
            {
                if (!gameLocation.IsEnabled || !gameLocation.ShowsOnTaxi || !gameLocation.IsSameState(Player.CurrentLocation?.CurrentZone?.GameState) || !gameLocation.IsCorrectMap(World.IsMPMapLoaded))
                {
                    continue;
                }
                UIMenu categoryToAdd = SubMenus.FirstOrDefault(x => x.SubtitleText == gameLocation.TypeName);
                if (categoryToAdd == null)
                {
                    categoryToAdd = MenuPool.AddSubMenu(DestinationSubMenu, gameLocation.TypeName);
                    SetMenuBanner(categoryToAdd);
                    SubMenus.Add(categoryToAdd);
                }
            }
            foreach (GameLocation gameLocation in DistanceSorted)
            {
                if (!gameLocation.IsEnabled || !gameLocation.ShowsOnTaxi || !gameLocation.IsSameState(Player.CurrentLocation?.CurrentZone?.GameState) || !gameLocation.IsCorrectMap(World.IsMPMapLoaded))
                {
                    continue;
                }
                UIMenu categoryToAdd = SubMenus.FirstOrDefault(x => x.SubtitleText == gameLocation.TypeName);
                if (categoryToAdd == null)
                {
                    continue;
                }


                float Distance = Player.Character.Position.DistanceTo2D(gameLocation.EntrancePosition);
                float distanceMile = Distance * 0.000621371f;
                UIMenuItem setGPSLocation = new UIMenuItem(gameLocation.Name, gameLocation.MapsInfo(Time.CurrentHour, Player.Character.DistanceTo2D(gameLocation.EntrancePosition))) { RightLabel = ($"~s~{Math.Round(distanceMile, 2)} mi") };
                setGPSLocation.Activated += (sender, e) =>
                {
                    Player.GPSManager.AddGPSRoute(gameLocation.Name, gameLocation.EntrancePosition, true);
                    sender.Visible = false;
                };

                categoryToAdd.OnMenuOpen += (sender) =>
                {
                    UIMenuItem currentItem = sender.CurrentItem;

                    if (currentItem == setGPSLocation)
                    {
                        if (gameLocation.BannerImage == null)
                            gameLocation.BannerImage = Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\{gameLocation.BannerImagePath}");

                        if (gameLocation.BannerImage != null)
                        {
                            categoryToAdd.SetBannerType(gameLocation.BannerImage);
                        }
                        else
                        {
                            categoryToAdd.TitleText = gameLocation.Name;
                            categoryToAdd.SetBannerType(EntryPoint.LSRedColor);
                        }
                    }
                };

                categoryToAdd.OnIndexChange += (menu, newIndex) =>
                {
                    UIMenuItem currentItem = menu.MenuItems[newIndex];

                    if (currentItem == setGPSLocation)
                    {
                        if (gameLocation.BannerImage == null)
                            gameLocation.BannerImage = Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\{gameLocation.BannerImagePath}");

                        if (gameLocation.BannerImage != null)
                        {
                            categoryToAdd.SetBannerType(gameLocation.BannerImage);
                        }
                        else
                        {
                            categoryToAdd.TitleText = gameLocation.Name;
                            categoryToAdd.SetBannerType(EntryPoint.LSRedColor);
                        }
                    }
                };
                categoryToAdd.AddItem(setGPSLocation);
            }

            _destinationsBuilt = true;
        };
    }
    private void AdjustFilters(UIMenu menu)
    {
        UIMenuItem clearCriteria = new UIMenuItem("Clear Criteria", "Clear search criteria") { RightBadge = UIMenuItem.BadgeStyle.Alert };
        UIMenuItem addressFilter = new UIMenuItem("Address", $"Filter by address" + (string.IsNullOrEmpty(AddressFilterString) ? "" : $"\n~g~{AddressFilterString}")) { RightBadge = string.IsNullOrEmpty(AddressFilterString) ? UIMenuItem.BadgeStyle.None : UIMenuItem.BadgeStyle.Star };
        UIMenuItem nameFilter = new UIMenuItem("Name", $"Filter by location name" + (string.IsNullOrEmpty(NameFilterString) ? "" : $"\n~g~{NameFilterString}")) { RightBadge = string.IsNullOrEmpty(NameFilterString) ? UIMenuItem.BadgeStyle.None : UIMenuItem.BadgeStyle.Star };
        UIMenuItem typeFilter = new UIMenuItem("Type", $"Filter by location type" + (string.IsNullOrEmpty(TypeFilterString) ? "" : $"\n~g~{TypeFilterString}")) { RightBadge = string.IsNullOrEmpty(TypeFilterString) ? UIMenuItem.BadgeStyle.None : UIMenuItem.BadgeStyle.Star };
        UIMenuItem zoneFilter = new UIMenuItem("Zone", $"Filter by zone" + (string.IsNullOrEmpty(ZoneFilterString) ? "" : $"\n~g~{ZoneFilterString}")) { RightBadge = string.IsNullOrEmpty(ZoneFilterString) ? UIMenuItem.BadgeStyle.None : UIMenuItem.BadgeStyle.Star };
        UIMenuItem distanceFilter = new UIMenuItem("Distance", $"Filter by max distance (mi)" + (string.IsNullOrEmpty(DistanceFilterString) ? "" : $"\n~g~{DistanceFilterString}")) { RightBadge = string.IsNullOrEmpty(DistanceFilterString) ? UIMenuItem.BadgeStyle.None : UIMenuItem.BadgeStyle.Star };
        UIMenuCheckboxItem openFilter = new UIMenuCheckboxItem("Open?", OpenFilter);
        UIMenuListScrollerItem<string> interiorFilter = new UIMenuListScrollerItem<string>("Interior Filter", "Filter locations by interior availability\n~r~Activate filter for it to take effect.", new List<string>() { "Any", "Interior Only", "No Interior" })
        {
            SelectedItem = InteriorFilter
        };
        openFilter.CheckboxEvent += (sender, Checked) =>
        {
            OpenFilter = Checked;
        };
        interiorFilter.Activated += (sender, selectedItem) =>
        {
            InteriorFilter = interiorFilter.SelectedItem;
            Game.DisplaySubtitle($"Updated InteriorFilter to ~r~{InteriorFilter}");
        };

        menu.AddItem(clearCriteria);
        menu.AddItem(addressFilter);
        menu.AddItem(nameFilter);
        menu.AddItem(typeFilter);
        menu.AddItem(zoneFilter);
        menu.AddItem(distanceFilter);
        menu.AddItem(openFilter);
        menu.AddItem(interiorFilter);

        menu.OnItemSelect += (sender, selectedItem, index) =>
        {
            _destinationsBuilt = false;

            if (selectedItem.Text.Contains("Clear Criteria"))
            {
                AddressFilterString = "";
                NameFilterString = "";
                TypeFilterString = "";
                ZoneFilterString = "";
                DistanceFilterString = "";

                addressFilter.Description = $"Filter by address";
                nameFilter.Description = $"Filter by location name";
                typeFilter.Description = $"Filter by location type";
                zoneFilter.Description = $"Filter by zone";
                distanceFilter.Description = $"Filter by max distance (mi)";

                addressFilter.RightBadge = UIMenuItem.BadgeStyle.None;
                nameFilter.RightBadge = UIMenuItem.BadgeStyle.None;
                typeFilter.RightBadge = UIMenuItem.BadgeStyle.None;
                zoneFilter.RightBadge = UIMenuItem.BadgeStyle.None;
                distanceFilter.RightBadge = UIMenuItem.BadgeStyle.None;

                OpenFilter = true;
                InteriorFilter = "Any";
                interiorFilter.SelectedItem = InteriorFilter;
            }
            else if (selectedItem.Text.Contains("Address"))
            {
                AddressFilterString = (NativeHelper.GetKeyboardInput("") ?? string.Empty).Trim();
                addressFilter.Description = "Filter by address" + (string.IsNullOrEmpty(AddressFilterString) ? "" : $"\n~g~{AddressFilterString}");
                addressFilter.RightBadge = string.IsNullOrEmpty(AddressFilterString) ? UIMenuItem.BadgeStyle.None : UIMenuItem.BadgeStyle.Star;
            }
            else if (selectedItem.Text.Contains("Name"))
            {
                NameFilterString = (NativeHelper.GetKeyboardInput("") ?? string.Empty).Trim();
                nameFilter.Description = $"Filter by location name" + (string.IsNullOrEmpty(NameFilterString) ? "" : $"\n~g~{NameFilterString}");
                nameFilter.RightBadge = string.IsNullOrEmpty(NameFilterString) ? UIMenuItem.BadgeStyle.None : UIMenuItem.BadgeStyle.Star;
            }
            else if (selectedItem.Text.Contains("Type"))
            {
                TypeFilterString = (NativeHelper.GetKeyboardInput("") ?? string.Empty).Trim();
                typeFilter.Description = $"Filter by location type" + (string.IsNullOrEmpty(TypeFilterString) ? "" : $"\n~g~{TypeFilterString}");
                typeFilter.RightBadge = string.IsNullOrEmpty(TypeFilterString) ? UIMenuItem.BadgeStyle.None : UIMenuItem.BadgeStyle.Star;
            }
            else if (selectedItem.Text.Contains("Zone"))
            {
                ZoneFilterString = (NativeHelper.GetKeyboardInput("") ?? string.Empty).Trim();
                zoneFilter.Description = $"Filter by zone" + (string.IsNullOrEmpty(ZoneFilterString) ? "" : $"\n~g~{ZoneFilterString}");
                zoneFilter.RightBadge = string.IsNullOrEmpty(ZoneFilterString) ? UIMenuItem.BadgeStyle.None : UIMenuItem.BadgeStyle.Star;
            }
            else if (selectedItem.Text.Contains("Distance"))
            {
                DistanceFilterString = (NativeHelper.GetKeyboardInput("") ?? string.Empty).Trim();
                if (!DistanceFilterString.All(char.IsDigit)) DistanceFilterString = "";
                distanceFilter.Description = $"Filter by max distance (mi)" + (string.IsNullOrEmpty(DistanceFilterString) ? "" : $"\n~g~{DistanceFilterString}");
                distanceFilter.RightBadge = string.IsNullOrEmpty(DistanceFilterString) ? UIMenuItem.BadgeStyle.None : UIMenuItem.BadgeStyle.Star;
            }
        };
    }
    private bool LocationFitsFilter(GameLocation x)
    {
        return (x.ShowsOnDirectory || Settings.SettingsManager.WorldSettings.ShowAllLocationsOnDirectory) &&
                x.IsEnabled && x.IsSameState(Player.CurrentLocation?.CurrentZone?.GameState) && x.IsCorrectMap(World.IsMPMapLoaded) &&
                (string.IsNullOrEmpty(AddressFilterString) || AddressFilterString == "" || x.FullStreetAddress.ToLower().Contains(AddressFilterString.ToLower())) &&
                (string.IsNullOrEmpty(NameFilterString) || NameFilterString == "" || x.Name.ToLower().Contains(NameFilterString.ToLower())) &&
                (string.IsNullOrEmpty(TypeFilterString) || TypeFilterString == "" || x.TypeName.ToLower().Contains(TypeFilterString.ToLower())) &&
                (string.IsNullOrEmpty(ZoneFilterString) || ZoneFilterString == "" || World.Zones.GetZone(x.EntrancePosition).DisplayName.ToLower().Contains(ZoneFilterString.ToLower())) &&
                (string.IsNullOrEmpty(DistanceFilterString) || DistanceFilterString == "" || Math.Round(Player.Character.DistanceTo2D(x.EntrancePosition) * 0.000621371, 2) < Convert.ToInt32(DistanceFilterString)) &&
                x.IsOpen(Time.CurrentHour) == OpenFilter && (InteriorFilter == "Any" || (x.HasInterior && InteriorFilter == "Interior Only") || (!x.HasInterior && InteriorFilter == "No Interior"));
    }
    private void AddRemoveGPS(UIMenu menu)
    {
        UIMenuItem RemoveGPS = new UIMenuItem("Remove GPS", "Remove GPS marker");
        RemoveGPS.Activated += (sender, e) =>
        {
            Player.GPSManager.RemoveGPSRoute(true);
        };
        menu.AddItem(RemoveGPS);
    }
}

