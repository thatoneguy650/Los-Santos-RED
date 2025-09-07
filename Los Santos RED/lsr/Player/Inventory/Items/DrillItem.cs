using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player;
using Rage;
using Rage.Native;
using System;
using System.Xml.Serialization;

[Serializable()]
public class DrillItem : ModItem
{
    private Rage.Object CreatedDrillProp;
    private string mainDict;

    public uint MinSafeDrillTime { get; set; } = 9000;
    public uint MaxSafeDrillTime { get; set; } = 18000;



    public uint MinDoorDrillTime { get; set; } = 5000;
    public uint MaxDoorDrillTime { get; set; } = 9000;
    public DrillItem()
    {

    }
    public DrillItem(string name, string description) : base(name, description, ItemType.Equipment)
    {

    }
    public DrillItem(string name) : base(name, ItemType.Equipment)
    {

    }
    public override bool UseItem(IActionable actionable, ISettingsProvideable settings, IEntityProvideable world, ICameraControllable cameraControllable, IIntoxicants intoxicants, ITimeControllable time)
    {
        DrillActivity activity = new DrillActivity(actionable, settings, this);
        if (activity.CanPerform(actionable))
        {
            actionable.ActivityManager.StartUpperBodyActivity(activity);
            return true;
        }
        return false;
    }
    public override void AddToList(PossibleItems possibleItems)
    {
        possibleItems?.DrillItems.RemoveAll(x => x.Name == Name);
        possibleItems?.DrillItems.Add(this);
    }

    public void PerformDrillingAnimation(IInteractionable Player, Action OnCompletedDrilling, bool isSafe)
    {
        SetupDrillingAnimation();
        CreateAndAttachItem(Player);
        PerformAnimation(Player, OnCompletedDrilling, isSafe);
    }
    private void SetupDrillingAnimation()
    {
        mainDict = "anim@heists@fleeca_bank@drilling";
        if (!string.IsNullOrEmpty(mainDict))
        {
            AnimationDictionary.RequestAnimationDictionay(mainDict);
        }
        uint GameTimeStartedRequesting = Game.GameTime;
        bool loaded = false;
        NativeFunction.Natives.REQUEST_AMBIENT_AUDIO_BANK<bool>("DLC_HEIST_FLEECA_SOUNDSET", false, -1);
        NativeFunction.Natives.REQUEST_MISSION_AUDIO_BANK<bool>("DLC_HEIST_FLEECA_SOUNDSET", false, -1);
        NativeFunction.Natives.REQUEST_AMBIENT_AUDIO_BANK<bool>("HEIST_FLEECA_DRILL", false, -1);
        NativeFunction.Natives.REQUEST_AMBIENT_AUDIO_BANK<bool>("HEIST_FLEECA_DRILL_2", false, -1);
        NativeFunction.Natives.REQUEST_AMBIENT_AUDIO_BANK<bool>("DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
        NativeFunction.Natives.REQUEST_AMBIENT_AUDIO_BANK<bool>("DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);
        while (Game.GameTime - GameTimeStartedRequesting <= 100)
        {
            loaded = NativeFunction.Natives.REQUEST_SCRIPT_AUDIO_BANK<bool>("DLC_HEIST_FLEECA_SOUNDSET", false, -1);
            if (loaded)
            {
                break;
            }
            GameFiber.Yield();
        }
        EntryPoint.WriteToConsole($"AUDIO loaded:{loaded}");
    }
    public void Dispose()
    {
        if (CreatedDrillProp.Exists())
        {
            CreatedDrillProp.Delete();
        }
    }
    private void CreateAndAttachItem(IInteractionable Player)
    {
        if (CreatedDrillProp.Exists())
        {
            CreatedDrillProp.Delete();
        }

        EntryPoint.WriteToConsole("SPAWNING DRILL ITEM");
        CreatedDrillProp = SpawnAndAttachItem(Player, true, true);
        
    }
    private void PerformAnimation(IInteractionable Player, Action OnCompletedDrilling, bool isSafe)
    {
        Player.ActivityManager.StopDynamicActivity();
        Player.ActivityManager.IsPerformingActivity = true;
        uint GameTimeStarted = Game.GameTime;
        uint DrillingTime = RandomItems.GetRandomNumber(MinSafeDrillTime, MaxSafeDrillTime);
        if(!isSafe)
        {
            DrillingTime = RandomItems.GetRandomNumber(MinDoorDrillTime, MaxDoorDrillTime);
        }


        //DO AUDIO
        bool hasStartedSound = false;
        int soundID = NativeFunction.Natives.GET_SOUND_ID<int>();
        unsafe
        {
            int lol = 0;
            NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
            NativeFunction.CallByName<bool>("TASK_PLAY_ANIM", 0, mainDict, "drill_straight_start", 2.0f, -2.0f, -1, 1, 0, false, false, false);
            NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
            NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
            NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Player.Character, lol);
            NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
        }
        while (Player.ActivityManager.CanPerformActivitiesExtended)
        {
            Player.WeaponEquipment.SetUnarmed();
            if (Player.IsMoveControlPressed || !Player.Character.IsAlive)
            {
                break;
            }
            if (!hasStartedSound && Game.GameTime - GameTimeStarted >= 500 && CreatedDrillProp.Exists())
            {
                EntryPoint.WriteToConsole("START PLAYING SOUND");
                NativeFunction.Natives.PLAY_SOUND_FROM_ENTITY(soundID, "Drill", CreatedDrillProp, "DLC_HEIST_FLEECA_SOUNDSET", false, 0);
                hasStartedSound = true;
            }

            if (Game.GameTime - GameTimeStarted >= DrillingTime)
            {
                //Game.DisplayHelp("Unlocked");
                //TheftInteract.SetUnlocked();

                OnCompletedDrilling();
                Game.DisplayHelp("Door Opened");


                NativeFunction.Natives.STOP_SOUND(soundID);
                NativeFunction.Natives.RELEASE_SOUND_ID(soundID);
                Dispose();

                NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
                GameFiber.Sleep(500);
                break;
            }
            GameFiber.Yield();
        }
        NativeFunction.Natives.STOP_SOUND(soundID);
        NativeFunction.Natives.RELEASE_SOUND_ID(soundID);
        Player.ActivityManager.IsPerformingActivity = false;
        Dispose();
    }


}

