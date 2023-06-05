using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

public class ScrapYard : GameLocation
{
    private UIMenu ScrapSubMenu;
    private readonly float VehiclePickupDistance = 25f;

    public ScrapYard() : base()
    {

    }
    public override string TypeName { get; set; } = "Scrap Yard";
    public override int MapIcon { get; set; } = (int)BlipSprite.CriminalCarsteal;
    public override float MapIconScale { get; set; } = 1.0f;
    public override string ButtonPromptText { get; set; }
    public ScrapYard(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {

    }
    public override bool CanCurrentlyInteract(ILocationInteractable player)
    {
        ButtonPromptText = $"Scrap Vehicle At {Name}";
        return true;
    }
    public override void OnInteract(ILocationInteractable player, IModItems modItems, IEntityProvideable world, ISettingsProvideable settings, IWeapons weapons, ITimeControllable time, IPlacesOfInterest placesOfInterest)
    {
        Player = player;
        ModItems = modItems;
        World = world;
        Settings = settings;
        Weapons = weapons;
        Time = time;

        if (IsLocationClosed())
        {
            return;
        }

        if (CanInteract)
        {
            Player.ActivityManager.IsInteractingWithLocation = true;
            CanInteract = false;

            GameFiber.StartNew(delegate
            {
                try
                {
                    StoreCamera = new LocationCamera(this, Player);
                    StoreCamera.Setup();
                    CreateInteractionMenu();
                    InteractionMenu.Visible = true;
                    InteractionMenu.OnItemSelect += InteractionMenu_OnItemSelect;

                    GenerateScrapYardMenu();

                    ProcessInteractionMenu();
                    DisposeInteractionMenu();
                    StoreCamera.Dispose();
                    Player.ActivityManager.IsInteractingWithLocation = false;
                    CanInteract = true;
                }
                catch (Exception ex)
                {
                    EntryPoint.WriteToConsole("Location Interaction" + ex.Message + " " + ex.StackTrace, 0);
                    EntryPoint.ModController.CrashUnload();
                }
            }, "HotelInteract");
        }
    }
    private void GenerateScrapYardMenu()
    {
        ScrapSubMenu = MenuPool.AddSubMenu(InteractionMenu, "Scrap a Vehicle");

        InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1].Description = "Select a vehicle to scrap for money. The bigger the better";
        InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1].RightBadge = UIMenuItem.BadgeStyle.Car;

        if (HasBannerImage)
        {
            BannerImage = Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\{BannerImagePath}");
            ScrapSubMenu.SetBannerType(BannerImage);
        }
        ScrapSubMenu.OnItemSelect += InteractionMenu_OnItemSelect;
        ScrapSubMenu.OnIndexChange += ScrapSubMenu_OnIndexChange;
        ScrapSubMenu.OnMenuOpen += ScrapSubMenu_OnMenuOpen;
        ScrapSubMenu.OnMenuClose += ScrapSubMenu_OnMenuClose;
        bool Added = false;
        foreach (VehicleExt veh in World.Vehicles.CivilianVehicleList)
        {
            if (IsValidForScrapping(veh))
            {
                string MakeName = NativeHelper.VehicleMakeName(veh.Vehicle.Model.Hash);
                string ModelName = NativeHelper.VehicleModelName(veh.Vehicle.Model.Hash);
                string ClassName = NativeHelper.VehicleClassName(veh.Vehicle.Model.Hash);
                string CarName = (MakeName + " " + ModelName).Trim();
                string CarDescription = "";
                float volume = GetVolume(veh);
                int ScrapPrice = GetScrapPrice(veh);
                if (volume != 0f)
                {
                    CarDescription += $"~n~Metal Volume: ~y~{Math.Round(volume, 2)}~s~ meters cubed~s~";
                }
                if (ScrapPrice != 0)
                {
                    CarDescription += $"~n~Metal Value: ~g~${ScrapPrice}~s~";
                }
                if (MakeName != "")
                {
                    CarDescription += $"~n~Manufacturer: ~b~{MakeName}~s~";
                }
                if (ModelName != "")
                {
                    CarDescription += $"~n~Model: ~g~{ModelName}~s~";
                }
                if (ClassName != "")
                {
                    CarDescription += $"~n~Class: ~p~{ClassName}~s~";
                }
                ScrapSubMenu.AddItem(new UIMenuItem(CarName, CarDescription) { RightLabel = ScrapPrice.ToString("C0") });
                Added = true;
            }
        }
        if(!Added)
        {
            InteractionMenu.Visible = false;
            PlayErrorSound();
            DisplayMessage("~r~Scrapping Failed", "Please come back with a vehicle to scrap!");
        }
    }
    private void ScrapSubMenu_OnMenuClose(UIMenu sender)
    {
        StoreCamera.ReHighlightStoreWithCamera();
    }
    private void ScrapSubMenu_OnMenuOpen(UIMenu sender)
    {
        ScrapSubMenu_OnIndexChange(sender, sender.CurrentSelection);
    }
    private void ScrapSubMenu_OnIndexChange(UIMenu sender, int newIndex)
    {
        if (sender != null && sender.MenuItems != null && sender.MenuItems.Any() && newIndex != -1)
        {
            UIMenuItem coolmen = sender.MenuItems[newIndex];
            if (coolmen != null)
            {
                VehicleExt carToScrap = GetVehicle(sender.MenuItems[newIndex].Text);
                if (carToScrap != null && carToScrap.Vehicle.Exists())
                {
                    StoreCamera.HighlightEntity(carToScrap.Vehicle);
                }
            }
        }

    }
    private void InteractionMenu_OnItemSelect(RAGENativeUI.UIMenu sender, UIMenuItem selectedItem, int index)
    {
        VehicleExt carToScrap = GetVehicle(selectedItem.Text);
        if(carToScrap != null && carToScrap.Vehicle.Exists())
        {
            ScrapVehicle(carToScrap, GetScrapPrice(carToScrap));
        }
    }
    private void ScrapVehicle(VehicleExt carToScrap, int Price)
    {
        if(carToScrap != null && carToScrap.Vehicle.Exists())
        {
            Game.FadeScreenOut(1000, true);     
            string MakeName = NativeHelper.VehicleMakeName(carToScrap.Vehicle.Model.Hash);
            string ModelName = NativeHelper.VehicleModelName(carToScrap.Vehicle.Model.Hash);
            string CarName = (MakeName + " " + ModelName).Trim();
            carToScrap.Vehicle.Delete();
            ScrapSubMenu.MenuItems.RemoveAll(x => x.Text == CarName);
            ScrapSubMenu.RefreshIndex();
            ScrapSubMenu.Close(true);
            Game.FadeScreenIn(1000, true);
            Player.BankAccounts.GiveMoney(Price);
            PlaySuccessSound();
            DisplayMessage("~g~Scrapped", $"Thank you for scrapping your ~p~{CarName}~s~ at ~y~{Name}~s~");
        }
        else
        {
            PlayErrorSound();
            DisplayMessage("~r~Scrapping Failed", "We are unable to complete this scrapping.");
        }        
    }
    private int GetScrapPrice(VehicleExt toScrap)
    {
        if(toScrap == null)
        {
            return 0;
        }
        if(toScrap.Vehicle.Exists())
        {
            return ((int)(GetVolume(toScrap) * 100)).Round(100);
        }
        else
        {
            return 0;
        }
    }
    private float GetVolume(VehicleExt toScrap)
    {
        if (toScrap == null)
        {
            return 0;
        }
        if (toScrap.Vehicle.Exists())
        {
            return toScrap.Vehicle.Model.Dimensions.X * toScrap.Vehicle.Model.Dimensions.Y * toScrap.Vehicle.Model.Dimensions.Z;
        }
        else
        {
            return 0;
        }
    }
    private VehicleExt GetVehicle(string menuEntry)
    {
        VehicleExt carToScrap = null;
        foreach (VehicleExt veh in World.Vehicles.CivilianVehicleList)
        {
            if (IsValidForScrapping(veh))
            {
                string MakeName = NativeHelper.VehicleMakeName(veh.Vehicle.Model.Hash);
                string ModelName = NativeHelper.VehicleModelName(veh.Vehicle.Model.Hash);
                string CarName = (MakeName + " " + ModelName).Trim();
                if (menuEntry == CarName)
                {
                    carToScrap = veh;
                }
            }
        }
        return carToScrap;
    }
    private bool IsValidForScrapping(VehicleExt toScrap)
    {
        if(toScrap.Vehicle.Exists() && toScrap.Vehicle.DistanceTo2D(EntrancePosition) <= VehiclePickupDistance && !toScrap.Vehicle.HasOccupants)
        {
            return true;
        }
        return false;
    }
}

