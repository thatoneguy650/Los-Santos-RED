using ExtensionsMethods;
using LosSantosRED.lsr.Helper;
using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

public class DeadDrop : InteractableLocation
{
    private bool IsCancelled;

    public DeadDrop() : base()
    {
        
    }
    public override bool ShowsOnDirectory { get; set; } = false;
    public override string TypeName { get; set; } = "Dead Drop";
    public override int MapIcon { get; set; } = (int)BlipSprite.Destination;
    public override Color MapIconColor { get; set; } = Color.Blue;
    public override string ButtonPromptText { get; set; }
    public override float MapIconScale { get; set; } = 1.0f;
    public override float MapIconRadius { get; set; } = 55.0f;
    public override float MapIconAlpha { get; set; } = 0.35f;

    [XmlIgnore]
    public bool InteractionComplete { get; set; } = false;

    public bool IsDropOff { get; set; } = true;
    public int MoneyAmount { get; set; } = 500;
  //  public Gang AssociatedGang { get; set; }
    public DeadDrop(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {

    }
    public override void OnInteract(ILocationInteractable Player, IModItems modItems, IEntityProvideable world, ISettingsProvideable settings, IWeapons weapons, ITimeControllable time)
    {
        if (IsEnabled)
        {
            GameFiber.StartNew(delegate
            {
                if (IsDropOff)
                {
                    DoDropOff(Player);
                }
                else
                {
                    DoPickup(Player);
                }
                
            }, "DeadDropLoop");
        }
        //base.OnInteract(player);
    }
    private void DoDropOff(IActivityPerformable Player)
    {
        if (Player.Money >= Math.Abs(MoneyAmount))
        {
            Player.IsInteractingWithLocation = true;
            CanInteract = false;
            if (!MoveToDrop(Player) || !PlayMoneyAnimation(Player))
            {
                Player.IsInteractingWithLocation = false;
                CanInteract = true;
                return;
            }
            Game.DisplayHelp("You have dropped off the cash, leave the area");
            Player.GiveMoney(-1 * MoneyAmount);
            //CompleteOnLeaveArea(Player);
            //IsEnabled = false;
            CanInteract = false;
            IsEnabled = false;
            InteractionComplete = true;


            Dispose();
            ButtonPromptText = "";
            //ClearActiveGangTasks(Player);
            Player.IsInteractingWithLocation = false;
        }
        else
        {
            Game.DisplayHelp("You do not have enought cash to make the drop");
        }
    }
    private void DoPickup(IActivityPerformable Player)
    {
        CanInteract = false;
        Player.IsInteractingWithLocation = true;
        if (!MoveToDrop(Player) || !PlayMoneyAnimation(Player))
        {
            Player.IsInteractingWithLocation = false;
            CanInteract = true;
            return;
        }
        Game.DisplayHelp("You have picked up the cash, don't hang around");
        Player.GiveMoney(MoneyAmount);

        IsEnabled = false;

        InteractionComplete = true;
       // Player.PlayerTasks.GetTask(AssociatedGang.ContactName).IsReadyForPayment = true;

       // SendMessageOnLeaveArea(Player);
        ButtonPromptText = "";
        Dispose();
        Player.IsInteractingWithLocation = false;
        //ClearActiveGangTasks(Player);
    }
    private bool PlayMoneyAnimation(IActivityPerformable Player)
    {
        Player.StopDynamicActivity();
        AnimationDictionary.RequestAnimationDictionay("mp_safehousevagos@");

        NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Player.Character, "mp_safehousevagos@", "package_dropoff", 4.0f, -4.0f, 2000, 0, 0, false, false, false);
        //NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "mp_car_bomb", "car_bomb_mechanic", 2.0f, -2.0f, 5000, 0, 0, false, false, false);
        IsCancelled = false;
        uint GameTimeStartedAnimation = Game.GameTime;

        Player.IsDoingSuspiciousActivity = true;

        while (Player.CanPerformActivities && Game.GameTime - GameTimeStartedAnimation <= 2000)
        {
            Player.SetUnarmed();
            float AnimationTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Player.Character, "mp_safehousevagos@", "package_dropoff");
            if (AnimationTime >= 1.0f)
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
        EntryPoint.WriteToConsole($"Dead Drop PlayMoneyAnimation IsCancelled: {IsCancelled}");
        NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);

        Player.IsDoingSuspiciousActivity = false;

        if (IsCancelled)
        {
            return false;
        }
        else
        {
            return true;
        }    
    }
    private bool MoveToDrop(IActivityPerformable Player)
    {
        Vector3 MovePosition = NativeHelper.GetOffsetPosition(EntrancePosition, EntranceHeading, 2f);//  Store.PropObject.GetOffsetPositionFront(-1f);
        MovePosition = new Vector3(MovePosition.X, MovePosition.Y, Game.LocalPlayer.Character.Position.Z);
        float ObjectHeading = EntranceHeading - 180f;
        if (ObjectHeading >= 180f)
        {
            ObjectHeading -= 180f;
        }
        else
        {
            ObjectHeading += 180f;
        }

        NativeFunction.Natives.TASK_GO_STRAIGHT_TO_COORD(Game.LocalPlayer.Character, MovePosition.X, MovePosition.Y, MovePosition.Z, 1.0f, -1, ObjectHeading, 0.2f);
        uint GameTimeStartedSitting = Game.GameTime;
        float heading = Game.LocalPlayer.Character.Heading;
        bool IsFacingDirection = false;
        bool IsCloseEnough = false;
        IsCancelled = false;
        while (Game.GameTime - GameTimeStartedSitting <= 5000 && !IsCloseEnough && !IsCancelled)
        {
            if (Player.IsMoveControlPressed)
            {
                IsCancelled = true;
            }
            IsCloseEnough = Game.LocalPlayer.Character.DistanceTo2D(MovePosition) < 3f;
            GameFiber.Yield();
        }
        GameFiber.Sleep(250);
        GameTimeStartedSitting = Game.GameTime;
        while (Game.GameTime - GameTimeStartedSitting <= 2000 && !IsFacingDirection && !IsCancelled)
        {
            if (Player.IsMoveControlPressed)
            {
                IsCancelled = true;
            }

            if(Extensions.PointIsDirectlyInFrontOfPed(Game.LocalPlayer.Character, MovePosition))
            {
                IsFacingDirection = true;
            }
            GameFiber.Yield();
        }
        GameFiber.Sleep(250);
        if (IsCloseEnough && IsFacingDirection && !IsCancelled)
        {
            EntryPoint.WriteToConsole($"Moving to drop IN POSITION {Game.LocalPlayer.Character.DistanceTo(MovePosition)} {ExtensionsMethods.Extensions.GetHeadingDifference(heading, ObjectHeading)} {heading} {ObjectHeading}", 5);
            return true;
        }
        else
        {
            NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
            return false;
        }
    }
    public void SetupDrop(int moneyToReceive, bool isDropOff)
    {
        IsEnabled = true;
        IsDropOff = isDropOff;
        MoneyAmount = moneyToReceive;
        InteractionComplete = false;
        if (IsDropOff)
        {
            ButtonPromptText = $"Drop ${Math.Abs(MoneyAmount)}";
        }
        else
        {
            ButtonPromptText = $"Pickup ${Math.Abs(MoneyAmount)}";
        }
    }
}

