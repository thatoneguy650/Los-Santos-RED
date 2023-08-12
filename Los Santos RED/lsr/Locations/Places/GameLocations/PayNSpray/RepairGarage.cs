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

public class RepairGarage : GameLocation
{
    private UIMenu RepairGarageSubMenu;
    private int FinalRepairCost;
    public RepairGarage() : base()
    {

    }
    public override string TypeName { get; set; } = "Garage";
    public override int MapIcon { get; set; } = (int)402;
    public override float MapIconScale { get; set; } = 1.0f;
    public override string ButtonPromptText { get; set; }
   // public float VehiclePickupDistance { get; set; } = 15f;
    public override bool CanInteractWhenWanted { get; set; } = true;

    public List<InteriorDoor> GarageDoors { get; set; }
    public int MaxRepairCost { get; set; } = 10000;
    public int ResprayCost { get; set; } = 1500;
    public int RepairHours { get; set; } = 3;


    public RepairGarage(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {

    }
    public override bool CanCurrentlyInteract(ILocationInteractable player)
    {
        ButtonPromptText = $"Fix Vehicle At {Name}";
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
        if(!Player.IsInVehicle)
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

        GameFiber.StartNew(delegate
        {
            try
            {
                CreateInteractionMenu();
                
                StoreCamera = new LocationCamera(this, Player, Settings);
                StoreCamera.StaysInVehicle = true;
                StoreCamera.Setup();
                HandleDoor();
                GeneratePayNSprayMenu();
                ProcessInteractionMenu();
                DisposeInteractionMenu();
                DisposeDoor();
                StoreCamera.Dispose();
                Player.ActivityManager.IsInteractingWithLocation = false;
                CanInteract = true;
            }
            catch (Exception ex)
            {
                EntryPoint.WriteToConsole("Location Interaction" + ex.Message + " " + ex.StackTrace, 0);
                EntryPoint.ModController.CrashUnload();
            }
        }, "PayNSprayInteract");
        
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
    private void GeneratePayNSprayMenu()
    {   
        if (!Player.IsInVehicle || Player.CurrentVehicle == null || !Player.CurrentVehicle.Vehicle.Exists())
        {
            return;
        }
        FinalRepairCost = MaxRepairCost;
        int CurrentHealth = Player.CurrentVehicle.Vehicle.Health;
        int MaxHealth = Player.CurrentVehicle.Vehicle.MaxHealth;
        float healthPercentage = (float)CurrentHealth / (float)MaxHealth;
        bool isFullHealth = CurrentHealth == MaxHealth;
        FinalRepairCost = (int)Math.Ceiling((1.0f - healthPercentage) * MaxRepairCost);
        FinalRepairCost.Round(100);
        UIMenuItem repairVehicle = new UIMenuItem("Repair Vehicle", $"Repair the current vehicle.~n~~g~Vehicle Health: {CurrentHealth}/{MaxHealth}") { RightLabel = FinalRepairCost.ToString("C0") };
        repairVehicle.Activated += (sender, e) =>
        {
            RepairVehicle(false);
        };
        if (!isFullHealth)
        {
            InteractionMenu.AddItem(repairVehicle);
        }
        UIMenuItem resprayVehicle = new UIMenuItem("Respray Vehicle", $"Repair and respray the current vehicle. Change the color and get a clean plate. Cops won't recognize you.~n~Respray Fee: ~r~{ResprayCost.ToString("C0")}~s~~n~~g~Vehicle Health: {CurrentHealth}/{MaxHealth}") { RightLabel = (ResprayCost + FinalRepairCost).ToString("C0") };
        resprayVehicle.Activated += (sender, e) =>
        {
            RepairVehicle(true);
        };
        InteractionMenu.AddItem(resprayVehicle);
        InteractionMenu.Visible = true;
    }
    private void RepairVehicle(bool withRespray)
    {
        int totalRepairCost = FinalRepairCost;
        if(withRespray)
        {
            totalRepairCost += ResprayCost;
        }
        if (!Player.IsInVehicle || Player.CurrentVehicle == null || !Player.CurrentVehicle.Vehicle.Exists())
        {
            return;
        }
        if (Player.BankAccounts.Money <= totalRepairCost)
        {
            PlayErrorSound();
            DisplayMessage("~r~Repair Failed", "Insufficient funds!");
            return;
        }   
        GameFiber.Sleep(500);
        Time.FastForward(Time.CurrentDateTime.AddHours(RepairHours));
        Time.ForceShowClock = true;
        DateTime timestartedRepair = Time.CurrentDateTime;
        DateTime timeToDoItems = Time.CurrentDateTime.AddHours(1);
        bool hasFixedVehicle = false;
        while (Time.IsFastForwarding)
        {
            if(!hasFixedVehicle && DateTime.Compare(timeToDoItems, timestartedRepair) >= 0)
            {
                Player.CurrentVehicle.Vehicle.Repair();
                Player.CurrentVehicle.Vehicle.Wash();
                if (withRespray)
                {
                    ResprayVehicle();
                    if (Player.IsWanted)
                    {
                        Player.SetWantedLevel(0, "Resprayed", true);
                    }
                    if (Player.Investigation.IsActive)
                    {
                        Player.Investigation.Expire();
                    }
                }
                hasFixedVehicle = true;
            }
            GameFiber.Yield();
        }
        Time.ForceShowClock = false;
        Player.BankAccounts.GiveMoney(-1 * totalRepairCost);
        PlaySuccessSound();
        if(withRespray)
        {
            DisplayMessage("~g~Resprayed", $"Thank you for respraying your vehicle at ~y~{Name}~s~");
        }
        else
        {
            DisplayMessage("~g~Repaired", $"Thank you for fixing your vehicle at ~y~{Name}~s~");
        }
        InteractionMenu.Visible = false;
    }
    private void ResprayVehicle()
    {
        if (!Player.IsInVehicle || Player.CurrentVehicle == null || !Player.CurrentVehicle.Vehicle.Exists())
        {
            return;
        }
        Player.CurrentVehicle.SetRandomColor();
        Player.CurrentVehicle.SetRandomPlate();
    }
    public override void Activate(IInteriors interiors, ISettingsProvideable settings, ICrimes crimes, IWeapons weapons, ITimeReportable time, IEntityProvideable world)
    {
        if (GarageDoors == null || !GarageDoors.Any())
        {
            return;
        }
        foreach (InteriorDoor id in GarageDoors)
        {
            if (id.Position != Vector3.Zero)
            {
                id.Activate();
            }
        }
        base.Activate(interiors, settings, crimes, weapons, time, world);
    }
}

