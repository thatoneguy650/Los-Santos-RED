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
    private UIMenu GetGasSubMenu;
    private Rage.Object SellingProp;
    private VehicleExt VehicleToFill;
    private UIMenuItem FillMenuItem;
    private UIMenuNumericScrollerItem<int> AddSomeMenuItem;
    private int pricePerUnit;
    private GasStation AssociatedStation;
    private bool KeepInteractionGoing;
    private Refueling Refueling;

    public Rage.Object PumpProp { get; private set; } = null;
    public GasPump() : base()
    {

    }

    public override bool ShowsOnDirectory { get; set; } = false;
    public override string TypeName { get; set; } = "Gas Pump";
    public override int MapIcon { get; set; } = (int)BlipSprite.PointOfInterest;
    public override float MapIconScale { get; set; } = 0.5f;
    public override string ButtonPromptText { get; set; }
    public bool IsFueling { get; set; } = false;
    public override bool CanCurrentlyInteract(ILocationInteractable player)
    {
        ButtonPromptText = $"Get Gas at {Name}";
        return PumpProp.Exists() && player.CurrentLookedAtObject.Exists() && PumpProp.Handle == player.CurrentLookedAtObject.Handle;
    }
    public GasPump(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description, string menuID, Rage.Object machineProp, GasStation gasStation) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {
        MenuID = menuID;
        PumpProp = machineProp;
        AssociatedStation = gasStation;
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
            Player.IsTransacting = true;
            IsFueling = false;
            GameFiber.StartNew(delegate
            {
                try
                {
                    GetPropEntry();
                    if (!MoveToMachine())
                    {
                        FullDispose();
                    }
                    CreateInteractionMenu();
                    InteractionMenu.Visible = true;
                    InteractionMenu.OnItemSelect += InteractionMenu_OnItemSelect;
                    SetupGeneral();
                    while (IsAnyMenuVisible || KeepInteractionGoing || IsFueling)
                    {
                        MenuPool.ProcessMenus();
                        GameFiber.Yield();
                    }
                    DisposeInteractionMenu();
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
        VehicleToFill = World.Vehicles.GetClosestVehicleExt(EntrancePosition, true, 6f);





        if (AssociatedStation != null)
        {
            pricePerUnit = AssociatedStation.PricePerGallon;
        }
        else
        {
            pricePerUnit = 3;
        }
        Refueling = new Refueling(Player, Name, pricePerUnit, VehicleToFill, Settings, this);
        Refueling.Setup();
        GenerateGasMenu();
    }
    private void GenerateGasMenu()
    {   
        if (Refueling.CanRefuel)
        {
            string MenuString = $"~n~Price Per Gallon: ~r~${pricePerUnit}~s~~n~Fuel Capacity: ~y~{Refueling.VehicleToFillFuelTankCapacity}~s~ Gallons~n~Fuel Needed: ~p~{Refueling.UnitsOfFuelNeeded}~s~ Gallons";
            GetGasSubMenu = MenuPool.AddSubMenu(InteractionMenu, $"Gas Up {VehicleToFill.FullName(false)}");
            InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1].Description = $"Gas Up Your Vehicle";
            InteractionMenu.MenuItems[InteractionMenu.MenuItems.Count() - 1].RightBadge = UIMenuItem.BadgeStyle.Car;
            if (HasBannerImage)
            {
                BannerImage = Game.CreateTextureFromFile($"Plugins\\LosSantosRED\\images\\{BannerImagePath}");
                GetGasSubMenu.SetBannerType(BannerImage);
            }
            GetGasSubMenu.OnItemSelect += InteractionMenu_OnItemSelect;
            GetGasSubMenu.OnMenuOpen += GetGasSubMenu_OnMenuOpen;
            GetGasSubMenu.OnMenuClose += GetGasSubMenu_OnMenuClose;
            FillMenuItem = new UIMenuItem("Fill", "Fill the entire tank" + MenuString) { RightLabel = Refueling.UnitsOfFuelNeeded + " Gallons - " + Refueling.AmountToFill.ToString("C0") };
            if (Refueling.UnitsOfFuelNeeded > 0)
            {
                AddSomeMenuItem = new UIMenuNumericScrollerItem<int>("Partial", "Add gasoline by the gallon" + MenuString, 1, Refueling.UnitsOfFuelNeeded, 1) { Formatter = v => v + " Gallons - " + (v * pricePerUnit).ToString("C0") };
                GetGasSubMenu.AddItem(AddSomeMenuItem);
            }
            GetGasSubMenu.AddItem(FillMenuItem);            
        }
        else
        {
            Refueling.DisplayFuelingFailedReason();
            InteractionMenu.Visible = false;
        }
    }
    private void GetGasSubMenu_OnMenuOpen(UIMenu sender)
    {
        if(VehicleToFill != null && VehicleToFill.Vehicle.Exists())
        {
            if (VehicleToFill != null && VehicleToFill.Vehicle.Exists())
            {
                NativeFunction.Natives.SET_GAMEPLAY_VEHICLE_HINT(VehicleToFill.Vehicle, 0f, 0f, 0f, true, -1, 2000, 2000);
            }
        }
    }
    private void GetGasSubMenu_OnMenuClose(UIMenu sender)
    {

    }
    public override void OnItemPurchased(ModItem modItem, MenuItem menuItem, int totalItems)
    {
        MenuPool.CloseAllMenus();
        StartMachineBuyAnimation();
        base.OnItemPurchased(modItem, menuItem, totalItems);
        Transaction.PurchaseMenu?.Show();
    }
    private void InteractionMenu_OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if(selectedItem == FillMenuItem && VehicleToFill != null && VehicleToFill.Vehicle.Exists())
        {
            //KeepInteractionGoing = true;
            IsFueling = true;
            InteractionMenu.Visible = false;
            sender.Visible = false;
            Refueling.RefuelSlow(Refueling.UnitsOfFuelNeeded, this);
        }
        else if (selectedItem == AddSomeMenuItem && VehicleToFill != null && VehicleToFill.Vehicle.Exists())
        {
            //KeepInteractionGoing = true;
            IsFueling = true;
            InteractionMenu.Visible = false;
            sender.Visible = false;
            Refueling.RefuelSlow(AddSomeMenuItem.Value, this);
        }
    }
    private void FullDispose()
    {
        NativeFunction.Natives.STOP_GAMEPLAY_HINT(false);
        NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
        Game.LocalPlayer.HasControl = true;
        KeepInteractionGoing = false;
        Player.ButtonPrompts.RemovePrompts("Fueling");
        IsFueling = false;
    }
    private void GetPropEntry()
    {
        if (PumpProp != null && PumpProp.Exists())
        {
            float DistanceToFront = Player.Position.DistanceTo2D(PumpProp.GetOffsetPositionFront(-1f));
            float DistanceToRear = Player.Position.DistanceTo2D(PumpProp.GetOffsetPositionFront(1f));
            if (DistanceToFront <= DistanceToRear)
            {
                PropEntryPosition = PumpProp.GetOffsetPositionFront(-1f);
                PropEntryPosition = new Vector3(PropEntryPosition.X, PropEntryPosition.Y, Game.LocalPlayer.Character.Position.Z);
                float ObjectHeading = PumpProp.Heading - 180f;
                if (ObjectHeading >= 180f)
                {
                    PropEntryHeading = ObjectHeading - 180f;
                }
                else
                {
                    PropEntryHeading = ObjectHeading + 180f;
                }
            }
            else
            {
                //EntryPoint.WriteToConsoleTestLong("Gas Pump You are Closer to the REAR, using that side");
                PropEntryPosition = PumpProp.GetOffsetPositionFront(1f);
                PropEntryPosition = new Vector3(PropEntryPosition.X, PropEntryPosition.Y, Game.LocalPlayer.Character.Position.Z);
                float ObjectHeading = PumpProp.Heading;
                if (ObjectHeading >= 180f)
                {
                    PropEntryHeading = ObjectHeading - 180f;
                }
                else
                {
                    PropEntryHeading = ObjectHeading + 180f;
                }
            }
        }
    }
    private bool MoveToMachine()
    {
        if (PropEntryPosition == Vector3.Zero)
        {
            return false;
        }
        NativeFunction.Natives.TASK_GO_STRAIGHT_TO_COORD(Game.LocalPlayer.Character, PropEntryPosition.X, PropEntryPosition.Y, PropEntryPosition.Z, 1.0f, -1, PropEntryHeading, 0.2f);
        uint GameTimeStartedSitting = Game.GameTime;
        float heading = Game.LocalPlayer.Character.Heading;
        bool IsFacingDirection = false;
        bool IsCloseEnough = false;
        while (Game.GameTime - GameTimeStartedSitting <= 5000 && !IsCloseEnough && !IsCancelled)
        {
            if (Player.IsMoveControlPressed)
            {
                IsCancelled = true;
            }
            IsCloseEnough = Game.LocalPlayer.Character.DistanceTo2D(PropEntryPosition) < 0.2f;
            GameFiber.Yield();
        }
        GameFiber.Sleep(250);
        GameTimeStartedSitting = Game.GameTime;
        while (Game.GameTime - GameTimeStartedSitting <= 5000 && !IsFacingDirection && !IsCancelled)
        {
            if (Player.IsMoveControlPressed)
            {
                IsCancelled = true;
            }
            heading = Game.LocalPlayer.Character.Heading;
            if (Math.Abs(ExtensionsMethods.Extensions.GetHeadingDifference(heading, PropEntryHeading)) <= 0.5f)//0.5f)
            {
                IsFacingDirection = true;
            }
            GameFiber.Yield();
        }
        GameFiber.Sleep(250);
        if (IsCloseEnough && IsFacingDirection && !IsCancelled)
        {
            return true;
        }
        else
        {
            NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
            return false;
        }
    }
    private void StartMachineBuyAnimation()
    {
        if (!MoveToMachine())
        {
            FullDispose();
        }
    }
}

