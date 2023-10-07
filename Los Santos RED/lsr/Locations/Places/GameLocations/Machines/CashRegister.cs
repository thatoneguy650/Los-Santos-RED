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

public class CashRegister : GameLocation
{
    private UIMenuItem completeTask;
    private Vector3 PropEntryPosition;
    private float PropEntryHeading;
    private bool IsCancelled;
    private string PlayingDict;
    private string PlayingAnim;
    private bool hasAttachedProp;
    private MachineInteraction MachineInteraction;
    private string registerStealEmptyText = "Register Empty";
    public CashRegister() : base()
    {

    }
    [XmlIgnore]
    public Rage.Object RegisterProp { get; set; }
    public override bool ShowsOnDirectory { get; set; } = false;
    public override bool ShowsOnTaxi { get; set; } = false;
    public override string TypeName { get; set; } = "Cash Register";
    public override int MapIcon { get; set; } = (int)BlipSprite.PointOfInterest;
    public override float MapIconScale { get; set; } = 0.25f;
    public override string ButtonPromptText { get; set; }


    public override bool CanCurrentlyInteract(ILocationInteractable player)
    {
        ButtonPromptText = $"Steal from {Name}";
        return RegisterProp.Exists() && player.CurrentLookedAtObject.Exists() && RegisterProp.Handle == player.CurrentLookedAtObject.Handle;
    }
    public CashRegister(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description, string menuID, Rage.Object machineProp, int cash) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {
        RegisterCash = cash;
        MenuID = menuID;
        RegisterProp = machineProp;
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
                    NativeFunction.Natives.SET_GAMEPLAY_COORD_HINT(EntrancePosition.X, EntrancePosition.Y, EntrancePosition.Z, -1, 2000, 2000);
                    MachineInteraction = new MachineInteraction(Player, RegisterProp);
                    MachineInteraction.IsSingleSided = true;
                    MachineInteraction.CloseDistance = 0.5f;
                    if (MachineInteraction.MoveToMachine())
                    {
                        if (RegisterCash == 0)
                        {
                            Game.DisplaySubtitle(registerStealEmptyText);
                        }
                        else
                        {
                            PlayMoneyAnimation();
                        }
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

    private void FullDispose()
    {
        NativeFunction.Natives.STOP_GAMEPLAY_HINT(false);
        NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
    }
    private bool PlayMoneyAnimation()
    {
        Player.ActivityManager.StopDynamicActivity();
        AnimationDictionary.RequestAnimationDictionay("oddjobs@shop_robbery@rob_till");
        unsafe
        {
            int lol = 0;
            NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
            NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", 0, "oddjobs@shop_robbery@rob_till", "enter", 4.0f, -4.0f, -1, 0, 0, false, false, false);
            NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", 0, "oddjobs@shop_robbery@rob_till", "loop", 4.0f, -4.0f, -1, 1, 0, false, false, false);
            NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
            NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
            NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Player.Character, lol);
            NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
        }
        IsCancelled = false;
        Player.Violations.TheftViolations.IsRobbingBank = true;
        uint GameTimeLastGotCash = Game.GameTime;
        while (Player.ActivityManager.CanPerformActivitiesExtended)
        {
            Player.WeaponEquipment.SetUnarmed();
            if (Game.GameTime - GameTimeLastGotCash >= 900)
            {
                GiveCash();
                GameTimeLastGotCash = Game.GameTime;
            }
            if (RegisterCash <= 0)
            {
                break;
            }
            if (Player.IsMoveControlPressed || !Player.Character.IsAlive)
            {
                IsCancelled = true;
                break;
            }
            GameFiber.Yield();
        }
        EntryPoint.WriteToConsole($"REGISTER PlayMoneyAnimation IsCancelled: {IsCancelled}");
        NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
        Player.Violations.TheftViolations.IsRobbingBank = false;
        if (IsCancelled)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    private void GiveCash()
    {
        if (RegisterCash <= Settings.SettingsManager.PlayerOtherSettings.RobberyCashPerSwipe)
        {
            Player.BankAccounts.GiveMoney(RegisterCash, false);
            PlaySuccessSound();
            RegisterCash = 0;
            EntryPoint.WriteToConsole($"REGISTER GAVE CASH 1 {RegisterCash}");
        }
        else
        {
            Player.BankAccounts.GiveMoney(Settings.SettingsManager.PlayerOtherSettings.RobberyCashPerSwipe, false);
            PlaySuccessSound();
            RegisterCash -= Settings.SettingsManager.PlayerOtherSettings.RobberyCashPerSwipe;
            EntryPoint.WriteToConsole($"REGISTER GAVE CASH 2 {RegisterCash}");
        }
        Game.DisplaySubtitle($"REGISTER Cash: ${RegisterCash}");
    }
}

