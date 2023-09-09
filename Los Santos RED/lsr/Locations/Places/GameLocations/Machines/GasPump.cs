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
    private MachineInteraction MachineInteraction;

    public Rage.Object PumpProp { get; private set; } = null;
    public GasPump() : base()
    {

    }

    public override bool ShowsOnDirectory { get; set; } = false;
    public override string TypeName { get; set; } = "Gas Pump";
    public override int MapIcon { get; set; } = 361;// (int)BlipSprite.PointOfInterest;
    public override float MapIconScale { get; set; } = 0.25f;
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


                    MachineInteraction = new MachineInteraction(Player, PumpProp);
                    if (MachineInteraction.MoveToMachine())
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
            GetGasSubMenu.OnMenuOpen += (sender) =>
            {
                if (VehicleToFill != null && VehicleToFill.Vehicle.Exists())
                {
                    NativeFunction.Natives.SET_GAMEPLAY_VEHICLE_HINT(VehicleToFill.Vehicle, 0f, 0f, 0f, true, -1, 2000, 2000);
                }
            };
            FillMenuItem = new UIMenuItem("Fill", "Fill the entire tank" + MenuString) { RightLabel = Refueling.UnitsOfFuelNeeded + " Gallons - " + Refueling.AmountToFill.ToString("C0") };
            FillMenuItem.Activated += (sender, e) =>
            {
                if (VehicleToFill != null && VehicleToFill.Vehicle.Exists())
                {
                    IsFueling = true;
                    InteractionMenu.Visible = false;
                    sender.Visible = false;
                    Refueling.RefuelSlow(Refueling.UnitsOfFuelNeeded, this);
                    StartMachineBuyAnimation();
                }
            };
            if (Refueling.UnitsOfFuelNeeded > 0)
            {
                AddSomeMenuItem = new UIMenuNumericScrollerItem<int>("Partial", "Add gasoline by the gallon" + MenuString, 1, Refueling.UnitsOfFuelNeeded, 1) { Formatter = v => v + " Gallons - " + (v * pricePerUnit).ToString("C0") };
                AddSomeMenuItem.Activated += (sender, e) =>
                {
                    if (VehicleToFill != null && VehicleToFill.Vehicle.Exists())
                    {
                        IsFueling = true;
                        InteractionMenu.Visible = false;
                        sender.Visible = false;
                        Refueling.RefuelSlow(AddSomeMenuItem.Value, this);
                        StartMachineBuyAnimation();
                    }
                };
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
    public override void OnItemPurchased(ModItem modItem, MenuItem menuItem, int totalItems)
    {
        MenuPool.CloseAllMenus();
        StartMachineBuyAnimation();
        base.OnItemPurchased(modItem, menuItem, totalItems);
        Transaction.PurchaseMenu?.Show();
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
    private void StartMachineBuyAnimation()
    {
        if (!MachineInteraction.MoveToMachine())
        {
            FullDispose();
            return;
        }
        //if (UseMachine())
        //{

        //}
    }
    //private bool UseMachine()
    //{
    //    PlayingDict = "mini@sprunk";
    //    PlayingAnim = "plyr_buy_drink_pt1";
    //    AnimationDictionary.RequestAnimationDictionay(PlayingDict);
    //    NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayingDict, PlayingAnim, 2.0f, -4.0f, -1, 0, 0, false, false, false);//-1
    //    bool IsCompleted = false;
    //    while (Player.ActivityManager.CanPerformActivitiesExtended && !IsCancelled)
    //    {
    //        Player.WeaponEquipment.SetUnarmed();
    //        float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, PlayingDict, PlayingAnim);
    //        if (AnimationTime >= 0.4f)
    //        {
    //            IsCompleted = true;
    //            break;
    //        }
    //        GameFiber.Yield();
    //    }
    //    NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
    //    return IsCompleted;
    //}
}

