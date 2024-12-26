using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
using Microsoft.VisualBasic;
using Mod;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Runtime;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

public class PoliceStation : GameLocation, ILocationRespawnable, ILicensePlatePreviewable, ILocationImpoundable, ILocationAreaRestrictable, IAssaultSpawnable, ILEDispatchableLocation
{
    private ShopMenu agencyMenu;
    private UIMenu ImpoundSubMenu;
    public PoliceStation(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {

    }
    public PoliceStation() : base()
    {

    }
    public string LicensePlatePreviewText { get; set; } = "UNIT1";
    public override string TypeName { get; set; } = "Police Station";
    public override int MapIcon { get; set; } = (int)BlipSprite.PoliceStation;
    public Vector3 RespawnLocation { get; set; }
    public float RespawnHeading { get; set; }
    public VehicleImpoundLot VehicleImpoundLot { get; set; }
    public bool HasImpoundLot => VehicleImpoundLot != null;
    public int MaxAssaultSpawns { get; set; } = 15;
    public List<SpawnPlace> AssaultSpawnLocations { get; set; }
    public bool RestrictAssaultSpawningUsingPedSpawns { get; set; } = false;
    public float AssaultSpawnHeavyWeaponsPercent { get; set; } = 80f;
    public override bool CanCurrentlyInteract(ILocationInteractable player)
    {
        ButtonPromptText = $"Enter {Name}";
        return true;
    }
    public override void StoreData(IShopMenus shopMenus, IAgencies agencies, IGangs gangs, IZones zones, IJurisdictions jurisdictions, IGangTerritories gangTerritories, INameProvideable Names, ICrimes Crimes, IPedGroups PedGroups, IEntityProvideable world, 
        IStreets streets, ILocationTypes locationTypes, ISettingsProvideable settings, IPlateTypes plateTypes, IOrganizations associations, IContacts contacts, IInteriors interiors,
        ILocationInteractable player, IModItems modItems, IWeapons weapons, ITimeControllable time, IPlacesOfInterest placesOfInterest, IIssuableWeapons issuableWeapons, IHeads heads, IDispatchablePeople dispatchablePeople)
    {
        base.StoreData(shopMenus, agencies, gangs, zones, jurisdictions, gangTerritories, Names, Crimes, PedGroups, world, streets,locationTypes, settings, plateTypes, associations, contacts, interiors, player, modItems, weapons, time, placesOfInterest, issuableWeapons, heads, dispatchablePeople);
        VehicleImpoundLot?.Setup(this);
        if (AssignedAgency == null)
        {
            Zone assignedZone = zones.GetZone(EntrancePosition);
            if (assignedZone != null)
            {
                AssignedAgency = assignedZone.AssignedLEAgency;
            }
            if (AssignedAgency == null && assignedZone != null)
            {
                EntryPoint.WriteToConsole("POLICE STATION FALLBACK TO COUNTY AGENCY");
                AssignedAgency = jurisdictions.GetRespondingAgency(null, assignedZone.CountyID, ResponseType.LawEnforcement);
            }
        }
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
        if (!CanInteract)
        {
            return;        
        }
        if (AssignedAgency == null)
        {
            Game.DisplayHelp("No Agency Assigned");
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
                if (Player.IsCop)
                {
                    InteractAsCop(ModItems, World, Settings, Weapons, Time);
                }
                else
                {
                    InteractAsOther();
                }
                DisposeInteractionMenu();
                DisposeCamera(isInside);
                DisposeInterior();
                Player.ActivityManager.IsInteractingWithLocation = false;
                Player.IsTransacting = false;
                CanInteract = true;
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole("Location Interaction" + ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "PoliceStationInteract");
    }

    private void InteractAsCop(IModItems modItems, IEntityProvideable world, ISettingsProvideable settings, IWeapons weapons, ITimeControllable time)
    {
        agencyMenu = AssignedAgency.GenerateMenu(ModItems);
        Transaction = new Transaction(MenuPool, InteractionMenu, agencyMenu, this);
        Transaction.LicensePlatePreviewable = this;
        if((VehicleDeliveryLocations == null || !VehicleDeliveryLocations.Any()) && PossibleVehicleSpawns?.Any() == true)
        {
            List<SpawnPlace> places = new List<SpawnPlace>();
            foreach(ConditionalLocation place in PossibleVehicleSpawns)
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
        UIMenuItem addComplaintMenu = new UIMenuItem("File Complaint","File a complaint about the conduct of the officers. After all, you pay their salary!");
        addComplaintMenu.Activated += (sender,selectedItem) =>
        {      
            InteractionMenu.Visible = false;
            Game.DisplaySubtitle("Go Fuck Yourself Prick.");
            Player.SetAngeredCop();
        };
        InteractionMenu.AddItem(addComplaintMenu);
        UIMenuItem PayBailFees = new UIMenuItem("Pay Bail Fees", "Pay your outstanding bail fees.") { RightLabel = $"${Player.Respawning.PastDueBailFees}" };
        PayBailFees.Activated += (sender, selectedItem) =>
        {
            if (Player.BankAccounts.GetMoney(true) <= Player.Respawning.PastDueBailFees)
            {
                new GTANotification(Name, "~r~Insufficient Funds", "We are sorry, we are unable to complete this transaction.").Display();
                NativeHelper.PlayErrorSound();
                return;
            }
            Player.BankAccounts.GiveMoney(-1 * Player.Respawning.PastDueBailFees, true);
            new GTANotification(Name, "~g~Accepted", $"Your bail fees have been paid.").Display();
            Player.Respawning.PayPastDueBail();
            PayBailFees.Enabled = Player.Respawning.PastDueBailFees > 0;
        };
        PayBailFees.Enabled = Player.Respawning.PastDueBailFees > 0;
        InteractionMenu.AddItem(PayBailFees);

        List<VehicleExt> ImpoundedVehicles = Player.VehicleOwnership.OwnedVehicles.Where(x => x.IsImpounded && x.Vehicle.Exists() && x.ImpoundedLocation == Name).ToList();// x.Vehicle.DistanceTo2D(EntrancePosition) <= 300f).ToList();
        if (HasImpoundLot && ImpoundedVehicles.Any())
        {       
            ImpoundSubMenu = MenuPool.AddSubMenu(InteractionMenu, "Impounded Vehicles");
            if (HasBannerImage)
            {
                ImpoundSubMenu.SetBannerType(BannerImage);
            }
            foreach (VehicleExt impoundedVehicle in ImpoundedVehicles)
            {
                EntryPoint.WriteToConsole("ADDING VEHICLE TO IMPOUND MENU");
                impoundedVehicle.AddToImpoundMenu(this,ImpoundSubMenu, Player, Time);
            }
        }  
        InteractionMenu.Visible = true;
        ProcessInteractionMenu();
    }
    public override void AddDistanceOffset(Vector3 offsetToAdd)
    {
        if (RespawnLocation != Vector3.Zero)
        {
            RespawnLocation += offsetToAdd;
        }
        VehicleImpoundLot?.AddDistanceOffset(offsetToAdd);
        VehiclePreviewLocation?.AddDistanceOffset(offsetToAdd);
        base.AddDistanceOffset(offsetToAdd);
    }


    [OnDeserialized()]
    private void SetValuesOnDeserialized(StreamingContext context)
    {
        MaxAssaultSpawns = 15;
        AssaultSpawnHeavyWeaponsPercent = 80f;
    }
    public override void AddLocation(PossibleLocations possibleLocations)
    {
        possibleLocations.PoliceStations.Add(this);
        base.AddLocation(possibleLocations);
    }

}

