using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using RAGENativeUI.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

public class VendingMachine : InteractableLocation
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

    private Rage.Object SellingProp;
    public VendingMachine() : base()
    {

    }
    //[XmlIgnore]
    //public ShopMenu Menu { get; set; }
    //public string MenuID { get; set; }
    [XmlIgnore]
    public Rage.Object MachineProp { get; set; }
    public override bool ShowsOnDirectory { get; set; } = false;
    public override string TypeName { get; set; } = "Vending Machine";
    public override int MapIcon { get; set; } = 434;// (int)BlipSprite.PointOfInterest;
    public override Color MapIconColor { get; set; } = Color.White;
    public override float MapIconScale { get; set; } = 0.5f;
    public override string ButtonPromptText { get; set; }
    public VendingMachine(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description, string menuID, Rage.Object machineProp) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {
        MenuID = menuID;
        MachineProp = machineProp;
        ButtonPromptText = $"Shop at {Name}";
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
            Player.IsInteractingWithLocation = true;
            CanInteract = false;
            Player.IsTransacting = true;
            GameFiber.StartNew(delegate
            {

                NativeFunction.Natives.SET_GAMEPLAY_COORD_HINT(EntrancePosition.X, EntrancePosition.Y, EntrancePosition.Z, -1, 2000, 2000);
                GetPropEntry();
                if (!MoveToMachine())
                {
                    EntryPoint.WriteToConsole("Transaction: TOP LEVE DISPOSE AFTER NO MOVE FUCKER", 5);
                    FullDispose();
                }

                CreateInteractionMenu();
                Transaction = new Transaction(MenuPool, InteractionMenu, Menu, this);

                Transaction.PreviewItems = false;

                Transaction.CreateTransactionMenu(Player, modItems, world, settings, weapons, time);

                InteractionMenu.Visible = true;
                InteractionMenu.OnItemSelect += InteractionMenu_OnItemSelect;
                Transaction.ProcessTransactionMenu();


                Transaction.DisposeTransactionMenu();
                DisposeInteractionMenu();
                FullDispose();

                //NativeFunction.Natives.STOP_GAMEPLAY_HINT(false);
                //NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);

                Player.IsInteractingWithLocation = false;
                Player.IsTransacting = false;
                CanInteract = true;
            }, "GangDenInteract");
        }
    }
    public override void OnItemPurchased(ModItem modItem,MenuItem menuItem, int totalItems)
    {
        StartMachineBuyAnimation(modItem, false);
        base.OnItemPurchased(modItem, menuItem, totalItems);
    }
    private void InteractionMenu_OnItemSelect(RAGENativeUI.UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (selectedItem.Text == "Buy")
        {
            Transaction?.SellMenu?.Dispose();
            Transaction?.PurchaseMenu?.Show();
        }
        else if (selectedItem.Text == "Sell")
        {
            Transaction?.PurchaseMenu?.Dispose();
            Transaction?.SellMenu?.Show();
        }
    }
    private void FullDispose()
    {
        Deactivate();
        NativeFunction.Natives.STOP_GAMEPLAY_HINT(false);
        NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
    }
    private void GetPropEntry()
    {
        if (MachineProp != null && MachineProp.Exists())
        {
            PropEntryPosition = MachineProp.GetOffsetPositionFront(-1f);
            PropEntryPosition = new Vector3(PropEntryPosition.X, PropEntryPosition.Y, Game.LocalPlayer.Character.Position.Z);
            float ObjectHeading = MachineProp.Heading - 180f;
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
    private bool MoveToMachine()
    {
        if(PropEntryPosition == Vector3.Zero)
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
                EntryPoint.WriteToConsole($"Moving to Machine FACING TRUE {Game.LocalPlayer.Character.DistanceTo(PropEntryPosition)} {ExtensionsMethods.Extensions.GetHeadingDifference(heading, PropEntryHeading)} {heading} {PropEntryHeading}", 5);
            }
            GameFiber.Yield();
        }
        GameFiber.Sleep(250);
        if (IsCloseEnough && IsFacingDirection && !IsCancelled)
        {
            EntryPoint.WriteToConsole($"Moving to Machine IN POSITION {Game.LocalPlayer.Character.DistanceTo(PropEntryPosition)} {ExtensionsMethods.Extensions.GetHeadingDifference(heading, PropEntryHeading)} {heading} {PropEntryHeading}", 5);
            return true;
        }
        else
        {
            NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
            return false;
        }
    }
    private void StartMachineBuyAnimation(ModItem item, bool isIllicit)
    {

        if (MoveToMachine())
        {
            if (UseMachine(item))
            {

            }
        }
        else
        {
            FullDispose();
        }
        if (SellingProp.Exists())
        {
            SellingProp.Delete();
        }
    }
    private bool UseMachine(ModItem item)
    {
        string modelName = "";
        bool HasProp = false;
        if (item.PackageItem != null && item.PackageItem.ModelName != "")
        {
            modelName = item.PackageItem.ModelName;
            HasProp = true;
        }
        else if (item.ModelItem != null && item.ModelItem.ModelName != "")
        {
            modelName = item.ModelItem.ModelName;
            HasProp = true;
        }
        


        PlayingDict = "mini@sprunk";
        PlayingAnim = "plyr_buy_drink_pt1";
        AnimationDictionary.RequestAnimationDictionay(PlayingDict);

        NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayingDict, PlayingAnim, 2.0f, -4.0f, -1, 0, 0, false, false, false);//-1
        EntryPoint.WriteToConsole($"Vending Activity Playing {PlayingDict} {PlayingAnim}", 5);
        bool IsCompleted = false;


        string HandBoneName = "BONETAG_R_PH_HAND";
        Vector3 HandOffset = Vector3.Zero;
        Rotator HandRotator = Rotator.Zero;
        PropAttachment pa = item?.ModelItem?.Attachments?.FirstOrDefault(x => x.Name == "RightHand" && (x.Gender == "U" || x.Gender == Player.Gender));
        if (pa != null)
        {
            HandOffset = pa.Attachment;
            HandRotator = pa.Rotation;
            HandBoneName = pa.BoneName;
        }
        while (Player.CanPerformActivities && !IsCancelled)
        {
            Player.WeaponEquipment.SetUnarmed();
            float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, PlayingDict, PlayingAnim);
            if (AnimationTime >= 0.5f)
            {
                if (HasProp && modelName != "" && !hasAttachedProp)
                {
                    SellingProp = new Rage.Object(modelName, Player.Character.GetOffsetPositionUp(50f));
                    GameFiber.Yield();                  
                    if (SellingProp.Exists())
                    {
                        SellingProp.AttachTo(Player.Character, NativeFunction.CallByName<int>("GET_ENTITY_BONE_INDEX_BY_NAME", Player.Character, HandBoneName), HandOffset, HandRotator);
                    }
                    hasAttachedProp = true;
                }
            }
            if (AnimationTime >= 0.7f)
            {
                IsCompleted = true;
                break;
            }
            GameFiber.Yield();
        }
        NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
        return IsCompleted;
    }
}

