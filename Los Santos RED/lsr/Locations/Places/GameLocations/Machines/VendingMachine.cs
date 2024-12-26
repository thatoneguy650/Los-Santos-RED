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

public class VendingMachine : GameLocation
{
    private UIMenuItem completeTask;
    private Vector3 PropEntryPosition;
    private float PropEntryHeading;
    private bool IsCancelled;
    private string PlayingDict;
    private string PlayingAnim;
    private bool hasAttachedProp;
    private MachineOffsetResult MachineInteraction;

    private Rage.Object SellingProp;
    private MoveInteraction MoveInteraction;

    public VendingMachine() : base()
    {

    }
    [XmlIgnore]
    public Rage.Object MachineProp { get; set; }
    public override bool ShowsOnDirectory { get; set; } = false;
    public override bool ShowsOnTaxi { get; set; } = false;
    public override string TypeName { get; set; } = "Vending Machine";
    public override int MapIcon { get; set; } = (int)BlipSprite.PointOfInterest;
    public override float MapIconScale { get; set; } = 0.25f;
    public override string ButtonPromptText { get; set; }
    public override bool CanCurrentlyInteract(ILocationInteractable player) 
    {
        ButtonPromptText = $"Shop at {Name}";
        return EntrancePosition != Vector3.Zero || (MachineProp.Exists() && player.CurrentLookedAtObject.Exists() && MachineProp.Handle == player.CurrentLookedAtObject.Handle);
    }
    public VendingMachine(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description, string menuID, Rage.Object machineProp) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {
        MenuID = menuID;
        MachineProp = machineProp;     
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
            GameFiber.StartNew(delegate
            {
                try
                {
                    if (Settings.SettingsManager.PlayerOtherSettings.SetHintCameraWhenUsingMachineInteractions)
                    {
                        NativeFunction.Natives.SET_GAMEPLAY_COORD_HINT(EntrancePosition.X, EntrancePosition.Y, EntrancePosition.Z, -1, 2000, 2000);
                    }
                    Vector3 FinalPlayerPos = new Vector3();
                    float FinalPlayerHeading = 0f;
                    if (MachineProp != null && MachineProp.Exists())
                    {
                        MachineOffsetResult machineInteraction = new MachineOffsetResult(Player, MachineProp);
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
                        Transaction = new Transaction(MenuPool, InteractionMenu, Menu, this);
                        Transaction.PreviewItems = false;
                        Transaction.CreateTransactionMenu(Player, ModItems, World, Settings, Weapons, Time);
                        InteractionMenu.Visible = true;
                        Transaction.ProcessTransactionMenu();
                        Transaction.DisposeTransactionMenu();
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
            }, "GangDenInteract");
        }
    }
    public override void OnItemPurchased(ModItem modItem,MenuItem menuItem, int totalItems)
    {
        MenuPool.CloseAllMenus();
        StartMachineBuyAnimation(modItem, false);
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
    }
    private void StartMachineBuyAnimation(ModItem item, bool isIllicit)
    {
        if (MoveInteraction.MoveToMachine(1.0f))// MoveToMachine())
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
       // EntryPoint.WriteToConsole($"Vending Activity Playing {PlayingDict} {PlayingAnim}");
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
        while (Player.ActivityManager.CanPerformActivitiesExtended && !IsCancelled)
        {
            Player.WeaponEquipment.SetUnarmed();
            float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, PlayingDict, PlayingAnim);
            if (AnimationTime >= 0.5f)
            {
                if (HasProp && modelName != "" && !hasAttachedProp && NativeFunction.Natives.IS_MODEL_VALID<bool>(Game.GetHashKey(modelName)))
                {
                    try
                    {
                        SellingProp = new Rage.Object(modelName, Player.Character.GetOffsetPositionUp(50f));
                    }
                    catch (Exception ex)
                    {
                        //EntryPoint.WriteToConsoleTestLong($"Error Spawning Model {ex.Message} {ex.StackTrace}");
                    }
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
    public override void AddLocation(PossibleLocations possibleLocations)
    {
        possibleLocations.VendingMachines.Add(this);
        base.AddLocation(possibleLocations);
    }
}

