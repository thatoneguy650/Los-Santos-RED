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
    public override int MapIcon { get; set; } = 779;// (int)402;
    public override string ButtonPromptText { get; set; }
   // public float VehiclePickupDistance { get; set; } = 15f;
    public override bool CanInteractWhenWanted { get; set; } = true;

    public List<InteriorDoor> GarageDoors { get; set; }
    public int MaxRepairCost { get; set; } = 10000;
    public int ResprayCost { get; set; } = 1500;
    public int RepairHours { get; set; } = 3;
    public int WashHours { get; set; } = 1;
    public int WashCost { get; set; } = 10;

    public bool HasNoGarageDoors { get; set; } = false;

    public RepairGarage(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {

    }
    public override bool CanCurrentlyInteract(ILocationInteractable player)
    {
        ButtonPromptText = $"Fix Vehicle At {Name}";
        return true;
    }
    public override void OnInteract()
    {
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
        Player.IsTransacting = true;
        GameFiber.StartNew(delegate
        {
            try
            {
                CreateInteractionMenu();
                
                StoreCamera = new LocationCamera(this, Player, Settings, NoEntryCam);
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
                Player.IsTransacting = false;
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
        UIMenuItem repairVehicle = new UIMenuItem("Repair Vehicle", $"Repair the current vehicle.~n~~g~Vehicle Health: {CurrentHealth}/{MaxHealth}") { RightLabel = "~r~" + FinalRepairCost.ToString("C0") + "~s~" };
        repairVehicle.Activated += (sender, e) =>
        {
            DoRepairItems(false);
        };
        UIMenuItem washVehicle = new UIMenuItem("Wash Vehicle", $"Wash the current vehicle.") { RightLabel = "~r~" + WashCost.ToString("C0") + "~s~" };
        washVehicle.Activated += (sender, e) =>
        {
            DoWashItems();
        };
        if (!isFullHealth)
        {
            InteractionMenu.AddItem(repairVehicle);
        }
        else
        {
            InteractionMenu.AddItem(washVehicle);
        }
        UIMenuItem resprayVehicle = new UIMenuItem("Respray Vehicle", $"Repair and respray the current vehicle. Change the color and get a clean plate. Cops won't recognize you.~n~Respray Fee: ~r~{ResprayCost.ToString("C0")}~s~~n~~g~Vehicle Health: {CurrentHealth}/{MaxHealth}") {  RightLabel = "~r~" + (ResprayCost + FinalRepairCost).ToString("C0") + "~s~" };
        resprayVehicle.Activated += (sender, e) =>
        {
            DoRepairItems(true);
        };
        InteractionMenu.AddItem(resprayVehicle);
        InteractionMenu.Visible = true;
    }

    private void WashVehicle()
    {
        if (!Player.IsInVehicle || Player.CurrentVehicle == null || !Player.CurrentVehicle.Vehicle.Exists())
        {
            return;
        }
        Player.CurrentVehicle.Vehicle.Wash();
    }
    private void DoWashItems()
    {
        if (!Player.IsInVehicle || Player.CurrentVehicle == null || !Player.CurrentVehicle.Vehicle.Exists())
        {
            return;
        }
        if (Player.BankAccounts.GetMoney(false) <= WashCost)
        {
            PlayErrorSound();
            DisplayMessage("~r~Cash Only", "You do not have enough cash on hand.");
            return;
        }
        Player.CurrentVehicle.Engine.SetState(false);
        GameFiber.Sleep(500);
        Time.FastForward(Time.CurrentDateTime.AddHours(WashHours));
        Time.ForceShowClock = true;
        WashVehicle();
        while (Time.IsFastForwarding)
        {
            GameFiber.Yield();
        }
        Time.ForceShowClock = false;
        Player.BankAccounts.GiveMoney(-1 * WashCost, false);
        PlaySuccessSound();
        DisplayMessage("~g~Washed", $"Thank you for washing your vehicle at ~y~{Name}~s~");      
        InteractionMenu.Visible = false;
    }
    private void DoRepairItems(bool withRespray)
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
        if (Player.BankAccounts.GetMoney(false) <= totalRepairCost)
        {
            PlayErrorSound();
            DisplayMessage("~r~Cash Only", "You do not have enough cash on hand.");
            return;
        }
        Player.CurrentVehicle.Engine.SetState(false);
        GameFiber.Sleep(500);
        Time.FastForward(Time.CurrentDateTime.AddHours(RepairHours));
        Time.ForceShowClock = true;
        DateTime timestartedRepair = Time.CurrentDateTime;
        DateTime timeToDoItems = Time.CurrentDateTime.AddHours(1);
        bool hasFixedVehicle = false;
        while (Time.IsFastForwarding)
        {
            if(!hasFixedVehicle && DateTime.Compare(Time.CurrentDateTime, timeToDoItems) >= 0)
            {
                RepairVehicle();
                if (withRespray)
                {
                    ResprayVehicle();
                    RemoveWanted();
                }
                hasFixedVehicle = true;
            }
            GameFiber.Yield();
        }
        Time.ForceShowClock = false;
        Player.BankAccounts.GiveMoney(-1 * totalRepairCost, false);
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
    private void RepairVehicle()
    {
        if (!Player.IsInVehicle || Player.CurrentVehicle == null || !Player.CurrentVehicle.Vehicle.Exists())
        {
            return;
        }

        float OldFuel = Player.CurrentVehicle.Vehicle.FuelLevel;

        Player.CurrentVehicle.Vehicle.Repair();
        Player.CurrentVehicle.Vehicle.Wash();

        if (Settings.SettingsManager.VehicleSettings.RefuelVehicleAfterPayNSprayRepair)
        {
            Player.CurrentVehicle.Vehicle.FuelLevel = Settings.SettingsManager.VehicleSettings.CustomFuelSystemFuelMax;
        }
        else
        {
            Player.CurrentVehicle.Vehicle.FuelLevel = OldFuel;
        }



    }
    private void RemoveWanted()
    {
        if (Player.IsWanted)
        {
            Player.SetWantedLevel(0, "Resprayed", true);
        }
        if (Player.Investigation.IsActive)
        {
            Player.Investigation.Expire();
        }
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
        possibleLocations.RepairGarages.Add(this);
        base.AddLocation(possibleLocations);
    }
}

