using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

public class Hospital : GameLocation, ILocationRespawnable, ILicensePlatePreviewable
{
    private ShopMenu agencyMenu;
    private List<MedicalTreatment> MedicalTreatments;
    public Hospital(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {

    }
    public Hospital() : base()
    {

    }
    public string LicensePlatePreviewText { get; set; } = "UNIT1";
    public override string TypeName { get; set; } = "Hospital";
    public override int MapIcon { get; set; } = (int)BlipSprite.Hospital;
    public Vector3 RespawnLocation { get; set; }
    public float RespawnHeading { get; set; }
    public string TreatmentOptionsID { get; set; } = "DefaultMedicalTreatments";
    public override void StoreData(IShopMenus shopMenus, IAgencies agencies, IGangs gangs, IZones zones, IJurisdictions jurisdictions, IGangTerritories gangTerritories, INameProvideable Names, ICrimes Crimes, IPedGroups PedGroups, IEntityProvideable world,
        IStreets streets, ILocationTypes locationTypes, ISettingsProvideable settings, IPlateTypes plateTypes, IOrganizations associations)
    {
        base.StoreData(shopMenus, agencies, gangs, zones, jurisdictions, gangTerritories, Names, Crimes, PedGroups, world, streets, locationTypes, settings, plateTypes, associations);
        if (AssignedAgency == null)
        {
            AssignedAgency = zones.GetZone(EntrancePosition)?.AssignedEMSAgency;
        }
        if(!string.IsNullOrEmpty(TreatmentOptionsID))
        {
            MedicalTreatments = shopMenus.GetMedicalTreatments(TreatmentOptionsID);
        }
    }
    public override void AddDistanceOffset(Vector3 offsetToAdd)
    {
        if (RespawnLocation != Vector3.Zero)
        {
            RespawnLocation += offsetToAdd;
        }
        base.AddDistanceOffset(offsetToAdd);
    }
    public override bool CanCurrentlyInteract(ILocationInteractable player)
    {
        ButtonPromptText = $"Enter {Name}";
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
        if (!CanInteract)
        {
            return;
        }
        if (AssignedAgency == null)
        {
            Game.DisplayHelp("No Agency Assigned");
            return;
        }
        Player.ActivityManager.IsInteractingWithLocation = true;
        CanInteract = false;
        Player.IsTransacting = true;
        GameFiber.StartNew(delegate
        {
            try
            {
                StoreCamera = new LocationCamera(this, Player, Settings);
                StoreCamera.Setup();
                CreateInteractionMenu();
                if (Player.IsEMT)
                {            
                    InteractAsEMT(modItems, world, settings, weapons, time);
                }
                else
                {
                    InteractAsOther();
                }    
                DisposeInteractionMenu();
                StoreCamera.Dispose();
                Player.IsTransacting = false;
                Player.ActivityManager.IsInteractingWithLocation = false;
                CanInteract = true;
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole("Location Interaction" + ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "BarInteract");    
    }

    private void InteractAsEMT(IModItems modItems, IEntityProvideable world, ISettingsProvideable settings, IWeapons weapons, ITimeControllable time)
    {
        agencyMenu = AssignedAgency.GenerateMenu(ModItems);
        Transaction = new Transaction(MenuPool, InteractionMenu, agencyMenu, this);
        Transaction.LicensePlatePreviewable = this;
        if ((VehicleDeliveryLocations == null || !VehicleDeliveryLocations.Any()) && PossibleVehicleSpawns?.Any() == true)
        {
            List<SpawnPlace> places = new List<SpawnPlace>();
            foreach (ConditionalLocation place in PossibleVehicleSpawns)
            {
                places.Add(new SpawnPlace(place.Location, place.Heading));
            }
            Transaction.VehicleDeliveryLocations = places;
        }
        else
        {
            Transaction.VehicleDeliveryLocations = VehicleDeliveryLocations;
        }
        Transaction.VehiclePreviewPosition = VehiclePreviewLocation;
        Transaction.IsFreeItems = true;
        Transaction.IsFreeWeapons = true;
        Transaction.IsFreeVehicles = true;
        Transaction.IsPurchasing = false;
        Transaction.RotateVehiclePreview = false;
        Transaction.CreateTransactionMenu(Player, modItems, world, settings, weapons, time);
        InteractionMenu.Visible = true;
        Transaction.ProcessTransactionMenu();
        Transaction.DisposeTransactionMenu();
    }
    private void InteractAsOther()
    {
        UIMenu treatmentOptionsSubMenu = MenuPool.AddSubMenu(InteractionMenu, "Treatment Options");
        treatmentOptionsSubMenu.SubtitleText = "Pick a Treatment";
        InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1].Description = "Pick one of our state of the art treatment options!";
        if (MedicalTreatments != null && MedicalTreatments.Any())
        {
            foreach (MedicalTreatment medicalTreatment in MedicalTreatments)
            {
                medicalTreatment.AddToMenu(this, treatmentOptionsSubMenu,Player);
            }
        }
        InteractionMenu.Visible = true;
        ProcessInteractionMenu();
    }
    public void DisplayInsufficientFundsMessage()
    {
        PlayErrorSound();
        DisplayMessage("~r~Insufficient Funds", "We are sorry, we are unable to complete this transation.");
    }
    public void DisplayPurchaseMessage()
    {
        PlaySuccessSound();
        DisplayMessage("~g~Purchase", $"Thank you for your purchase.");
    }
}

