using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using LSR.Vehicles;
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

public class VehicleModShop : GameLocation
{
    private UIMenu RepairGarageSubMenu;
    private int FinalRepairCost;
    private OrbitCamera OrbitCamera;
    private bool hasteleported;

    public VehicleModShop() : base()
    {

    }
    public override string TypeName { get; set; } = "Mod Shop";
    public override int MapIcon { get; set; } = 779;// (int)402;
    public override string ButtonPromptText { get; set; }
    // public float VehiclePickupDistance { get; set; } = 15f;
    public override bool CanInteractWhenWanted { get; set; } = true;

    public List<InteriorDoor> GarageDoors { get; set; }
    public int MaxRepairCost { get; set; } = 10000;
    public int RepairHours { get; set; } = 3;
    public int WashHours { get; set; } = 1;
    public int WashCost { get; set; } = 10;

    public bool HasNoGarageDoors { get; set; } = false;


    public int DefaultPrice { get; set; } = 500;
    public int DefaultPriceScalar { get; set; } = 100;

    public string VehicleVariationShopMenuID { get; set; } = "GenericModShop";
    [XmlIgnore]
    public VehicleVariationShopMenu VehicleVariationShopMenu { get; set; }

    public VehicleModShop(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {

    }
    public override bool CanCurrentlyInteract(ILocationInteractable player)
    {
        ButtonPromptText = $"Mod Vehicle At {Name}";
        return true;
    }
    public override void OnInteract()
    {
        if (IsLocationClosed())
        {
            return;
        }
        if (!Player.IsInVehicle)
        {
            Game.DisplayHelp("Come Back With A Vehicle");
            return;
        }
        if (!CanInteract)
        {
            return;
        }
        Player.ActivityManager.IsInteractingWithLocation = true;
        CanInteract = false;
        Player.IsTransacting = true;
        Player.IsSetDisabledControls = true;
        GameFiber.StartNew(delegate
        {
            try
            {
                if(VehiclePreviewLocation != null)
                {
                    Game.FadeScreenOut(500, true);
                    Player.GPSManager.TeleportToCoords(VehiclePreviewLocation.Position, VehiclePreviewLocation.Heading, false,true,0);
                    hasteleported = true;
                    GameFiber.Sleep(500);
                    
                }
                Player.CurrentVehicle?.Radio.SetOff();

                CreateInteractionMenu();
                SetupOrbitCamera();

                if(hasteleported)
                {
                    Game.FadeScreenIn(500, true);
                }

                HandleDoor();
                GenerateModMenu();
                ProcessMenu();
                DisposeInteractionMenu();
                DisposeDoor();


                if (VehiclePreviewLocation != null && hasteleported)
                {
                    Game.FadeScreenOut(500, true);
                    Player.GPSManager.TeleportToCoords(EntrancePosition, EntranceHeading, false, true, 0);
                    OrbitCamera.Dispose();
                    GameFiber.Sleep(500);         
                    Game.FadeScreenIn(500, true);
                }
                else
                {
                    OrbitCamera.Dispose();
                }

                





                Player.ActivityManager.IsInteractingWithLocation = false;
                CanInteract = true;
                Player.IsTransacting = false;
                Player.IsSetDisabledControls = false;
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole("Location Interaction" + ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "ModShopInteract");

    }

    private void SetupOrbitCamera()
    {
        OrbitCamera = new OrbitCamera(Player,Player.CurrentVehicle.Vehicle, null, Settings, MenuPool);
        OrbitCamera.HandleUpdates = true;
        OrbitCamera.Setup();
    }

    private void ProcessMenu()
    {
        while (IsAnyMenuVisible)
        {

            if (Game.IsKeyDownRightNow(System.Windows.Forms.Keys.LShiftKey) && Game.IsKeyDownRightNow(System.Windows.Forms.Keys.Z))
            {

                EntryPoint.WriteToConsole("Z KEY HIT EXITING DEBUG");
                break;
            }

            //if (Game.IsKeyDownRightNow(System.Windows.Forms.Keys.T))
            //{

            //    EntryPoint.WriteToConsole($"{InteractionMenu.Visible}");
            //    InteractionMenu.Visible = true;
            //}

            GameFiber.Yield();
        }
        EntryPoint.WriteToConsole("BREAK HAPPENED");
    }
    private void HandleDoor()
    {
        if (GarageDoors == null)
        {
            return;
        }
        foreach (InteriorDoor id in GarageDoors)
        {
            if (id.Position != Vector3.Zero)
            {
                id.LockDoor();
            }
        }
        GameFiber.Sleep(1000);
    }
    private void DisposeDoor()
    {
        if (GarageDoors == null)
        {
            return;
        }
        foreach (InteriorDoor id in GarageDoors)
        {
            if (id.Position != Vector3.Zero)
            {
                id.UnLockDoor();
            }
        }
        GameFiber.Sleep(1000);
    }

    public override void StoreData(IShopMenus shopMenus, IAgencies agencies, IGangs gangs, IZones zones, IJurisdictions jurisdictions, IGangTerritories gangTerritories, INameProvideable names, ICrimes crimes, IPedGroups PedGroups, IEntityProvideable world, IStreets streets, ILocationTypes locationTypes, ISettingsProvideable settings, IPlateTypes plateTypes, IOrganizations associations, IContacts contacts, IInteriors interiors, ILocationInteractable player, IModItems modItems, IWeapons weapons, ITimeControllable time, IPlacesOfInterest placesOfInterest, IIssuableWeapons issuableWeapons, IHeads heads, IDispatchablePeople dispatchablePeople, ModDataFileManager modDataFileManager)
    {
        VehicleVariationShopMenu = shopMenus.GetVehicleVariationMenu(VehicleVariationShopMenuID);
        base.StoreData(shopMenus, agencies, gangs, zones, jurisdictions, gangTerritories, names, crimes, PedGroups, world, streets, locationTypes, settings, plateTypes, associations, contacts, interiors, player, modItems, weapons, time, placesOfInterest, issuableWeapons, heads, dispatchablePeople, modDataFileManager);
    }
    private void GenerateModMenu()
    {
        if (!Player.IsInVehicle || Player.CurrentVehicle == null || !Player.CurrentVehicle.Vehicle.Exists())
        {
            return;
        }
        EntryPoint.WriteToConsole("ORBIT CAMERA START");
        
        ModShopMenu modShopMenu = new ModShopMenu(Player, MenuPool, InteractionMenu, this, VehicleVariationShopMenu, MaxRepairCost, RepairHours, WashCost, WashHours, null, DefaultPrice, DefaultPriceScalar, PlateTypes);
        modShopMenu.CreateMenu();
    }
    public override void Activate(IInteriors interiors, ISettingsProvideable settings, ICrimes crimes, IWeapons weapons, ITimeReportable time, IEntityProvideable world)
    {
        if (!HasNoGarageDoors && (GarageDoors == null || !GarageDoors.Any()))
        {
            return;
        }
        if (IsOpen(time.CurrentHour))
        {
            if (GarageDoors != null)
            {
                foreach (InteriorDoor id in GarageDoors)
                {
                    if (id.Position != Vector3.Zero)
                    {
                        id.Activate();
                    }
                }
            }
        }
        base.Activate(interiors, settings, crimes, weapons, time, world);
    }
    public override void AddDistanceOffset(Vector3 offsetToAdd)
    {
        if (GarageDoors != null)
        {
            foreach (InteriorDoor id in GarageDoors)
            {
                id.AddDistanceOffset(offsetToAdd);
            }
        }
        base.AddDistanceOffset(offsetToAdd);
    }
    public override void AddLocation(PossibleLocations possibleLocations)
    {
        possibleLocations.VehicleModShops.Add(this);
        base.AddLocation(possibleLocations);
    }
}

