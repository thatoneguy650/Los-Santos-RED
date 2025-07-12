using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player;
using Rage;
using Rage.Native;
using System;
using System.Xml.Serialization;

[Serializable()]
public class ScrewdriverItem : ModItem
{
    private string PlayingDict;
    private string PlayingAnim;
    private uint GameTimeStartedLockPickAnimation;

    public int MinDoorPickTime { get; set; } = 9000;
    public int MaxDoorPickTime { get; set; } = 20000;

    public ScrewdriverItem()
    {

    }
    public ScrewdriverItem(string name, string description) : base(name, description, ItemType.Equipment)
    {

    }
    public ScrewdriverItem(string name) : base(name, ItemType.Equipment)
    {

    }
    public override bool UseItem(IActionable actionable, ISettingsProvideable settings, IEntityProvideable world, ICameraControllable cameraControllable, IIntoxicants intoxicants, ITimeControllable time)
    {
        ScrewdriverActivity activity = new ScrewdriverActivity(actionable, settings, this);
        if (activity.CanPerform(actionable))
        {
            actionable.ActivityManager.StartUpperBodyActivity(activity);
            return true;
        }
        return false;
    }
    public override void AddToList(PossibleItems possibleItems)
    {
        possibleItems?.ScrewdriverItems.RemoveAll(x => x.Name == Name);
        possibleItems?.ScrewdriverItems.Add(this);
        base.AddToList(possibleItems);
    }


    public void PickDoorLock(IInteractionable Player, IBasicUseable BasicUseable, Action OnCompletedDrilling)
    {
        PlayingDict = "missheistfbisetup1";
        PlayingAnim = "hassle_intro_loop_f";
        AnimationDictionary.RequestAnimationDictionay(PlayingDict);
        NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", Player.Character, PlayingDict, PlayingAnim, 2.0f, -2.0f, -1, 1, 0, false, false, false);
        GameTimeStartedLockPickAnimation = Game.GameTime;
        Rage.Object ScrewDriverObject = null;
        uint TimeToPick = RandomItems.GetRandomNumber(MinDoorPickTime, MaxDoorPickTime);
        ScrewDriverObject = SpawnAndAttachItem(BasicUseable, true, true);
        while (Player.ActivityManager.CanPerformActivitiesExtended)
        {
            Player.WeaponEquipment.SetUnarmed();
            if (Player.IsMoveControlPressed || !Player.Character.IsAlive)
            {
                break;
            }
            if (Game.GameTime - GameTimeStartedLockPickAnimation >= TimeToPick)
            {
                OnCompletedDrilling();
                Game.DisplayHelp("Door Opened");
                break;
            }
            GameFiber.Yield();
        }
        if (ScrewDriverObject.Exists())
        {
            ScrewDriverObject.Delete();
        }
    }


}

