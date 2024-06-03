using LosSantosRED.lsr.Interface;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DrillUsePreInteract : ItemUsePreInteract
{
    private DrillItem DrillItem;
    private Rage.Object CreatedDrillProp;
    private TheftInteract TheftInteract;
    private string mainDict;
    public uint DrilTimeMin { get; set; } = 4000;
    public uint DrilTimeMax { get; set; } = 10000;
    public DrillUsePreInteract()
    {

    }
    public override void Start(IInteractionable player, ILocationInteractable locationInteractable, ISettingsProvideable settings, IModItems modItems, TheftInteract theftInteract)
    {
        Player = player;
        LocationInteractable = locationInteractable;
        Settings = settings;
        ModItems = modItems;
        TheftInteract = theftInteract;

        if (Player.ActivityManager.HasDrillInHand && Player.ActivityManager.CurrentDrill != null)
        {
            DrillItem = Player.ActivityManager.CurrentDrill;
        }
        else
        {
            InventoryItem drillInventory = Player.Inventory.ItemsList.Where(x => x.ModItem != null && x.ModItem.ItemType == ItemType.Equipment && x.ModItem.ItemSubType == ItemSubType.Tool && x.ModItem.GetType() == typeof(DrillItem)).FirstOrDefault();
            if (drillInventory == null)
            {
                Game.DisplayHelp("Need a Drill to interact");
                return;
            }
            DrillItem = (DrillItem)drillInventory.ModItem;
            if (DrillItem == null)
            {
                Game.DisplayHelp("Need a Drill to interact");
                return;
            }
        }
        Setup();
        CreateAndAttachItem();
        PerformAnimation();
    }
    private void Setup()
    {
        //anim@heists@fleeca_bank@drilling
        mainDict = "anim@heists@fleeca_bank@drilling";
        if (!string.IsNullOrEmpty(mainDict))
        {
            AnimationDictionary.RequestAnimationDictionay(mainDict);
        }
        uint GameTimeStartedRequesting = Game.GameTime;
        bool loaded = false;


        //NativeFunction.Natives.REQUEST_SCRIPT_AUDIO_BANK<bool>("DLC_MPHEIST/HEIST_FLEECA_DRILL", false, -1);
        //NativeFunction.Natives.REQUEST_SCRIPT_AUDIO_BANK<bool>("DLC_MPHEIST/HEIST_FLEECA_DRILL_2", false, -1);


        NativeFunction.Natives.REQUEST_AMBIENT_AUDIO_BANK<bool>("DLC_HEIST_FLEECA_SOUNDSET", false, -1);
        NativeFunction.Natives.REQUEST_MISSION_AUDIO_BANK<bool>("DLC_HEIST_FLEECA_SOUNDSET", false, -1);



        NativeFunction.Natives.REQUEST_AMBIENT_AUDIO_BANK<bool>("HEIST_FLEECA_DRILL", false, -1);
        NativeFunction.Natives.REQUEST_AMBIENT_AUDIO_BANK<bool>("HEIST_FLEECA_DRILL_2", false, -1);
        NativeFunction.Natives.REQUEST_AMBIENT_AUDIO_BANK<bool>("DLC_MPHEIST\\HEIST_FLEECA_DRILL", false, -1);
        NativeFunction.Natives.REQUEST_AMBIENT_AUDIO_BANK<bool>("DLC_MPHEIST\\HEIST_FLEECA_DRILL_2", false, -1);

        while ( Game.GameTime - GameTimeStartedRequesting <= 100)
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
    public void Dipose()
    {
        if (CreatedDrillProp.Exists())
        {
            CreatedDrillProp.Delete();
        }
    }
    private void CreateAndAttachItem()
    {
        if (CreatedDrillProp.Exists())
        {
            CreatedDrillProp.Delete();
        }
        if (DrillItem != null)
        {
            EntryPoint.WriteToConsole("SPAWNING DRILL ITEM");
            CreatedDrillProp = DrillItem.SpawnAndAttachItem(Player, true, true);
        }
    }
    private void PerformAnimation()
    {
        Player.ActivityManager.StopDynamicActivity();
        uint GameTimeStarted = Game.GameTime;
        uint DrillingTime = RandomItems.GetRandomNumber(DrillItem.MinSafeDrillTime, DrillItem.MaxSafeDrillTime);



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
            if(!hasStartedSound && Game.GameTime - GameTimeStarted >= 500 && CreatedDrillProp.Exists())
            {
                EntryPoint.WriteToConsole("START PLAYING SOUND");
                NativeFunction.Natives.PLAY_SOUND_FROM_ENTITY(soundID,"Drill",CreatedDrillProp, "DLC_HEIST_FLEECA_SOUNDSET",false,0);
                hasStartedSound = true;
            }

            if(Game.GameTime - GameTimeStarted >= DrillingTime)
            {
                //Game.DisplayHelp("Unlocked");
                TheftInteract.SetUnlocked();
                NativeFunction.Natives.STOP_SOUND(soundID);
                NativeFunction.Natives.RELEASE_SOUND_ID(soundID);
                Dipose();

                NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
                GameFiber.Sleep(500);
                break;
            }
            GameFiber.Yield();
        }
        NativeFunction.Natives.STOP_SOUND(soundID);
        NativeFunction.Natives.RELEASE_SOUND_ID(soundID);
        Dipose();
    }
}
