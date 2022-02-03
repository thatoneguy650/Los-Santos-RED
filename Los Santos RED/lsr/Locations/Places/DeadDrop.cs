using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using Rage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DeadDrop : InteractableLocation
{
    public bool HasDroppedMoney { get; set; } = false;
    public DeadDrop() : base()
    {
        
    }
    public override BlipSprite MapIcon { get; set; } = BlipSprite.Dead;
    public override Color MapIconColor { get; set; } = Color.White;
    public override string ButtonPromptText { get; set; }
    public int MoneyToReceive { get; set; } = 500;
    public Gang AssociatedGang { get; set; }
    public DeadDrop(Vector3 _EntrancePosition, float _EntranceHeading, string _Name, string _Description) : base(_EntrancePosition, _EntranceHeading, _Name, _Description)
    {

    }
    public override void OnInteract(IActivityPerformable Player)
    {
        if (!HasDroppedMoney)
        {
            if (MoneyToReceive < 0)
            {
                if (Player.Money >= Math.Abs(MoneyToReceive))
                {
                    Game.DisplayNotification("You have dropped off the cash");
                    Player.CellPhone.AddScheduledText(AssociatedGang.ContactName, AssociatedGang.ContactIcon,"Now leave the area.", 0);
                    Player.GiveMoney(MoneyToReceive);
                    HasDroppedMoney = true;
                    WaitToLeave(Player);
                    ButtonPromptText = "";
                }
                else
                {
                    Game.DisplayNotification("You do not have enought cash to make the drop");
                }
            }
        }
        //base.OnInteract(player);
    }
    private void WaitToLeave(IActivityPerformable Player)
    {
        GameFiber.StartNew(delegate
        {
            while (IsNearby)
            {
                GameFiber.Yield();
            }

            Player.GangRelationships.SetReputation(AssociatedGang, 0, false);
            List<string> Replies = new List<string>() {
                    "I guess we can forget about that shit.",
                    "No problem man, all is forgiven",
                    "That shit before? Forget about it.",
                    "We are square",
                    "You are off the hit list",
                    "This doesn't make us friends prick, just associates",

                    };

            Player.CellPhone.AddScheduledText(AssociatedGang.ContactName, AssociatedGang.ContactIcon, Replies.PickRandom(), 0);

            //Game.DisplayNotification(AssociatedGang.ContactIcon, AssociatedGang.ContactIcon, AssociatedGang.ContactName, "~o~Response", Replies.PickRandom());


        }, "LeaveChecker");
    }
    public void SetGang(Gang gang, int MoneyToDrop)
    {
        IsEnabled = true;
        AssociatedGang = gang;
        MoneyToReceive = MoneyToDrop;
        ButtonPromptText = $"Drop ${Math.Abs(MoneyToReceive)}";
        HasDroppedMoney = false;
    }
}

