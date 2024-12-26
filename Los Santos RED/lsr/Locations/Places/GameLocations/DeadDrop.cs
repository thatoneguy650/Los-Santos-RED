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

public class DeadDrop : GameLocation
{
    private bool IsCancelled;

    public DeadDrop() : base()
    {
        
    }
    public override bool ShowsOnDirectory { get; set; } = false;

    public override string TypeName { get; set; } = "Dead Drop";
    public override int MapIcon { get; set; } = (int)BlipSprite.Destination;
    public override string MapIconColorString { get; set; } = "Blue";
    public override string ButtonPromptText { get; set; }
    public override float MapIconScale { get; set; } = 1.0f;
    public override float MapIconRadius { get; set; } = 55.0f;
    public override float MapOpenIconAlpha { get; set; } = 0.35f;


    [XmlIgnore]
    public bool InteractionComplete { get; set; } = false;
    [XmlIgnore]
    public bool IsDropOff { get; set; } = true;
    [XmlIgnore]
    public int MoneyAmount { get; set; } = 500;
    public bool CanUse => !IsEnabled;

    public DeadDrop(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {

    }
    public override void OnInteract()//ILocationInteractable Player, IModItems modItems, IEntityProvideable world, ISettingsProvideable settings, IWeapons weapons, ITimeControllable time, IPlacesOfInterest placesOfInterest)
    {
        if (IsEnabled)
        {
            GameFiber.StartNew(delegate
            {
                try
                { 
                    if (IsDropOff)
                    {
                        DoDropOff(Player);
                    }
                    else
                    {
                        DoPickup(Player);
                    }
                }
                catch (Exception ex)
                {
                    EntryPoint.WriteToConsole("Location Interaction" + ex.Message + " " + ex.StackTrace, 0);
                    EntryPoint.ModController.CrashUnload();
                }

            }, "DeadDropLoop");
        }
        //base.OnInteract(player);
    }
    private void DoDropOff(ILocationInteractable Player)
    {
        if (Player.BankAccounts.GetMoney(false) >= Math.Abs(MoneyAmount))
        {
            Player.ActivityManager.IsInteractingWithLocation = true;
            CanInteract = false;
            if (!MoveToDrop(Player) || !PlayMoneyAnimation(Player))
            {
                Player.ActivityManager.IsInteractingWithLocation = false;
                CanInteract = true;
                return;
            }
            Game.DisplayHelp("You have dropped off the cash, leave the area");
            Player.BankAccounts.GiveMoney(-1 * MoneyAmount, false);
            CanInteract = false;
            IsEnabled = false;
            MoneyAmount = 0;
            InteractionComplete = true;
            Deactivate(true);
            ButtonPromptText = "";
            Player.ActivityManager.IsInteractingWithLocation = false;
        }
        else
        {
            Game.DisplayHelp("You do not have enought cash to make the drop!");
        }
    }
    private void DoPickup(ILocationInteractable Player)
    {
        CanInteract = false;
        Player.ActivityManager.IsInteractingWithLocation = true;
        if (!MoveToDrop(Player) || !PlayMoneyAnimation(Player))
        {
            Player.ActivityManager.IsInteractingWithLocation = false;
            CanInteract = true;
            return;
        }
        Game.DisplayHelp("You have picked up the cash, don't hang around");
        Player.BankAccounts.GiveMoney(MoneyAmount, false);
        IsEnabled = false;
        MoneyAmount = 0;
        InteractionComplete = true;
        ButtonPromptText = "";
        Deactivate(true);
        Player.ActivityManager.IsInteractingWithLocation = false;
    }
    private bool PlayMoneyAnimation(ILocationInteractable Player)
    {
        Player.ActivityManager.StopDynamicActivity();
        AnimationDictionary.RequestAnimationDictionay("mp_safehousevagos@");

        NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", Player.Character, "mp_safehousevagos@", "package_dropoff", 4.0f, -4.0f, 2000, 0, 0, false, false, false);
        //NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, "mp_car_bomb", "car_bomb_mechanic", 2.0f, -2.0f, 5000, 0, 0, false, false, false);
        IsCancelled = false;
        uint GameTimeStartedAnimation = Game.GameTime;

        Player.IsDoingSuspiciousActivity = true;

        while (Player.ActivityManager.CanPerformActivitiesExtended && Game.GameTime - GameTimeStartedAnimation <= 2000)
        {
            Player.WeaponEquipment.SetUnarmed();
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
        //EntryPoint.WriteToConsoleTestLong($"Dead Drop PlayMoneyAnimation IsCancelled: {IsCancelled}");
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
    private bool MoveToDrop(ILocationInteractable Player)
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

        // NativeFunction.Natives.TASK_GO_STRAIGHT_TO_COORD(Game.LocalPlayer.Character, MovePosition.X, MovePosition.Y, MovePosition.Z, 1.0f, -1, ObjectHeading, 0.2f);
        NativeFunction.Natives.TASK_FOLLOW_NAV_MESH_TO_COORD(Player.Character, MovePosition.X, MovePosition.Y, MovePosition.Z, 1.0f, -1, 0.5f, 0, ObjectHeading);
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
            //EntryPoint.WriteToConsole($"Moving to drop IN POSITION {Game.LocalPlayer.Character.DistanceTo(MovePosition)} {ExtensionsMethods.Extensions.GetHeadingDifference(heading, ObjectHeading)} {heading} {ObjectHeading}", 5);
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
    public override void Reset()
    {
        InteractionComplete = false;
        IsEnabled = false;
        IsDropOff = false;
        MoneyAmount = 0;
    }
    public override string ToString()
    {
        return Description;
    }
    public override void AddLocation(PossibleLocations possibleLocations)
    {
        possibleLocations.DeadDrops.Add(this);
        base.AddLocation(possibleLocations);
    }
}

