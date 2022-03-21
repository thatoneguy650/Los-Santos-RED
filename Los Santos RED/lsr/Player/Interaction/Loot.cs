using ExtensionsMethods;
using LosSantosRED.lsr.Interface;
using LosSantosRED.lsr.Player;
using Rage;
using Rage.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

public class Loot : DynamicActivity
{
    private uint GameTimeStartedConversing;
    private bool IsActivelyConversing;
    private bool IsTasked;
    private bool IsBlockedEvents;
    private PedExt Ped;
    private IInteractionable Player;
    private bool CancelledConversation;
    private ISettingsProvideable Settings;
    private ICrimes Crimes;
    private dynamic pedHeadshotHandle;
    private IModItems ModItems;
    private bool IsCancelled;

    public Loot(IInteractionable player, PedExt ped, ISettingsProvideable settings, ICrimes crimes, IModItems modItems)
    {
        Player = player;
        Ped = ped;
        Settings = settings;
        Crimes = crimes;
        ModItems = modItems;
    }
    public override string DebugString => $"TimesInsultedByPlayer {Ped.TimesInsultedByPlayer} FedUp {Ped.IsFedUpWithPlayer}";
   public override ModItem ModItem { get; set; }
    public override void Start()
    {
        if (Ped.Pedestrian.Exists())
        {
            EntryPoint.WriteToConsole($"Looting Started Money: {Ped.Money} Dead: {Ped.IsDead} Unconsc: {Ped.IsUnconscious}");

            Player.IsLootingBody = true;
           // NativeFunction.Natives.SET_GAMEPLAY_PED_HINT(Ped.Pedestrian, 0f, 0f, 0f, true, -1, 2000, 2000);
            GameFiber.StartNew(delegate
            {
                LootBody();
                Cancel();
            }, "Conversation");
        }
    }

    private void LootBody()
    {
        EntryPoint.WriteToConsole("Looting Body");
        if (MoveToBody())
        {
            bool hasCompletedTasks = DoLootAnimation();
            if(hasCompletedTasks)
            {
                FinishLoot();
                PlayAnimation("amb@medic@standing@tendtodead@exit", "exit");
            }
        }
    }

    private void FinishLoot()
    {
        bool hasAddedItem = false;
        bool hasAddedCash = false;
        string ItemsFound = "";
        int CashAdded = 0;
        if (Ped.Pedestrian.Exists())
        {
            Ped.HasBeenLooted = true;
            
            if (Ped.HasMenu)
            {
                foreach (MenuItem mi in Ped.ShopMenu.Items.Where(x => x.Purchaseable))
                {
                    ModItem localModItem = ModItems.Get(mi.ModItemName);
                    if (localModItem != null && localModItem.ModelItem?.Type == ePhysicalItemType.Prop)
                    {
                        hasAddedItem = true;
                        Player.Inventory.Add(localModItem, mi.NumberOfItemsToSellToPlayer);
                        ItemsFound += $"~n~~p~{localModItem.Name}~s~ - {mi.NumberOfItemsToSellToPlayer} {localModItem.MeasurementName}(s)";
                    }
                }
            }
            if (Ped.Money > 0 && !Ped.IsDead)//dead peds already drop it
            {
                Player.GiveMoney(Ped.Money);
                CashAdded = Ped.Money;
                Ped.Money = 0;
                Ped.Pedestrian.Money = 0;
                hasAddedCash = true;
            }
        }

        string Description = "";
        if (hasAddedCash)
        {
            Description += $"Cash Stolen: ~n~~g~${CashAdded}";
        }
        if (hasAddedItem)
        {
            if(hasAddedCash)
            {
                Description += $"~n~Items Stolen:";
                Description += ItemsFound;
            }
            else
            {
                Description += $"Items Stolen:";
                Description += ItemsFound;
            }
        }
        if(!hasAddedCash && !hasAddedItem)
        {
            Description = "Nothing Found";
        }


        if (NativeFunction.Natives.IsPedheadshotReady<bool>(pedHeadshotHandle))
        {
            string str = NativeFunction.Natives.GetPedheadshotTxdString<string>(pedHeadshotHandle);
            Game.DisplayNotification(str, str, "~r~Ped Looted", $"~y~{Ped.Name}", Description);
        }
        else
        {
            Game.DisplayNotification("CHAR_BLANK_ENTRY", "CHAR_BLANK_ENTRY", "~r~Ped Looted", $"~y~{Ped.Name}", Description);
        }



    }

    private bool MoveToBody()
    {

        pedHeadshotHandle = NativeFunction.Natives.RegisterPedheadshot<uint>(Ped.Pedestrian);

        Vector3 DesiredPosition = Ped.Pedestrian.Position;
        float DesiredHeading = Game.LocalPlayer.Character.Heading;
        //NativeFunction.Natives.TASK_GO_STRAIGHT_TO_COORD(Game.LocalPlayer.Character, DesiredPosition.X, DesiredPosition.Y, DesiredPosition.Z, 1.0f, -1, DesiredHeading, 0.2f);
        NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", Player.Character, Ped.Pedestrian, -1, 1.0f, 0.75f, 1073741824, 1); //Original and works ok
        uint GameTimeStartedMovingToBody = Game.GameTime;
        float heading = Game.LocalPlayer.Character.Heading;
        bool IsFacingDirection = true;
        bool IsCloseEnough = false;
        while (Game.GameTime - GameTimeStartedMovingToBody <= 5000 && !IsCloseEnough && !IsCancelled)
        {
            if (Player.IsMoveControlPressed)
            {
                IsCancelled = true;
            }
            if(!Ped.Pedestrian.Exists())
            {
                IsCancelled = true;
                break;
            }
            IsCloseEnough = Game.LocalPlayer.Character.DistanceTo2D(Ped.Pedestrian) < 0.75f;
            GameFiber.Yield();
        }


        NativeFunction.CallByName<bool>("TASK_TURN_PED_TO_FACE_ENTITY", Player.Character, Ped.Pedestrian, 750);

        GameFiber.Sleep(750);
        if (IsCloseEnough && IsFacingDirection && !IsCancelled)
        {
            EntryPoint.WriteToConsole($"MoveToBody IN POSITION {Game.LocalPlayer.Character.DistanceTo(DesiredPosition)} {Extensions.GetHeadingDifference(heading, DesiredHeading)} {heading} {DesiredHeading}", 5);
            return true;
        }
        else
        {
            NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
            EntryPoint.WriteToConsole($"MoveToBody NOT IN POSITION EXIT {Game.LocalPlayer.Character.DistanceTo(DesiredPosition)} {Extensions.GetHeadingDifference(heading, DesiredHeading)} {heading} {DesiredHeading}", 5);
            return false;
        }
    }
    private bool DoLootAnimation()
    {
        AnimationDictionary.RequestAnimationDictionay("amb@medic@standing@tendtodead@enter");
        AnimationDictionary.RequestAnimationDictionay("amb@medic@standing@tendtodead@base");
        AnimationDictionary.RequestAnimationDictionay("amb@medic@standing@tendtodead@exit");
        AnimationDictionary.RequestAnimationDictionay("amb@medic@standing@tendtodead@idle_a");
       // WeaponInformation previousWeapon = Player.CurrentWeapon;
        Player.SetUnarmed();

        if(PlayAnimation("amb@medic@standing@tendtodead@enter", "enter") && PlayAnimation("amb@medic@standing@tendtodead@idle_a", "idle_a"))
        {
            
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool PlayAnimation(string dictionary, string animation)
    {
        NativeFunction.Natives.TASK_PLAY_ANIM(Player.Character, dictionary, animation, 8.0f, -8.0f, -1, 2, 0, false, false, false);
        uint GameTimeStartedLootAnimation = Game.GameTime;
        float AnimationTime = 0.0f;
        while (AnimationTime < 1.0f && !IsCancelled && Game.GameTime - GameTimeStartedLootAnimation <= 10000)
        {
            AnimationTime = NativeFunction.Natives.GET_ENTITY_ANIM_CURRENT_TIME<float>(Player.Character, dictionary, animation);
            if (Player.IsMoveControlPressed)
            {
                IsCancelled = true;
            }
            if (!Ped.Pedestrian.Exists())
            {
                IsCancelled = true;
                break;
            }
            GameFiber.Yield();
        }
        if(!IsCancelled && AnimationTime >= 1.0f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public override void Continue()
    {

    }

    public override void Cancel()
    {

       // Player.SetPlayerToLastWeapon();

        NativeFunction.Natives.CLEAR_PED_TASKS(Player.Character);
        Player.IsLootingBody = false;
        //NativeFunction.Natives.STOP_GAMEPLAY_HINT(false);
    }

    public override void Pause()
    {
        Cancel();
    }


    private bool SayAvailableAmbient(Ped ToSpeak, List<string> Possibilities, bool WaitForComplete, bool isPlayer)
    {
        bool Spoke = false;
        foreach (string AmbientSpeech in Possibilities.OrderBy(x => RandomItems.MyRand.Next()))
        {
            if (isPlayer)
            {
                if (Player.CharacterModelIsFreeMode)
                {
                    ToSpeak.PlayAmbientSpeech(Player.FreeModeVoice, AmbientSpeech, 0, SpeechModifier.Force);
                }
                else
                {
                    ToSpeak.PlayAmbientSpeech(null, AmbientSpeech, 0, SpeechModifier.Force);
                }
            }
            else
            {
                if (Ped.VoiceName != "")
                {
                    ToSpeak.PlayAmbientSpeech(Ped.VoiceName, AmbientSpeech, 0, SpeechModifier.Force);
                }
                else
                {
                    ToSpeak.PlayAmbientSpeech(null, AmbientSpeech, 0, SpeechModifier.Force);
                }
            }

            GameFiber.Sleep(300);//100
            if (ToSpeak.IsAnySpeechPlaying)
            {
                Spoke = true;
            }
            EntryPoint.WriteToConsole($"SAYAMBIENTSPEECH: {ToSpeak.Handle} Attempting {AmbientSpeech}, Result: {Spoke}", 5);
            if (Spoke)
            {
                break;
            }
        }
        GameFiber.Sleep(100);
        while (ToSpeak.IsAnySpeechPlaying && WaitForComplete)
        {
            Spoke = true;
            GameFiber.Yield();
        }
        if (!Spoke)
        {
            Game.DisplayNotification($"\"{Possibilities.FirstOrDefault()}\"");
        }  
        return Spoke;
    }

    private void LootBodyOLD()
    {
        EntryPoint.WriteToConsole("Looting Body");
        if (MoveToBody())
        {

        }


        AnimationDictionary.RequestAnimationDictionay("amb@medic@standing@tendtodead@enter");
        AnimationDictionary.RequestAnimationDictionay("amb@medic@standing@tendtodead@base");
        AnimationDictionary.RequestAnimationDictionay("amb@medic@standing@tendtodead@exit");
        AnimationDictionary.RequestAnimationDictionay("amb@medic@standing@tendtodead@idle_a");

        uint GameTimeStarted = Game.GameTime;

        unsafe
        {
            int lol = 0;
            NativeFunction.CallByName<bool>("OPEN_SEQUENCE_TASK", &lol);
            NativeFunction.CallByName<bool>("TASK_GO_TO_ENTITY", 0, Ped.Pedestrian, -1, 1.0f, 0.75f, 1073741824, 1); //Original and works ok
            NativeFunction.CallByName<bool>("TASK_TURN_PED_TO_FACE_ENTITY", 0, Ped.Pedestrian, 1000);
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", 0, "amb@medic@standing@tendtodead@enter", "enter", 8.0f, -8.0f, -1, 0, 0, false, false, false);
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", 0, "amb@medic@standing@tendtodead@idle_a", "idle_a", 8.0f, -8.0f, -1, 0, 0, false, false, false);
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", 0, "amb@medic@standing@tendtodead@idle_a", "idle_b", 8.0f, -8.0f, -1, 0, 0, false, false, false);
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", 0, "amb@medic@standing@tendtodead@idle_a", "idle_c", 8.0f, -8.0f, -1, 0, 0, false, false, false);
            NativeFunction.CallByName<uint>("TASK_PLAY_ANIM", 0, "amb@medic@standing@tendtodead@exit", "exit", 8.0f, -8.0f, -1, 0, 0, false, false, false);
            NativeFunction.CallByName<bool>("SET_SEQUENCE_TO_REPEAT", lol, false);
            NativeFunction.CallByName<bool>("CLOSE_SEQUENCE_TASK", lol);
            NativeFunction.CallByName<bool>("TASK_PERFORM_SEQUENCE", Player.Character, lol);
            NativeFunction.CallByName<bool>("CLEAR_SEQUENCE_TASK", &lol);
        }

        SayAvailableAmbient(Player.Character, new List<string>() { "GENERIC_HOWS_IT_GOING", "GENERIC_HI" }, false, true);
        bool hasCompletedTasks = false;
        while (Game.GameTime - GameTimeStarted <= 15000)
        {
            if (NativeFunction.Natives.GET_SEQUENCE_PROGRESS(Player.Character) == -1)
            {
                break;
            }
            if (NativeFunction.Natives.GET_SEQUENCE_PROGRESS(Player.Character) == 6)
            {
                hasCompletedTasks = true;
            }
            GameFiber.Yield();
        }
        bool hasAddedItem = false;
        if (Ped.Pedestrian.Exists() && hasCompletedTasks)
        {
            Ped.HasBeenLooted = true;

            foreach (MenuItem mi in Ped.ShopMenu.Items.Where(x => x.Purchaseable))
            {
                ModItem modItem = ModItems.Get(mi.ModItemName);
                if (modItem != null)
                {
                    hasAddedItem = true;
                    Player.Inventory.Add(modItem, mi.NumberOfItemsToSellToPlayer);
                }

            }
        }
        if (hasAddedItem)
        {
            Game.DisplayNotification("You got some items");
        }

    }


}