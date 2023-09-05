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

public class ATMMachine : GameLocation// i know m stand for machine, makes it neater tho
{
    private UIMenuItem completeTask;
    private Vector3 PropEntryPosition;
    private float PropEntryHeading;
    private bool IsCancelled;
    private string PlayingDict;
    private string PlayingAnim;
    private bool hasAttachedProp;
    private UIMenu AccountSubMenu;
    private Rage.Object SellingProp;
    private Bank AssociatedBank;
    private bool KeepInteractionGoing;
    private BankInteraction BankInteraction;

    public Rage.Object ATMObject { get; private set; } = null;
    public ATMMachine() : base()
    {

    }
    public override bool ShowsOnDirectory { get; set; } = false;
    public override string TypeName { get; set; } = "ATM";
    public override int MapIcon { get; set; } = 500;//361;// (int)BlipSprite.PointOfInterest;
    public override float MapIconScale { get; set; } = 0.25f;
    public override string ButtonPromptText { get; set; }
    public override bool CanCurrentlyInteract(ILocationInteractable player)
    {
        ButtonPromptText = $"Access {Name} ATM";
        return ATMObject.Exists() && player.CurrentLookedAtObject.Exists() && ATMObject.Handle == player.CurrentLookedAtObject.Handle;
    }
    public ATMMachine(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description, string menuID, Rage.Object machineProp, Bank bank) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {
        MenuID = menuID;
        ATMObject = machineProp;
        AssociatedBank = bank;
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



                    BankInteraction = new BankInteraction(Player, AssociatedBank);
                    BankInteraction.Start(MenuPool, InteractionMenu);





                    while (IsAnyMenuVisible || KeepInteractionGoing)
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
            }, "ATM Interact");
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
        Player.ButtonPrompts.RemovePrompts("ATM");
        //IsFueling = false;
    }
    private void GetPropEntry()
    {
        if (ATMObject != null && ATMObject.Exists())
        {
            float DistanceToFront = Player.Position.DistanceTo2D(ATMObject.GetOffsetPositionFront(-1f));
            float DistanceToRear = Player.Position.DistanceTo2D(ATMObject.GetOffsetPositionFront(1f));
            if (DistanceToFront <= DistanceToRear)
            {
                PropEntryPosition = ATMObject.GetOffsetPositionFront(-1f);
                PropEntryPosition = new Vector3(PropEntryPosition.X, PropEntryPosition.Y, Game.LocalPlayer.Character.Position.Z);
                float ObjectHeading = ATMObject.Heading - 180f;
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
                PropEntryPosition = ATMObject.GetOffsetPositionFront(1f);
                PropEntryPosition = new Vector3(PropEntryPosition.X, PropEntryPosition.Y, Game.LocalPlayer.Character.Position.Z);
                float ObjectHeading = ATMObject.Heading;
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

