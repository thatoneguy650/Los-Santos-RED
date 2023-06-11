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

public class CarCrusher : GameLocation
{
    private UIMenu CrusherSubMenu;
    private readonly float VehiclePickupDistance = 25f;

    public CarCrusher() : base()
    {

    }
    public override string TypeName { get; set; } = "Car Crusher";
    public override int MapIcon { get; set; } = (int)BlipSprite.CriminalCarsteal;
    public override float MapIconScale { get; set; } = 1.0f;
    public override string ButtonPromptText { get; set; }
    public CarCrusher(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {

    }
    public override bool CanCurrentlyInteract(ILocationInteractable player)
    {
        ButtonPromptText = $"Crush Vehicle At {Name}";
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
                    StoreCamera = new LocationCamera(this, Player, Settings);
                    StoreCamera.Setup();
                    CreateInteractionMenu();
                    InteractionMenu.Visible = true;

                    GenerateCrusherMenu();

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
    private void GenerateCrusherMenu()
    {
        CrusherSubMenu = MenuPool.AddSubMenu(InteractionMenu, "Crush a Vehicle");

        InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1].Description = "Select a vehicle to crush. We don't ask questions";
        InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1].RightBadge = UIMenuItem.BadgeStyle.Car;

        if (HasBannerImage)
        {
            BannerImage = Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\{BannerImagePath}");
            CrusherSubMenu.SetBannerType(BannerImage);
        }
        CrusherSubMenu.OnIndexChange += ScrapSubMenu_OnIndexChange;
        CrusherSubMenu.OnMenuOpen += ScrapSubMenu_OnMenuOpen;
        CrusherSubMenu.OnMenuClose += ScrapSubMenu_OnMenuClose;
        bool Added = false;
        foreach (VehicleExt veh in World.Vehicles.CivilianVehicleList)
        {
            if (IsValidForCrushing(veh))
            {
                string MakeName = NativeHelper.VehicleMakeName(veh.Vehicle.Model.Hash);
                string ModelName = NativeHelper.VehicleModelName(veh.Vehicle.Model.Hash);
                string ClassName = NativeHelper.VehicleClassName(veh.Vehicle.Model.Hash);
                string CarName = (MakeName + " " + ModelName).Trim();
                string CarDescription = "";

                int CrushPrice = 500;
                //float volume = GetVolume(veh);
                //int ScrapPrice = GetScrapPrice(veh);
                //if (volume != 0f)
                //{
                //    CarDescription += $"~n~Metal Volume: ~y~{Math.Round(volume, 2)}~s~ meters cubed~s~";
                //}
                //if (ScrapPrice != 0)
                //{
                //    CarDescription += $"~n~Metal Value: ~g~${ScrapPrice}~s~";
                //}
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
                UIMenuItem vehicleCrusherItem = new UIMenuItem(CarName, CarDescription) { RightLabel = CrushPrice.ToString("C0") };
                vehicleCrusherItem.Activated += (sender, e) =>
                {
                    CrushVehicle(veh, -500);
                };
                CrusherSubMenu.AddItem(vehicleCrusherItem);
                Added = true;
            }
        }
        if (!Added)
        {
            InteractionMenu.Visible = false;
            PlayErrorSound();
            DisplayMessage("~r~Crushing Failed", "Please come back with a vehicle to crush!");
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
    private void CrushVehicle(VehicleExt carToScrap, int Price)
    {
        if(carToScrap == null || !carToScrap.Vehicle.Exists())
        {
            PlayErrorSound();
            DisplayMessage("~r~Crushing Failed", "We are unable to complete this crushing.");
            return;
        }
        if(Player.BankAccounts.Money < Price)
        {
            PlayErrorSound();
            DisplayMessage("~r~Insufficient Funds", "We are sorry, we are unable to complete this transation.");
            return;
        }
        Game.FadeScreenOut(1000, true);
        string MakeName = NativeHelper.VehicleMakeName(carToScrap.Vehicle.Model.Hash);
        string ModelName = NativeHelper.VehicleModelName(carToScrap.Vehicle.Model.Hash);
        string CarName = (MakeName + " " + ModelName).Trim();
        carToScrap.Vehicle.Delete();
        CrusherSubMenu.MenuItems.RemoveAll(x => x.Text == CarName);
        CrusherSubMenu.RefreshIndex();
        CrusherSubMenu.Close(true);
        Game.FadeScreenIn(1000, true);
        Player.BankAccounts.GiveMoney(Price);
        PlaySuccessSound();
        DisplayMessage("~g~Crushed", $"Thank you for crushing your ~p~{CarName}~s~ at ~y~{Name}~s~");
    }
    private VehicleExt GetVehicle(string menuEntry)
    {
        VehicleExt carToScrap = null;
        foreach (VehicleExt veh in World.Vehicles.CivilianVehicleList)
        {
            if (IsValidForCrushing(veh))
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
    private bool IsValidForCrushing(VehicleExt toScrap)
    {
        if (toScrap.Vehicle.Exists() && toScrap.Vehicle.DistanceTo2D(EntrancePosition) <= VehiclePickupDistance && !toScrap.Vehicle.HasOccupants)
        {
            return true;
        }
        return false;
    }
}

