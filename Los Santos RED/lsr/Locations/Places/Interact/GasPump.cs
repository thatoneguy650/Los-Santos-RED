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

public class GasPump : InteractableLocation
{
    private LocationCamera StoreCamera;
    private ILocationInteractable Player;
    private IModItems ModItems;
    private IEntityProvideable World;
    private ISettingsProvideable Settings;
    private IWeapons Weapons;
    private ITimeControllable Time;
    private UIMenuItem completeTask;
    private Transaction Transaction;
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
    private float PercentFuelNeeded;
    private int VehicleToFillFuelTankCapacity;
    private int UnitsOfFuelNeeded;
    private float PercentFilledPerUnit;
    private float AmountToFill;
    private string VehicleToFillMakeName;
    private string VehicleToFillModelName;
    private string VehicleToFillClassName;
    private string VehicleToFillName;
    private GasStation AssociatedStation;
    private bool KeepInteractionGoing;
    private bool IsFueling;
    public Rage.Object PumpProp { get; private set; } = null;
    public GasPump() : base()
    {

    }

    public override bool ShowsOnDirectory { get; set; } = false;
    public override string TypeName { get; set; } = "Gas Pump";
    public override int MapIcon { get; set; } = (int)BlipSprite.PointOfInterest;
    public override Color MapIconColor { get; set; } = Color.White;
    public override float MapIconScale { get; set; } = 0.5f;
    public override string ButtonPromptText { get; set; }
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
    public override void OnInteract(ILocationInteractable player, IModItems modItems, IEntityProvideable world, ISettingsProvideable settings, IWeapons weapons, ITimeControllable time)
    {
        Player = player;
        ModItems = modItems;
        World = world;
        Settings = settings;
        Weapons = weapons;
        Time = time;

        if (CanInteract)
        {
            Player.ActivityManager.IsInteractingWithLocation = true;
            CanInteract = false;
            Player.IsTransacting = true;
            GameFiber.StartNew(delegate
            {
                GetPropEntry();
                if (!MoveToMachine())
                {
                    FullDispose();
                }
                CreateInteractionMenu();
                InteractionMenu.Visible = true;
                InteractionMenu.OnItemSelect += InteractionMenu_OnItemSelect;
                GenerateGasMenu();
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
            }, "Gas Station Interact");
        }
    }
    private void GenerateGasMenu()
    {
        VehicleToFill = World.Vehicles.GetClosestVehicleExt(EntrancePosition, true, 6f);
        if (VehicleToFill != null && VehicleToFill.Vehicle.Exists() && !VehicleToFill.Vehicle.IsEngineOn && VehicleToFill.Vehicle.FuelLevel < 100f && VehicleToFill.RequiresFuel)
        {
            GetVehicleData();
            string MenuString = $"~n~Price Per Gallon: ~r~${pricePerUnit}~s~~n~Fuel Capacity: ~y~{VehicleToFillFuelTankCapacity}~s~ Gallons~n~Fuel Needed: ~p~{UnitsOfFuelNeeded}~s~ Gallons";
            GetGasSubMenu = MenuPool.AddSubMenu(InteractionMenu, $"Gas Up {VehicleToFillName}");
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
            FillMenuItem = new UIMenuItem("Fill", "Fill the entire tank" + MenuString) { RightLabel = UnitsOfFuelNeeded + " Gallons - " +  AmountToFill.ToString("C0") };
            if (UnitsOfFuelNeeded > 1)
            {
                AddSomeMenuItem = new UIMenuNumericScrollerItem<int>("Partial", "Add gasoline by the gallon" + MenuString, 1, UnitsOfFuelNeeded, 1) { Formatter = v => v + " Gallons - " + (v * pricePerUnit).ToString("C0") };
                GetGasSubMenu.AddItem(AddSomeMenuItem);
            }
            GetGasSubMenu.AddItem(FillMenuItem);            
        }
        else
        {
            if(VehicleToFill == null || (VehicleToFill != null && !VehicleToFill.Vehicle.Exists()))
            {
                Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", Name, "~r~Fueling Failed", $"No vehicle found to fuel");
            }
            else if (VehicleToFill != null && VehicleToFill.Vehicle.Exists() && !VehicleToFill.RequiresFuel)
            {
                Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", Name, "~r~Fueling Failed", $"Incompatible Fueling");
            }
            else if (VehicleToFill != null && VehicleToFill.Vehicle.Exists() && VehicleToFill.Vehicle.IsEngineOn)
            {
                Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", Name, "~r~Fueling Failed", $"Vehicle engine is still on");
            }
            else if (VehicleToFill != null && VehicleToFill.Vehicle.Exists() && !VehicleToFill.Vehicle.IsEngineOn && VehicleToFill.Vehicle.FuelLevel >= 100f)
            {
                Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", Name, "~r~Fueling Failed", $"Vehicle fuel tank is already full");
            }
            InteractionMenu.Visible = false;
        }
    }
    private void GetVehicleData()
    {
        VehicleToFillMakeName = NativeHelper.VehicleMakeName(VehicleToFill.Vehicle.Model.Hash);
        VehicleToFillModelName = NativeHelper.VehicleModelName(VehicleToFill.Vehicle.Model.Hash);
        VehicleToFillClassName = NativeHelper.VehicleClassName(VehicleToFill.Vehicle.Model.Hash);
        VehicleToFillName = (VehicleToFillMakeName + " " + VehicleToFillModelName).Trim();
        string CarDescription = "";
        if (VehicleToFillMakeName != "")
        {
            CarDescription += $"~n~Manufacturer: ~b~{VehicleToFillMakeName}~s~";
        }
        if (VehicleToFillModelName != "")
        {
            CarDescription += $"~n~Model: ~g~{VehicleToFillModelName}~s~";
        }
        if (VehicleToFillClassName != "")
        {
            CarDescription += $"~n~Class: ~p~{VehicleToFillClassName}~s~";
        }
        if(AssociatedStation != null)
        {
            pricePerUnit = AssociatedStation.PricePerGallon;
        }
        else
        {
            pricePerUnit = 3;
        }    
        PercentFuelNeeded = (100f - VehicleToFill.Vehicle.FuelLevel)/100f;
        VehicleToFillFuelTankCapacity = VehicleToFill.FuelTankCapacity;
        UnitsOfFuelNeeded = (int)Math.Ceiling(PercentFuelNeeded * VehicleToFillFuelTankCapacity);

        if (VehicleToFillFuelTankCapacity == 0)
        {
            PercentFilledPerUnit = 0;
        }
        else
        {
            PercentFilledPerUnit = 100f / VehicleToFillFuelTankCapacity;
        }

        AmountToFill = UnitsOfFuelNeeded * pricePerUnit;
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
        StartMachineBuyAnimation();
        base.OnItemPurchased(modItem, menuItem, totalItems);
    }
    private void InteractionMenu_OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if(selectedItem == FillMenuItem && VehicleToFill != null && VehicleToFill.Vehicle.Exists())
        {
            KeepInteractionGoing = true;
            InteractionMenu.Visible = false;
            sender.Visible = false;
            FuelVehicle(UnitsOfFuelNeeded);
        }
        else if (selectedItem == AddSomeMenuItem && VehicleToFill != null && VehicleToFill.Vehicle.Exists())
        {
            KeepInteractionGoing = true;
            InteractionMenu.Visible = false;
            sender.Visible = false;
            FuelVehicle(AddSomeMenuItem.Value);
        }
    }
    private void FuelVehicle(int UnitsToAdd)
    {
        if (UnitsToAdd * pricePerUnit > Player.BankAccounts.Money)
        {
            NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "ERROR", "HUD_LIQUOR_STORE_SOUNDSET", 0);
            Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", Name, "~r~Purchase Failed", "We are sorry, we are unable to complete this transation. Please make sure you have the funds.");
            FullDispose();
        }
        else
        {
            Player.ButtonPrompts.AddPrompt("Fueling", "Cancel Fueling", "CancelFueling", Settings.SettingsManager.KeySettings.InteractCancel, 99);
            KeepInteractionGoing = true;
            int UnitsAdded = 0;
            GameFiber FastForwardWatcher = GameFiber.StartNew(delegate
            {
                IsFueling = true;
                uint GameTimeBetweenUnits = 1500;
                uint GameTimeAddedUnit = Game.GameTime;
                int dotsAdded = 0;
                if (VehicleToFill.Vehicle.Exists())
                {
                    unsafe
                    {
                        int lol = 0;
                        NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
                        NativeFunction.CallByName<bool>("TASK_TURN_PED_TO_FACE_ENTITY", 0, VehicleToFill.Vehicle, 2000);
                        NativeFunction.CallByName<bool>("TASK_LOOK_AT_ENTITY", 0, VehicleToFill.Vehicle, -1, 0, 2);
                        NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, true);
                        NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
                        NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Player.Character, lol);
                        NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
                    }

                }
                while (UnitsAdded < UnitsToAdd && VehicleToFill.Vehicle.Exists() && !VehicleToFill.Vehicle.IsEngineOn)
                {
                    string tabs = new string('.', dotsAdded);
                    Game.DisplayHelp($"Fueling Progress {UnitsAdded}/{UnitsToAdd}");
                    NativeHelper.DisablePlayerControl();
                    //Game.LocalPlayer.HasControl = false;
                    if (Game.GameTime - GameTimeAddedUnit >= GameTimeBetweenUnits)
                    {
                        UnitsAdded++;
                        GameTimeAddedUnit = Game.GameTime;
                        if (VehicleToFill.Vehicle.FuelLevel + PercentFilledPerUnit > 100f)
                        {
                            VehicleToFill.Vehicle.FuelLevel = 100f;
                        }
                        else
                        {
                            VehicleToFill.Vehicle.FuelLevel += PercentFilledPerUnit;
                        }
                        Player.BankAccounts.GiveMoney(-1 * pricePerUnit);
                        NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "PURCHASE", "HUD_LIQUOR_STORE_SOUNDSET", 0);

                        EntryPoint.WriteToConsole($"Gas pump added unit of gas Percent Added {PercentFilledPerUnit} Money Subtracted {-1 * pricePerUnit}");
                    }
                    if (Player.ButtonPrompts.IsPressed("CancelFueling"))
                    {
                        break;
                    }
                    GameFiber.Yield();
                }
                if (UnitsAdded > 0)
                {
                    NativeFunction.Natives.PLAY_SOUND_FRONTEND(-1, "PURCHASE", "HUD_LIQUOR_STORE_SOUNDSET", 0);
                    Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", Name, "~g~Purchased", $"Thank you for purchasing {UnitsAdded} gallons of fuel for a total price of ~r~${UnitsAdded * pricePerUnit}~s~ at {Name}");
                }

                NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
                // Game.LocalPlayer.HasControl = true;
                NativeFunction.Natives.ENABLE_ALL_CONTROL_ACTIONS(0);
                KeepInteractionGoing = false;
                Player.ButtonPrompts.RemovePrompts("Fueling");
                IsFueling = false;

            }, "FastForwardWatcher");
        }
    }
    private void FullDispose()
    {
      // Deactivate();
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
                EntryPoint.WriteToConsole("Gas Pump You are Closer to the REAR, using that side");
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

