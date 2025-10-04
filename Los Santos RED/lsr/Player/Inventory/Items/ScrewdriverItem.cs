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


    public int MinVehicleDoorPickTime { get; set; } = 4000;
    public int MaxVehicleDoorPickTime { get; set; } = 8000;


    public float AlarmPercentageMin { get; set; } = 5f;//60f;
    public float AlarmPercentageMax { get; set; } = 15f;//75f;
    public uint GameTimeBetweenAlarmChecksMin { get; set; } = 4000;
    public uint GameTimeBetweenAlarmChecksMax { get; set; } = 8000;

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


    public bool DoLockpickAnimation(IInteractionable Player, IBasicUseable BasicUseable, Action OnCompletedPicking, ISettingsProvideable Settings, bool showHelpText, bool isVehicle, Interior interior) => DoLockpickAnimation(Player, BasicUseable, OnCompletedPicking, Settings, "", "", showHelpText, isVehicle, null, -1, -1, interior);

    public bool DoLockpickAnimation(IInteractionable Player, IBasicUseable BasicUseable, Action OnCompletedPicking, ISettingsProvideable Settings, string dictionary, string anim, bool showHelpText, bool isVehicle, Vehicle TargetVehicle, int SeatTryingToEnter, int DoorIndex, Interior interior)
    {
        float LockpickAnimStopPercentage = 0.5f;
        float LockpickAnimRestartPercentage = 0.3f;
        float LockpickAnimIntroRate = 8.0f;
        float LockpickAnimOutroRate = -8.0f;
        int LockpickAnimFlags = Settings.SettingsManager.DebugSettings.LockpickAnimFlags;
        float LockpickAnimAnimRate = Settings.SettingsManager.DebugSettings.LockpickAnimAnimRate;



        Player.ActivityManager.IsPerformingActivity = true;
        string animDictionary = "veh@break_in@0h@p_m_one@";
        string animName = "std_force_entry_ds";

        if(!string.IsNullOrEmpty(dictionary))
        {
            animDictionary = dictionary;
        }
        if (!string.IsNullOrEmpty(anim))
        {
            animName = anim;
        }
        AnimationDictionary.RequestAnimationDictionay(animDictionary);
        NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, animDictionary, animName, LockpickAnimIntroRate,
            LockpickAnimOutroRate, -1, LockpickAnimFlags, 0, false, false, false);
        GameTimeStartedLockPickAnimation = Game.GameTime;
        Rage.Object ScrewDriverObject = null;
        uint TimeToPick = isVehicle ? RandomItems.GetRandomNumber(MinVehicleDoorPickTime, MaxVehicleDoorPickTime) : RandomItems.GetRandomNumber(MinDoorPickTime, MaxDoorPickTime);
        ScrewDriverObject = SpawnAndAttachItem(BasicUseable, true, true);
        bool isLooping = false;
        bool hasPickedLock = false;
        uint GameTimeLastCheckedAlarm = Game.GameTime;
        uint GameTimeBetweenAlarmChecks = RandomItems.GetRandomNumber(GameTimeBetweenAlarmChecksMin, GameTimeBetweenAlarmChecksMax);
        while (Player.IsAliveAndFree)
        {
            float pedAnimTime = NativeFunction.CallByName<float>("GET_ENTITY_ANIM_CURRENT_TIME", Game.LocalPlayer.Character, animDictionary, animName);
            if (pedAnimTime >= LockpickAnimStopPercentage)
            {
                isLooping = true;
                NativeFunction.Natives.SET_ANIM_RATE(Player.Character, -1.0f * LockpickAnimAnimRate, 0, false);
                NativeFunction.Natives.SET_ANIM_RATE(Player.Character, -1.0f * LockpickAnimAnimRate, 1, false);
                NativeFunction.Natives.SET_ANIM_RATE(Player.Character, -1.0f * LockpickAnimAnimRate, 2, false);
            }
            else if (isLooping && pedAnimTime <= LockpickAnimRestartPercentage)
            {
                NativeFunction.Natives.SET_ANIM_RATE(Player.Character, LockpickAnimAnimRate, 0, false);
                NativeFunction.Natives.SET_ANIM_RATE(Player.Character, LockpickAnimAnimRate, 1, false);
                NativeFunction.Natives.SET_ANIM_RATE(Player.Character, LockpickAnimAnimRate, 2, false);
            }
            Player.WeaponEquipment.SetUnarmed();

            if (Game.GameTime - GameTimeLastCheckedAlarm >= GameTimeBetweenAlarmChecks)
            {
                EntryPoint.WriteToConsole("SCREWNDRIVER, DID ALRM CHECK");
                if (interior != null && RandomItems.RandomPercent(RandomItems.GetRandomNumber(AlarmPercentageMin, AlarmPercentageMax)))
                {
                    interior.SetOffAlarm();
                    Player.HasSetOffAlarm(interior.GameLocation);
                }
                GameTimeBetweenAlarmChecks = RandomItems.GetRandomNumber(GameTimeBetweenAlarmChecksMin, GameTimeBetweenAlarmChecksMax);
                GameTimeLastCheckedAlarm = Game.GameTime;
            }


            if (Player.IsMoveControlPressed || !Player.Character.IsAlive)
            {
                break;
            }
            if (Game.GameTime - GameTimeStartedLockPickAnimation >= TimeToPick)
            {
                OnCompletedPicking();
                hasPickedLock = true;
                if (showHelpText)
                {
                    Game.DisplayHelp("Door Opened");
                }
                break;
            }

            if(isVehicle && TargetVehicle.Exists())
            {
                Player.ButtonPrompts.AttemptAddPrompt("VehicleLockpick", "Break Window", "VehicleLockpick", GameControl.Enter, 999);
                if(Player.ButtonPrompts.IsPressed("VehicleLockpick"))
                {
                    Player.SetCarBreakIn();
                    NativeFunction.Natives.TASK_ENTER_VEHICLE(Player.Character, TargetVehicle, -1, SeatTryingToEnter, 2.0f, 1, 0);
                    break;
                }
            }

            if (isVehicle && TargetVehicle.Exists() && TargetVehicle.Doors[DoorIndex].IsOpen)
            {
                EntryPoint.WriteToConsole($"LOCK PICK ENTRY BREAK 2: {Player.IsMoveControlPressed} TargetVehicle.Doors[DoorIndex].IsOpen {TargetVehicle.Doors[DoorIndex].IsOpen}");
                NativeFunction.Natives.TASK_ENTER_VEHICLE(Player.Character, TargetVehicle, -1, SeatTryingToEnter, 2.0f, 1, 0);
                //Continue = false;
                break;
            }
            GameFiber.Yield();
        }
        Player.ButtonPrompts.RemovePrompts("VehicleLockpick");
        if (hasPickedLock)
        {
            NativeFunction.Natives.SET_ENTITY_ANIM_CURRENT_TIME(Player.Character, animDictionary, animName, LockpickAnimStopPercentage);
            NativeFunction.Natives.SET_ANIM_RATE(Player.Character, 1.0f, 2, false);
        }
        Player.ActivityManager.IsPerformingActivity = false;
        if (ScrewDriverObject.Exists())
        {
            ScrewDriverObject.Delete();
        }
        if(hasPickedLock)
        {
            return true;
        }
        return false;
    }


}

