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

public class GasPump : GameLocation
{
    private UIMenuItem completeTask;
    private Vector3 PropEntryPosition;
    private float PropEntryHeading;
    private bool IsCancelled;
    private string PlayingDict;
    private string PlayingAnim;
    private bool hasAttachedProp;
    //private UIMenu GetGasSubMenu;
    private Rage.Object SellingProp;
   // private VehicleExt VehicleToFill;
   // private UIMenuItem FillMenuItem;
   //private UIMenuNumericScrollerItem<int> AddSomeMenuItem;
    private int pricePerUnit;
    private IGasPumpable AssociatedStation;
    private bool KeepInteractionGoing;
   // private Refueling Refueling;
    private MachineOffsetResult MachineInteraction;
    private MoveInteraction MoveInteraction;
    [XmlIgnore]
    public Rage.Object PumpProp { get; private set; } = null;
    public GasPump() : base()
    {

    }

    public override bool ShowsOnDirectory { get; set; } = false;
    public override bool ShowsOnTaxi { get; set; } = false;
    public override string TypeName { get; set; } = "Gas Pump";
    public override int MapIcon { get; set; } = 361;// (int)BlipSprite.PointOfInterest;
    public override float MapIconScale { get; set; } = 0.25f;
    public override string ButtonPromptText { get; set; }
    [XmlIgnore]
    public bool IsFueling { get; set; } = false;
    public override bool CanCurrentlyInteract(ILocationInteractable player)
    {
        ButtonPromptText = $"Get Gas at {Name}";
        return EntrancePosition != Vector3.Zero || (PumpProp.Exists() && player.CurrentLookedAtObject.Exists() && PumpProp.Handle == player.CurrentLookedAtObject.Handle);
    }
    public GasPump(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description, string menuID, Rage.Object machineProp, IGasPumpable gasStation) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {
        MenuID = menuID;
        PumpProp = machineProp;
        AssociatedStation = gasStation;
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

        if (CanInteract)
        {
            Player.ActivityManager.IsInteractingWithLocation = true;
            CanInteract = false;
            Player.IsTransacting = true;
            IsFueling = false;
            GameFiber.StartNew(delegate
            {
                try
                {
   
                    Vector3 FinalPlayerPos = new Vector3();
                    float FinalPlayerHeading = 0f;
                    if (PumpProp != null && PumpProp.Exists())
                    {
                        MachineOffsetResult machineInteraction = new MachineOffsetResult(Player, PumpProp);
                        machineInteraction.StandingOffsetPosition = 0.5f;
                        machineInteraction.GetPropEntry();
                        FinalPlayerPos = machineInteraction.PropEntryPosition;
                        FinalPlayerHeading = machineInteraction.PropEntryHeading;
                    }
                    else
                    {
                        FinalPlayerPos = EntrancePosition;
                        FinalPlayerHeading = EntranceHeading;
                    }
                    MoveInteraction = new MoveInteraction(Player, FinalPlayerPos, FinalPlayerHeading);
                    if (MoveInteraction.MoveToMachine(1.0f))
                    {
                        CreateInteractionMenu();
                        InteractionMenu.Visible = true;
                        SetupGeneral();
                        while (IsAnyMenuVisible || KeepInteractionGoing || IsFueling)
                        {
                            MenuPool.ProcessMenus();
                            GameFiber.Yield();
                        }
                        DisposeInteractionMenu();
                    }
                    FullDispose();
                    Player.ActivityManager.IsInteractingWithLocation = false;
                    Player.IsTransacting = false;
                    CanInteract = true;
                }
                catch (Exception ex)
                {
                    EntryPoint.WriteToConsole("Location Interaction" + ex.Message + " " + ex.StackTrace, 0);
                    EntryPoint.ModController.CrashUnload();
                }
            }, "Gas Station Interact");
        }
    }
    private void SetupGeneral()
    {
        if (AssociatedStation != null)
        {
            pricePerUnit = AssociatedStation.PricePerGallon;
        }
        else
        {
            pricePerUnit = 3;
        }
        foreach (VehicleExt vehicle in World.Vehicles.AllVehicleList.Where(x=> x.HasBeenEnteredByPlayer && x.Vehicle.Exists() && x.Vehicle.DistanceTo2D(EntrancePosition) <= 8.0f).OrderBy(x=> x.Vehicle.DistanceTo2D(Player.Character)))
        {
            Refueling Refueling = new Refueling(Player, Name, pricePerUnit, vehicle, Settings, this);
            Refueling.Setup();
            GenerateGasMenu(Refueling);
        }
    }
    private void GenerateGasMenu(Refueling Refueling)
    {   
        string MenuString = $"~n~Price Per Gallon: ~r~${pricePerUnit}~s~~n~Fuel Capacity: ~y~{Refueling.VehicleToFillFuelTankCapacity}~s~ Gallons~n~Fuel Needed: ~p~{Refueling.UnitsOfFuelNeeded}~s~ Gallons";
        UIMenu GetGasSubMenu = MenuPool.AddSubMenu(InteractionMenu, $"Gas Up {Refueling.VehicleExt.FullName(true)}");
        UIMenuItem GetGasSubMenuItem = InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1];
        GetGasSubMenuItem.Description = $"Gas Up Your Vehicle";
        GetGasSubMenuItem.RightBadge = UIMenuItem.BadgeStyle.Car;
        if (HasBannerImage)
        {
            BannerImage = Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\{BannerImagePath}");
            GetGasSubMenu.SetBannerType(BannerImage);
        }
        GetGasSubMenu.OnMenuOpen += (sender) =>
        {
            if (Refueling.VehicleExt != null && Refueling.VehicleExt.Vehicle.Exists() && Settings.SettingsManager.PlayerOtherSettings.SetHintCameraWhenUsingMachineInteractions)
            {
                NativeFunction.Natives.SET_GAMEPLAY_VEHICLE_HINT(Refueling.VehicleExt.Vehicle, 0f, 0f, 0f, true, -1, 2000, 2000);
            }
        };
        GetGasSubMenu.OnMenuClose += (sender) =>
        {
            NativeFunction.Natives.STOP_GAMEPLAY_HINT(false);
        };
        UIMenuItem FillMenuItem = new UIMenuItem("Fill", "Fill the entire tank" + MenuString) { RightLabel = Refueling.UnitsOfFuelNeeded + " Gallons - " + Refueling.AmountToFill.ToString("C0") };
        FillMenuItem.Activated += (sender, e) =>
        {
            if (Refueling.VehicleExt != null && Refueling.VehicleExt.Vehicle.Exists())
            {
                IsFueling = true;
                InteractionMenu.Visible = false;
                sender.Visible = false;
                Refueling.RefuelSlow(Refueling.UnitsOfFuelNeeded, this);
                //StartMachineBuyAnimation();
            }
        };
        UIMenuNumericScrollerItem<int> AddSomeMenuItem = new UIMenuNumericScrollerItem<int>("Partial", "Add gasoline by the gallon" + MenuString, 1, Refueling.UnitsOfFuelNeeded, 1) { Formatter = v => v + " Gallons - " + (v * pricePerUnit).ToString("C0") };
        AddSomeMenuItem.Activated += (sender, e) =>
        {
            if (Refueling.VehicleExt != null && Refueling.VehicleExt.Vehicle.Exists())
            {
                IsFueling = true;
                InteractionMenu.Visible = false;
                sender.Visible = false;
                Refueling.RefuelSlow(AddSomeMenuItem.Value, this);
                //StartMachineBuyAnimation();
            }
        };
        if (Refueling.UnitsOfFuelNeeded > 0)
        {
            GetGasSubMenu.AddItem(AddSomeMenuItem);
        }
        GetGasSubMenu.AddItem(FillMenuItem);
        GetGasSubMenuItem.Description = $"Gas Up Your Vehicle.";
        if (!Refueling.CanRefuel)
        {
            AddSomeMenuItem.Enabled = false;
            FillMenuItem.Enabled = false;
            GetGasSubMenuItem.Description = $"~r~{Refueling.GetFuelingFailedReason()}~";
        }
    }
    public override void OnItemPurchased(ModItem modItem, MenuItem menuItem, int totalItems)
    {
        MenuPool.CloseAllMenus();
        StartMachineBuyAnimation();
        base.OnItemPurchased(modItem, menuItem, totalItems);
        Transaction.PurchaseMenu?.Show();
    }
    private void FullDispose()
    {
        if (Settings.SettingsManager.PlayerOtherSettings.SetHintCameraWhenUsingMachineInteractions)
        {
            NativeFunction.Natives.STOP_GAMEPLAY_HINT(false);
        }
        NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
        Game.LocalPlayer.HasControl = true;
        KeepInteractionGoing = false;
        Player.ButtonPrompts.RemovePrompts("Fueling");
        IsFueling = false;
    }
    private void StartMachineBuyAnimation()
    {
        if (!MoveInteraction.MoveToMachine(1.0f))
        {
            FullDispose();
            return;
        }
    }
    public override void AddLocation(PossibleLocations possibleLocations)
    {
        possibleLocations.GasPumps.Add(this);
        base.AddLocation(possibleLocations);
    }
}

