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

public class ScrapYard : InteractableLocation
{
    private StoreCamera StoreCamera;

    private IActivityPerformable Player;
    private IModItems ModItems;
    private IEntityProvideable World;
    private ISettingsProvideable Settings;
    private IWeapons Weapons;
    private ITimeControllable Time;
    private UIMenu ScrapSubMenu;

    public ScrapYard() : base()
    {

    }
    public override int MapIcon { get; set; } = (int)BlipSprite.CriminalCarsteal;
    public override Color MapIconColor { get; set; } = Color.White;
    public override float MapIconScale { get; set; } = 1.0f;
    public override string ButtonPromptText { get; set; }
    public ScrapYard(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {
        ButtonPromptText = $"Scrap Vehicle At {Name}";
    }
    public override void OnInteract(IActivityPerformable player, IModItems modItems, IEntityProvideable world, ISettingsProvideable settings, IWeapons weapons, ITimeControllable time)
    {
        Player = player;
        ModItems = modItems;
        World = world;
        Settings = settings;
        Weapons = weapons;
        Time = time;

        if (CanInteract)
        {
            Player.IsInteractingWithLocation = true;
            CanInteract = false;

            GameFiber.StartNew(delegate
            {
                StoreCamera = new StoreCamera(this, Player);
                StoreCamera.Setup();
                CreateInteractionMenu();
                InteractionMenu.Visible = true;
                InteractionMenu.OnItemSelect += InteractionMenu_OnItemSelect;



                GenerateScrapYardMenu();

                ProcessInteractionMenu();
                DisposeInteractionMenu();
                StoreCamera.Dispose();
                Player.IsInteractingWithLocation = false;
                CanInteract = true;
            }, "HotelInteract");
        }
    }
    private void GenerateScrapYardMenu()
    {
        ScrapSubMenu = MenuPool.AddSubMenu(InteractionMenu, "Select Vehicle To Scrap");

        InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1].Description = "Select a vehicle to scrap for money.";
        InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1].RightBadge = UIMenuItem.BadgeStyle.Car;

        if (HasBannerImage)
        {
            BannerImage = Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\{BannerImagePath}");
            ScrapSubMenu.SetBannerType(BannerImage);
        }
        ScrapSubMenu.OnItemSelect += InteractionMenu_OnItemSelect;
        ScrapSubMenu.OnIndexChange += ScrapSubMenu_OnIndexChange;
        bool Added = false;
        foreach (VehicleExt veh in World.Vehicles.CivilianVehicleList.Where(x=> x.Vehicle.Exists() && x.Vehicle.DistanceTo2D(EntrancePosition) <= 25f))
        {
            string MakeName = NativeHelper.VehicleMakeName(veh.Vehicle.Model.Hash);
            string ModelName = NativeHelper.VehicleModelName(veh.Vehicle.Model.Hash);
            string ClassName = NativeHelper.VehicleClassName(veh.Vehicle.Model.Hash);
            string CarName = (MakeName + " " + ModelName).Trim();
            string CarDescription = "";
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
            float Length = veh.Vehicle.Model.Dimensions.Y;
            int ScrapPrice = ((int)(Length * 500)).Round(100);
            ScrapSubMenu.AddItem(new UIMenuItem(CarName, CarDescription) { RightLabel = ScrapPrice.ToString("C0") });
            Added = true;
        }
        if(!Added)
        {
            InteractionMenu.Visible = false;
            Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", Name, "~r~Scrapping Failed", "Please come back with a vehicle to scrap!");
        }
    }

    private void ScrapSubMenu_OnIndexChange(UIMenu sender, int newIndex)
    {
        VehicleExt carToScrap = null;
        foreach (VehicleExt veh in World.Vehicles.CivilianVehicleList.Where(x => x.Vehicle.Exists() && x.Vehicle.DistanceTo2D(EntrancePosition) <= 25f))
        {
            string MakeName = NativeHelper.VehicleMakeName(veh.Vehicle.Model.Hash);
            string ModelName = NativeHelper.VehicleModelName(veh.Vehicle.Model.Hash);
            string ClassName = NativeHelper.VehicleClassName(veh.Vehicle.Model.Hash);
            string CarName = (MakeName + " " + ModelName).Trim();
            if (sender.MenuItems[newIndex].Text == CarName)
            {
                carToScrap = veh;
            }
        }
        if (carToScrap != null && carToScrap.Vehicle.Exists())
        {
            StoreCamera.HighlightEntity(carToScrap.Vehicle);
            //float Length = carToScrap.Vehicle.Model.Dimensions.Y;
            //int ScrapPrice = ((int)(Length * 250)).Round(100);
            //ScrapVehicle(carToScrap, ScrapPrice);
        }
    }

    private void InteractionMenu_OnItemSelect(RAGENativeUI.UIMenu sender, UIMenuItem selectedItem, int index)
    {
        VehicleExt carToScrap = null;
        foreach (VehicleExt veh in World.Vehicles.CivilianVehicleList.Where(x => x.Vehicle.Exists() && x.Vehicle.DistanceTo2D(EntrancePosition) <= 25f))
        {
            string MakeName = NativeHelper.VehicleMakeName(veh.Vehicle.Model.Hash);
            string ModelName = NativeHelper.VehicleModelName(veh.Vehicle.Model.Hash);
            string ClassName = NativeHelper.VehicleClassName(veh.Vehicle.Model.Hash);
            string CarName = (MakeName + " " + ModelName).Trim();
            if(selectedItem.Text == CarName)
            {
                carToScrap = veh;
            }
        }
        if(carToScrap != null && carToScrap.Vehicle.Exists())
        {
            float Length = carToScrap.Vehicle.Model.Dimensions.Y;
            int ScrapPrice = ((int)(Length * 250)).Round(100);
            ScrapVehicle(carToScrap, ScrapPrice);
        }
    }
    private void ScrapVehicle(VehicleExt carToScrap, int Price)
    {
        if(carToScrap != null && carToScrap.Vehicle.Exists())
        {
            Game.FadeScreenOut(1000, true);
            Player.GiveMoney(Price);
            carToScrap.Vehicle.Delete();
            Game.FadeScreenIn(1000, true);
            InteractionMenu.Visible = false;
            Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", Name, "~g~Scrapped", $"Thank you for scrapping your vehicle at {Name}");
        }
        else
        {
            Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", Name, "~r~Scrapping Failed", "We are unable to complete this scrapping.");
        }        
    }
}

