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
using System.Windows.Forms;
using System.Xml.Serialization;

public class CarCrusher : GameLocation
{
    private UIMenu CrusherSubMenu;

    public CarCrusher() : base()
    {

    }
    public override string TypeName { get; set; } = "Car Crusher";
    public override int MapIcon { get; set; } = 380;// (int)527;
    public override string ButtonPromptText { get; set; }
    public float VehiclePickupDistance { get; set; } = 15f;
    public int StandardCrushPrice { get; set; } = 500;
    public int PerBodyCrushFee { get; set; } = 2000;
    public CarCrusher(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {

    }
    public override bool CanCurrentlyInteract(ILocationInteractable player)
    {
        ButtonPromptText = $"Crush Vehicle At {Name}";
        return true;
    }
    public override void OnInteract()//ILocationInteractable player, IModItems modItems, IEntityProvideable world, ISettingsProvideable settings, IWeapons weapons, ITimeControllable time, IPlacesOfInterest placesOfInterest)
    {
        //Player = player;
        //ModItems = modItems;
        //World = world;
        //Settings = settings;
        //Weapons = weapons;
        //Time = time;
        if (IsLocationClosed())
        {
            return;
        }
        if(!CanInteract)
        {
            return;
        }
        if (Interior != null && Interior.IsTeleportEntry)
        {
            DoEntranceCamera(true);
            Interior.Teleport(Player, this, StoreCamera);
        }
        else
        {
            StandardInteract(null, false);
        }
    }
    public override void StandardInteract(LocationCamera locationCamera, bool isInside)
    {
        Player.ActivityManager.IsInteractingWithLocation = true;
        CanInteract = false;
        Player.IsTransacting = true;
        GameFiber.StartNew(delegate
        {
            try
            {
                SetupLocationCamera(locationCamera, isInside, true);
                CreateInteractionMenu();
                InteractionMenu.Visible = true;
                GenerateCrusherMenu();
                ProcessInteractionMenu();
                DisposeInteractionMenu();
                DisposeCamera(isInside);
                DisposeInterior();
                Player.ActivityManager.IsInteractingWithLocation = false;
                CanInteract = true;
                Player.IsTransacting = false;
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole("Location Interaction" + ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "CarCrusherInteract");
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
        foreach (VehicleExt veh in World.Vehicles.AllVehicleList)
        {
            if (!IsValidForCrushing(veh))
            {
                continue;
            }
            string CarName = veh.GetCarName();
            string CarDescription = veh.GetCarDescription();
            int CrushPrice = StandardCrushPrice;
            int StoredBodies = veh.VehicleBodyManager.StoredBodies.Count();
            int BodiesFee = PerBodyCrushFee * StoredBodies;
            CrushPrice += BodiesFee;
            if(StoredBodies > 0)
            {
                CarDescription += $"~n~~n~EXTRA Disposal Fee: ~r~${BodiesFee}~s~";
            }
            UIMenuItem vehicleCrusherItem = new UIMenuItem(CarName, CarDescription) { RightLabel = "~r~" + CrushPrice.ToString("C0") + "~s~" };
            vehicleCrusherItem.Activated += (sender, e) =>
            {
                CrushVehicle(veh, CrushPrice);
            };
            CrusherSubMenu.AddItem(vehicleCrusherItem);
            Added = true;      
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
        if(Player.BankAccounts.GetMoney(false) < Price)
        {
            PlayErrorSound();
            DisplayMessage("~r~Cash Only","You do not have enough cash on hand.");
            return;
        }
        Game.FadeScreenOut(1000, true);
        string CarName = carToScrap.GetCarName();
        foreach (StoredBody sb in carToScrap.VehicleBodyManager.StoredBodies)
        {
            if(sb.PedExt != null && sb.PedExt.Pedestrian.Exists())
            {
                sb.PedExt.WasCrushed = true;
                sb.PedExt.Pedestrian.Delete();
            }
        }
        foreach(Ped otherPed in carToScrap.Vehicle.Occupants)
        {
            if(otherPed.Exists())
            {
                otherPed.Delete();
            }
        }
        carToScrap.WasCrushed = true;
        carToScrap.Vehicle.Delete();
        CrusherSubMenu.MenuItems.RemoveAll(x => x.Text == CarName);
        CrusherSubMenu.RefreshIndex();
        CrusherSubMenu.Close(true);
        Game.FadeScreenIn(1000, true);
        Player.BankAccounts.GiveMoney(-1 * Price, false);
        PlaySuccessSound();
        DisplayMessage("~g~Crushed", $"Thank you for crushing your ~p~{CarName}~s~ at ~y~{Name}~s~");
    }
    private VehicleExt GetVehicle(string menuEntry)
    {
        VehicleExt carToScrap = null;
        foreach (VehicleExt veh in World.Vehicles.AllVehicleList)
        {
            if (!IsValidForCrushing(veh))
            {
                continue;
            }
            string CarName = veh.GetCarName();
            if (menuEntry == CarName)
            {
                carToScrap = veh;
            }
            
        }
        return carToScrap;
    }
    private bool IsValidForCrushing(VehicleExt toCrush)
    {
        if (toCrush.Vehicle.Exists() && toCrush.Vehicle.DistanceTo2D(EntrancePosition) <= VehiclePickupDistance && toCrush.HasBeenEnteredByPlayer)
        {
            return true;
        }
        return false;
    }
    public override void AddLocation(PossibleLocations possibleLocations)
    {
        possibleLocations.CarCrushers.Add(this);
        base.AddLocation(possibleLocations);
    }
}

