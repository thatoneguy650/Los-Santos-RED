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
                CreateInteractionMenu();
                SetupOrbitCamera();
                HandleDoor();
                GenerateModMenu();
                ProcessMenu();
                DisposeInteractionMenu();
                DisposeDoor();
                OrbitCamera.Dispose();
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
        OrbitCamera = new OrbitCamera(Player.CurrentVehicle.Vehicle, null, Settings, MenuPool);
        OrbitCamera.Setup();
    }

    private void ProcessMenu()
    {
        while (IsAnyMenuVisible)
        {
            GameFiber.Yield();
        }
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
    private void GenerateModMenu()
    {
        if (!Player.IsInVehicle || Player.CurrentVehicle == null || !Player.CurrentVehicle.Vehicle.Exists())
        {
            return;
        }
        EntryPoint.WriteToConsole("ORBIT CAMERA START");
        ModShopMenu modShopMenu = new ModShopMenu(Player, MenuPool, InteractionMenu, this, MaxRepairCost, RepairHours, WashCost, WashHours, null);
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
            foreach (InteriorDoor id in GarageDoors)
            {
                if (id.Position != Vector3.Zero)
                {
                    id.Activate();
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

